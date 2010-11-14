/*******************************************************************************
 * Chinook Database - Version 1.3
 * Description: Test fixture for Chinook database.
 * DB Server: EffiProz
 * Author: Luis Rocha
 * License: http://www.codeplex.com/ChinookDatabase/license
 * 
 * IMPORTANT: In order to run these test fixtures, you will need to:
 *            1. Run the generated SQL script to create the database to be tested.
 *            2. Verify that app.config has the proper connection string (user/password).
 ********************************************************************************/
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.EffiProz;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChinookDatabase.Test.DatabaseTests
{
    public partial class ChinookEffiProzFixture
    {
        private const char Delimiter = '\x0013';
        const string InitialCatalogPattern = "Initial\\sCatalog(\\s)*=(\\s)*(?<InitialCatalog>([^;]*))";

        /// <summary>
        /// Retrieves the cached connection object.
        /// </summary>
        /// <param name="connectionName">Connection name in the configuration file.</param>
        /// <returns>A connection object for this specific database.</returns>
        protected EfzConnection GetConnection(string connectionName)
        {
            // Creates an ADO.NET connection to the database, if not created yet.
            if (!Connections.ContainsKey(connectionName))
            {
                var section = (ConnectionStringsSection)ConfigurationManager.GetSection("connectionStrings");

                foreach (var entry in section.ConnectionStrings.Cast<ConnectionStringSettings>()
                                                                .Where(entry => entry.Name == connectionName))
                {
                    var regex = new Regex(InitialCatalogPattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    var match = regex.Match(entry.ConnectionString);
                    var filename = match.Groups["InitialCatalog"].Value;
                    var dbname = "Test_" + Guid.NewGuid();
                    var connectionString = entry.ConnectionString.Replace(filename, dbname);
                    var conn = Connections[connectionName] = new EfzConnection(connectionString);

                    // Read the script to initialize database.
                    var content = File.ReadAllText(filename).Replace("\r\n\\", Delimiter.ToString())
                                                            .Replace(";\r\n", Delimiter.ToString())
                                                            .Replace("*/\r\n", "*/" + Delimiter)
                                                            .Replace("\r\n", " ");

                    var commands = (from item in content.Split(Delimiter)
                                    let trim = item.Trim()
                                    where
                                        !string.IsNullOrEmpty(trim) &&
                                        !(trim.StartsWith("/*") && trim.EndsWith("*/"))
                                    select trim).ToArray();

                    // Initialize database.
                    try
                    {
                        conn.Open();
                        foreach (var processingCommand in commands)
                        {
                            var command = conn.CreateCommand();
                            command.CommandText = processingCommand;
                            command.ExecuteNonQuery();
                        }
                    }
                    finally
                    {
                        //conn.Close();
                    }

                    Trace.WriteLine("Test database created: " + dbname);
                    break;
                }

                // If we failed to create a connection, then throw an exception.
                if (!Connections.ContainsKey(connectionName))
                    throw new ApplicationException("There is no connection string defined in app.config file.");
            }

            return Connections[connectionName];
        }

        internal void CreateDatabase(string filename)
        {
            var dbname = "Test_" + Guid.NewGuid();
            var connectionString = string.Format("Connection Type=File; Initial Catalog={0}; User=sa; Password=;", dbname);

            try
            {
                var content = File.ReadAllText(filename).Replace("\r\n\\", Delimiter.ToString())
                                                        .Replace(";\r\n", Delimiter.ToString())
                                                        .Replace("*/\r\n", "*/" + Delimiter)
                                                        .Replace("\r\n", " ");
                Console.WriteLine(content.Length);

                using (DbConnection conn = new EfzConnection(connectionString))
                {
                    conn.ConnectionString = connectionString;
                    conn.Open();

                    var commands = (from item in content.Split(Delimiter)
                                    let trim = item.Trim()
                                    where !string.IsNullOrEmpty(trim) && !(trim.StartsWith("/*") && trim.EndsWith("*/"))
                                    select trim).ToArray();

                    foreach (var processingCommand in commands)
                    {
                        var command = conn.CreateCommand();
                        command.CommandText = processingCommand;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Error: " + e.Message);
            }
        }
    }
}
