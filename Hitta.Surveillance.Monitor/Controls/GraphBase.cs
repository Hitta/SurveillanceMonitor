using System;
using System.Drawing;
using System.Windows.Forms;
using Hitta.Surveillance.Monitor.MonitorPanels;

namespace Hitta.Surveillance.Monitor
{
    public abstract class GraphBase : Control
    {
        private PipPanel pipPanel;
        private PipPanel pipPanelRight;
        private Color foreColor2;
        public int Value { get; set; }

        public PipPanel PipPanel
        {
            set { pipPanel = value; }
            get { return pipPanel; }
        }

        public PipPanel PipPanelRight
        {
            set { pipPanelRight = value; }
            get { return pipPanelRight; }
        }

        protected override void OnResize(EventArgs e)
        {
            int side = Height / 2;
            if (pipPanel != null)
            {
                var bounds = new Rectangle(new Point(Left, Top), new Size(side, side));
                pipPanel.SetBounds(bounds);
            }
            if (pipPanelRight != null)
            {
                var bounds = new Rectangle(new Point(Width - side, Top), new Size(side, side));
                pipPanelRight.SetBounds(bounds);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if(pipPanel != null)
            {
                pipPanel.OnCreate();
            }
            if (pipPanelRight != null)
            {
                pipPanelRight.OnCreate();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if(pipPanel != null)
            {
                pipPanel.OnDestroy();
            }
            if (pipPanelRight != null)
            {
                pipPanelRight.OnDestroy();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if(pipPanel != null)
            {
                pipPanel.PaintPanel(e);
            }
            if (pipPanelRight != null)
            {
                pipPanelRight.PaintPanel(e);
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