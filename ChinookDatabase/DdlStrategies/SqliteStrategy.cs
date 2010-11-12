using System;
using System.Data.Metadata.Edm;
using Microsoft.Data.Entity.Design.DatabaseGeneration;

namespace ChinookDatabase.DdlStrategies
{
    public class SqliteStrategy : AbstractDdlStrategy
    {
        public SqliteStrategy()
        {
            CommandLineFormat = "sqlite3 -init {0} {0}ite";
        }

        public override string Name { get { return "Sqlite"; } }

        public override string Identity
        {
            get { return "PRIMARY KEY AUTOINCREMENT"; }
        }

        public override KeyDefinition PrimaryKeyDef
        {
            get { return (IsIdentityEnabled ? KeyDefinition.OnCreateTableColumn : KeyDefinition.OnCreateTableBottom); }
        }

        public override KeyDefinition ForeignKeyDef
        {
            get { return KeyDefinition.OnCreateTableBottom; }
        }

        public override string FormatStringValue(string value)
        {
            return string.Format("'{0}'", value.Replace("'", "''"));
        }

        public override string FormatDateValue(string value)
        {
            var date = Convert.ToDateTime(value);
            return string.Format("'{0}'", date.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return FormatName(name);
        }

        public override string GetStoreType(EdmProperty property)
        {
            if (property.TypeUsage.EdmType.Name == "int")
                return "INTEGER";

            return base.GetStoreType(property);
        }

        public override string WriteCreateColumn(EdmProperty property, Version targetVersion)
        {
            var notnull = (property.Nullable ? "" : "NOT NULL");
            var identity = GetIdentity(property, targetVersion);
            return string.Format("{0} {1} {2} {3}",
                                 FormatName(property.Name),
                                 GetStoreType(property),
                                 identity, notnull).Trim();
        }

        public override string WriteDropTable(EntitySet entitySet)
        {
            return string.Format("DROP TABLE IF EXISTS {0};", FormatName(entitySet.GetTableName()));
        }

        public override string WriteDropForeignKey(AssociationSet associationSet)
        {
            return string.Empty;
        }
    }
}
