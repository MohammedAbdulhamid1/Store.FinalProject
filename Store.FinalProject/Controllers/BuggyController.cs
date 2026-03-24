using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entites.Context;
using Store.FinalProject.Errors;
using System.Reflection.Metadata.Ecma335;

namespace Store.FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreDbContext context;

        public BuggyController(StoreDbContext context)
        {
            this.context = context;
        }
        [HttpGet("NotFound")]
        public async Task<IActionResult> GetNotFoundRequestError()
        {
            var brand = await context.ProductBrands.FindAsync(100);
            if (brand is null) return NotFound(new ApiErrorResponse(404));
            return Ok(brand);
            
        }
        //[HttpGet("Servererror")]
        //public async Task<IActionResult> GetServerError()
        //{
        //    var brand = await context.ProductBrands.FindAsync(100);
        //    var x = brand.ToString();
        //    return Ok(brand);

        //}
        [HttpGet("badrequest")]
        public async Task<IActionResult> GetBadRequestError()
        {
            return BadRequest(new ApiErrorResponse(400));

        }
        [HttpGet("badrequest/{id}")]
        public async Task<IActionResult> GetBadRequestError(int id)
        {
            return  BadRequest();

        }
        [HttpGet("unauthorized")]
        public async Task<IActionResult> GetunauthorizedError()
        {
            return Unauthorized(new ApiErrorResponse(401));

        }
    }
}
