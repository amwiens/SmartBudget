using AutoCompleteTextBox.Editors;

using SmartBudget.Core.Factories;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SmartBudget.Core.Providers
{
    public class PayeeSuggestionProvider : Button, ISuggestionProvider
    {
        public IPayeeService _payeeService;

        public static readonly DependencyProperty ListOfPayeesProperty =
            DependencyProperty.Register(
                "ListOfPayees", typeof(IEnumerable<Payee>),
                typeof(PayeeSuggestionProvider));
        public IEnumerable<Payee> ListOfPayees
        {
            get { return (IEnumerable<Payee>)GetValue(ListOfPayeesProperty); }
            set
            {
                SetValue(ListOfPayeesProperty, value);
                Payees = value;
            }
        }

        public IEnumerable<Payee> Payees { get; set; }

        public PayeeSuggestionProvider()
        {
            Payees = ListOfPayees;
            //ListOfPayees = payees;
        }

        //public PayeeSuggestionProvider(IPayeeService payeeService)
        //{
        //    _payeeService = payeeService;
        //    ListOfPayees = _payeeService.GetAll().Result;
        //}

        public Payee GetExactSuggestion(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            return Payees
                .FirstOrDefault(p => string.Equals(p.Name, filter, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<Payee> GetSuggestions(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            System.Threading.Thread.Sleep(1000);
            return Payees
                .Where(p => p.Name.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) > -1)
                .ToList();
        }

        IEnumerable ISuggestionProvider.GetSuggestions(string filter)
        {
            return GetSuggestions(filter);
        }
    }
}