using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Expense_Tracker.Models
{
    public class Settings : INotifyPropertyChanged
    {
        private string _currency = "₽";
        private string _language = "ru-RU";
        private bool _isDarkTheme;

        public string Currency
        {
            get => _currency;
            set
            {
                if (_currency != value)
                {
                    _currency = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (_isDarkTheme != value)
                {
                    _isDarkTheme = value;
                    OnPropertyChanged();
                }
            }
        }

        public static string[] AvailableCurrencies => new[]
        {
            "₽", "$", "€", "£", "¥"
        };

        public static string[] AvailableLanguages => new[]
        {
            "ru-RU", "en-US"
        };

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
