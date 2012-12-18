using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Hitta.Surveillance.Monitor.DataCollectors;
using Hitta.Surveillance.Monitor.HealthLevels;

namespace Hitta.Surveillance.Monitor
{
    public partial class MonitorPanel : UserControl
    {
        readonly DataCollectorAdapter _dataCollectorAdapter;
        readonly HealthLevel _healthLevel;
        readonly Thread worker;

        public MonitorPanel(DataCollectorAdapter dataCollectorAdapter, HealthLevel healthLevel, int yScale)
        {
            _dataCollectorAdapter = dataCollectorAdapter;
            _healthLevel = healthLevel;
            InitializeComponent();

            graph1.Interval = dataCollectorAdapter.Interval;
            graph1.YScale = yScale;
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
                graph1.ForeColor = value;
            }
        }

        public Color ForeColor2
        {
            get { return graph1.GridColor; }
            set
            {
                graph1.GridColor = value;
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
                graph1.BackColor = value;
            }
        }

        void SetValue(int value)
        {
            ColorCoders.ColorCoder colorCoder = _healthLevel.GetColorCoder(value);

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

        protected override void OnHandleCreated(System.EventArgs e)
        {
            Trace.WriteLine("Handle Created!");

            if (!DesignMode) worker.Start();
        }
        protected override void OnHandleDestroyed(System.EventArgs e)
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
                    Thread.Sleep(1000 * _dataCollectorAdapter.Interval);

                    if(!_dataCollectorAdapter.Initialized)
                        _dataCollectorAdapter.InitializeAdapter();

                    int measuredValue = _dataCollectorAdapter.MeasuredValue;
                    graph1.Value = measuredValue;
                    SetValue(measuredValue);
                }
            }
            catch (ThreadAbortException)
            {
                Trace.WriteLine("Graph worker thread aborted!");
            }
        }
    }
}
