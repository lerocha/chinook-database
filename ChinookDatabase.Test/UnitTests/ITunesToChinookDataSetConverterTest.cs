using System.IO;
using ChinookDatabase.Utilities;
using Xunit;

namespace ChinookDatabase.Test.UnitTests
{
    public class ITunesToChinookDataSetConverterTest
    {
        private const string TestData = @"TestData\iTunesLibraryTestData.xml";

        [Fact]
        public void TestConversion()
        {
            var testFile = new FileInfo(TestData);
            Assert.True(File.Exists(testFile.FullName));

            var ignorePlaylists = new[] { "Audiobooks", "Genius" };

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, null, ignorePlaylists);
            var ds = builder.BuildDataSet();

            // Convert dataset into XML text and saves to a file.
            var fs = new FileStream(testFile.DirectoryName + @"\ResultDataSet.xml" , FileMode.Create);
            ds.WriteXml(fs);
            fs.Position = 0;
            var reader = new StreamReader(fs);
            string actual = reader.ReadToEnd();
            fs.Close();

            // Reads the expected XML text.
            string expected = File.ReadAllText(testFile.DirectoryName + @"\ExpectedDataSet.xml");

            Assert.Equal(expected, actual);
        }

		[Fact]
        public void TestConversionWithPlaylistsToIgnore()
        {
            var testFile = new FileInfo(TestData);
			Assert.True(File.Exists(testFile.FullName));

            var ignorePlaylists = new[] { "Audiobooks", "Genius" };

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, null, ignorePlaylists);
            var ds = builder.BuildDataSet();

            // Assert that playlists were ignored.
            foreach (var playlist in ds.Playlist)
            {
                foreach (var name in ignorePlaylists)
                {
                    Assert.NotEqual(name, playlist.Name);
                }
            }
        }

		[Fact]
        public void TestConversionWithNonMediaData()
        {
            var testFile = new FileInfo(TestData);
            Assert.True(File.Exists(testFile.FullName));

            string xmlNonMediaDataFilename = testFile.DirectoryName + @"\NonMediaTestData.xml";
			Assert.True(File.Exists(xmlNonMediaDataFilename));

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, xmlNonMediaDataFilename);
            var ds = builder.BuildDataSet();

            Assert.True(ds.Customer.Count > 0);
            Assert.True(ds.Employee.Count > 0);
        }

		[Fact]
        public void TestConversionWithNonMediaDataAndInvalidITunesLibrary()
        {
            var testFile = new FileInfo(@"TestData\invalidFile.xml");
            Assert.False(File.Exists(testFile.FullName));

            string xmlNonMediaDataFilename = testFile.DirectoryName + @"\NonMediaTestData.xml";
			Assert.True(File.Exists(xmlNonMediaDataFilename));

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, xmlNonMediaDataFilename);
            var ds = builder.BuildDataSet();

            Assert.True(ds.Customer.Count > 0);
            Assert.True(ds.Employee.Count > 0);
			Assert.Equal(0, ds.Album.Count);
			Assert.Equal(0, ds.Artist.Count);
			Assert.Equal(0, ds.Genre.Count);
			Assert.Equal(0, ds.Invoice.Count);
			Assert.Equal(0, ds.InvoiceLine.Count);
			Assert.Equal(0, ds.MediaType.Count);
			Assert.Equal(0, ds.Playlist.Count);
			Assert.Equal(0, ds.PlaylistTrack.Count);
			Assert.Equal(0, ds.Track.Count);
        }

		[Fact]
        public void TestConversionWithNullITunesLibrary()
        {
            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(null);
            var ds = builder.BuildDataSet();

			Assert.Equal(0, ds.Album.Count);
			Assert.Equal(0, ds.Artist.Count);
			Assert.Equal(0, ds.Genre.Count);
			Assert.Equal(0, ds.Invoice.Count);
			Assert.Equal(0, ds.InvoiceLine.Count);
			Assert.Equal(0, ds.MediaType.Count);
			Assert.Equal(0, ds.Playlist.Count);
			Assert.Equal(0, ds.PlaylistTrack.Count);
			Assert.Equal(0, ds.Track.Count);
        }

		[Fact]
        public void TestConversionWithNullNonMediaDataFile()
        {
            var testFile = new FileInfo(TestData);
			Assert.True(File.Exists(testFile.FullName));

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, null);
            var ds = builder.BuildDataSet();

			Assert.Equal(0, ds.Customer.Count);
			Assert.Equal(0, ds.Employee.Count);
        }

		[Fact]
        public void TestConversionWithInvalidNonMediaDataFile()
        {
            var testFile = new FileInfo(TestData);
            Assert.True(File.Exists(testFile.FullName));

            string xmlNonMediaDataFilename = testFile.DirectoryName + @"\NonExistingFile.xml";
            Assert.False(File.Exists(xmlNonMediaDataFilename));

            // Import data from iTunes library.
            var builder = new ITunesToChinookDataSetConverter(testFile.FullName, xmlNonMediaDataFilename);
            var ds = builder.BuildDataSet();

			Assert.Equal(0, ds.Customer.Count);
			Assert.Equal(0, ds.Employee.Count);
        }
    }
}
