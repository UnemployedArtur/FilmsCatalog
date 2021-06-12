using AutoMapper;
using FilmsCatalog.Models.Dto;
using FilmsCatalog.Models.Entities;

namespace FilmsCatalog.Mappings
{
    public class FilmsProfile : Profile
    {
        public FilmsProfile()
        {
            CreateMap<AddFilmDto, Film>();
            CreateMap<EditFilmDto, Film>();
            CreateMap<Film, FilmDto>();
        }
    }
}
