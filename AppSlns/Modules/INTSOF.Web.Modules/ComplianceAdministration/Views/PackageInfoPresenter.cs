using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Linq;


namespace CoreWeb.ComplianceAdministration.Views
{
    public class PackageInfoPresenter : Presenter<IPackageInfoView>
    {

    
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void getPackageInfo()
        {
            View.CompliancePackage = ComplianceSetupManager.GetCurrentPackageInfo(View.CurrentPackageId,View.SelectedTenantId);            
        }

        public string GetHierarchyTextForBundle()
        {
           return ComplianceSetupManager.GetHierarchyTextForBundle(View.CurrentPackageId, View.SelectedTenantId);
        }

        public void GetPackageBundleNodeHierarchy()
        {
            var nodeHierarchy = ComplianceSetupManager.GetPackageBundleNodeHierarchy(View.CurrentPackageId, View.SelectedTenantId);            
            View.PackageBundleNodeHierarchy = String.Join(",", nodeHierarchy
                .Select(node => node.GetType()
                .GetProperty("DpmLabel")
                .GetValue(node, null)));
        }

        public void UpdatePackageDetail()
        {
            if (ComplianceSetupManager.CheckIfPackageNameAlreadyExist(View.ViewContract.PackageName, View.ViewContract.CompliancePackageId,View.SelectedTenantId))
            {
                View.ErrorMessage = "Package Name can not be duplicate.";
            }
            else
            {
                View.ErrorMessage = String.Empty;
                CompliancePackage packageDetail = new CompliancePackage();
                packageDetail.CompliancePackageID = View.ViewContract.CompliancePackageId;
                packageDetail.PackageName = View.ViewContract.PackageName;
                packageDetail.PackageLabel = View.ViewContract.PackageLabel;
                packageDetail.ScreenLabel = View.ViewContract.ScreenLabel;
                packageDetail.Description = View.ViewContract.Description;
                packageDetail.IsActive = View.ViewContract.State;
                packageDetail.IsViewDetailsInOrderEnabled = View.ViewContract.ViewDetails;
                packageDetail.TenantID = View.SelectedTenantId;
                packageDetail.PackageDetail = View.ViewContract.PackageDetail;
                packageDetail.CompliancePackageTypeID = View.ViewContract.CompliancePackageTypeID;
                packageDetail.ChecklistURL = View.ViewContract.ChecklistDocumentURL;
                packageDetail.NotesDisplayPositionId = View.ViewContract.NotesDisplayPositionId; //UAT-2219

                //Dictionary notesDetail will contain Content type as keys and content value as values. 
                Dictionary<String, String> notesDetail = new Dictionary<String, String>();
                notesDetail.Add(LCContentType.ExplanatoryNotes.GetStringValue(), View.ViewContract.ExplanatoryNotes);
                notesDetail.Add(LCContentType.ExceptionDescription.GetStringValue(), View.ViewContract.ExceptionDescription);

                ComplianceSetupManager.UpdateCompliancePackageDetail(packageDetail, View.CurrentLoggedInUserId, notesDetail);
                //View.SuccessMessage = "Package Updated successfully.";
            }
        }

        public void GetLargeContent()
        {
            LargeContent notesRecord = ComplianceSetupManager.getLargeContentRecord(View.ViewContract.CompliancePackageId, LCObjectType.CompliancePackage.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.SelectedTenantId);

            if (notesRecord != null)
                View.ViewContract.ExplanatoryNotes = notesRecord.LC_Content;

            LargeContent exceptionRecord = ComplianceSetupManager.getLargeContentRecord(View.ViewContract.CompliancePackageId, LCObjectType.CompliancePackage.GetStringValue(), LCContentType.ExceptionDescription.GetStringValue(), View.SelectedTenantId);

            if (exceptionRecord != null)
                View.ViewContract.ExceptionDescription = exceptionRecord.LC_Content;
        }

        public Int32 getTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }
        public List<lkpCompliancePackageType> GetCompliancePackageTypes()
        {
            return ComplianceSetupManager.GetCompliancePackageTypes(View.SelectedTenantId);
        }

        /// <summary>
        /// UAT-2219-Ability to put package detail notes either above or below the package name in order flow
        /// </summary>
        /// <param name="selectedPositionCode"></param>
        public void GetPackageNotesPosition(String selectedPositionCode)
        {
            View.NotesPositionId = ComplianceSetupManager.GetPackageNotesPosition(View.SelectedTenantId, selectedPositionCode).PNP_ID;
        }

        #region UAT-3716
        public Boolean AddApprovedPkgsToCopyDataQueue()
        {
            return UniversalMappingDataManager.AddApprovedPkgsToCopyDataQueue(View.SelectedTenantId, View.CurrentPackageId, View.CurrentLoggedInUserId);
        }

        #endregion

    }
}




