using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Contract
    {
        public Contract()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string? Beneficiary { get; set; }
        public int Duration { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpriredAt { get; set; }
        public int? CustomerId { get; set; }
        public int? InsuranceId { get; set; }
        public bool? Status { get; set; }
        public decimal Total { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Insurance? Insurance { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
