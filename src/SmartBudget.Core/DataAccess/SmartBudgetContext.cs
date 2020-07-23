using Microsoft.EntityFrameworkCore;

namespace SmartBudget.Core.DataAccess
{
    public class SmartBudgetContext : DbContext
    {
        public static SmartBudgetContext Create(string dbPath)
        {
            SmartBudgetContext ctx = new DataAccess.SmartBudgetContext(dbPath);
            ctx.Database.EnsureCreated();
            ctx.Database.Migrate();
            //SeedData.AddSampleData(ctx);
            return ctx;
        }

        protected SmartBudgetContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        private string _dbPath;



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}