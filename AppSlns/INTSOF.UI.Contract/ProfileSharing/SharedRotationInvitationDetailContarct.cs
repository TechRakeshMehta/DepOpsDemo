using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class SharedRotationInvitationDetailContarct
    {
        public RequirementPackageSubscription RequirementPackageSubscription { get; set; }
        public List<SharedInvitationRequirementDetail> SharedInvitationRequirementDetails { get; set; }

        public SharedRotationInvitationDetailContarct(RequirementPackage clientRequirementPackage, RequirementPackageSubscription requirementPackageSubscription, String incompleteItemStatus, List<Int32> SharedRequirementCategoryIdList)
        {
            RequirementPackageSubscription = requirementPackageSubscription;
            SharedInvitationRequirementDetails = null;
            if (clientRequirementPackage.IsNull() || clientRequirementPackage.RequirementPackageCategories.IsNull())
            {
                return;
            }
            SharedInvitationRequirementDetails = new List<SharedInvitationRequirementDetail>();
            var categories = clientRequirementPackage.RequirementPackageCategories.Where(cpc => !cpc.RPC_IsDeleted
                                                                                    && SharedRequirementCategoryIdList.Contains(cpc.RPC_RequirementCategoryID));
            foreach (RequirementPackageCategory category in categories)
            {
                ApplicantRequirementCategoryData categoryData = GetCategoryData(category.RPC_RequirementCategoryID);

                SharedInvitationRequirementDetails.Add(new SharedInvitationRequirementDetail(category.RequirementCategory, categoryData, incompleteItemStatus));

                if (categoryData.IsNotNull() && category.RequirementCategory.RequirementCategoryItems.IsNotNull())
                {
                    GetComplianceItems(category, categoryData, requirementPackageSubscription.lkpRequirementPackageStatu.RPS_Code, requirementPackageSubscription.RPS_ID);
                }
            }
        }

        public ApplicantRequirementCategoryData GetCategoryData(Int32 CategoryID)
        {
            if (RequirementPackageSubscription.ApplicantRequirementCategoryDatas == null)
                return null;
            return RequirementPackageSubscription.ApplicantRequirementCategoryDatas.FirstOrDefault(x => x.ARCD_RequirementCategoryID == CategoryID && !x.ARCD_IsDeleted);
        }

        public ApplicantRequirementItemData GetItemData(ApplicantRequirementCategoryData categoryData, Int32 itemID)
        {
            String exceptionExpiredCode = lkpCategoryExceptionStatus.EXCEPTION_EXPIRED.GetStringValue();
            String approvedByOverrideCode = lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue();
            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
            if (categoryData == null || categoryData.ApplicantRequirementItemDatas == null)
                return null;
            return categoryData.ApplicantRequirementItemDatas.FirstOrDefault(x => x.ARID_RequirementItemID == itemID && !x.ARID_IsDeleted);
        }

        private void GetComplianceItems(RequirementPackageCategory category, ApplicantRequirementCategoryData categoryData, String packageComplianceStatus, Int32 requirementSubscriptionId)
        {

            List<RequirementCategoryItem> requirementCategoryItems = category.RequirementCategory.RequirementCategoryItems.Where(cci => !cci.RCI_IsDeleted).ToList();

            foreach (RequirementCategoryItem ccItem in requirementCategoryItems)
            {
                ApplicantRequirementItemData itemData = GetItemData(categoryData, ccItem.RequirementItem.RI_ID);
                if (itemData != null)
                    SharedInvitationRequirementDetails.Add(new SharedInvitationRequirementDetail(ccItem, itemData, requirementSubscriptionId));
            }
        }

        #region Get Compliance Data From Snapshot
        public SharedRotationInvitationDetailContarct(DataSet requirementData, String incompleteItemStatus, List<lkpRequirementCategoryStatu> lstRequirementCategoryStatus,
                                                      List<lkpRequirementPackageStatu> lstRequirementPackageStatus, List<lkpRequirementItemStatu> lstRequirementItemStatus,
                                                      List<lkpRequirementFieldDataType> lstRequirementFieldDataType, List<BackgroundDocumentPermissionContract> lstBackgroundDocumentPermissionContract)
        {
            DataTable packageSubscriptionData = requirementData.Tables[0];
            DataTable packageCategoriesData = requirementData.Tables[1];
            DataTable categoryItemdata = requirementData.Tables[2];
            Int32 packageSubscriptionId = 0;
            Int32 packageCompStatusId = 0;

            if (!packageSubscriptionData.Rows.IsNullOrEmpty() && packageSubscriptionData.Rows.Count > 0)
            {
                packageSubscriptionId = Convert.ToInt32(packageSubscriptionData.Rows[0]["RequirementPackageSubscriptionID"]);
                packageCompStatusId = (packageSubscriptionData.Rows[0]["PackageStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE
                                                                        : Convert.ToInt32(packageSubscriptionData.Rows[0]["PackageStatusID"]);
            }

            var packageCompStatus = lstRequirementPackageStatus.FirstOrDefault(cnd => cnd.RPS_ID == packageCompStatusId);

            SharedInvitationRequirementDetails = null;
            if ((packageSubscriptionData.IsNull()) || packageCategoriesData.IsNull())
            {
                return;
            }
            SharedInvitationRequirementDetails = new List<SharedInvitationRequirementDetail>();
            var categoryData = packageCategoriesData.AsEnumerable().OrderBy(ordr => ordr["RequirementCategoryID"]);
            foreach (DataRow category in categoryData)
            {

                SharedInvitationRequirementDetails.Add(new SharedInvitationRequirementDetail(category, categoryItemdata, incompleteItemStatus,
                                                                                           lstRequirementCategoryStatus));

                if (categoryData.IsNotNull() && category.IsNotNull())
                {
                    GetComplianceItemsFromSnapshot(category, categoryItemdata, requirementData, packageCompStatus.RPS_Code, packageSubscriptionId,
                                       lstRequirementCategoryStatus, lstRequirementItemStatus, lstRequirementFieldDataType, lstBackgroundDocumentPermissionContract);
                }
            }
        }

        private void GetComplianceItemsFromSnapshot(DataRow categoryDatarow, DataTable categoryItemData, DataSet immunizationData,
                                                    String packageComplianceStatus, Int32 packageSubscription, List<lkpRequirementCategoryStatu> lstRequirementCategoryStatus,
                                                 List<lkpRequirementItemStatu> lstRequirementItemStatus, List<lkpRequirementFieldDataType> lstRequirementFieldDataType, List<BackgroundDocumentPermissionContract> lstBackgroundDocumentPermissionContract
                                       )
        {

            Int32 categoryId = (categoryDatarow["RequirementCategoryID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(categoryDatarow["RequirementCategoryID"]);
            Int32 compStatusId = (categoryDatarow["CategoryStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(categoryDatarow["CategoryStatusID"]);

            var catComplianceStatus = lstRequirementCategoryStatus.FirstOrDefault(cnd => cnd.RCS_ID == compStatusId && !cnd.RCS_IsDeleted);

            var categories = categoryItemData.AsEnumerable().Where(x => Convert.ToString(x["PackageCategoryID"]) == Convert.ToString(categoryDatarow["PackageCategoryID"]));

            foreach (DataRow itemData in categories)
            {
                //ApplicantComplianceItemData itemData = GetItemData(categoryData, ccItem.ComplianceItem.ComplianceItemID);
                if (itemData["RequirementItemDataID"].GetType().Name != "DBNull")
                    SharedInvitationRequirementDetails.Add(new SharedInvitationRequirementDetail(itemData, immunizationData, categoryDatarow, packageSubscription, categoryId, lstRequirementItemStatus, lstRequirementFieldDataType, lstBackgroundDocumentPermissionContract));
            }
        }
        #endregion

    }

    public class SharedInvitationRequirementDetail
    {
        #region Public Properties

        public String NodeID { get; set; }

        public String ParentNodeID { get; set; }

        public String Name { get; set; }

        public int ApplicantRequirementItemDataId { get; set; }

        public String ReviewStatus { get; set; }

        public String ImgReviewStatus { get; set; }

        public String ImageReviewStatusPath { get; set; }

        public Boolean IsParent { get; set; }

        public String Notes { get; set; }

        public RequirementCategory RequirementCategory { get; set; }

        public RequirementItem RequirementItem { get; set; }

        public Boolean IsCategoryDataEntered { get; set; }

        public String FieldHtml { get; set; }

        public String FieldHtmlItem { get; set; }

        public String FieldHtmlDescription { get; set; }

        public Int32 FirstItemDocumentId
        {
            get;
            set;
        }

        /// <summary>
        /// Property to identify whether the category is Required or not, 
        /// based on the Compliance Required and Date Range for Compliance Required.
        /// </summary>
        public Boolean IsComplianceRequired
        {
            get;
            set;
        }

        #endregion

        public String GetRequirementFieldHtml(List<ApplicantRequirementFieldData> RequirementFieldsData, Int32 packageSubscriptionId)
        {
            if (RequirementFieldsData == null || RequirementFieldsData.Count == 0) return String.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
            //String RequirementFieldHtml = "<table border='0' cellpadding='0' cellspacing='0'>";

            foreach (ApplicantRequirementFieldData RequirementFieldData in RequirementFieldsData)
            {
                String _RequirementFieldValue = RequirementFieldData.ARFD_FieldValue;
                RequirementField requirementField = RequirementFieldData.RequirementField;
                if (!requirementField.IsNullOrEmpty())
                {
                    if (requirementField.lkpRequirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.OPTIONS.GetStringValue().ToLower())
                    {
                        try
                        {
                            Int32 _requirementFieldId = RequirementFieldData.ARFD_RequirementFieldID;

                            if (RequirementFieldData.RequirementField.RequirementFieldOptions == null)
                                _RequirementFieldValue = String.Empty;
                            else
                            {
                                RequirementFieldOption requirementFieldrOption = requirementField.RequirementFieldOptions.Where(opt => opt.RFO_RequirementFieldID == _requirementFieldId && opt.RFO_OptionValue == _RequirementFieldValue && !opt.RFO_IsDeleted).FirstOrDefault();
                                if (requirementFieldrOption.IsNotNull())
                                    _RequirementFieldValue = requirementFieldrOption.RFO_OptionValue;
                                else
                                    _RequirementFieldValue = String.Empty;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (requirementField.lkpRequirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.SIGNATURE.GetStringValue().ToLower())
                    {
                        try
                        {
                            try
                            {
                                string value = "No";
                                if (!string.IsNullOrEmpty(_RequirementFieldValue) && _RequirementFieldValue.ToLower() == "true")
                                    value = "Yes";

                                _RequirementFieldValue = value;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (requirementField.lkpRequirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower())
                    {
                        try
                        {
                            Int32 atributeDocumentCount = RequirementFieldData.ApplicantRequirementDocumentMaps.Where(doc => !doc.ARDM_IsDeleted).Count();
                            _RequirementFieldValue = atributeDocumentCount + " document(s)";
                            var documentDetailData = RequirementFieldData.ApplicantRequirementDocumentMaps.Where(mappedDocs => !mappedDocs.ARDM_IsDeleted).Select(x => x.ApplicantDocument);
                            String fileName = String.Empty;
                            String fileDescription = String.Empty;

                            sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>", !String.IsNullOrEmpty(RequirementFieldData.RequirementField.RF_FieldLabel) ? RequirementFieldData.RequirementField.RF_FieldLabel + ":" : RequirementFieldData.RequirementField.RF_FieldName + ":");

                            if (atributeDocumentCount > 0)
                            {
                                foreach (var documentDetail in documentDetailData)
                                {
                                    if (this.FirstItemDocumentId == AppConsts.NONE)
                                    {
                                        this.FirstItemDocumentId = documentDetail.ApplicantDocumentID;
                                    }
                                    sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}'  onclick=\"ShowDocument('" + documentDetail.ApplicantDocumentID + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span></br>", documentDetail.FileName, ParentNodeID, NodeID, Convert.ToString(documentDetail.ApplicantDocumentID));
                                }
                            }
                            sb.Append("</td></tr>");

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        if (requirementField.lkpRequirementFieldDataType.RFDT_Name.Equals(RequirementFieldDataType.DATE.ToString())
                            && !String.IsNullOrEmpty(_RequirementFieldValue))
                        {
                            try
                            {
                                _RequirementFieldValue = Convert.ToDateTime(_RequirementFieldValue).ToShortDateString();
                            }
                            catch (Exception ex) { }
                        }
                    }
                    if (!(requirementField.lkpRequirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower()))
                    {
                        sb.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}</td></tr>", !String.IsNullOrEmpty(RequirementFieldData.RequirementField.RF_FieldLabel) ? RequirementFieldData.RequirementField.RF_FieldLabel + ":" : RequirementFieldData.RequirementField.RF_FieldName + ":", _RequirementFieldValue);
                    }
                }
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        private String GetImagePathOfCategoryReviewStatus(String reviewStatus, String catRuleStatusID)
        {
            //UAT-2165: Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            if (!this.IsComplianceRequired)
            {
                //UAT 3106 
                if (catRuleStatusID.Trim() == AppConsts.STR_ONE)
                {
                    return "~/Resources/Mod/Compliance/icons/yes16.png";
                }
                else
                {
                    return "~/Resources/Mod/Compliance/icons/optional.png";
                }
            }

            if (!String.IsNullOrEmpty(reviewStatus))
            {
                if (RequirementCategoryStatus.APPROVED.GetStringValue().Equals(reviewStatus))
                    return "~/Resources/Mod/Compliance/icons/yes16.png";
                else if (RequirementCategoryStatus.INCOMPLETE.GetStringValue().Equals(reviewStatus))
                    return "~/Resources/Mod/Compliance/icons/no16.png";
                else if (RequirementCategoryStatus.PENDING_REVIEW.GetStringValue().Equals(reviewStatus))
                    return "~/Resources/Mod/Compliance/icons/attn16.png";
            }
            return "~/Resources/Mod/Compliance/icons/no16.png";
        }

        public SharedInvitationRequirementDetail(RequirementCategory requirementCategory, ApplicantRequirementCategoryData categoryData, String incompleteItemStatus)
        {
            this.ParentNodeID = String.Empty;
            this.IsParent = true;
            this.NodeID = String.Format("P_{0}", requirementCategory.RC_ID);
            this.RequirementCategory = requirementCategory;
            this.Name = !String.IsNullOrEmpty(requirementCategory.RC_CategoryLabel) ? requirementCategory.RC_CategoryLabel : requirementCategory.RC_CategoryName;

            if (categoryData == null)
            {
                this.Notes = String.Empty;
                this.ReviewStatus = incompleteItemStatus;
                this.ImgReviewStatus = incompleteItemStatus;
                this.ImageReviewStatusPath = GetImagePathOfCategoryReviewStatus(RequirementCategoryStatus.INCOMPLETE.GetStringValue(), String.Empty);
            }
            else
            {
                this.Notes = String.Empty;
                //Changes as per UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.

                this.ReviewStatus = categoryData.lkpRequirementCategoryStatu.RCS_Name;
                this.ImgReviewStatus = categoryData.lkpRequirementCategoryStatu.RCS_Name;
                this.ImageReviewStatusPath = GetImagePathOfCategoryReviewStatus(categoryData.lkpRequirementCategoryStatu.RCS_Code, String.Empty);
            }

            if (categoryData.IsNullOrEmpty() ||
                (!categoryData.IsNullOrEmpty()
                    && (categoryData.ApplicantRequirementItemDatas.IsNullOrEmpty()
                        || categoryData.ApplicantRequirementItemDatas.Count() == 0))
                )
            {
                this.IsCategoryDataEntered = false;
            }
            else
                this.IsCategoryDataEntered = true;
        }

        public SharedInvitationRequirementDetail(RequirementCategoryItem requirementItem, ApplicantRequirementItemData itemData, Int32 requirementSubscriptionId)
        {
            this.ParentNodeID = String.Format("P_{0}", requirementItem.RCI_RequirementCategoryID);
            this.IsParent = false;
            this.NodeID = requirementItem.RCI_RequirementItemID.ToString();
            this.RequirementCategory = requirementItem.RequirementCategory;
            this.RequirementItem = requirementItem.RequirementItem;
            this.Name = !String.IsNullOrEmpty(requirementItem.RequirementItem.RI_ItemLabel) ? requirementItem.RequirementItem.RI_ItemLabel : requirementItem.RequirementItem.RI_ItemName;

            StringBuilder sb = new StringBuilder();
            ApplicantRequirementItemDataId = itemData.ARID_ID;
            if (itemData == null)
            {
                this.Notes = this.FieldHtml = String.Empty;
                this.ReviewStatus = ApplicantItemComplianceStatus.Incomplete.ToString();
            }
            else
            {

                this.Notes = String.Empty;
                this.ReviewStatus = itemData.lkpRequirementItemStatu.RIS_Name;
                var RequirementFieldData = new List<ApplicantRequirementFieldData>();
                var fieldDataTemp = itemData.ApplicantRequirementFieldDatas.Where(x => !x.ARFD_IsDeleted).ToList();
                if (fieldDataTemp != null && fieldDataTemp.Count > 0)
                {
                    foreach (var requirementFieldDataItem in fieldDataTemp)
                    {
                        var fieldDataItem = requirementFieldDataItem;
                        Int32 requirementFieldID = requirementFieldDataItem.RequirementField.RF_ID;
                        RequirementFieldData.Add(fieldDataItem);
                    }

                }

                this.FieldHtmlItem = GetRequirementFieldHtml(RequirementFieldData, requirementSubscriptionId);
            }
        }

        #region Get Compliance Data From Snapshot
        public SharedInvitationRequirementDetail(DataRow categoryDatarow, DataTable categoryItemData, String incompleteItemStatus,
                                                        List<lkpRequirementCategoryStatu> lstRequirementCategoryStatus)
        {
            //UAT-2165: Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            //String complianceRequired = Convert.ToString(categoryDatarow["IsComplianceRequired"]);
            this.IsComplianceRequired = categoryDatarow["IsComplianceRequired"] == DBNull.Value ? false : Convert.ToBoolean(categoryDatarow["IsComplianceRequired"]);
            this.ParentNodeID = String.Empty;
            this.IsParent = true;
            this.NodeID = String.Format("P_{0}", categoryDatarow["RequirementCategoryID"].ToString());

            //Set Requirement Category
            RequirementCategory category = new RequirementCategory();
            category.RC_CategoryName = Convert.ToString(categoryDatarow["RequirementCategoryName"]);
            category.RC_CategoryLabel = Convert.ToString(categoryDatarow["RequirementCategoryLabel"]);
            category.RC_ID = Convert.ToInt32(categoryDatarow["RequirementCategoryID"]);
            category.RC_Description = String.Empty;

            this.RequirementCategory = category;

            this.Name = !(categoryDatarow["RequirementCategoryLabel"].ToString().IsNullOrEmpty()) ? categoryDatarow["RequirementCategoryLabel"].ToString() : categoryDatarow["RequirementCategoryName"].ToString();
            Int32 compStatusId = (categoryDatarow["CategoryStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(categoryDatarow["CategoryStatusID"]);
            var catComplianceStatus = lstRequirementCategoryStatus.FirstOrDefault(cnd => cnd.RCS_ID == compStatusId && !cnd.RCS_IsDeleted);
            var catRuleStatusID = Convert.ToString(categoryDatarow["RuleStatusID"]); //UAT 3106

            Boolean isItemDataEntered = categoryItemData.AsEnumerable().Any(cnd => cnd["PackageCategoryID"] == categoryDatarow["PackageCategoryID"]);


            if (categoryDatarow["RequirementCategoryDataID"].GetType().Name == "DBNull")
            {
                this.Notes = String.Empty;
                this.ReviewStatus = incompleteItemStatus;
                this.ImgReviewStatus = incompleteItemStatus;
                this.ImageReviewStatusPath = GetImagePathOfCategoryReviewStatus(RequirementCategoryStatus.INCOMPLETE.GetStringValue(), catRuleStatusID);
            }
            else
            {
                this.Notes = String.Empty;
                if (catComplianceStatus.IsNotNull())
                {
                    this.ReviewStatus = catComplianceStatus.RCS_Name;
                    this.ImgReviewStatus = catComplianceStatus.RCS_Name;
                    this.ImageReviewStatusPath = GetImagePathOfCategoryReviewStatus(catComplianceStatus.RCS_Code, catRuleStatusID);
                }
            }

            if (categoryDatarow["RequirementCategoryDataID"].GetType().Name == "DBNull" ||
                (categoryDatarow["RequirementCategoryDataID"].GetType().Name != "DBNull"
                    && (!isItemDataEntered))
                )
            {
                this.IsCategoryDataEntered = false;
            }
            else
                this.IsCategoryDataEntered = true;
        }

        public SharedInvitationRequirementDetail(DataRow itemDataRow, DataSet immunizationData, DataRow categoryData, Int32 packageSubscriptionId,
                                                   Int32 categoryId, List<lkpRequirementItemStatu> lstRequirementItemStatus,
                                                   List<lkpRequirementFieldDataType> lstRequirementFieldDataType, List<BackgroundDocumentPermissionContract> lstBackgroundDocumentPermissionContract)
        {
            DataTable itemRequirementFieldData = immunizationData.Tables[3];
            DataTable requirementFieldDocdata = immunizationData.Tables[4];

            Int32 itemId = (itemDataRow["RequirementItemID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(itemDataRow["RequirementItemID"]);
            this.ParentNodeID = String.Format("P_{0}", categoryId);
            this.IsParent = false;
            this.NodeID = itemId.ToString();

            //Set Requirement Category
            RequirementCategory category = new RequirementCategory();
            category.RC_CategoryName = Convert.ToString(categoryData["RequirementCategoryName"]);
            category.RC_ID = Convert.ToInt32(categoryId);
            category.RC_Description = String.Empty;
            this.RequirementCategory = category;


            this.Name = !(itemDataRow["RequirementItemLabel"].ToString().IsNullOrEmpty()) ? itemDataRow["RequirementItemLabel"].ToString() : itemDataRow["RequirementItemName"].ToString();

            StringBuilder sb = new StringBuilder();
            ApplicantRequirementItemDataId = (itemDataRow["RequirementItemDataID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(itemDataRow["RequirementItemDataID"]);
            Int32 itemStatusId = (itemDataRow["ItemStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(itemDataRow["ItemStatusID"]);
            var itemComplianceStatus = lstRequirementItemStatus.FirstOrDefault(cnd => cnd.RIS_ID == itemStatusId && !cnd.RIS_IsDeleted);
            if (itemDataRow["RequirementItemDataID"].GetType().Name == "DBNull")
            {
                this.Notes = this.FieldHtml = String.Empty;
                this.ReviewStatus = ApplicantItemComplianceStatus.Incomplete.ToString();
                this.ImgReviewStatus = ApplicantItemComplianceStatus.Incomplete.ToString();
                this.ImageReviewStatusPath = GetImagePathOfCategoryReviewStatus(ApplicantItemComplianceStatus.Incomplete.GetStringValue(),String.Empty);
            }
            else
            {

                //this.Notes = itemData.Notes;
                this.ReviewStatus = itemComplianceStatus.RIS_Name;
                this.ImgReviewStatus = itemComplianceStatus.RIS_Name;
                this.ImageReviewStatusPath = GetImagePathOfCategoryReviewStatus(itemComplianceStatus.RIS_Code,String.Empty);
                var requirementFieldData = new List<DataRow>();
                var requirementFieldDataTemp = itemRequirementFieldData;
                if (requirementFieldDataTemp != null && requirementFieldDataTemp.Rows.Count > 0)
                {
                    IEnumerable<DataRow> requirementFieldDataItemList = requirementFieldDataTemp.AsEnumerable().Where(x => Convert.ToString(x["CategoryItemID"]) == Convert.ToString(itemDataRow["CategoryItemID"]) && x["RequirementFieldDataID"].GetType().Name != "DBNull");
                    foreach (DataRow requirementFieldDataItem in requirementFieldDataItemList)
                    {
                        var RequirementFieldDataItem = requirementFieldDataItem;
                        String requirementFieldID = (requirementFieldDataItem["RequirementFieldID"]).GetType().Name == "DBNull" ? "0" : Convert.ToString(requirementFieldDataItem["RequirementFieldID"]);
                        requirementFieldData.Add(RequirementFieldDataItem);
                    }
                }

                this.FieldHtmlItem = GetRequirementFieldHtmlFromSnapshot(requirementFieldData, requirementFieldDocdata, packageSubscriptionId, lstRequirementFieldDataType, lstBackgroundDocumentPermissionContract);

            }
        }

        public String GetRequirementFieldHtmlFromSnapshot(List<DataRow> RequirementFieldsData, DataTable RequirementFieldDocData, Int32 packageSubscriptionId, List<lkpRequirementFieldDataType> lstComplianceRequirementFieldDataType, List<BackgroundDocumentPermissionContract> lstBackgroundDocumentPermissionContract)
        {           
            if (RequirementFieldsData == null || RequirementFieldsData.Count == 0) return String.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
            String RequirementFieldHtml = "<table border='0' cellpadding='0' cellspacing='0'>";

            foreach (DataRow RequirementFieldData in RequirementFieldsData)
            {
                Int32 RequirementFieldDataTypeId = (RequirementFieldData["RequirementFieldDataTypeID"]).GetType().Name == "DBNull" ? AppConsts.NONE :
                                                                                        Convert.ToInt32(RequirementFieldData["RequirementFieldDataTypeID"]);

                var requirementFieldDataType = lstComplianceRequirementFieldDataType.FirstOrDefault(cnd => cnd.RFDT_ID == RequirementFieldDataTypeId);
                String _requirementFieldValue = RequirementFieldData["RequirementFieldValue"].ToString();
                String requirementFieldName = !Convert.ToString(RequirementFieldData["RequirementFieldLabel"]).IsNullOrEmpty() ? RequirementFieldData["RequirementFieldLabel"].ToString() : RequirementFieldData["RequirementFieldName"].ToString();

                if (requirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.OPTIONS.GetStringValue().ToLower())
                {
                    try
                    {
                        Int32 _requirementFieldId = Convert.ToInt32(RequirementFieldData["RequirementFieldID"]);

                        if (RequirementFieldData["RequirementFieldOptionID"].GetType().Name == "DBNull")
                            _requirementFieldValue = String.Empty;
                        else
                        {
                            if (RequirementFieldData["RequirementFieldOptionText"].GetType().Name != "DBNull")
                                _requirementFieldValue = RequirementFieldData["RequirementFieldOptionText"].ToString();
                            else
                                _requirementFieldValue = String.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (requirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.SIGNATURE.GetStringValue().ToLower())
                {
                    try
                    {
                        string value = "No";
                        if (!string.IsNullOrEmpty(_requirementFieldValue) && _requirementFieldValue.ToLower() == "true")
                            value = "Yes";

                        _requirementFieldValue = value;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (requirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower())
                {
                    try
                    {
                        var applicantDocument = RequirementFieldDocData.AsEnumerable().Where(x => Convert.ToString(x["RequirementFieldDataID"]) == Convert.ToString(RequirementFieldData["RequirementFieldDataID"]));
                        String fileDescription = String.Empty;

                        sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>", requirementFieldName + ":");

                        foreach (var documentDetail in applicantDocument)
                        {
                            String fileName = Convert.ToString(documentDetail["FileName"]);
                            Int32 applicantDocumentId = documentDetail["ApplicantDocumentID"].GetType().Name == "DBNull" ? AppConsts.NONE
                                                                                                        : Convert.ToInt32(documentDetail["ApplicantDocumentID"]);

                            String fileExtension = Path.GetExtension(fileName);
                            if (1 == 1)
                            {
                                // sb.AppendFormat("<a href='/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}' class='attr_val'>{2}</a>", applicantDocumentId, 104, fileName);
                            }
                            if (!lstBackgroundDocumentPermissionContract.IsNullOrEmpty())
                            {
                                Int32 _requirementFieldId = Convert.ToInt32(RequirementFieldData["RequirementFieldID"]);
                                BackgroundDocumentPermissionContract backgroundDocumentPermissionContract = lstBackgroundDocumentPermissionContract.Where(x => x.RequirementFieldID == _requirementFieldId).FirstOrDefault();
                                if (backgroundDocumentPermissionContract.IsNotNull() && backgroundDocumentPermissionContract.IsDisabled)
                                {
                                    sb.AppendFormat("<span style='text-decoration:none;cursor:auto;color:black !important;' class='attr_val details_viewer'>{0}</span></br>", fileName);
                                }
                                else
                                {
                                    if (this.FirstItemDocumentId == AppConsts.NONE)
                                    {
                                        this.FirstItemDocumentId = applicantDocumentId;
                                    }

                                    sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}'  _ps='{4}'  onclick=\"ShowRotationDocument('" + applicantDocumentId + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span></br>", fileName, ParentNodeID, NodeID, applicantDocumentId, Convert.ToString(packageSubscriptionId));
                                }
                            }
                            else
                            {
                                if (this.FirstItemDocumentId == AppConsts.NONE)
                                {
                                    this.FirstItemDocumentId = applicantDocumentId;
                                }

                                sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}'  _ps='{4}'  onclick=\"ShowRotationDocument('" + applicantDocumentId + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span></br>", fileName, ParentNodeID, NodeID, applicantDocumentId, Convert.ToString(packageSubscriptionId));
                            }
                        }
                        sb.Append("</td></tr>");
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (requirementFieldDataType.RFDT_Code.ToLower().ToLower() == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower()) //UAT-3532
                {
                    try
                    {
                        var applicantDocument = RequirementFieldDocData.AsEnumerable().Where(x => Convert.ToString(x["RequirementFieldDataID"]) == Convert.ToString(RequirementFieldData["RequirementFieldDataID"]));
                        String fileDescription = String.Empty;
                        String IsAttributeValueTrue = String.Empty;

                        if (!_requirementFieldValue.IsNullOrEmpty() && _requirementFieldValue == "1" && applicantDocument.Count() > 0)                       
                            IsAttributeValueTrue = " (";

                        sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}" + IsAttributeValueTrue , requirementFieldName + ":", Convert.ToInt32(_requirementFieldValue) == AppConsts.NONE ? "No" : "Yes");

                        if (!_requirementFieldValue.IsNullOrEmpty() && _requirementFieldValue == "1" && applicantDocument.Count() > 0)
                        {
                            foreach (var documentDetail in applicantDocument)
                            {
                                String fileName = Convert.ToString(documentDetail["FileName"]);
                                Int32 applicantDocumentId = documentDetail["ApplicantDocumentID"].GetType().Name == "DBNull" ? AppConsts.NONE
                                                                                                            : Convert.ToInt32(documentDetail["ApplicantDocumentID"]);

                                String fileExtension = Path.GetExtension(fileName);
                                if (!lstBackgroundDocumentPermissionContract.IsNullOrEmpty())
                                {
                                    Int32 _requirementFieldId = Convert.ToInt32(RequirementFieldData["RequirementFieldID"]);
                                    BackgroundDocumentPermissionContract backgroundDocumentPermissionContract = lstBackgroundDocumentPermissionContract.Where(x => x.RequirementFieldID == _requirementFieldId).FirstOrDefault();
                                    if (backgroundDocumentPermissionContract.IsNotNull() && backgroundDocumentPermissionContract.IsDisabled)
                                    {
                                        sb.AppendFormat("<span style='text-decoration:none;cursor:auto;color:black !important;' class='attr_val details_viewer'>{0}</span></br>", fileName);
                                    }
                                    else
                                    {
                                        if (this.FirstItemDocumentId == AppConsts.NONE)
                                        {
                                            this.FirstItemDocumentId = applicantDocumentId;
                                        }

                                        sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}'  _ps='{4}'  onclick=\"ShowRotationDocument('" + applicantDocumentId + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span>)</br>", fileName, ParentNodeID, NodeID, applicantDocumentId, Convert.ToString(packageSubscriptionId));
                                    }
                                }
                                else
                                {
                                    if (this.FirstItemDocumentId == AppConsts.NONE)
                                    {
                                        this.FirstItemDocumentId = applicantDocumentId;
                                    }

                                    sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}'  _ps='{4}'  onclick=\"ShowRotationDocument('" + applicantDocumentId + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span>)</br>", fileName, ParentNodeID, NodeID, applicantDocumentId, Convert.ToString(packageSubscriptionId));
                                }
                            }
                        }
                        sb.Append("</td></tr>");
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    if (requirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.DATE.GetStringValue().ToLower()
                        && !String.IsNullOrEmpty(_requirementFieldValue))
                    {
                        try
                        {
                            _requirementFieldValue = Convert.ToDateTime(_requirementFieldValue).ToShortDateString();
                        }
                        catch (Exception ex) { }
                    }
                }
                if (!(requirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower()) && !(requirementFieldDataType.RFDT_Code.ToLower() == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower()))
                {
                    sb.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}</td></tr>", requirementFieldName + ":", _requirementFieldValue);
                }
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        #endregion
    }
}
