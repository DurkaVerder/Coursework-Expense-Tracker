using System;
using System.Windows.Input;
using Expense_Tracker.Commands;
using Expense_Tracker.Models;
using MaterialDesignThemes.Wpf;

namespace Expense_Tracker.ViewModels
{
    public class SettingsViewModel
    {
        private readonly Settings _settings;

        public event EventHandler<bool> CloseRequested;

        public SettingsViewModel(Settings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public string Currency
        {
            get => _settings.Currency;
            set => _settings.Currency = value;
        }

        public string Language
        {
            get => _settings.Language;
            set => _settings.Language = value;
        }

        public bool IsDarkTheme
        {
            get => _settings.IsDarkTheme;
            set => _settings.IsDarkTheme = value;
        }

        public string[] AvailableCurrencies => Settings.AvailableCurrencies;
        public string[] AvailableLanguages => Settings.AvailableLanguages;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save()
        {
            CloseRequested?.Invoke(this, true);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }
    }
}
