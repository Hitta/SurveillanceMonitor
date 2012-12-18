using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

namespace Hitta.Surveillance.Monitor
{
    public partial class Graph : GraphBase
    {
        readonly bool showValue;
        readonly object lockObject = new object();
        readonly Thread worker;
        int graphOffset;
        readonly List<int> values = new List<int>();
        const int STEP_LENGTH = 3;

        void AddValue(int value)
        {
            lock (lockObject)
            {
                values.Add(value);
                if (values.Count > 1000)
                    values.RemoveAt(0);
            }
        }

        public int Interval { get; set; }
        

        public Graph():this(false)
        {
        }
        public Graph(bool showValue)
        {
            this.showValue = showValue;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            InitializeComponent();

            Interval = 1;
            YScale = 100;
            ForeColor2 = Color.DarkGreen;

            if(this.showValue)
            {
                
            }

            worker = new Thread(RunWorker);

        }

        protected override void OnHandleCreated(System.EventArgs e)
        {
            base.OnHandleCreated(e);

            Trace.WriteLine("Handle Created!");

            if (!DesignMode) worker.Start();
        }
        protected override void OnHandleDestroyed(System.EventArgs e)
        {
            base.OnHandleDestroyed(e);

            Trace.WriteLine("Handle Destroyed!");
            worker.Abort();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            lock (lockObject)
            {

                using (var path = new GraphicsPath())
                using (var pathPen = new Pen(ForeColor, 3))
                using (Brush brush = new SolidBrush(Color.FromArgb(130, ForeColor2)))
                {
                    int lineNum = 0;
                    for (var index = values.Count - 2; index > -1; index -= 1)
                    {
                        var xOffset = DisplayRectangle.Width - (lineNum * 3);
                        lineNum++;

                        var yScale1 = values[index + 1] / (double)YScale;
                        var yScale2 = values[index] / (double)YScale;

                        var y1 = DisplayRectangle.Height - (int)(DisplayRectangle.Height * yScale1);
                        var y2 = DisplayRectangle.Height - (int)(DisplayRectangle.Height * yScale2);

                        path.AddLine(xOffset, y1, xOffset - STEP_LENGTH, y2);

                        if (xOffset < 0) break;
                    }
                    var oldMode = e.Graphics.SmoothingMode;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                    if (showValue)
                    {
                        var stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        stringFormat.FormatFlags = StringFormatFlags.NoWrap;

                        var textualValue = values.LastOrDefault().ToString();
                        var fontSize = TextUtils.GetFontSize(e.Graphics, textualValue, Font, ClientRectangle.Size);

                        e.Graphics.DrawString(textualValue, new Font(Font.FontFamily, fontSize), brush, ClientRectangle, stringFormat);
                    }
                    
                    
                    e.Graphics.DrawPath(pathPen, path);
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
                    Thread.Sleep(1000 * Interval);
                    graphOffset -= STEP_LENGTH;
                    if (graphOffset == -30)
                        graphOffset = 0;

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
