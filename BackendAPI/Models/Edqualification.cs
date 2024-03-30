using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Edqualification
    {
        public int EdqualificationId { get; set; }
        public decimal? Percentage { get; set; }
        public int? PassingYear { get; set; }
        public int QualificationId { get; set; }
        public int StreamId { get; set; }
        public int CollegeId { get; set; }
        public int UserId { get; set; }
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }
        public string? OtherCollege { get; set; }
        public string? OtherCollegeLocation { get; set; }

        public virtual College College { get; set; } = null!;
        public virtual Qualification Qualification { get; set; } = null!;
        public virtual Stream Stream { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
