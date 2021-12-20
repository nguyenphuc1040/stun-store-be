using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Game = new HashSet<Game>();
        }

        public string IdDiscount { get; set; }
        public double? PercentDiscount { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Game> Game { get; set; }
    }
}
