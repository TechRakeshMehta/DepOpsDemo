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

namespace CoreWeb.BkgOperations.Views
{
    public class DisclosureAndReleasePresenter : Presenter<IDisclosureAndReleaseView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// Get D and R Documents from Procedure. 
        /// </summary>
        /// <param name="Country"></param>
        /// <param name="State"></param>
        public void GetDandRDocuments(List<string> Country, List<string> State, Int32 TenantId,String disclosureAgeGroup)
        {
            View.DandRDocuments = BackgroundSetupManager.GetDisclosureAndReleaseDocuments(Country, State, View.HierarchyNodeID, View.RegulatoryNodeIDs, View.BkgServiceIds, TenantId
                                                                                          ,disclosureAgeGroup);
        }

        /// <summary>
        /// Get list of services based on BPA_ID
        /// </summary>
        /// <param name="BkgPackages"></param>
        public void GetServicesList(List<Int32> BkgPackages)
        {
            StringBuilder sb = new StringBuilder();
            View.lstServiceIds = BackgroundSetupManager.GetServicesIds(View.TenantID, BkgPackages);
            if (View.lstServiceIds.IsNotNull())
            {
                foreach (var item in View.lstServiceIds)
                {
                    sb.Append(item);
                    sb.Append(",");
                }
            }

            View.BkgServiceIds = sb.ToString();
        }

        /// <summary>
        /// Get list of RegulatoryType based on HierarchyNodeID
        /// </summary>
        /// <param name="HierarchyNodeID"></param>
        public void GetRegulatoryTypeList(Int32? HierarchyNodeID)
        {
            StringBuilder sb = new StringBuilder();
            View.RegulatoryNodeType = BackgroundSetupManager.GetInstHierarchyRegulatoryEntityMappingDetails(View.TenantID, HierarchyNodeID.Value);
            if (View.RegulatoryNodeType.IsNotNull())
            {
                foreach (var item in View.RegulatoryNodeType)
                {
                    sb.Append(item.IHRE_RegulatoryEntityTypeID);
                    sb.Append(",");
                }
            }

            View.RegulatoryNodeIDs = sb.ToString();
        }


        public void FetchDandRAttributes(ApplicantOrderCart applicantOrderCart)
        {
            
            BkgDataStore _bkgDataStore = new BkgDataStore();
            String inputXML = _bkgDataStore.ConvertApplicantDataIntoXML(applicantOrderCart, View.TenantID, true);

            #region OLD Implementation 
            /*Commneted below code for UAT-1744
             * Shifted below code in common method [BackgroundSetupManager.FetchDandRAttributes]
            //View.DictAttributeGroupIDs = StoredProcedureManagers.GetPricingDataDictionary(View.TenantID, inputXML);

            ////MVR Fields
            //if (!applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID > 0)
            //{
            //    Int32 MVRDvrLicenseNumberID = applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID;
            //    String LicenceNumber = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.GetValue(MVRDvrLicenseNumberID);
            //    View.DictAttributeGroupIDs.Add(MVRDvrLicenseNumberID, LicenceNumber);
            //}
            //if (!applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID > 0)
            //{
            //    Int32 MVRDvrLicenseNumberStateID = applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID;
            //    String StateName = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.GetValue(MVRDvrLicenseNumberStateID);
            //    View.DictAttributeGroupIDs.Add(MVRDvrLicenseNumberStateID, StateName);
            //}

            ////UAT-1744:Forms filled out at the time of order should be able to pull in data from custom forms within the order.
            //if (!applicantOrderCart.lstApplicantOrder[0].IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty())
            //{
            //    View.DictAttributeGroupIDs = BackgroundSetupManager.GetDandAFormAttributeDataDictionary(View.DictAttributeGroupIDs,
            //                                                        applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData);
            //}

            //// View.DocumentAttributeMappingList = BackgroundSetupManager.GetFieldNames(View.DictAttributeGroupIDs, View.DandRDocuments);
            //List<SysDocumentFieldMappingContract> tempDocumentAttributeMappingList = BackgroundSetupManager.GetFieldNames(View.DictAttributeGroupIDs, View.DandRDocuments);
            //if (tempDocumentAttributeMappingList.IsNotNull() && tempDocumentAttributeMappingList.Count > AppConsts.NONE)
            //{
            //    tempDocumentAttributeMappingList.ForEach(cond =>
            //    {
            //        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DateofBirth.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
            //        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DigitallySigned.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
            //        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DateDigitallySigned.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
            //        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DateSigned.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
            //        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DatesAtCurrentResidency.GetStringValue()) cond.FieldValue = cond.FieldValue.IsNotNull() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
            //    });
            //}*/
            #endregion

            View.DocumentAttributeMappingList = BackgroundSetupManager.FetchDandRAttributes(applicantOrderCart, View.TenantID, View.DandRDocuments, inputXML);
        }

        public byte[] FillAttributesInPdf(String documentPath, List<SysDocumentFieldMappingContract> tempMappings, Int32 TenantID, String fullName = null)
        {
            return BackgroundSetupManager.FillDataInPdfForm(documentPath, tempMappings, TenantID, fullName);
        }

        public void GetPackageName(Int32 DPP_ID, Int32 tenantId)
        {
            DeptProgramPackage depProgramPackage = ComplianceDataManager.GetDeptProgramPackageById(DPP_ID, tenantId);
            if (depProgramPackage.IsNotNull())
            {
                View.PackageName = depProgramPackage.CompliancePackage.PackageName;
            }
        }

        public ApplicantDocument SaveDisclosureReleaseDocument(Int32 tenantId, String pdfDocPath, String filename, Int32 fileSize, String documentTypeCode, Int32 currentLoggedInUserId, Int32 orgUserID)
        {
            return ComplianceSetupManager.SaveEsignedDocumentAsPdf(tenantId, pdfDocPath, filename, fileSize, documentTypeCode, currentLoggedInUserId, orgUserID);
        }
    }
}
