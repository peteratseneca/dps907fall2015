using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
// new...
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AssociationsIntro.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        // DbSet<TEntity> properties
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // The default behaviour for the one (Manufacturer) to many (Vehicles)
            // association is "cascade delete"
            // This means that deleting a Manufacturer will cause the Vehicles to be deleted
            // In this code example, we do NOT want that behaviour

            // We cannot do this with a data annotation
            // We must use this Fluent API

            // First, call the base OnModelCreating method,
            // which uses the existing class definitions and conventions

            base.OnModelCreating(modelBuilder);

            // Then, change the "cascade delete" setting
            // This can be done in at least two ways; un-comment the desired code

            // Alternative #1
            // Turn off "cascade delete" for all default convention-based associations

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Alternative #2
            // Turn off "cascade delete" for a specific association

            //modelBuilder.Entity<Vehicle>()
            //    .HasRequired(m => m.Manufacturer)
            //    .WithMany(v => v.Vehicles)
            //    .WillCascadeOnDelete(false);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}