using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Hitta.Surveillance.Monitor.ColorCoders;
using Hitta.Surveillance.Monitor.DataCollectors;
using Hitta.Surveillance.Monitor.HealthLevels;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public partial class GraphMonitorPanel : MonitorPanel
    {
        readonly DataCollectorAdapter dataCollectorAdapter;
        readonly HealthLevel healthLevel;
        readonly Thread worker;
        private readonly GraphBase graph;

        public GraphMonitorPanel(DataCollectorAdapter dataCollectorAdapter, HealthLevel healthLevel, int yScale)
            : this(new Graph(false) { YScale = yScale, Interval = dataCollectorAdapter.Interval }, dataCollectorAdapter, healthLevel)
        {
        }
        public GraphMonitorPanel(GraphBase graph, DataCollectorAdapter dataCollectorAdapter, HealthLevel healthLevel)
        {
            this.dataCollectorAdapter = dataCollectorAdapter;
            this.healthLevel = healthLevel;
            InitializeComponent();

            this.graph = graph;
            this.graph.Dock = DockStyle.Fill;
            this.graph.BackColor = ColorCoderUnknown.Instance.BackColor;
            this.graph.ForeColor = ColorCoderUnknown.Instance.ForeColor1;
            this.graph.ForeColor2 = ColorCoderUnknown.Instance.ForeColor2;

            mainLayoutTable.Controls.Add(this.graph, 0, 0);

            CounterName = dataCollectorAdapter.Name;
            Description = dataCollectorAdapter.Description;

            worker = new Thread(RunWorker);
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                textPanelValue.ForeColor = value;
                textPanelName.ForeColor = value;
                textPanelDescription.ForeColor = value;
                graph.ForeColor = value;
            }
        }

        public Color ForeColor2
        {
            get { return graph.ForeColor2; }
            set
            {
                graph.ForeColor2 = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                textPanelValue.BackColor = value;
                textPanelName.BackColor = value;
                textPanelDescription.BackColor = value;
                graph.BackColor = value;
            }
        }

        void SetValue(int value)
        {
            var colorCoder = healthLevel.GetColorCoder(value);

            if(InvokeRequired)
            {
                MethodInvoker invoker = delegate
                                            {
                                                textPanelValue.Text = value.ToString(CultureInfo.CurrentCulture);
                                                
                                                ForeColor = colorCoder.ForeColor1;
                                                ForeColor2 = colorCoder.ForeColor2;
                                                BackColor = colorCoder.BackColor;

                                            };
                Invoke(invoker);
            }
            else
            {
                textPanelValue.Text = value.ToString(CultureInfo.CurrentCulture);
                ForeColor = colorCoder.ForeColor1;
                ForeColor2 = colorCoder.ForeColor2;
                BackColor = colorCoder.BackColor;
            }
            graph.Value = value;
        }

        public string CounterName
        {
            get { return textPanelName.Text; }
            set { textPanelName.Text = value; }
        }

        public string Description
        {
            get { return textPanelDescription.Text; }
            set { textPanelDescription.Text = value; }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            Trace.WriteLine("Handle Created!");

            if (!DesignMode) worker.Start();
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            Trace.WriteLine("Handle Destroyed!");
            worker.Abort();
        }

        void RunWorker()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1000 * dataCollectorAdapter.Interval);

                    if(!dataCollectorAdapter.Initialized)
                        dataCollectorAdapter.InitializeAdapter();

                    int measuredValue = dataCollectorAdapter.MeasuredValue;
                    
                    SetValue(measuredValue);
                }
            }
            catch (ThreadAbortException)
            {
                Trace.WriteLine("Graph worker thread aborted!");
            }
            finally
            {
                dataCollectorAdapter.Dispose();
            }
        }
    }
}
