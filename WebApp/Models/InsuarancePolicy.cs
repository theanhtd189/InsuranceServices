using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class InsuarancePolicy
    {
        public int Id { get; set; }
        public int? InsuarenceId { get; set; }
        public int? PolicyId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Insurance? Insuarence { get; set; }
        public virtual Policy? Policy { get; set; }
    }
}
