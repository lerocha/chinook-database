using System.Data;

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

        public override string FormatName(string name) => $"`{name}`";

        public override string GetStoreType(DataColumn column) => column.DataType.ToString() switch
        {
            "System.String" => $"NVARCHAR({column.MaxLength})",
            "System.Int32" => "INT",
            "System.Decimal" => "NUMERIC(10,2)",
            "System.DateTime" => "DATETIME",
            _ => "error_" + column.DataType
        };

        public override string WriteDropDatabase(string databaseName) => $"DROP DATABASE IF EXISTS {FormatName(databaseName)};";

        public override string WriteDropTable(string tableName) => $"DROP TABLE IF EXISTS {FormatName(tableName)};";

        public override string WriteCreateDatabase(string databaseName) => $"CREATE DATABASE {FormatName(databaseName)};";

        public override string WriteUseDatabase(string databaseName) => $"USE {FormatName(databaseName)};";
    }
}