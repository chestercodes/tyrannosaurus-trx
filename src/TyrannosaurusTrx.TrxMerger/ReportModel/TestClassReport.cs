using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRX_Merger.ReportModel
{
    public class TestClassReport
    {
        public TestClassReport(string testClass, List<UnitTestResultReport> tests)
        {
            TestClassName = testClass;

            Tests = tests;

            TotalTests = tests.Count;

            Passed = tests.Count(t => t.Result.Outcome == "Passed");

            Failed = tests.Count(t => t.Result.Outcome == "Failed");

            Aborted = tests.Count(t => t.Result.Outcome == "Aborted");

            Timeout = tests.Count(t => t.Result.Outcome == "Timeout");

            var durations = tests.Select(t => TimeSpan.Parse(t.Result.Duration)).ToList<TimeSpan>();
            Duration = new TimeSpan();
            durations.ForEach(d => Duration += d);

            Dll = tests[0].Dll;
        }

        public string TestClassName { get; private set; }
        public string FriendlyTestClassName
        {
            get
            {
                return TestClassName.Split(new char[] { '.' }).Last();
            }
        }

        public string TestClassId
        {
            get
            {
                return TestClassName.Replace(".", "_");
            }
        }

        public List<UnitTestResultReport> Tests { get; private set; }

        public string Dll { get; private set; }

        public int TotalTests { get; private set; }

        public int Passed { get; private set; }

        public int Failed { get; private set; }

        public int Timeout { get; private set; }

        public int Aborted { get; private set; }

        public TimeSpan Duration { get; private set; }
    }
}