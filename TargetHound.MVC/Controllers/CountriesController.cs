using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TargetHound.Data;

namespace TargetHound.MVC.Controllers
{
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
