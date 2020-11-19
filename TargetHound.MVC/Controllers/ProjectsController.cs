namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using TargetHound.Models;
    using TargetHound.MVC.Areas.Identity;
    using TargetHound.Services.Interfaces;
    using TargetHound.SharedViewModels.InputModels;
    using TargetHound.SharedViewModels.ViewModels;

    public class ProjectsController : Controller
    {
        private readonly IProjectService projectService;
        private readonly ICountriesService countriesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectsController(IProjectService projectService,
            ICountriesService countriesService,
            UserManager<ApplicationUser> userManager)
        {
            this.projectService = projectService;
            this.countriesService = countriesService;
            this.userManager = userManager;
        }

        public IActionResult Planning()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            ICollection<CountryViewModel> countries = await this.countriesService.GetAllCountriesAsync<CountryViewModel>();
            ProjectCountryInputModel viewModel = new ProjectCountryInputModel
            {
                Project = new ProjectInputModel(),
                Countries = countries,
            };

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Load()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ICollection<ProjectViewModel> projects = await this.projectService.GetProjectsByUserId<ProjectViewModel>(userId);

            foreach (var project in projects)
            {
                project.AdminName = await this.projectService.GetProjectAdminName(project.Id);
                project.IsCurrentUserAdmin = await this.projectService.IsUserIdSameWithProjectAdminId(userId, project.Id);
            };

            return this.View(projects);
        }

        [Authorize]
        public async Task<IActionResult> LoadProject(string projectId)
        {
            bool userIsInProject = await this.projectService.IsUserInProject(this.userManager.GetUserId(this.User), projectId);

            if (userIsInProject)
            {
                return this.Json(await this.projectService.GetProjectById<ProjectViewModel>(projectId));
            }

            return this.Redirect("/Projects/Load");
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCountryInputModel input)
        {
            ProjectInputModel project = input.Project;
            string userId = this.userManager.GetUserId(this.User);

            if (userId != null)
            {
                await this.projectService.CreateAsync(userId, project.Name, project.MagneticDeclination, project.CountryId);
                ApplicationUser user = await this.userManager.GetUserAsync(this.User);
                await this.userManager.AddToRoleAsync(user, SiteIdentityRoles.ProjectAdmin);
            }

            CountryViewModel country = await this.countriesService.GetCountryByIdAsync<CountryViewModel>(input.Project.CountryId);
            ProjectViewModel projectView = new ProjectViewModel { Name = project.Name, CountryName = country.Name };

            return this.RedirectToAction("/Planning", projectView);
        }

        [Authorize(Roles = SiteIdentityRoles.ProjectAdmin)]
        public async Task<IActionResult> Edit(string projectId)
        {
            ProjectEditInputModel project = await this.projectService.GetProjectById<ProjectEditInputModel>(projectId);
            bool isCurrentUserAdmin = 
                await this.projectService.IsUserIdSameWithProjectAdminId(this.userManager.GetUserId(this.User), project.Id);

            if (!isCurrentUserAdmin)
            {
                return this.Redirect("/Projects/Load");
            }

            return this.View(project);
        }
    }
}
