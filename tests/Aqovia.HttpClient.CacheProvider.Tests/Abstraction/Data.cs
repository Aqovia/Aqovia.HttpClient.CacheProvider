using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqovia.HttpClient.CacheProvider.Tests.Abstraction
{
   public static class Data
    {
        public static readonly List<Profile> Profiles = new List<Profile>
        {
            new Profile {Id = 1,  Name = "Test User 1", CompanyId="sxF00121"},
            new Profile {Id = 2, Name = "Test User 2", CompanyId="sxF00122"},
            new Profile {Id = 3, Name = "Test User 3", CompanyId="sxF00123"},
            new Profile {Id = 4, Name = "Test User 4", CompanyId="sxF00123"},
        };

    }
}
