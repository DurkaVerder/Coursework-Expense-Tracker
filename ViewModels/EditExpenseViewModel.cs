using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Expense_Tracker.Commands;
using Expense_Tracker.Models;

namespace Expense_Tracker.ViewModels
{
    public class EditExpenseViewModel : INotifyPropertyChanged
    {
        private readonly Models.Expense _expense;

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

        public EditExpenseViewModel(Models.Expense expense)
        {
            _expense = expense;
            
            Title = expense.Title;
            Description = expense.Description;
            Category = expense.Category;
            Amount = expense.Amount;
            Date = expense.Date;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
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
                }
            }
        }

        public string Description
        {
            get => _expense.Description;
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

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                Title = "Без названия";
            }

            if (string.IsNullOrWhiteSpace(Category))
            {
                Category = "Прочее";
            }

            CloseRequested?.Invoke(this, true);
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
