using System.Windows;
using Expense_Tracker.ViewModels;

namespace Expense_Tracker.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
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
