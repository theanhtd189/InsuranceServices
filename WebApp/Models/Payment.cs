using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Payment
    {
        public Payment()
        {
            PaymentDetails = new HashSet<PaymentDetail>();
        }

        public int Id { get; set; }
        public decimal? Total { get; set; }
        public int? ContractId { get; set; }

        public virtual Contract? Contract { get; set; }
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
