using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBudget.Core.Models
{
    public enum AccountType
    {
        Card,
        Bank,
        Credit
    }

    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

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
    }
}