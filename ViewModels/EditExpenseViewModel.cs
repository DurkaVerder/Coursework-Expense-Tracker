using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Expense_Tracker.Commands;
using Expense_Tracker.Models;

namespace Expense_Tracker.ViewModels
{
    public class EditExpenseViewModel : INotifyPropertyChanged
    {
        private readonly Expense _expense;
        private readonly bool _isNewExpense;

        public static readonly string[] PredefinedCategories = new[]
        {
            "Продукты",
            "Транспорт",
            "Жилье",
            "Развлечения",
            "Здоровье",
            "Одежда",
            "Образование",
            "Путешествия",
            "Подарки",
            "Прочее"
        };

        public EditExpenseViewModel(Expense expense, bool isNewExpense = false)
        {
            _expense = expense ?? throw new ArgumentNullException(nameof(expense));
            _isNewExpense = isNewExpense;
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave()
        {
            if (string.IsNullOrWhiteSpace(Title))
                return false;

            if (Amount <= 0)
                return false;

            return true;
        }

        public string Title
        {
            get => _expense.Title;
            set
            {
                if (_expense.Title != value)
                {
                    _expense.Title = value;
                    OnPropertyChanged();
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string Description
        {
            get => _expense.Description ?? string.Empty;
            set
            {
                if (_expense.Description != value)
                {
                    _expense.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Amount
        {
            get => _expense.Amount;
            set
            {
                if (_expense.Amount != value)
                {
                    _expense.Amount = value;
                    OnPropertyChanged();
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string Category
        {
            get => _expense.Category;
            set
            {
                if (_expense.Category != value)
                {
                    _expense.Category = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => _expense.Date;
            set
            {
                if (_expense.Date != value)
                {
                    _expense.Date = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] Categories => PredefinedCategories;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler<bool>? CloseRequested;

        public string WindowTitle => _isNewExpense ? "Добавление расхода" : "Редактирование расхода";

        private void Save()
        {
            try
            {
                if (!CanSave())
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля корректно.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Category))
                {
                    Category = "Прочее";
                }

                CloseRequested?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
