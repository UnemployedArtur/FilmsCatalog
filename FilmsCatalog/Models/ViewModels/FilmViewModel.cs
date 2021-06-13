using System;

namespace FilmsCatalog.Models.ViewModels
{
    public class FilmViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public string Director { get; set; }

        public string PosterPath { get; set; }

        public string UserName { get; set; }
    }
}
