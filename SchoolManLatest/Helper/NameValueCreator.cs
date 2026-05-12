using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace TrackTap.Helper
{
    public class NameValueCreator
    {
        public static SortedDictionary<string, string> SortNameValueCollection(NameValueCollection nvc)
        {
            SortedDictionary<string, string> sortedDict = new SortedDictionary<string, string>();
            foreach (String key in nvc.AllKeys)
                sortedDict.Add(key, nvc[key]);
            return sortedDict;
        }
    }
}