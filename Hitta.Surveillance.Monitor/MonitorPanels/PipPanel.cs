using System.Drawing;
using System.Windows.Forms;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public interface PipPanel
    {
        void PaintPanel(PaintEventArgs e);
        void SetBounds(Rectangle bounds);

        void OnCreate();
        void OnDestroy();
    }
}
