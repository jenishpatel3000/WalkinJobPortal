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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IConfiguration _configuration;
        private readonly walkin_portalContext _context;
        // private const string SecretKey = "sRwvYz$LtzB#WqEf!aTdDgHkMnOpQrSt"; // Replace with your actual secret key
        // private readonly SymmetricSecurityKey _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes());
      
        public UserController(IUserService userService, walkin_portalContext context, IConfiguration configuration)
        {
            _userService = userService;
            _context = context;
            _configuration = configuration;
        }
        //  private readonly SymmetricSecurityKey _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtConfig:key"]));

        [HttpGet]
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
                // Unauthorized: Invalid username or password
                return Unauthorized();
            }

            // If authentication is successful, return a JWT token
            var token = GenerateJwtToken(user.username);
            return Ok(new { Token = token, userObject.UserId });
        }
        private string GenerateJwtToken(string username)
        {
             // Convert userId to string
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

                //Subject = new ClaimsIdentity(claims),
                //Expires = DateTime.UtcNow.AddHours(10), // Token expiration time
                //SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            );

           // var token = new JwtSecurityTokenHandler(tokenDescriptor);
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
