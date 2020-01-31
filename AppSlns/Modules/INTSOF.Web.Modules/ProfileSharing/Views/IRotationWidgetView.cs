using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IRotationWidgetView
    {
        Int32 InviteeOrgUserID { get; set; }

        Guid UserId
        {
            get;
        }

        Int32 CurrentUserId
        {
            get;
        }

        List<InstitutionProfileContract> LstInstitutionProfileContract
        {
            get;
            set;
        }

        List<DashBoardRotationDataContract> lstDashboardRotations
        {
            get;
            set;
        }

        AgencyUserDashboardDetailsContract SharedUserDetails
        {
            get;
            set;
        }

        Boolean IsInstructorPreceptor
        {
            get;
            set;
        }

        DateTime? FromDate { get; set; }

        DateTime? ToDate { get; set; }

        Boolean HideDetailLink { get; }//UAT-3220

        //UAT-3628
        String FileName_UserGuide { get; set; }

        String DocumentPath_UserGuide { get; set; }

        Boolean ReloadAllData { get; set; }
    }
}
