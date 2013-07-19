using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public partial class TextPanel : UserControl, PipPanel
    {
        readonly StringAlignment alignment;

        public TextPanel(StringAlignment alignment)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            this.alignment = alignment;
        }

        public TextPanel():this(StringAlignment.Near)
        {
            InitializeComponent();
        }

        public Point Position { get; set; }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var stringFormat = new StringFormat();
            stringFormat.Alignment = alignment;
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.FormatFlags = StringFormatFlags.NoWrap;

            RectangleF safeFrame = ClientRectangle;
            safeFrame.Location = Position;

            var fontSize = TextUtils.GetFontSize(e.Graphics, Text, Font, safeFrame.Size);

            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            using (Brush brush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(Text, new Font(Font.FontFamily, fontSize), brush, safeFrame, stringFormat);
            }
        }

        public void PaintPanel(PaintEventArgs e)
        {
            OnPaint(e);
        }

        public void SetBounds(Rectangle bounds)
        {
            throw new NotImplementedException();
        }

        public void OnCreate()
        {
            throw new NotImplementedException();
        }

        public void OnDestroy()
        {
            throw new NotImplementedException();
        }
    }
}
