using System.Data.Metadata.Edm;
using System.Text;
using Microsoft.Data.Entity.Design.DatabaseGeneration;

namespace ChinookDatabase.DdlStrategies
{
    public class SqlServerStrategy : AbstractDdlStrategy
    {
        public SqlServerStrategy()
        {
            Name = "SqlServer";
            Identity = "IDENTITY";
            IsReCreateDatabaseEnabled = true;
            CommandLineFormat = @"sqlcmd -E -S .\sqlexpress -i {0} -b -m 1";
        }

        public override string GetClustered(StoreItemCollection store, EntityType entityType)
        {
            return entityType.IsJoinTable(store) ? "NONCLUSTERED" : "CLUSTERED";
        }

        public override string WriteDropDatabase(string databaseName)
        {
            var name = FormatName(databaseName);
            var builder = new StringBuilder();

            builder.AppendFormat("IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'{0}')\r\n",
                                 databaseName)
                .AppendFormat("BEGIN\r\n")
                .AppendFormat("\tALTER DATABASE {0} SET OFFLINE WITH ROLLBACK IMMEDIATE;\r\n", name)
                .AppendFormat("\tALTER DATABASE {0} SET ONLINE;\r\n", name)
                .AppendFormat("\tDROP DATABASE {0};\r\n", name)
                .AppendFormat("END\r\n");

            return builder.ToString();
        }

        public override string WriteDropTable(EntitySet entitySet)
        {
            var fqName = GetFullyQualifiedName(entitySet.GetSchemaName(), entitySet.GetTableName());
            return string.Format("IF OBJECT_ID(N'{0}', 'U') IS NOT NULL DROP TABLE {0};", fqName);
        }

        public override string WriteCreateDatabase(string databaseName)
        {
            return string.Format("CREATE DATABASE {0};", FormatName(databaseName));
        }

        public override string WriteUseDatabase(string databaseName)
        {
            return string.Format("USE {0};", FormatName(databaseName));
        }

        public override string WriteExecuteCommand()
        {
            return "GO";
        }
    }
}