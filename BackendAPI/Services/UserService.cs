using BackendAPI.DTOS;
using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using Newtonsoft.Json.Linq;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using Microsoft.AspNetCore.Mvc;
using iText.Layout.Element;
using iText.StyledXmlParser.Jsoup.Nodes;
using System.Data;
using System.IO;
using System.Reflection.Metadata;
using System.Diagnostics;
namespace BackendAPI.Services
{
    public class UserService:IUserService
    {
        private readonly walkinportalContext _context;
        private readonly IEmailService _emailService;
        public UserService(walkinportalContext context)
        {
            _context = context;
            _emailService = new EmailService();
        }
        public async Task<RegistrationData> getRegistrationDataAsync()
        {
            var collegesTask = await _context.Colleges.ToListAsync();
            var streamsTask = await _context.Streams.ToListAsync();
            var locationsTask = await _context.Locations.ToListAsync();
            var techsTask = await _context.Techs.ToListAsync();
            var qualificationsTask = await _context.Qualifications.ToListAsync();
            var rolesTask = await _context.Roles.ToListAsync();
            var applicationTypesTask = await _context.ApplicationTypes.ToListAsync();


            var registrationData = new RegistrationData
            {
                college = collegesTask,
                location = locationsTask,
                stream = streamsTask,
                qualification = qualificationsTask,
                tech = techsTask,
                role = rolesTask,
                applicationTypes = applicationTypesTask
            };
            return registrationData;
        }
        public async Task<List<Job>> GetAllJobsAsync()
        {
            var jobs = await _context.Jobs
                .Include(j => j.Location)
                .Include(j => j.JobSlots)
                    .ThenInclude(s => s.Slot)
                .Include(j => j.JobDescs)
                .Include(j => j.JobRoles)
                    .ThenInclude(r => r.Role)
                .Include(j => j.JobRoles)
                    .ThenInclude(r => r.JobRolesDescs)
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();

            var jobDtoList = jobs.Select(job => new Job
            {
                JobId = job.JobId,
                JobName = job.JobName,
                FromTime = job.FromTime,
                ToTime = job.ToTime,
                Venue = job.Venue,
                ThingsToRemember = job.ThingsToRemember,
                LocationId = job.LocationId,
                Location = new Location
                {
                    LocationId = job.Location.LocationId,
                    LocationName = job.Location.LocationName
                },
                JobDescs = job.JobDescs.Select(jd => new JobDesc
                {
                    Id = jd.Id,
                    DescTitle = jd.DescTitle,
                    Description = jd.Description,
                    JobId = jd.JobId
                }).ToList(),
                JobSlots = job.JobSlots.Select(js => new JobSlot
                {
                    SlotId = js.SlotId,
                    JobId = js.JobId,
                    Slot = new Slot
                    {
                        SlotId = js.Slot.SlotId,
                        FromTime = js.Slot.FromTime,
                        ToTime = js.Slot.ToTime
                    },
                }).ToList(),
                JobRoles = job.JobRoles.Select(jr => new JobRole
                {
                    Id = jr.Id,
                    Package = jr.Package,
                    JobId = jr.JobId,
                    RoleId = jr.RoleId,
                    Role = new Role
                    {
                        RoleId = jr.Role.RoleId,
                        RoleName = jr.Role.RoleName
                    },
                    JobRolesDescs = jr.JobRolesDescs.Select(jrd => new JobRolesDesc
                    {
                        Id = jrd.Id,
                        DescTitle = jrd.DescTitle,
                        Description = jrd.Description,
                        RolesId = jrd.RolesId
                    }).ToList(),
                }).ToList(),
            }).ToList();
            return jobDtoList;
        }

        public async Task<Job> GetJobByIdAsync(int jobId)
        {
            var job = await _context.Jobs
                .Where(j => j.JobId == jobId)
                .Include(j => j.Location)
                .Include(j => j.JobSlots)
                    .ThenInclude(s => s.Slot)
                .Include(j => j.JobDescs)
                .Include(j => j.JobRoles)
                    .ThenInclude(r => r.Role)
                .Include(j => j.JobRoles)
                    .ThenInclude(r => r.JobRolesDescs)
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return null;
            }

