using BackendAPI.DTOS;

namespace BackendAPI.Services
{
    public interface IAdminService
    {
        
        public Task<LoginRequest> AuthenticateUser(string username, string password, bool RememberMe);
        public Task SendEmailAsync(MailRequest mailRequest);
    }
}
