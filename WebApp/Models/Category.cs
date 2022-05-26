using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Category
    {
        public Category()
        {
            Insurances = new HashSet<Insurance>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Insurance> Insurances { get; set; }
    }
}
