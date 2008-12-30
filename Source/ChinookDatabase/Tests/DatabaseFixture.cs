
/*******************************************************************************
 * Chinook Database
 * Description: Test fixture for Chinook database.
 * DB Server: SQL Server
 * Version: 1.1
 * License: http://www.codeplex.com/ChinookDatabase/license
 * 
 * IMPORTANT: In order to run these test fixtures, you will need to:
 *            1. Run the generated SQL script to create the database to be tested.
 *            2. Verify that app.config has the proper connection string (user/password).
 ********************************************************************************/
using System;
using System.Data;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
namespace ChinookDatabase.Tests
{
    /// <summary>
    /// Base class for Chinook database test fixture.
    /// </summary>
    public abstract class DatabaseFixture
    {
        /// <summary>
        /// Method to execute a SQL query and return a dataset.
        /// </summary>
        /// <param name="query">Query string to be executed.</param>
        /// <returns>DataSet with the query results.</returns>
        protected abstract DataSet ExecuteQuery(string query);
        
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void RecordsWithProperUnicodeCharacters()
        {
            // The first customer record is used to test for Unicode problems.
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 1");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Artist table or the SQL script did not use Unicode characters properly.");
            
            DataRow row = dataSet.Tables[0].Rows[0];
            
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
        /// Verifies that the Genre table was populated properly.
        /// </summary>
        [Test]
        public void GenreTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Genre");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(27), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Genre table has the proper information.
        /// </summary>
        [Test]
        public void GenreLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Genre ORDER BY GenreId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["GenreId"].ToString(), Is.EqualTo("27"), "GenreId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("Opera"), "Name mismatch.");
        }

