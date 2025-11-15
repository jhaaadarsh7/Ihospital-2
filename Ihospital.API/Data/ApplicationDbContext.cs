using Microsoft.EntityFrameworkCore;
using Ihospital.API.Models;

namespace Ihospital.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Staff> Staff { get; set; }
        public DbSet<Respondent> Respondents { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<OptionList> OptionLists { get; set; }
        public DbSet<ResponseDetail> ResponseDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Staff
            modelBuilder.Entity<Staff>()
                .HasIndex(s => s.UserName)
                .IsUnique();

            // Configure Question
            modelBuilder.Entity<Question>()
                .HasIndex(q => q.QuestionCode)
                .IsUnique();

            // Configure relationships
            modelBuilder.Entity<SurveyResponse>()
                .HasOne(sr => sr.Respondent)
                .WithMany(r => r.SurveyResponses)
                .HasForeignKey(sr => sr.RespondentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OptionList>()
                .HasOne(ol => ol.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(ol => ol.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResponseDetail>()
                .HasOne(rd => rd.SurveyResponse)
                .WithMany(sr => sr.ResponseDetails)
                .HasForeignKey(rd => rd.SurveyResponseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResponseDetail>()
                .HasOne(rd => rd.Question)
                .WithMany(q => q.ResponseDetails)
                .HasForeignKey(rd => rd.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ResponseDetail>()
                .HasOne(rd => rd.Option)
                .WithMany(o => o.ResponseDetails)
                .HasForeignKey(rd => rd.OptionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default admin staff
            modelBuilder.Entity<Staff>().HasData(
                new Staff
                {
                    StaffId = 1,
                    UserName = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123")
                }
            );
        }
    }
}
