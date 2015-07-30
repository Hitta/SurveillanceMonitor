using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Hitta.Surveillance.Monitor.Controls
{
    public partial class Bucket : Control
    {
        private readonly bool showValue;
        private int value;
        private readonly int maxValue;
        private Color foreColor2 = Color.Silver;
        private const int margin = 5;
        private RectangleF maxFillSpace;
        private GraphicsPath path;

        public Bucket():this(true, 100)
        {
        }

        public Bucket(bool showValue, int maxValue)
        {
            this.showValue = showValue;
            this.maxValue = maxValue;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            
            InitializeComponent();
        }

        public int Value
        {
            get { return value; }
            set
            {
                if(this.value != value)
                {
                    this.value = value;
                    Invalidate();
                }
            }
        }

        

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            maxFillSpace = new RectangleF(DisplayRectangle.Left + margin, DisplayRectangle.Top + margin, DisplayRectangle.Width - (2 * margin), DisplayRectangle.Height - (2 * margin));

            path = new GraphicsPath();
            path.AddLine(maxFillSpace.Left, maxFillSpace.Top, maxFillSpace.Left, maxFillSpace.Bottom);
            path.AddLine(maxFillSpace.Left, maxFillSpace.Bottom, maxFillSpace.Right, maxFillSpace.Bottom);
            path.AddLine(maxFillSpace.Right, maxFillSpace.Bottom, maxFillSpace.Right, maxFillSpace.Top);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using(var brush = new SolidBrush(ForeColor))
            {
                var fillHeight = 0;
                if(maxValue > 0)
                {
                    fillHeight = (int)((value / (float)maxValue) * maxFillSpace.Height);
                }

                e.Graphics.FillRectangle(brush, maxFillSpace.Left, maxFillSpace.Bottom - fillHeight, maxFillSpace.Width, fillHeight);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            using (var bucketPen = new Pen(Color.Silver, 5))
            {
                bucketPen.LineJoin = LineJoin.Round;
                bucketPen.StartCap = LineCap.Round;
                bucketPen.EndCap = LineCap.Round;

                e.Graphics.DrawPath(bucketPen, path);
            }
            
            if(showValue)
            {
                var vertical = DisplayRectangle.Height > DisplayRectangle.Width;
                var size = vertical
                    ? new SizeF(maxFillSpace.Height, maxFillSpace.Width)
                    : new SizeF(maxFillSpace.Width, maxFillSpace.Height);
                var fontSize = TextUtils.GetFontSize(e.Graphics, value.ToString(), Font, size);
                var stringSize = e.Graphics.MeasureString(value.ToString(), new Font(Font.FontFamily, fontSize));

                var xOffset = vertical ? (DisplayRectangle.Width/2f) - (stringSize.Height/2): (DisplayRectangle.Width / 2f) - (stringSize.Width / 2);
                var yOffset = vertical ? maxFillSpace.Height : (maxFillSpace.Height / 2) - (stringSize.Height / 2);

                var matrix = new Matrix();
                if (vertical)
                {
                    matrix.RotateAt(-90f, new PointF(0, 0));
                }

                matrix.Translate(xOffset,yOffset, MatrixOrder.Append);

                e.Graphics.Transform = matrix;

                using (var textBrush = new SolidBrush(ForeColor2))
                {
                    e.Graphics.DrawString(value.ToString(), new Font(Font.FontFamily, fontSize), textBrush, 0, 0);
                }
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (value.Equals(ForeColor)) return;

                base.ForeColor = value;
                Invalidate();
            }
        }

        public Color ForeColor2
        {
            get { return foreColor2; }
            set
            {
                if (value.Equals(foreColor2)) return;

                foreColor2 = value;
                Invalidate();
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
                if (value.Equals(base.BackColor)) return;

                base.BackColor = value;
                Invalidate();
            }
        }


    }
}
