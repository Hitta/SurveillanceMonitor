using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class BWCPerfCountersDataReader : ConsoleAppDataReader
    {
        private double lastMeasure = -1;
        private readonly Regex regexp = new Regex(@"(?<=XML request:\t)\d+", RegexOptions.Compiled);

        public BWCPerfCountersDataReader(string application, string arguments)
            : base(application, arguments)
        {
        }

        public override void GetData(StreamReader reader, int interval)
        {
            try
            {
                var match = regexp.Match(reader.ReadToEnd());
                if(match.Success)
                {
                    var current = double.Parse(match.Value);

                    var perSecond = (int)Math.Abs((current - lastMeasure) / interval);

                    Measurements["perfcounters.xmlrequests"] = perSecond;

                    lastMeasure = current;
                }
            }
            catch (Exception e)
            {
                throw new IOException("failed to read from stream", e);
            }
            
        }
    }
}
