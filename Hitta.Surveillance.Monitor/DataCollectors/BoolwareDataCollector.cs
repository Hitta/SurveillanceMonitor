using Boolware;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class BoolwareDataCollector : DataCollectorAdapterBase
    {
        private const string XML_REQUESTS = "XML request:";

        private bool initialized;
        private readonly Client client;

        private double lastMeasure = -1;

        public BoolwareDataCollector(string displayName, string description, int interval, string hostname) : base(displayName, description, interval)
        {
            client = new Client(true);

            client.Connect(hostname, "", "");
        }

        private double GetValue()
        {
            foreach (var counter in client.GetPerfCounters().counters)
            {
                if (XML_REQUESTS.Equals(counter.counterName))
                {
                    return counter.accumulated;
                }
            }

            return -1;
        }

        private int GetMeasuredValue()
        {
            var value = GetValue();
            var last = lastMeasure;
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
