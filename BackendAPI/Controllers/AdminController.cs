using BackendAPI.DTOS;
using BackendAPI.Models;
using BackendAPI.Services;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace AdminAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly walkin_portalContext _context;
        private IConfiguration _configuration;
        private IAdminService _adminService;
        public AdminController(walkin_portalContext context, IConfiguration configuration, IAdminService adminService)
        {
            _context = context;
            _configuration = configuration;
            _adminService = adminService;
        }
        [HttpPost]
        [Route("/AddJob")]
        public async Task<IActionResult> AddJob(jobRequest request)
        {




            var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationName == request.Venue);
            if (location == null)
            {
                // Location not found, return error
                return NotFound("Location not found");
            }

            // Create a new Job entity and map properties from the jobRequest DTO
            var job = new Job
            {
                JobName = request.JobName,
                FromTime = request.StartDate,
                ToTime = request.EndDate,
                Venue = request.Venue,
                ThingsToRemember = request.ThingsToRemember,
                LocationId = location.LocationId,
                DtCreated = DateTime.Now,
                DtModified = DateTime.Now,
            };

            // Retrieve LocationId based on provided location information




            // Add job entry to database
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            // Add job descriptions
            var jobDesc = new JobDesc
            {
                DescTitle = request.JobTitle,
                Description = request.JobDescription,
                JobId = job.JobId, // Use the generated JobId
                DtCreated = DateTime.Now,
                DtModified = DateTime.Now
            };
            _context.JobDescs.Add(jobDesc);

            // Add role mappings
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.JobRole);
            if (role == null)
            {
                // Role not found, return error
                return NotFound("Role not found");
            }

            var jobRole = new JobRole
            {
                JobId = job.JobId, // Use the generated JobId
                RoleId = role.RoleId,
                Package = request.Package,// Use the generated RoleId
                DtCreated = DateTime.Now,
                DtModified = DateTime.Now
            };
            _context.JobRoles.Add(jobRole);

            // Add slot mappings
            foreach (var slotName in request.TimeSlots)
            {
                var slot = await _context.Slots.FirstOrDefaultAsync(s => s.FromTime.ToString() == slotName);
                if (slot != null)
                {
                    var jobSlot = new JobSlot
                    {
                        JobId = job.JobId, // Use the generated JobId
                        SlotId = slot.SlotId,
                        DtCreated = DateTime.Now,
                        DtModified = DateTime.Now
                    };
                    _context.JobSlots.Add(jobSlot);
                }
            }

            await _context.SaveChangesAsync();

            return Ok();


        }
        [HttpGet]
        [Route("/Get/{id}")]
        public async Task<ActionResult<jobDto>> GetJob(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.Location)
                .Include(j => j.JobDescs)
                .Include(j => j.JobRoles)
                    .ThenInclude(jr => jr.Role)
                .Include(j => j.JobSlots)
                    .ThenInclude(js => js.Slot)

                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
            {
                return NotFound(); // Returns a 404 Not Found response if job with given ID is not found
            }
            var endDate = job.Date.AddDays(10);
            // Map the job entity to the JobDTO
            var jobDTO = new jobDto
            {
                JobName = job.JobName,

                JobRole = job.JobRoles.FirstOrDefault()?.Role?.RoleName,
                JobTitle = job.JobDescs.FirstOrDefault()?.DescTitle,
                JobDescription = job.JobDescs.FirstOrDefault()?.Description,
                StartDate = job.Date,
                EndDate = endDate,
                Venue = job.Location?.LocationName,
                ThingsToRemember = job.ThingsToRemember,
                TimeSlots = job.JobSlots.Select(js => $"{js.Slot.FromTime}-{js.Slot.ToTime}").ToList(),
                Package = job.JobRoles.FirstOrDefault()?.Package,
                // Include the LocationName field from the Location table
            };

            return Ok(jobDTO); // Returns the job DTO with the given ID
        }
        [HttpPost]
        [Route("/loginAdmin")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            Console.WriteLine(loginRequest);
            var user = await _adminService.AuthenticateUser(loginRequest.username, loginRequest.password, loginRequest.rememberMe);
            Console.WriteLine(user);
            if (user == null)
            {
                // Unauthorized: Invalid username or password
                return Unauthorized();
            }

            // If authentication is successful, return a JWT token
            var token = GenerateJwtToken(user.username);
            return Ok(new { Token = token });
        }
        private string GenerateJwtToken(string username)
        {
            // Convert userId to string
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtConfig:key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
                {
        new Claim(ClaimTypes.Name, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var tokenDescriptor = new JwtSecurityToken
            (
                _configuration["jwtConfig:Issuer"],
                _configuration["jwtConfig:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: signIn

            //Subject = new ClaimsIdentity(claims),
            //Expires = DateTime.UtcNow.AddHours(10), // Token expiration time
            //SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            );

            // var token = new JwtSecurityTokenHandler(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        [HttpPut]
        [Route("/Update/{id}")]
        public async Task<IActionResult> UpdateJob(int id, jobDto request)
        {


            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            // Update job entity properties with values from the request DTO
            job.JobName = request.JobName;
            job.FromTime = request.StartDate;
            job.ToTime = request.EndDate;
            job.Venue = request.Venue;
            job.ThingsToRemember = request.ThingsToRemember;

            // Update location if it exists
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationName == request.Venue);
            if (location != null)
            {
                job.LocationId = location.LocationId;
            }

            // Update job descriptions
            var jobDesc = await _context.JobDescs.FirstOrDefaultAsync(jd => jd.JobId == id);
            if (jobDesc != null)
            {
                jobDesc.DescTitle = request.JobTitle;
                jobDesc.Description = request.JobDescription;
            }

            // Update job role
            var jobRole = await _context.JobRoles.FirstOrDefaultAsync(jr => jr.JobId == id);
            if (jobRole != null)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.JobRole);
                if (role != null)
                {
                    jobRole.RoleId = role.RoleId;
                    jobRole.Package = request.Package;
                }
            }

            // Update job slots (optional based on your requirement)

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {


            }

            return NoContent();
        }

        // DELETE: api/Jobs/5
        [HttpDelete]
        [Route("/Delete/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            var relatedApplications = await _context.Applications
                                        .Where(a => a.JobId == id)
                                        .ToListAsync();

            // Remove all related applications
            _context.Applications.RemoveRange(relatedApplications);

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost]
        [Route("/forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            // Check if the email exists in the database
            var user = await _context.Adminusers.FirstOrDefaultAsync(u => u.Email == forgotPasswordRequest.Email);
            if (user == null)
            {
                // Email not found, return NotFound
                return NotFound("User with provided email not found");
            }

            // Generate a unique token for password reset
            var token = GeneratePasswordResetToken(user);
            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Subject = "Password Reset",
                Body = $"Please click the following link to reset your password: <a href='http://localhost:4200/reset-password?token={token}'>Reset Password</a>"
            };
            try
            {

                await _adminService.SendEmailAsync(mailRequest);
                return Ok(new { message = "Password reset email sent successfully" });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during email sending
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending password reset email");
            }
            // Send the password reset email with the token to the user's email address
            // Implement your email sending logic here


        }
        [HttpPost]
        [Route("/reject-application")]
        public async Task<IActionResult> ApplicationState(ApplicationStatusRequest applicationStatusRequest)
        {
            // Check if the email exists in the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == applicationStatusRequest.Email);
            if (user == null)
            {
                // Email not found, return NotFound
                return NotFound("User with provided email not found");
            }

            // Generate a unique token for password reset
            //var token = GeneratePasswordResetToken(user);
            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Subject = "Application Status",
                Body = $"We regret to inform {applicationStatusRequest.userName} that your application for Job at Zeus Learning has been rejected Wish you best of luck for future career"
            };
            try
            {

                await _adminService.SendEmailAsync(mailRequest);
                return Ok(new { message = "email sent successfully" });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during email sending
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending password reset email");
            }
            // Send the password reset email with the token to the user's email address
            // Implement your email sending logic here


        }
        [HttpPost]
        [Route("/select-application")]
        public async Task<IActionResult> ApplicationStatus(ApplicationStatusRequest applicationStatusRequest)
        {
            // Check if the email exists in the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == applicationStatusRequest.Email);
            if (user == null)
            {
                // Email not found, return NotFound
                return NotFound("User with provided email not found");
            }

            // Generate a unique token for password reset
            //var token = GeneratePasswordResetToken(user);
            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Subject = "Application Status",
                Body = $"We are happy to inform{applicationStatusRequest.userName}   that your application for Job at Zeus Learning has been select your date and time of interview is same as you selected Wish you best of luck for future career"
            };
            try
            {

                await _adminService.SendEmailAsync(mailRequest);
                return Ok(new { message = "email sent successfully" });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during email sending
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending password reset email");
            }
            // Send the password reset email with the token to the user's email address
            // Implement your email sending logic here


        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send(MailRequest request)
        {

            await _adminService.SendEmailAsync(request);
            return Ok();



        }


        private string GeneratePasswordResetToken(Adminuser user)
        {
            // Generate a unique token for password reset
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["jwtConfig:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.UserId.ToString()),
                     new Claim("Email", user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10), // Token expiration time (e.g., 1 hour)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [HttpPut]
        [Route("/reset-password")]

        public async Task<IActionResult> ResetPassword(UpdatePasswordRequest resetPasswordRequest)
        {
            // Your logic to verify the token and update the password in the database
            // Make sure to validate the token and ensure its authenticity

            // Example logic:
            var user = await _context.Adminusers.FirstOrDefaultAsync(u => u.Email == resetPasswordRequest.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update the user's password with the new password
            user.Password = resetPasswordRequest.NewPassword;
            _context.Adminusers.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Password updated successfully");
        }
        [HttpGet]
        [Route("/getApplications")]
        public async Task<ActionResult<object>> GetAllApplicationDetails()
        {




            var applicationDetails = await _context.Applications
                .Include(a => a.User)
        .ThenInclude(u => u.Userassets)
                .Include(a => a.User)
                .ThenInclude(u => u.Userdetails)
                .Include(a => a.User)
        .ThenInclude(u => u.Edqualifications)
            .ThenInclude(eq => eq.Qualification)
    .Include(a => a.User)
        .ThenInclude(u => u.Edqualifications)
            .ThenInclude(eq => eq.Stream)
    .Include(a => a.User)
        .ThenInclude(u => u.Edqualifications)
            .ThenInclude(eq => eq.College)
                .Include(a => a.ApplicationRoles)
                .ThenInclude(ar => ar.Role)
                .Include(a => a.User.Proqualifications)
                .ThenInclude(pq => pq.ApplicationType)
                .Include(a => a.User.Proqualifications)
                .ThenInclude(pq => pq.ProqualificationFamiliarteches)
                .ThenInclude(pft => pft.Tech)
                .Include(a => a.Slot)
                .ToListAsync();

            if (applicationDetails == null || !applicationDetails.Any())
            {

                return NotFound();
            }



            var response = applicationDetails.Select(a => new
            {
                ApplicationId = a.ApplicationId,

                Email = a.User?.Email,
                Name = a.User?.Userdetails?.FirstOrDefault()?.FirstName + " " + a.User?.Userdetails?.FirstOrDefault()?.LastName,
                ProfilePhoto = a.User?.Userassets?.FirstOrDefault()?.ProfilePhoto,
                //LastName = a.User?.Userdetails?.FirstOrDefault()?.LastName,
                MobileNo = a.User?.Userdetails?.FirstOrDefault()?.PhoneNo,
                QualificationName = a.User?.Edqualifications?.FirstOrDefault()?.Qualification?.QualificationName,
                StreamName = a.User?.Edqualifications?.FirstOrDefault()?.Stream?.StreamName,
                CollegeName = a.User?.Edqualifications?.FirstOrDefault()?.College?.CollegeName,
                PassingYear = a.User?.Edqualifications?.FirstOrDefault()?.PassingYear,
                Percentage = a.User?.Edqualifications?.FirstOrDefault()?.Percentage,
                // RoleName = a.ApplicationRoles?.FirstOrDefault()?.Role?.RoleName,
                RoleNames = a.ApplicationRoles?.Select(ar => ar.Role?.RoleName).ToList(),
                ApplicationTypeName = a.User?.Proqualifications?.FirstOrDefault()?.ApplicationType?.ApplicationTypeName,
                //TechnologyName = a.User?.Proqualifications?.FirstOrDefault()?.ProqualificationFamiliarteches?.FirstOrDefault()?.Tech?.TechName,
                TechnologyNames = a.User?.Proqualifications?.FirstOrDefault()?.ProqualificationFamiliarteches
                        ?.Select(pt => pt.Tech?.TechName).ToList(),
                ExpectedCtc = a.User?.Proqualifications?.FirstOrDefault()?.ExpectedCtc,
                TimeSlot = $"{a.Slot?.FromTime}-{a.Slot?.ToTime}"
            }).ToList();


            return response;

        }

    }
}



