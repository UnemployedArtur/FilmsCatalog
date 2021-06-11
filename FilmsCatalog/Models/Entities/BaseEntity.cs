using System;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
