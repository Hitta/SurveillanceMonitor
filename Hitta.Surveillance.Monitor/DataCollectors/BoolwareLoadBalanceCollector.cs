using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    class BoolwareLoadBalanceCollector : DataCollectorAdapterBase
    {

        private readonly int BOOLWARE_PORT = 7008;
        private System.Net.Sockets.TcpClient tcpClient;
        private string hostname = string.Empty;
        private bool initialized;

        public BoolwareLoadBalanceCollector(string displayName, string description, int interval, string hostname)
            : base(displayName, description, interval)
        {
            this.hostname = hostname;
        }

        /* Connect socket to boolware server and send LBST to get loadbalancing state.
         * Socket returns YES == boolware is in loadbalancing, NO == Not in loadbalancing.
         * Returns 0 == not in load, 1 == in load
         */
        private int GetValue(){
            int val = 404;        // default not in load
            try
            {
                tcpClient = new TcpClient(hostname, BOOLWARE_PORT);
                if( tcpClient.Connected ){
                    tcpClient.SendTimeout = 5;
                    NetworkStream ns = tcpClient.GetStream();
                    byte[] buff = Encoding.Default.GetBytes("LBST");
                    ns.Write(buff, 0, buff.Length);
                   
                    int nbytes = ns.Read(buff, 0, 4);
                    String resv = Encoding.Default.GetString(buff).Substring(0, nbytes);
                    if ("YES".Equals(resv))
                        val = 200;
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    tcpClient.Close();
                }
            }

            return val;
        }

        public override int MeasuredValue
        {
            get { return GetValue(); }
        }

        public override void InitializeAdapter()
        {
            initialized = true;
        }

        public override bool Initialized
        {
            get { return initialized; }
        }
    }
}
