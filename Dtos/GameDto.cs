using game_store_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Dtos
{
    public class GameDto
    {

        public string IdGame { get; set; }
        public string IdDiscount { get; set; }
        public string NameGame { get; set; }
        public double? AverageRate { get; set; }
        public int? NumOfRate { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Plaform { get; set; }
        public int? Cost { get; set; }
        public string LastestVersion { get; set; }
        public int? NumberOfBuyer { get; set; }
        public int? NumberOfDownloaders { get; set; }
        public virtual Discount IdDiscountNavigation { get; set; }
    }
}
