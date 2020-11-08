using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRX_Merger.ReportModel;
using TRX_Merger.TrxModel;
using TRX_Merger.Utilities;

namespace TRX_Merger.ReportGenerator
{
    public static class TrxReportGenerator
    {
        public static string GenerateReport(TestRun run, string reportTitle = "Test results")
        {
            if (!string.IsNullOrEmpty(reportTitle))
                run.Name = reportTitle;

            string template = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReportGenerator/trx_report_template.cshtml"));

            var testRunReport = new TestRunReport(run);

            string result = Engine.Razor.RunCompile(
                template,
                "rawTemplate",
                null,
                testRunReport);
            
            return result;
        }
    }
}
