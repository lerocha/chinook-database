using System.Data;
using System.Text;

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

        public override string GetFullyQualifiedName(string name) => $"[dbo].{FormatName(name)}";

        public override string GetClustered(DataTable table) => table.PrimaryKey.Length > 1 ? "NONCLUSTERED" : "CLUSTERED";

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

        public override string WriteDropTable(string tableName) => $"IF OBJECT_ID(N'{GetFullyQualifiedName(tableName)}', 'U') IS NOT NULL DROP TABLE {GetFullyQualifiedName(tableName)};";

        public override string WriteCreateDatabase(string databaseName) => $"CREATE DATABASE {FormatName(databaseName)};";

        public override string WriteUseDatabase(string databaseName) => $"USE {FormatName(databaseName)};";

        public override string WriteExecuteCommand() => "GO";
    }
}