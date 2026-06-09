
namespace opimerchant.Services
{
    public class LocalFileUploadService : IFileUploadService
    {
        IWebHostEnvironment _env;
        public LocalFileUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null)
            {
                return "";
            }

            if (file.ContentType != "image/png")
            {
                return "";
            }
            var filePath = Path.Combine(_env.ContentRootPath, @"wwwroot\images", file.FileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return file.FileName;
        } 
    }
}
