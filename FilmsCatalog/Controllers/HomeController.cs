using AutoMapper;
using FilmsCatalog.Models.Dto;
using FilmsCatalog.Models.ViewModels;
using FilmsCatalog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFilmsService _filmsService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IFilmsService filmsService,
            IMapper mapper)
        {
            _logger = logger;
            _filmsService = filmsService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 12)
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
                Films = _mapper.Map<List<FilmDto>, List<FilmViewModel>>(pagedFilms.Items)
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
