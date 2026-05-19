using System;

namespace TheMatchaClubDomain.Models
{
    /// <summary>
    /// Represents a single line item within an Order.
    /// Tracks what product was purchased, at what price, and in what quantity.
    /// </summary>
    public class OrderItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal LineTotal => UnitPrice * Quantity;
    }
}
