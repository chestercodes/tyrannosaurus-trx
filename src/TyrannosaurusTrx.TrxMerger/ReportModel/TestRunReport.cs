using System.Collections.Generic;
using System.Linq;
using TRX_Merger.TrxModel;

namespace TRX_Merger.ReportModel
{
    public class TestRunReport
    {
        public TestRunReport(TestRun run)
        {
            Run = run;
            TestClasses = Run.TestDefinitions.Select(td => td.TestMethod.ClassName).Distinct().ToList<string>();

            AllFailedTests = new List<UnitTestResultReport>();
            TestClassReports = new List<TestClassReport>();
            foreach (var testClass in TestClasses)
            {
                var t = GetTestClassReport(testClass);
                TestClassReports.Add(t);
                AllFailedTests.AddRange(t.Tests.Where(r =>
                {
                    var result = r.Result.Outcome.ToLower();
                    var isNotPassing = result != "passed" && result != "notexecuted";
                    return isNotPassing;
                }).ToList());
            }

            TestClassReports = TestClassReports.OrderByDescending(x => x.Failed).ToList();
        }

        public TestRun Run { get; private set; }

        public List<string> TestClasses { get; private set; }

        public List<UnitTestResultReport> AllFailedTests { get; private set; }

        public List<TestClassReport> TestClassReports { get; private set; }

        public string TestClassReportsJson()
        {
            var test = System.Text.Json.JsonSerializer.Serialize(TestClassReports.Select(
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
                var dll = Run.TestDefinitions.Where(d => d.Id == r.TestId).FirstOrDefault().TestMethod.CodeBase;
                resultReports.Add(new UnitTestResultReport(r, className, dll));
            }

            return new TestClassReport(className, resultReports);
        }
    }
}