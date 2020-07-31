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

        public DateTime EndDate { get; set; }

        public decimal StartingAmount { get; set; }

        public decimal PaidAmount { get; set; }

        [InverseProperty(nameof(Transaction.Account))]
        public virtual ICollection<Transaction> Transactions { get; set; }

        [InverseProperty(nameof(Transaction.TargetAccount))]
        public virtual ICollection<Transaction> TargetTransactions { get; set; }

        [NotMapped]
        public decimal Balance
        {
            get
            {
                if (Transactions.Count > 0)
                {
                    var expenses = Transactions.Where(t => t.TransactionType == TransactionType.Expense).Sum(x => x.Amount);
                    var income = Transactions.Where(t => t.TransactionType == TransactionType.Income).Sum(x => x.Amount);
                    return StartingAmount - expenses + income;
                }
                else
                    return StartingAmount;
            }
        }
    }
}