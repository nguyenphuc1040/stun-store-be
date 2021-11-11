using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class Games
    {
        public Games()
        {
            Bills = new HashSet<Bills>();
            Collections = new HashSet<Collections>();
            Comments = new HashSet<Comments>();
            GameVersions = new HashSet<GameVersions>();
            SystemRequirements = new HashSet<SystemRequirements>();
            WishLists = new HashSet<WishLists>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string IdGenre { get; set; }
        public double? AverageRate { get; set; }
        public int? NumOfRate { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Platform { get; set; }
        public int? Cost { get; set; }
        public string? IdDiscount { get; set; }
        public string LastestVersion { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Discounts IdDiscountNavigation { get; set; }
        public virtual Genres IdGenreNavigation { get; set; }
        public virtual ICollection<Bills> Bills { get; set; }
        public virtual ICollection<Collections> Collections { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<GameVersions> GameVersions { get; set; }
        public virtual ICollection<SystemRequirements> SystemRequirements { get; set; }
        public virtual ICollection<WishLists> WishLists { get; set; }
    }
}
