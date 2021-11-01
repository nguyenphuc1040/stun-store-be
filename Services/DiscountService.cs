using game_store_be.Interfaces;
using game_store_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace game_store_be.Services
{
    public class DiscountService : IDiscountService
    {
        List<DiscountModel> DISCOUNTS = new List<DiscountModel>()
        {
            new DiscountModel()
            {
                Id =  Guid.NewGuid(),
                Percent = 50,
                StartDay = new DateTime(),
                EndDay = new DateTime()
            },
            new DiscountModel()
            {
                Id =  Guid.NewGuid(),
                Percent = 20,
                StartDay = new DateTime(),
                EndDay = new DateTime()
            },
            new DiscountModel()
            {
                Id =  Guid.NewGuid(),
                Percent = 10,
                StartDay = new DateTime(),
                EndDay = new DateTime()
            },

        };

        public DiscountModel AddDiscount(DiscountModel newDiscount)
        {
            newDiscount.Id = Guid.NewGuid();
            DISCOUNTS.Add(newDiscount);
            return newDiscount;
        }

        public DiscountModel GetDiscount(Guid id)
        {
            return DISCOUNTS.SingleOrDefault(discount => discount.Id == id);
        }

        public List<DiscountModel> GetDiscounts()
        {
            return DISCOUNTS;
        }

        public DiscountModel UpdateDiscount(DiscountModel newDiscont)
        {
            DiscountModel existDiscount = GetDiscount(newDiscont.Id);
            existDiscount.Percent = newDiscont.Percent;
            return existDiscount;
        }

        public bool DeleteDiscount(Guid id)
        {
            DiscountModel existDiscount = GetDiscount(id);
            if (existDiscount != null)
            {
                DISCOUNTS.Remove(existDiscount);
                return true;
            }
            return false;
        }
    }
}
