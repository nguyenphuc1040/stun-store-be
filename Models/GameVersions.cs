using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class GameVersions
    {
        public string Id { get; set; }
        public string IdGame { get; set; }
        public string Version { get; set; }
        public string DateUpdate { get; set; }
        public string UrlDownload { get; set; }
        public string IdGenre { get; set; }
        public string MainImage { get; set; }
        public string Thumbnails { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string PrivacyPolicy { get; set; }
        public string IdSystemRequirement { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Games IdGameNavigation { get; set; }
        public virtual Genres IdGenreNavigation { get; set; }
        public virtual SystemRequirements IdSystemRequirementNavigation { get; set; }
    }
}
