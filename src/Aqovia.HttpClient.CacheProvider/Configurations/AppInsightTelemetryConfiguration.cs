using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aqovia.HttpClient.CacheProvider.Configurations
{
   public class AppInsightTelemetryConfiguration
   {
       private readonly string InstrumentationKey = "";
        public AppInsightTelemetryConfiguration(string instrumentationKey)
        {
            InstrumentationKey = instrumentationKey;

        }
        public TelemetryClient GetTelemetryClient() {

            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();

            configuration.InstrumentationKey = InstrumentationKey;
            configuration.TelemetryInitializers.Add(new AppInsightTelemetryInitializer());

            return new TelemetryClient(configuration);
        }

    }
}
