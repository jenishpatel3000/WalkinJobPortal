using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BackendAPI.Models
{
    public partial class walkin_portalContext : DbContext
    {
        public walkin_portalContext()
        {
        }

        public walkin_portalContext(DbContextOptions<walkin_portalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Adminuser> Adminusers { get; set; } = null!;
        public virtual DbSet<Application> Applications { get; set; } = null!;
        public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; } = null!;
        public virtual DbSet<ApplicationType> ApplicationTypes { get; set; } = null!;
        public virtual DbSet<College> Colleges { get; set; } = null!;
        public virtual DbSet<Edqualification> Edqualifications { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<JobDesc> JobDescs { get; set; } = null!;
        public virtual DbSet<JobRole> JobRoles { get; set; } = null!;
        public virtual DbSet<JobRolesDesc> JobRolesDescs { get; set; } = null!;
        public virtual DbSet<JobSlot> JobSlots { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<Proqualification> Proqualifications { get; set; } = null!;
        public virtual DbSet<ProqualificationExperttech> ProqualificationExperttechs { get; set; } = null!;
        public virtual DbSet<ProqualificationFamiliartech> ProqualificationFamiliartechs { get; set; } = null!;
        public virtual DbSet<Qualification> Qualifications { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<Stream> Streams { get; set; } = null!;
        public virtual DbSet<Tech> Techs { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Userasset> Userassets { get; set; } = null!;
        public virtual DbSet<Userdetail> Userdetails { get; set; } = null!;
        public virtual DbSet<UserdetailsRole> UserdetailsRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;port=3307;user=root;password=Rachit@15;database=walkin_portal");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adminuser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("adminusers");

                entity.HasIndex(e => e.Email, "email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("application");

                entity.HasIndex(e => e.JobId, "fk_application_jobs1_idx");

                entity.HasIndex(e => e.SlotId, "fk_application_slots1_idx");

                entity.HasIndex(e => e.UserId, "fk_application_users1_idx");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Hallticket).HasColumnName("hallticket");

                entity.Property(e => e.JobId).HasColumnName("job_id");

                entity.Property(e => e.Resume).HasColumnName("resume");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_application_jobs1");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_application_slots1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_application_users1");
            });

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.ApplicationId })
                    .HasName("PRIMARY");

                entity.ToTable("application_roles");

                entity.HasIndex(e => e.ApplicationId, "fk_roles_has_application_application1_idx");

                entity.HasIndex(e => e.RoleId, "fk_roles_has_application_roles1_idx");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationRoles)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_roles_has_application_application1");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ApplicationRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_roles_has_application_roles1");
            });

            modelBuilder.Entity<ApplicationType>(entity =>
            {
                entity.ToTable("application_types");

                entity.HasIndex(e => e.ApplicationTypeName, "application_type_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ApplicationTypeId).HasColumnName("application_type_id");

                entity.Property(e => e.ApplicationTypeName).HasColumnName("application_type_name");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<College>(entity =>
            {
                entity.ToTable("colleges");

                entity.HasIndex(e => e.CollegeName, "college_name_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.LocationId, "fk_colleges_locations_idx");

                entity.Property(e => e.CollegeId).HasColumnName("college_id");

                entity.Property(e => e.CollegeName).HasColumnName("college_name");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Colleges)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_colleges_locations");
            });

            modelBuilder.Entity<Edqualification>(entity =>
            {
                entity.ToTable("edqualification");

                entity.HasIndex(e => e.CollegeId, "fk_edqualification_colleges1_idx");

                entity.HasIndex(e => e.QualificationId, "fk_edqualification_qualifications1_idx");

                entity.HasIndex(e => e.StreamId, "fk_edqualification_streams1_idx");

                entity.HasIndex(e => e.UserId, "fk_edqualification_users1_idx");

                entity.Property(e => e.EdqualificationId).HasColumnName("edqualification_id");

                entity.Property(e => e.CollegeId).HasColumnName("college_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.OtherCollege).HasMaxLength(255);

                entity.Property(e => e.OtherCollegeLocation).HasMaxLength(255);

                entity.Property(e => e.PassingYear)
                    .HasColumnType("year")
                    .HasColumnName("passing_year");

                entity.Property(e => e.Percentage)
                    .HasPrecision(5)
                    .HasColumnName("percentage");

                entity.Property(e => e.QualificationId).HasColumnName("qualification_id");

                entity.Property(e => e.StreamId).HasColumnName("stream_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.College)
                    .WithMany(p => p.Edqualifications)
                    .HasForeignKey(d => d.CollegeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_edqualification_colleges1");

                entity.HasOne(d => d.Qualification)
                    .WithMany(p => p.Edqualifications)
                    .HasForeignKey(d => d.QualificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_edqualification_qualifications1");

                entity.HasOne(d => d.Stream)
                    .WithMany(p => p.Edqualifications)
                    .HasForeignKey(d => d.StreamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_edqualification_streams1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Edqualifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_edqualification_users1");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("jobs");

                entity.HasIndex(e => e.LocationId, "fk_jobs_locations1_idx");

                entity.Property(e => e.JobId).HasColumnName("job_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FromTime)
                    .HasColumnType("date")
                    .HasColumnName("from_time");

                entity.Property(e => e.JobName)
                    .HasMaxLength(255)
                    .HasColumnName("job_name");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.ThingsToRemember)
                    .HasColumnType("text")
                    .HasColumnName("things_to_remember");

                entity.Property(e => e.ToTime)
                    .HasColumnType("date")
                    .HasColumnName("to_time");

                entity.Property(e => e.Venue)
                    .HasMaxLength(255)
                    .HasColumnName("venue");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_jobs_locations1");
            });

            modelBuilder.Entity<JobDesc>(entity =>
            {
                entity.ToTable("job_desc");

                entity.HasIndex(e => e.JobId, "fk_job_desc_jobs1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DescTitle)
                    .HasMaxLength(255)
                    .HasColumnName("desc_title");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.JobId).HasColumnName("job_id");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobDescs)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_job_desc_jobs1");
            });

            modelBuilder.Entity<JobRole>(entity =>
            {
                entity.ToTable("job_roles");

                entity.HasIndex(e => e.JobId, "fk_job_roles_jobs1_idx");

                entity.HasIndex(e => e.RoleId, "fk_job_roles_roles1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.JobId).HasColumnName("job_id");

                entity.Property(e => e.Package)
                    .HasPrecision(10)
                    .HasColumnName("package");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobRoles)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_job_roles_jobs1");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.JobRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_job_roles_roles1");
            });

            modelBuilder.Entity<JobRolesDesc>(entity =>
            {
                entity.ToTable("job_roles_desc");

                entity.HasIndex(e => e.RolesId, "fk_job_roles_desc_job_roles1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DescTitle)
                    .HasMaxLength(255)
                    .HasColumnName("desc_title");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.RolesId).HasColumnName("roles_id");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.JobRolesDescs)
                    .HasForeignKey(d => d.RolesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_job_roles_desc_job_roles1");
            });

            modelBuilder.Entity<JobSlot>(entity =>
            {
                entity.HasKey(e => new { e.SlotId, e.JobId })
                    .HasName("PRIMARY");

                entity.ToTable("job_slots");

                entity.HasIndex(e => e.JobId, "fk_slots_has_jobs_jobs1_idx");

                entity.HasIndex(e => e.SlotId, "fk_slots_has_jobs_slots1_idx");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.Property(e => e.JobId).HasColumnName("job_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobSlots)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_slots_has_jobs_jobs1");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.JobSlots)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_slots_has_jobs_slots1");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("locations");

                entity.HasIndex(e => e.LocationName, "location_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LocationName).HasColumnName("location_name");
            });

            modelBuilder.Entity<Proqualification>(entity =>
            {
                entity.ToTable("proqualification");

                entity.HasIndex(e => e.ApplicationTypeId, "fk_proqualification_application_types1_idx");

                entity.HasIndex(e => e.UserId, "fk_proqualification_users1_idx");

                entity.Property(e => e.ProqualificationId).HasColumnName("proqualification_id");

                entity.Property(e => e.AppearedZeusTest).HasColumnName("appeared_zeus_test");

                entity.Property(e => e.ApplicationTypeId).HasColumnName("application_type_id");

                entity.Property(e => e.CurrentCtc)
                    .HasPrecision(10)
                    .HasColumnName("current_ctc");

                entity.Property(e => e.CurrentlyOnNoticePeriod).HasColumnName("currently_on_notice_period");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ExpYear)
                    .HasColumnType("year")
                    .HasColumnName("exp_year");

                entity.Property(e => e.ExpectedCtc)
                    .HasPrecision(10)
                    .HasColumnName("expected_ctc");

                entity.Property(e => e.NoticeEnd)
                    .HasColumnType("date")
                    .HasColumnName("notice_end");

                entity.Property(e => e.NoticePeriodLength).HasColumnName("notice_period_length");

                entity.Property(e => e.OtherExpertTechs).HasMaxLength(255);

                entity.Property(e => e.OtherFamiliarTechs).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.ZeusTestRole)
                    .HasMaxLength(255)
                    .HasColumnName("zeus_test_role");

                entity.HasOne(d => d.ApplicationType)
                    .WithMany(p => p.Proqualifications)
                    .HasForeignKey(d => d.ApplicationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_proqualification_application_types1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Proqualifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_proqualification_users1");
            });

            modelBuilder.Entity<ProqualificationExperttech>(entity =>
            {
                entity.HasKey(e => new { e.TechId, e.ProqualificationId })
                    .HasName("PRIMARY");

                entity.ToTable("proqualification_experttechs");

                entity.HasIndex(e => e.ProqualificationId, "fk_techs_has_proqualification_proqualification1_idx");

                entity.HasIndex(e => e.TechId, "fk_techs_has_proqualification_techs1_idx");

                entity.Property(e => e.TechId).HasColumnName("tech_id");

                entity.Property(e => e.ProqualificationId).HasColumnName("proqualification_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Proqualification)
                    .WithMany(p => p.ProqualificationExpertteches)
                    .HasForeignKey(d => d.ProqualificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_techs_has_proqualification_proqualification1");

                entity.HasOne(d => d.Tech)
                    .WithMany(p => p.ProqualificationExpertteches)
                    .HasForeignKey(d => d.TechId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_techs_has_proqualification_techs1");
            });

            modelBuilder.Entity<ProqualificationFamiliartech>(entity =>
            {
                entity.HasKey(e => new { e.TechId, e.ProqualificationId })
                    .HasName("PRIMARY");

                entity.ToTable("proqualification_familiartechs");

                entity.HasIndex(e => e.ProqualificationId, "fk_techs_has_proqualification_proqualification2_idx");

                entity.HasIndex(e => e.TechId, "fk_techs_has_proqualification_techs2_idx");

                entity.Property(e => e.TechId).HasColumnName("tech_id");

                entity.Property(e => e.ProqualificationId).HasColumnName("proqualification_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Proqualification)
                    .WithMany(p => p.ProqualificationFamiliarteches)
                    .HasForeignKey(d => d.ProqualificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_techs_has_proqualification_proqualification2");

                entity.HasOne(d => d.Tech)
                    .WithMany(p => p.ProqualificationFamiliarteches)
                    .HasForeignKey(d => d.TechId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_techs_has_proqualification_techs2");
            });

            modelBuilder.Entity<Qualification>(entity =>
            {
                entity.ToTable("qualifications");

                entity.HasIndex(e => e.QualificationName, "qualification_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.QualificationId).HasColumnName("qualification_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.QualificationName).HasColumnName("qualification_name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasIndex(e => e.RoleName, "role_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.RoleName).HasColumnName("role_name");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("slots");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FromTime)
                    .HasColumnType("time")
                    .HasColumnName("from_time");

                entity.Property(e => e.ToTime)
                    .HasColumnType("time")
                    .HasColumnName("to_time");
            });

            modelBuilder.Entity<Stream>(entity =>
            {
                entity.ToTable("streams");

                entity.HasIndex(e => e.StreamName, "stream_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.StreamId).HasColumnName("stream_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.StreamName).HasColumnName("stream_name");
            });

            modelBuilder.Entity<Tech>(entity =>
            {
                entity.ToTable("techs");

                entity.HasIndex(e => e.TechName, "tech_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.TechId).HasColumnName("tech_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TechName).HasColumnName("tech_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<Userasset>(entity =>
            {
                entity.ToTable("userassets");

                entity.HasIndex(e => e.UserId, "fk_userassets_users1_idx");

                entity.Property(e => e.UserassetId).HasColumnName("userasset_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ProfilePhoto).HasColumnName("profile_photo");

                entity.Property(e => e.Resume).HasColumnName("resume");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userassets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_userassets_users1");
            });

            modelBuilder.Entity<Userdetail>(entity =>
            {
                entity.ToTable("userdetails");

                entity.HasIndex(e => e.UserId, "fk_userdetails_users1_idx");

                entity.Property(e => e.UserdetailId).HasColumnName("userdetail_id");

                entity.Property(e => e.Countrycode).HasColumnName("countrycode");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(45)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(45)
                    .HasColumnName("last_name");

                entity.Property(e => e.PhoneNo)
                    .HasPrecision(10)
                    .HasColumnName("phone_no");

                entity.Property(e => e.PortfolioUrl)
                    .HasMaxLength(255)
                    .HasColumnName("portfolio_url");

                entity.Property(e => e.ReferalEmpName)
                    .HasMaxLength(255)
                    .HasColumnName("referal_emp_name");

                entity.Property(e => e.SendMeUpdate).HasColumnName("send_me_update");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userdetails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_userdetails_users1");
            });

            modelBuilder.Entity<UserdetailsRole>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.UserdetailId })
                    .HasName("PRIMARY");

                entity.ToTable("userdetails_roles");

                entity.HasIndex(e => e.RoleId, "fk_roles_has_userdetails_roles1_idx");

                entity.HasIndex(e => e.UserdetailId, "fk_roles_has_userdetails_userdetails1_idx");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserdetailId).HasColumnName("userdetail_id");

                entity.Property(e => e.DtCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DtModified)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("dt_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserdetailsRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_roles_has_userdetails_roles1");

                entity.HasOne(d => d.Userdetail)
                    .WithMany(p => p.UserdetailsRoles)
                    .HasForeignKey(d => d.UserdetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_roles_has_userdetails_userdetails1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
