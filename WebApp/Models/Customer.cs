using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Contracts = new HashSet<Contract>();
            Feedbacks = new HashSet<Feedback>();
            Loans = new HashSet<Loan>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
