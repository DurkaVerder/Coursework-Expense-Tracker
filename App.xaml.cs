using System;
using System.Windows;
using Expense_Tracker.Data;
using Expense_Tracker.ViewModels;
using Expense_Tracker.Views;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker
{
    public partial class App : Application
    {
        private AppDbContext? _dbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _dbContext = new AppDbContext();
            _dbContext.Database.Migrate();

            var mainViewModel = new MainViewModel(_dbContext);
            var mainWindow = new MainWindow { DataContext = mainViewModel };

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _dbContext?.Dispose();
        }
    }
}
