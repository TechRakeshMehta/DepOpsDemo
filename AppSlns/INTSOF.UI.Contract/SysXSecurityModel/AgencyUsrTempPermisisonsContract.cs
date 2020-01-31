using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
   public class AgencyUsrTempPermisisonsContract
    {
       public Boolean AGU_RotationPackagePermission { get; set; }
       public Boolean AGU_RotationPackageViewPermission { get; set; }

       public Boolean AGU_AllowJobPosting { get; set; }
       public Boolean AGU_AgencyUserPermission { get; set; }

       public Boolean AGU_AttestationReport { get; set; }
       public Boolean AGU_AgencyApplicantStatus { get; set; }
       //public Boolean AgencyUsrAgencyApplicantStatus { get; set; }

    }
}
