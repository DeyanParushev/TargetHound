namespace TargetHound.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TargetHound.Data;
    using TargetHound.DataModels;
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

        public async Task<ICollection<T>> GetProjectUsersAsync<T>(string projectId)
        {
            this.CheckProjectExists(projectId);

            var users = this.dbContext
                .UsersProjects
                .Where(x => x.ProjectId == projectId && x.Project.IsDeleted == false)
                .Select(x => x.ApplicationUser)
                .To<T>()
                .ToList();

            return users;
        }

        public async Task<T> GetDetailsAsync<T>(string projectId)
        {
            this.CheckProjectExists(projectId);

            var details = this.dbContext.Projects
                .Where(x => x.Id == projectId && x.IsDeleted == false)
                .To<T>()
                .ToList()[0];

            return details;
        }

        public async Task<int> GetUserCountAsync(string projectId)
        {
            this.CheckProjectExists(projectId);

            var userCount = this.dbContext.Projects
                .SingleOrDefault(x => x.Id == projectId && x.IsDeleted == false)
                .ProjectUsers
                .Count();

            return userCount;
        }

        public async Task<string> GetCurrentContractorAsync(string projectId)
        {
            this.CheckProjectExists(projectId);

            var contractorName = this.dbContext.ProjectsContractors
                .Where(x => x.Id == projectId && x.IsDeleted == false)
                .Select(x => x.Contractor.Name)
                .FirstOrDefault();

            return contractorName;
        }

        public async Task<ICollection<T>> GetBoreholesAsync<T> (string projectId)
        {
            this.CheckProjectExists(projectId);

            var boreholes = this.dbContext.Projects
                .Where(x => x.Id == projectId && x.IsDeleted == false)
                .Select(x => x.Boreholes)
                .To<T>()
                .ToList();

            return boreholes;
        }

        public async Task<ICollection<T>> GetCollarsAsync<T>(string projectId)
        {
            this.CheckProjectExists(projectId);

            var collars = this.dbContext.Projects
                .Where(x => x.Id == projectId && x.IsDeleted == false)
                .Select(x => x.Collars)
                .To<T>()
                .ToList();

            return collars;
        }

        public async Task<ICollection<T>> GetTargetsAsync<T>(string projectId)
        {
            this.CheckProjectExists(projectId);

            var targets = this.dbContext.Projects
                .Where(x => x.Id == projectId && x.IsDeleted == false)
                .Select(x => x.Targets)
                .To<T>()
                .ToList();

            return targets;
        }

        private void CheckProjectExists(string projectId)
        {
            if (!this.dbContext.Projects.Any(x => x.Id == projectId && x.IsDeleted == false))
            {
                throw new NullReferenceException("Project does not exist");
            }
        }
    }
}
