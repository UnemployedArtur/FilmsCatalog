using AutoMapper;
using FilmsCatalog.Localization;
using FilmsCatalog.Models.Dto;
using FilmsCatalog.Models.Entities;
using FilmsCatalog.Models.ViewModels;
using FilmsCatalog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    [Route("films")]
    public class FilmsController : Controller
    {
        private readonly IFilmsService _filmsService;
        private readonly IMapper _mapper;
        private readonly IUserMessages _userMessages;
        private readonly UserManager<User> _userManager;

        public FilmsController(IFilmsService filmsService, IMapper mapper, IUserMessages userMessages,
            UserManager<User> userManager)
        {
            _filmsService = filmsService;
            _mapper = mapper;
            _userMessages = userMessages;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Films([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 12)
        {
            var dto = new GetPagedFilmsDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var pagedFilms = await _filmsService.GetPagedFilmsAsync(dto);

            var viewModel = new FilmsViewModel()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                HasNextPage = pagedFilms.HasNextPage,
                HasPreviousPage = pagedFilms.HasPreviousPage,
                Films = pagedFilms.Items
            };

            return View(viewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Film([FromRoute] Guid id)
        {
            var dto = new BaseDto() { Id = id };
            var film = await _filmsService.GetFilmAsync(dto);

            if (film == null)
            {
                ModelState.AddModelError(string.Empty, "Такого фильма не существует.");

                return View(new FilmViewModel());
            }

            var viewModel = _mapper.Map<FilmViewModel>(film);

            return View(viewModel);
        }

        [HttpGet("add")]
        public IActionResult AddFilm()
        {
            return View(new AddFilmViewModel());
        }

        [HttpGet("{id}/edit")]
        public async Task<IActionResult> EditFilm([FromRoute] Guid id)
        {
            var dto = new BaseDto() { Id = id };
            var film = await _filmsService.GetFilmAsync(dto);

            if (film == null)
            {
                ModelState.AddModelError(string.Empty, _userMessages.Error_NotFoundFilm);

                return View(new EditFilmViewModel());
            }

            var viewModel = _mapper.Map<EditFilmViewModel>(film);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddFilm(AddFilmViewModel viewModel)
        {
            var dto = _mapper.Map<AddFilmDto>(viewModel);
            dto.Poster = await ReadFormFile(viewModel.Poster);
            dto.UserId = await GetUserId();

            var result = await _filmsService.AddFilmAsync(dto);

            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(viewModel);
            }

            return RedirectToAction("Film", "Films", new { result.Id });
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFilm(EditFilmViewModel viewModel)
        {
            var dto = _mapper.Map<EditFilmDto>(viewModel);
            var result = await _filmsService.EditFilmAsync(dto);

            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(viewModel);
            }

            return RedirectToAction("Film", "Films", new { result.Id });
        }

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFilm([FromRoute] Guid id)
        {
            var dto = new BaseDto() { Id = id };
            await _filmsService.DeleteFilmAsync(dto);

            return RedirectToAction("Films", "Films");
        }

        private async Task<byte[]> ReadFormFile(IFormFile formFile)
        {
            var fileLength = formFile.Length;

            if (fileLength < 0)
            {
                return null;
            }

            using var fileStream = formFile.OpenReadStream();
            var bytes = new byte[fileLength];

            await fileStream.ReadAsync(bytes, 0, (int)fileLength);

            return bytes;
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.GetUserAsync(User);

            return user.Id;
        }
    }
}
