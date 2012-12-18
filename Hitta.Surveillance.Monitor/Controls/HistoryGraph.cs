using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using Timer = System.Threading.Timer;
using Hitta.Surveillance.Monitor.MonitorPanels;

namespace Hitta.Surveillance.Monitor
{
    public partial class HistoryGraph : GraphBase
    {
        readonly bool showValue;
        private readonly TimeSpan updateInterval;
        private readonly int chunkSize;
        private readonly TimeSpan chunkUpdateInterval;
        readonly object lockObject = new object();
        readonly Thread worker;
        readonly List<int> values = new List<int>();
        readonly List<int> values2 = new List<int>();

        const int STEP_LENGTH = 2;
        Color gridColor;
        Timer chunkUpdateTimer;

        public HistoryGraph()
            : this(false, new TimeSpan(0, 0, 0, 1), 60, new TimeSpan(0, 0, 1, 0))
        {
        }
        public HistoryGraph(bool showValue, TimeSpan updateInterval, int chunkSize, TimeSpan chunkUpdateInterval)
        {
            this.showValue = showValue;
            this.updateInterval = updateInterval;
            this.chunkSize = chunkSize;
            this.chunkUpdateInterval = chunkUpdateInterval;
            
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            InitializeComponent();

            
            YScale = 100;
            ForeColor2 = Color.DarkGreen;

            worker = new Thread(RunWorker);
        }

        void AddValue(int value)
        {
            Console.WriteLine("Adding value");
            lock (lockObject)
            {
                values.Add(value);
                if (values.Count > chunkSize)
                {
                    values.RemoveAt(0);
                }
            }
        }

        void AddValue2(int value)
        {
            Console.WriteLine("Adding value2");
            lock (lockObject)
            {
                values2.Add(value);
                if (values2.Count > chunkSize)
                {
                    values2.RemoveAt(0);
                }
            }
        }

        public int ChunkSize { get; set; }

        private void ChunkUpdateTimerCallback(object state)
        {
            if(values.Count >= chunkSize)
            {
                Console.WriteLine("ChunkUpdateTimerCallback - adding value");
                int averageChunkValue = values.GetRange(values.Count - chunkSize, chunkSize).Sum() / chunkSize;
                AddValue2(averageChunkValue);
            }
            else
            {
                Console.WriteLine("ChunkUpdateTimerCallback - changing");
                chunkUpdateTimer.Change(new TimeSpan(0, 0, 0, 1), chunkUpdateInterval);
            }
        }

        protected override void OnHandleCreated(System.EventArgs e)
        {
            base.OnHandleCreated(e);

            Trace.WriteLine("Handle Created!");

            if (!DesignMode)
            {
                worker.Start();
                chunkUpdateTimer = new Timer(new TimerCallback(ChunkUpdateTimerCallback), null, chunkUpdateInterval, chunkUpdateInterval);
            }
        }
        protected override void OnHandleDestroyed(System.EventArgs e)
        {
            base.OnHandleDestroyed(e);

            Trace.WriteLine("Handle Destroyed!");
            worker.Abort();
            if(chunkUpdateTimer != null) chunkUpdateTimer.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            lock (lockObject)
            {

                using (GraphicsPath path = new GraphicsPath())
                using (GraphicsPath path2 = new GraphicsPath())
                using (Pen pathPen = new Pen(ForeColor, 3))
                using (Pen pathPen2 = new Pen(Color.FromArgb(150, ForeColor), 3))
                using (Brush brush = new SolidBrush(Color.FromArgb(130, ForeColor2)))
                {
                    int lineNum = 0;
                    float xOffset = ClientRectangle.Width;
                    float stepLength = ClientRectangle.Width / 2f / (float)(chunkSize - 1);

                    for (int index = values.Count - 2; index > -1; index -= 1)
                    {
                        lineNum++;

                        float yScale1 = values[index + 1] / (float)YScale;
                        float yScale2 = values[index] / (float)YScale;

                        float y1 = ClientRectangle.Height - (ClientRectangle.Height * yScale1);
                        float y2 = ClientRectangle.Height - (ClientRectangle.Height * yScale2);

                        path.AddLine(xOffset, y1, xOffset-=stepLength, y2);

                        if(lineNum == chunkSize)break;
                    }

                    lineNum = 0;
                    xOffset = ClientRectangle.Width / 2f;
                    for (int index = values2.Count - 2; index > -1; index -= 1)
                    {
                        lineNum++;

                        float yScale1 = values2[index + 1] / (float)YScale;
                        float yScale2 = values2[index] / (float)YScale;

                        float y1 = ClientRectangle.Height - (ClientRectangle.Height * yScale1);
                        float y2 = ClientRectangle.Height - (ClientRectangle.Height * yScale2);

                        path2.AddLine(xOffset, y1, xOffset -= stepLength, y2);

                        if (lineNum == chunkSize) break;
                    }

                    e.Graphics.DrawLine(Pens.Blue, ClientRectangle.Width / 2f, 0, ClientRectangle.Width / 2f, ClientRectangle.Bottom);

                    SmoothingMode oldMode = e.Graphics.SmoothingMode;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                    if (showValue)
                    {
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        stringFormat.FormatFlags = StringFormatFlags.NoWrap;

                        string textualValue = values.LastOrDefault().ToString();
                        int fontSize = TextUtils.GetFontSize(e.Graphics, textualValue, Font, ClientRectangle.Size);

                        e.Graphics.DrawString(textualValue, new Font(Font.FontFamily, fontSize), brush, ClientRectangle, stringFormat);
                    }

                    e.Graphics.DrawPath(pathPen, path);
                    e.Graphics.DrawPath(pathPen2, path2);
                    e.Graphics.SmoothingMode = oldMode;
                    
                }

                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.DarkGray, ButtonBorderStyle.Solid);
            }
        }

        void RunWorker()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(updateInterval);

                    AddValue(Value);
                    Invalidate();
                }
            }
            catch (ThreadAbortException)
            {
                Trace.WriteLine("Graph worker thread aborted!");
            }
        }
    }
}
