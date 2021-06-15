using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PhotoWarehouseApp.Services
{
    public class FileService
    {
        public static string EnsureCorrectPathAndFileName(
            IWebHostEnvironment webHostEnvironment, 
            IConfiguration configuration,
            string filename
            )
        {
            filename = filename.Trim('"');
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            string uploadsFolder = configuration["UploadsFolder"];

            return $"{webHostEnvironment.WebRootPath}\\{uploadsFolder}\\{filename}";
        }

        public static string GetExtension(string path)
        {
            return System.IO.Path.GetExtension(path);
        }
    }
}
