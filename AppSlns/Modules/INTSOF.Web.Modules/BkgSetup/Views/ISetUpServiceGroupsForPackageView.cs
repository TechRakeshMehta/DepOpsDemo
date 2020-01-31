using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface ISetUpServiceGroupsForPackageView
    {
        List<BkgSvcGroup> lstSvcGroup { get; set; }
        Int32 tenantId { get; set; }
        String ErrorMessage { get; set; }
        String NodeId { get; set; }
        Int32 packageId { get; }
        Int32 SelectedAttrGrp { get; set; }
        String InstructionText { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        //UAT-823
        List<Entity.State> lstState { get; set; }
        List<BkgPackageStateSearchContract> lstStateSearchContract { get; set; }
        List<Entity.ClientEntity.BkgPkgStateSearch> lstBkgPkgStateSearch { get; set; }
        Int32 NotesPositionId { get; set; }

        #region Package DropDown :UAT-2388
        List<BackgroundPackage> lstBackgroundPackage
        {
            set;
            get;
        }

        List<Int32> SelectedBkgPackageIdList
        {
            get;
            set;
        }
        Boolean isAutomaticPackageInvitationActive { get; set; }
        #endregion

        #region UAT-3268
        Boolean IsReqToQualifyInRotation { get; set; }
        #endregion
        String Passcode { get; set; }
    }
}
