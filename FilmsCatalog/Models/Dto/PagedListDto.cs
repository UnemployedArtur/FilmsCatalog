using System;
using System.Collections.Generic;

namespace FilmsCatalog.Models.Dto
{
    public class PagedListDto<T>
    {
        public List<T> Items { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages 
        { 
            get
            {
                return (int)Math.Ceiling(TotalCount / (double)PageSize);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageNumber >= 1 && PageNumber < TotalPages;
            }
        }
    }
}
