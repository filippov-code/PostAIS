using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostAIS.Models
{
    public class ProductPurchaseService : Service
    {
        public override OperationType OperationType => OperationType.Purchase;
        public List<ShoppingCartItem> ShoppingCart { get; set; } = new();

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var cartItem in ShoppingCart)
                sb.Append($"{cartItem.Product?.Title}: {cartItem.Count} шт.\n");
            return sb.ToString();
        }
    }
}
