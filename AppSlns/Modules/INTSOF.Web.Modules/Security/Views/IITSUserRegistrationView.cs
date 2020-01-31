using System;
using System.Collections.Generic;
using Entity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Globalization;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IITSUserRegistrationView
    {
        String UserName { get; }
        String Password { get; }
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
        //String Alias1 { get; }
        //String Alias2 { get; }
        //String Alias3 { get; }
        String Address1 { get; }
        String Address2 { get; }
        Int32 ZipId { get; set; }
        Int32 SelectedTenantId { get; set; }
        //Int32 SelectedProgramStudyId { get; }
        //Int32 SelectedDepartmentId { get; }
        String FilePath { get; set; }
        String OriginalFileName { get; set; }
        String ErrorMessage { set; }
        /// <summary>
        /// LoginErrorMessage
        /// </summary>
        String LoginErrorMessage { set; }
        String SuccessMessage { set; }
        // List<OrganizationUserProgram> UserPrograms { set; }
        List<Tenant> Tenants { set; }
        //List<Program> AdminProgramStudies { set; }
        List<lkpGender> Gender { set; }
        //List<Organization> Departments { set; }
        //List<int> Programs { get; 
        //    //set; 
        //}
        String WebsiteUrl { get; set; }
        List<PersonAliasContract> PersonAliasList { get; set; }
        /// <summary>
        /// Current User Id.
        /// </summary>
        /// <value>
        /// The identifier of the current user.
        /// </value>
        Int32 CurrentUserId
        {
            get;
        }

        List<LookupContract> ExistingUsersList
        {
            set;
        }

        String LoginUserName { get; }
        String LoginPassword { get; }
        String setSubmitbuttonText { set; }
        OrganizationUser ExistingOrganisationUser
        {
            get;
            set;
        }
        String StateName { get; }
        String CityName { get; }
        String PostalCode { get; }
        Int32 CountryId { get; }
        Boolean IsSSNDisabled { get; set; }
        Boolean IsAutoActive { get; set; }

        String NoMiddleNameText
        {
            get;
        }

        List<Entity.ClientEntity.TypeCustomAttributes> SaveCustomAttributeList
        {
            get;
            set;
        }

        #region UAT-2515
        String ExternalId { get; set; }
        Int32 IntegrationClientId { get; set; }
        Int32 ExternalUserTenantId { get; set; }
        String WebsiteLoginUrl { get; set; }
        List<INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract> matchingUserList { get; set; }
        String MappingCode { get; set; }
        #endregion

        #region UAT-2447
        Boolean IsMaskingOfPrimaryPhoneNumber { get; }
        Boolean IsMaskingOfSecondaryPhoneNumber { get; }
        #endregion

        #region UAT-2792
        Boolean IsShibbolethLogin { get; set; }
        //  String PeopleSoftID { get; set; }
        // String NetID { get; set; }
        String ShibbolethHost { get; set; }
        List<String> lstShibbolethRole { get; set; }
        String ShibbolethEmail { get; set; }
        Int32 ShibbolethHostID { get; set; }
        #endregion
        #region UAT-2883
        String ShibbolethUniqueIdentifier { get; set; }
        String ShibbolethAttributeId { get; set; }
        String ShibbolethFirstName { get; set; }
        String ShibbolethLastName { get; set; }
        String ShibbolethUserName { get; set; }
        String ShibbolethHandlerType { get; set; }
        //String ShibbolethHost { get; set; }
        Boolean IsRandomGeneratedPassword { get; set; }//UAT-2958
        String RandomGeneratedPassword { get; set; }//UAT-2958
        String AttributesWithID { get; set; }
        #endregion

        String ShibbolethRoleString { get; set; }//UAT-3540


        #region UAT-3601
        Boolean IsPasswordRetain { get; set; }
        #endregion

        //CBI|| CABS || Property to check whether location service tenant or not.
        Boolean IsLocationServiceTenant { get; set; }
        List<lkpSuffix> lstSuffixes { get; set; }
        Int32? SelectedSuffixID { get; }

        //UAT 3824
        List<lkpLanguage> CommLanguage { set; }
        Int32? SelectedCommLang { get; set; }
        string Suffix { get; set; }

        LanguageContract CurrentLanguageContract { get; }

        String UsernameAlreadyInUse { get; }
        String EmailIdAlreadyInUse { get; }
        String TryAgain { get; }
        String FromText { get; }
        String IamText { get; }
        String InvalidUsernamePswd { get; }
        String LanguageCode { get; }
        bool IsSuffixDropDownType { get; set; }
    }

    //public class Program
    //{
    //    public String Name { get; set; }
    //    public Int32 Id { get; set; }
    //}



}




