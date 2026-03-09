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
        public async Task<ActionResult<List<productDto>>> Get(int? pId, string? name, int position, int skip, float? minPrice, float? maxPrice, string? desc, int?[] categoryIds)
        {
           
            List<productDto> product= await _productService.GetProducts(pId, name, position, skip, minPrice, maxPrice, desc, categoryIds);
            if (product == null)
                   return NoContent();
            return Ok(product);
        }
    


    }
}
