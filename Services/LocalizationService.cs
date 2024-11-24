using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Expense_Tracker.Services
{
    public static class LocalizationService
    {
        public static void SetLanguage(string language)
        {
            try
            {
                var culture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                // Обновляем словарь ресурсов
                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri($"/Resources/Localization.{language}.xaml", UriKind.Relative)
                };

                // Находим и заменяем словарь локализации
                var app = Application.Current;
                var oldDict = app.Resources.MergedDictionaries[0];
                app.Resources.MergedDictionaries.RemoveAt(0);
                app.Resources.MergedDictionaries.Insert(0, resourceDictionary);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при смене языка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
