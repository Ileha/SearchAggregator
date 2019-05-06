using SearchAggregator.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SearchAggregator.Search.google
{
    public class GoogleSearch : ISearch
    {
        public static readonly DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(JsonData));
        public const string URL = "https://www.googleapis.com/customsearch/v1?key={0}&cx={1}&q={2}";

        public Query search(string query)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(URL, Program.config.key, Program.config.cx, query));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            JsonData responseData;
            using (Stream dataStream = response.GetResponseStream())
            {
                responseData = jsonFormatter.ReadObject(dataStream) as JsonData;
            }

            response.Close();

            //for loading from disk
            //JsonData responseData;
            //using (FileStream fs = new FileStream("./data.txt", FileMode.Open))
            //{
            //    responseData = jsonFormatter.ReadObject(fs) as JsonData;
            //}

            List<Link> result = new List<Link>(responseData.items.Length);

            for (int i = 0; i < responseData.items.Length; i++) {
                result.Add(new Link(responseData.items[i].title, responseData.items[i].link));
            }

            return new Query(query, result);
        }
    }
}
