using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ManageHrAdminPermissionDetailsPresenter : Presenter<IManageHrAdminPermissionDetailsView>
    {

        public Boolean ValidateCBIUniqueIds(String CBIUniqueIDs)
        {
            List<String> lstCBIUniqueID = SplitBulkData(CBIUniqueIDs);

            if (lstCBIUniqueID.Any())
            {
                View.ErrorMessage = SecurityManager.ValidateCBIUniqueIDs(lstCBIUniqueID);
            }
            else
            {
                View.ErrorMessage = "Please provide atleast one CBI UniqueID";
            }
            

            return View.ErrorMessage.IsNullOrEmpty();
        }

        private List<string> SplitBulkData(string bulkData)
        {
             return bulkData.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.ToUpperInvariant().Trim()).Distinct().ToList();
        }

        public Boolean ValidateAccountNames(String accountNames)
        {
            var lstaccountNames = SplitBulkData(accountNames);

            if (lstaccountNames.Any())
            {
                View.ErrorMessage = SecurityManager.ValidateAccountNames(lstaccountNames);
            }
            else
            {
                View.ErrorMessage = "Please provide atleast one Account Name";
            }

            return View.ErrorMessage.IsNullOrEmpty();
        }

        public Boolean AssignCBIUniqueIDs()
        {
            List<String> lstCBIUniqueID = SplitBulkData(View.CBIUniqueIds);
            var lstCBIUniqueIDToSave = FingerPrintSetUpManager.FilterCBIUniqueIdsToSave(View.SelectedOrganizationUserID, lstCBIUniqueID);
            if (lstCBIUniqueIDToSave.Any())
            {
                return FingerPrintSetUpManager.AssignCBIUniqueIds(View.CurrentLoggedInUserID, View.SelectedOrganizationUserID, lstCBIUniqueIDToSave);
            }
            else
            {
                View.ErrorMessage = "Entered CBI UniqueID(s) are already added.";
                return false;
            }
        }

        public Boolean AssignAccountName()
        {
            var lstaccountNames = SplitBulkData(View.AccountNames);
            var lstaccountNamesToSave = FingerPrintSetUpManager.FilterAccountNamesToSave(View.SelectedOrganizationUserID, lstaccountNames);
            if (lstaccountNamesToSave.Any())
            {
                return FingerPrintSetUpManager.AssignAccountNames(View.CurrentLoggedInUserID, View.SelectedOrganizationUserID, lstaccountNamesToSave);
            }
            else
            {
                View.ErrorMessage = "Entered Account Name(s) are already added.";
                return false;
            }
            
        }

        public Boolean UnAssignPermission()
        {
            return FingerPrintSetUpManager.UnAssignHRAdminPermission(View.CurrentLoggedInUserID, View.SelectedPermissionId);
        }

        public void GetHrAdminPermission()
        {
            List<Entity.LocationEntity.UserCABSPermissionMapping> lstPermission = FingerPrintSetUpManager.GetHRAdminPermissions(View.SelectedOrganizationUserID);
            View.lstCBIUniqueIDs = lstPermission.Where(cond => cond.lkpCABSPermissionType.LCPT_Code == "AAAA").ToList();
            View.lstAccountNames = lstPermission.Where(cond => cond.lkpCABSPermissionType.LCPT_Code == "AAAB").ToList(); 
        }

    }
}
