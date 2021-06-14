using AutoMapper;
using FilmsCatalog.Data;
using FilmsCatalog.Localization;
using FilmsCatalog.Models.Dto;
using FilmsCatalog.Models.Entities;
using FilmsCatalog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public class FilmsService : IFilmsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserMessages _userMessages;
        private readonly IFilesService _filesService;
        private readonly ILogger<FilmsService> _logger;

        public FilmsService(ApplicationDbContext dbContext, IMapper mapper, IUserMessages userMessages,
            IFilesService filesService, ILogger<FilmsService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userMessages = userMessages;
            _filesService = filesService;
            _logger = logger;
        }

        public async Task<ResultDto> AddFilmAsync(AddFilmDto dto)
        {
            var entity = _mapper.Map<Film>(dto);

            _dbContext.Films.Add(entity);

            if (dto.Poster != null)
            {
                var posterPath = await _filesService.SavePosterAsync(dto.Poster);

                if (posterPath == null)
                {
                    return new ResultDto() { Errors = new List<string>() { _userMessages.Error_SavePoster } };
                }

                entity.PosterPath = posterPath;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                await _filesService.DeleteFileAsync(entity.PosterPath);
                _logger.LogError(exception.ToString());

                return new ResultDto() { Errors = new List<string>() { _userMessages.Error_SaveFilm } };
            }

            return new ResultDto() { IsSuccess = true, Id = entity.Id };
        }

        public async Task<ResultDto> DeleteFilmAsync(BaseDto dto)
        {
            var entity = await _dbContext.Films.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity == null)
            {
                return new ResultDto() { Errors = new List<string>() { _userMessages.Error_NotFoundFilm } };
            }

            _dbContext.Films.Remove(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
                await _filesService.DeleteFileAsync(entity.PosterPath);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());

                return new ResultDto() { Errors = new List<string>() { _userMessages.Error_DeleteFilm } };
            }

            return new ResultDto() { IsSuccess = true };
        }

        public async Task<ResultDto> EditFilmAsync(EditFilmDto dto)
        {
            var entity = await _dbContext.Films.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity == null)
            {
                return new ResultDto() { Errors = new List<string>() { _userMessages.Error_NotFoundFilm } };
            }

            if (dto.UserId != entity.UserId)
            {
                return new ResultDto() { Errors = new List<string>() { _userMessages.Error_WrongUserFilm } };
            }

            _mapper.Map(dto, entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());

                return new ResultDto() { Errors = new List<string>() { _userMessages.Error_SaveFilm } };
            }

            return new ResultDto() { IsSuccess = true, Id = entity.Id };
        }

        public async Task<FilmDto> GetFilmAsync(BaseDto dto)
        {
            var entity = await _dbContext.Films
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity == null)
            {
                return null;
            }

            var film = _mapper.Map<FilmDto>(entity);

            film.UserName = $"{ entity.User.FirstName } { entity.User.LastName }";
            
            return film;
        }

        public async Task<PagedListDto<FilmDto>> GetPagedFilmsAsync(GetPagedFilmsDto dto)
        {
            var entities = await _dbContext.Films
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            var filmsDto = _mapper.Map<List<FilmDto>>(entities);
            var totalCount = await _dbContext.Films.CountAsync();

            return new PagedListDto<FilmDto>
            {
                PageSize = dto.PageSize,
                PageNumber = dto.PageNumber,
                Items = filmsDto,
                TotalCount = totalCount
            };
        }
    }
}
