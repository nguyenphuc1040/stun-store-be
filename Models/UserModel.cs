using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
    }
}
