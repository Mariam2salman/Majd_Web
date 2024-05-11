using Majd_Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Majd_Web.Data
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<AlumniExperience> AlumniExperiences { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Major>()
           .HasMany(e => e.Students)
           .WithOne(e => e.Major)
           .HasForeignKey(e => e.MajorId)
           .IsRequired(false);

            modelBuilder.Entity<ApplicationUser>()
           .HasMany(e => e.JobApplications)
           .WithOne(e => e.User)
           .HasForeignKey(e => e.UserId)
           .IsRequired(false);

            modelBuilder.Entity<Job>()
          .HasMany(e => e.JobApplications)
          .WithOne(e => e.Job)
          .HasForeignKey(e => e.JobId)
          .IsRequired(false);

            modelBuilder.Entity<ApplicationUser>()
            .HasMany(e => e.Jobs)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false);

            modelBuilder.Entity<ApplicationUser>()
           .HasMany(e => e.Posts)
           .WithOne(e => e.User)
           .HasForeignKey(e => e.UserId)
           .IsRequired(false);


            modelBuilder.Entity<ApplicationUser>()
           .HasMany(e => e.News)
           .WithOne(e => e.User)
           .HasForeignKey(e => e.UserId)
           .IsRequired(false);


            modelBuilder.Entity<Job>()
        .Property(j => j.Id)
        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Post>()
        .Property(p => p.Id)
        .ValueGeneratedOnAdd();

            modelBuilder.Entity<ApplicationUser>()
        .HasMany(e => e.Experiences)
        .WithOne(e => e.User)
        .HasForeignKey(e => e.UserId)
        .IsRequired(false);


            modelBuilder.Entity<AlumniExperience>()
        .Property(j => j.Id)
        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Major>().HasData(new Major
            {
                Id = "1",
                MajorName = "علوم الحاسب"
            },
            new Major
            {
                Id = "2",
                MajorName = "الفيزياء"
            }, new Major
            {
                Id = "3",
                MajorName = " الكيمياء"
            }, new Major
            {
                Id = "4",
                MajorName = "الصيدلة"
            });

        }


    }
}
