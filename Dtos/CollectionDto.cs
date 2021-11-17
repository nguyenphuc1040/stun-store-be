using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Dtos
{
    public class CollectionDto
    {
        public bool IsInstalled { get; set; }
        public GameDto Game { get; set; }
    }
}
