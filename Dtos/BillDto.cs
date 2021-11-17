using game_store_be.Models;
using game_store_be.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Dtos
{
    public class BillDto
    {
        public string IdBill { get; set; }
        public DateTime? DatePay { get; set; }
        public double Cost { get; set; }
        public string Actions { get; set; }
        public BillDiscount DiscountApplied { get; set; }

        public GameDto Game { get; set; }
        public UserDto User { get; set; }
    }
}
