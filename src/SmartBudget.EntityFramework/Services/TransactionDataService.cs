using Microsoft.EntityFrameworkCore;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;
using SmartBudget.EntityFramework.Services.Common;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBudget.EntityFramework.Services
{
    public class TransactionDataService : ITransactionService
    {
        private readonly SmartBudgetDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Transaction> _nonQueryDataService;

        public TransactionDataService(SmartBudgetDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Transaction>(contextFactory);
        }

        public async Task<Transaction> Create(Transaction entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Transaction> Get(int id)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                Transaction entity = await context.Transactions
                    .Include(t => t.Account)
                    .Include(t => t.Payee)
                    .FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Transaction> entities = await context.Transactions
                    .Include(t => t.Payee)
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<Transaction>> GetByAccountId(int accountId)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                //IEnumerable<Transaction> entities = new
                IEnumerable<Transaction> ownEntities = await context.Transactions
                    .Include(t => t.Payee)
                    .Where(e => e.AccountId == accountId)
                    .ToListAsync();
                IEnumerable<Transaction> targetEntities = await context.Transactions
                    .Include(t => t.Payee)
                    .Where(e => e.TargetAccountId == accountId)
                    .ToListAsync();

                var entities = ownEntities.Concat(targetEntities);
                return entities;
            }
        }

        public async Task<IEnumerable<Transaction>> GetByTargetAccountId(int accountId)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Transaction> entities = await context.Transactions
                    .Where((e) => e.TargetAccountId == accountId)
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<Transaction> Update(int id, Transaction entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }
    }
}