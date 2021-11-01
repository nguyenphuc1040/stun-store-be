using game_store_be.Interfaces;
using game_store_be.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace game_store_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : Controller
    {
        private IDiscountService discountService;
        public DiscountController(IDiscountService _discountService)
        {
            discountService = _discountService;
        }

        [HttpGet]
        public IActionResult GetDiscounts()
        {
            return Ok(discountService.GetDiscounts());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDiscount(Guid id)
        {
            DiscountModel existDiscount = discountService.GetDiscount(id);
            if (existDiscount != null)
            {
                return Ok(existDiscount);
            }
            return NotFound(new { message = $"Discount {id} not found" });
        }

        [HttpPost]
        [Route("create")]
        public IActionResult AddDiscount(DiscountModel newDiscount)
        {
            return Ok(discountService.AddDiscount(newDiscount));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteDiscount(Guid id)
        {
            bool isDeleted = discountService.DeleteDiscount(id);
            if (isDeleted)
            {
                return Ok(new { message = "Delete discount success" });
            }
            return NotFound(new { message = $"Discount {id} not found" });
        }

        [HttpPatch]
        [Route("update/{id}")]
        public IActionResult UpdateDiscount(Guid id, DiscountModel newDiscount)
        {
            newDiscount.Id = id;
            DiscountModel discountUpdated = discountService.UpdateDiscount(newDiscount);
            if (discountUpdated != null)
            {
                return Ok(discountUpdated);
            }
            return NotFound(new { message = $"Discount {id} not found" });
        }
    }
}
