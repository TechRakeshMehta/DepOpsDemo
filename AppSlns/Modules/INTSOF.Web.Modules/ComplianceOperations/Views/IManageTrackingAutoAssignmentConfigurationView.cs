using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageTrackingAutoAssignmentConfigurationView
    {
        List<TrackingAssignmentConfigurationContract> lstAdminsConfiguration { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        //Boolean? IsSuccessfullySaved { get; set; }
        //UAT-3075
        List<CompliancePriorityObjectContract> lstObjects { get; set; }
        List<TrackingConfigObjectMappingContract> lstTrackConfigObjectsMapped { get; set; }
        Int32 selectedTrackConfigID { get; set; }
    }
}
