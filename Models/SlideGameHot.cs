using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class SlideGameHot
    {
        public string IdGame { get; set; }
        public string UrlVideo { get; set; }

        public virtual Game IdGameNavigation { get; set; }
    }
}
