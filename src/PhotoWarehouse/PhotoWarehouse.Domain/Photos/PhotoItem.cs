﻿using PhotoWarehouse.Domain.Orders;
using System;
using System.Collections.Generic;

namespace PhotoWarehouse.Domain.Photos
{
    public class PhotoItem
    {
        public int Id { get; set; }
        public DateTimeOffset DateUploaded { get; set; }

        public Photo Photo { get; set; }
        public int PhotoId { get; set; }
        public IEnumerable<Order> OrderPhotos { get; set; }
    }
}
