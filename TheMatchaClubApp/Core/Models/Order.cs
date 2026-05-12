using System;
using System.Collections.Generic;

namespace TheMatchaClubApp.Core.Models
{
    public class Order
    {
        public string OrderId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public decimal Subtotal { get; set; }
        public decimal VatAmount { get; set; }
        public decimal Total { get; set; }
        public bool IsDineIn { get; set; }
        public string OrderType { get; set; } = "Dine-In"; // "Dine-In" or "Take-Out"
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new();
    }
}
