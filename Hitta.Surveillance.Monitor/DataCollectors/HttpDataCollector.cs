using System;
using System.Net;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class HttpDataCollector : DataCollectorAdapterBase
    {
        protected readonly string _url;
        private bool initialized;
        protected readonly WebClient client;

        protected HttpDataCollector(string displayName, string description, int interval, string url) : base(displayName, description, interval)
        {
            _url = url;
            client = new WebClient {Proxy = null};
        }

        protected virtual int GetResponseInternal(string response)
        {
            return int.Parse(response.Trim());
        }

        protected virtual int GetHttpResponse()
        {
            try
            {
                client.Proxy = null;
                return GetResponseInternal(client.DownloadString(_url));
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public override int MeasuredValue
        {
            get { return GetHttpResponse(); }
        }

        public override void InitializeAdapter()
        {
            initialized = true;
        }

        public override bool Initialized
        {
            get { return initialized; }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if(client != null)
                    client.Dispose();
            }
            finally 
            {
                base.Dispose(disposing);
            }
            
        }
    }
}
