using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class BackroundServiceGroupContract
    {

        public Int32 OrderID
        {
            get;
            set;
        }

        public String OrderPaymentStatus
        {
            get;
            set;
        }
        public String OrderStatusType
        {
            get;
            set;
        }
      
        public Int32 ServiceGroupId
        {
            get;
            set;
        }
        public Int32 BkgOrderPackageSvcGroupID
        {
            get;
            set;
        }
        public String ServicreGroupName
        {
            get;
            set;
        }
        public Boolean IsServiceGroupFlagged
        {
            get;
            set;
        }

        public Boolean IsServiceGroupComplete
        {
            get;
            set;
        }
        public Boolean IsOperationSupportAutoCompleteServiceType
        {
            get;
            set;
        }
        public Int32 ServiceCount
        {
            get;
            set;
        }

        public Boolean IsServiceGroupStatusComplete
        {
            get;
            set;
        }

        public Boolean IsAllServiceNonReportable
        {
            get;
            set;
        }
        public Int32 BkgPackageSvcGroupId
        {
            get;
            set;
        }
    }
}
