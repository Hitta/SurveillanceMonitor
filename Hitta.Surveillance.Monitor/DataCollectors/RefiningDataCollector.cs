using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;

namespace Hitta.Surveillance.Monitor.DataCollectors
{
    public class RefiningDataCollector:DataCollectorAdapterBase
    {
        readonly int _scheduleId;
        bool _initialized;
        readonly string _oracleConnectionString;
        OracleConnection _connection;

        public RefiningDataCollector(int scheduleId, string displayName, string description, int interval) : base(displayName, description, interval)
        {
            _scheduleId = scheduleId;
            _oracleConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public override int MeasuredValue
        {
            get { return GetStatus(); }
        }

        bool IsRunning()
        {
            using(OracleCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT ID FROM JOBS.VRUNNINGJOBS WHERE SCHEDULEID = :id";
                command.CommandTimeout = 3;
                command.CommandType = CommandType.Text;

                var parameter = new OracleParameter("id", OracleType.Number) {Value = _scheduleId};
                command.Parameters.Add(parameter);

                var data = command.ExecuteScalar();

                return data != null;
            }
        }

        public int GetStatus()
        {
            try
            {
                _connection.Open();

                if (IsRunning()) return 4;

                var command = _connection.CreateCommand();
                command.CommandText = "SELECT * FROM JOBS.TBSCHEDULES WHERE SCHEDULEID = :id";
                command.CommandTimeout = 3;
                command.CommandType = CommandType.Text;
                
                var parameter = new OracleParameter("id", OracleType.Number) { Value = _scheduleId };
                command.Parameters.Add(parameter);

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.HasRows) return 0;

                        var crasched = DataReader.GetData<DateTime>(reader, reader.GetDateTime, "CRASHED");
                        if (crasched != DateTime.MinValue)
                            return 3;


                        var nextRunDate = DataReader.GetData<DateTime>(reader, reader.GetDateTime, "NEXT");
                        if (nextRunDate < DateTime.Now)
                            return 2;
                    }
                }
            }
            catch
            {
                return -1;
            }
            finally
            {
                _connection.Close();
            }

            return 1;
        }

        public override void InitializeAdapter()
        {
            if(!_initialized)
            {
                _connection = new OracleConnection(_oracleConnectionString);
            }
            _initialized = true;
        }

        public override bool Initialized
        {
            get { return _initialized; }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if(disposing)
                {
                    if(_connection != null)
                        _connection.Dispose();
                }
            }
            finally 
            {
                base.Dispose(disposing);
            }
            
        }

        class DataReader
        {
            public delegate T DataFetcher<T>(int id);

            public static T GetData<T>(IDataRecord record, DataFetcher<T> fetcher, string fieldName)
            {
                return GetData(record, fetcher, fieldName, default(T));
            }

            private static T GetData<T>(IDataRecord record, DataFetcher<T> fetcher, string fieldName, T defaultValue)
            {
                int ordinal = record.GetOrdinal(fieldName);

                if (record.IsDBNull(ordinal))
                {
                    return defaultValue;
                }

                return fetcher(ordinal);
            }
        }
    }

}
