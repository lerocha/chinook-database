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

        public override string WriteDropDatabase(string databaseName) => string.Empty; // User created by container

        public override string WriteCreateDatabase(string databaseName)
        {
            // Grant necessary privileges to the APP_USER created by container
            var username = GetUsername(databaseName);
            var builder = new StringBuilder();
            builder.AppendFormat("GRANT UNLIMITED TABLESPACE TO {0};\r\n", username)
                .AppendFormat("GRANT CREATE TABLE TO {0};\r\n", username)
                .AppendFormat("GRANT CREATE VIEW TO {0};\r\n", username);
            return builder.ToString();
        }

        public override string WriteUseDatabase(string databaseName) => $"CONNECT {GetUsername(databaseName)}/{GetPassword(databaseName)}@FREEPDB1;\r\n";

        public override string WriteForeignKeyDeleteAction(ForeignKeyConstraint foreignKeyConstraint) => foreignKeyConstraint.DeleteRule switch
        {
            Rule.Cascade => "ON DELETE CASCADE",
            _ => ""
        };

        public override string WriteForeignKeyUpdateAction(ForeignKeyConstraint foreignKeyConstraint) => string.Empty;

        public override string WriteFinishCommit() => "commit;\r\nexit;";

        private static string GetUsername(String databaseName) => databaseName.ToLower();
        private static string GetPassword(String databaseName) => databaseName.ToLower();
    }

}