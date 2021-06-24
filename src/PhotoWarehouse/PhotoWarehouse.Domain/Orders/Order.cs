using PhotoWarehouse.Domain.Photos;
using PhotoWarehouse.Domain.Users;
using System;
using System.Collections.Generic;

namespace PhotoWarehouse.Domain.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }


        public OrderStatus Status { get; set; }
        public int? OrderStatusId { get; set; }
        public IList<PhotoItem> OrderItems { get; set; }
        public ApplicationUser Customer { get; set; }
        public string CustomerId { get; set; }
    }
}
