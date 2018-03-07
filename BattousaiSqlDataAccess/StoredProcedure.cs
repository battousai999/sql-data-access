using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattousaiSqlDataAccess
{
    public class StoredProcedure : ISqlDataCommand, IDisposable
    {
        public SqlParameterCollection Parameters { get; private set; }
        public SqlConnection Connection { get; set; }

        public StoredProcedure(string storedProcedureName)
            : this(new SqlConnection(GetConnectionStringFromCurrentContext()), storedProcedureName)
        {
        }

        public StoredProcedure(SqlConnection connection, string storedProcedureName)
        {
            Parameters = new SqlParameterCollection();
            Connection = connection;
        }

        private static string GetConnectionStringFromCurrentContext()
        {
            var connectionString = SqlContext.Current.ConnectionString;

            if (String.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("No connection string in the current SqlContext.");

            return connectionString;
        }

        public int ExecuteNonQuery()
        {
            CheckConnection();

            return -1;
        }

        public async Task<int> ExecuteNonQueryAsync()
        {
            await CheckConnectionAsync();

            return -1;
        }

        private void CheckConnection()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
        }

        private async Task CheckConnectionAsync()
        {
            if (Connection.State == ConnectionState.Closed)
                await Connection.OpenAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
