using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Core
{
    /// <summary>
    /// Centralized profit calculation utility.
    /// Currently uses an estimated margin. When a real Cost field is added
    /// to the Product model, update EstimateProfit to use actual cost data.
    /// </summary>
    public static class ProfitCalculator
    {
        /// <summary>
        /// Default estimated profit margin (65%). Replace with actual
        /// cost-based calculation when inventory costing is implemented.
        /// </summary>
        private const decimal DefaultMargin = 0.65m;

        /// <summary>
        /// Estimates profit for a given revenue amount using the default margin.
        /// </summary>
        public static decimal EstimateProfit(decimal revenue)
        {
            return revenue * DefaultMargin;
        }

        /// <summary>
        /// Estimates profit for a product based on its price and quantity sold.
        /// When a Cost field is added to Product, replace with: (Price - Cost) * quantity
        /// </summary>
        public static decimal EstimateProductProfit(Product product, int quantitySold)
        {
            // Future: return (product.Price - product.Cost) * quantitySold;
            return product.Price * quantitySold * DefaultMargin;
        }

        /// <summary>
        /// Calculates total estimated profit from a collection of orders.
        /// </summary>
        public static decimal EstimateOrdersProfit(IEnumerable<Order> orders)
        {
            return EstimateProfit(orders.Sum(o => o.Total));
        }
    }
}
