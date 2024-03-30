using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Net.Http;

namespace BackendAPI.Services
{
    public class EmailService:IEmailService
    {
        private readonly string apiKey = "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";
        private readonly string apiEndpoint = "https://api.sendinblue.com/v3/smtp/email";

        public async Task SendTransactionalEmailAsync(string jsonBody)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("api-key", apiKey);
            using HttpRequestMessage request = new(HttpMethod.Post, apiEndpoint);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Email sent successfully!");
            }
            else
            {
                Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
                Console.WriteLine($"Response body: {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
