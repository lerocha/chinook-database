using System;
using System.Data.Metadata.Edm;
using System.Text;
using Microsoft.Data.Entity.Design.DatabaseGeneration;

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
			return string.Format("'{0}'", date.ToString("yyyy-MM-dd HH:mm:ss"));
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
					return "DATE";
				case "nvarchar":
					return property.ToStoreType().Replace("nvarchar", "VARCHAR");
				default:
					return base.GetStoreType(property);
			}
		}
	}
}
