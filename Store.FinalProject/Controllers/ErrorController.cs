using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.FinalProject.Errors;

namespace Store.FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorController : ControllerBase
    {
        
        public IActionResult Error()
        {
            return  NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound,"Not Found"));
        }
    }
}
