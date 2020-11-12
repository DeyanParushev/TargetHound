namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using TargetHound.InputModels;
    using TargetHound.MVC.Models.InputModels;
    using TargetHound.MVC.Models.ViewModels;
    using TargetHound.Services.Interfaces;
    using TargetHound.ViewModels.ViewModels;

    public class ProjectController : Controller
    {
        private readonly IProjectService projectService;
        private readonly ICountriesService countriesService;

        public ProjectController(IProjectService projectService, ICountriesService countriesService)
        {
            this.projectService = projectService;
            this.countriesService = countriesService;
        }

        public IActionResult Planning()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            var countries = this.countriesService.GetAllCountriesAsync<CountryViewModel>();
            ProjectCountryInputModel viewModel = new ProjectCountryInputModel
            {
                Project = new ProjectInputModel(),
                Cuntries = countries,
            };

            return this.View(viewModel);
        }

        public IActionResult Load()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projects = this.projectService.GetProjectsByUserId<ProjectViewModel>(userId);

            return this.View(projects);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectInputModel input)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                await this.projectService.CreateAsync(userId, input.Name, input.MagneticDeclination, input.CountryId);
            }

            return this.Redirect("/Planning");
        }
    }
}
