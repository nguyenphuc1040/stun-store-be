using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class GameModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float AverageRate { get; set; }
        public string Developer { get; set; }

        public string Publisher { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Flatform { get; set; }

        public long Cost { get; set; }

        public DiscountModel Discount { get; set; }
    }
}
