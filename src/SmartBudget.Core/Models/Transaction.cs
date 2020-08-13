using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey(nameof(Payee)), Column(Order = 2)]
        public int PayeeId { get; set; }

        public Payee Payee { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public bool IsCleared { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Note { get; set; }

        [ForeignKey(nameof(Account)), Column(Order = 0)]
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        [ForeignKey(nameof(TargetAccount)), Column(Order = 1)]
        public int? TargetAccountId { get; set; }

        public virtual Account? TargetAccount { get; set; }

        [InverseProperty(nameof(TransactionCategory.Transaction))]
        public virtual ICollection<TransactionCategory> TransactionCategories { get; set; }

        [NotMapped]
        public bool IsIncome => TransactionType == TransactionType.Income;

        [NotMapped]
        public bool IsTransferIncome => TargetAccountId == WorkingAccountId;

        [NotMapped]
        public int WorkingAccountId { get; set; }
    }
}