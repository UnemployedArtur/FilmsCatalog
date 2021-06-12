using FilmsCatalog.Models.Dto;
using FilmsCatalog.Services.Interfaces;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public class FilesService : IFilesService
    {
        public Task<ResultDto> SavePosterAsync(SavePosterDto dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
