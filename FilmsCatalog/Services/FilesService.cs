using FilmsCatalog.Configuration;
using FilmsCatalog.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _environment;

        public FilesService(IOptions<PostersConfiguration> postersConfig, ILogger<FilesService> logger,
            IWebHostEnvironment environment)
        {
            _postersConfig = postersConfig.Value;
            _logger = logger;
            _environment = environment;
        }

        public async Task<string> SavePosterAsync(byte[] poster)
        {
            if (poster == null)
            {
                return null;
            }

            return await SaveImageAsync(poster, _postersConfig.Width, _postersConfig.Height, _postersConfig.Path);
        }

        public async Task DeleteFileAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            await Task.Run(() =>
            {
                var fullPath = _environment.WebRootPath + path;

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            });
        }

        private async Task<string> SaveImageAsync(byte[] bytes, int width, int height, string path)
        {
            var fileName = $"{ Guid.NewGuid() }.jpg";
            var fullPath = Path.Combine(_environment.WebRootPath, path, fileName);

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

            return $"\\{ Path.Combine(path, fileName) }";
        }
    }
}
