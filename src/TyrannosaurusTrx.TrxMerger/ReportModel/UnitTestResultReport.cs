using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TRX_Merger.TrxModel;

namespace TRX_Merger.ReportModel
{
    public class UnitTestResultReport
    {
        public UnitTestResultReport(UnitTestResult result, string className, string dll)
        {
            ClassName = className;
            Dll = dll;
            Result = result;

            if (Result.TestName.Contains('.'))
            {
                // Test.Project.TestClass.MethodName
                TestFullName = Result.TestName;
            }
            else
            {
                // MethodName
                TestFullName = $"{ClassName}.{Result.TestName}";
            }

            ErrorMessage = Result.Output?.ErrorInfo?.Message ?? "";

            if (!string.IsNullOrEmpty(Result.Output.StdOut))
            {
                if (Result.Output.StdOut.Contains("-> done")
                    || Result.Output.StdOut.Contains("-> error")
                    || Result.Output.StdOut.Contains("-> skipped"))
                {
                    //set cucumber output
                    cucumberStdOut = new List<KeyValuePair<string, string>>();
                    var rows = Result.Output.StdOut.Split(new char[] { '\n' });
                    for (int i = 1; i < rows.Length; i++)
                    {
                        if (rows[i].StartsWith("-> done"))
                        {
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i - 1], "success"));
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i], "success"));
                        }


                        else if (rows[i].StartsWith("-> error"))
                        {
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i - 1], "danger"));
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i], "danger"));
                        }
                        else if (rows[i].StartsWith("-> skipped"))
                        {
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i - 1], "warning"));
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i], "warning"));
                        }
                    }
                }
                else
                {
                    //set standard output
                    StdOutRows = Result.Output.StdOut.Split(new char[] { '\n' }).ToList();
                }
            }

            if (!string.IsNullOrEmpty(Result.Output.StdErr))
            {
                StdErrRows = Result.Output.StdErr.Split(new char[] { '\n' }).ToList();
            }

            if (result.Output.ErrorInfo != null)
            {
                if (!string.IsNullOrEmpty(Result.Output.ErrorInfo.Message))
                {
                    //set MessageRows
                    ErrorMessageRows = Result.Output.ErrorInfo.Message.Split(new char[] { '\n' }).ToList();
                }

                if (!string.IsNullOrEmpty(Result.Output.ErrorInfo.StackTrace))
                {
                    //set StackTraceRows
                    ErrorStackTraceRows = Result.Output.ErrorInfo.StackTrace.Split(new char[] { '\n' }).ToList();
                }
            }

            ErrorImage = null;
        }


        public string TestId
        {
            get
            {
                // examples of TestName can be
                // TrxTests.XUnit.AllPass.Tests.UnitTest1.Test_Passes_Easily
                //  TrxTests.XUnit.AllPass.Tests.ParamTests.Test_Passes_Easily(b: False, i: 2)
                var str =  $"{ClassName}_{Result.TestName}".Replace("-", "negative");
                var sanitised = Regex.Replace(str, "[^a-zA-Z0-9-_]", "_");
                return sanitised;
            }
        }

        private List<KeyValuePair<string, string>> cucumberStdOut;
        public List<KeyValuePair<string, string>> CucumberStdOut
        {
            get
            {
                return cucumberStdOut;
            }
        }

        public List<string> StdOutRows { get; set; }

        public List<string> StdErrRows { get; set; }

        public List<string> ErrorMessageRows { get; set; }
        public List<string> ErrorStackTraceRows { get; set; }

        public string AsJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public string FormattedStartTime
        {
            get
            {
                return DateTime.Parse(Result.StartTime).ToString("MM.dd.yyyy hh\\:mm\\:ss");
            }
        }

        public string FormattedEndTime
        {
            get
            {
                return DateTime.Parse(Result.EndTime).ToString("MM.dd.yyyy hh\\:mm\\:ss");
            }
        }

        public string FormattedDuration
        {
            get
            {
                return TimeSpan.Parse(Result.Duration).TotalSeconds.ToString("n2") + " sec.";
            }
        }

        public UnitTestResult Result { get; private set; }
        public string Dll { get; private set; }
        public string ClassName { get; private set; }
        public string ErrorImage { get; private set; }
        public string TestFullName { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}