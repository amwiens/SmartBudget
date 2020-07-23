using SmartBudget.Core.DataAccess;

using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBudget.Core.Services
{
    public class SmartBudgetService : ISmartBudgetService
    {
        public SmartBudgetService()
        {
            this.context = App.CreateDatabase();
        }

        private SmartBudgetContext context;
        public static string ActionAdd => "add";
        public static string ActionUpdate => "update";
        public static string ActionDelete => "delete";
    }
}