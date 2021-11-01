using System;

namespace game_store_be.Models
{
    public class DiscountModel
    {
        public Guid Id { get; set; }
        public float Percent { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
    }
}
