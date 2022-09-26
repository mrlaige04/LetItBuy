using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Data.Entities
{
    public class CartItem
    {
        public Cart? Cart { get; set; }

        public Guid CartItemID { get; set; }
        public Item Item { get; set; }
    }
}
