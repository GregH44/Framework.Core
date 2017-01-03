using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controller
{
    public class MiscellaneousController : Microsoft.AspNetCore.Mvc.Controller
    {
        [Route("/Error")]
        public virtual IActionResult Error()
        {
            return View();
        }
    }
}
