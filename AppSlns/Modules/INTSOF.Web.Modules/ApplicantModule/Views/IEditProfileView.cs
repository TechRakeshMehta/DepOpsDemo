using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Linq;

namespace CoreWeb.ApplicantModule.Views
{
    public interface IEditProfileView
    {
        Int32 loggedInUserId { get; }
        Boolean IsApplicant { get; }
        String UserName { get; }
        String FirstName { get; }
        String MiddleName { get; }
        String LastName { get; }
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
        Int32 SelectedTenantId { get; }
        //Int32 SelectedDepartmentId { get; }
        //List<Entity.Organization> Departments { set; }
        //List<Entity.AdminProgramStudy> AdminProgramStudies { set; }
        //Int32 SelectedProgramStudyId { get; }
        //bool Active { get; }
        //bool IsApplicant { get; }
        //Int32 OrganizationId{ get;}
        String FilePath { get; }
        String OriginalFileName { get; }
        String ErrorMessage { set; }
        String SuccessMessage { set; get; }
        //List<int> Programs { get; set; }
        List<ApplicantDocument> ToSaveApplicantUploadedDocuments { get; set; }
        Boolean UpdateAspnetEmail { get; }
        String CurrentEmailAddress { get; set; }
        String PswdRecoveryEmail { get; set; }
        List<PersonAliasContract> PersonAliasList { get; set; }
        List<PreviousAddressContract> ResidentialHistoryList { get; set; }
        DateTime? DateResidentFrom { get; }
        String StateName { get; }
        String CityName { get; }
        String PostalCode { get; }
        Int32 CountryId { get; }
        List<ApplicantInstitutionHierarchyMapping> lstApplicantInstitutionHierarchyMapping { get; set; }
        Boolean IsSSNDisabled { get; set; }

        #region UAT-806 Creation of granular permissions for Client Admin users
        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        Int32 CurrentUserId
        { get; }
        #endregion

        List<UserNodePermissionsContract> lstUserNodePermissionsContract { get; set; }

        //UAT-781
        String DecryptedSSN { get; set; }

        Boolean IsUserGroupCustomAttributeExist { get; set; }

        //UAT-1578 : Addition of SMS notification
        Boolean IsReceiveTextNotification { get; set; }
        String PhoneNumber { get; set; }
        OrganisationUserTextMessageSetting OrganisationUserTextMessageSettingData { get; set; }
        String SMSNotificationErrorMessage { get; set; }

        Int32 OrgUsrID { get; }

        String NoMiddleNameText
        {
            get;
        }

        List<Entity.ClientEntity.TypeCustomAttributes> ProfileCustomAttributeList
        {
            get;
            set;
        }
        //UAT-2447
        Boolean IsInternationalPhoneNumber { get; }
        Boolean IsInternationalSecondaryPhone { get; }
        Boolean IsPersonAliasAddUpdate { get; set; }//UAT-3084
        Boolean IsMultipleValsSelected { get; set; }//UAT-3455
        Boolean IsMultiSelectionAllowed { get; set; }//UAT-3455 
        //CBI|| CABS || Property to check whether location service tenant or not.
        Boolean IsLocationServiceTenant { get; set; }
        List<lkpSuffix> lstSuffixes { get; set; }
        Int32? SelectedSuffixID { get;  }

        string Suffix { get; set; }
        String UserSuffix { get; set; }
        //UAT 3824
        List<Entity.lkpLanguage> CommLanguage { set; }
        Int32? SelectedCommLang { get; set; }

        IQueryable<Entity.ClientEntity.UserGroup> lstUserGroups { get; set; }

        IList<Entity.ClientEntity.UserGroup> lstUserGroupsForUser { get; set; }

       List<Int32> lstUserGroupIDs { get; }
        Boolean IsUserGroupExist { get; set; }
        Boolean IsSuffixDropDownType { get; set; }


    }
}




