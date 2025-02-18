using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CartService.Service;
using System.Collections.Generic;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartServiceClass _service;

        public CartController(CartServiceClass service)
        {
            _service = service;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Cart Service is running!");
        }

        [HttpGet("{customerId}/items")]
        public async Task<IActionResult> GetCartItems(int customerId)
        {
            var books = await _service.GetCartItems(customerId);
            return books.Count > 0 ? Ok(books) : NotFound("No books in cart.");
        }

        [HttpPost("{customerId}/items/{bookId}/{quantity}")]
        public async Task<IActionResult> AddToCart(int customerId, int bookId, int quantity)
        {
            bool success = await _service.AddToCart(customerId, bookId, quantity);
            return success ? Ok("Book added to cart.") : BadRequest("Invalid book ID.");
        }

        [HttpDelete("{customerId}/items/{bookId}")]
        public async Task<IActionResult> RemoveFromCart(int customerId, int bookId)
        {
            bool success = await _service.RemoveFromCart(customerId, bookId);
            return success ? Ok("Book removed from cart.") : NotFound("Book not found in cart.");
        }
    }
}
