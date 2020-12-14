namespace TargetHound.MVC.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using TargetHound.DataModels;
    using TargetHound.DTOs;
    using TargetHound.Services.Interfaces;

    [RequireHttps]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectApiController : ControllerBase
    {
        private readonly IProjectService projectService;
        private readonly IMapper mapper;

        public ProjectApiController(IProjectService projectService, IMapper mapper)
        {
            this.projectService = projectService;
            this.mapper = mapper;
        }

        [HttpGet("{projectId}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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

        [HttpPost, Route("Save")]
        [Authorize]
        public async Task<IActionResult> PutProject(JsonContent project)
        {
            //var projectDataModel = this.mapper.Map<Project>(inputProject);

            ////await this.projectService.SaveProject(projectDataModel);
            return this.StatusCode(200);
        }
    }
}
