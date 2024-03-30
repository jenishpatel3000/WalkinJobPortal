namespace BackendAPI.DTOS
{
    public class UserRegistrationRequest
    {
        public UserRegistrationRequest()
        {
            ExpertTechsId = new HashSet<int>();
            FamiliarTechsId = new HashSet<int>();
            RolesId = new HashSet<int>();
        }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal PhoneNo { get; set; }
        public string PortfolioUrl { get; set; }
        public string ReferalEmpName { get; set; }
        public sbyte? SendMeUpdate { get; set; }
        public int UserId { get; set; }
        public int Countrycode { get; set; }
        public string Resume { get; set; }
        public string ProfilePhoto { get; set; }
        public decimal? Percentage { get; set; }
        public int? PassingYear { get; set; }
        public int QualificationId { get; set; }
        public int StreamId { get; set; }
        public int CollegeId { get; set; }
        public string OtherCollege { get; set; }
        public string OtherCollegeLocations { get; set; }
        public int? ExpYear { get; set; }
        public decimal? CurrentCtc { get; set; }
        public decimal? ExpectedCtc { get; set; }
        public sbyte? CurrentlyOnNoticePeriod { get; set; }
        public DateTime? NoticeEnd { get; set; }
        public int? NoticePeriodLength { get; set; }
        public sbyte? AppearedZeusTest { get; set; }
        public string ZeusTestRole { get; set; }
        public int ApplicationTypeId { get; set; }
        public virtual ICollection<int> ExpertTechsId { get; set; }
        public virtual ICollection<int> FamiliarTechsId { get; set; }
        public virtual ICollection<int> RolesId { get; set; }
        public string OtherExpertTechs { get; set; }
        public string OtherFamiliarTechs { get; set; }

    }
}
