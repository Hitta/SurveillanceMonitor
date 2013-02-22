using System;
using System.Net;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class BasicAuthHttpDataCollector : HttpDataCollector
    {

        protected BasicAuthHttpDataCollector(string displayName, string description, int interval, string url, string user, string pwd)
            : base(displayName, description, interval, url)
        {
            client.Credentials = new NetworkCredential(user, pwd);
        }

    }
}
