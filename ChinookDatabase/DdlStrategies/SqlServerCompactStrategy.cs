namespace ChinookDatabase.DdlStrategies
{
    public class SqlServerCompactStrategy : AbstractDdlStrategy
    {
        public SqlServerCompactStrategy()
        {
            Name = "SqlServerCompact";
            FileExtension = "sqlce";
            Identity = "IDENTITY";
            IsReCreateDatabaseEnabled = true;
        }

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return FormatName(name);
        }

    }
}
