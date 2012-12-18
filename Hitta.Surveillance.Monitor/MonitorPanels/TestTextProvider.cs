using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;


namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public class TestTextProvider : TextProvider
    {
        private int index;
        public event EventHandler<TextEventArgs> OnText;
        public void start()
        {
            throw new NotImplementedException();
        }

        private Random random = new Random(30);

        private long counter = 0;

        private readonly System.Threading.Thread worker;

        public TestTextProvider()
        {
            //var timer = new Timer(500);
            //timer.Elapsed += timer_Elapsed;
            //timer.Start();

            worker = new Thread(RunWorker);
            worker.Start();
        }

        void RunWorker()
        {
            try
            {
                while (true)
                {
                    counter++;
                    if (counter % 20 == 0)
                    {
                        Console.WriteLine("***************** sleeping ****************");
                        Thread.Sleep(2000);
                    }

                    Thread.Sleep((int)(500 + random.NextDouble() * 500D));
                    OnText(this, new TextEventArgs("test" + ++index));
                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("test text provider aborted!");
            }
        }

        //void timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    if(OnText != null)
        //    {
        //        counter++;
        //        if(counter % 10 == 0)
        //        {
        //            Console.WriteLine("sleeping");
        //            System.Threading.Thread.Sleep(2000);
        //        }

        //        System.Threading.Thread.Sleep((int) (random.NextDouble()*500D));
        //        OnText(this, new TextEventArgs("test" + ++index));
        //    }
        //}


        public void Dispose()
        {
            worker.Abort();
        }
    }
}