using System.Windows.Forms.Layout;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public partial class ContainerPanel : MonitorPanel
    {
        private readonly LayoutEngine layoutEngine;

        public ContainerPanel(LayoutEngine layoutEngine)
        {
            this.layoutEngine = layoutEngine;

            InitializeComponent();
        }

        public override LayoutEngine LayoutEngine
        {
            get
            {
                return layoutEngine;
            }
        }
    }
}
