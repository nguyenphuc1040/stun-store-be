using game_store_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.CustomModel
{
    public class PostGameBody
    {
        public Game Game { get; set; }
        public GameVersion GameVersion { get; set; }

        public List<string>? ListImageDetail { get; set; }
        public List<string>? ListGenreDetail { get; set; }
    }

    public class LazyLoadBrowseBody
    {
        public List<string>? ListGenreDetail { get; set; }
        public int count { get; set; }
        public int start { get; set; }
        public string sortBy { get; set; }
    }

}
