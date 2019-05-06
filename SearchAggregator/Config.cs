using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace SearchAggregator
{
    [DataContract]
    class Config
    {
        [DataMember]
        public String key { get; set; }
        [DataMember]
        public String cx { get; set; }
        [DataMember]
        public String DBConnectionString { get; set; }
    }
}
