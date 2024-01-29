/*******************************************************************************
 * Chinook Database - Version 1.4.3
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
        public void InvoiceTotalMustMatchSumOfInvoiceLines(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT invoice.invoice_id, SUM(invoice_line.unit_price * invoice_line.quantity) AS CalculatedTotal, invoice.total AS Total FROM invoice_line INNER JOIN invoice ON invoice_line.invoice_id = invoice.invoice_id GROUP BY invoice.invoice_id, invoice.total");

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                Assert.True(row["CalculatedTotal"].ToString() == row["Total"].ToString(), string.Format("The total field of InvoiceId={0} does not match its invoice lines.", row["InvoiceId"]));
            }
        }

        /// <summary>
        /// Verifies that the Genre table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void GenreLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM genre ORDER BY genre_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("25", row["GenreId"].ToString());
            Assert.Equal("Opera", row["Name"].ToString());
        }

        /// <summary>
        /// Verifies that the MediaType table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void MediaTypeLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM media_type ORDER BY media_type_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("5", row["MediaTypeId"].ToString());
            Assert.Equal("AAC audio file", row["Name"].ToString());
        }

        /// <summary>
        /// Verifies that the Artist table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void ArtistLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM artist ORDER BY artist_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("275", row["ArtistId"].ToString());
            Assert.Equal("Philip Glass Ensemble", row["Name"].ToString());
        }

        /// <summary>
        /// Verifies that the Album table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void AlbumLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM album ORDER BY album_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("347", row["AlbumId"].ToString());
            Assert.Equal("Koyaanisqatsi (Soundtrack from the Motion Picture)", row["Title"].ToString());
            Assert.Equal("275", row["ArtistId"].ToString());
        }

        /// <summary>
        /// Verifies that the Track table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void TrackLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM track ORDER BY track_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("3503", row["TrackId"].ToString());
            Assert.Equal("Koyaanisqatsi", row["Name"].ToString());
            Assert.Equal("347", row["AlbumId"].ToString());
            Assert.Equal("2", row["MediaTypeId"].ToString());
            Assert.Equal("10", row["GenreId"].ToString());
            Assert.Equal("Philip Glass", row["Composer"].ToString());
            Assert.Equal("206005", row["Milliseconds"].ToString());
            Assert.Equal("3305164", row["Bytes"].ToString());
            Assert.Equal("0.99", row["UnitPrice"].ToString());
        }

        /// <summary>
        /// Verifies that the Employee table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void EmployeeLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM employee ORDER BY employee_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("8", row["EmployeeId"].ToString());
            Assert.Equal("Callahan", row["LastName"].ToString());
            Assert.Equal("Laura", row["FirstName"].ToString());
            Assert.Equal("IT Staff", row["Title"].ToString());
            Assert.Equal("6", row["ReportsTo"].ToString());
            Assert.Equal("1/9/1968 12:00:00 AM", row["BirthDate"].ToString());
            Assert.Equal("3/4/2004 12:00:00 AM", row["HireDate"].ToString());
            Assert.Equal("923 7 ST NW", row["Address"].ToString());
            Assert.Equal("Lethbridge", row["City"].ToString());
            Assert.Equal("AB", row["State"].ToString());
            Assert.Equal("Canada", row["Country"].ToString());
            Assert.Equal("T1H 1Y8", row["PostalCode"].ToString());
            Assert.Equal("+1 (403) 467-3351", row["Phone"].ToString());
            Assert.Equal("+1 (403) 467-8772", row["Fax"].ToString());
            Assert.Equal("laura@chinookcorp.com", row["Email"].ToString());
        }

        /// <summary>
        /// Verifies that the Customer table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void CustomerLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer ORDER BY customer_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("59", row["CustomerId"].ToString());
            Assert.Equal("Puja", row["FirstName"].ToString());
            Assert.Equal("Srivastava", row["LastName"].ToString());
            Assert.Equal("", row["Company"].ToString());
            Assert.Equal("3,Raj Bhavan Road", row["Address"].ToString());
            Assert.Equal("Bangalore", row["City"].ToString());
            Assert.Equal("", row["State"].ToString());
            Assert.Equal("India", row["Country"].ToString());
            Assert.Equal("560001", row["PostalCode"].ToString());
            Assert.Equal("+91 080 22289999", row["Phone"].ToString());
            Assert.Equal("", row["Fax"].ToString());
            Assert.Equal("puja_srivastava@yahoo.in", row["Email"].ToString());
            Assert.Equal("3", row["SupportRepId"].ToString());
        }

        /// <summary>
        /// Verifies that the Invoice table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void InvoiceLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM invoice ORDER BY invoice_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("412", row["InvoiceId"].ToString());
            Assert.Equal("58", row["CustomerId"].ToString());
            Assert.Equal("12/22/2025 12:00:00 AM", row["InvoiceDate"].ToString());
            Assert.Equal("12,Community Centre", row["BillingAddress"].ToString());
            Assert.Equal("Delhi", row["BillingCity"].ToString());
            Assert.Equal("", row["BillingState"].ToString());
            Assert.Equal("India", row["BillingCountry"].ToString());
            Assert.Equal("110017", row["BillingPostalCode"].ToString());
            Assert.Equal("1.99", row["Total"].ToString());
        }

        /// <summary>
        /// Verifies that the InvoiceLine table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void InvoiceLineLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM invoice_line ORDER BY invoice_line_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("2240", row["InvoiceLineId"].ToString());
            Assert.Equal("412", row["InvoiceId"].ToString());
            Assert.Equal("3177", row["TrackId"].ToString());
            Assert.Equal("1.99", row["UnitPrice"].ToString());
            Assert.Equal("1", row["Quantity"].ToString());
        }

        /// <summary>
        /// Verifies that the Playlist table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void PlaylistLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM playlist ORDER BY playlist_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("18", row["PlaylistId"].ToString());
            Assert.Equal("On-The-Go 1", row["Name"].ToString());
        }

        /// <summary>
        /// Verifies that the PlaylistTrack table was populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
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
        public void PlaylistTrackLastRecordHasProperInfo(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM playlist_track ORDER BY playlist_id, track_id");
            var table = dataSet.Tables[0];
            Assert.NotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.NotNull(row);

			// Assert that the last record has the proper information.            
            Assert.Equal("18", row["PlaylistId"].ToString());
            Assert.Equal("597", row["TrackId"].ToString());
        }

        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId01HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 1");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("1", row["CustomerId"].ToString());
            Assert.Equal("Luís", row["FirstName"].ToString());
            Assert.Equal("Gonçalves", row["LastName"].ToString());
            Assert.Equal("Embraer - Empresa Brasileira de Aeronáutica S.A.", row["Company"].ToString());
            Assert.Equal("Av. Brigadeiro Faria Lima, 2170", row["Address"].ToString());
            Assert.Equal("São José dos Campos", row["City"].ToString());
            Assert.Equal("SP", row["State"].ToString());
            Assert.Equal("Brazil", row["Country"].ToString());
            Assert.Equal("12227-000", row["PostalCode"].ToString());
            Assert.Equal("+55 (12) 3923-5555", row["Phone"].ToString());
            Assert.Equal("+55 (12) 3923-5566", row["Fax"].ToString());
            Assert.Equal("luisg@embraer.com.br", row["Email"].ToString());
            Assert.Equal("3", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId02HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 2");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("2", row["CustomerId"].ToString());
            Assert.Equal("Leonie", row["FirstName"].ToString());
            Assert.Equal("Köhler", row["LastName"].ToString());
            Assert.Equal("", row["Company"].ToString());
            Assert.Equal("Theodor-Heuss-Straße 34", row["Address"].ToString());
            Assert.Equal("Stuttgart", row["City"].ToString());
            Assert.Equal("", row["State"].ToString());
            Assert.Equal("Germany", row["Country"].ToString());
            Assert.Equal("70174", row["PostalCode"].ToString());
            Assert.Equal("+49 0711 2842222", row["Phone"].ToString());
            Assert.Equal("", row["Fax"].ToString());
            Assert.Equal("leonekohler@surfeu.de", row["Email"].ToString());
            Assert.Equal("5", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId03HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 3");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("3", row["CustomerId"].ToString());
            Assert.Equal("François", row["FirstName"].ToString());
            Assert.Equal("Tremblay", row["LastName"].ToString());
            Assert.Equal("", row["Company"].ToString());
            Assert.Equal("1498 rue Bélanger", row["Address"].ToString());
            Assert.Equal("Montréal", row["City"].ToString());
            Assert.Equal("QC", row["State"].ToString());
            Assert.Equal("Canada", row["Country"].ToString());
            Assert.Equal("H2G 1A7", row["PostalCode"].ToString());
            Assert.Equal("+1 (514) 721-4711", row["Phone"].ToString());
            Assert.Equal("", row["Fax"].ToString());
            Assert.Equal("ftremblay@gmail.com", row["Email"].ToString());
            Assert.Equal("3", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId04HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 4");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("4", row["CustomerId"].ToString());
            Assert.Equal("Bjørn", row["FirstName"].ToString());
            Assert.Equal("Hansen", row["LastName"].ToString());
            Assert.Equal("", row["Company"].ToString());
            Assert.Equal("Ullevålsveien 14", row["Address"].ToString());
            Assert.Equal("Oslo", row["City"].ToString());
            Assert.Equal("", row["State"].ToString());
            Assert.Equal("Norway", row["Country"].ToString());
            Assert.Equal("0171", row["PostalCode"].ToString());
            Assert.Equal("+47 22 44 22 22", row["Phone"].ToString());
            Assert.Equal("", row["Fax"].ToString());
            Assert.Equal("bjorn.hansen@yahoo.no", row["Email"].ToString());
            Assert.Equal("4", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId05HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 5");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("5", row["CustomerId"].ToString());
            Assert.Equal("František", row["FirstName"].ToString());
            Assert.Equal("Wichterlová", row["LastName"].ToString());
            Assert.Equal("JetBrains s.r.o.", row["Company"].ToString());
            Assert.Equal("Klanova 9/506", row["Address"].ToString());
            Assert.Equal("Prague", row["City"].ToString());
            Assert.Equal("", row["State"].ToString());
            Assert.Equal("Czech Republic", row["Country"].ToString());
            Assert.Equal("14700", row["PostalCode"].ToString());
            Assert.Equal("+420 2 4172 5555", row["Phone"].ToString());
            Assert.Equal("+420 2 4172 5555", row["Fax"].ToString());
            Assert.Equal("frantisekw@jetbrains.com", row["Email"].ToString());
            Assert.Equal("4", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId06HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 6");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("6", row["CustomerId"].ToString());
            Assert.Equal("Helena", row["FirstName"].ToString());
            Assert.Equal("Holý", row["LastName"].ToString());
            Assert.Equal("", row["Company"].ToString());
            Assert.Equal("Rilská 3174/6", row["Address"].ToString());
            Assert.Equal("Prague", row["City"].ToString());
            Assert.Equal("", row["State"].ToString());
            Assert.Equal("Czech Republic", row["Country"].ToString());
            Assert.Equal("14300", row["PostalCode"].ToString());
            Assert.Equal("+420 2 4177 0449", row["Phone"].ToString());
            Assert.Equal("", row["Fax"].ToString());
            Assert.Equal("hholy@gmail.com", row["Email"].ToString());
            Assert.Equal("5", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId07HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 7");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("7", row["CustomerId"].ToString());
            Assert.Equal("Astrid", row["FirstName"].ToString());
            Assert.Equal("Gruber", row["LastName"].ToString());
            Assert.Equal("", row["Company"].ToString());
            Assert.Equal("Rotenturmstraße 4, 1010 Innere Stadt", row["Address"].ToString());
            Assert.Equal("Vienne", row["City"].ToString());
            Assert.Equal("", row["State"].ToString());
            Assert.Equal("Austria", row["Country"].ToString());
            Assert.Equal("1010", row["PostalCode"].ToString());
            Assert.Equal("+43 01 5134505", row["Phone"].ToString());
            Assert.Equal("", row["Fax"].ToString());
            Assert.Equal("astrid.gruber@apple.at", row["Email"].ToString());
            Assert.Equal("5", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId08HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 8");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("8", row["CustomerId"].ToString());
            Assert.Equal("Daan", row["FirstName"].ToString());
            Assert.Equal("Peeters", row["LastName"].ToString());
            Assert.Equal("", row["Company"].ToString());
            Assert.Equal("Grétrystraat 63", row["Address"].ToString());
            Assert.Equal("Brussels", row["City"].ToString());
            Assert.Equal("", row["State"].ToString());
            Assert.Equal("Belgium", row["Country"].ToString());
            Assert.Equal("1000", row["PostalCode"].ToString());
            Assert.Equal("+32 02 219 03 03", row["Phone"].ToString());
            Assert.Equal("", row["Fax"].ToString());
            Assert.Equal("daan_peeters@apple.be", row["Email"].ToString());
            Assert.Equal("4", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId09HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 9");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("9", row["CustomerId"].ToString());
            Assert.Equal("Kara", row["FirstName"].ToString());
            Assert.Equal("Nielsen", row["LastName"].ToString());
            Assert.Equal("", row["Company"].ToString());
            Assert.Equal("Sønder Boulevard 51", row["Address"].ToString());
            Assert.Equal("Copenhagen", row["City"].ToString());
            Assert.Equal("", row["State"].ToString());
            Assert.Equal("Denmark", row["Country"].ToString());
            Assert.Equal("1720", row["PostalCode"].ToString());
            Assert.Equal("+453 3331 9991", row["Phone"].ToString());
            Assert.Equal("", row["Fax"].ToString());
            Assert.Equal("kara.nielsen@jubii.dk", row["Email"].ToString());
            Assert.Equal("4", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId10HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 10");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("10", row["CustomerId"].ToString());
            Assert.Equal("Eduardo", row["FirstName"].ToString());
            Assert.Equal("Martins", row["LastName"].ToString());
            Assert.Equal("Woodstock Discos", row["Company"].ToString());
            Assert.Equal("Rua Dr. Falcão Filho, 155", row["Address"].ToString());
            Assert.Equal("São Paulo", row["City"].ToString());
            Assert.Equal("SP", row["State"].ToString());
            Assert.Equal("Brazil", row["Country"].ToString());
            Assert.Equal("01007-010", row["PostalCode"].ToString());
            Assert.Equal("+55 (11) 3033-5446", row["Phone"].ToString());
            Assert.Equal("+55 (11) 3033-4564", row["Fax"].ToString());
            Assert.Equal("eduardo@woodstock.com.br", row["Email"].ToString());
            Assert.Equal("4", row["SupportRepId"].ToString());
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Theory]
		[InlineData("Chinook_PostgreSql")]
        public void CustomerId11HasProperUnicodeCharacters(string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM customer WHERE customer_id = 11");
            Assert.Equal(1, dataSet.Tables[0].Rows.Count); // Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.Equal("11", row["CustomerId"].ToString());
            Assert.Equal("Alexandre", row["FirstName"].ToString());
            Assert.Equal("Rocha", row["LastName"].ToString());
            Assert.Equal("Banco do Brasil S.A.", row["Company"].ToString());
            Assert.Equal("Av. Paulista, 2022", row["Address"].ToString());
            Assert.Equal("São Paulo", row["City"].ToString());
            Assert.Equal("SP", row["State"].ToString());
            Assert.Equal("Brazil", row["Country"].ToString());
            Assert.Equal("01310-200", row["PostalCode"].ToString());
            Assert.Equal("+55 (11) 3055-3278", row["Phone"].ToString());
            Assert.Equal("+55 (11) 3055-8131", row["Fax"].ToString());
            Assert.Equal("alero@uol.com.br", row["Email"].ToString());
            Assert.Equal("5", row["SupportRepId"].ToString());
		}
		
    }
	
}
