using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Job
    {
        public Job()
        {
            Applications = new HashSet<Application>();
            JobDescs = new HashSet<JobDesc>();
            JobRoles = new HashSet<JobRole>();
            JobSlots = new HashSet<JobSlot>();
        }

        public int JobId { get; set; }
        public string JobName { get; set; } = null!;
        public DateTime? FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string Venue { get; set; } = null!;
        public string? ThingsToRemember { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual Location Location { get; set; } = null!;
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<JobDesc> JobDescs { get; set; }
        public virtual ICollection<JobRole> JobRoles { get; set; }
        public virtual ICollection<JobSlot> JobSlots { get; set; }
    }
}
