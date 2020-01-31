using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace INTSOF.ServiceUtil
{
    public class ServiceContextData
    {
        private Dictionary<String, ObjectContext> _dbContextDict = new Dictionary<String, ObjectContext>();
        private Dictionary<String, Object> _dataDict = new Dictionary<String, Object>();
        public Dictionary<String, ObjectContext> DBContexts
        {
            get
            {
                return _dbContextDict;
            }
        }

        public Dictionary<String, Object> DataDict
        {
            get;
            set;            
        }
    }
}
