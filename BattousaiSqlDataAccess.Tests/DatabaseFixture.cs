using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BattousaiSqlDataAccess.Tests
{
    public class DatabaseFixture : IDisposable
    {
        private readonly TestDatabase database;

        public string ConnectionString { get; private set; }

        public DatabaseFixture()
        {
            database = new TestDatabase(new TestDatabaseOptions("testing"));
            database.CreateDatabase();

            ConnectionString = database.GetConnectionString();

            SqlContext.Current.ConnectionString = ConnectionString;

            InitializeDatabaseData();
        }

        private void InitializeDatabaseData()
        {
            RunCommand(DefineTestStoredProcedure);
        }

        private void RunCommand(string sqlText)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sqlText, conn))
            {
                conn.Open();

                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            database.DropDatabase();
        }

        private readonly string DefineTestStoredProcedure = "CREATE PROCEDURE dbo.TestStoredProcedure @Parameter1 VARCHAR(50), @Parameter2 INT AS BEGIN SELECT 0 END";
    }

    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}
