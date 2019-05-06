using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAggregator.DataModel;

namespace SearchAggregator.Search
{
    public interface ISearch {
        Query search(string query);
    }
}
