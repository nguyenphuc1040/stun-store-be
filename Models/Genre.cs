using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class Genre
    {
        public Genre()
        {
            DetailGenre = new HashSet<DetailGenre>();
        }

        public string IdGenre { get; set; }
        public string NameGenre { get; set; }

        public virtual ICollection<DetailGenre> DetailGenre { get; set; }
    }
}
