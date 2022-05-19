using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class InsurancePoly
    {
        public int Id { get; set; }
        public int? InsuranceId { get; set; }
        public int? PolicyId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Insurance? Insurance { get; set; }
        public virtual Policy? Policy { get; set; }
    }
}
