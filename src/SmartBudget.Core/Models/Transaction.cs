using System;

namespace SmartBudget.Core.Models
{
    public enum TransactionType
    {
        Income,
        Expense,
        Transfer
    }

    public class Transaction : DomainObject
    {
        public string Payee { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public bool IsCleared { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Note { get; set; }

        public virtual Account ChargedAccount { get; set; }

        public virtual Account? TargetAccount { get; set; }
    }
}