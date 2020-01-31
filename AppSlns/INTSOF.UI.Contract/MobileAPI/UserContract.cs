using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace INTSOF.UI.Contract.MobileAPI
{
    public class UserContract
    {
        public int? SelectedGenderId;
        public Int32? SelectedGenderDefaultKeyID;
        public Int32 ID { get; set; }
        public String PrimaryEmail { get; set; }
        public String SecondaryEmail { get; set; }
        public  String PswdRecoveryEmail { get; set; }
        public String SSN { get; set; }
        public String SSNL4 { get; set; }
        public String Username { get; set; }
        public string UserName { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String UserID { get; set; }
        public String InstitutionName { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public DateTime? DOB { get; set; }
        public Int32 TenantID { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FilePath { get; set; }
        public string OriginalFileName { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public int? SelectedCommLang { get; set; }
        public string ErrorMessage { get; set; }

        public Boolean IsAutoActive { get; set; }
        public Int32 MasterZipcodeID { get; set; }
        public Int32 CountryId { get; set; }
        public string Address { get; set; }
        public int ZipId { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public bool IsLocationServiceTenant { get; set; }
        public List<PersonAliasContract> PersonAliasList { get; set; }
        public string NoMiddleNameText { get; set; }
        public bool IsMaskingOfPrimaryPhoneNumber { get; set; }
        public bool IsMaskingOfSecondaryPhoneNumber { get; set; }
        public Int32? SelectedSuffixID { get; set; }

        public String Gender { get; set; }
        public String CountryName { get; set; }
        public Int32 OrganizationUserProfileID { get; set; }
		public string Suffix { get; set; }
        public int? StateId { get; set; }
        public string SMSPhoneNumber { get; set; }
        public bool IsReceiveTextNotification { get; set; }
        public String SelectedLanguageCode { get; set; }
        public string VerificationPermissionCode { get; set; }
        public Boolean UpdateAspnetEmail { get; set; }
        public Boolean IsSuffixDropDown { get; set; }
    }
}
