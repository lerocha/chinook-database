
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
        
        #region Public Tests
        /// <summary>
        /// Verifies that the Unicode characters are populated properly.
        /// </summary>
        [Test]
        public void RecordsWithProperUnicodeCharacters()
        {
			AssertThatCustomerId1HasProperUnicodeCharacters();
			AssertThatCustomerId2HasProperUnicodeCharacters();
			AssertThatCustomerId3HasProperUnicodeCharacters();
			AssertThatCustomerId4HasProperUnicodeCharacters();
			AssertThatCustomerId5HasProperUnicodeCharacters();
			AssertThatCustomerId6HasProperUnicodeCharacters();
			AssertThatCustomerId7HasProperUnicodeCharacters();
			AssertThatCustomerId8HasProperUnicodeCharacters();
			AssertThatCustomerId9HasProperUnicodeCharacters();
			AssertThatCustomerId10HasProperUnicodeCharacters();
			AssertThatCustomerId11HasProperUnicodeCharacters();
        }

        /// <summary>
        /// Asserts that all invoices contain invoice lines.
        /// </summary>
        [Test]
        public void AllInvoicesMustHaveInvoiceLines()
        {
            DataSet dataSet = ExecuteQuery("SELECT count(InvoiceId) FROM Invoice WHERE InvoiceId NOT IN (SELECT InvoiceId FROM InvoiceLine GROUP BY InvoiceId)");
            Assert.That(dataSet.Tables[0].Rows[0][0], Is.EqualTo(0), "The number of invoices with no invoice lines must be zero.");
        }
        
        /// <summary>
        /// Asserts that invoice total matches sum of invoice lines.
        /// </summary>
        [Test]
        public void InvoiceTotalMustMatchSumOfInvoiceLines()
        {
            DataSet dataSet = ExecuteQuery("SELECT Invoice.InvoiceId, SUM(InvoiceLine.UnitPrice * InvoiceLine.Quantity) AS CalculatedTotal, Invoice.Total AS Total FROM InvoiceLine INNER JOIN Invoice ON InvoiceLine.InvoiceId = Invoice.InvoiceId GROUP BY Invoice.InvoiceId, Invoice.Total");

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                Assert.That(row["CalculatedTotal"].ToString(), Is.EqualTo(row["Total"].ToString()), string.Format("The total field of InvoiceId={0} does not match its invoice lines.", row["InvoiceId"]));
            }
        }

        /// <summary>
        /// Verifies that the Genre table was populated properly.
        /// </summary>
        [Test]
        public void GenreTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Genre");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(25), "Total number of records mismatch.");
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
            Assert.That(row["GenreId"].ToString(), Is.EqualTo("25"), "GenreId mismatch.");
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
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(62), "Total number of records mismatch.");
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
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("62"), "CustomerId mismatch.");
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
        public void InvoiceTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Invoice");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(520), "Total number of records mismatch.");
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
            Assert.That(row["InvoiceId"].ToString(), Is.EqualTo("520"), "InvoiceId mismatch.");
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("22"), "CustomerId mismatch.");
            Assert.That(row["InvoiceDate"].ToString(), Is.EqualTo(Convert.ToDateTime("12/26/2010 12:00:00 AM").ToString()), "InvoiceDate mismatch.");
            Assert.That(row["BillingAddress"].ToString(), Is.EqualTo("801 W 4th Street"), "BillingAddress mismatch.");
            Assert.That(row["BillingCity"].ToString(), Is.EqualTo("Reno"), "BillingCity mismatch.");
            Assert.That(row["BillingState"].ToString(), Is.EqualTo("NV"), "BillingState mismatch.");
            Assert.That(row["BillingCountry"].ToString(), Is.EqualTo("USA"), "BillingCountry mismatch.");
            Assert.That(row["BillingPostalCode"].ToString(), Is.EqualTo("89503"), "BillingPostalCode mismatch.");
            Assert.That(row["Total"].ToString(), Is.EqualTo("10.91"), "Total mismatch.");
        }

        /// <summary>
        /// Verifies that the InvoiceLine table was populated properly.
        /// </summary>
        [Test]
        public void InvoiceLineTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM InvoiceLine");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(3075), "Total number of records mismatch.");
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
            Assert.That(row["InvoiceLineId"].ToString(), Is.EqualTo("3075"), "InvoiceLineId mismatch.");
            Assert.That(row["InvoiceId"].ToString(), Is.EqualTo("520"), "InvoiceId mismatch.");
            Assert.That(row["TrackId"].ToString(), Is.EqualTo("976"), "TrackId mismatch.");
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
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(18), "Total number of records mismatch.");
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
            Assert.That(row["PlaylistId"].ToString(), Is.EqualTo("18"), "PlaylistId mismatch.");
            Assert.That(row["Name"].ToString(), Is.EqualTo("On-The-Go 1"), "Name mismatch.");
        }

        /// <summary>
        /// Verifies that the PlaylistTrack table was populated properly.
        /// </summary>
        [Test]
        public void PlaylistTrackTableShouldBePopulated()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM PlaylistTrack");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(8715), "Total number of records mismatch.");
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
            Assert.That(row["PlaylistId"].ToString(), Is.EqualTo("18"), "PlaylistId mismatch.");
            Assert.That(row["TrackId"].ToString(), Is.EqualTo("597"), "TrackId mismatch.");
        }
		#endregion

		#region Private Methods
        /// <summary>
        /// Verifies that CustomerId 1 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId1HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 1");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
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
        /// Verifies that CustomerId 2 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId2HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 2");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("2"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Sergey"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Kuznetsov"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("ул. Усиевича, 22"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Moscow"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Russia"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("125315"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+7 8 (495) 787-68-68"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("sergey.kuznetsov@goldnet.ru"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("5"), "SupportRepId mismatch.");
		}

        /// <summary>
        /// Verifies that CustomerId 3 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId3HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 3");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("3"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Ioannis"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Papadopoulos"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("1 Κουμπαρη"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Athens"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Greece"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("10674"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+30 210-3935500"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("ioannis_papadopoulos@yahoo.gr"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("4"), "SupportRepId mismatch.");
		}

        /// <summary>
        /// Verifies that CustomerId 4 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId4HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 4");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("4"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Dmitri"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Petrov"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Миллионная ул., 23"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Saint Petersburg"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Russia"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("191186"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+7 8 (812) 315-87-87"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("dmitri.petrov@yahoo.ru"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("4"), "SupportRepId mismatch.");
		}

        /// <summary>
        /// Verifies that CustomerId 5 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId5HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 5");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("5"), "CustomerId mismatch.");
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
        /// Verifies that CustomerId 6 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId6HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 6");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("6"), "CustomerId mismatch.");
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
        /// Verifies that CustomerId 7 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId7HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 7");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("7"), "CustomerId mismatch.");
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
        /// Verifies that CustomerId 8 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId8HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 8");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("8"), "CustomerId mismatch.");
            Assert.That(row["FirstName"].ToString(), Is.EqualTo("Astrid"), "FirstName mismatch.");
            Assert.That(row["LastName"].ToString(), Is.EqualTo("Gruber"), "LastName mismatch.");
            Assert.That(row["Company"].ToString(), Is.EqualTo(""), "Company mismatch.");
            Assert.That(row["Address"].ToString(), Is.EqualTo("Rotenturmstraße 4, 1010 Innere Stadt"), "Address mismatch.");
            Assert.That(row["City"].ToString(), Is.EqualTo("Vienne"), "City mismatch.");
            Assert.That(row["State"].ToString(), Is.EqualTo(""), "State mismatch.");
            Assert.That(row["Country"].ToString(), Is.EqualTo("Austria"), "Country mismatch.");
            Assert.That(row["PostalCode"].ToString(), Is.EqualTo("1010"), "PostalCode mismatch.");
            Assert.That(row["Phone"].ToString(), Is.EqualTo("+43 01 5134505‎"), "Phone mismatch.");
            Assert.That(row["Fax"].ToString(), Is.EqualTo(""), "Fax mismatch.");
            Assert.That(row["Email"].ToString(), Is.EqualTo("astrid.gruber@apple.at"), "Email mismatch.");
            Assert.That(row["SupportRepId"].ToString(), Is.EqualTo("5"), "SupportRepId mismatch.");
		}

        /// <summary>
        /// Verifies that CustomerId 9 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId9HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 9");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("9"), "CustomerId mismatch.");
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
        /// Verifies that CustomerId 10 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId10HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 10");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("10"), "CustomerId mismatch.");
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
        /// Verifies that CustomerId 11 has expected Unicode characters.
        /// </summary>
        private void AssertThatCustomerId11HasProperUnicodeCharacters()
        {
            DataSet dataSet = ExecuteQuery("SELECT * FROM Customer WHERE CustomerId = 11");
            Assert.That(dataSet.Tables[0].Rows.Count, Is.EqualTo(1), "Cannot find the Customer record that contains unicode characters. This record was not added to the Customer table or the SQL script did not use Unicode characters properly.");
            DataRow row = dataSet.Tables[0].Rows[0];
            
            Assert.That(row["CustomerId"].ToString(), Is.EqualTo("11"), "CustomerId mismatch.");
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

		#endregion
    }
}
