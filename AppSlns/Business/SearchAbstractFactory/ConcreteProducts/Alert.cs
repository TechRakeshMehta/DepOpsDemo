using Business.RepoManagers;
using Entity;
using System.Collections.Generic;
using System.Linq;

namespace Business.SearchAbstractFactory
{
    public class Alert<TFactory> : ISearchEntities<Message>
    {
        public void AlertSpecificOperation()
        {
            return;
        }
        public IQueryable SearchByText(string SearchText, List<string> Fields, string orderby = "", bool exactmatch = false)
        {
            return SearchManager.QuickSearch<vwMessage>(Fields, SearchText, orderby, exactmatch); 
        }
    }
}
