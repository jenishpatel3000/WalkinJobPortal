using BackendAPI.DTOS;
using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BackendAPI.Services
{
    public class AdminService:IAdminService
    {
        private readonly walkinportalContext _context;
        private readonly MailSettings _mailSettings;
        public AdminService(walkinportalContext context, IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _mailSettings = mailSettings.Value;
        }
        public async Task<LoginRequest> AuthenticateUser(string username, string password, bool RememberMe)
        {
            var user = await _context.Adminusers.FirstOrDefaultAsync(u => u.Email == username && u.Password == password);

            if (user == null)
            {
                return null;
            }

            var userDto = new LoginRequest
            {
                username = user.Email,
                password = user.Password,
                rememberMe = RememberMe
            };

            return userDto;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
           
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
