using Company.Core.Entities;
using Company.Core.Entities.Order;
using Company.Core.Interfaces;
using Company.Core.Interfaces.Services;
using Company.Core.Specifications;
using Company.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public OrderService(
            IBasketRepository basketRepo,
            IGenericRepository<Product> productRepo,
            IUnitOfWork unitOfWork,
            IAuthService authService)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId)
        {
            // 1️⃣ Get basket
            var basket = await _basketRepo.GetBasketAsync(basketId);

            if (basket == null || basket.Id != buyerEmail)
                return null;

            // 2️⃣ Get user address
            var userAddress = await _authService.GetUserAddressAsync(buyerEmail);

            if (userAddress == null)
                return null;

            var orderItems = new List<OrderItem>();

            // 3️⃣ Loop basket items
            foreach (var item in basket.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);

                if (product == null)
                    return null;

                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl
                };

                orderItems.Add(new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = product.Price,
                    Quantity = item.Quantity
                });
            }

            if (!orderItems.Any())
                return null;

            // 4️⃣ Calculate subtotal
            var subtotal = orderItems.Sum(i => i.Price * i.Quantity);

            // 5️⃣ Create order
            var order = new Order
            {
                BuyerEmail = buyerEmail,
                OrderItems = orderItems,
                Subtotal = subtotal,
                ShipToAddress = new Address
                {
                    FirstName = userAddress.FirstName,
                    LastName = userAddress.LastName,
                    Street = userAddress.Street,
                    City = userAddress.City,
                    Country = userAddress.Country
                }
            };

            // 6️⃣ Save
            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.CompleteAsync();

            // 7️⃣ Delete basket
            await _basketRepo.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetByIdAsync(orderId);

            if (order == null || order.BuyerEmail != buyerEmail)
                return null;

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsSpecification(buyerEmail);

            var orders = await _unitOfWork
                .Repository<Order>()
                .GetAllWithSpecAsync(spec);

            return orders
                .Where(o => o.BuyerEmail == buyerEmail)
                .ToList();
        }
    }
}