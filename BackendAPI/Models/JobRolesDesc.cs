using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class JobRolesDesc
    {
        public int Id { get; set; }
        public string? DescTitle { get; set; }
        public string? Description { get; set; }
        public int RolesId { get; set; }
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual JobRole Roles { get; set; } = null!;
    }
}
