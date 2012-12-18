using System;
using System.Drawing;
using System.Windows.Forms;
using Hitta.Surveillance.Monitor.MonitorPanels;

namespace Hitta.Surveillance.Monitor
{
    public abstract class GraphBase : Control
    {
        private PipPanel pipPanel;
        private Color foreColor2;
        public int Value { get; set; }

        public PipPanel PipPanel
        {
            set { pipPanel = value; }
            get { return pipPanel; }
        }
        protected override void OnResize(EventArgs e)
        {
            if (pipPanel != null)
            {
                var bounds = new Rectangle(new Point(Left, Top), new Size(Height / 2, Height / 2));
                pipPanel.SetBounds(bounds);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if(pipPanel != null)
            {
                pipPanel.OnCreate();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if(pipPanel != null)
            {
                pipPanel.OnDestroy();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if(pipPanel != null)
            {
                pipPanel.PaintPanel(e);
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
                Invalidate(true);
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

        public int YScale { get; set; }
    }
}