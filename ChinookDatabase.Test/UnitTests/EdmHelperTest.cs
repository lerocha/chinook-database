using System.IO;
using ChinookDatabase.Utilities;
using NUnit.Framework;

namespace ChinookDatabase.Test.UnitTests
{
    [TestFixture]
    class EdmHelperTest
    {
        [Test]
        public void GetSsdlFromEdmxTest()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\ChinookDatabase\Chinook.edmx");
            var ssdl = EdmHelper.GetSsdlFromEdmx(path);
            Assert.NotNull(ssdl);
        }
    }
}
