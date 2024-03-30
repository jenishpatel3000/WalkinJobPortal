using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class User
    {
        public User()
        {
            Applications = new HashSet<Application>();
            Edqualifications = new HashSet<Edqualification>();
            Proqualifications = new HashSet<Proqualification>();
            Userassets = new HashSet<Userasset>();
            Userdetails = new HashSet<Userdetail>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Edqualification> Edqualifications { get; set; }
        public virtual ICollection<Proqualification> Proqualifications { get; set; }
        public virtual ICollection<Userasset> Userassets { get; set; }
        public virtual ICollection<Userdetail> Userdetails { get; set; }
    }
}
