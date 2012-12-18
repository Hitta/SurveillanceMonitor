using System;
using System.Threading;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public abstract class DataCollectorAdapterBase:DataCollectorAdapter
    {
        readonly string _displayName;
        readonly string _description;
        readonly int _interval;
        bool _disposed;

        protected DataCollectorAdapterBase(string displayName, string description, int interval)
        {
            _displayName = displayName;
            _description = description;
            _interval = interval;
        }

        public int Interval
        {
            get { return _interval; }
        }

        public string Name
        {
            get { return _displayName; }
        }

        public string Description
        {
            get { return _description; }
        }

        protected bool Disposed { get { return _disposed; } }

        public abstract int MeasuredValue{get;}
        public abstract void InitializeAdapter();
        public abstract bool Initialized{get; }

        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
