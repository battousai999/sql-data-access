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
            get { return (Name.StartsWith("@") ? Name : ("@" + Name)); }
        }
    }
}
