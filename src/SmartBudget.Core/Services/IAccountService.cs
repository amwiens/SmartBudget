using SmartBudget.Core.Models;

using System.Threading.Tasks;

namespace SmartBudget.Core.Services
{
    public interface IAccountService : IDataService<Account>
    {
        Task<Account> GetWithTransactions(int accountId);
    }
}