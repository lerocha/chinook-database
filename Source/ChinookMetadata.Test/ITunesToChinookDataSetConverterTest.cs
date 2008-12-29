using System.IO;
using ChinookMetadata.Convert;
using NUnit.Framework;

namespace ChinookMetadata.Test
{
    [TestFixture]
    public class ITunesToChinookDataSetConverterTest
    {
        [Test]
        public void TestConversion()
        {
            var testFile = new FileInfo(@"TestData\iTunesLibraryTestData.xml");
            Assert.That(File.Exists(testFile.FullName));

            var ignorePlaylists = new[] { "Audiobooks", "Genius" };

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, null, ignorePlaylists);
            var ds = builder.BuildDataSet();

            // Convert dataset into XML text.
            var memstream = new MemoryStream();
            ds.WriteXml(memstream);
            memstream.Position = 0;
            var reader = new StreamReader(memstream);
            string actual = reader.ReadToEnd();

            // Reads the expected XML text.
            string expected = File.ReadAllText(testFile.DirectoryName + @"\ExpectedDataSet.xml");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestConversionWithPlaylistsToIgnore()
        {
            var testFile = new FileInfo(@"TestData\iTunesLibraryTestData.xml");
            Assert.That(File.Exists(testFile.FullName));

            var ignorePlaylists = new[] { "Audiobooks", "Genius" };

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, null, ignorePlaylists);
            var ds = builder.BuildDataSet();

            // Assert that playlists were ignored.
            foreach (var playlist in ds.Playlist)
            {
                foreach (var name in ignorePlaylists)
                {
                    Assert.AreNotEqual(name, playlist.Name);
                }
            }
        }

        [Test]
        public void TestConversionWithNonMediaData()
        {
            var testFile = new FileInfo(@"TestData\iTunesLibraryTestData.xml");
            Assert.That(File.Exists(testFile.FullName));

            string xmlNonMediaDataFilename = testFile.DirectoryName + @"\NonMediaTestData.xml";
            Assert.That(File.Exists(xmlNonMediaDataFilename));

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, xmlNonMediaDataFilename);
            var ds = builder.BuildDataSet();

            Assert.Greater(ds.Customer.Count, 0);
            Assert.Greater(ds.Employee.Count, 0);
        }

        [Test]
        public void TestConversionWithNonMediaDataAndInvalidITunesLibrary()
        {
            var testFile = new FileInfo(@"TestData\invalidFile.xml");
            Assert.That(!File.Exists(testFile.FullName));

            string xmlNonMediaDataFilename = testFile.DirectoryName + @"\NonMediaTestData.xml";
            Assert.That(File.Exists(xmlNonMediaDataFilename));

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, xmlNonMediaDataFilename);
            var ds = builder.BuildDataSet();

            Assert.Greater(ds.Customer.Count, 0);
            Assert.Greater(ds.Employee.Count, 0);
            Assert.AreEqual(0, ds.Album.Count);
            Assert.AreEqual(0, ds.Artist.Count);
            Assert.AreEqual(0, ds.Genre.Count);
            Assert.AreEqual(0, ds.Invoice.Count);
            Assert.AreEqual(0, ds.InvoiceLine.Count);
            Assert.AreEqual(0, ds.MediaType.Count);
            Assert.AreEqual(0, ds.Playlist.Count);
            Assert.AreEqual(0, ds.PlaylistTrack.Count);
            Assert.AreEqual(0, ds.Track.Count);
        }

        [Test]
        public void TestConversionWithNullITunesLibrary()
        {
            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(null);
            var ds = builder.BuildDataSet();

            Assert.AreEqual(0, ds.Album.Count);
            Assert.AreEqual(0, ds.Artist.Count);
            Assert.AreEqual(0, ds.Genre.Count);
            Assert.AreEqual(0, ds.Invoice.Count);
            Assert.AreEqual(0, ds.InvoiceLine.Count);
            Assert.AreEqual(0, ds.MediaType.Count);
            Assert.AreEqual(0, ds.Playlist.Count);
            Assert.AreEqual(0, ds.PlaylistTrack.Count);
            Assert.AreEqual(0, ds.Track.Count);
        }

        [Test]
        public void TestConversionWithNullNonMediaDataFile()
        {
            var testFile = new FileInfo(@"TestData\iTunesLibraryTestData.xml");
            Assert.That(File.Exists(testFile.FullName));

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, null);
            var ds = builder.BuildDataSet();

            Assert.AreEqual(0, ds.Customer.Count);
            Assert.AreEqual(0, ds.Employee.Count);
        }

        [Test]
        public void TestConversionWithInvalidNonMediaDataFile()
        {
            var testFile = new FileInfo(@"TestData\iTunesLibraryTestData.xml");
            Assert.That(File.Exists(testFile.FullName));

            string xmlNonMediaDataFilename = testFile.DirectoryName + @"\NonExistingFile.xml";
            Assert.That(!File.Exists(xmlNonMediaDataFilename));

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, xmlNonMediaDataFilename);
            var ds = builder.BuildDataSet();

            Assert.AreEqual(0, ds.Customer.Count);
            Assert.AreEqual(0, ds.Employee.Count);
        }
    }
}
