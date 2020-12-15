namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TargetHound.Data;

    [RequireHttps]
    public class CountriesController : Controller
    {
        private readonly TargetHoundContext dbContext;

        public CountriesController(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> GetAll()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://api.first.org/data/v1/countries");

            return this.View();
        }
    }
}
