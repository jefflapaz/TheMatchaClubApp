namespace TheMatchaClubApp.Models
{
    public class ProductItem
    {
        public string Id { get; }
        public string Name { get; }
        public string Category { get; }
        public decimal Price { get; }
        public int Stock { get; }

        public ProductItem(string id, string name, string category, decimal price, int stock)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Stock = stock;
        }
    }

    public class CartLine
    {
        public ProductItem Product { get; }
        public int Qty { get; set; }

        public CartLine(ProductItem product, int qty)
        {
            Product = product;
            Qty = qty;
        }

        public decimal Total => Product.Price * Qty;
    }

    public class MockOrder
    {
        public string OrderNo { get; }
        public string Time { get; }
        public string Date { get; }
        public string Customer { get; }
        public string ItemsSummary { get; }
        public string Type { get; }
        public decimal Total { get; }

        public MockOrder(string orderNo, string time, string date, string customer,
                         string itemsSummary, string type, decimal total)
        {
            OrderNo = orderNo;
            Time = time;
            Date = date;
            Customer = customer;
            ItemsSummary = itemsSummary;
            Type = type;
            Total = total;
        }
    }

    public class MockCustomer
    {
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public int TotalOrders { get; }
        public decimal LifetimeValue { get; }
        public string LastVisit { get; }

        public MockCustomer(string name, string email, string phone,
                            int totalOrders, decimal lifetimeValue, string lastVisit)
        {
            Name = name;
            Email = email;
            Phone = phone;
            TotalOrders = totalOrders;
            LifetimeValue = lifetimeValue;
            LastVisit = lastVisit;
        }
    }
}
