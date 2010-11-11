using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;

namespace ChinookDatabase.DdlStrategies
{
    public interface IDdlStrategy
    {
        #region Read-only properties.

        string Name { get; }
        string FileExtension { get; }
        string Identity { get; }
        bool CreatePrimaryKeyOnTableCreate { get; }
        bool CreateForeignKeyOnTableCreate { get; }

        #endregion

        #region Customizable properties.

        bool IsIdentityEnabled { get; set; }
        bool CanReCreateDatabase { get; set; }
        string CommandLineFormat { get; set; }

        #endregion

        #region Methods used to build SQL commands.

        string FormatName(string name);
        string GetFullyQualifiedName(string schema, string name);
        string GetStoreType(EdmProperty property);
        string GetIdentity(EdmProperty property, Version targetVersion);
        string GetClustered(StoreItemCollection store, EntityType entityType);
        string GetForeignKeyConstraintName(ReferentialConstraint constraint);
        string GetColumns(IEnumerable<EdmProperty> properties, char delimiter);
        string GetDeleteAction(ReferentialConstraint refConstraint);

        #endregion

        #region Methods used to write full SQL commands.

        string WriteDropDatabase(string databaseName);
        string WriteDropTable(EntitySet entitySet);
        string WriteDropForeignKey(AssociationSet associationSet);
        string WriteCreateDatabase(string databaseName);
        string WriteUseDatabase(string databaseName);
        string WriteCreateColumn(EdmProperty property, Version targetVersion);
        string WriteExecuteCommand();
        string WriteCreateTableDelimiter();

        #endregion
    }
}