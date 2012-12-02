using System.Data.Metadata.Edm;
using System.Text;
using Microsoft.Data.Entity.Design.DatabaseGeneration;

namespace ChinookDatabase.DdlStrategies
{
    public class PostgreSqlStrategy : AbstractDdlStrategy
    {
        public PostgreSqlStrategy()
        {
            var builder = new StringBuilder();

            builder.AppendLine("SET \"PGOPTIONS=-c client_min_messages=WARNING\"")
				.AppendLine("dropdb --if-exists -U postgres Chinook")
				.AppendLine("createdb -U postgres Chinook")
				.AppendLine("psql -f {0} -q Chinook postgres");

            Name = "PostgreSql";
            IsReCreateDatabaseEnabled = true;
            CommandLineFormat = builder.ToString();
        }

        public override string FormatName(string name)
        {
            return string.Format("\"{0}\"", name);
        }

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return FormatName(name);
        }

        public override string GetStoreType(EdmProperty property)
        {
            switch (property.TypeUsage.EdmType.Name)
            {
                case "datetime":
                    return "TIMESTAMP";
                case "nvarchar":
                    return property.ToStoreType().Replace("nvarchar", "VARCHAR");
                default:
                    return base.GetStoreType(property);
            }
        }
    }
}
