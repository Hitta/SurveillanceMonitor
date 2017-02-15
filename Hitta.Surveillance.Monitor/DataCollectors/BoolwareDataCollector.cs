using Boolware;
using System;
using System.Text;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class BoolwareDataCollector : DataCollectorAdapterBase
    {
        private const string XML_REQUESTS = "XML/JSON request:";

        protected bool initialized;
        protected Client client;
        private string hostname = string.Empty;
        protected double lastMeasure = -1;

        public BoolwareDataCollector(string displayName, string description, int interval, string hostname) : base(displayName, description, interval)
        {
            this.hostname = hostname;
        }

        protected void Connect()
        {
            try
            {
                /* The client can be null after a network error try to restore connection */
                if (client == null)
                {
                    client = new Client(true);
                    client.Connect(this.hostname, "", "");
                }
            }
            catch(Exception){
                Disconnect();
            }
        }

        protected void Disconnect()
        {
            if (client != null)
            {
                client.Disconnect(true);
                client.Dispose();
                client = null;
            }
        }

        protected virtual double GetValue()
        {
            Connect();
            try
            {
                foreach (var counter in client.GetPerfCounters().counters)
                {
                    if (XML_REQUESTS.Equals(counter.counterName))
                    {
                        return counter.accumulated;
                    }
                }
            }
            catch (Exception)
            {
                Disconnect();
            }
            return -1;
        }

        protected virtual int GetMeasuredValue()
        {
            var value = GetValue();
            double last = lastMeasure;
            lastMeasure = value;

            if (value == -1 || last == -1) return -1;

            return (int)(value - last);
        }

        public override int MeasuredValue
        {
            get { return GetMeasuredValue(); }
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
                {
                    client.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
