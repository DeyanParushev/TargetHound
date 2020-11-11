namespace TargetHound.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.Models;
    using TargetHound.Services.Interfaces;

    public class ProjectService : IProjectService
    {
        private readonly TargetHoundContext dbContext;

        public ProjectService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(string userId, string projectName, double magneticDeclination)
        {
            ApplicationUser user = this.dbContext.ApplicationUsers.FirstOrDefault(x => x.Id == userId);

            if(user == null)
            {
                return;
            }

            if(user.UserProjects.Any(x => x.Project.Name == projectName))
            {
                return;
            }

            Project project = new Project
            {
                Name = projectName,
                AdminId = userId,
                MagneticDeclination = magneticDeclination
            };

            this.dbContext.Projects.Add(project);
            this.dbContext.UsersProjects.Add(new UserProject
            {
                ApplicationUserId = userId,
                ProjectId = project.Id,
                IsAdmin = true,
            });

            await this.dbContext.SaveChangesAsync();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task GetUserProjects(string userId)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var projects = this.dbContext.UsersProjects
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Project.Name,
                    Boreholes = x.Project.Boreholes.Select(y => new
                    {
                        Id = y.Id,
                        Name = y.Name
                    })
                })
                .ToList();
        } 
    }
}
