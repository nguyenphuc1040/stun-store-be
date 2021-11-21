using game_store_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.CustomModel
{
    public class PostBillBody
    {
        public Card Card { get; set; }
        public Bill NewBill { get; set; }
    }
}
