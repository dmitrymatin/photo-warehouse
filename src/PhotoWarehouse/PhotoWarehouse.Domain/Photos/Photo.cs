using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoWarehouse.Domain.Photos
{
    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Initial upload date")]
        public DateTimeOffset InitialUploadDate { get; set; }

        [Display(Name = "Date Taken")]
        public DateTimeOffset? DateTaken { get; set; }

        public PhotoCategory Category { get; set; }
        
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public IList<PhotoItem> PhotoItems { get; set; }

    }
}
