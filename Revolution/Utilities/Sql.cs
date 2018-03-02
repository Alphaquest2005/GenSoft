using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Sql
    {
        public static string CleanSql(string sql)
        {
            return sql.Replace("= ,", "= NULL,")
                    .Replace("= ''", "= NULL")
                    .Replace(",,", ",NULL,")
                    .Replace(",False,", ",0,")
                    .Replace(",True,", ",1,")
                    .Replace(",''", ",NULL")
                    .Replace("(''", "('")
                ;
        }
    }
}
