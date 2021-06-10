using System;
using System.Collections.Generic;

namespace PhotoWarehouse.Domain.Photos
{
    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset InitialUploadDate { get; set; }
        public DateTimeOffset DateTaken { get; set; }

        public PhotoCategory Category { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<PhotoItem> PhotoItems { get; set; }

    }
}
