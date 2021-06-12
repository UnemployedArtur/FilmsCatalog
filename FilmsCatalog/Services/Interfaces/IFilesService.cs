using FilmsCatalog.Models.Dto;
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Interfaces
{
    public interface IFilesService
    {
        Task<ResultDto> SavePosterAsync(SavePosterDto dto);
    }
}
