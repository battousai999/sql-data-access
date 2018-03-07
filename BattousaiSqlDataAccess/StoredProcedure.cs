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
        private readonly bool isManagingConnection;

        public SqlParameterCollection Parameters { get; private set; }
        public SqlConnection Connection { get; private set; }

        public StoredProcedure(string storedProcedureName)
            : this(new SqlConnection(GetConnectionStringFromCurrentContext()), storedProcedureName, true)
        {
        }

        public StoredProcedure(SqlConnection connection, string storedProcedureName)
            : this(connection, storedProcedureName, false)
        {
        }

        private StoredProcedure(SqlConnection connection, string storedProcedureName, bool isManagingConnection)
        {
            Parameters = new SqlParameterCollection();
            Connection = connection;
            this.isManagingConnection = isManagingConnection;
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
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (isManagingConnection)
                        Connection.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
