using System.Collections.Generic;

namespace PhotoWarehouse.Domain.Photos
{
    public class PhotoCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Photo> Photos { get; set; }
    }
}
