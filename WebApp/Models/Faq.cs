using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Faq
    {
        public Faq()
        {
            Replies = new HashSet<Reply>();
        }

        public int Id { get; set; }
        public string? Question { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }
    }
}
