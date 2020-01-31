using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using INTSOF.Utils.Consts;

namespace CoreWeb.SearchUI.Views
{
    public class ApplicantComprehensiveSearchPresenter : Presenter<IApplicantComprehensiveSearchView>
    {
        public void GetTenantList()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode); ;
        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch(Boolean IsAlltenantSelected)
        {
            SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();
            searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
            searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
            if (View.OrganizationUserID > SysXDBConsts.NONE)
            {
                searchDataContract.OrganizationUserId = View.OrganizationUserID;
            }
            searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
            //searchDataContract.ApplicantSSN = String.IsNullOrEmpty(View.SSN) ? null : View.SSN;
            searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);
            searchDataContract.DateOfBirth = View.DateOfBirth;

            try
            {
                String selectedTenants = String.Empty;
                if (!IsAlltenantSelected)
                {
                    XmlDocument doc = new XmlDocument();
                    XmlElement exp = (XmlElement)doc.AppendChild(doc.CreateElement("TenantList"));
                    List<Int32> selectedTenantIds = View.SelectedTenantIds;
                    foreach (Int32 tenantId in selectedTenantIds)
                    {
                        XmlNode e1 = exp.AppendChild(doc.CreateElement("Tenant"));
                        e1.AppendChild(doc.CreateElement("TenantId")).InnerText = tenantId.ToString();
                    }
                    selectedTenants = doc.OuterXml.ToString();
                }
                View.ApplicantSearchData = ComplianceDataManager.GetApplicantComprehensivePortfolioSearch(searchDataContract, View.GridCustomPaging, selectedTenants, IsAlltenantSelected);
                if (View.ApplicantSearchData.IsNotNull() && View.ApplicantSearchData.Count > 0)
                {
                    if (View.ApplicantSearchData[0].TotalCount > 0)
                    {
                        View.VirtualRecordCount = View.ApplicantSearchData[0].TotalCount;
                    }
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCount = 0;
                    View.CurrentPageIndex = 1;
                }
            }
            catch (Exception e)
            {
                View.ApplicantSearchData = null;
                throw e;
            }

        }

        /// <summary>
        /// Getting Formatted SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }

        #endregion
    }
}
