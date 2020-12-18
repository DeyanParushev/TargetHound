namespace TargetHound.Data
{
    using System.Linq;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    
    using TargetHound.DataModels;

    public class TargetHoundContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public TargetHoundContext()
        {
        }

        public TargetHoundContext(DbContextOptions<TargetHoundContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Collar> Collars { get; set; }
        
        public DbSet<Borehole> Boreholes { get; set; }

        public DbSet<Client> Clients { get; set; }
       
        public DbSet<ClientContractor> ClientsContractors { get; set; }
        
        public DbSet<Contractor> Contractors { get; set; }
        
        public DbSet<Country> Countries { get; set; }
        
        public DbSet<DrillRig> DrillRigs { get; set; }
        
        public DbSet<Project> Projects { get; set; }
        
        public DbSet<ProjectContractor> ProjectsContractors { get; set; }
        
        public DbSet<SurveyPoint> SurveyPoints { get; set; }
        
        public DbSet<Target> Targets { get; set; }

        public DbSet<Dogleg> Doglegs { get; set; }

        public DbSet<UserProject> UsersProjects { get; set; }
       
        public DbSet<ClientInvitation> ClientInvitations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Client>()
                .HasOne(x => x.Admin)
                .WithOne(x => x.Client)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Collar>()
                .HasMany(x => x.Boreholes)
                .WithOne(x => x.Collar)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borehole>()
                .HasOne(x => x.Target)
                .WithOne(x => x.Borehole)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borehole>()
                .HasMany<SurveyPoint>()
                .WithOne(x => x.Borehole)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}
