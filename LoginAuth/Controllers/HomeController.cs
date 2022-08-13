using LoginAuth.Models;
using LoginAuth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace LoginAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthenticateService _authenticateService;
        private IConfiguration _config;

        public HomeController(ILogger<HomeController> logger
            , IAuthenticateService authenticateService
            , IConfiguration config)
        {
            _logger = logger;
            _authenticateService = authenticateService;
            _config = config;
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
                token = Generate(user);

            return Json(token);
        }

        [Authorize]
        public IActionResult Authenticate()
        {
            var currentUser = GetCurrentUser();

            if (currentUser.Username != null)
                return Json(new { isAuthenticated = true });

            return Json(new { isAuthenticated = false });
        }

        public string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.GivenName, user.GivenName),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var expiration = DateTime.Now.AddMinutes(5);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                , _config["Jwt:Audience"]
                , claims
                , expires: expiration
                , signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel GetCurrentUser()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }

            return null;
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
