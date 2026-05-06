using Company.Core.DTOs.Basket;
using Company.Core.Entities.Basket;
using Company.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Company.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepo;

        public BasketController(IBasketRepository basketRepo)
        {
            _basketRepo = basketRepo;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var basket = await _basketRepo.GetBasketAsync(email);

            return Ok(basket ?? new CustomerBasket { Id = email });
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basketDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var basket = new CustomerBasket
            {
                Id = email,
                Items = basketDto.Items.Select(i => new BasketItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    PictureUrl = i.PictureUrl,
                    Brand = i.Brand,
                    Type = i.Type
                }).ToList()
            };

            var updated = await _basketRepo.UpdateBasketAsync(basket);

            return Ok(updated);
        }

        [Authorize]
        [HttpDelete]
        public async Task DeleteBasket()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            await _basketRepo.DeleteBasketAsync(email);
        }
    }
}