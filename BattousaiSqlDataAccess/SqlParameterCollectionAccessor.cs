using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattousaiSqlDataAccess
{
    public class SqlParameterCollectionAccessor : ISqlParameterCollectionAccessor
    {
        private readonly SqlParameterCollection parameters;

        internal SqlParameterCollectionAccessor(SqlParameterCollection parameters)
        {
            this.parameters = parameters;
        }

        public SqlParameterCollectionAccessor Add(string parameterName, object value)
        {
            return parameters.Add(parameterName, value);
        }
    }
}
