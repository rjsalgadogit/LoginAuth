using LoginAuth.Models;
using LoginAuth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace LoginAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthenticateService _authenticateService;        

        public HomeController(ILogger<HomeController> logger, IAuthenticateService authenticateService)
        {
            _logger = logger;
            _authenticateService = authenticateService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Login(LoginViewModel model)
        {
            var user = _authenticateService.Authenticate(model);
            var token = string.Empty;

            if (user != null)
                token = _authenticateService.Generate(user);

            return Json(token);
        }        

        #region Default

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}
