using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System.Linq;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class SetupComplianceAttributesPresenter : Presenter<ISetupComplianceAttributesView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
        }

        public List<ComplianceAttribute> GetComplianceItemAttributes()
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            return ComplianceSetupManager.GetComplianceAttributes(View.SelectedTenantId, getTenantName);
        }

        public ComplianceAttribute GetComplianceItemAttribute(Int32 complianceItemAttributeID)
        {
            return ComplianceSetupManager.GetComplianceAttribute(complianceItemAttributeID, View.SelectedTenantId);
        }

        public List<lkpComplianceAttributeDatatype> GetComplianceAttributeDatatype()
        {
            return ComplianceSetupManager.GetComplianceAttributeDatatype(View.SelectedTenantId);
        }

        public List<lkpComplianceAttributeType> GetComplianceAttributeType()
        {
            return ComplianceSetupManager.GetComplianceAttributeType(View.SelectedTenantId);
        }

        /// <summary>
        /// Get the ID of the Screening DocumentType.
        /// </summary>
        /// <returns></returns>
        public Int32 GetScreeningDocumentAttributeDataTypeId()
        {
            var _lstAttributeDataTypes = LookupManager.GetLookUpData<lkpComplianceAttributeDatatype>(View.SelectedTenantId);
            return _lstAttributeDataTypes.First(adt => adt.Code == ComplianceAttributeDatatypes.Screening_Document.GetStringValue()).ComplianceAttributeDatatypeID;
        }

        /// <summary>
        /// To get compliance attribute group list
        /// </summary>
        /// <returns></returns>
        public List<ComplianceAttributeGroup> GetComplianceAttributeGroup()
        {
            var complianceAttributeGroupList = ComplianceSetupManager.GetComplianceAttributeGroup(View.SelectedTenantId);
            complianceAttributeGroupList.Insert(0, new ComplianceAttributeGroup { CAG_ID = 0, CAG_Name = "--SELECT--" });
            return complianceAttributeGroupList;
        }

        public Boolean AddComplianceAttribute(ComplianceAttributeContract complianceAttributeContract)
        {
            ComplianceAttribute complianceAttribute = complianceAttributeContract.TranslateToEntity();
            ComplianceSetupManager.AddComplianceAttribute(complianceAttribute, complianceAttributeContract.ExplanatoryNotes);
            return true;
        }

        public Boolean UpdateComplianceAttribute(ComplianceAttributeContract complianceAttributeContract)
        {
            ComplianceAttribute complianceAttribute = complianceAttributeContract.TranslateToEntity();
            ComplianceSetupManager.UpdateComplianceAttribute(complianceAttribute, complianceAttributeContract.ExplanatoryNotes);
            //    )
            //{
            //    LargeContent notesContent = new LargeContent
            //    {
            //        LC_ObjectID = complianceAttribute.ComplianceAttributeID,
            //        LC_Content = complianceAttributeContract.ExplanatoryNotes,
            //        LC_IsDeleted = false
            //    };
            //    ComplianceSetupManager.SaveLargeContentRecord(notesContent, LCObjectType.ComplianceATR.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.currentLoggedInUserId, View.tenantId);
            //    return true;
            //}
            if (complianceAttribute.ComplianceAttributeDatatypeID == Convert.ToInt32(ComplianceAttributeDatatypes.Options))
                CallParallelTaskForUniversalAttributeMapping(complianceAttribute.ComplianceAttributeID);
            return true;
        }
        #region UAT-3563
        /// <summary>
        /// Call Parallel Task for Pkg Status Changed Mail
        /// </summary>
        /// <param name="pkgStatusCode"></param>
        public void CallParallelTaskForUniversalAttributeMapping(Int32 complianceAttributeID)
        {
            if (complianceAttributeID > AppConsts.NONE)
            {
                Dictionary<String, Object> mailData = new Dictionary<String, Object>();
                mailData.Add("tenantID", View.SelectedTenantId);
                mailData.Add("complianceAttributeID", complianceAttributeID.ToString());
                mailData.Add("currentLoggedInUserId", View.currentLoggedInUserId.ToString());
                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                ParallelTaskContext.PerformParallelTask(UniversalAttributeMapping, mailData, LoggerService, ExceptiomService);
            }
        }

        /// <summary>
        /// Send Notification on Rotation Pkg Status Changed from Comp to NC
        /// </summary>
        /// <param name="data"></param>
        public void UniversalAttributeMapping(Dictionary<String, Object> data)
        {
            Int32 tenantID = Convert.ToInt32(data.GetValue("tenantID"));
            Int32 complianceAttributeID = Convert.ToInt32(data.GetValue("complianceAttributeID"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("currentLoggedInUserId"));
            ComplianceDataManager.UniversalAttributeMapping(tenantID, complianceAttributeID, currentLoggedInUserId);
        }
        #endregion
        public Boolean DeleteComplianceAttribute(Int32 complianceAttributeID, Int32 currentUserID)
        {
            IntegrityCheckResponse response = IntegrityManager.IfAttributeCanBeDeleted(complianceAttributeID, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                ComplianceAttribute cmpAttribute = ComplianceSetupManager.GetComplianceAttribute(complianceAttributeID, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpAttribute.Name);
                return false;
            }
            else
            {
                return ComplianceSetupManager.DeleteComplianceAttribute(complianceAttributeID, currentUserID, View.SelectedTenantId);
            }
        }

        public String GetLargeContent(Int32 complianceAttributeID)
        {
            LargeContent notesRecord = ComplianceSetupManager.getLargeContentRecord(complianceAttributeID, LCObjectType.ComplianceATR.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.SelectedTenantId);
            if (notesRecord != null)
                return notesRecord.LC_Content;
            else
                return String.Empty;
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.currentLoggedInUserId).Organization.TenantID.Value;
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
        public Boolean IfAttributeCanBeupdated(Int32 complianceAttributeID)
        {
            IntegrityCheckResponse response = IntegrityManager.IfAttributeCanBeUpdated(complianceAttributeID, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                ComplianceAttribute cmpAttribute = ComplianceSetupManager.GetComplianceAttribute(complianceAttributeID, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpAttribute.Name);
                return false;
            }
            return true;
        }

        public Boolean checkIfMappingIsDefinedForAttribute(Int32 complianceAttributeID)
        {
            return MobilityManager.checkIfMappingIsDefinedForAttribute(complianceAttributeID, View.SelectedTenantId);
        }

        public List<ClientSystemDocument> GetComplianceViewDocumentSysDocs()
        {
            List<ClientSystemDocument> lstComplianceViewDocumentSysDocs = ComplianceSetupManager.GetComplianceViewDocumentSysDocs(View.SelectedTenantId);
            if (lstComplianceViewDocumentSysDocs.IsNull())
            {
                lstComplianceViewDocumentSysDocs = new List<ClientSystemDocument>();
            }
            lstComplianceViewDocumentSysDocs.Insert(0, new ClientSystemDocument { CSD_ID = 0, CSD_FileName = "--SELECT--" });
            return lstComplianceViewDocumentSysDocs;
        }

        #region UAT-4558
        public List<Entity.SystemDocument> GetFileUploadAdditionalDocs()//(lkpComplianceAttributeDatatype attributeDatatype)
        {
            //String docTypeCode = String.Empty;
            List<Entity.SystemDocument> lstComplianceFileUploadDocumentSysDocs = new List<Entity.SystemDocument>();

            List<String> lstSatusCodeToFetch = new List<String>();
            List<Int32> lstDocStatusIDs = new List<Int32>();


            //  if (!attributeDatatype.IsNullOrEmpty() && !String.IsNullOrEmpty(attributeDatatype.Code))
            //   {
            //if (String.Compare(attributeDatatype.Code.ToLower(), ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower()) == 1)
            // {
            lstSatusCodeToFetch.Add(DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue());
            //Fetch only Additional Documents
            lstDocStatusIDs = SecurityManager.GetDocumentTypes().Where(x => lstSatusCodeToFetch.Contains(x.DT_Code) && x.DT_IsActive).Select(cond => cond.DT_ID).ToList();
            // }
            //   }

            if (!lstDocStatusIDs.IsNullOrEmpty() && lstDocStatusIDs.Count > AppConsts.NONE)
                lstComplianceFileUploadDocumentSysDocs = BackgroundSetupManager.GetBothUploadedDisclosureDocuments(lstDocStatusIDs).Where(x => !x.IsOperational.HasValue || (x.IsOperational.HasValue && x.IsOperational == false)).ToList();
            //lstComplianceFileUploadDocumentSysDocs.Insert(0, new Entity.SystemDocument { SystemDocumentID = 0, FileName = "--SELECT--" });
            return lstComplianceFileUploadDocumentSysDocs;
        }

        #endregion

    }
}




