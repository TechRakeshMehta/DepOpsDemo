using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
    public class ApplicantInsituteDataContract
    {
        public String Token { get; set; }
        public String UserID { get; set; }
        public String TagetInstURL { get; set; }
        public DateTime TokenCreatedTime { get; set; }
        public Int32 TenantID { get; set; }

        /// <summary>
        /// Property to identify if the Applicant has used incorrect url 
        /// to login into the application
        /// </summary>
        public Boolean IsIncorrectLogin { get; set; }

        public Boolean IsSharedUser { get; set; }

        #region UAT-1218
        public String UserTypeSwitchViewCode { get; set; }
        #endregion

        //UAT-1261
        public Int32? AdminOrgUserID { get; set; }
    }
}
