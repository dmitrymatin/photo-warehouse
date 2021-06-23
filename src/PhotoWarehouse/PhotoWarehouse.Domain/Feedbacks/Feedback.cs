using PhotoWarehouse.Domain.Users;
using System;

namespace PhotoWarehouse.Domain.Feedbacks
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset DateSubmitted { get; set; }
        public DateTimeOffset? DateReviewed { get; set; }

        public ApplicationUser ReviewedBy { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
