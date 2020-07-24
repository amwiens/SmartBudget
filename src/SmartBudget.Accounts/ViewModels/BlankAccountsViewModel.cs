using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using System;

namespace SmartBudget.Accounts.ViewModels
{
    public class BlankAccountsViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand AddCardAccountCommand { get; private set; }
        public DelegateCommand AddBankAccountCommand { get; private set; }

        public BlankAccountsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            AddCardAccountCommand = new DelegateCommand(AddCardAccount);
            AddBankAccountCommand = new DelegateCommand(AddBankAccount);
        }

        private void AddCardAccount()
        {
            throw new NotImplementedException();
        }

        private void AddBankAccount()
        {
            throw new NotImplementedException();
        }
    }
}