using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBudget.Core.Models
{
    public class Category : DomainObject
    {
        [Required]
        public string Name { get; set; }

        public string Note { get; set; }

        public int? MainCategory { get; set; }

        [InverseProperty(nameof(TransactionCategory.Category))]
        public virtual ICollection<TransactionCategory> TransactionCategories { get; set; }
    }
}