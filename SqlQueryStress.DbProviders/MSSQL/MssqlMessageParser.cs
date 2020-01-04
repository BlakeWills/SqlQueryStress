using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlQueryStress.DbProviders.MSSQL
{
    internal sealed class MssqlMessageParser
    {
        private static readonly Regex _readsRegex = new Regex(@"(?:Table (\'\w{1,}\'|'#\w{1,}\'|'##\w{1,}\'). Scan count \d{1,}, logical reads )(\d{1,})", RegexOptions.Compiled);

        private static readonly Regex _timesRegex = new Regex(
                @"(?:SQL Server Execution Times:|SQL Server parse and compile time:)(?:\s{1,}CPU time = )(\d{1,})(?: ms,\s{1,}elapsed time = )(\d{1,})",
                RegexOptions.Compiled);

        public MssqlMessageParseResult ParseSqlInfoMessage(SqlInfoMessageEventArgs message)
        {
            var result = new MssqlMessageParseResult();

            foreach (SqlError error in message.Errors)
            {
                var reads = _readsRegex.Split(error.Message);
                if (reads.Count() > 1)
                {
                    result.LogicalReads += int.Parse(reads[2]);
                    continue;
                }

                var times = _timesRegex.Split(error.Message);
                if (times.Count() > 1)
                {
                    result.CpuTime += int.Parse(times[1]);
                    result.ElapsedTime += int.Parse(times[2]);
                }
            }

            return result;
        }
    }

    internal sealed class MssqlMessageParseResult
    {
        public double CpuTime { get; set; }

        public double ElapsedTime { get; set; }

        public int LogicalReads { get; set; }
    }
}
