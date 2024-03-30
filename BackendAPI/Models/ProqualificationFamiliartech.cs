using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class ProqualificationFamiliartech
    {
        public int TechId { get; set; }
        public int ProqualificationId { get; set; }
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual Proqualification Proqualification { get; set; } = null!;
        public virtual Tech Tech { get; set; } = null!;
    }
}
