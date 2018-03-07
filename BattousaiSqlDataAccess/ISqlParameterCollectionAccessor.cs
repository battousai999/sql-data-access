using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattousaiSqlDataAccess
{
    public interface ISqlParameterCollectionAccessor
    {
        SqlParameterCollectionAccessor Add(string parameterName, object value);
    }
}
