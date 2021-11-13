using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class ImageGameDetail
    {
        public string IdImage { get; set; }
        public string IdGame { get; set; }
        public string Url { get; set; }

        public virtual Game IdGameNavigation { get; set; }
    }
}
