using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattousaiSqlDataAccess
{
    public class SqlParameter
    {
        public string Name { get; private set; }
        public object Value { get; set; }

        public SqlParameter(string name, object value = null)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Invalid name for SqlParameter.", nameof(name));

            this.Name = name;
            this.Value = value;
        }

        public string NormalizedName
        {
            get { return NormalizeParameterName(Name); }
        }

        public static string NormalizeParameterName(string parameterName)
        {
            return (parameterName.StartsWith("@") ? parameterName : ("@" + parameterName));
        }

        public static bool AreParameterNamesIsomorphic(string name1, string name2)
        {
            var normalizedName1 = NormalizeParameterName(name1);
            var normalizedName2 = NormalizeParameterName(name2);

            return StringComparer.CurrentCultureIgnoreCase.Equals(normalizedName1, normalizedName2);
        }

        public static bool ContainsParameter(System.Data.SqlClient.SqlParameterCollection collection, string parameterName)
        {
            return collection.Cast<System.Data.SqlClient.SqlParameter>().Any(x => AreParameterNamesIsomorphic(x.ParameterName, parameterName));
        }
    }
}
