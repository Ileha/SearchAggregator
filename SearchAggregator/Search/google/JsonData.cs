using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
