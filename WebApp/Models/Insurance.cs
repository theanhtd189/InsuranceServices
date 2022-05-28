using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Insurance
    {
        public Insurance()
        {
            Contracts = new HashSet<Contract>();
            Feedbacks = new HashSet<Feedback>();
            Policies = new HashSet<Policy>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal Amount { get; set; }
        public string? Image { get; set; }
        public decimal Claim { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Policy> Policies { get; set; }
    }
}
