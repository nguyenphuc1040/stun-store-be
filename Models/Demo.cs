using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class Demo
    {
        public long Id { get; set; }
        public long? Uid { get; set; }
        public string Gamedata { get; set; }
    }
}
