using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Dtos
{
    public class UserDto
    {
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public string Avatar { get; set; }
        public string Background { get; set; }
        public string Roles { get; set; }
    }
}
