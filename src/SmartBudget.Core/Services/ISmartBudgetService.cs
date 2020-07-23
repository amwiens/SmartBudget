using SmartBudget.Core.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.Core.Services
{
    public interface ISmartBudgetService
    {
        Task<Account> AddAccountAsync(Account account);
        Task<Account> UpdateAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(Account account);
        IReadOnlyCollection<Account> GetAccounts();
    }
}