using System.Windows;
using Expense_Tracker.Data;
using Expense_Tracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker
{
    public partial class App : Application
    {
        private AppDbContext _dbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Загружаем сохраненные настройки
            var settings = SettingsService.LoadSettings();

            // Применяем язык
            LocalizationService.SetLanguage(settings.Language);

            // Инициализируем базу данных
            _dbContext = new AppDbContext();
            _dbContext.Database.Migrate();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _dbContext?.Dispose();
            base.OnExit(e);
        }
    }
}
