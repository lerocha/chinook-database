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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;
using System.Data.EffiProz;

namespace ChinookDatabase.Test.DatabaseTests
{
    /// <summary>
    /// Test fixtures for EffiProz databases.
    /// </summary>
    [TestFixture]
    public partial class ChinookEffiProzFixture
    {
        protected IDictionary<string, EfzConnection> Connections;

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
            using (var adapter = new EfzDataAdapter(query, connection))
            {
                adapter.Fill(dataset);
            }

            return dataset;
        }
        
        /// <summary>
        /// Initialize connections dictionary.
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            Connections = new Dictionary<string, EfzConnection>();
        }

        /// <summary>
        /// Close all connections.
        /// </summary>
        [TestFixtureTearDown]
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
        [Test]
        public void AllInvoicesMustHaveInvoiceLines([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT count(\"InvoiceId\") FROM \"Chinook\".\"Invoice\" WHERE \"InvoiceId\" NOT IN (SELECT \"InvoiceId\" FROM \"Chinook\".\"InvoiceLine\" GROUP BY \"InvoiceId\")");
            Assert.That(dataSet.Tables[0].Rows[0][0], Is.EqualTo(0), "The number of invoices with no invoice lines must be zero.");
        }
        
        /// <summary>
        /// Asserts that invoice total matches sum of invoice lines.
        /// </summary>
        [Test]
        public void InvoiceTotalMustMatchSumOfInvoiceLines([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT \"Chinook\".\"Invoice\".\"InvoiceId\", SUM(\"Chinook\".\"InvoiceLine\".\"UnitPrice\" * \"Chinook\".\"InvoiceLine\".\"Quantity\") AS CalculatedTotal, \"Chinook\".\"Invoice\".\"Total\" AS Total FROM \"Chinook\".\"InvoiceLine\" INNER JOIN \"Chinook\".\"Invoice\" ON \"Chinook\".\"InvoiceLine\".\"InvoiceId\" = \"Chinook\".\"Invoice\".\"InvoiceId\" GROUP BY \"Chinook\".\"Invoice\".\"InvoiceId\", \"Chinook\".\"Invoice\".\"Total\"");

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                Assert.That(row["CalculatedTotal"].ToString(), Is.EqualTo(row["Total"].ToString()), string.Format("The total field of InvoiceId={0} does not match its invoice lines.", row["InvoiceId"]));
            }
        }

        /// <summary>
        /// Verifies that the Genre table was populated properly.
        /// </summary>
        [Test]
        public void GenreTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Genre\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(25), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Genre table has the proper information.
        /// </summary>
        [Test]
        public void GenreLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Genre\" ORDER BY \"GenreId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["GenreId"].ToString(), Is.EqualTo("25"), "GenreId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("Opera"), "Name mismatch.");
        }

        /// <summary>
        /// Verifies that the MediaType table was populated properly.
        /// </summary>
        [Test]
        public void MediaTypeTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"MediaType\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(5), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of MediaType table has the proper information.
        /// </summary>
        [Test]
        public void MediaTypeLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"MediaType\" ORDER BY \"MediaTypeId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["MediaTypeId"].ToString(), Is.EqualTo("5"), "MediaTypeId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("AAC audio file"), "Name mismatch.");
        }

        /// <summary>
        /// Verifies that the Artist table was populated properly.
        /// </summary>
        [Test]
        public void ArtistTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Artist\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(275), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Artist table has the proper information.
        /// </summary>
        [Test]
        public void ArtistLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Artist\" ORDER BY \"ArtistId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["ArtistId"].ToString(), Is.EqualTo("275"), "ArtistId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("Philip Glass Ensemble"), "Name mismatch.");
        }

        /// <summary>
        /// Verifies that the Album table was populated properly.
        /// </summary>
        [Test]
        public void AlbumTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Album\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(347), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Album table has the proper information.
        /// </summary>
        [Test]
        public void AlbumLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Album\" ORDER BY \"AlbumId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["AlbumId"].ToString(), Is.EqualTo("347"), "AlbumId mismatch.");
            Assert.That(row["Title"].ToString(), Is.EqualTo("Koyaanisqatsi (Soundtrack from the Motion Picture)"), "Title mismatch.");
            Assert.That(row["ArtistId"].ToString(), Is.EqualTo("275"), "ArtistId mismatch.");
        }

        /// <summary>
        /// Verifies that the Track table was populated properly.
        /// </summary>
        [Test]
        public void TrackTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Track\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(3503), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Track table has the proper information.
        /// </summary>
        [Test]
        public void TrackLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Track\" ORDER BY \"TrackId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["TrackId"].ToString(), Is.EqualTo("3503"), "TrackId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("Koyaanisqatsi"), "Name mismatch.");
            Assert.That(row["AlbumId"].ToString(), Is.EqualTo("347"), "AlbumId mismatch.");
            Assert.That(row["MediaTypeId"].ToString(), Is.EqualTo("2"), "MediaTypeId mismatch.");
            Assert.That(row["GenreId"].ToString(), Is.EqualTo("10"), "GenreId mismatch.");
            Assert.That(row["Composer"].ToString(), Is.EqualTo("Philip Glass"), "Composer mismatch.");
            Assert.That(row["Milliseconds"].ToString(), Is.EqualTo("206005"), "Milliseconds mismatch.");
            Assert.That(row["Bytes"].ToString(), Is.EqualTo("3305164"), "Bytes mismatch.");
            Assert.That(row["UnitPrice"].ToString(), Is.EqualTo("0.99"), "UnitPrice mismatch.");
        }

        /// <summary>
        /// Verifies that the Employee table was populated properly.
        /// </summary>
        [Test]
        public void EmployeeTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Employee\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(8), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Employee table has the proper information.
        /// </summary>
        [Test]
        public void EmployeeLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Employee\" ORDER BY \"EmployeeId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["EmployeeId"].ToString(), Is.EqualTo("8"), "EmployeeId mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Callahan"), "LastName mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Laura"), "FirstName mismatch.");
            Assert.That(row["Title"].ToString(), Is.EqualTo("IT Staff"), "Title mismatch.");
            Assert.That(row["ReportsTo"].ToString(), Is.EqualTo("6"), "ReportsTo mismatch.");
            Assert.That(row["BirthDate"].ToString(), Is.EqualTo(Convert.ToDateTime("1/9/1968 12:00:00 AM").ToString()), "BirthDate mismatch.");
            Assert.That(row["HireDate"].ToString(), Is.EqualTo(Convert.ToDateTime("3/4/2004 12:00:00 AM").ToString()), "HireDate mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("923 7 ST NW"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Lethbridge"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo("AB"), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Canada"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("T1H 1Y8"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+1 (403) 467-3351"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo("+1 (403) 467-8772"), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("laura@chinookcorp.com"), "Email mismatch.");
        }

        /// <summary>
        /// Verifies that the Customer table was populated properly.
        /// </summary>
        [Test]
        public void CustomerTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(59), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Customer table has the proper information.
        /// </summary>
        [Test]
        public void CustomerLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" ORDER BY \"CustomerId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("59"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Puja"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Srivastava"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("3,Raj Bhavan Road"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Bangalore"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("India"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("560001"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+91 080 22289999"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("puja_srivastava@yahoo.in"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("3"), "SupportRepId mismatch.");
        }

        /// <summary>
        /// Verifies that the Invoice table was populated properly.
        /// </summary>
        [Test]
        public void InvoiceTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Invoice\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(412), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Invoice table has the proper information.
        /// </summary>
        [Test]
        public void InvoiceLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Invoice\" ORDER BY \"InvoiceId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["InvoiceId"].ToString(), Is.EqualTo("412"), "InvoiceId mismatch.");
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("58"), "CustomerId mismatch.");
            Assert.That(row["InvoiceDate"].ToString(), Is.EqualTo(Convert.ToDateTime("12/22/2013 12:00:00 AM").ToString()), "InvoiceDate mismatch.");
            Assert.That(row["BillingAddress"].ToString(), Is.EqualTo("12,Community Centre"), "BillingAddress mismatch.");
            Assert.That(row["BillingCity"].ToString(), Is.EqualTo("Delhi"), "BillingCity mismatch.");
            Assert.That(row["BillingState"].ToString(), Is.EqualTo(""), "BillingState mismatch.");
            Assert.That(row["BillingCountry"].ToString(), Is.EqualTo("India"), "BillingCountry mismatch.");
            Assert.That(row["BillingPostalCode"].ToString(), Is.EqualTo("110017"), "BillingPostalCode mismatch.");
            Assert.That(row["Total"].ToString(), Is.EqualTo("1.99"), "Total mismatch.");
        }

        /// <summary>
        /// Verifies that the InvoiceLine table was populated properly.
        /// </summary>
        [Test]
        public void InvoiceLineTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"InvoiceLine\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(2240), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of InvoiceLine table has the proper information.
        /// </summary>
        [Test]
        public void InvoiceLineLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"InvoiceLine\" ORDER BY \"InvoiceLineId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["InvoiceLineId"].ToString(), Is.EqualTo("2240"), "InvoiceLineId mismatch.");
            Assert.That(row["InvoiceId"].ToString(), Is.EqualTo("412"), "InvoiceId mismatch.");
            Assert.That(row["TrackId"].ToString(), Is.EqualTo("3177"), "TrackId mismatch.");
            Assert.That(row["UnitPrice"].ToString(), Is.EqualTo("1.99"), "UnitPrice mismatch.");
            Assert.That(row["Quantity"].ToString(), Is.EqualTo("1"), "Quantity mismatch.");
        }

        /// <summary>
        /// Verifies that the Playlist table was populated properly.
        /// </summary>
        [Test]
        public void PlaylistTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Playlist\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(18), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Playlist table has the proper information.
        /// </summary>
        [Test]
        public void PlaylistLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Playlist\" ORDER BY \"PlaylistId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["PlaylistId"].ToString(), Is.EqualTo("18"), "PlaylistId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("On-The-Go 1"), "Name mismatch.");
        }

        /// <summary>
        /// Verifies that the PlaylistTrack table was populated properly.
        /// </summary>
        [Test]
        public void PlaylistTrackTableShouldBePopulated([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"PlaylistTrack\"");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(8715), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of PlaylistTrack table has the proper information.
        /// </summary>
        [Test]
        public void PlaylistTrackLastRecordHasProperInfo([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"PlaylistTrack\" ORDER BY \"PlaylistId\", \"TrackId\"");
            var table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            var row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["PlaylistId"].ToString(), Is.EqualTo("18"), "PlaylistId mismatch.");
            Assert.That(row["TrackId"].ToString(), Is.EqualTo("597"), "TrackId mismatch.");
        }

        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId01HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 1");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("1"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Luís"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Gonçalves"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo("Embraer - Empresa Brasileira de Aeronáutica S.A."), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Av. Brigadeiro Faria Lima, 2170"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("São José dos Campos"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo("SP"), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Brazil"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("12227-000"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+55 (12) 3923-5555"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo("+55 (12) 3923-5566"), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("luisg@embraer.com.br"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("3"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId02HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 2");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("2"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Leonie"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Köhler"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Theodor-Heuss-Straße 34"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Stuttgart"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Germany"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("70174"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+49 0711 2842222"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("leonekohler@surfeu.de"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("5"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId03HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 3");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("3"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("François"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Tremblay"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("1498 rue Bélanger"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Montréal"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo("QC"), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Canada"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("H2G 1A7"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+1 (514) 721-4711"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("ftremblay@gmail.com"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("3"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId04HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 4");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("4"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Bjørn"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Hansen"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Ullevålsveien 14"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Oslo"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Norway"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("0171"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+47 22 44 22 22"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("bjorn.hansen@yahoo.no"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("4"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId05HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 5");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("5"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("František"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Wichterlová"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo("JetBrains s.r.o."), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Klanova 9/506"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Prague"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Czech Republic"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("14700"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+420 2 4172 5555"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo("+420 2 4172 5555"), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("frantisekw@jetbrains.com"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("4"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId06HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 6");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("6"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Helena"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Holý"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Rilská 3174/6"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Prague"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Czech Republic"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("14300"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+420 2 4177 0449"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("hholy@gmail.com"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("5"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId07HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 7");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("7"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Astrid"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Gruber"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Rotenturmstraße 4, 1010 Innere Stadt"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Vienne"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Austria"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("1010"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+43 01 5134505"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("astrid.gruber@apple.at"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("5"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId08HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 8");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("8"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Daan"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Peeters"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Grétrystraat 63"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Brussels"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Belgium"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("1000"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+32 02 219 03 03"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("daan_peeters@apple.be"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("4"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId09HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 9");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("9"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Kara"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Nielsen"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Sønder Boulevard 51"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Copenhagen"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Denmark"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("1720"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+453 3331 9991"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("kara.nielsen@jubii.dk"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("4"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId10HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 10");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("10"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Eduardo"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Martins"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo("Woodstock Discos"), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Rua Dr. Falcão Filho, 155"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("São Paulo"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo("SP"), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Brazil"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("01007-010"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+55 (11) 3033-5446"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo("+55 (11) 3033-4564"), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("eduardo@woodstock.com.br"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("4"), "SupportRepId mismatch.");
		}
		
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void CustomerId11HasProperUnicodeCharacters([Values("Chinook_EffiProz", "Chinook_EffiProz_AutoIncrement")] string connectionName)
        {
            var dataSet = ExecuteQuery(connectionName, "SELECT * FROM \"Chinook\".\"Customer\" WHERE \"CustomerId\" = 11");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            var row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("11"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Alexandre"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Rocha"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo("Banco do Brasil S.A."), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Av. Paulista, 2022"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("São Paulo"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo("SP"), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Brazil"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("01310-200"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+55 (11) 3055-3278"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo("+55 (11) 3055-8131"), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("alero@uol.com.br"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("5"), "SupportRepId mismatch.");
		}
		
    }
	
}
