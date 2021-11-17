using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class Transaction
    {
        public string MasterCardNumberSend { get; set; }
        public string MasterCardNumberReceive { get; set; }
        public string TransactionMessage { get; set; }
        public double AmountOfMoney { get; set; }
        public double Fee { get; set; }
    }
}
