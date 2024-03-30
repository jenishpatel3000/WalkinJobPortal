using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Adminuser
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }
    }
}
