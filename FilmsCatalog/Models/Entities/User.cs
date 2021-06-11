using Microsoft.AspNetCore.Identity;

namespace FilmsCatalog.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}