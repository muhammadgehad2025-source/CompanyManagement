using Company.Core.DTOs.Identity;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace Company.Core.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }

        public DateTimeOffset Date { get; set; }

        public decimal Total { get; set; }

        public string Status { get; set; }

        public AddressDto ShippingAddress { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }
}