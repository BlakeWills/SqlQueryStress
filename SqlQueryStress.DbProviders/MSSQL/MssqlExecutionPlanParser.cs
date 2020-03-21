using System.Linq;
using System.Xml.Linq;

namespace SqlQueryStress.DbProviders.MSSQL
{
    internal class MssqlExecutionPlanParser
    {
        public MssqlExecutionPlanStats Parse(string executionPlan)
        {
            var xDoc = XDocument.Parse(executionPlan);
            var xmlNamespace = xDoc.Root.Name.Namespace;

            var queryTimeStatsNode = xDoc.Descendants($"{{{xmlNamespace}}}QueryTimeStats").First();
            var cpuTime = double.Parse(queryTimeStatsNode.Attribute("CpuTime").Value);
            var elapsedTime = double.Parse(queryTimeStatsNode.Attribute("ElapsedTime").Value);

            var logicalReads = xDoc.Descendants().Attributes("ActualLogicalReads").Sum(x => int.Parse(x.Value));

            return new MssqlExecutionPlanStats()
            {
                CpuTime = cpuTime,
                ElapsedTime = elapsedTime,
                LogicalReads = logicalReads
            };
        }
    }
}
