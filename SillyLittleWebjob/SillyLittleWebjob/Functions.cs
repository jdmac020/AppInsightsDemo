using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;

namespace SillyLittleWebjob
{
    public class Functions
    {
        TelemetryClient _appInsights = new TelemetryClient { InstrumentationKey = ConfigurationManager.AppSettings["appInsights"] };
        
        [NoAutomaticTrigger]
        public void ProcessDates()
        {
            _appInsights.TrackEvent("Starting Function Run!");

            var dateStrings = LoadDateStrings();

            Console.WriteLine();

            foreach (var date in dateStrings)
            {

                try
                {
                    
                    var response = ProcessDate(date);
                    Console.WriteLine(response);

                    _appInsights.TrackTrace($"Input String [{date}] Triggered Response [{response}]");
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    _appInsights.TrackException(new Exception($"Unhandled Exception While Processing Input String [{date}]", e));
                }

                Console.WriteLine();

            }
            
            _appInsights.TrackEvent("Finished With Function Run!");
            Console.ReadLine();
        }

        private string ProcessDate(string dateString)
        {
            var date = DateTime.Parse(dateString);

            if (date.DayOfWeek == DayOfWeek.Friday && date.Day == 13)
            {
                throw new Exception("Encountered Unhandled Superstition...");
            }
            if (date.Month == 10 && date.Day == 5)
            {
                return "Hey My Birthday!";
            }
            if (date.Month == 7 && date.Day == 4)
            {
                return "Happy Birthday America!";
            }
            else
            {
                return "BORING";
            }
        }

        private List<string> LoadDateStrings()
        {
            var strings = new List<string>();

            strings.Add("10/5/2018");
            strings.Add("9/22/2018");
            strings.Add("2/28/2019");
            strings.Add("9/13/2019");
            strings.Add("7/4/2019");

            return strings;
        }
    }
}
