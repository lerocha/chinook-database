using System;
using System.Data.Entity.Core.Metadata.Edm;

namespace ChinookDatabase.DdlStrategies
{
    /// <summary>
    /// Extensions from Microsoft.Data.Entity.Design.DatabaseGeneration.dll
    /// </summary>
    public static class MetadataEdmExtensions
    {

        public static string ToStoreType(this EdmProperty property)
        {
            return "";
        }

        public static StoreGeneratedPattern GetStoreGeneratedPatternValue(this EdmProperty property, Version targetVersion, DataSpace dataSpace)
        {
            return StoreGeneratedPattern.None;
        }

        public static string GetTableName(this EntitySet entitySet)
        {
            return "";
        }

        public static string GetSchemaName(this EntitySet entitySet)
        {
            return "";
        }

        public static bool IsJoinTable(this EntityType entityType, StoreItemCollection store)
        {
            return false;
        }

    }

}
