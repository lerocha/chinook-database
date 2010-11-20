using ChinookDatabase.DataModel;
using NUnit.Framework;

namespace ChinookDatabase.Test.UnitTests
{
    [TestFixture]
    public class ChinookModelTest
    {
        [Test]
        public void TestChinookModelCreation()
        {
            var model = ChinookModel.CreateModel();
            Assert.IsNotNull(model);
        }
    }
}
