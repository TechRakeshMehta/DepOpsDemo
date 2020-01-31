using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class RequiredDocumentationLoaderPresenter : Presenter<IRequiredDocumentationLoaderView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// Get Additional Documents from Procedure. 
        /// </summary>
        /// <param name="Country"></param>
        /// <param name="State"></param>
        public void GetAdditionalDocuments(List<Int32> BPAIds, Int32 selectedHierarchyId, Int32 TenantId, List<Int32> compPackageIds)
        {
            View.AdditionalDocuments = BackgroundSetupManager.GetAdditionalDocuments(BPAIds, compPackageIds, selectedHierarchyId, TenantId);
        }

        public void FetchAdditionalDocumentAttributes(ApplicantOrderCart applicantOrderCart)
        {
            BkgDataStore _bkgDataStore = new BkgDataStore();
            String inputXML = _bkgDataStore.ConvertApplicantDataIntoXML(applicantOrderCart, View.TenantID, true, true);
            /* Commented below code related to UAT-1744:
             * Shifted below code in common method [BackgroundSetupManager.FetchDandRAttributes]
            View.DictAttributeGroupIDs = StoredProcedureManagers.GetPricingDataDictionary(View.TenantID, inputXML);

            //MVR Fields
            if (!applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID > 0)
            {
                Int32 MVRDvrLicenseNumberID = applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID;
                String LicenceNumber = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.GetValue(MVRDvrLicenseNumberID);
                View.DictAttributeGroupIDs.Add(MVRDvrLicenseNumberID, LicenceNumber);
            }
            if (!applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID > 0)
            {
                Int32 MVRDvrLicenseNumberStateID = applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID;
                String StateName = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.GetValue(MVRDvrLicenseNumberStateID);
                View.DictAttributeGroupIDs.Add(MVRDvrLicenseNumberStateID, StateName);
            }
            // View.DocumentAttributeMappingList = BackgroundSetupManager.GetFieldNames(View.DictAttributeGroupIDs, View.DandRDocuments);
            List<SysDocumentFieldMappingContract> tempDocumentAttributeMappingList = BackgroundSetupManager.GetFieldNames(View.DictAttributeGroupIDs, View.AdditionalDocuments);
            if (tempDocumentAttributeMappingList.IsNotNull() && tempDocumentAttributeMappingList.Count > AppConsts.NONE)
            {
                tempDocumentAttributeMappingList.ForEach(cond =>
                {
                    if (cond.FieldName.ToLower() == AttributeMappingFieldName.DateofBirth.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                    if (cond.FieldName.ToLower() == AttributeMappingFieldName.DigitallySigned.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                    if (cond.FieldName.ToLower() == AttributeMappingFieldName.DateDigitallySigned.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                    if (cond.FieldName.ToLower() == AttributeMappingFieldName.DateSigned.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                    if (cond.FieldName.ToLower() == AttributeMappingFieldName.DatesAtCurrentResidency.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                });
            }*/
            View.DocumentAttributeMappingList = BackgroundSetupManager.FetchDandRAttributes(applicantOrderCart, View.TenantID, View.AdditionalDocuments, inputXML);
        }

        public byte[] FillAttributesInPdf(String documentPath, List<SysDocumentFieldMappingContract> tempMappings, Int32 TenantID, String fullName = null)
        {
            return BackgroundSetupManager.FillDataInPdfForm(documentPath, tempMappings, TenantID, fullName);
        }

        public ApplicantDocument SaveAditionalDocument(Int32 tenantId, String pdfDocPath, String filename, Int32 fileSize, String documentTypeCode, Int32 currentLoggedInUserId,
                                                       Int32 orgUserID, Boolean isSetDataEntryDocStatusToCompleted, Boolean isSearchableOnly)
        {
            return ComplianceSetupManager.SaveEsignedAdditionalDocumentAsPdf(tenantId, pdfDocPath, filename, fileSize, documentTypeCode, currentLoggedInUserId, orgUserID,
                                                                             isSetDataEntryDocStatusToCompleted, isSearchableOnly);
        }

        ///// <summary>
        ///// Method to check is any subscription purchased by applicant or not
        ///// </summary>
        ///// <param name="orgUserId"></param>
        ///// <param name="tenantId"></param>
        ///// <returns></returns>
        //public void IsSubscriptionExistForApplicant(Int32 orgUsrID)
        //{
        //    var subscriptions = ComplianceDataManager.GetSubscribedPackagesForUser(View.TenantID, orgUsrID);
        //    if (!subscriptions.IsNullOrEmpty())
        //    {
        //        View.IsSubscriptionExist = true;
        //    }
        //    else
        //    {
        //        View.IsSubscriptionExist = false;
        //    }
        //}

        #region UAT-3745

        public void GetAddtionalDocBkgSvcMapping(List<Int32> bkgPackagesIds, String additionalDocIds, Int32 tenantID)
        {
            View.lstServiceDocBkgSvcMapping = new List<SystemDocBkgSvcMapping>();
            View.lstServiceDocBkgSvcMapping = BackgroundSetupManager.GetAddtionalDocBkgSvcMapping(bkgPackagesIds, additionalDocIds, tenantID);
        }

        public Int16 GetlkpRecordTypeIdByCode(Int32 tenantId, String bkgSvcRecordTypeCode)
        {
            return BackgroundProcessOrderManager.GetlkpRecordTypeIdByCode(tenantId, bkgSvcRecordTypeCode);
        }
        #endregion
    }
}
