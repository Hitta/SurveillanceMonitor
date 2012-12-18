using System.Drawing;

namespace Hitta.Surveillance.Monitor.ColorCoders
{
    public class ColorCoderNormal:ColorCoder
    {
        public Color ForeColor1
        {
            get { return Color.LightGreen; }
        }

        public Color ForeColor2
        {
            get { return Color.Green; }
        }

        public Color BackColor
        {
            get { return Color.Black; }
        }
    }
}
