using System.Drawing;

namespace Hitta.Surveillance.Monitor.ColorCoders
{
    public class ColorCoderWarning:ColorCoder
    {
        public Color ForeColor1
        {
            get { return Color.Green; }
        }

        public Color ForeColor2
        {
            get { return Color.Chocolate; }
        }

        public Color BackColor
        {
            get { return Color.Orange; }
        }
    }
}
