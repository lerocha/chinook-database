using ChinookDatabase.DataModel;
using NUnit.Framework;

namespace ChinookDatabase.Test.UnitTests
{
    [TestFixture]
    public class ChinookModelTest
    {
        [Test]
        [Ignore]
        public void TestChinookModelCreation()
        {
            using (var entities = new ChinookEntities())
            {
                var model = ChinookModel.CreateModel(entities.Connection);
                Assert.IsNotNull(model);
            }
        }
    }
}
