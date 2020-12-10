namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using TargetHound.DTOs;
    using TargetHound.Services.Interfaces;

    [ApiController]
    [Route("api/[controller]")]
    public class ProjectApiController : ControllerBase
    {
        private readonly IProjectService projectService;

        public ProjectApiController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet("{projectId}")]
        [Authorize]
        public async Task<ActionResult<ProjectDTO>> GetProject(string projectId)
        {
            try
            {
                var project = await this.projectService.GetProjectById<ProjectDTO>(projectId);
                return project;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
