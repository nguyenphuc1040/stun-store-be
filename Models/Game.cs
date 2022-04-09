using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class Game
    {
        public Game()
        {
            Bill = new HashSet<Bill>();
            Collection = new HashSet<Collection>();
            Comments = new HashSet<Comments>();
            DetailGenre = new HashSet<DetailGenre>();
            GameVersion = new HashSet<GameVersion>();
            ImageGameDetail = new HashSet<ImageGameDetail>();
            WishList = new HashSet<WishList>();
        }

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
        public string UrlVideo {get; set;}
        public double GameSize {get; set;}

        public virtual Discount IdDiscountNavigation { get; set; }
        public virtual SlideGameHot SlideGameHot { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
        public virtual ICollection<Collection> Collection { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<DetailGenre> DetailGenre { get; set; }
        public virtual ICollection<GameVersion> GameVersion { get; set; }
        public virtual ICollection<ImageGameDetail> ImageGameDetail { get; set; }
        public virtual ICollection<WishList> WishList { get; set; }
    }
}
