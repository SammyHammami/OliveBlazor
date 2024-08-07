using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OlivePatterns.Core.Domain.Accounts;
using OlivePatterns.Infrastructure.Indentity;

namespace ServerBlazorIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<OliveUser> OliveUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserIdentity>()
                .ToTable("AspNetUsers")
                .HasOne(ob => ob.OliveUser)
                .WithOne()
                .IsRequired(false);

        
            base.OnModelCreating(builder);
        }
    }
}