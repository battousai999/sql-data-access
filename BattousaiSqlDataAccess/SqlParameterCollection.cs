using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattousaiSqlDataAccess
{
    public class SqlParameterCollection : ISqlParameterCollectionAccessor, IEnumerable<SqlParameter>
    {
        private readonly Lazy<SqlParameterCollectionAccessor> _accessor;
        private SqlParameterCollectionAccessor accessor { get { return _accessor.Value; } }
        private List<SqlParameter> parameters = new List<SqlParameter>();

        public SqlParameterCollection()
        {
            _accessor = new Lazy<SqlParameterCollectionAccessor>(() => new SqlParameterCollectionAccessor(this));
        }

        public object this[string parameterName]
        {
            get
            {
                var parameter = parameters.FirstOrDefault(x => AreParameterNamesIsomorphic(parameterName, x.Name));

                return (parameter == null ? null : parameter.Value);
            }

            set
            {
                var parameter = parameters.FirstOrDefault(x => AreParameterNamesIsomorphic(parameterName, x.Name));

                if (parameter == null)
                    parameters.Add(new SqlParameter(parameterName, value));
                else
                    parameter.Value = value;
            }
        }

        private bool AreParameterNamesIsomorphic(string name1, string name2)
        {
            if (name1 != null && name1.StartsWith("@"))
                name1 = name1.Substring(1);

            if (name2 != null && name2.StartsWith("@"))
                name2 = name2.Substring(1);

            return StringComparer.CurrentCultureIgnoreCase.Equals(name1, name2);
        }

        public SqlParameterCollectionAccessor Add(string parameterName, object value)
        {
            this[parameterName] = value;

            return accessor;
        }

        public IEnumerator<SqlParameter> GetEnumerator()
        {
            return parameters.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)parameters.ToList()).GetEnumerator();
        }
    }
}
