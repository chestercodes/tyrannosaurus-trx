using System.Collections.Generic;
using System.Linq;
using TRX_Merger.TrxModel;

namespace TRX_Merger.ReportModel
{
    public class TestName
    {
        public TestName(string testClass, string testMethod)
        {
            TestClass = testClass;
            TestMethod = testMethod;
        }
        public string TestClass { get; }
        public string TestMethod { get; }
    }

    public class TestRunReport
    {
        public TestRunReport(TestRun run)
        {
            Run = run;
            TestClasses = Run.TestDefinitions.Select(td => td.TestMethod.ClassName).Distinct().ToList<string>();

            AllFailedTests = new List<UnitTestResultReport>();
            TestClassReports = new Dictionary<string, TestClassReport>();
            foreach (var testClass in TestClasses)
            {
                var t = GetTestClassReport(testClass);
                TestClassReports.Add(testClass, t);
                AllFailedTests.AddRange(t.Tests.Where(r =>
                {
                    var isNotPassing = r.Result.Outcome != "Passed";
                    return isNotPassing;
                }).ToList());
            }
        }

        public TestRun Run { get; private set; }

        public List<string> TestClasses { get; private set; }
        
        public List<UnitTestResultReport> AllFailedTests { get; private set; }

        public Dictionary<string, TestClassReport> TestClassReports { get; private set; }

        public string TestClassReportsJson()
        {
            var test =  System.Text.Json.JsonSerializer.Serialize(TestClassReports.Select(s => s.Value).Select(
                c => 
                    new 
                    { 
                        ClassName = c.TestClassId,
                        Passed = c.Passed,
                        Failed = c.Failed,
                        Timeout = c.Timeout,
                        Aborted = c.Aborted,
                    }).ToList());
            return test;
        }
 

        public TestClassReport GetTestClassReport(string className)
        {
            var testIds = Run.TestDefinitions
                    .Where(td => td.TestMethod.ClassName.EndsWith(className))
                    .Select(ttdd => ttdd.Id).ToList();

            var results = Run.Results.Where(r => testIds.Contains(r.TestId)).ToList();
           
            List<UnitTestResultReport> resultReports = new List<UnitTestResultReport>();
            foreach (var r in results)
            {
                resultReports.Add(
                    new UnitTestResultReport(r)
                    { 
                        ClassName = className,
                        Dll = Run.TestDefinitions.Where(d => d.Id == r.TestId).FirstOrDefault().TestMethod.CodeBase
                    });
            }

            return new TestClassReport(className, resultReports);
        }
    }
}
