using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBudget.EntityFramework.Services
{
    public class SmartBudgetService : ISmartBudgetService
    {
        public SmartBudgetService()
        {
            this.context = App.CreateDatabase();
        }

        private readonly SmartBudgetDbContext context;
        public static string ActionAdd => "add";
        public static string ActionUpdate => "update";
        public static string ActionDelete => "delete";

        public Account GetAccountItem(int id)
        {
            return context.Accounts
                .Where(a => a.Id == id)
                .SingleOrDefault();
        }

        public async Task<Account> AddAccountAsync(Account account)
        {
            var entity = this.context.Accounts.Add(account);
            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw e;
            }
            Account storedItem = GetAccountItem(entity.Entity.Id);
            //MessagingCenter.Send<ISmartBudgetService, Account>(this, ActionAdd, storedItem);
            return storedItem;
        }

        public async Task<Account> UpdateAccountAsync(Account account)
        {
            var localDto = this.context.Accounts.SingleOrDefault(a => a.Id == account.Id);
            var entity = this.context.Accounts.Update(localDto);
            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw e;
            }
            Account storedItem = GetAccountItem(entity.Entity.Id);
            //MessagingCenter.Send<ISmartBudgetService, Account>(this, ActionUpdate, storedItem);
            return storedItem;
        }

        public async Task<bool> DeleteAccountAsync(Account account)
        {
            if (account == null)
                return false;

            var localDto = this.context.Accounts.SingleOrDefault(a => a.Id == account.Id);
            if (localDto == null)
                return false;

            this.context.Accounts.Remove(localDto);
            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw e;
            }
            //MessagingCenter.Send<ISmartBudgetService, Account>(this, ActionDelete, account);
            return true;
        }

        public IReadOnlyCollection<Account> GetAccounts()
        {
            return context.Accounts
                .ToArray();
        }
    }
}