using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;

namespace game_store_be.Utils
{
    public class AutoMapProfiles : Profile
    {
        public AutoMapProfiles()
        {
            CreateMap<Genre, GenreDto>();
            CreateMap<Game, GameDto>();
        }
    }
}
