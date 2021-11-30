using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class DisLikeComment
    {
        public string IdComment { get; set; }
        public string IdUser { get; set; }

        public virtual Comments IdCommentNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
