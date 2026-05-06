using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Company.Core.Entities.Order
{
    public class Order
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public Address ShipToAddress { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();

        public decimal Subtotal { get; set; }

        public string Status { get; set; } = "Pending";
    }
}