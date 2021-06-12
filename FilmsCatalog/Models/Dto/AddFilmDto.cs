namespace FilmsCatalog.Models.Dto
{
    public class AddFilmDto
    {
        public string Title { get; set; }
        
        public string Description { get; set; }

        public int Year { get; set; }

        public string Director { get; set; }

        public byte[] Poster { get; set; }

        public string UserId { get; set; }
    }
}
