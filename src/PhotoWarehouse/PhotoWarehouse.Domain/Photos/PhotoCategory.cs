using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoWarehouse.Domain.Photos
{
    public class PhotoCategory
    {
        [Display(Name = "Category")]
        public int Id { get; set; }

        [Display(Name = "Category")]
        public string Name { get; set; }

        public IEnumerable<Photo> Photos { get; set; }
    }
}
