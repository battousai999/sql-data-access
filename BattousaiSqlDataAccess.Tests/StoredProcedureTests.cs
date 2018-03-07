using System;
using System.Data.SqlClient;
using System.Linq;
using Xunit;

namespace BattousaiSqlDataAccess.Tests
{
    [Collection("DatabaseCollection")]
    public class StoredProcedureTests
    {
        private const string TestStoredProcedureName = "dbo.TestStoredProcedure";
        private string connectionString;

        public StoredProcedureTests(DatabaseFixture fixture)
        {
            connectionString = fixture.ConnectionString;
        }

        [Fact]
        public void WhenCreatedWithoutConnectionThenCreatesNewConnection()
        {
            using (var command = new StoredProcedure(TestStoredProcedureName))
            {
                command.ExecuteNonQuery();
            }
        }

        [Fact]
        public void WhenCreatedWithoutConnectionAndDefaultAccessorParametersThenExecutes()
        {
            using (var command = new StoredProcedure(TestStoredProcedureName))
            {
                command.Parameters["Parameter1"] = "test";
                command.Parameters["Parameter2"] = 11;

                int result = command.ExecuteNonQuery();
            }
        }

        [Fact]
        public void WhenCreatedWithoutConnectionAndAddParametersThenExecutes()
        {
            using (var command = new StoredProcedure(TestStoredProcedureName))
            {
                command.Parameters
                    .Add("Parameter1", "test")
                    .Add("Parameter2", 11);

                int result = command.ExecuteNonQuery();
            }
        }
        
        [Fact]
        public void WhenUsingExecuteNonQueryThenExecutes()
        {
            using (var command = new StoredProcedure(TestStoredProcedureName))
            {
                command.Parameters["Parameter1"] = "test";
                command.Parameters["Parameter2"] = 11;

                int result = command.ExecuteNonQuery();
            }
        }

        [Fact]
        public async void WhenUsingExecuteNonQueryAsyncThenExecutes()
        {
            using (var command = new StoredProcedure(TestStoredProcedureName))
            {
                command.Parameters
                    .Add("Parameter1", "test")
                    .Add("Parameter2", 11);

                int result = await command.ExecuteNonQueryAsync();
            }
        }
    }
}
