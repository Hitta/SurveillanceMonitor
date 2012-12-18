using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public abstract class ConsoleAppDataReader
    {
        readonly Thread worker;
        private readonly string application;
        private readonly string arguments;
        private bool started;

        protected Dictionary<string, int> Measurements = new Dictionary<string, int>();  

        protected ConsoleAppDataReader(string application, string arguments)
        {
            this.application = application;
            this.arguments = arguments;

            worker = new Thread(RunWorker);
        }

        public int GetValue(string key)
        {
            if(Measurements.ContainsKey(key))
            {
                return Measurements[key];
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void Start(int interval)
        {
            if (!started)
            {
                worker.Start(interval);
                started = true;
            }
        }

        public void Stop()
        {
            worker.Abort();
        }

        public abstract void GetData(StreamReader reader, int interval);

        private void ExecuteApplication(int interval)
        {
            var p = new Process();
            
            p.StartInfo.FileName = this.application;
            p.StartInfo.Arguments = this.arguments;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            try
            {
                p.Start();

                GetData(p.StandardOutput, interval);

                p.WaitForExit();
            }
            catch (Win32Exception e)
            {
            }
            catch (InvalidOperationException e)
            {
            }
            catch(IOException e)
            {
            }
        }

        void RunWorker(object data)
        {
            var interval = (int) data;
            try
            {
                while (true)
                {

                    Thread.Sleep(1000 * interval);

                    ExecuteApplication(interval);
                }
            }
            catch (ThreadAbortException)
            {
                Trace.WriteLine("Console app executer aborted!");
            }
        }
    }
}
