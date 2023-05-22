using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.Sqlite;
using RingSoft.DevLogix.Tests.UserManagement;

namespace RingSoft.DevLogix.Tests
{
    public class TestGlobals<TViewModel, TView> : IControlsUserInterface where TViewModel : DbMaintenanceViewModelBase
        where TView : IDbMaintenanceView, new()
    {
        public TestDataRepository DataRepository { get; private set; } = new TestDataRepository();
        public TViewModel ViewModel { get; private set; }

        public TView View { get; private set; }

        public void Initialize()
        {
            AppGlobals.UnitTesting = true;
            AppGlobals.Initialize();
            AppGlobals.DataRepository = DataRepository;
            AppGlobals.LookupContext.Initialize(new DevLogixSqliteDbContext(), DbPlatforms.Sqlite);
            AppGlobals.MainViewModel = new MainViewModel();
            ControlsGlobals.UserInterface = this;

            var viewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel));
            ViewModel = viewModel;

            var view = (TView)Activator.CreateInstance(typeof(TView));
            View = view;

            ViewModel.OnViewLoaded(View);
        }

        public void ClearData()
        {
            ViewModel.NewCommand.Execute(null);
            DataRepository.ClearData();
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            
        }

        public void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            
        }

        public MessageBoxButtonsResult ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxButtonsResult.Yes;
        }

        public MessageBoxButtonsResult ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxButtonsResult.Yes;
        }
    }
}
