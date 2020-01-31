using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class ServiceLevelDetailsForOrderContract
    {
        public Int32 PackageID
        {
            get;
            set;
        }

        public String PackageName
        {
            get;
            set;
        }

        public Int32 ServiceGroupID
        {
            get;
            set;
        }

        public String ServiceGroupName
        {
            get;
            set;
        }

        public String ServiceGroupCompletionDate
        {
            get;
            set;
        }

        public String ServiceGroupStatus
        {
            get;
            set;
        }

        public Boolean IsServiceGroupStatusComplete
        {
            get;
            set;
        }

        public Int32 ServiceID
        {
            get;
            set;
        }

        public String ServiceName
        {
            get;
            set;
        }

        public String ServiceTypeCode
        {
            get;
            set;
        }

        public String ServiceStatus
        {
            get;
            set;
        }

        public String ServiceForms
        {
            get;
            set;
        }

        public Int32 PackageServiceGroupID
        {
            get;
            set;
        }

        public Boolean IsServiceFlagged
        {
            get;
            set;
        }

        public Boolean IsServiceCompleted
        {
            get;
            set;
        }
    }
}

