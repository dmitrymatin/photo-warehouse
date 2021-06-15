using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PhotoWarehouseApp.Services
{
    public class FileService
    {
        public static string EnsureCorrectFileName(string filename)
        {
            filename = filename.Trim('"');

            if (filename.Contains(@"\"))
                filename = filename.Substring(filename.LastIndexOf(@"\") + 1);

            return filename;
        }

        public static string GetUserImageAbsolutePath(
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            string filename
            )
        {

            string uploadsFolder = configuration["UploadsFolder"];

            return $@"{webHostEnvironment.WebRootPath}\{uploadsFolder}\{filename}";
        }

        public static string GetFileExtension(string path)
        {
            return System.IO.Path.GetExtension(path);
        }
    }
}
