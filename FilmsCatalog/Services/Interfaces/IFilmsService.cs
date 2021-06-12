using FilmsCatalog.Models.Dto;
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Interfaces
{
    public interface IFilmsService
    {
        /// <summary>
        /// Добавление фильма
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultDto> AddFilmAsync(AddFilmDto dto);

        /// <summary>
        /// Редактирование фильма
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultDto> EditFilmAsync(EditFilmDto dto);

        /// <summary>
        /// Получение фильма
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<FilmDto> GetFilmAsync(BaseDto dto);

        /// <summary>
        /// Получение пагинированного списка фильмов
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedListDto<FilmDto>> GetPagedFilmsAsync(GetPagedFilmsDto dto);

        /// <summary>
        /// Удаление фильма
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultDto> DeleteFilmAsync(BaseDto dto);
    }
}
