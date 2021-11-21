using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class Users
    {
        public Users()
        {
            Bill = new HashSet<Bill>();
            Collection = new HashSet<Collection>();
            Comments = new HashSet<Comments>();
            WishList = new HashSet<WishList>();
        }

        public string IdUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public string Avatar { get; set; }
        public string Background { get; set; }
        public string Roles { get; set; }

        public virtual ICollection<Bill> Bill { get; set; }
        public virtual ICollection<Collection> Collection { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<WishList> WishList { get; set; }
    }
}
