using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Hitta.Surveillance.Monitor.LayoutEngines;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public partial class CompositPanel : MonitorPanel
    {
        public CompositPanel(IEnumerable<UserControl> childControls):this(null, childControls, null, 0, null)
        {
        }
        public CompositPanel(LayoutEngine layoutEngine, IEnumerable<UserControl> childControls)
            : this(layoutEngine, childControls, null, 0, null)
        {
        }
        public CompositPanel(IEnumerable<UserControl> childControls, string title, int headerHeight)
            : this(null, childControls, title, headerHeight, null)
        {
        }
        public CompositPanel(LayoutEngine layoutEngine, IEnumerable<UserControl> childControls, string title, int headerHeight)
            : this(layoutEngine, childControls, title, headerHeight, null)
        {
        }
        public CompositPanel(LayoutEngine layoutEngine, IEnumerable<UserControl> childControls, string title, int headerHeight, UserControl headerControl)
        {
            InitializeComponent();

            var containerPanel = new ContainerPanel(layoutEngine ?? new HorizontalLayoutEngine()) { Dock = DockStyle.Fill, Margin = new Padding(0)};

            var tableLayoutPanel = new TableLayoutPanel{Dock = DockStyle.Fill};
            tableLayoutPanel.ColumnCount = 2;

            foreach (var control in childControls)
            {
                containerPanel.Controls.Add(control);
            }

            if (title != null)
            {
                var titlePanel = new TextPanel { Text = title, Dock = DockStyle.Fill, ForeColor = Color.LightGray};

                tableLayoutPanel.RowCount = 2;
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, headerHeight));
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                if (headerControl != null)
                {
                    tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, headerHeight));
                    tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                    tableLayoutPanel.Controls.Add(headerControl, 0, 0);
                    tableLayoutPanel.Controls.Add(titlePanel, 1, 0);
                }
                else
                {
                    tableLayoutPanel.Controls.Add(titlePanel, 0, 0);
                    tableLayoutPanel.SetColumnSpan(titlePanel, 2);
                }

                tableLayoutPanel.Controls.Add(containerPanel, 0, 1);
            }
            else
            {
                tableLayoutPanel.RowCount = 1;
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                tableLayoutPanel.Controls.Add(containerPanel, 0, 0);
            }

            tableLayoutPanel.SetColumnSpan(containerPanel, 2);
            Controls.Add(tableLayoutPanel);
        }
    }
}
