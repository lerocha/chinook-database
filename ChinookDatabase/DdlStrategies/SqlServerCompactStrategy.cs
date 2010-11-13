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

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return FormatName(name);
        }

    }
}
