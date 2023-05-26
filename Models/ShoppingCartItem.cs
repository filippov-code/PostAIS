using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostAIS.Models
{
    public class ShoppingCartItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid ProductPurchaseServiceId { get; set; }
        public int Count { get; set; }

        public ShoppingCartItem()
        {

        }

        public ShoppingCartItem(Product product, int count)
        {
            Product = product;
            Count = count;
        }
    }
}
