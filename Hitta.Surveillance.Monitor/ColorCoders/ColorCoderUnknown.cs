using System.Drawing;

namespace Hitta.Surveillance.Monitor.ColorCoders
{
    public class ColorCoderUnknown : ColorCoder
    {
        private static ColorCoder instance = null;

        public Color ForeColor1
        {
            get { return Color.Gray; }
        }

        public Color ForeColor2
        {
            get { return Color.Gray; }
        }

        public Color BackColor
        {
            get { return Color.Black; }
        }

        public static ColorCoder Instance
        {
            get
            {
                if (instance == null)
                    instance = new ColorCoderUnknown();

                return instance;
            }
        }
    }
}
