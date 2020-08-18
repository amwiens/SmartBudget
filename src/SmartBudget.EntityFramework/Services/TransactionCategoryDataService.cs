using Microsoft.EntityFrameworkCore;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;
using SmartBudget.EntityFramework.Services.Common;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBudget.EntityFramework.Services
{
    public class TransactionCategoryDataService : ITransactionCategoryService
    {
        private readonly SmartBudgetDbContextFactory _contextFactory;
        private readonly NonQueryDataService<TransactionCategory> _nonQueryDataService;

        public TransactionCategoryDataService(SmartBudgetDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<TransactionCategory>(contextFactory);
        }

        public async Task<TransactionCategory> Create(TransactionCategory entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<bool> DeleteByTransactionId(int transactionId)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                var entities = await GetByTransactionId(transactionId);
                if (entities.Count() > 0)
                {
                    foreach (var entity in entities)
                    {
                        context.Set<TransactionCategory>().Remove(entity);
                        await context.SaveChangesAsync();
                    }
                }

                return true;
            }
        }

        public async Task<TransactionCategory> Get(int id)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                TransactionCategory entity = await context.TransactionCategories
                    .FirstOrDefaultAsync(c => c.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<TransactionCategory>> GetByTransactionId(int transactionId)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<TransactionCategory> entities = await context.TransactionCategories
                    .Where(t => t.TransactionId == transactionId)
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<TransactionCategory>> GetAll()
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<TransactionCategory> entities = await context.TransactionCategories
                    .Include(x => x.Category)
                    .Include(x => x.Transaction)
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<TransactionCategory> Update(int id, TransactionCategory entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }
    }
}