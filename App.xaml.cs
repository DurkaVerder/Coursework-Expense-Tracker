using System.Windows;
using Expense_Tracker.Data;
using Expense_Tracker.ViewModels;
using Expense_Tracker.Views;

namespace Expense_Tracker
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var dbContext = new AppDbContext();
            var mainViewModel = new MainViewModel(dbContext);
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
