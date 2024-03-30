using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Userdetail
    {
        public Userdetail()
        {
            UserdetailsRoles = new HashSet<UserdetailsRole>();
        }

        public int UserdetailId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public decimal PhoneNo { get; set; }
        public string? PortfolioUrl { get; set; }
        public string? ReferalEmpName { get; set; }
        public sbyte? SendMeUpdate { get; set; }
        public int UserId { get; set; }
        public int Countrycode { get; set; }
        public DateTime? DtCreated { get; set; }
        public DateTime? DtModified { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<UserdetailsRole> UserdetailsRoles { get; set; }
    }
}
