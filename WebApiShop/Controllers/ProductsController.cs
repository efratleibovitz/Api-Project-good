using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<productDto>>> Get([FromQuery] int position, [FromQuery] int skip, [FromQuery] int? productId,
            [FromQuery] string? name, [FromQuery] float? minPrice, [FromQuery] float? maxPrice, [FromQuery] int[]? categoryIds, [FromQuery] string? descripion)
        {
            List<productDto> product = await _productService.GetProducts(position, skip, productId, name, minPrice, maxPrice, categoryIds, descripion);
            if (product == null) return NoContent();
            return Ok(product);
        }

        [HttpPost("rate")]
        [Authorize]
        public async Task<IActionResult> RateProduct([FromQuery] int userId, [FromQuery] int productId, [FromQuery] int value)
        {
            if (!await _productService.CanUserRateAsync(userId, productId))
                return BadRequest("You have already rated this product today.");

            await _productService.AddRatingAsync(userId, productId, value);
            return Ok("Rating submitted.");
        }
    }
}
