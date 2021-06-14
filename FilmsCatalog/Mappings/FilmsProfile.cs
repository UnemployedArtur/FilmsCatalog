using AutoMapper;
using FilmsCatalog.Models.Dto;
using FilmsCatalog.Models.Entities;
using FilmsCatalog.Models.ViewModels;

namespace FilmsCatalog.Mappings
{
    public class FilmsProfile : Profile
    {
        public FilmsProfile()
        {
            CreateMap<AddFilmDto, Film>();
            CreateMap<EditFilmDto, Film>();
            CreateMap<Film, FilmDto>();
            CreateMap<FilmDto, FilmViewModel>();
            CreateMap<FilmDto, EditFilmViewModel>();
            CreateMap<AddFilmViewModel, AddFilmDto>()
                .ForMember(dest => dest.Poster, option => option.Ignore());

            CreateMap<EditFilmViewModel, EditFilmDto>();
        }
    }
}
