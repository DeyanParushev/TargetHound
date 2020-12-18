namespace TargetHound.Services
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.DataModels;

    public class BoreholeService : IBoreholeService
    {
        private readonly TargetHoundContext dbContext;

        public BoreholeService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UpdateBoreholes(string projectId, string userId, Borehole borehole)
        {
            if (!this.dbContext.Users.Any(x => x.Id == userId && x.IsDeleted == false))
            {
                throw new ArgumentNullException("User does not exist.");
            }

            if (!this.dbContext.Projects.Any(x => x.Id == projectId && x.IsDeleted == false))
            {
                throw new ArgumentNullException("Project does not exist.");
            }

            if (!this.dbContext.UsersProjects.Any(x => x.ApplicationUserId == userId && x.ProjectId == projectId))
            {
                throw new ArgumentException("User is not in the project.");
            }

            if (!this.dbContext.Boreholes.Any(x => x.Id == borehole.Id && x.IsDeleted == false))
            {
                if (this.dbContext.Collars.Any(x => x.ProjectId == projectId
                 && x.IsDeleted == false
                 && x.Id == borehole.Collar.Id))
                {
                    this.dbContext.Entry(borehole.Collar).State = EntityState.Modified;
                }

                this.dbContext.Boreholes.Add(borehole);
            }
            else if (this.dbContext.Entry(borehole).State == EntityState.Modified)
            {
                this.dbContext.Boreholes.Update(borehole);
            }

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
