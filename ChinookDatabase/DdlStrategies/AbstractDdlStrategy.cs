using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
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
        public bool IsIdentityEnabled { get; set; }
        public bool IsReCreateDatabaseEnabled { get; set; }
        public string CommandLineFormat { get; set; }
        public Encoding Encoding { get; set; }

        public virtual string FormatName(string name)
        {
            return $"[{name}]";
        }

        public virtual string FormatStringValue(string value)
        {
            return $"N'{value.Replace("'", "''")}'";
        }

        public virtual string FormatDateValue(string value)
        {
            var date = Convert.ToDateTime(value);
            return $"'{date.Year}/{date.Month:0}/{date.Day:0}'";
        }

        public virtual string GetFullyQualifiedName(string schema, string name)
        {
            return $"{FormatName(schema)}.{FormatName(name)}";
        }

        public virtual string GetStoreType(DataColumn column)
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
        
        public virtual string GetClustered(DataTable table)
        {
            return string.Empty;
        }

        public virtual string GetForeignKeyConstraintName(ReferentialConstraint constraint)
        {
            var name = constraint.FromRole.DeclaringType.Name;

            if (!name.StartsWith("FK_", StringComparison.InvariantCultureIgnoreCase))
            {
                name = "FK_" + name;
            }

            return name;
        }

        public virtual string GetColumns(IEnumerable<EdmProperty> properties, char delimiter)
        {
            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                builder.AppendFormat("{0}{1} ", FormatName(property.Name), delimiter);
            }
            return builder.ToString().Trim().TrimEnd(delimiter);
        }

        public virtual string GetColumns(HashSet<String> properties, char delimiter)
        {
            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                builder.AppendFormat("{0}{1} ", FormatName(property), delimiter);
            }
            return builder.ToString().Trim().TrimEnd(delimiter);

        }

        public virtual string WriteDropDatabase(string databaseName)
        {
            return string.Empty;
        }

        public virtual string WriteDropTable(string tableName)
        {
            return $"DROP TABLE {tableName};";
        }

        public virtual string WriteDropForeignKey(string tableName, string columnName)
        {
            return $"ALTER TABLE {tableName} DROP CONSTRAINT {columnName};";
        }

        public virtual string WriteCreateDatabase(string databaseName)
        {
            return string.Empty;
        }

        public virtual string WriteUseDatabase(string databaseName)
        {
            return string.Empty;
        }

        public virtual string WriteCreateColumn(DataColumn column)
        {
            var notnull = (column.AllowDBNull ? "" : "NOT NULL");
            var identity = IsIdentityEnabled ? Identity : String.Empty;
            return string.Format("{0} {1} {2} {3}",
                                 FormatName(column.ColumnName),
                                 GetStoreType(column),
                                 notnull, identity).Trim();
        }

        public virtual string WriteForeignKeyDeleteAction(ReferentialConstraint refConstraint)
        {
            return refConstraint.FromRole.DeleteBehavior == OperationAction.Cascade ? "ON DELETE CASCADE" : "ON DELETE NO ACTION";
        }

        public virtual string WriteForeignKeyUpdateAction()
        {
            return "ON UPDATE NO ACTION";
        }

        public virtual string WriteExecuteCommand()
        {
            return string.Empty;
        }

        public virtual string WriteFinishCommit()
        {
            return string.Empty;
        }

        #endregion
    }

}