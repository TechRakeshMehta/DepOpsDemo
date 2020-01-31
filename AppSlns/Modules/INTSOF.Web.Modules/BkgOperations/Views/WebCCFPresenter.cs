#region NameSpaces
#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using Entity.ClientEntity;
#endregion

#region Project Specific
#endregion
#endregion

namespace CoreWeb.BkgOperations.Views
{
    public class WebCCFPresenter : Presenter<IWebCCFView>
    {

        /// <summary>
        /// Method to get StateAbbriviation by countyId.
        /// </summary>
        public void GetStateAbbriviationByZipCode()
        {
            Entity.ZipCode zipCodeObj = BackgroundProcessOrderManager.GetZipCodeObjByZipCode(View.TenantId, View.ZipCode);
            if (zipCodeObj.IsNotNull())
            {
                View.StateAbbreviation = zipCodeObj.County.IsNotNull() ? zipCodeObj.County.State.StateAbbreviation : String.Empty;
                if ((zipCodeObj.County.IsNotNull()
                    && zipCodeObj.County.State.Country.Alpha2Code == "US") && (zipCodeObj.County.State.Country.ISO3Code == "USA"))
                {
                    View.IsUSCitizen = "Yes";
                }
                else if (zipCodeObj.County.IsNull())
                {
                    View.IsUSCitizen = "No";
                }
                else
                {
                    View.IsUSCitizen = "No";
                }
            }
        }

        /// <summary>
        /// Method to get ClearStar id and External Vendor Account number.
        /// </summary>
        public void GetClearStarSvcIdAndVendorAcctNumber()
        {
            Int32 extVendorId = 0;
            String eDrugScrnServiceTypeCode = BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue();
            String result = BackgroundProcessOrderManager.GetClearStarServiceId(View.TenantId, View.BackgroundPackageIdList, eDrugScrnServiceTypeCode);
            if (!result.IsNullOrEmpty())
            {
                String[] separator = { "," };
                String[] splitIds = result.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                View.ClearStarServiceId = splitIds[0];
                extVendorId = Convert.ToInt32(splitIds[1]);
                //UAT-3056
                //Int32 DPM_ID = BackgroundProcessOrderManager.GetDPM_IDForEDSPackage(View.TenantId, View.BackgroundPackageIdList, eDrugScrnServiceTypeCode);
                Int32 DPM_ID = View.SelectedNodeId;
                View.ExtVendorAccountNumber = BackgroundProcessOrderManager.GetVendorAccountNumber(View.TenantId, extVendorId, DPM_ID);
            }
        }

        public string GetRegistrationIdAttributeName(Int32 attributeGroupId)
        {
            Guid attGrpMappingCode = new Guid(AppConsts.DRUG_SCREEN_REGISTRATION_ID);
            return BackgroundProcessOrderManager.GetRegistrationIdAttributeName(View.TenantId, attGrpMappingCode, attributeGroupId);
        }
    }
}
