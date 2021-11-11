using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class Comments
    {
        public string Id { get; set; }
        public string IdUser { get; set; }
        public string IdGame { get; set; }
        public string Content { get; set; }
        public DateTime? AtDay { get; set; }
        public double? Rate { get; set; }
        public int? Like { get; set; }
        public int? Dislike { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Games IdGameNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
