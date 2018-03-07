using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattousaiSqlDataAccess
{
    public interface ISqlDataCommand
    {
        int ExecuteNonQuery();
        Task<int> ExecuteNonQueryAsync();
    }
}
