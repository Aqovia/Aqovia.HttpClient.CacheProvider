using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Aqovia.HttpClient.CacheProvider.Configurations
{
    public class AppInsightTelemetryInitializer : ITelemetryInitializer
    {

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = "Interxion.Caching";
        }
    }
}
