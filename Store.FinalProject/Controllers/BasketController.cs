using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entites;
using Store.Core.Entites.Dtos.Basket;
using Store.Core.Repositories.Contract;
using Store.FinalProject.Errors;

namespace Store.FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>>GetBasket(string id)
        {
            if(id== null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket == null) return new CustomerBasket()
            {
                Id = id
            };
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>>UpdateBasket(CustomerBasketDto model)
        {
            var basket = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustomerBasket>(model));
            if (basket == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(basket);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string basketid)
        {
            if (string.IsNullOrEmpty(basketid))
                return BadRequest(new ApiErrorResponse(400, "Basket ID is required"));

            await _basketRepository.DeleteBasketAsync(basketid);

            return NoContent();   // ← 204 No Content (الصحيح)
        }

      /*  [HttpDelete]
        public async Task DeleteBasket(string basketid)
        {
            await _basketRepository.DeleteBasketAsync(basketid);
        }
      */
    }
}
