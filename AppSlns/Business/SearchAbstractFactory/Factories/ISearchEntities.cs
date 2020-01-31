using System.Collections.Generic;
using System.Linq;

namespace Business.SearchAbstractFactory
{
    public interface ISearchEntities<TFactory>
    {
        IQueryable SearchByText(string SearchText, List<string> Fields, string orderby = "", bool exactmatch = false);
    }
}
