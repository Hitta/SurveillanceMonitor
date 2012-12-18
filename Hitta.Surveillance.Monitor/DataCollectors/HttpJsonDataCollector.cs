using Newtonsoft.Json.Linq;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class HttpJsonDataCollector : HttpDataCollector
    {
        private double lastMeasure = -1;
        private string[] path;
        private bool calculateFromLastMeasure;
        private int multiplier;

        public HttpJsonDataCollector(string displayName, string description, int interval, string url, string path, bool calculateFromLastMeasure, int multiplier)
            : base(displayName, description, interval, url)
        {
            this.path = path.Split('/');
            this.calculateFromLastMeasure = calculateFromLastMeasure;
            this.multiplier = multiplier;
        }

        public HttpJsonDataCollector(string displayName, string description, int interval, string url, string path, bool calculateFromLastMeasure)
            : this(displayName, description, interval, url, path, calculateFromLastMeasure, 1)
        {}

        public HttpJsonDataCollector(string displayName, string description, int interval, string url)
            : this(displayName, description, interval, url, "count", false, 1)
        {}

        protected override int GetResponseInternal(string response)
        {
            var jObject = JObject.Parse(response);
            for (var i = 0; i < path.Length; i++)
            {
                if(i == (path.Length -1 ))
                {
                    var current = jObject.Value<double>(path[i]);
                    if (this.calculateFromLastMeasure)
                    {
                        try
                        {
                            if (this.lastMeasure == -1) return 0;

                            return (int)(this.multiplier * System.Math.Abs((current - lastMeasure) / base.Interval));
                        }
                        finally
                        {
                            this.lastMeasure = current;
                        }
                    }

                    return (int)(current * this.multiplier);
                }
                
                jObject = (JObject)jObject[path[i]];
            }
            return -1;
        }
    }
}
