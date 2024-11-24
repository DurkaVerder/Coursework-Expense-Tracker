using System.Windows;
using System.Windows.Controls;

namespace Expense_Tracker.Controls
{
    public static class WindowCommands
    {
        public static void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(sender as Button) is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        public static void MaximizeRestoreWindow(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(sender as Button) is Window window)
            {
                window.WindowState = window.WindowState == WindowState.Maximized 
                    ? WindowState.Normal 
                    : WindowState.Maximized;
            }
        }

        public static void CloseWindow(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(sender as Button) is Window window)
            {
                window.Close();
            }
        }
    }
}
