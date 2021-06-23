using PhotoWarehouse.Domain.Orders;
using PhotoWarehouse.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoWarehouse.Domain.Photos
{
    public class PhotoItem
    {
        public int Id { get; set; }
        public DateTimeOffset DateUploaded { get; set; }
        public string FileName { get; set; }

        [NotMapped]
        public string RelativePath { get; set; }

        public Photo Photo { get; set; }
        public int PhotoId { get; set; }
        public ICollection<Order> OrderPhotos { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public PhotoSize Size { get; set; }
        public int PhotoSizeId { get; set; }
        public FileFormat FileFormat { get; set; }
        public int FileFormatId { get; set; }
    }
}
