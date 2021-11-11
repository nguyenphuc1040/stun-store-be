using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class Discounts
    {
        public Discounts()
        {
            Games = new HashSet<Games>();
        }

        public string Id { get; set; }
        public double? Percent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Games> Games { get; set; }
    }
}
