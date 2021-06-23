using Microsoft.AspNetCore.Identity;
using PhotoWarehouse.Domain.Feedbacks;
using PhotoWarehouse.Domain.Orders;
using PhotoWarehouse.Domain.Photos;
using System;
using System.Collections.Generic;

namespace PhotoWarehouse.Domain.Users
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public DateTimeOffset DateJoined { get; set; }

        [PersonalData]
        public byte[] UserPhoto { get; set; }


        public IEnumerable<Feedback> FeedbackReviewed { get; set; }
        public IList<PhotoItem> PhotoItemsInBasket { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}