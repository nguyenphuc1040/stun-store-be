using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class Users
    {
        public Users()
        {
            Bills = new HashSet<Bills>();
            Collections = new HashSet<Collections>();
            Comments = new HashSet<Comments>();
            WishLists = new HashSet<WishLists>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Bills> Bills { get; set; }
        public virtual ICollection<Collections> Collections { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<WishLists> WishLists { get; set; }
    }
}
