using FilmsCatalog.Configuration;
using FilmsCatalog.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public class FilesService : IFilesService
    {
        private readonly PostersConfiguration _postersConfig;
        private readonly ILogger<FilesService> _logger;

        public FilesService(IOptions<PostersConfiguration> postersConfig, ILogger<FilesService> logger)
        {
            _postersConfig = postersConfig.Value;
            _logger = logger;
        }

        public async Task<string> SavePosterAsync(byte[] poster)
            => await SaveImageAsync(poster, _postersConfig.Width, _postersConfig.Height, _postersConfig.Path);

        public async Task DeleteFileAsync(string path)
            => await Task.Run(() =>
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            });

        private async Task<string> SaveImageAsync(byte[] bytes, int width, int height, string path)
        {
            var fileName = $"{ new Guid() }.jpg";
            var fullPath = Path.Combine(path, fileName);

            try
            {
                using var image = Image.Load(bytes);

                image.Mutate(x => x.Resize(width, height));
                await image.SaveAsync(fullPath);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());

                return null;
            };

            return fullPath;
        }
    }
}
