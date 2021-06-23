using PhotoWarehouse.Domain.Orders;
using PhotoWarehouse.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoWarehouse.Domain.Photos
{
    public class PhotoItem
    {
        [Display(Name = "Вариант изображения")]
        public int Id { get; set; }
        public DateTimeOffset DateUploaded { get; set; }
        public string FileName { get; set; }

        [NotMapped]
        public string RelativePath { get; set; }

        public Photo Photo { get; set; }
        public int PhotoId { get; set; }
        public ICollection<Order> OrderPhotos { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        [Display(Name = "Размер")]
        public PhotoSize Size { get; set; }

        [Display(Name = "Размер")]
        public int PhotoSizeId { get; set; }

        [Display(Name = "Формат изображения")]
        public FileFormat FileFormat { get; set; }

        [Display(Name = "Формат изображения")]
        public int FileFormatId { get; set; }
    }
}
