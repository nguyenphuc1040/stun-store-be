using game_store_be.Interfaces;
using game_store_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace game_store_be.Services
{
    public class GenreService :  IGenre
    {
        private GenreModel GENRE = new GenreModel()
        {
            Id = Guid.NewGuid(),
            Name = "Hanh dong"
        };

        private List<GenreModel> GENRES = new List<GenreModel>()
        {
            new GenreModel()
            {
                Id = Guid.NewGuid(),
                Name = "Hanh dong"
            },
            new GenreModel()
            {
                 Id = Guid.NewGuid(),
                Name = "Tri tue"
            },
            new GenreModel()
            {
                 Id = Guid.NewGuid(),
                Name = "Dua xe"
            }
        };

        public GenreModel AddGenre(GenreModel genre)
        {
            genre.Id = Guid.NewGuid();
            GENRES.Add(genre);
            return genre;
        }

        public void DeleteGenre(GenreModel genre)
        {
            GENRES.Remove(genre);
        }

        public GenreModel EditGenre(GenreModel genre)
        {
            var existGenre = GENRES.SingleOrDefault(x => x.Id == genre.Id);
            if (existGenre != null)
            {
                existGenre.Name = genre.Name;
            }
            return existGenre;
        }

        public List<GenreModel> GetGenres()
        {
            return GENRES;
        }
        public GenreModel GetGenre( Guid id)
        {
            return GENRES.SingleOrDefault(x => x.Id == id);
        }
    }
}
