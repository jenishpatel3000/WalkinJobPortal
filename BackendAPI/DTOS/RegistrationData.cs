using BackendAPI.Models;
using Stream = BackendAPI.Models.Stream;

namespace BackendAPI.DTOS
{
    public class RegistrationData
    {
        public RegistrationData()
        {
            college = new List<College>();
            location = new List<Location>();
            stream = new List<Stream>();
            qualification = new List<Qualification>();
            tech = new List<Tech>();
            role = new List<Role>();
            applicationTypes = new List<ApplicationType>();
        }
        public List<College> college { get; set; }
        public List<Location> location { get; set; }
        public List<Stream> stream { get; set; }
        public List<Qualification> qualification { get; set; }
        public List<Tech> tech { get; set; }
        public List<Role> role { get; set; }
        public List<ApplicationType> applicationTypes { get; set; }
    }
}
