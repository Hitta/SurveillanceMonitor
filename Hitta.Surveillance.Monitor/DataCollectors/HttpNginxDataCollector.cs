namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class HttpNginxDataCollector : HttpDataCollector
    {
        private long lastMeasure = -1;

        public HttpNginxDataCollector(string displayName, string description, int interval, string url)
            : base(displayName, description, interval, url)
        {}

        protected override int GetResponseInternal(string response)
        {
            string[] lines = response.Split('\n');
            if(lines.Length > 2)
            {
                string[] columns = lines[2].Split(' ');
                if(columns.Length > 2)
                {
                    long current = long.Parse(columns[3]);
                    try
                    {
                        if (lastMeasure == -1)
                        {
                            return 0;
                        }
                        return (int)System.Math.Abs((current - lastMeasure) / base.Interval);
                    }
                    finally
                    {
                        lastMeasure = current;
                    }
                }
            }
            return -1;
        }
    }
}