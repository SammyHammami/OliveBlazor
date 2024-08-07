using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OliveBlazor.Core.Domain.Accounts;
using OliveBlazor.Infrastructure.Indentity;

namespace OliveBlazor.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserIdentity, IdentityRole, string>

    {
        public DbSet<OliveUser> OliveUsers { get; set; }
        public DbSet<OliveRole> OliveRoles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserIdentity>()
                .ToTable("AspNetUsers")
                .HasOne(ob => ob.OliveUser)
                .WithOne()
                .IsRequired(false);

            builder.Entity<RoleIdentity>()
                .HasOne(ri => ri.OliveRole)
                .WithOne()
                .HasForeignKey<OliveRole>(or => or.IdentityRoleId)
                .IsRequired();

           
        }


       
    }

}