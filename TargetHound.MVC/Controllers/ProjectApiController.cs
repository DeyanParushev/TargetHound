﻿namespace TargetHound.MVC.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TargetHound.DataModels;
    using TargetHound.DTOs;
    using TargetHound.Services.Automapper;
    using TargetHound.Services.Interfaces;

    [RequireHttps]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectApiController : ControllerBase
    {
        private readonly IProjectService projectService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ProjectApiController(IProjectService projectService, UserManager<ApplicationUser> userManager)
        {
            this.projectService = projectService;
            this.userManager = userManager;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        [HttpGet("{projectId}")]
        [Authorize]
        [Produces("application/json")]
        public async Task<ActionResult<ProjectDTO>> GetProject(string projectId)
        {
            try
            {
                var project = await this.projectService.GetProjectById<ProjectDTO>(projectId);
                project.CurrentUserId = this.userManager.GetUserId(this.User);
                return project;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPut, Route("Save")]
        [Authorize]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> PutProject(ProjectDTO projectInput)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var userId = this.userManager.GetUserId(this.User);
                    var project = this.mapper.Map<ProjectDTO, Project>(projectInput);
                    await this.projectService.SaveProject(project, userId);
                    return this.StatusCode(200);
                }
                else
                {
                    return this.StatusCode(422);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(500);
            }
        }
    }
}
