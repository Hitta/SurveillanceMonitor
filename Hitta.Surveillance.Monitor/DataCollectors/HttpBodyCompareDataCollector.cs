using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class HttpBodyCompareDataCollector : HttpDataCollector
    {
        private string url2;

        public HttpBodyCompareDataCollector(string displayName, string description, int interval, string url, string url2)
            : base(displayName, description, interval, url)
        {
            this.url2 = url2;
        }

        protected override int GetResponseInternal(string response)
        {
            try
            {
                string response2 = Client.DownloadString(url2);
                return response.CompareTo(response2);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
