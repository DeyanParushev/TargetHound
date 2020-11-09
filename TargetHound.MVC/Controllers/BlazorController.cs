namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TargetHound.Services.Interfaces;

    public class BlazorController : Controller
    {
        public IActionResult Index()
        {
            return this.Redirect("/StartPlanning");
        }
    }
}
