using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.MasterData;
using RingSoft.DevLogix.Sqlite;
using RingSoft.DevLogix.SqlServer;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public enum SetFocusControls
    {
        OrganizationName = 0,
        FileName = 1
    }

    public enum OrganizationProcesses
    {
        Add = 0,
        Edit = 1,
        Connect = 2
    }

    public interface IAddEditOrganizationView : IDbLoginView
    {
        void SetFocus(SetFocusControls control);

        void SetPlatform();

        Organization Organization { get; set; }

        OrganizationProcesses OrganizationProcess { get; set; }
    }


    public class AddEditOrganizationViewModel : DbLoginViewModel<Organization>
    {
        public new IAddEditOrganizationView View { get; private set; }
        protected override string TestTable => "SystemMaster";
        public AddEditOrganizationViewModel()
        {
            DbName = "DevLogix";
        }

        public override void Initialize(IDbLoginView view, DbLoginProcesses loginProcess, SqliteLoginViewModel sqliteLoginViewModel,
            SqlServerLoginViewModel sqlServerLoginViewModel, Organization entity)
        {
            if (view is IAddEditOrganizationView addEditOrganizationView)
            {
                View = addEditOrganizationView;
            }

            base.Initialize(view, loginProcess, sqliteLoginViewModel, sqlServerLoginViewModel, entity);

            base.Initialize(view, loginProcess, sqliteLoginViewModel, sqlServerLoginViewModel, entity);
        }

        public override void LoadFromEntity(Organization entity)
        {
            EntityName = entity.Name;
            DbPlatform = (DbPlatforms)entity.Platform;
            var directory = entity.FilePath;
            if (!directory.IsNullOrEmpty() && !directory.EndsWith("\\"))
            {
                directory += "\\";
            }

            SqliteLoginViewModel.FilenamePath = $"{directory}{entity.FileName}";
            SqlServerLoginViewModel.Server = entity.Server;
            SqlServerLoginViewModel.Database = entity.Database;
            if (entity.AuthenticationType != null)
                SqlServerLoginViewModel.SecurityType = (SecurityTypes)entity.AuthenticationType.Value;
            SqlServerLoginViewModel.UserName = entity.Username;
            SqlServerLoginViewModel.Password = entity.Password.DecryptDatabasePassword();
        }

        protected override void ShowEntityNameFailure()
        {
            var message = $"Organization Name must have a value";
            ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid Organization Name", RsMessageBoxIcons.Exclamation);
            View.SetFocus(SetFocusControls.OrganizationName);

        }

        protected override void SaveEntity(Organization entity)
        {
            if (Object != null)
            {
                entity.Id = Object.Id;
            }
            entity.Name = EntityName;
            entity.FilePath = SqliteLoginViewModel.FilePath;
            entity.FileName = SqliteLoginViewModel.FileName;
            entity.Platform = (byte)DbPlatform;
            entity.Server = SqlServerLoginViewModel.Server;
            entity.Database = SqlServerLoginViewModel.Database;
            entity.AuthenticationType = (byte)SqlServerLoginViewModel.SecurityType;
            entity.Username = SqlServerLoginViewModel.UserName;
            entity.Password = SqlServerLoginViewModel.Password.EncryptDatabasePassword();

        }

        protected override bool PreDataCopy(ref LookupContext context, ref DbDataProcessor destinationProcessor, ITwoTierProcedure procedure)
        {
            DbContext destinationDbContext = null;
            IDevLogixDbContext sourceDbContext = null;
            context = AppGlobals.LookupContext;
            switch (DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    destinationProcessor = AppGlobals.LookupContext.SqliteDataProcessor;
                    LoadDbDataProcessor(destinationProcessor);
                    var sqliteHomeLogixDbContext = new DevLogixSqliteDbContext();
                    sqliteHomeLogixDbContext.SetLookupContext(AppGlobals.LookupContext);
                    destinationDbContext = sqliteHomeLogixDbContext;
                    break;
                case DbPlatforms.SqlServer:
                    var sqlServerProcessor = AppGlobals.LookupContext.SqlServerDataProcessor;
                    destinationProcessor = sqlServerProcessor;
                    LoadDbDataProcessor(destinationProcessor);
                    var sqlServerContext = new DevLogixSqlServerDbContext();
                    sqlServerContext.SetLookupContext(AppGlobals.LookupContext);
                    destinationDbContext = sqlServerContext;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }

            switch (OriginalDbPlatform)
            {
                case DbPlatforms.Sqlite:
                    sourceDbContext = new DevLogixSqliteDbContext(AppGlobals.LookupContext);
                    break;
                case DbPlatforms.SqlServer:
                    sourceDbContext = new DevLogixSqlServerDbContext(AppGlobals.LookupContext);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            AppGlobals.LoadDataProcessor(Object, OriginalDbPlatform);
            var systemMaster = new SystemMaster() { OrganizationName = Object.Name + "1" };
            sourceDbContext.SystemMaster.Add(systemMaster);
            try
            {
                sourceDbContext.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var dropResult = destinationProcessor.DropDatabase();
            if (dropResult.ResultCode != GetDataResultCodes.Success)
            {
                procedure.ShowError(dropResult.Message, "Error Dropping Database");
                return false;
            }

            sourceDbContext = AppGlobals.GetNewDbContext();
            sourceDbContext.SetLookupContext(AppGlobals.LookupContext);
            AppGlobals.LookupContext.Initialize(sourceDbContext, OriginalDbPlatform);


            var result = AppGlobals.MigrateContext(AppGlobals.GetNewDbContext().DbContext);
            if (!result.IsNullOrEmpty())
            {
                procedure.ShowError(result, "File Access Error");
                return false;
            }

            result = AppGlobals.MigrateContext(destinationDbContext);
            if (!result.IsNullOrEmpty())
            {
                procedure.ShowError(result, "File Access Error");
                return false;
            }
            return true;
        }
    }
}
