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
            SqlConnection connection1;
            SqlConnection connection2;

            using (var command = new StoredProcedure(TestStoredProcedureName))
            {
                connection1 = command.Connection;

                command.Parameters["Parameter1"] = "test";
                command.Parameters["Parameter2"] = 11;

                command.ExecuteNonQuery();
            }

            using (var command = new StoredProcedure(TestStoredProcedureName))
            {
                connection2 = command.Connection;

                command.Parameters["Parameter1"] = "test";
                command.Parameters["Parameter2"] = 11;

                command.ExecuteNonQuery();
            }

            Assert.NotSame(connection1, connection2);
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

        [Fact]
        public void WhenCreatedWithConnectionThenDoesNotDisposeConnection()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var wasDisposed = false;

                using (var command = new StoredProcedure(conn, TestStoredProcedureName))
                {
                    conn.Disposed += (sender, args) => { wasDisposed = true; };

                    command.Parameters["Parameter1"] = "test";
                    command.Parameters["Parameter2"] = 11;

                    command.ExecuteNonQuery();
                }

                Assert.False(wasDisposed);
            }
        }

        [Fact]
        public void WhenCreatedWithoutConnectionThenDisposesConnection()
        {
            var wasDisposed = false;

            using (var command = new StoredProcedure(TestStoredProcedureName))
            {
                command.Connection.Disposed += (sender, args) => { wasDisposed = true; };

                command.Parameters["Parameter1"] = "test";
                command.Parameters["Parameter2"] = 11;

                command.ExecuteNonQuery();
            }

            Assert.True(wasDisposed);
        }
    }
}
