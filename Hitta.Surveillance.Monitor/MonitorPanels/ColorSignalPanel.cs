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
    public partial class ColorSignalPanel : MonitorPanel, PipPanel
    {
        readonly DataCollectorAdapter dataCollectorAdapter;
        readonly HealthLevel healthLevel;
        readonly Thread worker;

        readonly SignalPanel signalPanel = new SignalPanel();
        readonly TextPanel textPanel;

        public ColorSignalPanel(DataCollectorAdapter dataCollectorAdapter, HealthLevel healthLevel)
        {
            this.dataCollectorAdapter = dataCollectorAdapter;
            this.healthLevel = healthLevel;

            InitializeComponent();

            worker = new Thread(RunWorker);

            var tableLayoutPanel = new TableLayoutPanel { Dock = DockStyle.Fill };
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            signalPanel.BackColor = ColorCoderUnknown.Instance.BackColor;
            signalPanel.Dock = DockStyle.Fill;
            signalPanel.ForeColor = ColorCoderUnknown.Instance.ForeColor1;

            if(String.IsNullOrEmpty(dataCollectorAdapter.Name))
            {
                tableLayoutPanel.RowCount = 1;
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            }
            else
            {
                tableLayoutPanel.RowCount = 2;
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

                textPanel = new TextPanel(StringAlignment.Center);
                textPanel.BackColor = ColorCoderUnknown.Instance.BackColor;
                textPanel.Dock = DockStyle.Fill;
                textPanel.ForeColor = ColorCoderUnknown.Instance.ForeColor1;
                textPanel.Margin = new Padding(0, 2, 0, 0);
                textPanel.Text = dataCollectorAdapter.Name;
                tableLayoutPanel.Controls.Add(textPanel, 0, 1);
            }

            tableLayoutPanel.Controls.Add(signalPanel, 0, 0);

            Controls.Add(tableLayoutPanel);
        }

        void SetValue(int value)
        {
            var colorCoder = healthLevel.GetColorCoder(value);

            if (InvokeRequired)
            {
                MethodInvoker invoker = delegate
                {
                    signalPanel.ForeColor = colorCoder.ForeColor1;
                };
                Invoke(invoker);
            }
            else
            {
                signalPanel.ForeColor = colorCoder.ForeColor1;
            }
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
                Trace.WriteLine("Color signal thread aborted!");
            }

            dataCollectorAdapter.Dispose();
        }

        public void PaintPanel(PaintEventArgs e)
        {
            signalPanel.PaintPanel(e);
            if(textPanel != null)
            {
                textPanel.PaintPanel(e);
            }
        }

        public void SetBounds(Rectangle bounds)
        {
            signalPanel.Position = bounds.Location;
            if(textPanel != null)
            {
                textPanel.Position = bounds.Location;
            }
            Top = bounds.Top;
            Left = bounds.Left;
            Width = bounds.Width;
            Height = bounds.Height;
            PerformLayout();
        }

        public void OnCreate()
        {
            OnHandleCreated(new EventArgs());
        }

        public void OnDestroy()
        {
            OnHandleDestroyed(new EventArgs());
        }
    }
}
