using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class Comments
    {

        public Comments()
        {
            LikeComment = new HashSet<LikeComment>();
            DisLikeComment = new HashSet<DisLikeComment>();
        }

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

        public virtual Game IdGameNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }

        public virtual ICollection<LikeComment> LikeComment { get; set; }
        public virtual ICollection<DisLikeComment> DisLikeComment { get; set; }


    }
}
