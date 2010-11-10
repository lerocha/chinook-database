namespace ChinookDatabase.DdlStrategies
{
    public class EffiProzStrategy : AbstractDdlStrategy
    {
        public override string Name
        {
            get { return "EffiProz"; }
        }

        public override string Identity
        {
            get { return "IDENTITY"; }
        }

        public override string FormatName(string name)
        {
            return string.Format("\"{0}\"", name);
        }
    }
}