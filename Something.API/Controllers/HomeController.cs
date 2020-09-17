using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Something.Application;
using Something.Persistence;
using Something.Security;
using System.Collections.Generic;
using System.Security.Claims;

namespace Something.API.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext ctx;
        private readonly ISomethingUserManager userManager;
        private readonly ISomethingCreateInteractor createInteractor;
        private readonly ISomethingReadInteractor readInteractor;

        public HomeController(ISomethingCreateInteractor createInteractor, ISomethingReadInteractor readInteractor, AppDbContext ctx, ISomethingUserManager userManager)
        {
            this.createInteractor = createInteractor;
            this.readInteractor = readInteractor;
            this.ctx = ctx;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public ActionResult Authenticate()
        {
            var token = userManager.GetUserToken();
            return Ok(new { access_token = token});
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
        private ActionResult GetAll()
        {
            var result = readInteractor.GetSomethingList();
            return Ok(result);
        }
    }
}
