using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Configuration;

namespace ChinookDatabase.Tests
{
    [TestFixture]
    public class T4TemplatesFixture
    {
        [Test]
        public void ReadiTunesLibraryFixture()
        {
            FileInfo thisFile = new FileInfo(@"C:\Code\ChinookDatabase\Trunk\Source\ChinookDatabase\Tests\T4TemplatesFixture.cs");
            string filename = thisFile.DirectoryName + @"\..\SampleData\iTunes Music Library.xml";
            Assert.That(File.Exists(filename));

            // This test is just a place holder to paste T4 template code (DataSetHelper.tt) in order to debug it and test it.
        }

    }
}
