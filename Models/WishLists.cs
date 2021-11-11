using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class WishLists
    {
        public string Id { get; set; }
        public string IdUser { get; set; }
        public string IdGame { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Games IdGameNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
