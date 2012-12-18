using System.Xml;
using System;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class HttpXpathDataCollector : HttpDataCollector
    {
        private long lastMeasure = -1;
        private string xpath;
        private bool calculateFromLastMeasure;

        public HttpXpathDataCollector(string displayName, string description, int interval, string url, string xpath, bool calculateFromLastMeasure)
            : base(displayName, description, interval, url)
        {
            this.xpath = xpath;
            this.calculateFromLastMeasure = calculateFromLastMeasure;
        }

        public HttpXpathDataCollector(string displayName, string description, int interval, string url)
            : this(displayName, description, interval, url, "count", false)
        {
        }

        public int doIt(string data)
        {
            return GetResponseInternal(data);
        }

        protected override int GetResponseInternal(string response)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNodeList nodes = doc.SelectNodes(this.xpath);
                int current = (nodes.Count > 0) ? int.Parse(nodes[0].InnerText) : -1;
                try
                {
                    if (lastMeasure == -1)
                    {
                        return 0;
                    }
                    return (int)System.Math.Abs((current - lastMeasure) / base.Interval);
                }
                finally
                {
                    lastMeasure = current;
                }
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public static void main(string[] args) {
            Console.WriteLine("foo");
        }
    }
}
