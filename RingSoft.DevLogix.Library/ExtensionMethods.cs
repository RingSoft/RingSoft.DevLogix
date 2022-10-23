using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DevLogix.Library
{
    public static class ExtensionMethods
    {
        public static bool HasRight(this TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            return AppGlobals.Rights.HasRight(tableDefinition, rightType);
        }
    }
}
