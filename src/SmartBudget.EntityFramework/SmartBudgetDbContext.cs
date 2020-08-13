using Microsoft.EntityFrameworkCore;

using SmartBudget.Core.Models;

using System.IO;

namespace SmartBudget.EntityFramework
{
    public class SmartBudgetDbContext : DbContext
    {
        public static SmartBudgetDbContext Create(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                var fs = File.Create(dbPath);
                fs.Close();
            }
            SmartBudgetDbContext ctx = new SmartBudgetDbContext(dbPath);
            ctx.Database.Migrate();
            return ctx;
        }

        protected SmartBudgetDbContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        public SmartBudgetDbContext(DbContextOptions<SmartBudgetDbContext> options, string dbPath)
            : base(options)
        {
            _dbPath = dbPath;
        }

        private readonly string _dbPath;

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Payee> Payees { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<TransactionCategory> TransactionCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite($"Filename={_dbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}