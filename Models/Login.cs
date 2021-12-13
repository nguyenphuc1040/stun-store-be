using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginWithSMA
    {
        public Login ILogin { get; set; }
        public Users IUser { get; set; }
    }

    public class Verification {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
