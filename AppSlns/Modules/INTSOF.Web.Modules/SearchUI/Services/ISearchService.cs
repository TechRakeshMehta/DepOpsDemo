using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreWeb.Search.Services
{
    public interface ISearchService
    {
        IQueryable SearchByText(string SearchEntity, string SearchText, List<string> Fields, string orderby = "", bool exactmatch = false);
    }
}
