using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattousaiSqlDataAccess
{
    internal class SqlParameterInfo
    {
        public string Name { get; private set; }
        public SqlDbType DbType { get; private set; }
    }
}
