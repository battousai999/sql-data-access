using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace BattousaiSqlDataAccess.Tests
{
    /// <summary>
    /// Create a temporary test database. Either use static <see cref="Create"/>, or create an instance and call <see cref="CreateDatabase"/> followed by <see cref="InitConnectionString"/>
    /// </summary>
    public class TestDatabase
    {
        private readonly TestDatabaseOptions _testDatabaseOptions;

        /// <summary>
        /// Creates a temporary database overriding the app.Config connectionString to create a SqlServer 14 localDb.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <exception cref="System.ArgumentNullException">connectionStringName</exception>
        public static string Create(string connectionStringName)
        {
            if (connectionStringName == null) throw new ArgumentNullException("connectionStringName");

            var td = new TestDatabase(new TestDatabaseOptions(connectionStringName));
            td.CreateDatabase();

            return td.GetConnectionString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDatabase" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <exception cref="System.ArgumentNullException">databaseName</exception>
        public TestDatabase(string connectionStringName)
            : this(new TestDatabaseOptions(connectionStringName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDatabase" /> class with a database in a specified folder.
        /// </summary>
        /// <param name="testDatabaseOptions">The test database options.</param>
        /// <exception cref="System.ArgumentNullException">databaseName</exception>
        public TestDatabase(TestDatabaseOptions testDatabaseOptions)
        {
            if (testDatabaseOptions == null) throw new ArgumentNullException("testDatabaseOptions");

            _testDatabaseOptions = testDatabaseOptions;
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "Used by testing only. Not deployed in production.")]
        public void CreateDatabase()
        {
            var localDbMaster = GetConnectionStringToMaster();

            var isDetached = DetachDatabase(localDbMaster);
            if (!isDetached)
            {
                return; //reuse database
            }
            DeleteDatabaseFiles();

            using (var connection = new SqlConnection(localDbMaster))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("CREATE DATABASE {0} ON (NAME = N'{0}', FILENAME = '{1}.mdf')",
                    _testDatabaseOptions.DatabaseName,
                    _testDatabaseOptions.DatabaseFilePath);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Call this to detach and delete the database files.
        /// </summary>
        public void DropDatabase()
        {
            var localDbMaster = GetConnectionStringToMaster();
            DetachDatabase(localDbMaster);
            DeleteDatabaseFiles();
        }

        private string GetConnectionStringToMaster()
        {
            var csb = new SqlConnectionStringBuilder
            {
                DataSource = _testDatabaseOptions.DataSource,
                InitialCatalog = "master",
                IntegratedSecurity = true
            };
            return csb.ConnectionString;
        }

        public string GetConnectionString()
        {
            var csb = new SqlConnectionStringBuilder
            {
                DataSource = _testDatabaseOptions.DataSource,
                InitialCatalog = _testDatabaseOptions.DatabaseName,
                AttachDBFilename = _testDatabaseOptions.DatabaseFilePath + ".mdf",
                IntegratedSecurity = true,
                MultipleActiveResultSets = true,
            };
            return csb.ConnectionString;
        }

        private void DeleteDatabaseFiles()
        {
            var filePath = _testDatabaseOptions.DatabaseFilePath;
            try
            {
                if (File.Exists(filePath + ".mdf")) File.Delete(filePath + ".mdf");
                if (File.Exists(filePath + "_log.ldf")) File.Delete(filePath + "_log.ldf");
            }
            catch
            {
                Console.WriteLine("Could not delete the files (open in Visual Studio?)");
            }
        }

        private bool DetachDatabase(string localDbMaster)
        {
            using (var connection = new SqlConnection(localDbMaster))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                var databaseName = _testDatabaseOptions.DatabaseName;
                cmd.CommandText = @"if(exists(select name from sys.databases where name = @dbName))
BEGIN
    exec sp_detach_db @dbName
END ";
                cmd.Parameters.AddWithValue("dbName", databaseName); //can't do this for the exec
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Could not detach " + databaseName + " " + exception.Message);
                    return false;
                }
            }
        }
    }
}