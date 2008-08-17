/*******************************************************************************
 * Chinook Database
 * Description: Test fixture for MySQL version of Chinook database.
 * DB Server: SQL Server
 * License: http://www.codeplex.com/ChinookDatabase/license
 * 
 * IMPORTANT: In order to run these test fixtures, you will need to:
 *            1. Run the generated SQL script to create the database to be tested.
 *            2. Verify that app.config has the proper connection string (user/password).
 ********************************************************************************/
using System;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System.Configuration;
using NUnit.Framework.SyntaxHelpers;

namespace ChinookDatabase.Tests
{
    /// <summary>
    /// Class fixture for MySQL version of Chinook database.
    /// </summary>
    [TestFixture]
    public class ChinookMySqlFixture : DatabaseFixture
    {
        static MySqlConnection _connection;

        /// <summary>
        /// Retrieves the cached connection object.
        /// </summary>
        /// <returns>A connection object for this specific database.</returns>
        protected static MySqlConnection GetConnection()
        {
            // Creates an ADO.NET connection to the database, if not created yet.
            if (_connection == null)
            {
                ConnectionStringsSection section = (ConnectionStringsSection) ConfigurationManager.GetSection("connectionStrings");
                foreach (ConnectionStringSettings entry in section.ConnectionStrings)
                {
                    if (entry.Name == "ChinookMySql")
                    {
                        _connection = new MySqlConnection(entry.ConnectionString);
                        break;
                    }
                }
            }

            // If we failed to create a connection, then throw an exception.
            if (_connection == null)
            {
                throw new ApplicationException("There is no connection string defined in app.config file.");
            }
            return _connection;
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
            using (MySqlDataAdapter adapter = new MySqlDataAdapter())
            {
                adapter.SelectCommand = new MySqlCommand(query, GetConnection());
                adapter.Fill(dataset);
            }

            return dataset;
        }
    }
}
