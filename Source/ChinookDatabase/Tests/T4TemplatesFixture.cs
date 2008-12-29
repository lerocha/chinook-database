using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.XPath;
using ChinookMetadata.Convert;
using ChinookMetadata.Schema;
using NUnit.Framework;

namespace ChinookDatabase.Tests
{
    [TestFixture]
    public class T4TemplatesFixture
    {
        [Test]
        public void ReadiTunesLibraryFixture()
        {
            var thisFile = new FileInfo(@"C:\Code\ChinookDatabase\Trunk\Source\ChinookDatabase\Tests\T4TemplatesFixture.cs");
            string filename = thisFile.DirectoryName + @"\..\SampleData\iTunes Music Library.xml";
            Assert.That(File.Exists(filename));

            string xmlDataFilename = thisFile.DirectoryName + @"\..\SampleData\ManualData.xml";

            ChinookDataSet ds = new ChinookDataSet();


        }

    }
}
