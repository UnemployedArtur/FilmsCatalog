using System;

namespace FilmsCatalog.Models.Dto
{
    public class EditFilmDto : AddFilmDto
    {
        public Guid Id { get; set; }
    }
}
