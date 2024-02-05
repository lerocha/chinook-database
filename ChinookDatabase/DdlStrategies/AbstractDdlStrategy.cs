using System.Data;
using System.Text;

namespace ChinookDatabase.DdlStrategies
{
    public abstract class AbstractDdlStrategy : IDdlStrategy
    {

        protected AbstractDdlStrategy()
        {
            Name = string.Empty;
            ScriptFileExtension = "sql";
            DatabaseFileExtension = string.Empty;
            Identity = string.Empty;
            IsIndexEnabled = true;
            PrimaryKeyDef = KeyDefinition.OnCreateTableBottom;
            ForeignKeyDef = KeyDefinition.OnAlterTable;

            Encoding = Encoding.UTF8;
        }

        #region Implementation of IDdlStrategy

        public string Name { get; protected set; }
        public string ScriptFileExtension { get; protected set; }
        public string DatabaseFileExtension { get; protected set; }
        public string Identity { get; protected set; }
        public bool IsIndexEnabled { get; protected set; }

        public KeyDefinition PrimaryKeyDef { get; set; }
        public KeyDefinition ForeignKeyDef { get; set; }
        public PrimaryKeyStrategy PrimaryKeyStrategy { get; set; }
        public bool IsReCreateDatabaseEnabled { get; set; }
        public string CommandLineFormat { get; set; }
        public Encoding Encoding { get; set; }

        public virtual string FormatName(string name) => $"[{name}]";

        public virtual string FormatCase(string text) => text;

        public virtual string FormatPrimaryKey(string name) => FormatName($"PK_{name}");

        public virtual string FormatForeignKey(string table, string column) => FormatName($"FK_{table}{column}");

        public virtual string FormatForeignKeyIndex(string table, string column) => FormatName($"IFK_{table}{column}");

        public virtual string FormatStringValue(string value) => $"N'{value.Replace("'", "''")}'";

        public virtual string FormatDateValue(string value)
        {
            var date = Convert.ToDateTime(value);
            return $"'{date.Year}/{date.Month:0}/{date.Day:0}'";
        }

        public virtual string GetFullyQualifiedName(string name) => $"{FormatName(name)}";

        public virtual string GetStoreType(DataColumn column) => column.DataType.ToString() switch
        {
            "System.String" => $"NVARCHAR({column.MaxLength})",
            "System.Int32" => "INT",
            "System.Decimal" => "NUMERIC(10,2)",
            "System.DateTime" => "DATETIME",
            _ => "error_" + column.DataType
        };

        public virtual string GetClustered(DataTable table) => string.Empty;

        public virtual string GetColumns(IEnumerable<String> keys, char delimiter)
        {
            var builder = new StringBuilder();
            foreach (var key in keys)
            {
                builder.AppendFormat("{0}{1} ", FormatName(key), delimiter);
            }
            return builder.ToString().Trim().TrimEnd(delimiter);

        }

        public virtual string WriteDropDatabase(string databaseName) => string.Empty;

        public virtual string WriteDropTable(string tableName) => $"DROP TABLE {tableName};";

        public virtual string WriteDropForeignKey(string tableName, string columnName) => $"ALTER TABLE {tableName} DROP CONSTRAINT {columnName};";

        public virtual string WriteCreateDatabase(string databaseName) => string.Empty;

        public virtual string WriteUseDatabase(string databaseName) => string.Empty;

        public virtual string WriteCreateColumn(DataColumn column)
        {
            var isPrimaryKey = column.Table?.PrimaryKey.Length == 1 && column.Table?.PrimaryKey.Contains(column) == true;
            var type = isPrimaryKey && (PrimaryKeyStrategy == PrimaryKeyStrategy.Serial) ? "SERIAL" : GetStoreType(column);
            var notnull = (column.AllowDBNull ? "" : "NOT NULL");
            var identity = (PrimaryKeyStrategy == PrimaryKeyStrategy.Identity) && isPrimaryKey ? Identity : String.Empty;
            return string.Format("{0} {1} {2} {3}",
                                 FormatName(column.ColumnName),
                                 type,
                                 notnull, identity).Trim();
        }

        public virtual string WriteForeignKeyDeleteAction(ForeignKeyConstraint foreignKeyConstraint) => foreignKeyConstraint.DeleteRule switch
        {
            Rule.Cascade => "ON DELETE CASCADE",
            _ => "ON DELETE NO ACTION"
        };

        public virtual string WriteForeignKeyUpdateAction(ForeignKeyConstraint foreignKeyConstraint) => "ON UPDATE NO ACTION";

        public virtual string WriteExecuteCommand() => string.Empty;

        public virtual string WriteFinishCommit() => string.Empty;

        #endregion
    }

}