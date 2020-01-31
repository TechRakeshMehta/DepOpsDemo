using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class SetupCompliancePackagesPresenter : Presenter<ISetupCompliancePackagesView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
        }

        public void GetCompliancePackages()
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            View.CompliancePackages = ComplianceSetupManager.GetCompliancePackage(View.SelectedTenantId, getTenantName);
        }

        public List<lkpCompliancePackageType> GetCompliancePackageTypes()
        {
            return ComplianceSetupManager.GetCompliancePackageTypes(View.SelectedTenantId);
        }


        public void UpdatePackageDetail()
        {
            if (ComplianceSetupManager.CheckIfPackageNameAlreadyExist(View.ViewContract.PackageName, View.ViewContract.CompliancePackageId, View.SelectedTenantId))
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
                packageDetail.TenantID = View.SelectedTenantId;
                packageDetail.IsActive = View.ViewContract.State;
                packageDetail.PackageDetail = View.ViewContract.PackageDetail;
                packageDetail.IsViewDetailsInOrderEnabled = View.ViewContract.ViewDetails;
                packageDetail.CompliancePackageTypeID = View.ViewContract.CompliancePackageTypeID;
                packageDetail.ChecklistURL = View.ViewContract.ChecklistDocumentURL; //UAT 1337
                packageDetail.NotesDisplayPositionId = View.ViewContract.NotesDisplayPositionId;
                //Dictionary notesDetail will contain Content type as keys and content value as values. 
                Dictionary<String, String> notesDetail = new Dictionary<String, String>();
                notesDetail.Add(LCContentType.ExplanatoryNotes.GetStringValue(), View.ViewContract.ExplanatoryNotes);
                notesDetail.Add(LCContentType.ExceptionDescription.GetStringValue(), View.ViewContract.ExceptionDescription);

                ComplianceSetupManager.UpdateCompliancePackageDetail(packageDetail, View.CurrentLoggedInUserId, notesDetail);

                //CompliancePackage package = ComplianceSetupManager.SaveCompliancePackageDetail(packageDetail, View.CurrentLoggedInUserId, notesDetail);

                //CompliancePackage package = ComplianceSetupManager.SaveCompliancePackageDetail(packageDetail, View.CurrentLoggedInUserId, View.TenantId);

                //LargeContent exceptionContent = new LargeContent
                //{
                //    LC_ObjectID = package.CompliancePackageID,
                //    LC_Content = View.ViewContract.ExceptionDescription,
                //    LC_IsDeleted = false
                //};
                //ComplianceSetupManager.SaveLargeContentRecord(exceptionContent, LCObjectType.CompliancePackage.GetStringValue(), LCContentType.ExceptionDescription.GetStringValue(), View.CurrentLoggedInUserId, View.TenantId);
                //LargeContent notesContent = new LargeContent
                //{
                //LC_ObjectID = package.CompliancePackageID,
                //LC_Content = View.ViewContract.ExplanatoryNotes,
                //LC_IsDeleted = false
                //};
                //ComplianceSetupManager.SaveLargeContentRecord(notesContent, LCObjectType.CompliancePackage.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.CurrentLoggedInUserId, View.TenantId);
            }
        }

        /// <summary>
        /// Add a package in selected teanant database.
        /// </summary>
        public void SavePackagedetail()
        {
            if (ComplianceSetupManager.CheckIfPackageNameAlreadyExist(View.ViewContract.PackageName, View.ViewContract.CompliancePackageId, View.SelectedTenantId))
            {
                View.ErrorMessage = "Package Name can not be duplicate.";
            }
            else
            {
                View.ErrorMessage = String.Empty;
                CompliancePackage newPackage = new CompliancePackage
                {
                    PackageName = View.ViewContract.PackageName,
                    Description = View.ViewContract.Description,
                    PackageLabel = View.ViewContract.PackageLabel,
                    ScreenLabel = View.ViewContract.ScreenLabel,
                    TenantID = View.SelectedTenantId,
                    IsActive = View.ViewContract.State,
                    IsViewDetailsInOrderEnabled =  View.ViewContract.ViewDetails,
                    PackageDetail = View.ViewContract.PackageDetail,
                    IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false,
                    CompliancePackageTypeID = View.ViewContract.CompliancePackageTypeID,
                    ChecklistURL = View.ViewContract.ChecklistDocumentURL,  //UAT 1337
                    NotesDisplayPositionId = View.ViewContract.NotesDisplayPositionId
                };

                //Dictionary notesDetail will contain Content type as keys and content value as values. 
                Dictionary<String, String> notesDetail = new Dictionary<String, String>();
                notesDetail.Add(LCContentType.ExplanatoryNotes.GetStringValue(), View.ViewContract.ExplanatoryNotes);
                notesDetail.Add(LCContentType.ExceptionDescription.GetStringValue(), View.ViewContract.ExceptionDescription);

                ComplianceSetupManager.SaveCompliancePackageDetail(newPackage, View.CurrentLoggedInUserId, notesDetail);
            }
        }

        /// <summary>
        /// To delete Packages
        /// </summary>
        /// <returns></returns>
        public Boolean DeletePackage()
        {
            IntegrityCheckResponse response = IntegrityManager.IfPackageCanBeDeleted(View.ViewContract.CompliancePackageId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                CompliancePackage cmpPackage = ComplianceSetupManager.GetCurrentPackageInfo(View.ViewContract.CompliancePackageId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpPackage.PackageName);
                return false;
            }
            else
            {
                return ComplianceSetupManager.DeleteCompliancePackage(View.ViewContract.CompliancePackageId, View.CurrentLoggedInUserId, View.SelectedTenantId);
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

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        /// <summary>
        /// Check if package can be updated or not
        /// </summary>
        /// <returns></returns>
        public Boolean IfPackageCanBeupdated()
        {
            IntegrityCheckResponse response = IntegrityManager.IfPackageCanBeUpdated(View.ViewContract.CompliancePackageId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                CompliancePackage cmpPackage = ComplianceSetupManager.GetCurrentPackageInfo(View.ViewContract.CompliancePackageId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpPackage.PackageName);
                return false;
            }
            return true;
        }

        public void GetPackageNotesPosition(String selectedPositionCode)
        {
            View.NotesPositionId = ComplianceSetupManager.GetPackageNotesPosition(View.SelectedTenantId, selectedPositionCode).PNP_ID;
        }
    }
}




