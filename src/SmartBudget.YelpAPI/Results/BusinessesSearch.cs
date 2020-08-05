using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBudget.YelpAPI.Results
{
    public class BusinessesSearch
    {
        public int Total { get; set; }

        public IEnumerable<Business> Businesses { get; set; }

        public Region Region { get; set; }
    }

    public class Business
    {
        public IEnumerable<Category> Categories { get; set; }

        public Coordinates Coordinates { get; set; }

        public string DisplayPhone { get; set; }

        public decimal Distance { get; set; }

        public string Id { get; set; }

        public string Alias { get; set; }

        public string ImageUrl { get; set; }

        public bool IsClosed { get; set; }

        public Location Location { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Price { get; set; }

        public decimal Rating { get; set; }

        public int ReviewCount { get; set; }

        public string Url { get; set; }

        public string[] Transactions { get; set; }
    }

    public class Category
    {
        public string Alias { get; set; }
        public string Title { get; set; }
    }

    public class Coordinates
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class Location
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string[] DisplayAddress { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class Region
    {
        public Center Center { get; set; }
    }

    public class Center
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}