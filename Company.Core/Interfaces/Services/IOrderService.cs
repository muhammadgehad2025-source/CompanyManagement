using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Company.Core.Entities.Order;

namespace Company.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId);

       
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail);
    }
}