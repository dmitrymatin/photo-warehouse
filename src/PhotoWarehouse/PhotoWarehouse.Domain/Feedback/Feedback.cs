using System;

namespace PhotoWarehouse.Domain.Feedback
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset DateSubmitted { get; set; }
        public DateTimeOffset DateReviewed { get; set; }
    }
}
