using System;
using Newtonsoft.Json.Linq;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class ElasticsearchDataCollector : HttpDataCollector
    {
        private double lastMeasure = -1;
        private string[] path;
        private bool calculateFromLastMeasure;
        private int multiplier;

        public ElasticsearchDataCollector(string displayName, string description, int interval, string url, string path, bool calculateFromLastMeasure, int multiplier, string httpMethod, string body)
            : base(displayName, description, interval, url, httpMethod, body)
        {
            this.path = path.Split('/');
            this.calculateFromLastMeasure = calculateFromLastMeasure;
            this.multiplier = multiplier;
        }

        public ElasticsearchDataCollector(string displayName, string description, int interval, string url, string path, bool calculateFromLastMeasure, int multiplier)
            : this(displayName, description, interval, url, path, calculateFromLastMeasure, multiplier, "GET", string.Empty)
        { }


        public ElasticsearchDataCollector(string displayName, string description, int interval, string url, string path, bool calculateFromLastMeasure)
            : this(displayName, description, interval, url, path, calculateFromLastMeasure, 1)
        {}

        public ElasticsearchDataCollector(string displayName, string description, int interval, string url)
            : this(displayName, description, interval, url, "count", false)
        {}

        protected override int GetResponseInternal(string response)
        {
            var jObject = JObject.Parse(response);
            for (var i = 0; i < path.Length; i++)
            {
                if(i == (path.Length -1 ))
                {
                    var jVal = (JValue)jObject[path[i]];

                    double current = 0;

                    if (jVal.Type == JTokenType.Boolean)
                    {
                        current = (bool)jVal.Value ? 1 : 0;
                    }
                    else
                    {
                        current = jVal.Value<double>();
                    }

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

                if (path[i] == "*")
                {
                    foreach(var value in jObject.Values())
                    {
                        jObject = (JObject)value;
                    }

                    var x = jObject.First.Values();
                }
                else
                {
                    jObject = (JObject)jObject[path[i]];
                }
                
            }
            return -1;
        }
    }
}
