using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class PeriodicPaymentMethod
    {
        public PeriodicPaymentMethod()
        {
            PaymentDetails = new HashSet<PaymentDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int NumberOfMonth { get; set; }

        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
