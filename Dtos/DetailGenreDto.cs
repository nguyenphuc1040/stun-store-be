using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Dtos
{
    public class DetailGenreDto
    {
        //public string IdGenre { get; set; }
        //public string IdGame { get; set; }

        //public virtual Game IdGameNavigation { get; set; }
        public virtual GenreDto IdGenreNavigation { get; set; }
    }
}
