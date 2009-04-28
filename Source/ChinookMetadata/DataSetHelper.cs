using System;
using System.Data;
using System.Text;

namespace ChinookMetadata
{
    class DataSetHelper
    {
        public static string GetVersionNumber()
        {
            return "1.1";
        }

        public static string GetOracleType(DataColumn col)
        {
            switch (col.DataType.ToString())
            {
                case "System.String":
                    return string.Format("VARCHAR2({0})", col.MaxLength);
                case "System.Int32":
                    return "NUMBER";
                case "System.Decimal":
                    return "NUMBER";
                case "System.DateTime":
                    return "DATE";
                default:
                    return "error_" + col.DataType;
            }
        }

        public static string GetSqlServerType(DataColumn col)
        {
            switch (col.DataType.ToString())
            {
                case "System.String":
                    return string.Format("NVARCHAR({0})", col.MaxLength);
                case "System.Int32":
                    return "INTEGER";
                case "System.Decimal":
                    return "NUMERIC(10,2)";
                case "System.DateTime":
                    return "DATETIME";
                default:
                    return "error_" + col.DataType;
            }
        }

        public static string GetMySqlType(DataColumn col)
        {
            switch (col.DataType.ToString())
            {
                case "System.String":
                    return string.Format("VARCHAR({0})", col.MaxLength);
                case "System.Int32":
                    return "INTEGER";
                case "System.Decimal":
                    return "NUMERIC(10,2)";
                case "System.DateTime":
                    return "DATE";
                default:
                    return "error_" + col.DataType;
            }
        }

        public static bool IsLastCreateTableElement(DataTable table, DataColumn col)
        {
            if ((col == table.Columns[table.Columns.Count - 1]) && (table.Constraints.Count == 0))
                return true;

            return false;
        }

        /// <summary>
        /// Gets a string containing the primary key of the given table. For example "CustomerId" or "PlaylistId, TrackId".
        /// </summary>
        /// <param name="table"></param>
        /// <returns>A string representing the primary key (single or composite).</returns>
        public static string GetPrimaryKeyString(DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Constraint constraint in table.Constraints)
            {
                UniqueConstraint uniqueConstraint = constraint as UniqueConstraint;

                if (uniqueConstraint != null && uniqueConstraint.IsPrimaryKey)
                {
                    foreach (DataColumn pk in uniqueConstraint.Columns)
                    {
                        if (sb.Length != 0) sb.Append(", ");
                        sb.Append(pk.ColumnName);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the name of the primary key constraint, for example "PK_Customer".
        /// </summary>
        /// <param name="table"></param>
        /// <returns>Primary key constraint name.</returns>
        public static string GetPrimaryKeyConstraintName(DataTable table)
        {
            foreach (Constraint constraint in table.Constraints)
            {
                UniqueConstraint uniqueConstraint = constraint as UniqueConstraint;

                if (uniqueConstraint != null && uniqueConstraint.IsPrimaryKey)
                {
                    return uniqueConstraint.ConstraintName;
                }
            }
            return string.Empty;
        }

        public static string GetExpectedValue(DataColumn col, string value)
        {
            string expected;
            if (col.DataType == typeof(DateTime))
            {
                expected = string.Format("Convert.ToDateTime(\"{0}\").ToString()", value);
            }
            else
            {
                expected = string.Format("\"{0}\"", value);
            }

            return expected;
        }

    }
}