            var jobDto = new Job
            {
                JobId = job.JobId,
                JobName = job.JobName,
                FromTime = job.FromTime,
                ToTime = job.ToTime,
                Venue = job.Venue,
                ThingsToRemember = job.ThingsToRemember,
                LocationId = job.LocationId,
                DtCreated = job.DtCreated,
                DtModified = job.DtModified,
                Location = new Location
                {
                    LocationId = job.Location.LocationId,
                    LocationName = job.Location.LocationName,
                    DtCreated = job.Location.DtCreated,
                    DtModified = job.Location.DtModified,
                },
                JobDescs = job.JobDescs.Select(jd => new JobDesc
                {
                    Id = jd.Id,
                    DescTitle = jd.DescTitle,
                    Description = jd.Description,
                    JobId = jd.JobId,
                    DtCreated = jd.DtCreated,
                    DtModified = jd.DtModified,
                }).ToList(),
                JobSlots = job.JobSlots.Select(js => new JobSlot
                {
                    SlotId = js.SlotId,
                    JobId = js.JobId,
                    DtCreated = js.DtCreated,
                    DtModified = js.DtModified,
                    Slot = new Slot
                    {
                        SlotId = js.Slot.SlotId,
                        FromTime = js.Slot.FromTime,
                        ToTime = js.Slot.ToTime,
                        DtCreated = js.Slot.DtCreated,
                        DtModified = js.Slot.DtModified,
                    },
                }).ToList(),
                JobRoles = job.JobRoles.Select(jr => new JobRole
                {
                    Id = jr.Id,
                    Package = jr.Package,
                    JobId = jr.JobId,
                    RoleId = jr.RoleId,
                    DtCreated = jr.DtCreated,
                    DtModified = jr.DtModified,
                    Role = new Role
                    {
                        RoleId = jr.Role.RoleId,
                        RoleName = jr.Role.RoleName,
                        DtCreated = jr.Role.DtCreated,
                        DtModified = jr.Role.DtModified,
                    },
                    JobRolesDescs = jr.JobRolesDescs.Select(jrd => new JobRolesDesc
                    {
                        Id = jrd.Id,
                        DescTitle = jrd.DescTitle,
                        Description = jrd.Description,
                        RolesId = jrd.RolesId,
                        DtCreated = jrd.DtCreated,
                        DtModified = jrd.DtModified,
                    }).ToList(),
                }).ToList(),
            };
            return job;
        }

