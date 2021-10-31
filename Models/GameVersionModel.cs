using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class GameVersionModel
    {
        public string Id { get; set; }
        public GameModel Game { get; set; }
        public string Version { get; set; }
        public DateTime DateUpdate { get; set; }
        public string UrlDowload { get; set; }
        public GenreModel[] Genres { get; set; }
        public string MainImage { get; set; }
        public string[] ThumbNails { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string PrivacyPolicy { get; set; }
        public SystemRequirementModel SystemRequirement { get; set; }

    }
}
