using System;

namespace FilmsCatalog.Models.Dto
{
    public class EditFilmDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public string Director { get; set; }

        public string UserId { get; set; }
    }
}
