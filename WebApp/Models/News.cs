using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class News
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
    }
}
