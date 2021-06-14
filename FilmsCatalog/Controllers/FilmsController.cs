using AutoMapper;
using FilmsCatalog.Configuration;
using FilmsCatalog.Localization;
using FilmsCatalog.Models.Dto;
using FilmsCatalog.Models.Entities;
using FilmsCatalog.Models.ViewModels;
using FilmsCatalog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
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
        private readonly PostersConfiguration _postersConfiguration;

        public FilmsController(IFilmsService filmsService, IMapper mapper, IUserMessages userMessages,
            UserManager<User> userManager, IOptions<PostersConfiguration> postersConfiguration)
        {
            _filmsService = filmsService;
            _mapper = mapper;
            _userMessages = userMessages;
            _userManager = userManager;
            _postersConfiguration = postersConfiguration.Value;
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
            var currentUserId = await GetUserId();

            viewModel.IsOwner = currentUserId == film.UserId;

            return View(viewModel);
        }

        [HttpGet("add")]
        [Authorize]
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

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddFilm(AddFilmViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

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

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFilm(EditFilmViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var dto = _mapper.Map<EditFilmDto>(viewModel);

            dto.UserId = await GetUserId();

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

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFilm([FromForm] Guid id)
        {
            var dto = new BaseDto() { Id = id };

            await _filmsService.DeleteFilmAsync(dto);

            return RedirectToAction("Index", "Home");
        }

        private async Task<byte[]> ReadFormFile(IFormFile formFile)
        {
            if (formFile == null)
            {
                return null;
            }

            var fileExtension = Path.GetExtension(formFile.FileName);

            if (!_postersConfiguration.Extensions.Contains(fileExtension))
            {
                return null;
            }

            var fileLength = formFile.Length;

            if (fileLength < 0 || fileLength > _postersConfiguration.MaxSize)
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
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var user = await _userManager.GetUserAsync(User);

            return user.Id;
        }
    }
}
