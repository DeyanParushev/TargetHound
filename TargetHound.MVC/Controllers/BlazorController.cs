namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BlazorController : Controller
    {
        public IActionResult Index()
        {
            return this.Redirect("/Planning");
        }
    }
}
