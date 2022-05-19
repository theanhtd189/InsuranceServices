using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Insurance
    {
        public Insurance()
        {
            Contracts = new HashSet<Contract>();
            InsuarancePolicies = new HashSet<InsuarancePolicy>();
            InsurancePolies = new HashSet<InsurancePoly>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? NumberOfMonth { get; set; }
        public double? Rate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal? SumInsured { get; set; }
        public decimal? TotalFee { get; set; }
        public string? InsuredObject { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<InsuarancePolicy> InsuarancePolicies { get; set; }
        public virtual ICollection<InsurancePoly> InsurancePolies { get; set; }
    }
}
