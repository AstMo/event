using System;
using System.Text;

namespace PartyMaker.Common.Impl
{
    public class PostgreSqlDialect : DapperExtensions.Mapper.Sql.PostgreSqlDialect
    {
        public override string GetColumnName(string prefix, string columnName, string alias)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(nameof(columnName), "columnName cannot be null or empty.");
            }

            StringBuilder result = new StringBuilder();

            result.AppendFormat(QuoteString(columnName));

            if (!string.IsNullOrWhiteSpace(alias))
            {
                result.AppendFormat(" AS {0}", QuoteString(alias));
            }

            return result.ToString();
        }
    }
}
