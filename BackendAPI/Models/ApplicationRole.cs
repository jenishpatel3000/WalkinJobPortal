using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class ApplicationRole
    {
        public int RoleId { get; set; }
        public int ApplicationId { get; set; }
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual Application Application { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
