using SearchAggregator.DataModel;

namespace SearchAggregator.Search
{
    public interface ISearch {
        Query search(string query);
    }
}
