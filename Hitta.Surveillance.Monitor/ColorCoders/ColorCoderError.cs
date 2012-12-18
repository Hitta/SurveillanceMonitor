using System.Drawing;

namespace Hitta.Surveillance.Monitor.ColorCoders
{
    public class ColorCoderError:ColorCoder
    {
        public Color ForeColor1
        {
            get { return Color.DimGray; }
        }

        public Color ForeColor2
        {
            get { return Color.DarkRed; }
        }

        public Color BackColor
        {
            get { return Color.Red; }
        }
    }
}
