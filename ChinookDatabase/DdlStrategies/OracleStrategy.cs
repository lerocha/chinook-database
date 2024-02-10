using System.Data;
using System.Text;

namespace ChinookDatabase.DdlStrategies
{
    public class OracleStrategy : AbstractDdlStrategy
    {
        public OracleStrategy()
        {
            Name = "Oracle";
            IsReCreateDatabaseEnabled = true;
            IsIndexEnabled = false;

	        var builder = new StringBuilder();
	        builder.AppendLine("chcp 65001") // sets the active code page number
		        .AppendLine("set NLS_LANG=.AL32UTF8") // set NLS_LANG so that sqlplus knows we are using unicode
		        .AppendLine(@"sqlplus -S / as sysdba @ {0}");

            CommandLineFormat = builder.ToString();
        }

        public override string FormatName(string name) => name;

        public override string FormatStringValue(string value) => $"'{value.Replace("'", "'||chr(39)||'").Replace("&", "'||chr(38)||'")}'";

        public override string FormatDateValue(string value)
        {
            var date = Convert.ToDateTime(value);
            return $"TO_DATE('{date.Year}-{date.Month}-{date.Day} 00:00:00','yyyy-mm-dd hh24:mi:ss')";
        }

        public override string GetStoreType(DataColumn column) => column.DataType.ToString() switch
        {
            "System.String" => $"VARCHAR2({column.MaxLength})",
            "System.Int32" => "NUMBER",
            "System.Decimal" => "NUMBER(10,2)",
            "System.DateTime" => "DATE",
            _ => "error_" + column.DataType
        };

        public override string WriteDropDatabase(string databaseName) => $"DROP USER {GetUsername(databaseName)} CASCADE;";

        public override string WriteCreateDatabase(string databaseName)
        {
            var username = GetUsername(databaseName);
            var builder = new StringBuilder();

            builder.AppendFormat("CREATE USER {0}\r\n", username)
                .AppendFormat("IDENTIFIED BY {0}\r\n", GetPassword(databaseName))
                .AppendFormat("DEFAULT TABLESPACE users\r\n")
                .AppendFormat("TEMPORARY TABLESPACE temp\r\n")
                .AppendFormat("QUOTA 10M ON users;\r\n\r\n")
                .AppendFormat("GRANT connect to {0};\r\n", username)
                .AppendFormat("GRANT resource to {0};\r\n", username)
                .AppendFormat("GRANT create session TO {0};\r\n", username)
                .AppendFormat("GRANT create table TO {0};\r\n", username)
                .AppendFormat("GRANT create view TO {0};\r\n", username);

            return builder.ToString();
        }

        public override string WriteUseDatabase(string databaseName) => $"conn {GetUsername(databaseName)}/{GetPassword(databaseName)}";

        public override string WriteForeignKeyDeleteAction(ForeignKeyConstraint foreignKeyConstraint) => foreignKeyConstraint.DeleteRule switch
        {
            Rule.Cascade => "ON DELETE CASCADE",
            _ => ""
        };

        public override string WriteForeignKeyUpdateAction(ForeignKeyConstraint foreignKeyConstraint) => string.Empty;

        public override string WriteFinishCommit() => "commit;\r\nexit;";

        private static string GetUsername(String databaseName) => $"c##{databaseName.ToLower()}";
        private static string GetPassword(String databaseName) => databaseName.ToLower();
    }

}