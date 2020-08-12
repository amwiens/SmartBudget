using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartBudget.Core.Models
{
    public enum AccountType
    {
        Card,
        Bank,
        Credit
    }

    public class Account : DomainObject
    {
        [Required]
        public string Name { get; set; }

        public AccountType AccountType { get; set; }

        public bool Favorite { get; set; }

        public string AccountNumber { get; set; }

        public string CardNumber { get; set; }

        public decimal Rate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal StartingAmount { get; set; }

        public decimal PaidAmount { get; set; }

        [InverseProperty(nameof(Transaction.Account))]
        public virtual ICollection<Transaction> Transactions { get; set; }

        [InverseProperty(nameof(Transaction.TargetAccount))]
        public virtual ICollection<Transaction> TargetTransactions { get; set; }

        [NotMapped]
        public string Accured { get; set; }

        [NotMapped]
        public string Status { get; set; }

        [NotMapped]
        public decimal BlockedAmount { get; set; }

        [NotMapped]
        public string ExpirationDate { get; set; }

        [NotMapped]
        public decimal Amount { get; set; }

        [NotMapped]
        public decimal Balance
        {
            get
            {
                var expenses = 0.0M;
                var income = 0.0M;
                var transfersFrom = 0.0M;
                var transfersTo = 0.0M;

                if (Transactions != null && Transactions.Count > 0)
                {
                    expenses = Transactions.Where(t => t.TransactionType == TransactionType.Expense).Sum(x => x.Amount);
                    income = Transactions.Where(t => t.TransactionType == TransactionType.Income).Sum(x => x.Amount);
                    transfersFrom = Transactions.Where(t => t.TransactionType == TransactionType.Transfer).Sum(x => x.Amount);
                }
                if (TargetTransactions != null && TargetTransactions.Count > 0)
                {
                    transfersTo = TargetTransactions.Where(t => t.TransactionType == TransactionType.Transfer).Sum(x => x.Amount);
                }

                return StartingAmount - expenses + income - transfersFrom + transfersTo;
            }
        }
    }
}