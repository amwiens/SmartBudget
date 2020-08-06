using SmartBudget.Core.Models;

using System;
using System.Collections.Generic;

namespace SmartBudget.Core.Factories
{
    public static class PayeeFactory
    {
        private static Lazy<List<Payee>> payees = new Lazy<List<Payee>>(GetPayeeList);

        private static List<Payee> GetPayeeList()
        {
            return new List<Payee>
            {
                new Payee { Id = 1, Name = "Carissa Wiens" },
                new Payee { Id = 8, Name = "Debbie Goedtel" }
            };
        }

        public static IList<Payee> CreatePayeeList()
        {
            return payees.Value;
        }
    }
}