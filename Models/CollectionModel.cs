using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class CollectionModel
    {
        public string Id { get; set; }
        public UserModel User { get; set; }
        public GameModel[] Games { get; set; }

    }
}
