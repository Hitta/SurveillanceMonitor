using System;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public interface DataCollectorAdapter : IDisposable
    {
        int Interval { get; }
        string Name { get; }
        string Description { get; }
        int MeasuredValue { get; }
        void InitializeAdapter();
        bool Initialized{get;}
    }
}
