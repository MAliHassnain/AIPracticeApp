using Microsoft.AspNetCore.Mvc;
using PracticeApp.Core.Models;

namespace PracticeApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public HomeStatus Get()
        {
            return new HomeStatus("AI Practice App", "Ready to practice.");
        }
    }
}
