using game_store_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.CustomModel
{
    public class PostDiscountBody
    {
        public Discount Discount { get; set; }

        public List<string> ListGameDiscount { get; set; }
    }
}
