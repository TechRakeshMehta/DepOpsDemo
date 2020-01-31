using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface ITrackingAutoAssignmentConfigurationDetailView
    {
        List<Entity.OrganizationUser> lstOrganizationUser { get; set; }
        List<Int32> lstSelectedAdminsIds { get; }
        Int32 CurrentLoggedInUserId { get; }
        //UAT-3075
        List<CompliancePriorityObjectContract> lstObjects { get; set; }
        List<CompliancePriorityObjectContract> lstOldSelectedObjects { get; set; }
    }
}
