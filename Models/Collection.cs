using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class Collection
    {
        public string IdGame { get; set; }
        public string IdUser { get; set; }
        public bool? IsIntalled { get; set; }

        public virtual Game IdGameNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
