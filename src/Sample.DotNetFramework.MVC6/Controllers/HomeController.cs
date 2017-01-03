using Microsoft.AspNetCore.Mvc;

namespace Sample.DotNetFramework.MVC6.Controllers
{
    public class HomeController : Framework.Core.Controller.ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            
            return View();
        }
    }
}
