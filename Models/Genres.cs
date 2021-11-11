using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class Genres
    {
        public Genres()
        {
            GameVersions = new HashSet<GameVersions>();
            Games = new HashSet<Games>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<GameVersions> GameVersions { get; set; }
        public virtual ICollection<Games> Games { get; set; }
    }
}
