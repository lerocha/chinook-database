using System;
using System.Data.Metadata.Edm;
using System.Text;
using Microsoft.Data.Entity.Design.DatabaseGeneration;

namespace ChinookDatabase.DdlStrategies
{
    public class OracleStrategy : AbstractDdlStrategy
    {
        public OracleStrategy()
        {
            Name = "Oracle";
            IsReCreateDatabaseEnabled = true;
            IsIndexEnabled = false;
            CommandLineFormat = @"sqlplus -S / as sysdba @ {0}";
        }

        public override string FormatName(string name)
        {
            return string.Format("{0}", name);
        }

        public override string FormatStringValue(string value)
        {
            return string.Format("'{0}'", value.Replace("'", "'||chr(39)||'").Replace("&", "'||chr(38)||'"));
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

        public override string GetStoreType(EdmProperty property)
        {
            switch (property.TypeUsage.EdmType.Name)
            {
                case "datetime":
                    return "DATE";
                case "int":
                    return "NUMBER";
                case "numeric":
                    return property.ToStoreType().Replace("numeric", "NUMBER");
                case "nvarchar":
                    return property.ToStoreType().Replace("nvarchar", "VARCHAR2");
                default:
                    return base.GetStoreType(property);
            }
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

        public override string WriteForeignKeyDeleteAction(ReferentialConstraint refConstraint)
        {
            return refConstraint.FromRole.DeleteBehavior == OperationAction.Cascade ? "ON DELETE CASCADE" : "";
        }

        public override string WriteForeignKeyUpdateAction(ReferentialConstraint refConstraint)
        {
            return string.Empty;
        }
    }

}