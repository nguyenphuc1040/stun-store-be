using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class Card
    {
        public string MasterCardNumber { get; set; }
        public string MasterCardName { get; set; }
        public int MasterCardCCV { get; set; }
        public string MasterCardExpire { get; set; }
    }
}
