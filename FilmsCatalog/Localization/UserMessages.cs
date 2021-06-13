namespace FilmsCatalog.Localization
{
    public class UserMessages : IUserMessages
    {
        public string Error_SaveFilm => "Не удалось сохранить фильм. Попробуйте позже.";

        public string Error_NotFoundFilm => "Не удалось найти фильм.";

        public string Error_DeleteFilm => "Не удалось удалить фильм. Попробуй позже.";

        public string Error_SavePoster => "Не удалость сохранить постер к фильму. Попробуйте другое изображение.";

        public string Error_WrongUserFilm => "Вы не можете редактировать этот фильм";
    }
}
