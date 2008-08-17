/*******************************************************************************
 * Chinook Database
 * Description: Test fixture for SQL Server version of Chinook database.
 * DB Server: SQL Server
 * License: http://www.codeplex.com/ChinookDatabase/license
 * 
 * IMPORTANT: In order to run these test fixtures, you will need to:
 *            1. Run the generated SQL script to create the database to be tested.
 *            2. Verify that app.config has the proper connection string (user/password).
 ********************************************************************************/
using System;
using System.Data;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Configuration;

namespace ChinookDatabase.Tests
{
    /// <summary>
    /// Class fixture for SQL Server version of Chinook database.
    /// </summary>
    [TestFixture]
    public class ChinookSqlServerFixture : DatabaseFixture
    {
        static SqlConnection _sqlConnection;

        /// <summary>
        /// Retrieves the cached connection object.
        /// </summary>
        /// <returns>A connection object for this specific database.</returns>
        protected static SqlConnection GetConnection()
        {
            // Creates an ADO.NET connection to the database, if not created yet.
            if (_sqlConnection == null)
            {
                ConnectionStringsSection section = (ConnectionStringsSection) ConfigurationManager.GetSection("connectionStrings");

                foreach (ConnectionStringSettings entry in section.ConnectionStrings)
                {
                    if (entry.Name == "ChinookSqlServer")
                    {
                        _sqlConnection = new SqlConnection(entry.ConnectionString);
                        break;
                    }
                }
            }

            // If we failed to create a connection, then throw an exception.
            if (_sqlConnection == null)
            {
                throw new ApplicationException("There is no connection string defined in app.config file.");
            }

            return _sqlConnection;
        }

        /// <summary>
        /// Method to execute a SQL query and return a dataset.
        /// </summary>
        /// <param name="query">Query string to be executed.</param>
        /// <returns>DataSet with the query results.</returns>
        protected override DataSet ExecuteQuery(string query)
        {
            DataSet dataset = new DataSet();

            // Verify if number of entities match number of records.
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                adapter.SelectCommand = new SqlCommand(query, GetConnection());
                adapter.Fill(dataset);
            }

            return dataset;
        }
    }
}
