using System.Windows;
using Expense_Tracker.Models;
using Expense_Tracker.ViewModels;

namespace Expense_Tracker.Views
{
    public partial class EditExpenseWindow : Window
    {
        public EditExpenseWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditExpenseViewModel viewModel)
            {
                viewModel.CloseRequested += (s, result) =>
                {
                    DialogResult = result;
                    Close();
                };
            }
        }
    }
}