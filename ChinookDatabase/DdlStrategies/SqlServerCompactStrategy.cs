using System.Data;

namespace ChinookDatabase.DdlStrategies
{
    public class SqlServerCompactStrategy : AbstractDdlStrategy
    {
        public SqlServerCompactStrategy()
        {
            Name = "SqlServerCompact";
            ScriptFileExtension = "sqlce";
            DatabaseFileExtension = "sdf";
            Identity = "IDENTITY";
            IsReCreateDatabaseEnabled = true;
        }

        public override string GetStoreType(DataColumn column) => column.DataType.ToString() switch
        {
            "System.String" => $"NVARCHAR({column.MaxLength})",
            "System.Int32" => "INT",
            "System.Decimal" => "NUMERIC(10,2)",
            "System.DateTime" => "DATETIME",
            _ => "error_" + column.DataType
        };
    }
}
