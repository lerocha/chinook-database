using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;

namespace ChinookDatabase.DdlStrategies
{
    public interface IDdlStrategy
    {
        #region Read-only properties.

        string Name { get; }
        string ScriptFileExtension { get; }
        string DatabaseFileExtension { get; }
        string Identity { get; }
        bool IsIndexEnabled { get; }

        #endregion

        #region Customizable properties.

        KeyDefinition PrimaryKeyDef { get; set; }
        KeyDefinition ForeignKeyDef { get; set; }
        bool IsIdentityEnabled { get; set; }
        bool IsReCreateDatabaseEnabled { get; set; }
        string CommandLineFormat { get; set; }
        Encoding Encoding { get; set; }

        #endregion

        #region Methods used to build SQL commands.

        string FormatName(string name);
        string FormatStringValue(string value);
        string FormatDateValue(string date);
        string GetFullyQualifiedName(string name);
        string GetStoreType(DataColumn column);
        string GetClustered(DataTable table);
        string GetForeignKeyConstraintName(ReferentialConstraint constraint);
        string GetColumns(IEnumerable<String> keys, char delimiter);
        #endregion

        #region Methods used to write full SQL commands.

        string WriteDropDatabase(string databaseName);
        string WriteDropTable(string tableName);
        string WriteDropForeignKey(string tableName, string columnName);
        string WriteCreateDatabase(string databaseName);
        string WriteUseDatabase(string databaseName);
        string WriteCreateColumn(DataColumn column);
        string WriteForeignKeyDeleteAction(ForeignKeyConstraint foreignKeyConstraint);
        string WriteForeignKeyUpdateAction(ForeignKeyConstraint foreignKeyConstraint);
        string WriteExecuteCommand();
        string WriteFinishCommit();

        #endregion
    }
}