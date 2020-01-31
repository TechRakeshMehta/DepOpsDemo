using Entity.ClientEntity;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace CoreWeb.AdminEntryPortal.Views
{
    public interface IAdminEntryApplicantInformationView
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties



        #endregion

        #region Public Properties

        Int32 CurrentLoggedInUserId { get; }

        Entity.ClientEntity.OrganizationUser OrganizationUser { get; set; }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }

        List<TypeCustomAttributes> lst { get; set; }

        Int32 TenantId
        {
            get;
            set;
        }

        List<AttributeFieldsOfSelectedPackages> lstMvrAttGrp
        {
            get;
            set;
        }
        List<Entity.State> ListStates
        {
            get;
            set;
        }

        Int32 MinResidentailHistoryOccurances
        {
            get;
            set;
        }

        Int32? MaxResidentailHistoryOccurances//Make property nullable regarding UAT-605 
        {
            get;
            set;
        }
        Int32 MinPersonalAliasOccurances
        {
            get;
            set;
        }
        Int32? MaxPersonalAliasOccurances//Make property nullable regarding UAT-605 
        {
            get;
            set;
        }

        //Added property to check is personal information group existor not regarding UAT-672 changes.
        Boolean IsPersonalInformationGroupExist
        {
            get;
            set;
        }

        String PersonalInformationInstructionText
        {
            get;
            set;
        }

        String ResidentialHistoryInstructionText
        {
            get;
            set;
        }

        Boolean IsSSNDisabled { get; set; }

        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        String OrderType
        {
            get;
            set;
        }
        #endregion

        //UAT-781
        String DecryptedSSN { get; set; }

        //UAT 1438
        IQueryable<UserGroup> lstUserGroups { get; set; }

        //UAT 1438
        IList<UserGroup> lstUserGroupsForUser { get; set; }

        //UAT-3455
        IList<UserGroup> lstUsrGrpSavedValues { get; set; }
        List<Int32> lstPreviousSelectedUserGroupIds { get; set; }


        //UAT-1578 : Addition of SMS notification
        //Boolean IsReceiveTextNotification { get; set; }
        String PhoneNumber { get; set; }
        //Entity.OrganisationUserTextMessageSetting OrganisationUserTextMessageSettingData { get; set; }

        #endregion

        #region Methods

        #region Public Methods


        #endregion

        #region Private Methods


        #endregion

        #endregion

        List<AttributeFieldsOfSelectedPackages> LstInternationCriminalSrchAttributes { get; set; }
        //UAT-1834
        Int32 BkgPackageID { get; set; }
        Int32 OrderNodeID { get; set; }
        Int32 HierarchyNodeID { get; set; }
        Int32 BulkOrderUploadID { get; set; }

        Boolean IsMultipleValsSelected { get; set; }//UAT-3455
        Boolean IsMultiSelectionAllowed { get; set; }//UAT-3455

        //UAT-3545 CBI || CABS
        List<ValidateRegexDataContract> lstValidateRegexDataContract { get; set; }
        // Boolean IsLocationServiceTenant { get; set; }
        Boolean IsHavingSSN { get; set; }
        //List<Entity.lkpSuffix> lstSuffixes { get; set; }
        List<Entity.lkpAdminEntrySuffix> lstSuffixes { get; set; }
        //Int32? SelectedSuffixID { get; }
        //String UserSuffix { get; set; }
        List<Entity.lkpLanguage> CommLanguage { set; }
        Int32? SelectedCommLang { get; set; }
        String LanguageCode { get; }
        //Boolean IsSuffixDropDownType { get; set; }
        Int32 OrderID { get; set; }
        Int32 CurrentAddressId { get; set; }
    }
}
