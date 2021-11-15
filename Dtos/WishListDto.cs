using game_store_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Dtos
{
    public class WishListDto
    {
        //public string IdGame { get; set; }
        //public string IdUser { get; set; }
        public GameDto Game { get;set; }
        //public virtual Users IdUserNavigation { get; set; }
    }
}
