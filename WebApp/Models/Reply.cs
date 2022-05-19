using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Reply
    {
        public int Id { get; set; }
        public string? Answer { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? FaqsId { get; set; }

        public virtual Faq? Faqs { get; set; }
    }
}
