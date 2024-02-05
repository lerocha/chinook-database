using System.Data;

namespace ChinookDatabase.DdlStrategies
{
    public class SqliteStrategy : AbstractDdlStrategy
    {
        public SqliteStrategy()
        {
            Name = "Sqlite";
            DatabaseFileExtension = "sqlite";
            Identity = "PRIMARY KEY AUTOINCREMENT";
            ForeignKeyDef = KeyDefinition.OnCreateTableBottom;
            CommandLineFormat = "if exist {0}ite del {0}ite\nsqlite3 -init {0} {0}ite";
        }

        public override string FormatStringValue(string value) => string.Format("'{0}'", value.Replace("'", "''"));

        public override string FormatDateValue(string value)
        {
            var date = Convert.ToDateTime(value);
            return string.Format("'{0}'", date.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public override string GetStoreType(DataColumn column) => column.DataType.ToString() switch
        {
            "System.String" => $"NVARCHAR({column.MaxLength})",
            "System.Int32" => "INTEGER",
            "System.Decimal" => "NUMERIC(10,2)",
            "System.DateTime" => "DATETIME",
            _ => "error_" + column.DataType
        };

        public override string WriteCreateColumn(DataColumn column)
        {
            var notnull = (column.AllowDBNull ? "" : "NOT NULL");
            var isPrimaryKey = column.Table?.PrimaryKey.Length == 1 && column.Table?.PrimaryKey.Contains(column) == true;
            var identity = (PrimaryKeyStrategy == PrimaryKeyStrategy.Identity) && isPrimaryKey ? Identity : String.Empty;
            return string.Format("{0} {1} {2} {3}",
                                 FormatName(column.ColumnName),
                                 GetStoreType(column),
                                 identity, notnull).Trim();
        }

        public override string WriteDropTable(string tableName) => $"DROP TABLE IF EXISTS {FormatName(tableName)};";

        public override string WriteDropForeignKey(string tableName, string columnName) => string.Empty;
    }
}
