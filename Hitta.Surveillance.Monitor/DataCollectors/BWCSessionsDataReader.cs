using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class BWCSessionsDataReader : ConsoleAppDataReader
    {
        private readonly Regex regexp = new Regex(@"\s\|\s", RegexOptions.Compiled);

        public BWCSessionsDataReader(string application, string arguments) : base(application, arguments)
        {
        }

        public override void GetData(StreamReader reader, int interval)
        {
            var sessions = 0;
            var active = 0;

            try
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line != null)
                    {
                        var columns = regexp.Split(line);

                        if (columns.Length == 7)
                        {
                            sessions++;

                            if (!columns[6].StartsWith("Sleeping"))
                            {
                                active++;
                            }
                        }
                    }

                    Measurements["sessions.total"] = sessions;
                    Measurements["sessions.active"] = active;
                }
            }
            catch (Exception e)
            {
                throw new IOException("failed to read from stream", e);
            }
            
        }
    }
}
