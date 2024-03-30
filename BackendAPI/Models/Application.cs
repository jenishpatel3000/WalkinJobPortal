using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Application
    {
        public Application()
        {
            ApplicationRoles = new HashSet<ApplicationRole>();
        }

        public int ApplicationId { get; set; }
        public string Resume { get; set; } = null!;
        public int UserId { get; set; }
        public int JobId { get; set; }
        public int SlotId { get; set; }
        public string? Hallticket { get; set; }
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual Job Job { get; set; } = null!;
        public virtual Slot Slot { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ApplicationRole> ApplicationRoles { get; set; }
    }
}
