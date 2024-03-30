using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Stream
    {
        public Stream()
        {
            Edqualifications = new HashSet<Edqualification>();
        }

        public int StreamId { get; set; }
        public string StreamName { get; set; } = null!;
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual ICollection<Edqualification> Edqualifications { get; set; }
    }
}
