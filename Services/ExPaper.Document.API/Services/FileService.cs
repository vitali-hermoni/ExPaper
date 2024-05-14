using ExPaper.Document.API.Services.IServices;

namespace ExPaper.Document.API.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;


        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }




        public async Task<bool> CopyFileToNetworkShare(IFormFile file, string fileName, string filePath, string networkPath, string username, string password)
        {
            bool ret = false;

            try
            {
                var fullPath = Path.Combine(networkPath, filePath);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                    _logger.LogInformation("Ordner wurde erstellt!");
                }

                using (var stream = new FileStream(Path.Combine(fullPath, fileName), FileMode.Create))
                {
                    _logger.LogInformation("Starte Kopiervorgang...");
                    await file.CopyToAsync(stream);
                    ret = true;
                    _logger.LogInformation("Kopieren abgeschlossen");
                }

                return ret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }




        public bool DeleteFileFromNetworkShare(string filePath, string networkPath, string username, string password)
        {
            bool ret = false;

            try
            {
                var path = Path.Combine(networkPath, filePath);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    ret = true;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ret;
            }

            return ret;
        }
    }
}
