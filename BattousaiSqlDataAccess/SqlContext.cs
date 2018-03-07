using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattousaiSqlDataAccess
{
    public class SqlContext
    {
        public static SqlContext Current = new SqlContext();

        private SqlContext()
        {
        }

        public string ConnectionString { get; set; }
    }
}
