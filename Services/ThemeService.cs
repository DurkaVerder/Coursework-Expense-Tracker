using System.Windows;

namespace Expense_Tracker.Services
{
    public class ThemeService
    {
        private const string LightTheme = "/Themes/LightTheme.xaml";
        private const string DarkTheme = "/Themes/DarkTheme.xaml";
        private bool _isDarkTheme;

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            private set => _isDarkTheme = value;
        }

        public void ToggleTheme()
        {
            var app = Application.Current;
            var existingDict = app.Resources.MergedDictionaries[0];
            var source = existingDict.Source.ToString();
            var newTheme = source!.Contains("Dark") ? LightTheme : DarkTheme;
            
            var newDict = new ResourceDictionary
            {
                Source = new Uri(newTheme, UriKind.Relative)
            };
            
            app.Resources.MergedDictionaries[0] = newDict;
            IsDarkTheme = !IsDarkTheme;
        }
    }
}
