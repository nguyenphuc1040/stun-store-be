using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;

namespace game_store_be.Utils
{
    public class AutoMapProfiles : Profile
    {
        public AutoMapProfiles()
        {
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<Game, GameDto>();
            CreateMap<ImageDetail, DiscountDto>().ReverseMap();
            CreateMap<Users, UserDto>().ReverseMap();
            CreateMap<DetailGenre, DetailGenreDto>().ReverseMap();
            CreateMap<ImageGameDetail, ImageGameDetailDto>().ReverseMap();
        }
    }
}
