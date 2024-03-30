using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Slot
    {
        public Slot()
        {
            Applications = new HashSet<Application>();
            JobSlots = new HashSet<JobSlot>();
        }

        public int SlotId { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<JobSlot> JobSlots { get; set; }
    }
}
