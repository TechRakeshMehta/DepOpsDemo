using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CoreWeb.Search.Views
{
    public interface IApplicantPortfolioProfileView
    {
        #region Properties

        #region Public Properties

        IApplicantPortfolioProfileView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrganizationUserId { get; }
        Entity.OrganizationUser OrganizationUser { get; set; }
        Int32 TenantId { get; }
        Guid? AddressHandleId { get; set; }
        Int32 GenderId { get; set; }
        String Gender { get; set; }
        Int32 ZipCodeId { get; set; }
        Entity.ZipCode ApplicantZipCodeDetails { get; set; }
        List<PersonAliasContract> PersonAliasList { get; set; }

        #region UAT-806 Creation of granular permissions for Client Admin users
        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        #endregion

        #endregion

        #endregion

        //UAT-781
        String DecryptedSSN { get; set; }

        #region  UAT-1180: WB: Combine applicant and portfolio search.

        String UserName { get; }
        String FirstName { get; }
        String MiddleName { get; }
        String LastName { get; }
        List<Entity.lkpGender> GenderList { set; }
        DateTime? DOB { get; }
        String SSN { get; }
        Int32 SelectedGenderId { get; }
        String PrimaryPhone { get; }
        String SecondaryPhone { get; }
        String PrimaryEmail { get; }
        String SecondaryEmail { get; }
        String Address1 { get; }
        String Address2 { get; }
        Int32 ZipId { get; }
        Boolean UpdateAspnetEmail { get; }
        String PswdRecoveryEmail { get; set; }
        String CurrentEmailAddress { get; set; }
        List<PreviousAddressContract> ResidentialHistoryList { get; set; }
        DateTime? DateResidentFrom { get; }
        String StateName { get; }
        String CityName { get; }
        String PostalCode { get; }
        Int32 CountryId { get; }
        List<ApplicantInstitutionHierarchyMapping> lstApplicantInstitutionHierarchyMapping { get; set; }
        Boolean IsSSNDisabled { get; set; }
        String FilePath { get; }
        String OriginalFileName { get; }
        List<UserNodePermissionsContract> lstUserNodePermissionsContract { get; set; }

        #endregion

        //UAT 1438
        Boolean IsUserGroupCustomAttributeExist { get; set; }

        #region UAT-1578
        Boolean IsReceiveTextNotification
        {
            get;
            set;
        }
        String PhoneNumber
        {
            get;
            set;
        }

        Entity.OrganisationUserTextMessageSetting OrganisationUserTextMessageSettingData { get; set; }

        String SMSNotificationErrorMessage { get; set; }

        #endregion

        String UserNodePermissionCode { get; set; }

        String NoMiddleNameText
        {
            get;
        }

        Boolean IsSMSDataAvailableForSave
        {
            get;
        }

        List<Entity.ClientEntity.TypeCustomAttributes> ProfileCustomAttributeList
        {
            get;
            set;
        }
        //UAT-2447
        Boolean IsInternationalPhoneNumber
        {
            get;
        }
        Boolean IsInternationalSecondaryPhone
        {
            get;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        Boolean IsControlsHideForSupportPortal
        {
            get;
            set;
        }

        #region UAT-2930
        //Boolean IsUserTwoFactorAuthenticated
        //{
        //    get;
        //    set;
        //}
        String IsUserTwoFactorAuthenticatedPrevious
        {
            get;
            set;
        }
        #region UAT-3068
        String SelectedAuthenticationType
        {
            get;
            set;
        }
        #endregion

        #region UAT-3047
        Boolean IsLocked
        {
            get;
            set;
        }

        Boolean IsActive
        {
            get;
            set;
        }
        #endregion
        #endregion

        //CBI|| CABS || Property to check whether location service tenant or not.
        Boolean IsLocationServiceTenant { get; set; }
        List<Entity.lkpSuffix> lstSuffixes { get; set; }
        Int32? SelectedSuffixID { get; }
        string Suffix { get; set; }

        IQueryable<Entity.ClientEntity.UserGroup> lstUserGroups { get; set; }

        IList<Entity.ClientEntity.UserGroup> lstUserGroupsForUser { get; set; }

        List<Int32> lstUserGroupIDs { get; }
        Boolean IsUserGroupExist { get; set; }
        Boolean IsSuffixDropDownType { get; set; }
    }
}




