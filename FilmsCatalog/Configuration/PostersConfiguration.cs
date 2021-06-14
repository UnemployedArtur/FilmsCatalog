using System.Collections.Generic;

namespace FilmsCatalog.Configuration
{
    public class PostersConfiguration
    {
        public string Path { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int MaxSize { get; set; }

        public List<string> Extensions { get; set; }
    }
}
