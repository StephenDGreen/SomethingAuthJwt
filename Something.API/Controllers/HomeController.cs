using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Something.Application;
using Something.Persistence;
using System.Collections.Generic;
using System.Security.Claims;

namespace Something.API.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext ctx;
        private readonly ISomethingCreateInteractor createInteractor;
        private readonly ISomethingReadInteractor readInteractor;

        public HomeController(ISomethingCreateInteractor createInteractor, ISomethingReadInteractor readInteractor, AppDbContext ctx)
        {
            this.createInteractor = createInteractor;
            this.readInteractor = readInteractor;
            this.ctx = ctx;
        }

        [AllowAnonymous]
        public IActionResult Authenticate()
        {
            GetSignin(GetClaims());

            return RedirectToAction("GetList");
        }

        [HttpPost]
        [Route("api/things")]
        public ActionResult Create([FromForm] string name)
        {
            if (name.Length < 1)
                return GetAll();

            createInteractor.CreateSomething(name);
            return GetAll();
        }

        [HttpGet]
        [Route("api/things")]
        public ActionResult GetList()
        {
            return GetAll();
        }

        private static ClaimsPrincipal GetClaims()
        {
            var customClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Example"),
                new Claim(ClaimTypes.Email, "example@mail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var customIdentity = new ClaimsIdentity(customClaims, "Custom Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { customIdentity });
            return userPrincipal;
        }
        private void GetSignin(ClaimsPrincipal userPrincipal)
        {
            HttpContext.SignInAsync(userPrincipal);
        }
        private ActionResult GetAll()
        {
            var result = readInteractor.GetSomethingList();
            return Ok(result);
        }
    }
}
