using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class CommentModel
    {
        public string Id { get; set; }
        public GameModel Game { get; set; }
        public UserModel User { get; set; }
        public string Content { get; set; }
        public DateTime At_Day { get; set; }
        public float Rate { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
    }
}
