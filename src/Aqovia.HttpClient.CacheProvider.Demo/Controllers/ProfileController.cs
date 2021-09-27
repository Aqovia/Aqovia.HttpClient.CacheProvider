using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Aqovia.HttpClient.CacheProvider.Demo.Models;
using  Aqovia.HttpClient.CacheProvider;

namespace Aqovia.HttpClient.CacheProvider.Demo.Controllers
{
    
    public class ProfileController : ApiController
    {
        private static readonly List<Profile> Profiles = new List<Profile>
            {
                new Profile {Id = 1,  Name = "Test User 1", CompanyId="sxF00121"},
                new Profile {Id = 2, Name = "Test User 2", CompanyId="sxF00122"},
                new Profile {Id = 3, Name = "Test User 3", CompanyId="sxF00123"},
                new Profile {Id = 4, Name = "Test User 4", CompanyId="sxF00123"},
            };

        [CacheOutput(ClientTimeSpan = 50, 
                    ServerTimeSpan = 50)]
        public IHttpActionResult Get()
        {
            return Ok(Profiles);
        }
        [CacheOutput(ClientTimeSpan = 50, 
                    ServerTimeSpan = 50, 
                    RedisCacheConnectionString = "127.0.0.1:6379,ssl=false", 
                    AppInsightInstrumentationKey = "11111111-1111-1111-1111-111111111111")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var profile = Profiles.FirstOrDefault(_ => _.Id == id);
            return profile == null ? (IHttpActionResult) NotFound() : Ok(profile);
        }
    }
}
