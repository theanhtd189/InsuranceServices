using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Admin
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
