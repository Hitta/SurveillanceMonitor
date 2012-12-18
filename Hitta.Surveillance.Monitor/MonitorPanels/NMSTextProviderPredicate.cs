using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Apache.NMS;

namespace Hitta.Surveillance.Monitor.MonitorPanels
{
    public interface NMSTextProviderPredicate
    {
        bool IsMatch(ITextMessage textMessage);
        bool IsStandardSearch(ITextMessage textMessage, string where);
        bool IsGeoSearch(ITextMessage textMessage);
    }

    public class NMSTextProviderPredicateMobile : NMSTextProviderPredicate
    {
        public bool IsMatch(ITextMessage textMessage)
        {
           return IsMobile(textMessage);
        }

        public bool IsStandardSearch(ITextMessage textMessage, string where)
        {
            if (String.IsNullOrEmpty(where)) return false;

            var flow = textMessage.Properties.GetString("FLOW");
            return "cmpWhatWhere,prvWhatWhere,prdAd".Equals(flow) || "prvWhatWhere,cmpWhatWhere,prdAd".Equals(flow);
        }

        public bool IsGeoSearch(ITextMessage textMessage)
        {
            var flow = textMessage.Properties.GetString("FLOW");
            return "cmpNearby,prvNearby".Equals(flow) || "prvNearby,cmpNearby,prdAd".Equals(flow);
        }

        private static bool IsMobile(ITextMessage textMessage)
        {
            var origin = textMessage.Properties.GetString("ORIGIN");
            return origin.StartsWith("MOBILE") || origin.StartsWith("API");
        }
    }

    public class NMSTextProviderPredicateWeb : NMSTextProviderPredicate
    {
        public bool IsMatch(ITextMessage textMessage)
        {
            return textMessage.Properties.GetString("ORIGIN").StartsWith("WEB");
        }

        public bool IsStandardSearch(ITextMessage textMessage, string where)
        {
            if (String.IsNullOrEmpty(where)) return false;

            return "prvWhatWhere,cmpWhatWhere,prdAd,prdLinks".Equals(textMessage.Properties.GetString("FLOW"));
        }

        public bool IsGeoSearch(ITextMessage textMessage)
        {
            return false;
        }
    }
}
