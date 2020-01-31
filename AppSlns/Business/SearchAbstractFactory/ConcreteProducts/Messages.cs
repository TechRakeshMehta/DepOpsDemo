using Business.RepoManagers;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.SearchAbstractFactory
{
    public class Messages<TFactory> : ISearchEntities<Message>, ISearchEntities<NonMessage>
    {
        List<String> lstFields = new List<String>() { "Column1", "Column2" };

        public IQueryable SearchByText(string SearchText, List<string> Fields, string orderby = "", bool exactmatch = false)
        {
            return SearchManager.QuickSearch<vwMessage>(Fields, SearchText, orderby, exactmatch); 
        }
        
        public void ApplicantsSpecificOperation()
        {
            return;
        }

        
    }
}
