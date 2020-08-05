using System.ComponentModel.DataAnnotations;

namespace SmartBudget.Core.Models
{
    public class Payee : DomainObject
    {
        [Required]
        public string Name { get; set; }

        public string YelpId { get; set; }

        public string ImageUrl { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string DisplayAddress { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Phone { get; set; }
    }
}