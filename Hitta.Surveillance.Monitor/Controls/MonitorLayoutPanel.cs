using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Hitta.Surveillance.Monitor.LayoutEngines;

namespace Hitta.Surveillance.Monitor
{
    public partial class MonitorLayoutPanel : Panel
    {
        LayoutEngine layoutEngine;
        public MonitorLayoutPanel()
        {
            InitializeComponent();
        }

        public override LayoutEngine LayoutEngine
        {
            get { return layoutEngine ?? (layoutEngine = new VerticalFlowLayoutEngine()); }
        }
    }
}
