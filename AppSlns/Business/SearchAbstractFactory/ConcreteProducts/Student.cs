using Business.RepoManagers;
using Entity;
using System.Collections.Generic;
using System.Linq;

namespace Business.SearchAbstractFactory
{
    public class Student<TFactory> : ISearchEntities<NonMessage>
    {
         public IQueryable SearchByText(string SearchText, List<string> Fields, string orderby = "", bool exactmatch = false)
        {
            return SearchManager.QuickSearch<ADBMessage>(Fields, SearchText, orderby, exactmatch); 
        }
         public void StudentSpecificOperation()
        {
            return;
        }
    }
}
