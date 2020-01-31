using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ContractManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CoreWeb.ContractManagement.Views
{
    public class ManageContractPresenter : Presenter<IManageContract>
    {
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {

        }

        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);

        }

        public List<lkpContractExpirationType> GetRenewalType()
        {
            List<lkpContractExpirationType> contractRenewalType;
            if (View.SelectedTenantID == AppConsts.NONE)
            {
                contractRenewalType = new List<lkpContractExpirationType>();
            }
            else
            {
                contractRenewalType = LookupManager.GetLookUpData<lkpContractExpirationType>(View.SelectedTenantID);
            }
            return contractRenewalType;
        }

        public void GetContractSearch()
        {
            List<ContractManagementContract> contractSearchData;
            if (View.SelectedTenantID == AppConsts.NONE)
            {
                contractSearchData = new List<ContractManagementContract>();
            }
            else
            {
                View.SearchContract.CurrentLoggedInUserId = (View.TenantID == SecurityManager.DefaultTenantID) ? (Int32?)null : View.CurrentUserId;
                contractSearchData = ContractManager.GetContractSearch(View.SearchContract, View.GridCustomPaging, View.TenantID);
            }
            View.ContractDataList = contractSearchData;
            if (contractSearchData.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = contractSearchData[0].TotalRecordCount;
            }
        }

        /// <summary>
        /// Get the Contract Sites
        /// </summary>
        public void GetContractSitesContacts()
        {
            if (View.SelectedTenantID != AppConsts.NONE)
            {
                var _tpl = ContractManager.GetContractSitesContacts(View.ContractId, View.SelectedTenantID);

                View.lstContactContract = _tpl.Item2;
                View.lstSiteContracts = _tpl.Item1;
            }
            else
            {
                View.lstSiteContracts = new List<SiteContract>();
                View.lstContactContract = new List<ContactContract>();
            }
        }

        /// <summary>
        /// Save the new Contract and its related data.
        /// </summary>
        public void SaveContracts()
        {
            ContractManager.SaveContract(View.ContractData, View.CurrentUserId, View.SelectedTenantID);
        }

        /// <summary>
        /// Get the basic details of the Contract
        /// </summary>
        public void GetContractDetails()
        {
            View.ContractData = ContractManager.GetContractDetails(View.ContractId, View.SelectedTenantID);
        }

        /// <summary>
        /// Update the Contract and it's related Details
        /// </summary>
        public void UpdateContract()
        {
            ContractManager.UpdateContract(View.ContractData, View.CurrentUserId, View.SelectedTenantID);
        }

        /// <summary>
        /// Get the DPM_ID of the RootNode of the Tenant
        /// </summary>
        public void GetRootNodeDPMId()
        {
            View.RootDPMId = ComplianceDataManager.GetInstitutionDPMID(View.SelectedTenantID).ToString();
        }

        #region Contract Document

        public void GetContractDocuments()
        {
            if (View.SelectedTenantID != AppConsts.NONE)
            {
                View.ContractDocumentContractList = ContractManager.GetContractDocuments(View.ContractId, View.SelectedTenantID);
            }
            else
            {
                View.ContractDocumentContractList = new List<ContractDocumentContract>();
            }
        }

        public List<lkpDocumentType> GetDocumentType()
        {
            List<lkpDocumentType> lstDocType;
            if (View.SelectedTenantID == AppConsts.NONE)
            {
                lstDocType = new List<lkpDocumentType>();
            }
            else
            {
                //UAT-1667 : Include three new document types - Letter of Renewal, Amendment, and Other for the document dropdown.
                lstDocType = LookupManager.GetLookUpData<lkpDocumentType>(View.SelectedTenantID)
                    .Where(cond => cond.DMT_Code == DocumentType.INSURANCE_CERTIFICATE_DOCUMENT.GetStringValue()
                || cond.DMT_Code == DocumentType.CONTRACT_DOCUMENT.GetStringValue()
                || cond.DMT_Code == DocumentType.LETTER_OF_RENEWAL.GetStringValue()
                || cond.DMT_Code == DocumentType.AMENDMENT.GetStringValue()
                || cond.DMT_Code == DocumentType.OTHER.GetStringValue()).ToList();
            }
            return lstDocType;
        }

        //UAT-1475	Make it easier to view a contract entry.        
        public void GetContractDocumentForViewDocument(Int32 contractID)
        {
            if (View.SelectedTenantID > AppConsts.NONE && contractID > AppConsts.NONE)
            {
                View.ContractDocumentContractList = ContractManager.GetContractDocuments(contractID, View.SelectedTenantID);
            }
        }
        #endregion

        public Boolean DeleteContract(Int32 contractID)
        {
            return ContractManager.DeleteContract(contractID, View.CurrentUserId, View.SelectedTenantID);
        }

        public void GetSiteDocuments()
        {
            if (View.SelectedTenantID != AppConsts.NONE)
            {
                View.SiteDocumentContractList = ContractManager.GetSiteDocuments(View.SiteId, View.SelectedTenantID);
            }
            else
            {
                View.SiteDocumentContractList = new List<SiteDocumentContract>();
            }
        }

        public List<ContractSite> GetContractsites(Int32 contractId)
        {
            return ContractManager.GetContractsites(contractId, View.SelectedTenantID);
        }

        public void GetGranularPermission()
        {
            View.IsReadOnlyPermission = false;
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.MANAGE_CONTRACT.GetStringValue())
                        && dicPermissions[EnumSystemEntity.MANAGE_CONTRACT.GetStringValue()].ToUpper()
                            == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsReadOnlyPermission = true;
                }
            }
        }

        #region UAT-1665: Agreement Status

        /// <summary>
        /// Get Document Status list
        /// </summary>
        /// <returns></returns>
        public List<lkpContractDocumentStatu> GetDocumentStatus()
        {
            if (View.SelectedTenantID == AppConsts.NONE)
            {
                return new List<lkpContractDocumentStatu>();
            }
            else
            {
                return LookupManager.GetLookUpData<lkpContractDocumentStatu>(View.SelectedTenantID)
                    .Where(con => !con.CDS_IsDeleted).ToList();
            }
        }

        #endregion

        public List<ContractType> GetContractType()
        {
            List<ContractType> lstContractType = new List<ContractType>();
            if (View.SelectedTenantID > AppConsts.NONE)
            {
                lstContractType = ContractManager.GetContractTypes(View.SelectedTenantID).ToList();
                foreach (ContractType contractType in lstContractType)
                {
                    contractType.CT_Name = contractType.CT_Label.IsNullOrEmpty() ? contractType.CT_Name : contractType.CT_Label;
                }
            }
            return lstContractType;
        }

        public void GetSitesContacts()
        {
            if (View.SelectedTenantID != AppConsts.NONE)
            {
                View.lstSiteContacts = ContractManager.GetSiteContacts(View.ContractId, View.SiteId, View.SelectedTenantID);

            }
            else
            {
                View.lstSiteContacts = new List<ContactContract>();
            }
        }

        public List<Int32> GetContractIDs()
        {
            View.SearchContract.CurrentLoggedInUserId = (View.TenantID == SecurityManager.DefaultTenantID) ? (Int32?)null : View.CurrentUserId;
            View.lstContractIDs = ContractManager.GetContractIDs(View.SearchContract, View.GridCustomPaging, View.TenantID, View.VirtualRecordCount);
            return View.lstContractIDs;

        }
    }
}
