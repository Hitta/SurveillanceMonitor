using System.Collections.Generic;
using System.Windows.Forms;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public class PanelsCollection
    {
        readonly List<UserControl> panels;

        public PanelsCollection(List<UserControl> panels)
        {
            this.panels = panels;
        }
        
        public List<UserControl> Panels
        {
            get { return panels; }
        }
    }
}
