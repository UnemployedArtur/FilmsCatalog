using System.Threading.Tasks;

namespace FilmsCatalog.Services.Interfaces
{
    public interface IFilesService
    {
        Task<string> SavePosterAsync(byte[] poster);
        Task DeleteFileAsync(string path);
    }
}
