namespace TargetHound.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class TargetHoundContextFactory : IDesignTimeDbContextFactory<TargetHoundContext>
    {
        public TargetHoundContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TargetHoundContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=TargetHound;Integrated Security = true");

            return new TargetHoundContext(optionsBuilder.Options);
        }
    }

}

