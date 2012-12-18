using System.Drawing;

namespace Hitta.Surveillance.Monitor.ColorCoders
{
    public class ColorCoderCustom:ColorCoder
    {
        readonly Color foreColor1;
        readonly Color foreColor2;
        readonly Color backColor;

        public ColorCoderCustom(Color foreColor1, Color foreColor2, Color backColor)
        {
            this.foreColor1 = foreColor1;
            this.foreColor2 = foreColor2;
            this.backColor = backColor;
        }

        public Color ForeColor1
        {
            get { return foreColor1; }
        }

        public Color ForeColor2
        {
            get { return foreColor2; }
        }

        public Color BackColor
        {
            get { return backColor; }
        }
    }
}
