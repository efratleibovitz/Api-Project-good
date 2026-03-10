using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static WebApiShop.Controllers.UsersController;
using Entities;
using Repository;
using Services;
using DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        IProductService _productService ;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<productDto>>> Get([FromQuery] int position, [FromQuery] int skip, [FromQuery] int? productId,
            [FromQuery] string? name, [FromQuery] float? minPrice, [FromQuery] float? maxPrice, [FromQuery] int[]? categoryIds, [FromQuery] string? descripion)
        {
           
            List<productDto> product= await _productService.GetProducts(position,skip, productId, name,  minPrice, maxPrice, categoryIds,descripion);
            if (product == null)
                   return NoContent();
            return Ok(product);
        }
    


    }
}
