namespace TargetHound.MVC.Controllers
{
    using System.IO;
    using System.Threading.Tasks;
  
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
  
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
        private readonly IWebHostEnvironment environment;
        private IMapper mapper;
        private string boreholeId;

        public BoreholeApiController(
            IBoreholeService boreholeService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            this.boreholeService = boreholeService;
            this.userManager = userManager;
            this.environment = environment;
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

        [HttpPost, Route("ExportCsv")]
        [Authorize]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ExportBorehole(BoreholeDTO borehole)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    this.boreholeId = borehole.Id;
                    var userId = this.userManager.GetUserId(this.User);
                    var boreholeDataModel = this.mapper.Map<BoreholeDTO, Borehole>(borehole);
                    var saveDirectory = Path.Combine(environment.ContentRootPath, "FilesCSV", $"{borehole.Name}.csv");
                    await this.boreholeService.ExportBoreholeAsync(borehole.ProjectId, userId, boreholeDataModel, saveDirectory);
                    return this.Redirect(nameof(this.ExportFile));
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

        [HttpGet]
        [Authorize]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ExportFile()
        {
            var boreholeName = await this.boreholeService.GetBoreholeName(this.boreholeId);
            var directory = Path.Combine(this.environment.ContentRootPath, "FilesCSV", $"{boreholeName}.csv");

            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = $"{boreholeName}.csv",
                Inline = false,  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(System.IO.File.ReadAllBytes(directory), "text/csv");
        }
    }
}
