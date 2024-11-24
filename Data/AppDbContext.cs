using Expense_Tracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Expense_Tracker.Data
{
    public class AppDbContext : DbContext
    {
        private static readonly string DbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ExpenseTracker",
            "expenses.db");

        public DbSet<Expense> Expenses => Set<Expense>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Path.GetDirectoryName(DbPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder!);
            }

            optionsBuilder
                .UseSqlite($"Data Source={DbPath}")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Expense>()
                .Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");
        }

        public AppDbContext()
        {
            var exists = File.Exists(DbPath);
            Database.EnsureCreated();
            if (!exists)
            {
                // MessageBox.Show($"База данных создана: {DbPath}");
            }
        }
    }
}
