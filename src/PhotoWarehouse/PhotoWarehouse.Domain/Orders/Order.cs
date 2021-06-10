﻿using PhotoWarehouse.Domain.Photos;
using System;
using System.Collections.Generic;

namespace PhotoWarehouse.Domain.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }


        public OrderStatus Status { get; set; }
        public int OrderStatusId { get; set; }
        public IEnumerable<PhotoItem> OrderItems { get; set; }
    }
}
