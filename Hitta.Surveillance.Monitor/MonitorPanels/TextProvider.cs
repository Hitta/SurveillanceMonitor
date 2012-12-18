using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public interface TextProvider : IDisposable
    {
        event EventHandler<TextEventArgs> OnText;
        void start();
    }

    public class TextEventArgs : EventArgs
    {
        public readonly string Text;
        public readonly UserAgentType userAgentType;
        public readonly bool isGeo;

        public TextEventArgs(string text):this(text, UserAgentType.NETSCAPE, false)
        {
        }

        public TextEventArgs(string text, UserAgentType userAgentType, bool isGeo)
        {
            this.Text = text;
            this.userAgentType = userAgentType;
            this.isGeo = isGeo;
        }

        public override string ToString()
        {
            return "Message: " + this.Text;
        }
    }

    public enum UserAgentType
    {
        SAFARI, SAFARI_ANDROID, CHROME, IE, FIREFOX, NETSCAPE, OPERA, ANDROID, IPHONE
    }
}
