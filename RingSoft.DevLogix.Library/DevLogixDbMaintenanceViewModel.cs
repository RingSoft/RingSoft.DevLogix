using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.App.Library;

namespace RingSoft.DevLogix.Library
{
    public abstract class DevLogixDbMaintenanceViewModel<TEntity> : AppDbMaintenanceViewModel<TEntity> where TEntity : new()
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Processor is IAppDbMaintenanceProcessor appDbMaintenanceProcessor)
            {
                if (!TableDefinitionBase.HasRight(RightTypes.AllowEdit))
                {
                    appDbMaintenanceProcessor.WindowReadOnlyMode = true;
                    ReadOnlyMode = true;
                }
            }
        }
    }
}
