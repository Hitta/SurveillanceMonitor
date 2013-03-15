using Boolware;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class BoolwareTimestampDataCollector : BoolwareDataCollector
    {
        private const string TIME_STAMP_STRING = "data_time_stamp";
        private string indexname;
        private string dsnname;
        private string fieldname;

        public BoolwareTimestampDataCollector(string displayName, string description, int interval, string hostName, string dsnName, string indexName, string fieldName)
            : base(displayName, description, interval, hostName)
        {
            this.indexname = indexName;
            this.dsnname = dsnName;
            this.fieldname = fieldName;
        }

        protected override double GetValue()
        {
            double result = -1;
            try
            {
                Connect();
                client.Attach(dsnname);
                client.Execute(string.Format("tables({0}) FIND \"FINDNAME\":{1}", indexname, TIME_STAMP_STRING));
                Boolware.Tuple[] tuple = client.FetchTuples(indexname, fieldname, 0, 1, 500);
                string val = tuple[0].GetValue(fieldname);
                // parse the datetime from string //
                Match match = Regex.Match(val, @"created=[0-9]{8} [0-9]{2}:[0-9]{2}", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    val = match.Groups[0].Value.Replace("created=", "");
                    DateTime timestamp = DateTime.ParseExact(val, "yyyyMMdd HH:mm", null );
                    result = (DateTime.Now - timestamp).TotalHours;
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        protected override int GetMeasuredValue()
        {
            return (int)GetValue();
        }


    }
}
