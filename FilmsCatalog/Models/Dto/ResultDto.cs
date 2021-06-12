using System.Collections.Generic;

namespace FilmsCatalog.Models.Dto
{
    public class ResultDto : BaseDto
    {
        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }
    }
}
