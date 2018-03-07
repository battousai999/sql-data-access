using System;
using System.Collections.Generic;
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

        }

        public void Dispose()
        {
            database.DropDatabase();
        }
    }

    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}
