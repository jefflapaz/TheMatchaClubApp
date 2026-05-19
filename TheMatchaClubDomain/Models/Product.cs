using System;

namespace TheMatchaClubDomain.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public int SalesCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
