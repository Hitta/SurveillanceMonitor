using System;
using System.Drawing;

namespace Hitta.Surveillance.Monitor
{
    public class TextUtils
    {
        public static int GetFontSize(Graphics graphics, string text, Font font, SizeF bounds)
        {
            var fontSizeWidth = (int)bounds.Height;
            var fontSizeHeight = (int)bounds.Height;

            if (fontSizeWidth == 0) fontSizeWidth = 1;
            if (fontSizeHeight == 0) fontSizeHeight = 1;

            var newFont = new Font(font.FontFamily, fontSizeWidth);
            var textSize = graphics.MeasureString(text, newFont);

            if (textSize.Width > bounds.Width)
            {
                var reduceFontFactor = (bounds.Width / textSize.Width) * 1.1;
                fontSizeWidth = (int)Math.Floor(fontSizeWidth * reduceFontFactor);
            }

            if (textSize.Height > bounds.Height)
            {
                var reduceFontFactor = (bounds.Height / textSize.Height) * 1.2;
                fontSizeHeight = (int)Math.Floor(fontSizeWidth * reduceFontFactor);
            }

            var fontSize = fontSizeWidth > fontSizeHeight ? fontSizeHeight : fontSizeWidth;

            if (fontSize == 0)
                fontSize = 1;

            return fontSize;
        }
    }
}
