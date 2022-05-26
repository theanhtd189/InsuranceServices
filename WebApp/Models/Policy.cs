using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Policy
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? InsuranceId { get; set; }

        public virtual Insurance? Insurance { get; set; }
    }
}
