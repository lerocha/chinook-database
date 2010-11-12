namespace ChinookDatabase.DdlStrategies
{
    public class SqlServerCompactStrategy : AbstractDdlStrategy
    {
        public SqlServerCompactStrategy()
        {
            CanReCreateDatabase = true;
        }

        public override string Name { get { return "SqlServerCompact"; } }

        public override string FileExtension { get { return "sqlce"; } }

        public override string Identity
        {
            get { return "IDENTITY"; }
        }

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return FormatName(name);
        }

    }
}
