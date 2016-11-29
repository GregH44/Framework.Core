using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.DotNetFramework.Common.DTO;
using Sample.DotNetFramework.MVC6.Areas.UserManager.ViewModels;
using Sample.DotNetFramework.MVC6.Extensions;
using Sample.DotNetFramework.ServicesLayer.Interfaces;
using System.Threading.Tasks;

namespace Sample.DotNetFramework.MVC6.Areas.UserManager.Controller
{
    [Area("UserManager")]
    public class UserController : Framework.Core.Controller.ControllerBase
    {
        private readonly IUserService userService = null;

        public UserController(IUserService userService, ILogger<UserController> Logger)
            : base(Logger)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            return View((await userService.GetList()).ToModels<UserViewModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            userService.AddOrUpdate(user.ToModel<UserModel>());

            return RedirectToAction(nameof(UserController.Index));
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            return View(userService.Get(id).ToModel<UserViewModel>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            userService.AddOrUpdate(user.ToModel<UserModel>());

            return RedirectToAction(nameof(UserController.Index));
        }

        [HttpGet]
        public IActionResult Delete(long id)
        {
            return View(userService.Get(id).ToModel<UserViewModel>());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            userService.Delete(id);

            return RedirectToAction(nameof(UserController.Index));
        }
    }
}
