using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Hitta.Surveillance.Monitor.ColorCoders;
using Hitta.Surveillance.Monitor.Controls;
using Hitta.Surveillance.Monitor.DataCollectors;
using Hitta.Surveillance.Monitor.HealthLevels;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public partial class BucketMonitorPanel : MonitorPanel
    {
        private readonly DataCollectorAdapter dataCollectorAdapter;
        private readonly HealthLevel healthLevel;
        private readonly Bucket bucket;
        readonly TextPanel textPanel;
        readonly Thread worker;

        public BucketMonitorPanel(DataCollectorAdapter dataCollectorAdapter, HealthLevel healthLevel, Bucket bucket)
        {
            this.dataCollectorAdapter = dataCollectorAdapter;
            this.healthLevel = healthLevel;
            this.bucket = bucket;
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

            this.bucket.Dock = DockStyle.Fill;
            this.bucket.BackColor = ColorCoderUnknown.Instance.BackColor;
            this.bucket.ForeColor = ColorCoderUnknown.Instance.ForeColor1;
            this.bucket.ForeColor2 = ColorCoderUnknown.Instance.ForeColor2;

            mainLayoutTable.Controls.Add(bucket, 0, 0);
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
                bucket.ForeColor = value;
            }
        }

        public Color ForeColor2
        {
            get { return bucket.ForeColor2; }
            set
            {
                bucket.ForeColor2 = value;
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
                bucket.BackColor = value;
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

            bucket.Value = value;
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
                    if (!dataCollectorAdapter.Initialized)
                        dataCollectorAdapter.InitializeAdapter();

                    int measuredValue = dataCollectorAdapter.MeasuredValue;
                    SetValue(measuredValue);

                    Thread.Sleep(1000 * dataCollectorAdapter.Interval);
                }
            }
            catch (ThreadAbortException)
            {
                Trace.WriteLine("Bucket worker thread aborted!");
            }
            finally
            {
                dataCollectorAdapter.Dispose();
            }
        }
    }
}
