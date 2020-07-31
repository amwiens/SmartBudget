using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}