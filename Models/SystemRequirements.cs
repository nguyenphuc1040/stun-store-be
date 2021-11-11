using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class SystemRequirements
    {
        public SystemRequirements()
        {
            GameVersions = new HashSet<GameVersions>();
        }

        public string Id { get; set; }
        public string IdGame { get; set; }
        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Graphics { get; set; }
        public string Storage { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Games IdGameNavigation { get; set; }
        public virtual ICollection<GameVersions> GameVersions { get; set; }
    }
}
