using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Loan
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal? Amount { get; set; }
        public int? CustomerId { get; set; }
        public int? Duration { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool? Status { get; set; }

        public virtual Customer? Customer { get; set; }
    }
}
