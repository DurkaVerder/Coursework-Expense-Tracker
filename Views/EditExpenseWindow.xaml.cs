using System.Windows;
using Expense_Tracker.Models;
using Expense_Tracker.ViewModels;

namespace Expense_Tracker.Views
{
    public partial class EditExpenseWindow : Window
    {
        public EditExpenseWindow(Expense expense)
        {
            InitializeComponent();
            var viewModel = new EditExpenseViewModel(expense);
            viewModel.CloseRequested += (sender, dialogResult) =>
            {
                DialogResult = dialogResult;
                Close();
            };
            DataContext = viewModel;
        }
    }
}
