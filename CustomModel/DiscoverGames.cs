using System;
using System.Collections.Generic;
using game_store_be.Dtos;

namespace game_store_be.Utils{
    public class DiscoverGames {
        public List<GameDto> carousel {get; set;}
        public List<GameDto> topsellers {get; set;}
        public List<GameDto> newReleases {get; set;}
        public List<GameDto> mostFavorite {get; set;}
        public List<GameDto> freeGames {get; set;}
        public List<GameDto> mostPopular {get; set;}
        public List<GameDto> topGamesWeek {get; set;}
        public List<GameDto> topGamesMonth {get; set;}
        public List<GameDto> gameOnSales {get; set;}
        public List<GameDto> freeNow {get; set;}
    }
}