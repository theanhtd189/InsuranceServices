using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string? FeedbackContent { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CustomerId { get; set; }
    }
}
