using System.Data.Entity.Infrastructure;
using ChinookDatabase.DataModel;
using Xunit;

namespace ChinookDatabase.Test.UnitTests
{
    public class ChinookModelTest
    {
        [Fact]
        public void TestChinookModelCreation()
        {
            using (var entities = new ChinookEntities())
            {
                var provider = new DbProviderInfo("System.Data.SqlClient", "2008");
                var model = ChinookModel.CreateModel(provider);
                Assert.NotNull(model);
            }
        }
    }
}
