using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models.ViewModels
{
    public class AddFilmViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название")]
        [StringLength(50, ErrorMessage = "Максимальная длина названия = 50")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Описание")]
        [StringLength(50, ErrorMessage = "Максимальная длина описания = 200")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Год выпуска")]
        [Range(1900, 2021, ErrorMessage = "Год выпуска можеть быть с 1900 до 2021")]
        public int Year { get; set; }


        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Режиссер")]
        [StringLength(50, ErrorMessage = "Максимальная длина имени режиссера = 50")]
        public string Director { get; set; }

        [Display(Name = "Постер")]
        public IFormFile Poster { get; set; }
    }
}
