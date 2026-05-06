using AutoMapper;
using Company.Core.DTOs.Order;
using Company.Core.Entities.Order;
using Company.Core.Interfaces.Services;
using Company.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Company.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto dto)
        {
            // 🔥 Extract user email from JWT
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            // 🔥 Call service
            var order = await _orderService.CreateOrderAsync(email, dto.BasketId);

            if (order == null)
                return BadRequest("Problem creating order");

            return Ok(order);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var orders = await _orderService.GetOrdersForUserAsync(email);

            var ordersdto = _mapper.Map<IReadOnlyList<OrderDto>>(orders);
            return Ok(ordersdto);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order == null)
                return NotFound();

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }
    }
}