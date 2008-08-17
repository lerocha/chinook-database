
/*******************************************************************************
 * Chinook Database
 * Description: Test fixture for Chinook database.
 * DB Server: SQL Server
 * Version: 1.0
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
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(7), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that the Artist table was populated properly.
        /// </summary>
        [Test]
        public void ArtistTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Artist");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(241), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that the Album table was populated properly.
        /// </summary>
        [Test]
        public void AlbumTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Album");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(308), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that the Track table was populated properly.
        /// </summary>
        [Test]
        public void TrackTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Track");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(3433), "Total number of records mismatch.");
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
        /// Verifies that the Employee table was populated properly.
        /// </summary>
        [Test]
        public void EmployeeTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Employee");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(8), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that the Invoice table was populated properly.
        /// </summary>
        [Test]
        public void InvoiceTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Invoice");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(457), "Total number of records mismatch.");
        }

        /// <summary>
        /// Verifies that the InvoiceLine table was populated properly.
        /// </summary>
        [Test]
        public void InvoiceLineTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM InvoiceLine");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1104), "Total number of records mismatch.");
        }
    }
}
