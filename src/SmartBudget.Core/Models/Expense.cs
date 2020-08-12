using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBudget.Core.Models
{
    public enum ExpenseRecurrence
    {
        OneTime,
        Daily,
        DailyWithoutWeekend,
        Weekly,
        Monthly,
        Yearly,
        Biweekly,
        Bimonthly,
        Quarterly,
        Biannually
    }

    public class Expense : DomainObject
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsEndless { get; set; }

        public decimal Amount { get; set; }

        public ExpenseRecurrence Recurrence { get; set; }

        [NotMapped]
        public string Note { get; set; }
    }
}