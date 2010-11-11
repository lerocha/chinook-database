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

        public override bool CreateForeignKeyOnTableCreate
        {
            get { return true; }
        }

        public override string FormatStringValue(string value)
        {
            return string.Format("'{0}'", value.Replace("'", "''"));
        }

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return FormatName(name);
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
