using System.IO;
using ChinookDatabase.Utilities;
using Xunit;

namespace ChinookDatabase.Test.UnitTests
{
    public class EdmHelperTest
    {
        [Fact]
        public void GetSsdlFromEdmxTest()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\ChinookDatabase\Chinook.edmx");
            var ssdl = EdmHelper.GetSsdlFromEdmx(path);
            Assert.NotNull(ssdl);
        }
    }
}
