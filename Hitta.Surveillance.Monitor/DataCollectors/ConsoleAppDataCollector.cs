namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class ConsoleAppDataCollector : DataCollectorAdapterBase
    {
        private readonly ConsoleAppDataReader appDataReader;
        private readonly string valueKey;
        private bool initialized;

        public ConsoleAppDataCollector(string displayName, string description, int interval, ConsoleAppDataReader appDataReader, string valueKey) : base(displayName, description, interval)
        {
            this.appDataReader = appDataReader;
            this.valueKey = valueKey;
        }

        public override int MeasuredValue
        {
            get { return appDataReader.GetValue(valueKey); }
        }

        public override void InitializeAdapter()
        {
            initialized = true;
            appDataReader.Start(Interval);
        }

        public override bool Initialized
        {
            get { return initialized; }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if(disposing)
                {
                    appDataReader.Stop();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
