using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Expense_Tracker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class Expense
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }
        public int Cost { get; set; }

        public DateTime Created { get; set; }

        public Expense(string title, string description, string category, int cost)
        {
            this.Title = title;
            this.Description = description;
            this.Category = category;
            this.Cost = cost;
        }
    }

}
