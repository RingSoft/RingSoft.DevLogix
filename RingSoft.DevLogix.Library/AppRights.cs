using System.Collections.Generic;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library
{
    public class Right
    {
        public TableDefinitionBase TableDefinition { get; set; }

        public bool HasRight { get; set; }
    }
    public class AppRights
    {
        public List<Right> Rights { get; set; }

        public AppRights()
        {
            Rights = new List<Right>();
            foreach (var tableDefinition in AppGlobals.LookupContext.TableDefinitions)
            {
                if (tableDefinition.PrimaryKeyFields[0].ParentJoinForeignKeyDefinition != null)
                {
                    Rights.Add(new Right
                    {
                        TableDefinition = tableDefinition,
                        HasRight = false
                    });
                }
            }
        }

        public void LoadRights(User user)
        {
        }
    }
}
