using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Expense_Tracker.Commands;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Expense_Tracker.Services;
using Expense_Tracker.ViewModels;
using Microsoft.EntityFrameworkCore;
using MaterialDesignThemes.Wpf;

namespace Expense_Tracker.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;
        private ObservableCollection<Expense> _expenses;
        private ObservableCollection<string> _categories;
        private string _selectedCategory;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private decimal _totalAmount;
        private bool _isDarkTheme;

        public MainViewModel(AppDbContext context)
        {
            _context = context;
            LoadExpensesCommand = new RelayCommand(LoadExpenses);
            AddExpenseCommand = new RelayCommand(AddExpense);
            EditExpenseCommand = new RelayCommand<Expense>(EditExpense);
            DeleteExpenseCommand = new RelayCommand<Expense>(DeleteExpense);
            ResetFiltersCommand = new RelayCommand(ResetFilters);
            ToggleThemeCommand = new RelayCommand(ToggleTheme);

            LoadExpenses();
            LoadCategories();
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
                    ApplyTheme();
                }
            }
        }

        public ICommand LoadExpensesCommand { get; }
        public ICommand AddExpenseCommand { get; }
        public ICommand EditExpenseCommand { get; }
        public ICommand DeleteExpenseCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        public ObservableCollection<Expense> Expenses
        {
            get => _expenses;
            set
            {
                _expenses = value;
                OnPropertyChanged();
                UpdateFilteredExpenses();
            }
        }

        public ObservableCollection<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                UpdateFilteredExpenses();
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
                UpdateFilteredExpenses();
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
                UpdateFilteredExpenses();
            }
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Expense> _filteredExpenses;
        public ObservableCollection<Expense> FilteredExpenses
        {
            get => _filteredExpenses;
            set
            {
                _filteredExpenses = value;
                OnPropertyChanged();
                UpdateTotalAmount();
            }
        }

        private void LoadExpenses()
        {
            var expenses = _context.Expenses.ToList();
            Expenses = new ObservableCollection<Expense>(expenses);
            FilteredExpenses = new ObservableCollection<Expense>(expenses);
        }

        private void LoadCategories()
        {
            var categories = EditExpenseViewModel.PredefinedCategories.ToList();
            var dbCategories = _context.Expenses
                .Select(e => e.Category)
                .Distinct()
                .Where(c => !string.IsNullOrEmpty(c))
                .ToList();

            foreach (var category in dbCategories)
            {
                if (!categories.Contains(category))
                {
                    categories.Add(category);
                }
            }

            Categories = new ObservableCollection<string>(categories);
        }

        private void UpdateFilteredExpenses()
        {
            var filtered = Expenses.AsEnumerable();

            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                filtered = filtered.Where(e => e.Category == SelectedCategory);
            }

            if (StartDate.HasValue)
            {
                filtered = filtered.Where(e => e.Date >= StartDate.Value);
            }

            if (EndDate.HasValue)
            {
                filtered = filtered.Where(e => e.Date <= EndDate.Value);
            }

            FilteredExpenses = new ObservableCollection<Expense>(filtered);
        }

        private void AddExpense()
        {
            var expense = new Expense { Date = DateTime.Now };
            var dialog = new Views.EditExpenseWindow(expense);
            
            if (dialog.ShowDialog() == true)
            {
                _context.Expenses.Add(expense);
                _context.SaveChanges();
                
                // Обновляем списки
                if (!Categories.Contains(expense.Category))
                {
                    Categories.Add(expense.Category);
                }
                
                Expenses.Insert(0, expense);
                UpdateFilteredExpenses();
            }
        }

        private void EditExpense(Expense expense)
        {
            var dialog = new Views.EditExpenseWindow(expense);
            if (dialog.ShowDialog() == true)
            {
                _context.SaveChanges();
                
                if (!Categories.Contains(expense.Category))
                {
                    Categories.Add(expense.Category);
                }
                
                UpdateFilteredExpenses();
            }
        }

        private void DeleteExpense(Expense expense)
        {
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                _context.SaveChanges();
                Expenses.Remove(expense);
                UpdateFilteredExpenses();
            }
        }

        private void ResetFilters()
        {
            SelectedCategory = null;
            StartDate = null;
            EndDate = null;
        }

        private void ToggleTheme()
        {
            IsDarkTheme = !IsDarkTheme;
        }

        private void ApplyTheme()
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(IsDarkTheme ? 
                Theme.Dark : 
                Theme.Light);
        
            paletteHelper.SetTheme(theme);
        }

        private void UpdateTotalAmount()
        {
            TotalAmount = FilteredExpenses.Sum(e => e.Amount);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
