using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;

namespace DAL.Interfaces
{
    public interface ISearchRepository
    {
        /// <summary>
        /// Perform quick search.
        /// </summary>
        /// <param name="fieldNames">semi colon separated field names</param>
        /// <param name="searchKeys">space separate search key/keys depends on call.</param>
        /// <param name="exactMatch">Specifies if only the whole word or every single word should be searched.</param>
        /// <returns>list of <see cref="T"/></returns>
       IQueryable<T> QuickSearch<T>(List<String> fieldNames, String searchKeys, String orderByFieldName, Boolean exactMatch) where T : EntityObject;
    }
}
