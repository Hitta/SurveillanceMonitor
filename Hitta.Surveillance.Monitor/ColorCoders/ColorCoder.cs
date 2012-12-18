using System.Drawing;

namespace Hitta.Surveillance.Monitor.ColorCoders
{
    public interface ColorCoder
    {
        Color ForeColor1 { get; }
        Color ForeColor2 { get; }
        Color BackColor { get; }
    }
}
