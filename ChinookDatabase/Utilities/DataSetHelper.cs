using System;
using System.Data;
using System.Text;

namespace ChinookDatabase.Utilities
{
    class DataSetHelper
    {
        public static string GetVersionNumber()
        {
            return "1.4.5";
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
        /// <param name="nameFormat"></param>
        /// <returns>A string representing the primary key (single or composite).</returns>
        public static string GetPrimaryKeyString(DataTable table, string nameFormat = "{0}")
        {
            var sb = new StringBuilder();
            foreach (var constraint in table.Constraints)
            {
                var uniqueConstraint = constraint as UniqueConstraint;

                if (uniqueConstraint != null && uniqueConstraint.IsPrimaryKey)
                {
                    foreach (var pk in uniqueConstraint.Columns)
                    {
                        if (sb.Length != 0) sb.Append(", ");
                        sb.AppendFormat(nameFormat, pk.ColumnName);
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
                var uniqueConstraint = constraint as UniqueConstraint;

                if (uniqueConstraint != null && uniqueConstraint.IsPrimaryKey)
                {
                    return uniqueConstraint.ConstraintName;
                }
            }
            return string.Empty;
        }

        public static string GetExpectedValue(DataColumn column, string value) => column.DataType.ToString() switch
        {
            "System.DateTime" => $"DateTime.Parse(\"{value}\").ToString()",
            _ => $"\"{value}\""
        };
    }
}
