namespace TargetHound.MVC.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using TargetHound.DataModels;
    using TargetHound.DTOs;
    using TargetHound.MVC.Areas.Identity;
    using TargetHound.MVC.ErrorMessages;
    using TargetHound.MVC.Models;
    using TargetHound.Services.Interfaces;
    using TargetHound.SharedViewModels.InputModels;
    using TargetHound.SharedViewModels.ViewModels;

    [RequireHttps]
    public class ProjectsController : Controller
    {
        private readonly IProjectService projectService;
        private readonly ICountriesService countriesService;
        private readonly IContractorService contractorService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectsController(
            IProjectService projectService,
            ICountriesService countriesService,
            IContractorService contractorService,
            IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            this.projectService = projectService;
            this.countriesService = countriesService;
            this.contractorService = contractorService;
            this.userService = userService;
            this.userManager = userManager;
        }

        public IActionResult Options()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var countries = await this.countriesService.GetAllCountriesAsync<CountryViewModel>();
            var viewModel = new ProjectCountryInputModel
            {
                Project = new ProjectInputModel(),
                Countries = countries,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCountryInputModel input)
        {
            var project = input.Project;
            var userId = this.userManager.GetUserId(this.User);

            try
            {
                if (userId != null)
                {
                    await this.projectService.CreateAsync(userId, project.Name, project.MagneticDeclination, project.CountryId);
                    ApplicationUser user = await this.userManager.GetUserAsync(this.User);
                    await this.userManager.AddToRoleAsync(user, SiteIdentityRoles.ProjectAdmin);

                    return this.RedirectToAction(nameof(this.Load));
                }

                var projectModel = new ProjectDTO
                {
                    Name = input.Project.Name,
                    MagneticDeclination = input.Project.MagneticDeclination,
                };

                return this.Redirect($"/Planning/{projectModel.Name}/{projectModel.MagneticDeclination}/");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.Redirect(nameof(ErrorViewModel));
            }
        }

        [Authorize]
        public async Task<IActionResult> Load()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projects = await this.projectService.GetProjectsByUserId<ProjectViewModel>(userId);

            foreach (var project in projects)
            {
                project.AdminName = await this.projectService.GetProjectAdminName(project.Id);
                project.IsCurrentUserAdmin = await this.projectService.IsUserIdSameWithProjectAdminId(userId, project.Id);
            }

            return this.View(projects);
        }

        [Authorize]
        public async Task<IActionResult> LoadProject(string projectId)
        {
            var userIsInProject = await this.projectService.IsUserInProject(this.userManager.GetUserId(this.User), projectId);

            if (userIsInProject)
            {
                return this.Redirect($"/Planning/{projectId}");
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, ProjectControllerErrorMessages.NotPartOfProjectError);
                return this.View(nameof(ErrorViewModel));
            }
        }

        [Authorize(Roles = SiteIdentityRoles.ProjectAdmin)]
        public async Task<IActionResult> Edit(string projectId)
        {
            try
            {
                var project = await this.projectService.GetProjectById<ProjectEditInputModel>(projectId);
                var countries = await this.countriesService.GetAllCountriesAsync<CountryViewModel>();

                project.Countries = countries;

                var isCurrentUserAdmin =
                    await this.projectService
                    .IsUserIdSameWithProjectAdminId(this.userManager.GetUserId(this.User), project.Id);

                if (!isCurrentUserAdmin)
                {
                    return this.Redirect(nameof(this.Load));
                }

                return this.View(project);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(nameof(ErrorViewModel));
            }
        }

        [HttpPost]
        [Authorize(Roles = SiteIdentityRoles.ProjectAdmin)]
        public async Task<IActionResult> EditProject(ProjectEditInputModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.ModelState.AddModelError(string.Empty, ProjectControllerErrorMessages.InvalidProjectData);
                    return this.View(nameof(ErrorViewModel));
                }

                var userId = this.userManager.GetUserId(this.User);

                if (await this.projectService.IsUserIdSameWithProjectAdminId(userId, model.Id))
                {
                    await this.projectService.EditProjectAsync(model.Id, model.Name, model.MagneticDeclination, model.CountryId);
                    return this.RedirectToAction(nameof(this.Edit), new { projectId = model.Id });
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ProjectControllerErrorMessages.NotProjectAdminError);
                    return this.View(nameof(ErrorViewModel));
                }
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(nameof(ErrorViewModel));
            }
        }

        [Authorize]
        public async Task<IActionResult> Users(string projectId)
        {
            var users = await this.projectService.GetProjectUsersAsync<UserViewModel>(projectId);
            this.ViewData["projectId"] = projectId;
            return this.View(users);
        }

        //// TODO: check the details after you have some data
        [Authorize]
        public async Task<IActionResult> Details(string projectId)
        {
            var details = await this.projectService.GetDetailsAsync<ProjectDetailsViewModel>(projectId);
            details.CurrentContractorName = await this.projectService.GetCurrentContractorAsync(projectId);
            details.UserCount = await this.projectService.GetUserCountAsync(projectId);
            details.Machines = await this.contractorService.GetDrillRigsAsync<DrillRigViewModel>(projectId);
            details.Boreholes = await this.projectService.GetBoreholesAsync<BoreholeViewModel>(projectId);
            details.Collars = await this.projectService.GetBoreholesAsync<CollarViewModel>(projectId);
            details.Targets = await this.projectService.GetBoreholesAsync<TargetViewModel>(projectId);

            return this.View(details);
        }

        [Authorize(Roles = SiteIdentityRoles.ProjectAdmin)]
        public async Task<IActionResult> AddUser(string projectId)
        {
            var addUserModel = new AddUserModel
            {
                Id = projectId,
            };

            return this.View(addUserModel);
        }

        [HttpPost]
        [Authorize(Roles = SiteIdentityRoles.ProjectAdmin)]
        public async Task<IActionResult> AddUser(string projectId, string email)
        {
            try
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                var linkToJoin = this.Url.Action(
                   "Join", "Projects", new { projectId = projectId }, this.Request.Scheme);
                var receiverEmail = email;
                var receiver = await this.userManager.FindByEmailAsync(receiverEmail);

                if (receiver == null)
                {
                    await this.userService.SendProjectInvitationAsync(currentUser.Email, currentUser.UserName, receiverEmail, receiverEmail, projectId, linkToJoin);
                }
                else
                {
                    await this.userService.SendProjectInvitationAsync(currentUser.Email, currentUser.UserName, receiver.Email, receiver.UserName, projectId, linkToJoin);
                }

                return this.RedirectToAction(nameof(this.Users), new { projectId = projectId });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(nameof(ErrorViewModel));
            }
        }
    }
}
