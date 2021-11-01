using game_store_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Interfaces
{
    public interface IDiscountService
    {
        List<DiscountModel> GetDiscounts();
        DiscountModel GetDiscount(Guid id);
        DiscountModel AddDiscount(DiscountModel newDiscount);
        DiscountModel UpdateDiscount(DiscountModel newDiscont);
        bool DeleteDiscount(Guid id);
    }
}
