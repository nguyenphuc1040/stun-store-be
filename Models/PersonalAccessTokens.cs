using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class PersonalAccessTokens
    {
        public ulong Id { get; set; }
        public string TokenableType { get; set; }
        public ulong TokenableId { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Abilities { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
