namespace TargetHound.Services
{
    using System;
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

        public async Task CreateAsync(string userId, string projectName, double magneticDeclination, int countryId)
        {
            var user = this.dbContext.ApplicationUsers.
                FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return;
            }

            if (user.UserProjects.Any(x => x.Project.Name == projectName))
            {
                return;
            }

            Project project = new Project
            {
                Name = projectName,
                AdminId = userId,
                MagneticDeclination = magneticDeclination,
                CountryId = countryId,
                CreatedOn = DateTime.UtcNow,
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

        public async Task<ICollection<T>> GetProjectsByUserId<T>(string userId)
        {
            var project = this.dbContext
                .UsersProjects
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.Project)
                .To<T>()
                .ToList();

            return project;
        }

        public async Task<string> GetProjectAdminName(string projectId)
        {
            string adminId = this.dbContext.UsersProjects.SingleOrDefault(x => x.ProjectId == projectId && x.IsAdmin == true).ApplicationUserId;

            if (adminId == null)
            {
                return null;
            }

            string adminName = this.dbContext.ApplicationUsers.SingleOrDefault(x => x.Id == adminId).UserName;
            return adminName;
        }

        public async Task<T> GetProjectById<T>(string projectId)
        {
            return this.dbContext.Projects
                .Where(x => x.Id == projectId)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task<bool> IsUserIdSameWithProjectAdminId(string userId, string projectId)
        {
            bool userIsProjectAdmin = this.dbContext.Projects.SingleOrDefault(x => x.Id == projectId)?.AdminId == userId;
            return userIsProjectAdmin;
        }

        public async Task<bool> IsUserInProject(string userId, string projectId)
        {
            bool isUserInTheProject = this.dbContext.UsersProjects.Any(x => x.ProjectId == projectId && x.ApplicationUserId == userId);
            return isUserInTheProject;
        }
    }
}
