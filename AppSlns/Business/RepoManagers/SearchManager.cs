using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;


namespace Business.RepoManagers
{
    public static class SearchManager
    {
        public static IQueryable<T> QuickSearch<T>(List<String> fieldNames, String searchKeys, String orderByFieldName, Boolean exactMatch = false) where T : EntityObject
        {
            try
            {
                return new SearchRepository().QuickSearch<T>(fieldNames, searchKeys, orderByFieldName, exactMatch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
