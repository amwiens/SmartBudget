using SmartBudget.Core.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.Core.Services
{
    public interface ITransactionService : IDataService<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByAccountId(int accountId);

        Task<IEnumerable<Transaction>> GetByTargetAccountId(int accountId);
    }
}