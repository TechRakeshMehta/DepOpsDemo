using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class BackgroundOrderDailyReport
    {
        /// <summary>
        /// OrganizationUserId's of the admins
        /// </summary>
        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        /// <summary>
        /// Time when the BackgroundServiceExecutionHistory was populated for genertaing the report.
        /// </summary>
        public DateTime CaptureDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Service Type code for which the report is being generated
        /// </summary>
        public String ServiceTypeCode
        {
            get;
            set;
        }

        public String EmailAddress
        {
            get;
            set;
        }

        public String UserName
        {
            get;
            set;
        }

        public DateTime? FromDate
        {
            get;
            set;
        }

        public Int32 ServiceExecutionHistoryId
        {
            get;
            set;
        }
    }
}
