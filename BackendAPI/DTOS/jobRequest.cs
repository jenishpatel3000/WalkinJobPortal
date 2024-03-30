namespace BackendAPI.DTOS
{
    public class jobRequest
    {
        public string JobName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobRole { get; set; }
        public string RoleTitle { get; set; }
        public string RoleDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Venue { get; set; }
        public string ThingsToRemember { get; set; }
        public List<string> TimeSlots { get; set; }
        public int Package { get; set; }
    }
}
