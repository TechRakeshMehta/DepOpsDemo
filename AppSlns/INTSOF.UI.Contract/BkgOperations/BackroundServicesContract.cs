using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class BackroundServicesContract
    {

        public Int32 BkgOrderPackageSvcGroupID
        {
            get;
            set;
        }
        public Int32 ServiceID
        {
            get;
            set;
        }

        public String ServicreName
        {
            get;
            set;
        }

        public Int32? ServiceFormStatusTypeID
        {
            get;
            set;
        }
        public Boolean? ServiceFlagged
        {
            get;
            set;
        }

        public Int32? OrderItemResultStatusID
        {
            get;
            set;
        }
        public String LastStatusChangeDate
        {
            get;
            set;
        }
        public String ServiceFormStatus
        {
            get;
            set;
        }
        public String ServiceTypeCode
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
