using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;

namespace SillyLittleWebjob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        private static TelemetryClient _appInsights = new TelemetryClient();

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main()
        {
            _appInsights.InstrumentationKey = ConfigurationManager.AppSettings["appInsights"];

            _appInsights.TrackEvent($"{ConfigurationManager.AppSettings["appName"]} Is Starting!");

            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            var host = new JobHost(config);

            // Call specific method via reflection
            host.Call(typeof(Functions).GetMethod("ProcessDates"));

            // The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();

            _appInsights.TrackEvent($"{ConfigurationManager.AppSettings["appName"]} Is Starting!");
        }
    }
}
