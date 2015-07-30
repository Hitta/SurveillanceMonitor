using System;
using System.Net;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class HttpDataCollector : DataCollectorAdapterBase
    {
        protected readonly string Url;
        protected readonly WebClient Client;
        private bool _initialized;
        private readonly string _httpMethod;
        private readonly string _body;

        protected HttpDataCollector(string displayName, string description, int interval, string url, string httpMethod, string body) 
            : base(displayName, description, interval)
        {
            Url = url;
            Client = new WebClient { Proxy = null };
            _httpMethod = httpMethod.ToUpperInvariant();
            _body = body;
        }

        protected HttpDataCollector(string displayName, string description, int interval, string url) 
            : this(displayName, description, interval, url, "GET", string.Empty)
        {}

        protected virtual int GetResponseInternal(string response)
        {
            return int.Parse(response.Trim());
        }

        protected virtual int GetHttpResponse()
        {
            try
            {
                Client.Proxy = null;

                return GetResponseInternal(_httpMethod == "GET" ? Client.DownloadString(Url) : Client.UploadString(Url, _httpMethod, _body));
            }
            catch (Exception e)
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
            _initialized = true;
        }

        public override bool Initialized
        {
            get { return _initialized; }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if(Client != null)
                    Client.Dispose();
            }
            finally 
            {
                base.Dispose(disposing);
            }
            
        }
    }
}
