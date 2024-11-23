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
using System.Data.SQLite;

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

    public class DataBase 
    {
        private string connectionString = "Data Source=storage/storage.db;Version=3;";
        SQLiteConnection connection;

        private string insertQuery = "INSERT INTO expenses (Title, Description, Category, Cost, Created) VALUES (@Title, @Description, @Category, @Cost, @Created)";
        private string deleteQuery = "DELETE FROM expenses WHERE id = @id";

        public void ConnectDataBase()
        {
            using (connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("SQLite connection successfull");

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS expenses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Category TEXT NOT NULL,
                    Cost INTEGER NOT NULL, 
                    Created DATETIME DEFAULT CURRENT_TIMESTAMP
                );
            ";
                SQLiteCommand createTableCom = new SQLiteCommand(createTableQuery, connection);

                createTableCom.ExecuteNonQuery();

                Console.WriteLine("Table \'expenses\' is ready");
            }

            
        }

        public void Insert(Expense expense) 
        {
            SQLiteCommand insertCom = new SQLiteCommand(insertQuery, connection);
            insertCom.Parameters.AddWithValue("@title", expense.Title);
            insertCom.Parameters.AddWithValue("@descripion", expense.Description);
            insertCom.Parameters.AddWithValue("@created", expense.Created);
            insertCom.Parameters.AddWithValue("@cost", expense.Cost);
            insertCom.Parameters.AddWithValue("@created", expense.Created);
            insertCom.ExecuteNonQuery();
            

            Console.WriteLine("Expense inserted successfully.");
        }


        public void Delete(int expenseId) 
        {
            SQLiteCommand deleteCom = new SQLiteCommand(deleteQuery, connection);
            deleteCom.Parameters.AddWithValue(deleteQuery, expenseId);
            deleteCom.ExecuteNonQuery();

            Console.WriteLine("Deleted expense with id = " + expenseId);
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
