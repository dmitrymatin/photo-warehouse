using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoWarehouse.Domain.Photos
{
    public class Photo
    {
        public int Id { get; set; }
        
        [Display(Name = "Название")]
        public string Name { get; set; }
        
        [Display(Name = "Описание")]
        public string Description { get; set; }
        
        public int DownloadCount { get; set; }
        public int ViewCount { get; set; }

        [Display(Name = "Дата загрузки")]
        public DateTimeOffset InitialUploadDate { get; set; }

        [Display(Name = "Дата создания")]
        public DateTimeOffset? DateTaken { get; set; }

        [Display(Name = "Категория")]
        public PhotoCategory Category { get; set; }
        
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        public IList<PhotoItem> PhotoItems { get; set; }

    }
}
