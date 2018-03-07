using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattousaiSqlDataAccess
{
    public class SqlParameterCollection : ISqlParameterCollectionAccessor
    {
        private readonly Lazy<SqlParameterCollectionAccessor> _accessor;
        private SqlParameterCollectionAccessor accessor { get { return _accessor.Value; } }

        public SqlParameterCollection()
        {
            _accessor = new Lazy<SqlParameterCollectionAccessor>(() => new SqlParameterCollectionAccessor(this));
        }

        public object this[string parameterName]
        {
            get
            {
                return null;
            }

            set
            {

            }
        }

        public SqlParameterCollectionAccessor Add(string parameterName, object value)
        {
            return accessor;
        }
    }
}
