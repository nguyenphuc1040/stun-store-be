using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class BillModel
    {
        
        public string Id { get; set; }
        public UserModel User { get; set; }
        public GameModel Game { get; set; }
        public string ActionPayment { get; set; }
        public string At_day { get; set; }
        public long CostBill { get; set; }
    }
}
