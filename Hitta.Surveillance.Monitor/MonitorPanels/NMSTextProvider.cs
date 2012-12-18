using System;
using System.Web;
using System.Windows.Forms;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;

//ORIGIN = WEB|195.49.241.128||Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET4.0C; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET4.0E)|
//RECORDS = 4
//persons.total = 0
//FROM = 1
//companies.total = 1
//query.name = BwCompositeQuery [prvList,cmpList,CompositeProductsQuery [products,products]]
//log.options = 1
//upstream.request.time = 126
//query.formatting.time = 0
//PLACENAMES_METHOD = string
//pois.included = 0
//companies.included = 1
//SUGGEST = 0
//products.included = 0
//PRODUCTS = 40*
//FLOW = prvWhatWhere,cmpWhatWhere,prdAd,prdLinks
//WHAT = anticimex
//products.total = 0
//persons.included = 0
//WHERE = nässjö
//pois.total = 0
//PRODUCT_PLACENAMES = nässjö
//locations.included = 0
//locations.total = 0
//result.building.time = 10
//PRODUCTBEHAVIOUR = 1
//TO = 4
//Message: 
namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public class NmsTextProvider : TextProvider
    {
        private readonly string destination;
        private readonly string broker;
        private readonly NMSTextProviderPredicate nmsTextProviderPredicate;
        private Boolean disposed;
        private IMessageConsumer consumer;
        private IConnection connection;
        private ISession session;

        public NmsTextProvider(String destination, String broker, NMSTextProviderPredicate nmsTextProviderPredicate)
        {
            this.destination = destination;
            this.broker = broker;
            this.nmsTextProviderPredicate = nmsTextProviderPredicate;
        }

        private static UserAgentType GetUserAgentType(ITextMessage textMessage)
        {
            var origin = textMessage.Properties.GetString("ORIGIN");

            if (origin.StartsWith("MOBILE") || origin.StartsWith("WEB"))
            {
                if (origin.Contains("Chrome")) return UserAgentType.CHROME;
                if (origin.Contains("MSIE")) return UserAgentType.IE;
                if (origin.Contains("Firefox")) return UserAgentType.FIREFOX;
                if (origin.Contains("Opera")) return UserAgentType.OPERA;
                if (origin.Contains("Android")) return UserAgentType.SAFARI_ANDROID;
                if (origin.Contains("Safari")) return UserAgentType.SAFARI;

                return UserAgentType.NETSCAPE;
            }

            if (!origin.StartsWith("API")) return UserAgentType.NETSCAPE;

            if (origin.Contains("Android")) return UserAgentType.ANDROID;
            if (origin.Contains("iPhone")) return UserAgentType.IPHONE;


            return UserAgentType.NETSCAPE;
        }

        public event EventHandler<TextEventArgs> OnText;

        public void start()
        {
            var connectionFactory = new ConnectionFactory(broker, Environment.MachineName);
            connection = connectionFactory.CreateConnection();
            connection.Start();

            session = connection.CreateSession();

            var topic = new ActiveMQTopic(destination);

            consumer = session.CreateDurableConsumer(topic, "surveillance_app", null, false);

            consumer.Listener += (message =>
            {
                var textMessage = (ITextMessage)message;

                var what = HttpUtility.HtmlDecode(textMessage.Properties.GetString("WHAT"));
                var where = HttpUtility.HtmlDecode(textMessage.Properties.GetString("WHERE"));
                var companies = textMessage.Properties.GetInt("companies.total");

                if (String.IsNullOrEmpty(what)) return;
                if (companies == 0) return;

                if (nmsTextProviderPredicate.IsMatch(textMessage))
                {
                    var userAgentType = GetUserAgentType(textMessage);

                    if (nmsTextProviderPredicate.IsStandardSearch(textMessage, where))
                    {
                        if (OnText != null)
                        {
                            OnText(this, new TextEventArgs(what + " @ " + where, userAgentType, false));
                        }
                    }

                    if (nmsTextProviderPredicate.IsGeoSearch(textMessage))
                    {
                        if (OnText != null)
                        {
                            OnText(this, new TextEventArgs(what + " @", userAgentType, true));
                        }
                    }
                }
            });
        }

        public void Dispose()
        {
            if (disposed) return;

            if(consumer != null)
            {
                consumer.Close();
                consumer.Dispose();
            }

            if(session != null)
            {
                session.Close();
                session.Dispose();
            }

            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }

            disposed = true;
        }
    }
}
