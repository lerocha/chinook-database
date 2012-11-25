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

            builder.Append("SET \"PGOPTIONS=-c client_min_messages=WARNING\"\r\n")
                .Append("dropdb --if-exists -U postgres Chinook\r\n")
                .Append("createdb -U postgres Chinook\r\n")
                .Append("psql -f {0} -q Chinook postgres");

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
