using Microsoft.AspNetCore.Mvc;
using Something.Application;
using Something.Persistence;

namespace Something.API.Controllers
{
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
