using System.Windows;
using System.Windows.Input;
using Expense_Tracker.Models;
using Expense_Tracker.ViewModels;

namespace Expense_Tracker.Views
{
    public partial class EditExpenseWindow : Window
    {
        public EditExpenseWindow(bool isNewExpense = false)
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

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
