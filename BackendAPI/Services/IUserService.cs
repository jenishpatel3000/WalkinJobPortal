using BackendAPI.DTOS;
using BackendAPI.Models;

namespace BackendAPI.Services
{
    public interface IUserService
    {
        public Task<List<Job>> GetAllJobsAsync();
        public Task<Job> GetJobByIdAsync(int jobId);
        public Task<Int32> InsertApplicationAsync(ApplicationRequest application);
        public Task<Application> GetApplicationByIdAsync(int applicationId);
        public Task<LoginRequest> AuthenticateUser(string username, string password,bool RememberMe);
        public Task RegisterUser(UserRegistrationRequest userRegistrationRequest);
        public Task<RegistrationData> getRegistrationDataAsync();
    }
}
