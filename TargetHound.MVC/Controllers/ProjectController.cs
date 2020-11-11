namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using TargetHound.InputModels;
    using TargetHound.Models;
    using TargetHound.Services.Interfaces;
    using TargetHound.ViewModels.ViewModels;

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
                await this.projectService.Create(userId, input.Name, input.MagneticDeclination);
            }

            return this.Redirect("/Planning");
        }
    }
}
