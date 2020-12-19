namespace TargetHound.MVC.Controllers
{
    using System.Net.Http;
    using System.Threading.Tasks;
   
    using Microsoft.AspNetCore.Mvc;
    
    [RequireHttps]
    public class CountriesController : Controller
    {
        public CountriesController()
        {
        }

        public async Task<IActionResult> GetAll()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://api.first.org/data/v1/countries");

            return this.View();
        }
    }
}
