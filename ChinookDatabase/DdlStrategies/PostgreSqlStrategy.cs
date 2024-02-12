using System.Data;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace ChinookDatabase.DdlStrategies
{
    public class PostgreSqlStrategy : AbstractDdlStrategy
    {
        private static readonly SnakeCaseNamingStrategy snakeCaseNamingStrategy = new();

        public PostgreSqlStrategy()
        {
            var builder = new StringBuilder();

            builder.AppendLine("SET \"PGOPTIONS=-c client_min_messages=WARNING\"")
				.AppendLine("dropdb --if-exists -U postgres Chinook")
				.AppendLine("createdb -U postgres Chinook")
				.AppendLine("psql -f {0} -q Chinook postgres");

            Name = "PostgreSql";
            Identity = "GENERATED ALWAYS AS IDENTITY";
            IsReCreateDatabaseEnabled = true;
            CommandLineFormat = builder.ToString();
        }

        public override string FormatName(string name) => ToSnakeCase(name);

        public override string FormatCase(string text) => ToSnakeCase(text);

        public override string FormatPrimaryKey(string name) => $"{ToSnakeCase(name)}_pkey";

        public override string FormatForeignKey(string table, string column) => $"{ToSnakeCase(table)}_{ToSnakeCase(column)}_fkey";

        public override string FormatForeignKeyIndex(string table, string column) => $"{ToSnakeCase(table)}_{ToSnakeCase(column)}_idx";

        public override string GetStoreType(DataColumn column) => column.DataType.ToString() switch
        {
            "System.String" => $"VARCHAR({column.MaxLength})",
            "System.Int32" => "INT",
            "System.Decimal" => "NUMERIC(10,2)",
            "System.DateTime" => "TIMESTAMP",
            _ => "error_" + column.DataType
        };

        public override string WriteDropDatabase(string databaseName) => $"DROP DATABASE IF EXISTS {FormatName(databaseName)};";

        public override string WriteCreateDatabase(string databaseName) => $"CREATE DATABASE {FormatName(databaseName)};";

        public override string WriteUseDatabase(string databaseName) => $"\\c {FormatName(databaseName)};";

        private static string ToSnakeCase(string text) => snakeCaseNamingStrategy.GetPropertyName(text, false);
    }
}
