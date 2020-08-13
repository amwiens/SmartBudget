using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBudget.Core.Models
{
    public class TransactionCategory : DomainObject
    {
        [ForeignKey(nameof(Transaction)), Column(Order = 0)]
        public int TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }

        [ForeignKey(nameof(Category)), Column(Order = 1)]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public decimal Amount { get; set; }
    }
}