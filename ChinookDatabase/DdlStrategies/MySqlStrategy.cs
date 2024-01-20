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

        public override string FormatName(string name)
        {
            return $"`{name}`";
        }

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return FormatName(name);
        }

        public override string GetStoreType(DataColumn column)
        {
            return column.DataType.ToString() switch
            {
                "System.String" => $"NVARCHAR({column.MaxLength})",
                "System.Int32" => "INT",
                "System.Decimal" => "NUMERIC(10,2)",
                "System.DateTime" => "DATETIME",
                _ => "error_" + column.DataType
            };
        }
        
        public override string WriteDropDatabase(string databaseName)
        {
            return string.Format("DROP DATABASE IF EXISTS {0};", FormatName(databaseName));
        }

        public override string WriteDropTable(string tableName)
        {
            return $"DROP TABLE IF EXISTS {FormatName(tableName)};";
        }

        public override string WriteCreateDatabase(string databaseName)
        {
            return $"CREATE DATABASE {FormatName(databaseName)};";
        }

        public override string WriteUseDatabase(string databaseName)
        {
            return $"USE {FormatName(databaseName)};";
        }
    }
}