using System;
using System.Collections.Generic;

namespace game_store_be.Models
{

    public partial class LikeComment
    {
        public string IdComment { get; set; }
        public string IdUser { get; set; }
        public bool IsLike { get; set; }

        public LikeComment(string idComment, string idUser, bool isLike){
            this.IdComment = idComment;
            this.IdUser = idUser;
            this.IsLike = isLike;
        }
    }
}
