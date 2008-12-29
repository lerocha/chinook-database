
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
            
            Assert.That(dataSet.Tables[0].Rows[0]["CustomerId"].ToString(), Is.EqualTo("1"), "CustomerId mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["FirstName"].ToString(), Is.EqualTo("Luís"), "FirstName mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["LastName"].ToString(), Is.EqualTo("Gonçalves"), "LastName mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["Company"].ToString(), Is.EqualTo("Embraer - Empresa Brasileira de Aeronáutica S.A."), "Company mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["Address"].ToString(), Is.EqualTo("Av. Brigadeiro Faria Lima, 2170"), "Address mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["City"].ToString(), Is.EqualTo("São José dos Campos"), "City mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["State"].ToString(), Is.EqualTo("SP"), "State mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["Country"].ToString(), Is.EqualTo("Brazil"), "Country mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["PostalCode"].ToString(), Is.EqualTo("12227-000"), "PostalCode mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["Phone"].ToString(), Is.EqualTo("+55 (12) 3923-5555"), "Phone mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["Fax"].ToString(), Is.EqualTo("+55 (12) 3923-5566"), "Fax mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["Email"].ToString(), Is.EqualTo("luisg@embraer.com.br"), "Email mismatch.");
            Assert.That(dataSet.Tables[0].Rows[0]["SupportRepId"].ToString(), Is.EqualTo("3"), "SupportRepId mismatch.");
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
        /// Verifies that the MediaType table was populated properly.
        /// </summary>
        [Test]
        public void MediaTypeTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM MediaType");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(5), "Total number of records mismatch.");
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
        /// Verifies that the Album table was populated properly.
        /// </summary>
        [Test]
        public void AlbumTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Album");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(347), "Total number of records mismatch.");
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
        /// Verifies that the Employee table was populated properly.
        /// </summary>
        [Test]
        public void EmployeeTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Employee");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(8), "Total number of records mismatch.");
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
        /// Verifies that the Invoice table was populated properly.
        /// </summary>
        [Test]
        public void InvoiceTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Invoice");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(546), "Total number of records mismatch.");
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
        /// Verifies that the Playlist table was populated properly.
        /// </summary>
        [Test]
        public void PlaylistTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Playlist");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(4), "Total number of records mismatch.");
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
    }
}
