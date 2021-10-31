using game_store_be.Models;
using System;
using System.Collections.Generic;

namespace game_store_be.Interfaces
{
    public interface IGenre
    {
        List<GenreModel> GetGenres();
        GenreModel GetGenre(Guid id);
        GenreModel AddGenre(GenreModel genre);
        GenreModel EditGenre(GenreModel genre);
        void DeleteGenre(GenreModel genre);

    }
}
