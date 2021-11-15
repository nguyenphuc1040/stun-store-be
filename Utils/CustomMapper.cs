using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Utils
{
    public class CustomMapper
    {
        private readonly IMapper _mapper;

        public CustomMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ICollection<GameDto> CustomMapListGame(ICollection<Game> listGame)
        {
            var gamesDto = _mapper.Map<ICollection<GameDto>>(listGame);
            for (var i = 0; i < listGame.Count(); i++)
            {
                gamesDto.ToList().ElementAt(i).Discount = _mapper.Map<Discount, DiscountDto>(listGame.ToList().ElementAt(i).IdDiscountNavigation);
                gamesDto.ToList().ElementAt(i).Genres = _mapper.Map<ICollection<DetailGenreDto>>(listGame.ToList().ElementAt(i).DetailGenre);
                gamesDto.ToList().ElementAt(i).ImageGameDetail = _mapper.Map<ICollection<ImageGameDetailDto>>(listGame.ToList().ElementAt(i).ImageGameDetail);
            }
            return gamesDto;
        }
        public GameDto CustomMapGame (Game game)
        {
            var gameDto = _mapper.Map<Game,GameDto>(game);
            gameDto.Discount = _mapper.Map<Discount, DiscountDto>(game.IdDiscountNavigation);
            gameDto.Genres = _mapper.Map<ICollection<DetailGenreDto>>(game.DetailGenre);

            return gameDto;
        }

        public ICollection<WishListDto> CustomMapWishList (ICollection<WishList> wishList)
        {
            var wishListDto = _mapper.Map<ICollection<WishListDto>>(wishList);

            for (var i = 0; i < wishListDto.Count(); i++)
            {
                wishListDto.ToList().ElementAt(i).Game = CustomMapGame(wishList.ToList().ElementAt(i).IdGameNavigation);
            }
            return wishListDto;
        }
    }
}
