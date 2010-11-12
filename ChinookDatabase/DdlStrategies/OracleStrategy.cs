using System;
using System.Text;

namespace ChinookDatabase.DdlStrategies
{
    public class OracleStrategy : AbstractDdlStrategy
    {
        public OracleStrategy()
        {
            CanReCreateDatabase = true;
            CommandLineFormat = @"sqlplus -S / as sysdba @ {0}";
        }

        public override string Name
        {
            get { return "Oracle"; }
        }

        public override string FormatName(string name)
        {
            return string.Format("{0}", name);
        }

        public override string FormatStringValue(string value)
        {
            return string.Format("N'{0}'", value.Replace("'", "'||chr(39)||'").Replace("&", "'||chr(38)||'"));
        }

        public override string FormatDateValue(string value)
        {
            var date = Convert.ToDateTime(value);
            return string.Format("TO_DATE('{0}-{1}-{2} 00:00:00','yyyy-mm-dd hh24:mi:ss')", date.Year, date.Month, date.Day);
        }

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return FormatName(name);
        }

        public override string WriteDropDatabase(string databaseName)
        {
            return string.Format("DROP USER {0} CASCADE;", databaseName.ToLower());
        }

        public override string WriteCreateDatabase(string databaseName)
        {
            var name = databaseName.ToLower();
            var builder = new StringBuilder();

            builder.AppendFormat("CREATE USER {0}\r\n", name)
                .AppendFormat("IDENTIFIED BY p4ssw0rd\r\n")
                .AppendFormat("DEFAULT TABLESPACE users\r\n")
                .AppendFormat("TEMPORARY TABLESPACE temp\r\n")
                .AppendFormat("QUOTA 10M ON users;\r\n\r\n")
                .AppendFormat("GRANT connect to {0};\r\n", name)
                .AppendFormat("GRANT resource to {0};\r\n", name)
                .AppendFormat("GRANT create session TO {0};\r\n", name)
                .AppendFormat("GRANT create table TO {0};\r\n", name)
                .AppendFormat("GRANT create view TO {0};\r\n", name);

            return builder.ToString();
        }

        public override string WriteUseDatabase(string databaseName)
        {
            return string.Format("conn {0}/p4ssw0rd", databaseName.ToLower());
        }

    }

}