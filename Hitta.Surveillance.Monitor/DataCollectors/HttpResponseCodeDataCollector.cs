using System;
using System.Net;


namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class HttpResponseCodeDataCollector : HttpDataCollector
    {
        
        public HttpResponseCodeDataCollector(string displayName, string description, int interval, string url)
            : base(displayName, description, interval, url)
        {
            
        }

        protected override int GetHttpResponse()
        {
            try
            {
                try
                {
                    Client.DownloadString(base.Url);
                    return 200;
                }
                catch (WebException ex)
                {
                    return (int)((HttpWebResponse)ex.Response).StatusCode;
                }
            }
            catch(Exception)
            {
                return -1;
            }
        }
    }
}