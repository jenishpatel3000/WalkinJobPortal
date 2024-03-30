namespace BackendAPI.DTOS
{
    public class ApplicationRequest
    {
        public ApplicationRequest()
        {
            Rolesid = new HashSet<int>();
        }

        public string Resume { get; set; }
        public int UserId { get; set; }
        public int JobId { get; set; }
        public int SlotId { get; set; }

        public virtual ICollection<int> Rolesid { get; set; }
    }
}
