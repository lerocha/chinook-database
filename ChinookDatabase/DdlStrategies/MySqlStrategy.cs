using System.Data.Metadata.Edm;
using Microsoft.Data.Entity.Design.DatabaseGeneration;

namespace ChinookDatabase.DdlStrategies
{
    public class MySqlStrategy : AbstractDdlStrategy
    {
        public MySqlStrategy()
        {
            Name = "MySql"; 
            Identity = "AUTO_INCREMENT";
            IsReCreateDatabaseEnabled = true;
            CommandLineFormat = @"mysql -h localhost -u root --password=p4ssw0rd <{0}";
        }

        public override string FormatName(string name)
        {
            return string.Format("`{0}`", name);
        }

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return FormatName(name);
        }

        public override string GetStoreType(EdmProperty property)
        {
            if (property.TypeUsage.EdmType.Name == "guid")
                return "CHAR(36) BINARY";

            return base.GetStoreType(property);
        }

        public override string WriteDropDatabase(string databaseName)
        {
            return string.Format("DROP DATABASE IF EXISTS {0};", FormatName(databaseName));
        }

        public override string WriteDropTable(EntitySet entitySet)
        {
            return string.Format("DROP TABLE IF EXISTS {0};", FormatName(entitySet.GetTableName()));
        }

        public override string WriteCreateDatabase(string databaseName)
        {
            return string.Format("CREATE DATABASE {0};", FormatName(databaseName));
        }

        public override string WriteUseDatabase(string databaseName)
        {
            return string.Format("USE {0};", FormatName(databaseName));
        }
    }
}