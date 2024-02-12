/*******************************************************************************
 * Chinook Database - Version 1.4.5
 * Description: Test fixture for Chinook database.
 * DB Server: PostgreSql
 * Author: Luis Rocha
 * License: https://github.com/lerocha/chinook-database/blob/master/LICENSE.md
 * 
 * IMPORTANT: In order to run these test fixtures, you will need to:
 *            1. Run the generated SQL script to create the database to be tested.
 *            2. Verify that app.config has the proper connection string (user/password).
 ********************************************************************************/
using System.Data;
using Xunit;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace ChinookDatabase.Test.DatabaseTests
{
    /// <summary>
    /// Test fixtures for PostgreSql databases.
    /// </summary>
    public partial class ChinookPostgreSqlFixture : IDisposable
    {
        protected IDictionary<string, NpgsqlConnection> Connections;

        /// <summary>
        /// Retrieves the cached connection object.
        /// </summary>
        /// <param name="connectionName">Connection name in the configuration file.</param>
        /// <returns>A connection object for this specific database.</returns>
        protected NpgsqlConnection GetConnection(string connectionName)
        {
            // Creates an ADO.NET connection to the database, if not created yet.
            if (Connections.ContainsKey(connectionName))
            {
                return Connections[connectionName];
            }

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
            var connectionString = config.GetConnectionString(connectionName) ?? throw new ApplicationException("Cannot find connection string in appsettings.test.json");
            Connections[connectionName] = new NpgsqlConnection(connectionString);
            return Connections[connectionName];
        }

        /// <summary>
        /// Method to execute a SQL query and return a dataset.
        /// </summary>
        /// <param name="connectionName">Connection name in the configuration file.</param>
        /// <param name="query">Query string to be executed.</param>
        /// <returns>DataSet with the query results.</returns>
        protected DataSet ExecuteQuery(string connectionName, string query)
        {
            var dataset = new DataSet();
			var connection = GetConnection(connectionName);

            // Verify if number of entities match number of records.
            using (var adapter = new NpgsqlDataAdapter(query, connection))
            {
                adapter.Fill(dataset);
            }

            return dataset;
        }
        
        /// <summary>
        /// Initialize connections dictionary.
        /// </summary>
        public ChinookPostgreSqlFixture()
        {
            Connections = new Dictionary<string, NpgsqlConnection>();
        }

        /// <summary>
        /// Close all connections.
        /// </summary>
        public void Dispose()
        {
            foreach (var connection in Connections.Values)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Asserts that all invoices contain invoice lines.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void AllInvoicesMustHaveInvoiceLines(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT count(invoice_id) FROM invoice WHERE invoice_id NOT IN (SELECT invoice_id FROM invoice_line GROUP BY invoice_id)");
            Assert.Equal("0", dataSet.Tables[0].Rows[0][0].ToString());
        }
        
        /// <summary>
        /// Asserts that invoice total matches sum of invoice lines.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void InvoiceTotalMustMatchSumOfInvoiceLines(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT invoice.invoice_id, SUM(invoice_line.unit_price * invoice_line.quantity) AS CalculatedTotal, invoice.total AS Total FROM invoice_line INNER JOIN invoice ON invoice_line.invoice_id = invoice.invoice_id GROUP BY invoice.invoice_id, invoice.total");

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                Assert.True(row["CalculatedTotal"].ToString() == row["Total"].ToString(), $"The total field of InvoiceId={row["invoice_id"]} does not match its invoice lines.");
            }
        }

        /// <summary>
        /// Verifies that the Genre table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void GenreTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM genre");
            Assert.Equal(25, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of Genre table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void GenreLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM genre ORDER BY genre_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("25", row["genre_id"].ToString());
            Assert.Equal("Opera", row["name"].ToString());
        }

        /// <summary>
        /// Verifies that the MediaType table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void MediaTypeTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM media_type");
            Assert.Equal(5, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of MediaType table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void MediaTypeLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM media_type ORDER BY media_type_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("5", row["media_type_id"].ToString());
            Assert.Equal("AAC audio file", row["name"].ToString());
        }

        /// <summary>
        /// Verifies that the Artist table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void ArtistTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM artist");
            Assert.Equal(275, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of Artist table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void ArtistLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM artist ORDER BY artist_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("275", row["artist_id"].ToString());
            Assert.Equal("Philip Glass Ensemble", row["name"].ToString());
        }

        /// <summary>
        /// Verifies that the Album table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void AlbumTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM album");
            Assert.Equal(347, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of Album table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void AlbumLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM album ORDER BY album_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("347", row["album_id"].ToString());
            Assert.Equal("Koyaanisqatsi (Soundtrack from the Motion Picture)", row["title"].ToString());
            Assert.Equal("275", row["artist_id"].ToString());
        }

        /// <summary>
        /// Verifies that the Track table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void TrackTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM track");
            Assert.Equal(3503, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of Track table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void TrackLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM track ORDER BY track_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("3503", row["track_id"].ToString());
            Assert.Equal("Koyaanisqatsi", row["name"].ToString());
            Assert.Equal("347", row["album_id"].ToString());
            Assert.Equal("2", row["media_type_id"].ToString());
            Assert.Equal("10", row["genre_id"].ToString());
            Assert.Equal("Philip Glass", row["composer"].ToString());
            Assert.Equal("206005", row["milliseconds"].ToString());
            Assert.Equal("3305164", row["bytes"].ToString());
            Assert.Equal("0.99", row["unit_price"].ToString());
        }

        /// <summary>
        /// Verifies that the Employee table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void EmployeeTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM employee");
            Assert.Equal(8, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of Employee table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void EmployeeLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM employee ORDER BY employee_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("8", row["employee_id"].ToString());
            Assert.Equal("Callahan", row["last_name"].ToString());
            Assert.Equal("Laura", row["first_name"].ToString());
            Assert.Equal("IT Staff", row["title"].ToString());
            Assert.Equal("6", row["reports_to"].ToString());
            Assert.Equal(DateTime.Parse("1/9/1968 12:00:00 AM").ToString(), row["birth_date"].ToString());
            Assert.Equal(DateTime.Parse("3/4/2004 12:00:00 AM").ToString(), row["hire_date"].ToString());
            Assert.Equal("923 7 ST NW", row["address"].ToString());
            Assert.Equal("Lethbridge", row["city"].ToString());
            Assert.Equal("AB", row["state"].ToString());
            Assert.Equal("Canada", row["country"].ToString());
            Assert.Equal("T1H 1Y8", row["postal_code"].ToString());
            Assert.Equal("+1 (403) 467-3351", row["phone"].ToString());
            Assert.Equal("+1 (403) 467-8772", row["fax"].ToString());
            Assert.Equal("laura@chinookcorp.com", row["email"].ToString());
        }

        /// <summary>
        /// Verifies that the Customer table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer");
            Assert.Equal(59, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of Customer table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer ORDER BY customer_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("59", row["customer_id"].ToString());
            Assert.Equal("Puja", row["first_name"].ToString());
            Assert.Equal("Srivastava", row["last_name"].ToString());
            Assert.Equal("", row["company"].ToString());
            Assert.Equal("3,Raj Bhavan Road", row["address"].ToString());
            Assert.Equal("Bangalore", row["city"].ToString());
            Assert.Equal("", row["state"].ToString());
            Assert.Equal("India", row["country"].ToString());
            Assert.Equal("560001", row["postal_code"].ToString());
            Assert.Equal("+91 080 22289999", row["phone"].ToString());
            Assert.Equal("", row["fax"].ToString());
            Assert.Equal("puja_srivastava@yahoo.in", row["email"].ToString());
            Assert.Equal("3", row["support_rep_id"].ToString());
        }

        /// <summary>
        /// Verifies that the Invoice table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void InvoiceTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM invoice");
            Assert.Equal(412, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of Invoice table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void InvoiceLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM invoice ORDER BY invoice_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("412", row["invoice_id"].ToString());
            Assert.Equal("58", row["customer_id"].ToString());
            Assert.Equal(DateTime.Parse("12/22/2025 12:00:00 AM").ToString(), row["invoice_date"].ToString());
            Assert.Equal("12,Community Centre", row["billing_address"].ToString());
            Assert.Equal("Delhi", row["billing_city"].ToString());
            Assert.Equal("", row["billing_state"].ToString());
            Assert.Equal("India", row["billing_country"].ToString());
            Assert.Equal("110017", row["billing_postal_code"].ToString());
            Assert.Equal("1.99", row["total"].ToString());
        }

        /// <summary>
        /// Verifies that the InvoiceLine table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void InvoiceLineTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM invoice_line");
            Assert.Equal(2240, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of InvoiceLine table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void InvoiceLineLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM invoice_line ORDER BY invoice_line_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("2240", row["invoice_line_id"].ToString());
            Assert.Equal("412", row["invoice_id"].ToString());
            Assert.Equal("3177", row["track_id"].ToString());
            Assert.Equal("1.99", row["unit_price"].ToString());
            Assert.Equal("1", row["quantity"].ToString());
        }

        /// <summary>
        /// Verifies that the Playlist table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void PlaylistTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM playlist");
            Assert.Equal(18, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of Playlist table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void PlaylistLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM playlist ORDER BY playlist_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("18", row["playlist_id"].ToString());
            Assert.Equal("On-The-Go 1", row["name"].ToString());
        }

        /// <summary>
        /// Verifies that the PlaylistTrack table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void PlaylistTrackTableShouldBePopulated(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM playlist_track");
            Assert.Equal(8715, dataSet.Tables[0].Rows.Count);
        }

        /// <summary>
        /// Verifies that last record of PlaylistTrack table has the proper information.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void PlaylistTrackLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM playlist_track ORDER BY playlist_id, track_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("18", row["playlist_id"].ToString());
            Assert.Equal("597", row["track_id"].ToString());
        }

        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId01HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 1");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("1", row["customer_id"].ToString());
            Assert.Equal("Luís", row["first_name"].ToString());
            Assert.Equal("Gonçalves", row["last_name"].ToString());
            Assert.Equal("Embraer - Empresa Brasileira de Aeronáutica S.A.", row["company"].ToString());
            Assert.Equal("Av. Brigadeiro Faria Lima, 2170", row["address"].ToString());
            Assert.Equal("São José dos Campos", row["city"].ToString());
            Assert.Equal("SP", row["state"].ToString());
            Assert.Equal("Brazil", row["country"].ToString());
            Assert.Equal("12227-000", row["postal_code"].ToString());
            Assert.Equal("+55 (12) 3923-5555", row["phone"].ToString());
            Assert.Equal("+55 (12) 3923-5566", row["fax"].ToString());
            Assert.Equal("luisg@embraer.com.br", row["email"].ToString());
            Assert.Equal("3", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId02HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 2");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("2", row["customer_id"].ToString());
            Assert.Equal("Leonie", row["first_name"].ToString());
            Assert.Equal("Köhler", row["last_name"].ToString());
            Assert.Equal("", row["company"].ToString());
            Assert.Equal("Theodor-Heuss-Straße 34", row["address"].ToString());
            Assert.Equal("Stuttgart", row["city"].ToString());
            Assert.Equal("", row["state"].ToString());
            Assert.Equal("Germany", row["country"].ToString());
            Assert.Equal("70174", row["postal_code"].ToString());
            Assert.Equal("+49 0711 2842222", row["phone"].ToString());
            Assert.Equal("", row["fax"].ToString());
            Assert.Equal("leonekohler@surfeu.de", row["email"].ToString());
            Assert.Equal("5", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId03HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 3");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("3", row["customer_id"].ToString());
            Assert.Equal("François", row["first_name"].ToString());
            Assert.Equal("Tremblay", row["last_name"].ToString());
            Assert.Equal("", row["company"].ToString());
            Assert.Equal("1498 rue Bélanger", row["address"].ToString());
            Assert.Equal("Montréal", row["city"].ToString());
            Assert.Equal("QC", row["state"].ToString());
            Assert.Equal("Canada", row["country"].ToString());
            Assert.Equal("H2G 1A7", row["postal_code"].ToString());
            Assert.Equal("+1 (514) 721-4711", row["phone"].ToString());
            Assert.Equal("", row["fax"].ToString());
            Assert.Equal("ftremblay@gmail.com", row["email"].ToString());
            Assert.Equal("3", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId04HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 4");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("4", row["customer_id"].ToString());
            Assert.Equal("Bjørn", row["first_name"].ToString());
            Assert.Equal("Hansen", row["last_name"].ToString());
            Assert.Equal("", row["company"].ToString());
            Assert.Equal("Ullevålsveien 14", row["address"].ToString());
            Assert.Equal("Oslo", row["city"].ToString());
            Assert.Equal("", row["state"].ToString());
            Assert.Equal("Norway", row["country"].ToString());
            Assert.Equal("0171", row["postal_code"].ToString());
            Assert.Equal("+47 22 44 22 22", row["phone"].ToString());
            Assert.Equal("", row["fax"].ToString());
            Assert.Equal("bjorn.hansen@yahoo.no", row["email"].ToString());
            Assert.Equal("4", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId05HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 5");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("5", row["customer_id"].ToString());
            Assert.Equal("František", row["first_name"].ToString());
            Assert.Equal("Wichterlová", row["last_name"].ToString());
            Assert.Equal("JetBrains s.r.o.", row["company"].ToString());
            Assert.Equal("Klanova 9/506", row["address"].ToString());
            Assert.Equal("Prague", row["city"].ToString());
            Assert.Equal("", row["state"].ToString());
            Assert.Equal("Czech Republic", row["country"].ToString());
            Assert.Equal("14700", row["postal_code"].ToString());
            Assert.Equal("+420 2 4172 5555", row["phone"].ToString());
            Assert.Equal("+420 2 4172 5555", row["fax"].ToString());
            Assert.Equal("frantisekw@jetbrains.com", row["email"].ToString());
            Assert.Equal("4", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId06HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 6");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("6", row["customer_id"].ToString());
            Assert.Equal("Helena", row["first_name"].ToString());
            Assert.Equal("Holý", row["last_name"].ToString());
            Assert.Equal("", row["company"].ToString());
            Assert.Equal("Rilská 3174/6", row["address"].ToString());
            Assert.Equal("Prague", row["city"].ToString());
            Assert.Equal("", row["state"].ToString());
            Assert.Equal("Czech Republic", row["country"].ToString());
            Assert.Equal("14300", row["postal_code"].ToString());
            Assert.Equal("+420 2 4177 0449", row["phone"].ToString());
            Assert.Equal("", row["fax"].ToString());
            Assert.Equal("hholy@gmail.com", row["email"].ToString());
            Assert.Equal("5", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId07HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 7");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("7", row["customer_id"].ToString());
            Assert.Equal("Astrid", row["first_name"].ToString());
            Assert.Equal("Gruber", row["last_name"].ToString());
            Assert.Equal("", row["company"].ToString());
            Assert.Equal("Rotenturmstraße 4, 1010 Innere Stadt", row["address"].ToString());
            Assert.Equal("Vienne", row["city"].ToString());
            Assert.Equal("", row["state"].ToString());
            Assert.Equal("Austria", row["country"].ToString());
            Assert.Equal("1010", row["postal_code"].ToString());
            Assert.Equal("+43 01 5134505", row["phone"].ToString());
            Assert.Equal("", row["fax"].ToString());
            Assert.Equal("astrid.gruber@apple.at", row["email"].ToString());
            Assert.Equal("5", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId08HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 8");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("8", row["customer_id"].ToString());
            Assert.Equal("Daan", row["first_name"].ToString());
            Assert.Equal("Peeters", row["last_name"].ToString());
            Assert.Equal("", row["company"].ToString());
            Assert.Equal("Grétrystraat 63", row["address"].ToString());
            Assert.Equal("Brussels", row["city"].ToString());
            Assert.Equal("", row["state"].ToString());
            Assert.Equal("Belgium", row["country"].ToString());
            Assert.Equal("1000", row["postal_code"].ToString());
            Assert.Equal("+32 02 219 03 03", row["phone"].ToString());
            Assert.Equal("", row["fax"].ToString());
            Assert.Equal("daan_peeters@apple.be", row["email"].ToString());
            Assert.Equal("4", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId09HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 9");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("9", row["customer_id"].ToString());
            Assert.Equal("Kara", row["first_name"].ToString());
            Assert.Equal("Nielsen", row["last_name"].ToString());
            Assert.Equal("", row["company"].ToString());
            Assert.Equal("Sønder Boulevard 51", row["address"].ToString());
            Assert.Equal("Copenhagen", row["city"].ToString());
            Assert.Equal("", row["state"].ToString());
            Assert.Equal("Denmark", row["country"].ToString());
            Assert.Equal("1720", row["postal_code"].ToString());
            Assert.Equal("+453 3331 9991", row["phone"].ToString());
            Assert.Equal("", row["fax"].ToString());
            Assert.Equal("kara.nielsen@jubii.dk", row["email"].ToString());
            Assert.Equal("4", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId10HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 10");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("10", row["customer_id"].ToString());
            Assert.Equal("Eduardo", row["first_name"].ToString());
            Assert.Equal("Martins", row["last_name"].ToString());
            Assert.Equal("Woodstock Discos", row["company"].ToString());
            Assert.Equal("Rua Dr. Falcão Filho, 155", row["address"].ToString());
            Assert.Equal("São Paulo", row["city"].ToString());
            Assert.Equal("SP", row["state"].ToString());
            Assert.Equal("Brazil", row["country"].ToString());
            Assert.Equal("01007-010", row["postal_code"].ToString());
            Assert.Equal("+55 (11) 3033-5446", row["phone"].ToString());
            Assert.Equal("+55 (11) 3033-4564", row["fax"].ToString());
            Assert.Equal("eduardo@woodstock.com.br", row["email"].ToString());
            Assert.Equal("4", row["support_rep_id"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
		[InlineData("Chinook_PostgreSql_AutoIncrement")]
		[InlineData("Chinook_PostgreSql_Serial")]
        public void CustomerId11HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 11");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("11", row["customer_id"].ToString());
            Assert.Equal("Alexandre", row["first_name"].ToString());
            Assert.Equal("Rocha", row["last_name"].ToString());
            Assert.Equal("Banco do Brasil S.A.", row["company"].ToString());
            Assert.Equal("Av. Paulista, 2022", row["address"].ToString());
            Assert.Equal("São Paulo", row["city"].ToString());
            Assert.Equal("SP", row["state"].ToString());
            Assert.Equal("Brazil", row["country"].ToString());
            Assert.Equal("01310-200", row["postal_code"].ToString());
            Assert.Equal("+55 (11) 3055-3278", row["phone"].ToString());
            Assert.Equal("+55 (11) 3055-8131", row["fax"].ToString());
            Assert.Equal("alero@uol.com.br", row["email"].ToString());
            Assert.Equal("5", row["support_rep_id"].ToString());
		}
		
    }
	
}
