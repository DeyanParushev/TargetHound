namespace TargetHound.MVC.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TargetHound.DataModels;
    using TargetHound.DTOs;
    using TargetHound.Services;
    using TargetHound.Services.Automapper;

    [RequireHttps]
    [ApiController]
    [Route("api/[controller]")]
    public class BoreholeApiController : ControllerBase
    {
        private readonly IBoreholeService boreholeService;
        private readonly UserManager<ApplicationUser> userManager;
        private IMapper mapper;

        public BoreholeApiController(
            IBoreholeService boreholeService, 
            UserManager<ApplicationUser> userManager)
        {
            this.boreholeService = boreholeService;
            this.userManager = userManager;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        [HttpPost, Route("Save")]
        [Authorize]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> PutProject(BoreholeDTO borehole)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var userId = this.userManager.GetUserId(this.User);
                    var boreholeDataModel = this.mapper.Map<BoreholeDTO, Borehole>(borehole);

                    await this.boreholeService.UpdateBoreholesAsync(borehole.ProjectId, userId, boreholeDataModel);
                    return this.StatusCode(200);
                }
                else
                {
                    return this.StatusCode(422);
                }
            }
            catch
            {
                return this.StatusCode(500);
            }
        }

        [HttpGet, Route("Export")]
        [Authorize]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ExportProject(BoreholeDTO borehole)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var userId = this.userManager.GetUserId(this.User);
                    var boreholeDataModel = this.mapper.Map<BoreholeDTO, Borehole>(borehole);

                    var export = await this.boreholeService.ExportBoreholeAsync(borehole.ProjectId, userId, boreholeDataModel);
                    return File(new System.Text.UTF8Encoding().GetBytes(export), "text/csv", borehole.Name);
                }
                else
                {
                    return this.StatusCode(422);
                }
            }
            catch
            {
                return this.StatusCode(500);
            }
        }
    }
}
