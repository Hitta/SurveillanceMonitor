using System.Drawing;
using System.Windows.Forms;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public abstract class MonitorPanel : UserControl
    {
        Size minimumSize = new Size(0, 50);

        public override Size MinimumSize
        {
            get
            {
                return minimumSize;
            }
            set
            {
                minimumSize = value;
            }
        }
    }
}
