using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Globalization;


namespace ChinookDatabase.DdlStrategies
{
    /// <summary>
    /// Extensions from Microsoft.Data.Entity.Design.DatabaseGeneration.dll
    /// https://github.com/dotnet/ef6tools/blob/main/src/EFTools/EntityDesignDatabaseGeneration/MetadataWorkspaceExtensions.cs
    /// </summary>
    public static class MetadataEdmExtensions
    {

        //public static string ToStoreType(this EdmProperty property)
        //{
        //    var sqlTypeName = String.Empty;
        //    var storeTypeUsage = property.TypeUsage;
        //    Debug.Assert(storeTypeUsage != null, "TypeUsage for property: " + property.Name + " is null");
        //    if (storeTypeUsage != null)
        //    {
        //        var edmType = storeTypeUsage.EdmType;
        //        Debug.Assert(edmType != null, "Edm Type for: " + storeTypeUsage + " is null");
        //        if (edmType != null)
        //        {
        //            sqlTypeName = storeTypeUsage.EdmType.Name;
        //            var primType = storeTypeUsage.EdmType as PrimitiveType;
        //            if (primType != null)
        //            {
        //                Facet maxLengthFacet = null;
        //                Facet precisionFacet = null;
        //                Facet scaleFacet = null;
        //                switch (primType.PrimitiveTypeKind)
        //                {
        //                    case PrimitiveTypeKind.Binary:
        //                        storeTypeUsage.Facets.TryGetValue("MaxLength", false, out maxLengthFacet);
        //                        Debug.Assert(
        //                            maxLengthFacet != null, "MaxLength facet should exist for binary Store Type: " + storeTypeUsage);
        //                        if (maxLengthFacet != null
        //                            && maxLengthFacet.Description.IsConstant == false)
        //                        {
        //                            sqlTypeName = String.Format(CultureInfo.CurrentCulture, "{0}({1})", sqlTypeName, maxLengthFacet.Value);
        //                        }
        //                        break;
        //                    case PrimitiveTypeKind.String:
        //                        storeTypeUsage.Facets.TryGetValue("MaxLength", false, out maxLengthFacet);
        //                        Debug.Assert(
        //                            maxLengthFacet != null, "MaxLength facet should exist for string Store Type: " + storeTypeUsage);
        //                        if (maxLengthFacet != null
        //                            && maxLengthFacet.Description.IsConstant == false)
        //                        {
        //                            sqlTypeName = String.Format(CultureInfo.CurrentCulture, "{0}({1})", sqlTypeName, maxLengthFacet.Value);
        //                        }
        //                        break;
        //                    case PrimitiveTypeKind.Decimal:
        //                        storeTypeUsage.Facets.TryGetValue("Precision", false, out precisionFacet);
        //                        storeTypeUsage.Facets.TryGetValue("Scale", false, out scaleFacet);
        //                        Debug.Assert(
        //                            precisionFacet != null, "Precision facet should exist for decimal Store Type: " + storeTypeUsage);
        //                        Debug.Assert(scaleFacet != null, "Scale facet should exist for decimal Store Type: " + storeTypeUsage);
        //                        if (precisionFacet != null
        //                            &&
        //                            scaleFacet != null
        //                            &&
        //                            precisionFacet.Description.IsConstant == false)
        //                        {
        //                            sqlTypeName = String.Format(
        //                                CultureInfo.CurrentCulture,
        //                                "{0}({1},{2})", sqlTypeName, precisionFacet.Value, scaleFacet.Value);
        //                        }

        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    return sqlTypeName;
        //}

        //public static StoreGeneratedPattern GetStoreGeneratedPatternValue(this EdmProperty property, Version targetVersion, DataSpace dataSpace)
        //{
        //    return StoreGeneratedPattern.None;
        //}

        //public static string GetTableName(this EntitySet entitySet)
        //{
        //    return "";
        //}

        //public static string GetSchemaName(this EntitySet entitySet)
        //{
        //    return "";
        //}

        //public static bool IsJoinTable(this EntityType entityType, StoreItemCollection store)
        //{
        //    return false;
        //}

    }

}
