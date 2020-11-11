namespace TargetHound.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.Models;
    using TargetHound.Services.Automapper;
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
            var user = this.dbContext.ApplicationUsers.
                FirstOrDefault(x => x.Id == userId);

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

        public ICollection<T> GetProjectsByUserId<T>(string userId)
        {
            var project = this.dbContext.UsersProjects
                .Where(x => x.ApplicationUserId == userId)
                .To<T>()
                .ToList();

            return project;
        } 
    }
}
