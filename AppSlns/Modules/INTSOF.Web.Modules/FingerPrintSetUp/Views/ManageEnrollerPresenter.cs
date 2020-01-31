using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ManageEnrollerPresenter : Presenter<IManageEnrollerView>
    {
        //public void GetTenants()
        //{
        //    Boolean SortByName = true;
        //    String clientCode = TenantType.Institution.GetStringValue();
        // //   View.lstTenants = FingerPrintSetUpManager.GetTenants(SortByName, false, clientCode);
        //}

        public void GetEnrollerList(bool IsADBAdmin)
        {

            List<Entity.OrganizationUser> lstAllOrganizationUser = new List<Entity.OrganizationUser>();
            View.lstClientAdminUser = SecurityManager.GetOganisationUsersByTanentId(SecurityManager.DefaultTenantID, true, false, false, false, true).Select(x => new Entity.OrganizationUser
            {
                FirstName = x.FirstName + " " + x.LastName,
                OrganizationUserID = x.OrganizationUserID
            }).ToList();


            //if (!View.selectedTenantID.IsNullOrEmpty() && View.selectedTenantID > AppConsts.NONE)
            //{
            //    if (!IsADBAdmin)
            //    {
            //        View.lstClientAdminUser = SecurityManager.GetClientAdminUsersByTanentId(View.selectedTenantID).Where(cond => cond.IsActive == true
            //                                   && cond.IsDeleted == false).Select(x => new Entity.OrganizationUser
            //                                   {
            //                                       FirstName = x.FirstName + " " + x.LastName,
            //                                       OrganizationUserID = x.OrganizationUserID,
            //                                       PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email,
            //                                       UserID = x.UserID
            //                                   }).ToList();
            //    }
            //    //else
            //    //{
            //    //    View.lstClientAdminUser = SecurityManager.GetOganisationUsersByTanentId(View.selectedTenantID).Where(cond => cond.OrganizationID==1 && cond.IsActive == true && cond.IsDeleted == false  ).Select(x => new Entity.OrganizationUser
            //    //{
            //    //    FirstName = x.FirstName + " " + x.LastName,
            //    //    OrganizationUserID = x.OrganizationUserID
            //    //}).ToList();
            //    //}
            //}

        }


        public void GetEnrollerMappings()
        {
            View.lstEnrollerMappings = new List<ManageEnrollerMappingContract>();
            View.lstEnrollerMappings = FingerPrintSetUpManager.GetEnrollerMappings(View.GridCustomPaging, View.SelectedLocationId);
            if (View.lstEnrollerMappings.IsNullOrEmpty())
                View.VirtualRecordCount = AppConsts.NONE;
            else
                View.VirtualRecordCount = View.lstEnrollerMappings.FirstOrDefault().TotalCount;
        }

        public List<Int32> GetEnrollerMappedWithLocation()
        {
            return FingerPrintSetUpManager.GetEnrollerMappedWithLocation(View.SelectedLocationId);
        }
        public Boolean SaveEnrollerMapping(ManageEnrollerMappingContract enrollerPermissionContract)
        {
            return FingerPrintSetUpManager.SaveEnrollerMapping(enrollerPermissionContract, View.CurrentLoggedInUserID);
        }


        public Boolean DeleteEnrollerMapping(Int32 selectedEnrollerMappingID)
        {
            return FingerPrintSetUpManager.DeleteEnrollerMapping(View.CurrentLoggedInUserID, selectedEnrollerMappingID);
        }



    }
}
