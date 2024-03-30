using BackendAPI.DTOS;
using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IConfiguration _configuration;
        private readonly walkinportalContext _context;
        public UserController(IUserService userService, walkinportalContext context, IConfiguration configuration)
        {
            _userService = userService;
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/jobs")]
        public async Task<IActionResult> GetAllJobsAsync()
        {
            var jobDtoList = await _userService.GetAllJobsAsync();
            Console.Write(jobDtoList);
            return Ok(jobDtoList);
        }

        [HttpGet]
        [Route("/job/{id}")]
        public async Task<IActionResult> GetJobByIdAsync([FromRoute] int id)
        {
            var job = await _userService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }

        [HttpPost]
        [Route("/apply")]
        public async Task<IActionResult> InsertApplicationAsync([FromBody] ApplicationRequest application)
        {
            Int32 applicationId = await _userService.InsertApplicationAsync(application);
            return Ok(applicationId);
        }
        [HttpPost]
        
        [Route("/login")]
        public async Task<IActionResult>LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userService.AuthenticateUser(loginRequest.username, loginRequest.password, loginRequest.rememberMe);
            var userObject = await _context.Users.Where(u => u.Email == loginRequest.username && u.Password == loginRequest.password).SingleOrDefaultAsync();
            if (user == null)
            {
                return Unauthorized();
            }
            var token = GenerateJwtToken(user.username);
            return Ok(new { Token = token, userObject.UserId });
        }
        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtConfig:key"]));
            var signIn=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
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
                expires:DateTime.UtcNow.AddHours(10),
                signingCredentials:signIn
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        [HttpGet]
        [Route("/getapplication/{applicationId}")]
        public async Task<IActionResult> GetApplicationByIdAsync([FromRoute] int applicationId)
        {
           
            var application = await _userService.GetApplicationByIdAsync(applicationId);
            return Ok(application);
        }

        [HttpPost]
        [Route("/user")]
        public async Task<IActionResult> RegisterUser(UserRegistrationRequest userRegistrationRequest)
        {


            await _userService.RegisterUser(userRegistrationRequest);

            return Ok();
        }
        [HttpGet]
        [Route("/getregistrationdata")]
        public async Task<IActionResult> getRegistrationDataAsync()
        {
            
            var registrationData = await _userService.getRegistrationDataAsync();
            return Ok(registrationData);
        }
    }
}
