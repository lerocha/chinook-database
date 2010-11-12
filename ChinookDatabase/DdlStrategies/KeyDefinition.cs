namespace ChinookDatabase.DdlStrategies
{
    /// <summary>
    /// Indicates where the key definition should be: on table create, on alter table, etc.
    /// </summary>
    public enum KeyDefinition
    {
        OnCreateTableColumn,
        OnCreateTableBottom,
        OnAlterTable
    }
}
