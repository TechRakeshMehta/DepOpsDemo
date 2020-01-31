using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class PackageListPresenter : Presenter<IPackageListView>
    {

        public override void OnViewLoaded()
        {
            //View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetCompliancePackages()
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            View.CompliancePackages = ComplianceSetupManager.GetCompliancePackage(View.SelectedTenantId, getTenantName);
        }

        /// <summary>
        /// To save Package detail
        /// </summary>
        public void SavePackagedetail()
        {
            if (ComplianceSetupManager.CheckIfPackageNameAlreadyExist(View.ViewContract.PackageName, View.ViewContract.CompliancePackageId,View.SelectedTenantId))
            {
                View.ErrorMessage = "Package already exists.";
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
                    IsViewDetailsInOrderEnabled = View.ViewContract.ViewDetails,
                    PackageDetail = View.ViewContract.PackageDetail,
                    CompliancePackageTypeID=View.ViewContract.CompliancePackageTypeID,
                    ChecklistURL =View.ViewContract.ChecklistDocumentURL,
                    NotesDisplayPositionId = View.NotesPositionId
                };

                //Dictionary notesDetail will contain Content type as keys and content value as values. 
                Dictionary<String, String> notesDetail = new Dictionary<String, String>();
                notesDetail.Add(LCContentType.ExplanatoryNotes.GetStringValue(), View.ViewContract.ExplanatoryNotes);
                notesDetail.Add(LCContentType.ExceptionDescription.GetStringValue(), View.ViewContract.ExceptionDescription);

                ComplianceSetupManager.SaveCompliancePackageDetail(newPackage, View.CurrentLoggedInUserId, notesDetail);
                View.CompliancePackageID = newPackage.CompliancePackageID;

                //ComplianceSetupManager.SaveCompliancePackageDetail(newPackage, View.CurrentLoggedInUserId, View.TenantId);
                // View.SuccessMessage = "Package Inserted successfully.";
            }
        }

        public void DeletePackage()
        {
            ComplianceSetupManager.DeleteCompliancePackage(View.ViewContract.CompliancePackageId, View.CurrentLoggedInUserId, View.SelectedTenantId);
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
    }
}




