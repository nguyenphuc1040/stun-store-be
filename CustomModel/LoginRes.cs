using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using game_store_be.Dtos;

namespace game_store_be.CustomModel
{
    public class LoginRes
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
