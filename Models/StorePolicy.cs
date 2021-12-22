using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public partial class StorePolicy
    {
        public string IdStorePolicy { get; set; }
        public string NamePolicy { get; set; }
        public string Content { get; set; }
        public int DigitValue { get; set; }
    }
}