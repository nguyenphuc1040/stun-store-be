using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class Bill
    {
        public string IdBill { get; set; }
        public string IdGame { get; set; }
        public string IdUser { get; set; }
        public DateTime? DatePay { get; set; }
        public double Cost { get; set; }
        public string Actions { get; set; }
        public string Discount { get; set; }

        public virtual Game IdGameNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
