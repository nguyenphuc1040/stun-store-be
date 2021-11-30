using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Dtos
{
    public class CommentsDto
    {
        public string IdComment { get; set; }
        public string IdGame { get; set; }
        public string IdUser { get; set; }
        public string Content { get; set; }
        public int? Likes { get; set; }
        public int? Dislike { get; set; }
        public DateTime? Time { get; set; }
        public double Star { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
    }
}
