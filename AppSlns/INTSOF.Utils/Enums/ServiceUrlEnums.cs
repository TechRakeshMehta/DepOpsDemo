using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.Utils.Enums
{
    /// <summary>
    /// Repsresents the Enums of the Service Key
    /// </summary>
    public enum ServiceUrlEnum
    {
        [StringValue("ClinicalRotationSvcUrl")]
        ClinicalRotationSvcUrl,

        [StringValue("RequirementPackageSvcUrl")]
        RequirementPackageSvcUrl,

        [StringValue("ClientContactSvcUrl")]
        ClientContactSvcUrl,

        [StringValue("ApplicantClinicalRotationSvcUrl")]
        ApplicantClinicalRotationSvcUrl,

        [StringValue("AgencyHierarchySvcUrl")]
        AgencyHierarchySvcUrl
    }

    public struct ServiceAppConstants
    {
        public const String WCFSERVICE_TIMEOUT_KEY = "WCFServiceTimeOut";
    }
}
