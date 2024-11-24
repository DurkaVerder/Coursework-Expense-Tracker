using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Expense_Tracker.Commands;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Expense_Tracker.Services;
using Microsoft.EntityFrameworkCore;
using MaterialDesignThemes.Wpf;

namespace Expense_Tracker.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;
        private readonly Settings _settings;
        private ObservableCollection<Expense> _expenses;
        private ObservableCollection<string> _categories;
        private ObservableCollection<Expense> _filteredExpenses;
        private string _selectedCategory;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private decimal _totalAmount;
        private bool _isDarkTheme;

        public MainViewModel()
        {
            _context = new AppDbContext();
            _settings = Services.SettingsService.LoadSettings();
            
            // Инициализация коллекций
            _expenses = new ObservableCollection<Expense>();
            _categories = new ObservableCollection<string>();
            _filteredExpenses = new ObservableCollection<Expense>();
            
            // Команды
            AddNewExpenseCommand = new RelayCommand(AddExpense);
            EditExistingExpenseCommand = new RelayCommand<Expense>(EditExpense);
            RemoveExpenseCommand = new RelayCommand<Expense>(DeleteExpense);
            ClearFiltersCommand = new RelayCommand(ResetFilters);
            OpenSettingsCommand = new RelayCommand(OpenSettings);

            // Загрузка данных
            LoadExpenses();
            LoadCategories();

            // Применяем сохраненную тему
            ApplyTheme();
        }

        public ICommand AddNewExpenseCommand { get; }
        public ICommand EditExistingExpenseCommand { get; }
        public ICommand RemoveExpenseCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand OpenSettingsCommand { get; }

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

        public string CurrentCurrency => _settings.Currency;

        public bool IsDarkTheme
        {
            get => _settings.IsDarkTheme;
            set
            {
                if (_settings.IsDarkTheme != value)
                {
                    _settings.IsDarkTheme = value;
                    OnPropertyChanged();
                    ApplyTheme();
                }
            }
        }

        private void LoadExpenses()
        {
            var expenses = _context.Expenses.OrderByDescending(e => e.Date).ToList();
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
            try
            {
                var expense = new Expense 
                { 
                    Date = DateTime.Now,
                    Title = "Новый расход",
                    Category = "Прочее",
                    Amount = 0
                };
                
                var viewModel = new EditExpenseViewModel(expense);
                var dialog = new Views.EditExpenseWindow { DataContext = viewModel };
                dialog.Owner = Application.Current.MainWindow;
                
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        _context.Expenses.Add(expense);
                        _context.SaveChanges();
                        
                        // Обновляем списки
                        if (!string.IsNullOrEmpty(expense.Category) && !Categories.Contains(expense.Category))
                        {
                            Categories.Add(expense.Category);
                        }
                        
                        Expenses.Insert(0, expense);
                        UpdateFilteredExpenses();
                        UpdateTotalAmount();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании окна: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditExpense(Expense expense)
        {
            if (expense == null) return;
            
            var viewModel = new EditExpenseViewModel(expense);
            var dialog = new Views.EditExpenseWindow { DataContext = viewModel };
            dialog.Owner = Application.Current.MainWindow;
            
            if (dialog.ShowDialog() == true)
            {
                _context.SaveChanges();
                
                if (!Categories.Contains(expense.Category))
                {
                    Categories.Add(expense.Category);
                }
                
                UpdateFilteredExpenses();
                UpdateTotalAmount();
            }
        }

        private void DeleteExpense(Expense expense)
        {
            if (expense == null) return;

            _context.Expenses.Remove(expense);
            _context.SaveChanges();
            
            Expenses.Remove(expense);
            UpdateFilteredExpenses();
            UpdateTotalAmount();
        }

        private void ResetFilters()
        {
            SelectedCategory = null;
            StartDate = null;
            EndDate = null;
        }

        private void ApplyTheme()
        {
            try
            {
                var paletteHelper = new PaletteHelper();
                var theme = paletteHelper.GetTheme();
                theme.SetBaseTheme(_settings.IsDarkTheme ? Theme.Dark : Theme.Light);
                paletteHelper.SetTheme(theme);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при применении темы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenSettings()
        {
            try
            {
                var viewModel = new SettingsViewModel(_settings);
                var dialog = new Views.SettingsWindow { DataContext = viewModel };
                dialog.Owner = Application.Current.MainWindow;

                if (dialog.ShowDialog() == true)
                {
                    ApplyTheme();
                    Services.LocalizationService.SetLanguage(_settings.Language);
                    Services.SettingsService.SaveSettings(_settings);
                    OnPropertyChanged(nameof(CurrentCurrency));
                    OnPropertyChanged(nameof(FilteredExpenses));
                    OnPropertyChanged(nameof(TotalAmount));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии настроек: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTotalAmount()
        {
            TotalAmount = FilteredExpenses.Sum(e => e.Amount);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
