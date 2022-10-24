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
            if (Processor is IAppDbMaintenanceProcessor appDbMaintenanceProcessor)
            {
                appDbMaintenanceProcessor.WindowReadOnlyMode = true;
                ReadOnlyMode = true;
            }
            base.Initialize();
        }
    }
}
