using System;
using System.Diagnostics;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class PerformanceCounterDataCollector:DataCollectorAdapterBase
    {
        readonly string _categoryName;
        readonly string _counterName;
        readonly string _instanceName;
        readonly string _machineName;
        bool _initialized;

        PerformanceCounter _performanceCounter;

        public PerformanceCounterDataCollector(string categoryName, string counterName, string instanceName, string machineName, int interval, string displayName, string description):base(displayName, description, interval)
        {
            _categoryName = categoryName;
            _counterName = counterName;
            _instanceName = instanceName;
            _machineName = machineName;
        }


        public override int MeasuredValue
        {
            get
            {
                try
                {
                    return (int)_performanceCounter.NextValue();
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public override void InitializeAdapter()
        {
            _performanceCounter = new PerformanceCounter(_categoryName, _counterName, _instanceName, _machineName);
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
                if (!Disposed)
                {
                    if (_performanceCounter != null)
                        _performanceCounter.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
