using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Text;
using Microsoft.Data.Entity.Design.DatabaseGeneration;

namespace ChinookDatabase.DdlStrategies
{
    public abstract class AbstractDdlStrategy : IDdlStrategy
    {

        protected AbstractDdlStrategy()
        {
            Encoding = Encoding.UTF8;
        }

        #region Implementation of IDdlStrategy

        public virtual string Name
        {
            get { return string.Empty; }
        }

        public virtual string FileExtension
        {
            get { return "sql"; }
        }

        public virtual string Identity
        {
            get { return string.Empty; }
        }

        public virtual KeyDefinition PrimaryKeyDef
        {
            get { return KeyDefinition.OnCreateTableBottom; }
        }

        public virtual KeyDefinition ForeignKeyDef
        {
            get { return KeyDefinition.OnAlterTable; }
        }

        public bool IsIdentityEnabled { get; set; }

        public bool CanReCreateDatabase { get; set; }

        public string CommandLineFormat { get; set; }

        public Encoding Encoding { get; set; }

        public virtual string FormatName(string name)
        {
            return string.Format("[{0}]", name);
        }

        public virtual string FormatStringValue(string value)
        {
            return string.Format("N'{0}'", value.Replace("'", "''"));
        }

        public virtual string FormatDateValue(string value)
        {
            var date = Convert.ToDateTime(value);
            return string.Format("'{0}/{1:0}/{2:0}'", date.Year, date.Month, date.Day);
        }

        public virtual string GetFullyQualifiedName(string schema, string name)
        {
            return string.Format("{0}.{1}", FormatName(schema), FormatName(name));
        }

        public virtual string GetStoreType(EdmProperty property)
        {
            return property.ToStoreType().ToUpper();
        }

        public virtual string GetIdentity(EdmProperty property, Version targetVersion)
        {
            if (IsIdentityEnabled &&
                property.GetStoreGeneratedPatternValue(targetVersion, DataSpace.SSpace) ==
                StoreGeneratedPattern.Identity)
            {
                return Identity;
            }

            return String.Empty;
        }

        public virtual string GetClustered(StoreItemCollection store, EntityType entityType)
        {
            return string.Empty;
        }

        public virtual string GetForeignKeyConstraintName(ReferentialConstraint constraint)
        {
            var name = constraint.FromRole.DeclaringType.Name;

            if (!name.StartsWith("FK_", StringComparison.InvariantCultureIgnoreCase))
            {
                name = "FK_" + name;
            }

            return name;
        }

        public virtual string GetColumns(IEnumerable<EdmProperty> properties, char delimiter)
        {
            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                builder.AppendFormat("{0}{1} ", FormatName(property.Name), delimiter);
            }
            return builder.ToString().Trim().TrimEnd(delimiter);
        }

        public string GetDeleteAction(ReferentialConstraint refConstraint)
        {
            return refConstraint.FromRole.DeleteBehavior == OperationAction.Cascade ? "CASCADE" : "NO ACTION";
        }

        public virtual string WriteDropDatabase(string databaseName)
        {
            return string.Empty;
        }

        public virtual string WriteDropTable(EntitySet entitySet)
        {
            return string.Format("DROP TABLE {0};", FormatName(entitySet.GetTableName()));
        }

        public virtual string WriteDropForeignKey(AssociationSet associationSet)
        {
            var constraint = associationSet.ElementType.ReferentialConstraints.Single();
            var constraintName = GetForeignKeyConstraintName(constraint);
            var end = associationSet.AssociationSetEnds
                .Where(a => a.CorrespondingAssociationEndMember == constraint.ToRole)
                .Single();
            var fqTableName = GetFullyQualifiedName(end.EntitySet.GetSchemaName(), end.EntitySet.GetTableName());
            var name = FormatName(constraintName);
            return string.Format("ALTER TABLE {0} DROP CONSTRAINT {1};", fqTableName, name);
        }

        public virtual string WriteCreateDatabase(string databaseName)
        {
            return string.Empty;
        }

        public virtual string WriteUseDatabase(string databaseName)
        {
            return string.Empty;
        }

        public virtual string WriteCreateColumn(EdmProperty property, Version targetVersion)
        {
            var notnull = (property.Nullable ? "" : "NOT NULL");
            var identity = GetIdentity(property, targetVersion);
            return string.Format("{0} {1} {2} {3}",
                                 FormatName(property.Name),
                                 GetStoreType(property),
                                 notnull, identity).Trim();
        }

        public virtual string WriteExecuteCommand()
        {
            return string.Empty;
        }

        #endregion
    }

}