        public async Task<Int32> InsertApplicationAsync(ApplicationRequest application)
        {
            var job = await _context.Jobs.Where(j => j.JobId == application.JobId).Include(j => j.Location).FirstOrDefaultAsync();
            var userdetails = await _context.Userdetails.Where(u => u.UserId == application.UserId).FirstOrDefaultAsync();
            var user = await _context.Users.Where(u => u.UserId == application.UserId).FirstOrDefaultAsync();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (PdfWriter writer = new(memoryStream))
                {
                    using (PdfDocument pdf = new(writer))
                    {
                        iText.Layout.Document document = new iText.Layout.Document(pdf);

                        document.Add(new Paragraph($"Name: {userdetails.FirstName} {userdetails.LastName}"));

                        document.Add(new Paragraph("Roles:"));
                        foreach (var roleId in application.Rolesid)
                        {
                            var roleName = await _context.Roles.Where(r => r.RoleId == roleId).Select(r => r.RoleName).FirstOrDefaultAsync();

                            document.Add(new Paragraph($"- {roleName}"));
                        }

                        document.Add(new Paragraph($"Job Name: {job.JobName}"));
                        document.Add(new Paragraph($"Venue: {job.Venue}"));
                        document.Add(new Paragraph($"From Time: {job.FromTime}"));
                        document.Add(new Paragraph($"To Time: {job.ToTime}"));
                        document.Add(new Paragraph($"Things To Remember: {job.ThingsToRemember}"));

                        document.Close();

                        byte[] pdfBytes = memoryStream.ToArray();

                        string base64String = Convert.ToBase64String(pdfBytes);

                        Application app = new Application();
                        app.UserId = application.UserId;
                        app.SlotId = application.SlotId;
                        app.JobId = application.JobId;
                        app.Hallticket = base64String;
                        if (application.Resume.Length != 0)
                        {
                            app.Resume = application.Resume;
                        }
                        else
                        {
                            var userasset = await _context.Userassets.Where(u => u.UserId == application.UserId).FirstOrDefaultAsync();
                            app.Resume = userasset.Resume;
                        }

                        _context.Applications.Add(app);
                        await _context.SaveChangesAsync();

                        foreach (var roleId in application.Rolesid)
                        {
                            ApplicationRole appRole = new ApplicationRole
                            {
                                ApplicationId = app.ApplicationId,
                                RoleId = roleId
                            };
                            _context.ApplicationRoles.Add(appRole);
                        }
                        await _context.SaveChangesAsync();

                        string jsonBody = $@"{{
                    ""sender"": {{
                        ""name"": ""Meet Dadhania"",
                        ""email"": ""dadhaniameet1744@gmail.com""
                    }},
                    ""to"": [
                        {{
                            ""email"": ""{user.Email}""
                        }}
                    ],
                    ""templateId"": 6,
                    ""params"": {{
                        ""firstname"": ""{userdetails.FirstName}"",
                        ""jobname"": ""{job.JobName}"",
                        ""id"": ""{app.ApplicationId}"",
                        ""location"": ""{job.Location.LocationName}"",
                        ""address"": ""{job.Venue.Replace("\n", "\\n")}""
                    }}
                }}";
                        Console.WriteLine(jsonBody);
                        await _emailService.SendTransactionalEmailAsync(jsonBody);

                        return app.ApplicationId;
                    }
                }
            }
        }



        public async Task<Application> GetApplicationByIdAsync(int applicationId)
        {
            var application = _context.Applications.Where(a => a.ApplicationId == applicationId).Include(a => a.Job).Include(a => a.Slot).AsSplitQuery().FirstOrDefault();
            return application;
        }
        public async Task<LoginRequest> AuthenticateUser(string username, string password,bool RememberMe)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == username && u.Password == password);

            if (user == null)
            {
                return null;
            }

            // Mapping User entity to UserDTO (you can use AutoMapper if preferred)
            var userDto = new LoginRequest
            {
                username = user.Email,
                password = user.Password,
               rememberMe = RememberMe
                
                // Add other properties as needed
            };

            return userDto;
        }
        public async System.Threading.Tasks.Task RegisterUser(UserRegistrationRequest userRegistrationRequest)
        {
            User user = new User
            {
                Email = userRegistrationRequest.Email,
                Password = userRegistrationRequest.FirstName + "" + userRegistrationRequest.PassingYear,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            string jsonBody = $@"{{
                ""sender"": {{
                    ""name"": ""Meet Dadhania"",
                    ""email"": ""dadhaniameet1744@gmail.com""
                }},
                ""to"": [
                    {{
                        ""email"": ""{userRegistrationRequest.Email}""
                    }}
                ],
                ""templateId"": 5,
                ""params"": {{
                    ""firstname"": ""{userRegistrationRequest.FirstName}"",
                    ""password"": ""{user.Password}""
                }}
            }}";
            Userasset userasset = new Userasset
            {
                UserId = user.UserId,
                Resume = userRegistrationRequest.Resume,
                ProfilePhoto = userRegistrationRequest.ProfilePhoto
            };
            _context.Userassets.Add(userasset);
            await _context.SaveChangesAsync();

            Userdetail userdetail = new Userdetail
            {
                UserId = user.UserId,
                FirstName = userRegistrationRequest.FirstName,
                LastName = userRegistrationRequest.LastName,
                PhoneNo = userRegistrationRequest.PhoneNo,
                Countrycode = userRegistrationRequest.Countrycode,
                PortfolioUrl = userRegistrationRequest.PortfolioUrl,
                ReferalEmpName = userRegistrationRequest.ReferalEmpName,
                SendMeUpdate = userRegistrationRequest.SendMeUpdate,
            };
            _context.Userdetails.Add(userdetail);
            await _context.SaveChangesAsync();

            Edqualification edqualification = new Edqualification
            {
                UserId = user.UserId,
                Percentage = userRegistrationRequest.Percentage,
                PassingYear = userRegistrationRequest.PassingYear,
                QualificationId = userRegistrationRequest.QualificationId,
                StreamId = userRegistrationRequest.StreamId,
                CollegeId = userRegistrationRequest.CollegeId,
                OtherCollege = userRegistrationRequest.OtherCollege,
                OtherCollegeLocation = userRegistrationRequest.OtherCollegeLocations
            };
            _context.Edqualifications.Add(edqualification);
            await _context.SaveChangesAsync();

            Proqualification proqualification = new Proqualification
            {
                UserId = user.UserId,
                ExpYear = userRegistrationRequest.ExpYear,
                CurrentCtc = userRegistrationRequest.CurrentCtc,
                ExpectedCtc = userRegistrationRequest.ExpectedCtc,
                CurrentlyOnNoticePeriod = userRegistrationRequest.CurrentlyOnNoticePeriod,
                NoticeEnd = userRegistrationRequest.NoticeEnd,
                NoticePeriodLength = userRegistrationRequest.NoticePeriodLength,
                AppearedZeusTest = userRegistrationRequest.AppearedZeusTest,
                ZeusTestRole = userRegistrationRequest.ZeusTestRole,
                ApplicationTypeId = userRegistrationRequest.ApplicationTypeId,
                OtherExpertTechs = userRegistrationRequest.OtherExpertTechs,
                OtherFamiliarTechs = userRegistrationRequest.OtherFamiliarTechs
            };
            _context.Proqualifications.Add(proqualification);
            await _context.SaveChangesAsync();

            foreach (var expertTechId in userRegistrationRequest.ExpertTechsId)
            {
                ProqualificationExperttech proqualificationExperttech = new ProqualificationExperttech
                {
                    ProqualificationId = proqualification.ProqualificationId,
                    TechId = expertTechId
                };
                _context.ProqualificationExperttechs.Add(proqualificationExperttech);
            }
            await _context.SaveChangesAsync();

            foreach (var familiarTechId in userRegistrationRequest.FamiliarTechsId)
            {
                ProqualificationFamiliartech proqualificationFamiliartech = new ProqualificationFamiliartech
                {
                    ProqualificationId = proqualification.ProqualificationId,
                    TechId = familiarTechId
                };
                _context.ProqualificationFamiliartechs.Add(proqualificationFamiliartech);
            }
            await _context.SaveChangesAsync();
        }
    }
}
