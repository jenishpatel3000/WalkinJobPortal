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
        private readonly walkinportalContext _context;
        private IConfiguration _configuration;
        private IAdminService _adminService;
        public AdminController(walkinportalContext context, IConfiguration configuration, IAdminService adminService)
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
                return NotFound("Location not found");
            }

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

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            var jobDesc = new JobDesc
            {
                DescTitle = request.JobTitle,
                Description = request.JobDescription,
                JobId = job.JobId,
                DtCreated = DateTime.Now,
                DtModified = DateTime.Now
            };
            _context.JobDescs.Add(jobDesc);

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.JobRole);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            var jobRole = new JobRole
            {
                JobId = job.JobId,
                RoleId = role.RoleId,
                Package = request.Package,
                DtCreated = DateTime.Now,
                DtModified = DateTime.Now
            };
            _context.JobRoles.Add(jobRole);

            foreach (var slotName in request.TimeSlots)
            {
                var slot = await _context.Slots.FirstOrDefaultAsync(s => s.FromTime.ToString() == slotName);
                if (slot != null)
                {
                    var jobSlot = new JobSlot
                    {
                        JobId = job.JobId,
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
                return NotFound();
            }
            var endDate = job.Date.AddDays(10);
            
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
            };

            return Ok(jobDTO);
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
                return Unauthorized();
            }
            var userObject = await _context.Adminusers.Where(u => u.Email == loginRequest.username & u.Password == loginRequest.password).FirstOrDefaultAsync();

            var token = GenerateJwtToken(user.username);
            return Ok(new { Token = token, adminId = userObject.UserId});
        }

        private string GenerateJwtToken(string username)
        {
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
            );
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

            job.JobName = request.JobName;
            job.FromTime = request.StartDate;
            job.ToTime = request.EndDate;
            job.Venue = request.Venue;
            job.ThingsToRemember = request.ThingsToRemember;

            var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationName == request.Venue);
            if (location != null)
            {
                job.LocationId = location.LocationId;
            }

            var jobDesc = await _context.JobDescs.FirstOrDefaultAsync(jd => jd.JobId == id);
            if (jobDesc != null)
            {
                jobDesc.DescTitle = request.JobTitle;
                jobDesc.Description = request.JobDescription;
            }

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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return NoContent();
        }

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

            _context.Applications.RemoveRange(relatedApplications);

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost]
        [Route("/forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var user = await _context.Adminusers.FirstOrDefaultAsync(u => u.Email == forgotPasswordRequest.Email);
            if (user == null)
            {
                return NotFound("User with provided email not found");
            }

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
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending password reset email");
            }
        }
        [HttpPost]
        [Route("/reject-application")]
        public async Task<IActionResult> ApplicationState(ApplicationStatusRequest applicationStatusRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == applicationStatusRequest.Email);
            if (user == null)
            {
                return NotFound("User with provided email not found");
            }
            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Subject = "Application Status",
                Body = $"We regret to inform {applicationStatusRequest.userName} that your application for Job at Zeus Learning has been rejected Wish you best of luck for future career"
            };
            try
            {
                var application = await _context.Applications.FirstOrDefaultAsync(a => a.UserId==user.UserId);
                var roles = await _context.ApplicationRoles.Where(r => r.ApplicationId == application.ApplicationId).ToListAsync();
                foreach (var role in roles)
                {
                    _context.ApplicationRoles.Remove(role);
                }
                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
                await _adminService.SendEmailAsync(mailRequest);
                return Ok(new { message = "email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending password reset email");
            }
        }
        [HttpPost]
        [Route("/select-application")]
        public async Task<IActionResult> ApplicationStatus(ApplicationStatusRequest applicationStatusRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == applicationStatusRequest.Email);
            if (user == null)
            {
                return NotFound("User with provided email not found");
            }
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending password reset email");
            }
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send(MailRequest request)
        {
            await _adminService.SendEmailAsync(request);
            return Ok();
        }


        private string GeneratePasswordResetToken(Adminuser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["jwtConfig:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.UserId.ToString()),
                     new Claim("Email", user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [HttpPut]
        [Route("/reset-password")]

        public async Task<IActionResult> ResetPassword(UpdatePasswordRequest resetPasswordRequest)
        {
            var user = await _context.Adminusers.FirstOrDefaultAsync(u => u.Email == resetPasswordRequest.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }
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
                Resume = a.User?.Userassets.FirstOrDefault()?.Resume,
                MobileNo = a.User?.Userdetails?.FirstOrDefault()?.PhoneNo,
                QualificationName = a.User?.Edqualifications?.FirstOrDefault()?.Qualification?.QualificationName,
                StreamName = a.User?.Edqualifications?.FirstOrDefault()?.Stream?.StreamName,
                CollegeName = a.User?.Edqualifications?.FirstOrDefault()?.College?.CollegeName,
                PassingYear = a.User?.Edqualifications?.FirstOrDefault()?.PassingYear,
                Percentage = a.User?.Edqualifications?.FirstOrDefault()?.Percentage,
                RoleNames = a.ApplicationRoles?.Select(ar => ar.Role?.RoleName).ToList(),
                ApplicationTypeName = a.User?.Proqualifications?.FirstOrDefault()?.ApplicationType?.ApplicationTypeName,
                TechnologyNames = a.User?.Proqualifications?.FirstOrDefault()?.ProqualificationFamiliarteches?.Select(pt => pt.Tech?.TechName).ToList(),
                ExpectedCtc = a.User?.Proqualifications?.FirstOrDefault()?.ExpectedCtc,
                TimeSlot = $"{a.Slot?.FromTime}-{a.Slot?.ToTime}"
            }).ToList();
            return response;
        }
    }
}