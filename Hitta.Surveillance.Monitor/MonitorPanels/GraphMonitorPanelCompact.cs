using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Hitta.Surveillance.Monitor.ColorCoders;
using Hitta.Surveillance.Monitor.DataCollectors;
using Hitta.Surveillance.Monitor.HealthLevels;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public partial class GraphMonitorPanelCompact : MonitorPanel
    {
        readonly DataCollectorAdapter dataCollectorAdapter;
        readonly HealthLevel healthLevel;
        readonly Thread worker;
        readonly TextPanel textPanel;
        readonly GraphBase graph;

        public GraphMonitorPanelCompact(DataCollectorAdapter dataCollectorAdapter, HealthLevel healthLevel, int yScale)
            : this(new Graph(true) { YScale = yScale, Interval = dataCollectorAdapter.Interval}, dataCollectorAdapter, healthLevel)
        {
            
        }
        public GraphMonitorPanelCompact(GraphBase graph, DataCollectorAdapter dataCollectorAdapter, HealthLevel healthLevel)
            :this(graph, dataCollectorAdapter, healthLevel, null)
        {
            
        }
        public GraphMonitorPanelCompact(GraphBase graph, DataCollectorAdapter dataCollectorAdapter, HealthLevel healthLevel, PipPanel pipPanel)
        {
            graph.PipPanel = pipPanel;
            this.dataCollectorAdapter = dataCollectorAdapter;
            this.healthLevel = healthLevel;
            InitializeComponent();

            var mainLayoutTable = new TableLayoutPanel();

            mainLayoutTable.ColumnCount = 1;
            mainLayoutTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayoutTable.Dock = DockStyle.Fill;

            textPanel = new TextPanel(StringAlignment.Center);

            if (String.IsNullOrEmpty(dataCollectorAdapter.Name))
            {
                mainLayoutTable.RowCount = 1;
                mainLayoutTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            }
            else
            {
                mainLayoutTable.RowCount = 2;
                mainLayoutTable.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
                mainLayoutTable.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

                textPanel.BackColor = ColorCoderUnknown.Instance.BackColor;
                textPanel.Dock = DockStyle.Fill;
                textPanel.ForeColor = Color.White;
                textPanel.Margin = new Padding(0, 2, 0, 0);
                textPanel.Text = dataCollectorAdapter.Name;
                mainLayoutTable.Controls.Add(textPanel, 0, 1);
            }

            this.graph = graph;
            this.graph.Dock = DockStyle.Fill;
            this.graph.BackColor = ColorCoderUnknown.Instance.BackColor;
            this.graph.ForeColor = ColorCoderUnknown.Instance.ForeColor1;
            this.graph.ForeColor2 = ColorCoderUnknown.Instance.ForeColor2;

            mainLayoutTable.Controls.Add(graph, 0, 0);
            Controls.Add(mainLayoutTable);
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
                textPanel.ForeColor = value;
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
                textPanel.BackColor = value;
                graph.BackColor = value;
            }
        }

        void SetValue(int value)
        {
            var colorCoder = healthLevel.GetColorCoder(value);

            if (InvokeRequired)
            {
                MethodInvoker invoker = delegate
                {

                    ForeColor = colorCoder.ForeColor1;
                    ForeColor2 = colorCoder.ForeColor2;
                    BackColor = colorCoder.BackColor;

                };
                Invoke(invoker);
            }
            else
            {
                ForeColor = colorCoder.ForeColor1;
                ForeColor2 = colorCoder.ForeColor2;
                BackColor = colorCoder.BackColor;
            }
            graph.Value = value;
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

                    if (!dataCollectorAdapter.Initialized)
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
