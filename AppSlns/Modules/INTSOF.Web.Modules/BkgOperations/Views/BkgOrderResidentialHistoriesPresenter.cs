using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderResidentialHistoriesPresenter : Presenter<IBkgOrderResidentialHistoriesView>
    {

        private Guid LCNAttCode = new Guid("515BEF57-9072-4D2A-A97A-0C248BB045F9");////License Number
        private Guid MotherNameAttrCode = new Guid("3DA8912A-6337-4B8F-93C4-88BFC3032D2D");////Mother's Maiden Name
        private Guid IdentificationNumberAttrCode = new Guid("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211");////Identification Number

        public void GetResidentialHistoryByOrderId()
        {
            OrganizationUserProfile organizationUserProfile = BackgroundProcessOrderManager.GetOrganisationUserProfileByOrderId(View.SelectedTenantID, View.MasterOrderID);
            if (organizationUserProfile.IsNotNull())
            {
                List<Entity.ResidentialHistoryProfile> tempResidentialAddress = SecurityManager.GetUserResidentialHistoryProfiles(organizationUserProfile.OrganizationUserProfileID).ToList();
                if (tempResidentialAddress.IsNotNull() && tempResidentialAddress.Count > 0)
                {
                    List<PreviousAddressContract> tempList = tempResidentialAddress.Where(cond => cond.Address.ZipCodeID > 0).Select(x => new PreviousAddressContract
                    {
                        ID = x.RHIP_ID,
                        Address1 = x.Address.Address1,
                        Address2 = x.Address.Address2,
                        ZipCodeID = x.Address.ZipCodeID,
                        ResidenceStartDate = x.RHIP_ResidenceStartDate,
                        ResidenceEndDate = x.RHIP_ResidenceEndDate,
                        CityName = x.Address.ZipCode.City.CityName,
                        StateName = x.Address.ZipCode.County.State.StateName,
                        Country = x.Address.ZipCode.City.State.Country.FullName,
                        CountryId = x.Address.ZipCode.County.State.CountryID.Value,
                        CountyName = x.Address.ZipCode.County.CountyName,
                        Zipcode = x.Address.ZipCode.ZipCode1,
                        isCurrent = x.RHIP_IsCurrentAddress,
                        IdentificationNumber = x.RHIP_IdentificationNumber,
                        MotherName = x.RHIP_MotherMaidenName,
                        LicenseNumber = x.RHIP_DriverLicenseNumber
                    }).ToList();

                    tempList.AddRange(tempResidentialAddress.Where(cond => cond.Address.ZipCodeID == 0).Select(x => new PreviousAddressContract
                    {
                        ID = x.RHIP_ID,
                        Address1 = x.Address.Address1,
                        Address2 = x.Address.Address2,
                        ZipCodeID = x.Address.ZipCodeID,
                        ResidenceStartDate = x.RHIP_ResidenceStartDate,
                        ResidenceEndDate = x.RHIP_ResidenceEndDate,
                        CityName = x.Address.AddressExts.FirstOrDefault().AE_CityName,
                        StateName = x.Address.AddressExts.FirstOrDefault().AE_StateName,
                        Country = x.Address.AddressExts.FirstOrDefault().Country.FullName,
                        Zipcode = x.Address.AddressExts.FirstOrDefault().AE_ZipCode,
                        CountryId = x.Address.AddressExts.FirstOrDefault().Country.CountryID,
                        isCurrent = x.RHIP_IsCurrentAddress,
                        IdentificationNumber = x.RHIP_IdentificationNumber,
                        MotherName = x.RHIP_MotherMaidenName,
                        LicenseNumber = x.RHIP_DriverLicenseNumber
                    }).ToList());
                    View.lstPreviousAddress = tempList;
                    return;
                }
            }
            View.lstPreviousAddress = new List<PreviousAddressContract>();
        }

        public Boolean IsResidentialHistoryRequired()
        {
            //List<Int32> lstBackGroundPackages = BackgroundProcessOrderManager.GetBackGroundPackagesForOrderId(View.SelectedTenantID, View.MasterOrderID);
            Guid resHistory_ServiceAttributeGroup = new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue());
            List<PackageGroupContract> tempList = BackgroundProcessOrderManager.CheckShowResidentialHistory(View.SelectedTenantID, View.PackageIDs);
            return tempList.Any(fx => fx.Code.Equals(resHistory_ServiceAttributeGroup));
        }

        public void GetBackGroundPackagesForOrderId()
        {
            View.PackageIDs = BackgroundProcessOrderManager.GetBackGroundPackagesForOrderId(View.SelectedTenantID, View.MasterOrderID);
        }


        public void CheckInternationCriminalSearchAttributes()
        {
            String packageIds = String.Join(",", View.PackageIDs);
            List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstAttributeFields = BackgroundProcessOrderManager.GetAttributeFieldsOfSelectedPackages(packageIds, View.SelectedTenantID);
            if (!lstAttributeFields.IsNullOrEmpty())
            {
                List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstCriminalAttributes = lstAttributeFields
                                                                        .Where(cond => (cond.BSA_Code.ToUpper().Equals("3DA8912A-6337-4B8F-93C4-88BFC3032D2D")
                                                                                    || cond.BSA_Code.ToUpper().Equals("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211")
                                                                                    || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))).ToList();
                if (!lstCriminalAttributes.IsNullOrEmpty())
                {
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == LCNAttCode.ToString() && x.IsAttributeDisplay))
                    {
                        View.ShowCriminalAttribute_License = true;
                    }
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == MotherNameAttrCode.ToString() && x.IsAttributeDisplay))
                    {
                        View.ShowCriminalAttribute_MotherName = true;
                    }
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == IdentificationNumberAttrCode.ToString() && x.IsAttributeDisplay))
                    {
                        View.ShowCriminalAttribute_Identification = true;
                    }
                }
            }
        }

    }
}