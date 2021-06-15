using Microsoft.AspNetCore.Http;

namespace PhotoWarehouseApp.Services
{
    public static class FormFileExtensions
    {

        public static bool IsImage(this IFormFile formFile, string extension)
        {
            //  Check the image mime types
            string contentType = formFile.ContentType.ToLower();
            if (contentType != "image/jpg" &&
                        contentType != "image/jpeg" &&
                        contentType != "image/pjpeg" &&
                        contentType != "image/gif" &&
                        contentType != "image/x-png" &&
                        contentType != "image/png")
            {
                return false;
            }

            //  Check the image extension
            extension = extension.ToLower();
            if (extension != ".jpg"
                && extension != ".png"
                && extension != ".gif"
                && extension != ".jpeg")
            {
                return false;
            }

            return true;
        }
    }
}
