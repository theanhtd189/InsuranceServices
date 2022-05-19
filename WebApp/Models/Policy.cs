using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Policy
    {
        public Policy()
        {
            InsuarancePolicies = new HashSet<InsuarancePolicy>();
            InsurancePolies = new HashSet<InsurancePoly>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<InsuarancePolicy> InsuarancePolicies { get; set; }
        public virtual ICollection<InsurancePoly> InsurancePolies { get; set; }
    }
}
