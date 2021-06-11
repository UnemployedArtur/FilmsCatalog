using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmsCatalog.Models.Entities
{
    public class Film : BaseEntity
    {
        /// <summary>
        /// Название фильма
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Опиисание фильма
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Год выпуска
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Режиссер
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// Путь к постеру
        /// </summary>
        public string PosterPath { get; set; }

        /// <summary>
        /// Ключ к пользователю, который выложил фильм
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; }
    }

    public class FilmConfiguration : IEntityTypeConfiguration<Film>
    {
        public void Configure(EntityTypeBuilder<Film> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
