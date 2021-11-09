using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class Collections
    {
        public uint Id { get; set; }
        public uint IdUser { get; set; }
        public uint IdGame { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Games IdGameNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
