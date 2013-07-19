using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public partial class SignalPanel : MonitorPanel, PipPanel
    {
        public SignalPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            InitializeComponent();
        }

        public Point Position { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            const float fuzzinessPercentage = 0.3f;

            SmoothingMode oldSmothingMode = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int width = ClientRectangle.Width - (Margin.Left + Margin.Right);
            int height = ClientRectangle.Height - (Margin.Top + Margin.Bottom);

            int side = height > width ? width : height;

            RectangleF drawingRect = new Rectangle(Position.X + Margin.Left + ((width - side) / 2), Position.Y + Margin.Top + ((height - side) / 2), side, side);
            
            var path = new GraphicsPath();
            path.AddEllipse(drawingRect);

            var borderColor = Color.FromArgb(255, (int)(ForeColor.R / 1.3), (int)(ForeColor.G / 1.3), (int)(ForeColor.B / 1.3));
            
            var colorBlend = new ColorBlend();
            colorBlend.Colors = new[] { BackColor, Color.FromArgb(180, ForeColor), borderColor, ForeColor, ForeColor };
            colorBlend.Positions = new[] { 0.0f, fuzzinessPercentage, fuzzinessPercentage, fuzzinessPercentage * 1.2f, 1.0f };

            var gradientBrush = new PathGradientBrush(path);
            gradientBrush.InterpolationColors = colorBlend;
            e.Graphics.FillPath(gradientBrush, path);

            e.Graphics.SmoothingMode = oldSmothingMode;
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
