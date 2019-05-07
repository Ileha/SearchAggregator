using System;
using System.Runtime.Serialization;

namespace SearchAggregator.Search.google
{
    [DataContract]
    class JsonData
    {
        [DataMember]
        public item[] items { get; set; }
    }

    [DataContract]
    class item {
        [DataMember]
        public String title { get; set; }
        [DataMember]
        public String link { get; set; }
    }
}
