using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models.ViewModels
{
    public class EditFilmViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Год выпуска")]
        public int Year { get; set; }


        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Режиссер")]
        public string Director { get; set; }
    }
}
