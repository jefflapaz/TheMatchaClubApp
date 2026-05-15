using System;

namespace TheMatchaClubApp.Core.Models
{
    public class CartLine
    {
        public Product Product { get; }
        public int Qty { get; set; }
        public decimal Total => Product.Price * Qty;

        public CartLine(Product product, int qty)
        {
            Product = product;
            Qty = qty;
        }
    }
}
