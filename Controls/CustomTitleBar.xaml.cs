using System.Windows;
using System.Windows.Controls;

namespace Expense_Tracker.Controls
{
    public partial class CustomTitleBar : UserControl
    {
        public CustomTitleBar()
        {
            InitializeComponent();
            Loaded += CustomTitleBar_Loaded;
        }

        private void CustomTitleBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is Window window)
            {
                WindowTitle.Text = window.Title;
                window.StateChanged += Window_StateChanged;
            }
        }

        private void Window_StateChanged(object? sender, System.EventArgs e)
        {
            if (sender is Window window)
            {
                MaximizeIcon.Kind = window.WindowState == WindowState.Maximized
                    ? MaterialDesignThemes.Wpf.PackIconKind.WindowRestore
                    : MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
            }
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void MaximizeRestoreWindow_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is Window window)
            {
                window.WindowState = window.WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is Window window)
            {
                window.Close();
            }
        }
    }
}
