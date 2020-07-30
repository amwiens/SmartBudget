using Microsoft.EntityFrameworkCore;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;
using SmartBudget.EntityFramework.Services.Common;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.EntityFramework.Services
{
    public class AccountDataService : IDataService<Account>
    {
        private readonly SmartBudgetDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Account> _nonQueryDataService;

        public AccountDataService(SmartBudgetDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Account>(contextFactory);
        }

        public async Task<Account> Create(Account entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Account> Get(int id)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                Account entity = await context.Accounts
                    .FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Account> entities = await context.Accounts
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<Account> Update(int id, Account entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }
    }
}