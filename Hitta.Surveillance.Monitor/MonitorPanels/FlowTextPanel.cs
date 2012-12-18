using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public partial class FlowTextPanel : MonitorPanel
    {
        private readonly TextProvider textProvider;
        // 40 cols
        // 8 rows
        object lockObject = new object();

        private double yOffset = -50;
        private Size textBlockSize = new Size(300,50);
        private Rectangle fadeRectTop = new Rectangle();
        private Rectangle fadeRectBottom = new Rectangle();

        private Brush fadeBrushTop;
        private Brush fadeBrushBottom;

        Queue<TextBlock> textBlockQueue = new Queue<TextBlock>(10);

        private readonly System.Threading.Thread worker;


        public FlowTextPanel(TextProvider textProvider)
        {
            this.textProvider = textProvider;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            InitializeComponent();
            textProvider.OnText += textProvider_OnText;

            worker = new Thread(RunWorker);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            textBlockSize = new Size(this.Width, this.Height / 8);

            setupShade();
        }

        private void setupShade()
        {
            fadeRectTop = new Rectangle(0, 0, this.Width, (int)(this.Height * 0.1));
            fadeRectBottom = new Rectangle(0, this.Height - (int)(this.Height * 0.1), this.Width, (int)(this.Height * 0.1));
            fadeBrushTop = new LinearGradientBrush(fadeRectTop, Color.Black, Color.FromArgb(0, 0, 0, 0), LinearGradientMode.Vertical);

            var r = fadeRectBottom;
            r.Inflate(0, 1);
            fadeBrushBottom = new LinearGradientBrush(r, Color.FromArgb(0, 0, 0, 0), Color.Black, LinearGradientMode.Vertical);
        }

        protected override void OnHandleCreated(System.EventArgs e)
        {
            base.OnHandleCreated(e);
            setupShade();

            if (!DesignMode) worker.Start();

            textProvider.start();
        }
        protected override void OnHandleDestroyed(System.EventArgs e)
        {
            base.OnHandleDestroyed(e);

            textProvider.Dispose();
            worker.Abort();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            List<TextBlock> textBlocks = null;

            var offset = 0d;

            lock (lockObject)
            {
                textBlocks = new List<TextBlock>(textBlockQueue.Reverse().ToArray());
                offset = yOffset;
            }

            

            if (textBlocks.Count == 0)
            {
                return;
            }

            var paintOffset = offset;

            foreach (var block in textBlocks)
            {
                if ((paintOffset + textBlockSize.Height > 0) && (paintOffset < this.Height))
                {
                    e.Graphics.CompositingMode = CompositingMode.SourceCopy;
                    e.Graphics.InterpolationMode = InterpolationMode.Low;

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                    e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

                    e.Graphics.DrawImageUnscaled(block.bitmap, 0, (int)paintOffset);
                }

                paintOffset += textBlockSize.Height;
            }

            e.Graphics.CompositingMode = CompositingMode.SourceOver;
            e.Graphics.FillRectangle(fadeBrushTop, fadeRectTop);
            e.Graphics.FillRectangle(fadeBrushBottom, fadeRectBottom);

            lock (lockObject)
            {
                //Console.WriteLine("textblocks: " + textBlocks.Count + " offset: " + offset + " move: " + Math.Pow(yOffset / 100d, 2));

                if(yOffset < 0)
                {
                    yOffset += Math.Pow(yOffset / 150d, 2);
                }
            }
        }

        void textProvider_OnText(object sender, TextEventArgs e)
        {

            lock(lockObject)
            {
                while((textBlockQueue.Count * textBlockSize.Height) + yOffset > (this.Height + textBlockSize.Height))
                {
                    textBlockQueue.Dequeue().Dispose();
                }

                textBlockQueue.Enqueue(new TextBlock(e, textBlockSize, Font, ForeColor));

                yOffset -= textBlockSize.Height;
            }
        }

        void RunWorker()
        {
            try
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(1);

                    Invoke(new MethodInvoker(delegate
                    {
                        Invalidate();
                        Update();
                    }));
                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("flow text panel worker aborted!");
            }
        }
    }

    class TextBlock : IDisposable
    {
        private readonly Font font;
        private readonly Color foreColor;
        public readonly Bitmap bitmap;
        public long speed = -1;

        public TextBlock(TextEventArgs textEventArgs, Size size, Font font, Color foreColor)
        {
            this.font = font;
            this.foreColor = foreColor;
            this.bitmap = new Bitmap(size.Width, size.Height);

            SetText(textEventArgs);
        }

        public TextBlock SetDrawSpeed(long speed)
        {
            this.speed = speed;
            return this;
        }

        public void Dispose()
        {
            this.font.Dispose();
            this.bitmap.Dispose();
        }

        public TextBlock SetText(TextEventArgs textEventArgs)
        {
            using(var graphics = Graphics.FromImage(bitmap))
            {
                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.FormatFlags = StringFormatFlags.NoWrap;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                SizeF fontSizeTemplate = new SizeF(bitmap.Size);
                fontSizeTemplate.Width = fontSizeTemplate.Width - fontSizeTemplate.Height; //make room for useragent bitmap

                if(textEventArgs.isGeo)
                {
                    fontSizeTemplate.Width = fontSizeTemplate.Width - fontSizeTemplate.Height; //make room for geo bitmap
                }

                var fontSize = TextUtils.GetFontSize(graphics, textEventArgs.Text, font, fontSizeTemplate);

                var textSize = graphics.MeasureString(textEventArgs.Text, new Font(font.FontFamily, fontSize));

                using (Brush bgBrush = new SolidBrush(Color.Black))
                using (Brush brush = new SolidBrush(Color.FromArgb(72, 206, 247)))
                {
                    graphics.FillRectangle(bgBrush, new RectangleF(0,0,bitmap.Size.Width, bitmap.Size.Height));
                    graphics.DrawString(textEventArgs.Text, new Font(font.FontFamily, fontSize), brush, new RectangleF(fontSizeTemplate.Height, 0, fontSizeTemplate.Width, fontSizeTemplate.Height), stringFormat);

                    

                    if(textEventArgs.isGeo)
                    {
                        var xPos = fontSizeTemplate.Height + (fontSizeTemplate.Width/2) + (textSize.Width/2);
                        graphics.DrawImage(Resource.earth, new RectangleF(xPos, 0, fontSizeTemplate.Height, fontSizeTemplate.Height));
                    }

                    Bitmap userAgentBitmap = null;
                    switch (textEventArgs.userAgentType)
                    {
                        case UserAgentType.ANDROID:
                            userAgentBitmap = Resource.android;
                            break;
                        case UserAgentType.IPHONE:
                            userAgentBitmap = Resource.apple;
                            break;
                        case UserAgentType.CHROME:
                            userAgentBitmap = Resource.chrome;
                            break;
                        case UserAgentType.FIREFOX:
                            userAgentBitmap = Resource.firefox;
                            break;
                        case UserAgentType.IE:
                            userAgentBitmap = Resource.ie;
                            break;
                        case UserAgentType.NETSCAPE:
                            userAgentBitmap = Resource.netscape;
                            break;
                        case UserAgentType.OPERA:
                            userAgentBitmap = Resource.opera;
                            break;
                        case UserAgentType.SAFARI:
                            userAgentBitmap = Resource.safari;
                            break;
                        case UserAgentType.SAFARI_ANDROID:
                            userAgentBitmap = Resource.safari_android;
                            break;
                        default:
                            userAgentBitmap = Resource.netscape;
                            break;
                    }

                    if(bitmap != null)
                        graphics.DrawImage(userAgentBitmap, new RectangleF(0, 0, fontSizeTemplate.Height, fontSizeTemplate.Height));
                }
            }

            
            return this;
        }
    }
}
