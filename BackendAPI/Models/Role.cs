using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Role
    {
        public Role()
        {
            ApplicationRoles = new HashSet<ApplicationRole>();
            JobRoles = new HashSet<JobRole>();
            UserdetailsRoles = new HashSet<UserdetailsRole>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual ICollection<ApplicationRole> ApplicationRoles { get; set; }
        public virtual ICollection<JobRole> JobRoles { get; set; }
        public virtual ICollection<UserdetailsRole> UserdetailsRoles { get; set; }
    }
}
