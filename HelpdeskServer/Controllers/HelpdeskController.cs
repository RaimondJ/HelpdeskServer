using Microsoft.AspNetCore.Mvc;

namespace HelpdeskServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpdeskController : Controller
    {
        [HttpPost]
        [Route("post")]
        public string Index()
        {
            return "123";
        }

        [HttpGet]
        [Route("posts")]
        public string getPosts() 
        {
            return "0";
        }
    }
}
