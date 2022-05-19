using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class PaymentDetail
    {
        public int Id { get; set; }
        public string? ContentDetails { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PaymentId { get; set; }
        public int? PeriodicId { get; set; }
        public decimal? Total { get; set; }
        public decimal? AmountOfChange { get; set; }

        public virtual Payment? Payment { get; set; }
        public virtual PeriodicPaymentMethod? Periodic { get; set; }
    }
}
