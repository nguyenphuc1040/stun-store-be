using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class DetailGenre
    {
        public string IdGenre { get; set; }
        public string IdGame { get; set; }

        public virtual Game IdGameNavigation { get; set; }
        public virtual Genre IdGenreNavigation { get; set; }
    }
}
