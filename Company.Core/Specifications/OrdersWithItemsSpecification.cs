using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Company.Core.Entities.Order;

namespace Company.Core.Specifications
{
    public class OrdersWithItemsSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsSpecification(string email)
            : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
        }

        public OrdersWithItemsSpecification(int id, string email)
            : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
        }
    }
}
