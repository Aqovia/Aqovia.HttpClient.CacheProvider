using System;
using System.Collections.Generic;
using System.Text;

namespace Aqovia.HttpClient.CacheProvider
{
   public class CacheTime
    {
        public TimeSpan ClientTimeSpan { get; set; }

        public TimeSpan? SharedTimeSpan { get; set; }

        public DateTimeOffset AbsoluteExpiration { get; set; }
    }
}
