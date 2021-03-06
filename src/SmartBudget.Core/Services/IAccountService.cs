﻿using SmartBudget.Core.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.Core.Services
{
    public interface IAccountService : IDataService<Account>
    {
        Task<Account> GetWithTransactions(int accountId);

        Task<IEnumerable<Account>> GetAllWithTransactions();

        Task<IEnumerable<Account>> GetAccountDataBeforeDateByType(DateTime date, AccountType accountType);
    }
}