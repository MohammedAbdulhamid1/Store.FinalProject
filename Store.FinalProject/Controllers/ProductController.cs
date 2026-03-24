using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entites.Dtos;
using Store.Core.Helper;
using Store.Core.Services.Contract;
using Store.Core.Specification;
using Store.FinalProject.Errors;
using Store.Repository.Specification;
using Store.Service.Services;

namespace Store.FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProductAsync([FromQuery]ProductSpecParams productSpec)
        {
            
            var result=await productService.GetAllProductsAsync(productSpec);
            return Ok(result );
        }
        [Authorize]
        [HttpGet("Brands")]
        public async Task<IActionResult> GetAllBrandsAsync()
        {
            var x = await productService.GetAllBrandsAsync();
            return Ok(x);
        }
        [HttpGet("Types")]
        [Authorize]
        public async Task<IActionResult> GetAllTypesAsync()
        {
            var x = await productService.GetAllTypesAsync();
            return Ok(x);
        }
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            if (id == null) return BadRequest(new ApiErrorResponse(400));
            var x = await productService.GetProductById(id);
            if (x == null) return NotFound(404);
            return Ok(x);
        }



    }
}
