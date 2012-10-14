using System.Data.Entity.Infrastructure;
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
            using (var entities = new ChinookEntities())
            {
                var provider = new DbProviderInfo("System.Data.SqlClient", "2008");
                var model = ChinookModel.CreateModel(provider);
                Assert.IsNotNull(model);
            }
        }
    }
}
