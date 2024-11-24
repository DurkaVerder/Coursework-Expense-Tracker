using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Globalization;
using System.Threading;
using Expense_Tracker.Commands;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Expense_Tracker.Views;
using MaterialDesignThemes.Wpf;

namespace Expense_Tracker.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;
        private readonly ICollectionView _expensesView;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _selectedCategory;
        private bool _isDarkTheme;
        private readonly CultureInfo _ruCulture;

        public MainViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            // Создаем и настраиваем русскую культуру
            _ruCulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = _ruCulture;
            Thread.CurrentThread.CurrentUICulture = _ruCulture;

            // Загружаем расходы из базы данных
            Expenses = new ObservableCollection<Expense>(_dbContext.Expenses.ToList());
            _expensesView = CollectionViewSource.GetDefaultView(Expenses);
            _expensesView.Filter = ExpenseFilter;

            // Инициализируем команды
            AddExpenseCommand = new RelayCommand(AddExpense);
            EditExpenseCommand = new RelayCommand<Expense>(EditExpense);
            DeleteExpenseCommand = new RelayCommand<Expense>(DeleteExpense);
            ClearFiltersCommand = new RelayCommand(ClearFilters);
            ToggleThemeCommand = new RelayCommand(ToggleTheme);

            // Подписываемся на изменения коллекции
            Expenses.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalAmountString));
            _expensesView.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalAmountString));
        }

        public ObservableCollection<Expense> Expenses { get; }

        public string TotalAmountString => _expensesView.Cast<Expense>().Sum(e => e.Amount).ToString("C", _ruCulture);

        public string[] Categories => new[]
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

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                    _expensesView.Refresh();
                }
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                    _expensesView.Refresh();
                }
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    _expensesView.Refresh();
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

        public ICommand AddExpenseCommand { get; }
        public ICommand EditExpenseCommand { get; }
        public ICommand DeleteExpenseCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        private void AddExpense()
        {
            var expense = new Expense { Date = DateTime.Today };
            var viewModel = new EditExpenseViewModel(expense);
            var window = new EditExpenseWindow { DataContext = viewModel };

            if (window.ShowDialog() == true)
            {
                _dbContext.Expenses.Add(expense);
                _dbContext.SaveChanges();
                Expenses.Add(expense);
            }
        }

        private void EditExpense(Expense expense)
        {
            if (expense == null) return;

            var viewModel = new EditExpenseViewModel(expense);
            var window = new EditExpenseWindow { DataContext = viewModel };

            if (window.ShowDialog() == true)
            {
                _dbContext.SaveChanges();
                _expensesView.Refresh();
            }
        }

        private void DeleteExpense(Expense expense)
        {
            if (expense == null) return;

            _dbContext.Expenses.Remove(expense);
            _dbContext.SaveChanges();
            Expenses.Remove(expense);
        }

        private void ClearFilters()
        {
            StartDate = null;
            EndDate = null;
            SelectedCategory = null;
            OnPropertyChanged(nameof(TotalAmountString));
        }

        private void ToggleTheme()
        {
            IsDarkTheme = !IsDarkTheme;
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(IsDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }

        private bool ExpenseFilter(object obj)
        {
            if (obj is not Expense expense)
                return false;

            // Фильтр по дате начала
            if (StartDate.HasValue && expense.Date.Date < StartDate.Value.Date)
                return false;

            // Фильтр по дате окончания
            if (EndDate.HasValue && expense.Date.Date > EndDate.Value.Date)
                return false;

            // Фильтр по категории
            if (!string.IsNullOrEmpty(SelectedCategory) && expense.Category != SelectedCategory)
                return false;

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(StartDate) ||
                propertyName == nameof(EndDate) ||
                propertyName == nameof(SelectedCategory))
            {
                OnPropertyChanged(nameof(TotalAmountString));
            }
        }
    }
}
