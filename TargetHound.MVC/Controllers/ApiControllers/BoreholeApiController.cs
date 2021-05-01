namespace TargetHound.MVC.Controllers
{
    using System;
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

        //[HttpGet("{boreholeId}")]
        //[Authorize]
        //[IgnoreAntiforgeryToken]
        //public async Task<IActionResult> Get(string boreholeId)
        //{
        //    var boreholeName = await this.boreholeService.GetBoreholeName(boreholeId);
        //    var directory = Path.Combine(this.environment.ContentRootPath, "FilesCSV", $"{boreholeName}.csv");

        //    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
        //    {
        //        FileName = $"{boreholeName}.csv",
        //        Inline = false,  // false = prompt the user for downloading;  true = browser to try to show the file inline
        //    };
        //    Response.Headers.Add("Content-Disposition", cd.ToString());
        //    Response.Headers.Add("X-Content-Type-Options", "nosniff");

        //    return File(System.IO.File.ReadAllBytes(directory), "text/csv");
        //}

        //[HttpPost, Route("ExportCsv")]
        //[Authorize]
        //[IgnoreAntiforgeryToken]
        //public async Task<IActionResult> ExportBorehole(BoreholeDTO borehole)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var userId = this.userManager.GetUserId(this.User);
        //            var boreholeDataModel = this.mapper.Map<BoreholeDTO, Borehole>(borehole);
        //            var saveDirectory = Path.Combine(environment.ContentRootPath, "FilesCSV", $"{borehole.Name}.csv");
        //            await this.boreholeService.CreateBoreholeCsv(borehole.ProjectId, userId, boreholeDataModel, saveDirectory);
        //            return this.StatusCode(200);
        //        }
        //        else
        //        {
        //            return this.StatusCode(422);
        //        }
        //    }
        //    catch
        //    {
        //        return this.StatusCode(500);
        //    }
        //}

        [HttpGet("{boreholeId}")]
        public async Task<IActionResult> Save(string boreholeId)
        {
            try
            {
                var borehole = await this.boreholeService.GetBorehole<BoreholeDTO>(boreholeId);
                return Ok(borehole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            //try
            //{
            //    var borehole = this.mapper.Map<Borehole>(inputBorehole);
            //    await this.boreholeService.Create(borehole);
            //    return Created("", null);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            return Ok();
        }
    }
}
