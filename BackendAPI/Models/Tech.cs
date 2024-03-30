using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Tech
    {
        public Tech()
        {
            ProqualificationExpertteches = new HashSet<ProqualificationExperttech>();
            ProqualificationFamiliarteches = new HashSet<ProqualificationFamiliartech>();
        }

        public int TechId { get; set; }
        public string TechName { get; set; } = null!;
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual ICollection<ProqualificationExperttech> ProqualificationExpertteches { get; set; }
        public virtual ICollection<ProqualificationFamiliartech> ProqualificationFamiliarteches { get; set; }
    }
}
