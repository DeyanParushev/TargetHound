namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using TargetHound.InputModels;
    using TargetHound.Services.Interfaces;

    public class ProjectController : Controller
    {
        private readonly IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public IActionResult Planning()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        public IActionResult Load()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectInputModel input)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                await this.projectService.Create(userId, input.Name, input.MagneticDeclination);
            }

            return this.Redirect("/Planning");
        }
    }
}
