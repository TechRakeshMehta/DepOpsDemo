using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class AgencyUserPermissionTemplateMappingContract
    {
        public Int32 AGUPT_ID { get; set; }
        public Int32 AGUPTM_PermissionTypeID { get; set; }
        public Int32 AGUPTM_PermissionAccessTypeID { get; set; }
        public String AUPT_Name { get; set; }
        public String AUPT_Code { get; set; }

    }

    [Serializable]
    public class AgencyUserPermissionTemplateNotificationsContract
    {
        public Int32 AGUPT_ID { get; set; }
        public String AUN_Code { get; set; }
        public String AUN_Name { get; set; }
        public Boolean AGUPTNM_IsMailToBeSend { get; set; }
    }

}