        /// <summary>
        /// Verifies that the MediaType table was populated properly.
        /// </summary>
        [Test]
        public void MediaTypeTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM MediaType");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(5), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of MediaType table has the proper information.
        /// </summary>
        [Test]
        public void MediaTypeLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM MediaType ORDER BY MediaTypeId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["MediaTypeId"].ToString(), Is.EqualTo("5"), "MediaTypeId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("AAC audio file"), "Name mismatch.");
        }

        /// <summary>
        /// Verifies that the Artist table was populated properly.
        /// </summary>
        [Test]
        public void ArtistTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Artist");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(275), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Artist table has the proper information.
        /// </summary>
        [Test]
        public void ArtistLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Artist ORDER BY ArtistId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["ArtistId"].ToString(), Is.EqualTo("275"), "ArtistId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("Philip Glass Ensemble"), "Name mismatch.");
        }

        /// <summary>
        /// Verifies that the Album table was populated properly.
        /// </summary>
        [Test]
        public void AlbumTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Album");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(347), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Album table has the proper information.
        /// </summary>
        [Test]
        public void AlbumLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Album ORDER BY AlbumId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
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
        public void TrackTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Track");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(3503), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Track table has the proper information.
        /// </summary>
        [Test]
        public void TrackLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Track ORDER BY TrackId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["TrackId"].ToString(), Is.EqualTo("3503"), "TrackId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("Koyaanisqatsi"), "Name mismatch.");
            Assert.That(row["AlbumId"].ToString(), Is.EqualTo("347"), "AlbumId mismatch.");
            Assert.That(row["MediaTypeId"].ToString(), Is.EqualTo("2"), "MediaTypeId mismatch.");
            Assert.That(row["GenreId"].ToString(), Is.EqualTo("11"), "GenreId mismatch.");
            Assert.That(row["Composer"].ToString(), Is.EqualTo("Philip Glass"), "Composer mismatch.");
            Assert.That(row["Milliseconds"].ToString(), Is.EqualTo("206005"), "Milliseconds mismatch.");
            Assert.That(row["Bytes"].ToString(), Is.EqualTo("3305164"), "Bytes mismatch.");
            Assert.That(row["UnitPrice"].ToString(), Is.EqualTo("0.99"), "UnitPrice mismatch.");
        }

        /// <summary>
        /// Verifies that the Employee table was populated properly.
        /// </summary>
        [Test]
        public void EmployeeTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Employee");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(8), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Employee table has the proper information.
        /// </summary>
        [Test]
        public void EmployeeLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Employee ORDER BY EmployeeId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
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
        public void CustomerTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(10), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Customer table has the proper information.
        /// </summary>
        [Test]
        public void CustomerLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer ORDER BY CustomerId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("10"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Michelle"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Brooks"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("627 Broadway"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("New York"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo("NY"), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("USA"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("10012-2612"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+1 (212) 221-3546"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo("+1 (212) 221-4679"), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("michelleb@aol.com"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("3"), "SupportRepId mismatch.");
        }

        /// <summary>
        /// Verifies that the Invoice table was populated properly.
        /// </summary>
        [Test]
        public void InvoiceTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Invoice");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(546), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Invoice table has the proper information.
        /// </summary>
        [Test]
        public void InvoiceLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Invoice ORDER BY InvoiceId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["InvoiceId"].ToString(), Is.EqualTo("546"), "InvoiceId mismatch.");
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("5"), "CustomerId mismatch.");
            Assert.That(row["InvoiceDate"].ToString(), Is.EqualTo(Convert.ToDateTime("12/27/2009 12:00:00 AM").ToString()), "InvoiceDate mismatch.");
            Assert.That(row["BillingAddress"].ToString(), Is.EqualTo("Qe 7 Bloco G"), "BillingAddress mismatch.");
            Assert.That(row["BillingCity"].ToString(), Is.EqualTo("Brasília"), "BillingCity mismatch.");
            Assert.That(row["BillingState"].ToString(), Is.EqualTo("DF"), "BillingState mismatch.");
            Assert.That(row["BillingCountry"].ToString(), Is.EqualTo("Brazil"), "BillingCountry mismatch.");
            Assert.That(row["BillingPostalCode"].ToString(), Is.EqualTo("71020-677"), "BillingPostalCode mismatch.");
        }

        /// <summary>
        /// Verifies that the InvoiceLine table was populated properly.
        /// </summary>
        [Test]
        public void InvoiceLineTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM InvoiceLine");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1447), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of InvoiceLine table has the proper information.
        /// </summary>
        [Test]
        public void InvoiceLineLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM InvoiceLine ORDER BY InvoiceLineId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["InvoiceLineId"].ToString(), Is.EqualTo("1447"), "InvoiceLineId mismatch.");
            Assert.That(row["InvoiceId"].ToString(), Is.EqualTo("546"), "InvoiceId mismatch.");
            Assert.That(row["TrackId"].ToString(), Is.EqualTo("507"), "TrackId mismatch.");
            Assert.That(row["UnitPrice"].ToString(), Is.EqualTo("0.99"), "UnitPrice mismatch.");
            Assert.That(row["Quantity"].ToString(), Is.EqualTo("1"), "Quantity mismatch.");
        }

        /// <summary>
        /// Verifies that the Playlist table was populated properly.
        /// </summary>
        [Test]
        public void PlaylistTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Playlist");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(4), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of Playlist table has the proper information.
        /// </summary>
        [Test]
        public void PlaylistLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Playlist ORDER BY PlaylistId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["PlaylistId"].ToString(), Is.EqualTo("4"), "PlaylistId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("On-The-Go 1"), "Name mismatch.");
        }

        /// <summary>
        /// Verifies that the PlaylistTrack table was populated properly.
        /// </summary>
        [Test]
        public void PlaylistTrackTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM PlaylistTrack");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(76), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that last record of PlaylistTrack table has the proper information.
        /// </summary>
        [Test]
        public void PlaylistTrackLastRecordHasProperInfo()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM PlaylistTrack ORDER BY PlaylistId, TrackId");
            DataTable table = dataSet.Tables[0];
            Assert.IsNotNull(table);
            DataRow row = table.Rows[table.Rows.Count - 1];
            Assert.IsNotNull(row);

			// Assert that the last record has the proper information.            
            Assert.That(row["PlaylistId"].ToString(), Is.EqualTo("4"), "PlaylistId mismatch.");
            Assert.That(row["TrackId"].ToString(), Is.EqualTo("597"), "TrackId mismatch.");
        }
    }
}
