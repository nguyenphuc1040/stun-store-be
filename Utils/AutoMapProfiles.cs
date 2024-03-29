﻿using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using System.Collections;

namespace game_store_be.Utils
{
    public class AutoMapProfiles : Profile
    {
        public AutoMapProfiles()
        {
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<Game, GameDto>();
            CreateMap<Discount, DiscountDto>().ReverseMap();
            CreateMap<Users, UserDto>().ReverseMap();
            CreateMap<DetailGenre, DetailGenreDto>().ReverseMap();
            CreateMap<ImageGameDetail, ImageGameDetailDto>().ReverseMap();
            CreateMap<SlideGameHot, SlideGameHotDto>().ReverseMap();
            CreateMap<GameVersion, GameVersionDto>().ReverseMap();
            CreateMap<WishList, WishListDto>().ReverseMap();
            CreateMap<Bill, BillDto>().ReverseMap();
            CreateMap<Discount, DiscountDto>().ReverseMap();
            CreateMap<Collection, CollectionDto>().ReverseMap();
            CreateMap<Discount, Discount>().ReverseMap();
            CreateMap<Game, Game>().ReverseMap();
            CreateMap<Comments, CommentsDto>().ReverseMap();
            CreateMap<Discount,BillDiscount>().ReverseMap();
        }
    }
}
