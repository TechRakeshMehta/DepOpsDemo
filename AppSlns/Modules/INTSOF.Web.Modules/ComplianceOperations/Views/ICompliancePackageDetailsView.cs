using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;


namespace CoreWeb.ComplianceOperations.Views
{
    public interface ICompliancePackageDetailsView
    {
        Boolean IsItemSeries { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantID { get; set; }
        Int32 ClientCompliancePackageID { get; set; }
        PackageSubscription Subscription { get; set; }
        Int32 PackageId { get; set; }
        Int32 PackageSubscriptionId { get; set; }
        Int32 ComplianceStatusID { get; set; }
        String ComplianceStatus { get; set; }
        CompliancePackage ClientPackage { get; set; }
        List<ExpiringItemList> lstExpiringItem { get; set; }
        //UAT-3806
        List<ListItemEditableBies> lstEditableBy { get; set; }
        Int32 OrganiztionUserID { get; }
        /// <summary>
        /// Id of the Parent compliance Item, present in the ApplicantComplianceItem Entity. 
        /// </summary>
        Int32 ItemId { get; set; }


        Int32 ComplianceCategoryId { get; set; }

        Dictionary<Int32, Int32> AttributeDocuments { get; set; }
        Dictionary<Int32, Int32> ViewAttributeDocuments { get; set; }
        ICompliancePackageDetailsView CurrentViewContext { get; }
        ApplicantComplianceItemDataContract ItemDataContract { get; set; }
        ApplicantComplianceCategoryDataContract CategoryDataContract { get; set; }
        List<ApplicantComplianceAttributeDataContract> lstAttributesData { get; set; }
        List<ApplicantDocument> ApplicantUploadedDocuments { get; set; }
        ComplianceCategory SelectedCategory { get; set; }
        List<ComplianceItem> lstAvailableItems { get; set; }
        String UIValidationErrors { get; set; }
        Boolean IsUIValidationApplicable { get; set; }
        List<ApplicantDocument> ToSaveApplicantUploadedDocuments { get; set; }
        Dictionary<Int32, String> SavedApplicantDocuments { get; set; }
        Int32 AddedViewDocId { get; set; }

        /// <summary>
        /// To check whether it is opened by admin, from any workqueue
        /// </summary>
        String WorkQueue
        {
            get;
            set;
        }
        String ErrorMessage
        {
            get;
            set;
        }

        #region QUEUE MANAGEMENT
        String PackageName
        {
            get;
        }

        String ApplicantName
        {
            get;
            set;
        }

        Int32 ApplicantID
        {
            get;
            set;
        }

        Int32? RushOrderStatusId
        {
            get;
            set;
        }

        Int32? HierarchyID
        {
            get;
            set;
        }
        #endregion

        #region SEND NOTIFICATION FOR FIRST ITEM SUBMITT
        String FirstName { get; set; }
        String LastName { get; set; }
        String PrimaryEmailAddress { get; set; }
        #endregion


        //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
        Boolean IsFullPermissionForVerification
        {
            get;
            set;
        }

        #region UAT-1049:Admin Data Entry
        Int16 DataEntryDocNewStatusId { get; set; }
        Int16 DataEntryDocCompleteStatusId { get; set; }
        //Int32 ViewDocumentTypeID { get; set; }
        #endregion

        Int32? SelectedNodeID
        {
            get;
            set;
        }

        #region UAT-1137:Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
        List<String> lstExplanatoryNotes { get; set; }
        List<ComplianceItem> lstNotAllowedDataEntryItems { get; set; }
        #endregion

        /// <summary>
        /// UAT 1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        /// </summary>
        Int32 OrgUsrID
        {
            get;
        }

        Boolean IsFileUploadExists { get; set; }
        Int32 IsItemExpired { get; set; }
        #region UAT-1607:Student Data Entry Screen changes
        List<ItemSery> lstItemSeries { get; set; }
        List<Int32> ExpiringItemList { get; set; }
        Boolean IsItemSeriesSelected { get; set; }
        #endregion

        Dictionary<Int32, Boolean> LstComplncRqdMapping
        {
            get;
            set;
        }
        /// <summary>
        /// Dictionary contains the category id and its items included series items also.
        /// </summary>
        Dictionary<Int32, List<Int32>> DicCategoryDataForItemSeriesRule { get; set; }
        //UAT-1811
        List<INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail> ComplianceDetailList { get; set; }

        #region UAT-2028:Expired items should also show in the Enter Requirements item selection dropdown on the student screen
        List<Int32> lstMappedItems { get; set; }
        #endregion

        #region UAT-2159 : Show Category Explanatory note as a mouseover on the category name on the student data entry screen.
        Dictionary<Int32, String> dicCatExplanatoryNotes { get; set; }
        #endregion

        #region UAT-3240
        Boolean IsDisabledBothCategoryAndItemExceptionsForTenant { get; set; }

        #endregion

        String QuizConfigSetting { get; set; } //UAT 3299 
        Boolean IsItemEnterDataClick { get; set; } //UAT-3392
        ComplianceItem SelectedItemDetails { get; set; } //UAT 4300

        //UAT-4067
        List<String> allowedFileExtensions
        {
            get;
            set;
        }
        Boolean IsAutoSubmit { get; set; }
        Int32 NodeID { get; set; }
    }
}




