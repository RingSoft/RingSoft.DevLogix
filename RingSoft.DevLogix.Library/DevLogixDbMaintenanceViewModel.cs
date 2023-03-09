﻿using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.App.Library;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.Library
{
    public abstract class DevLogixDbMaintenanceViewModel<TEntity> : AppDbMaintenanceViewModel<TEntity> where TEntity : new()
    {
        public virtual bool SetReadOnlyMode { get; } = true;
        protected override void Initialize()
        {
            base.Initialize();
            if (Processor is IAppDbMaintenanceProcessor appDbMaintenanceProcessor)
            {
                if (TableDefinitionBase.HasRight(RightTypes.AllowEdit))
                {
                    SaveButtonEnabled = true;
                }
                else
                {
                    appDbMaintenanceProcessor.WindowReadOnlyMode = SetReadOnlyMode;
                    ReadOnlyMode = SetReadOnlyMode;
                }
            }
        }

        protected override bool CheckKeyValueTextChanged()
        {
            if (!TableDefinition.HasRight(RightTypes.AllowEdit))
            {
                return true;
            }
            return base.CheckKeyValueTextChanged();
        }
    }
}
