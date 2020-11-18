namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using TargetHound.InputModels;
    using TargetHound.Models;
    using TargetHound.MVC.Models.InputModels;
    using TargetHound.MVC.Models.ViewModels;
    using TargetHound.Services.Interfaces;
    using TargetHound.ViewModels.ViewModels;

    public class ProjectController : Controller
    {
        private readonly IProjectService projectService;
        private readonly ICountriesService countriesService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectController(IProjectService projectService,
            ICountriesService countriesService,
            IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            this.projectService = projectService;
            this.countriesService = countriesService;
            this.userService = userService;
            this.userManager = userManager;
        }

        public IActionResult Planning()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var countries = await this.countriesService.GetAllCountriesAsync<CountryViewModel>();
            ProjectCountryInputModel viewModel = new ProjectCountryInputModel
            {
                Project = new ProjectInputModel(),
                Countries = countries,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Load()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projects = this.projectService.GetProjectsByUserId<ProjectViewModel>(userId);
            foreach (var project in projects)
            {
                project.AdminName = await this.projectService.GetProjectAdminName(project.Id);
            }

            return this.View(projects);
        }

        public async Task<ProjectViewModel> LoadProject(string projectId)
        {
            return await this.projectService.GetProjectById<ProjectViewModel>(projectId);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCountryInputModel input)
        {
            ProjectInputModel project = input.Project;
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                await this.projectService.CreateAsync(userId, project.Name, project.MagneticDeclination, project.CountryId);
            }

            CountryViewModel country = await this.countriesService.GetCountryByIdAsync<CountryViewModel>(input.Project.CountryId);
            ProjectViewModel projectView = new ProjectViewModel { Name = project.Name, CountryName = country.Name };

            return this.RedirectToAction("/Planning", projectView);
        }
    }
}
