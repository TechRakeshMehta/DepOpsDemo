using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchySettingView
    {
        IAgencyHierarchySettingView CurrentViewContext { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 AgencyHierarchyID { get; set; }

        Boolean IsAgencyHierachySettingExisted { get; set; }

        Boolean IsRootNode { get; set; }

        AgencyHierarchySettingContract AgencyHierarchySettingContract { get; set; }

        #region UAT-3950
        AgencyHierarchySettingContract AutoArchivedRotationSettingContract { get; set; }

        Boolean IsAutoArchivedRotationSettingExisted { get; set; }

        //Start UAT-4673
        Boolean IsUpdateReviewStatusSettingExisted { get; set; } 

        AgencyHierarchySettingContract UpdateReviewStatusSettingContract { get; set; }
        //End UAT-4673
        #endregion

        #region UAT-3662

        AgencyHierarchySettingContract InstPrecepMandateIndividualShareContract { get; set; }
        Int32 SelectedRootNodeID { get; set; }
        #endregion
    }
}
