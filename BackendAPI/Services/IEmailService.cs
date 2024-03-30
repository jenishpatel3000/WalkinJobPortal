namespace BackendAPI.Services
{
    public interface IEmailService
    {
        public Task SendTransactionalEmailAsync(string jsonBody);
    }
}
