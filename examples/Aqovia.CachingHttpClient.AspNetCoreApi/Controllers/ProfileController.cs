using System.Collections.Generic;
using System.Linq;
using Aqovia.CachingHttpClient.AspNetCoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aqovia.CachingHttpClient.AspNetCoreApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private static readonly List<Profile> Profiles = new List<Profile>
        {
            new Profile { Id = 1, Name = "Test User 1", CompanyId = "sxF00121" },
            new Profile { Id = 2, Name = "Test User 2", CompanyId = "sxF00122" },
            new Profile { Id = 3, Name = "Test User 3", CompanyId = "sxF00123" },
            new Profile { Id = 4, Name = "Test User 4", CompanyId = "sxF00123" },
        };

        [HttpGet]
        [ResponseCache(Duration = 360)]
        public IActionResult Get()
        {
            return Ok(Profiles);
        }
    }
}
