using FilmsCatalog.Models.Dto;
using System.Collections.Generic;

namespace FilmsCatalog.Models.ViewModels
{
    public class FilmsViewModel
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public List<FilmDto> Films { get; set; }
    }
}
