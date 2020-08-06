using AutoCompleteTextBox.Editors;

using SmartBudget.Core.Factories;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SmartBudget.Core.Providers
{
    public class PayeeSuggestionProvider : ISuggestionProvider
    {
        public IPayeeService _payeeService;
        public IEnumerable<Payee> ListOfPayees { get; set; }

        public PayeeSuggestionProvider()
        {
            var payees = PayeeFactory.CreatePayeeList();
            ListOfPayees = payees;
        }

        //public PayeeSuggestionProvider(IPayeeService payeeService)
        //{
        //    _payeeService = payeeService;
        //    ListOfPayees = _payeeService.GetAll().Result;
        //}

        public Payee GetExactSuggestion(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            return ListOfPayees
                .FirstOrDefault(p => string.Equals(p.Name, filter, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<Payee> GetSuggestions(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            System.Threading.Thread.Sleep(1000);
            return ListOfPayees
                .Where(p => p.Name.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) > -1)
                .ToList();
        }

        IEnumerable ISuggestionProvider.GetSuggestions(string filter)
        {
            return GetSuggestions(filter);
        }
    }
}