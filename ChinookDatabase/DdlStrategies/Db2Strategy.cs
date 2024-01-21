using System.Data;
using System.Text;

namespace ChinookDatabase.DdlStrategies
{
	public class Db2Strategy : AbstractDdlStrategy
	{
		public Db2Strategy()
		{
			Name = "Db2";
			//Identity = "AUTO_INCREMENT";
			IsReCreateDatabaseEnabled = true;

			var builder = new StringBuilder();
			builder.AppendLine("db2 disconnect ALL")
				.AppendLine("db2 drop database Chinook")
				.AppendLine("db2 create database Chinook")
				.AppendLine("db2 connect to Chinook")
				.AppendLine("db2 -tf {0} -z {0}.log");

			CommandLineFormat = builder.ToString();
		}

		public override string FormatDateValue(string value)
		{
			var date = Convert.ToDateTime(value);
			return $"'{date:yyyy-MM-dd HH:mm:ss}'";
		}

        public override string FormatName(string name) => $"\"{name}\"";

        public override string GetStoreType(DataColumn column) => column.DataType.ToString() switch
        {
            "System.String" => $"VARCHAR({column.MaxLength})",
            "System.Int32" => "INT",
            "System.Decimal" => "NUMERIC(10,2)",
            "System.DateTime" => "DATE",
            _ => "error_" + column.DataType
        };
    }
}
