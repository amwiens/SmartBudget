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
            //if (!ctx.Database.EnsureCreated())
            ctx.Database.Migrate();
            //SeedData.AddSampleData(ctx);
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite($"Filename={_dbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}