using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation
{
    [Serializable]
    [DataContract]
    public class RotationRequirementContract
    {
        public RequirementPackageSubscriptionContract RequirementPackageSubscription { get; set; }
        public List<RotationRequirementUIContract> RotationRequiremenUIContractList { get; set; }
        public Boolean IsClinicalRotationExpired { get; set; }


        public RotationRequirementContract(RequirementPackageContract rotationRequirementPackage, RequirementPackageSubscriptionContract requirementPackageSubscription,
                                           List<Int32> expItemList, string incompleteCategoryStatus, Boolean isClinicalRotationExpired, Boolean isNeedToHideDocumentLink,
                                           Dictionary<Int32, Boolean> lstComplianceRqdCategoryMapping, List<RequirementItemPaymentContract> ItemPaymentList, Int32 clinicalRotationID, Boolean IsOptionalcategorysettingEnabled, String QuizConfigSetting, List<Int32> expRequirementItemsList, Int32 TenantID = 0, Int32 currentUserID = 0)
        {

            RequirementPackageSubscription = requirementPackageSubscription;
            IsClinicalRotationExpired = isClinicalRotationExpired;
            RotationRequiremenUIContractList = null;
            if (rotationRequirementPackage.IsNull() || rotationRequirementPackage.LstRequirementCategory.IsNull())
            {
                RotationRequiremenUIContractList = new List<RotationRequirementUIContract>();
                return;
            }
            RotationRequiremenUIContractList = new List<RotationRequirementUIContract>();
            List<RequirementCategoryContract> lstRequirementCategory = rotationRequirementPackage.LstRequirementCategory;
            foreach (RequirementCategoryContract requirementCategory in lstRequirementCategory)
            {
                ApplicantRequirementCategoryDataContract categoryData = null;
                if (requirementPackageSubscription.ApplicantRequirementCategoryData.IsNotNull())

                    categoryData = requirementPackageSubscription.ApplicantRequirementCategoryData.FirstOrDefault(x => x.RequirementCategoryID ==
                                                                                                                                   requirementCategory.RequirementCategoryID);

                var _showAddRequirements = true;

                if (ShowAddButton(requirementCategory, categoryData, expRequirementItemsList))
                    _showAddRequirements = true;
                else
                    _showAddRequirements = false;

                RotationRequiremenUIContractList.Add(new RotationRequirementUIContract(requirementCategory, categoryData, _showAddRequirements, incompleteCategoryStatus,
                                                                                       lstComplianceRqdCategoryMapping, IsOptionalcategorysettingEnabled, expRequirementItemsList));
                if (categoryData.IsNotNull() && requirementCategory.LstRequirementItem.IsNotNull())
                {
                    GetRotationRequirementItemUIContractList(requirementCategory, categoryData, expItemList, isClinicalRotationExpired,
                                                             requirementPackageSubscription.RequirementPackageSubscriptionStatusCode, isNeedToHideDocumentLink,
                                                             lstComplianceRqdCategoryMapping, ItemPaymentList, clinicalRotationID, QuizConfigSetting, expRequirementItemsList, TenantID, currentUserID);
                }
            }
        }

        private Boolean ShowAddButton(RequirementCategoryContract category, ApplicantRequirementCategoryDataContract categoryData, List<Int32> expRequirementItemsList)
        {
            Int32 _submittedItemCount = 0;
            Int32 _totalItemCount = 0;
            //For Resolution of Bug ID: 16313                               
            if (category.LstRequirementItem.IsNotNull())
            {
                //UAT 3792 Ability to turn off applicant editibility on rotation items/categories
                _totalItemCount = category.LstRequirementItem.Where(x => x.AllowItemDataEntry == true).ToList().Count;
            }
            //UAT 3792 Ability to turn off applicant editibility on rotation items/categories
            var lstAllowItemDataEntryItems = category.LstRequirementItem.Where(x => x.AllowItemDataEntry == true).Select(x => x.RequirementItemID);
            if (!categoryData.IsNullOrEmpty() && categoryData.ApplicantRequirementItemData.IsNotNull() && lstAllowItemDataEntryItems.IsNotNull() && lstAllowItemDataEntryItems.Count() > AppConsts.NONE) // Submitted items count will be zero in case no category data is entered, else get the count
            {
                foreach (var itemData in categoryData.ApplicantRequirementItemData.Where(x => lstAllowItemDataEntryItems.Contains(x.RequirementItemID)))
                {
                    _submittedItemCount++;
                }
            }


            #region UAT-3458
            //UAT-2028:Expired items should also show in the Enter Requirements item selection dropdown on the student screen
            List<Int32> lstReqCategoryItems = new List<Int32>();
            List<Int32> lstReqAllExpItemList = new List<Int32>();
            lstReqAllExpItemList.AddRange(expRequirementItemsList);
            //lstReqAllExpItemList.AddRange(expPendingItemList);
            //lstReqAllExpItemList.AddRange(expNotApprdItemLst);
            if (!category.IsNullOrEmpty() && !category.LstRequirementItem.IsNullOrEmpty())
            {
                lstReqCategoryItems = category.LstRequirementItem.Where(cnd => !cnd.IsDeleted).Select(slct => slct.RequirementItemID).ToList();
            }
            var expReqItemListForCategory = lstReqAllExpItemList.Where(x => lstReqCategoryItems.Contains(x));
            #endregion

            // If count of items for which data has been entered equals count of items that can be edited, then disable, else enable
            //If Clinical rotation is expired then add new requirement button not visible
            if ((_submittedItemCount >= _totalItemCount || IsClinicalRotationExpired))//&& expReqItemListForCategory.IsNullOrEmpty())
                return false;
            else
                return true;

        }

        private void GetRotationRequirementItemUIContractList(RequirementCategoryContract requirementCategory, ApplicantRequirementCategoryDataContract categoryData,
                                                              List<Int32> expItemList, Boolean isClinicalRotationIsExpired, String packageComplianceStatus,
                                                              Boolean isNeedToHideDocumentLink, Dictionary<Int32, Boolean> LstComplianceRqdCategoryMapping, List<RequirementItemPaymentContract> ItemPaymentList, Int32 clinicalRotationID, String QuizConfigSetting, List<Int32> expRequirementItemsList, Int32 TenantID = 0, Int32 currentUserID = 0)
        {
            List<RequirementItemContract> requirementItemContractLIst = requirementCategory.LstRequirementItem;
            foreach (RequirementItemContract requirementItemContract in requirementItemContractLIst)
            {
                ApplicantRequirementItemDataContract itemData = GetApplicantRequirementItem(categoryData, requirementItemContract.RequirementItemID);
                if (itemData != null)
                    RotationRequiremenUIContractList.Add(new RotationRequirementUIContract(requirementItemContract, requirementCategory, itemData, expItemList,
                                                                                           isClinicalRotationIsExpired, packageComplianceStatus, isNeedToHideDocumentLink,
                                                                                           LstComplianceRqdCategoryMapping, ItemPaymentList, clinicalRotationID, QuizConfigSetting, expRequirementItemsList, TenantID, currentUserID));
            }
        }

        public ApplicantRequirementItemDataContract GetApplicantRequirementItem(ApplicantRequirementCategoryDataContract categoryData, Int32 itemID)
        {
            if (categoryData == null || categoryData.ApplicantRequirementItemData == null)
                return null;
            return categoryData.ApplicantRequirementItemData.FirstOrDefault(x => x.RequirementItemID == itemID); //"AAAD"
        }
    }


    public class RotationRequirementUIContract
    {
        #region Private Variables

        private Boolean _showItemEditDelete = true;
        private Boolean _showItemDelete = true; //UAT-3511
        private Boolean _showAddRequirement = true;
        private Boolean _isNeedToHideDocumentLink = false;
        private Boolean _showViewRequirement = false;
        private Boolean _isPaymentItem = false;
        #endregion

        [DataMember]
        public String NodeID { get; set; }

        [DataMember]
        public String ParentNodeID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public int ApplicantRequirementItemDataId { get; set; }

        [DataMember]
        public String ReviewStatus { get; set; }

        [DataMember]
        public String ImgReviewStatus { get; set; }

        [DataMember]
        public String ImageReviewStatusPath { get; set; }

        [DataMember]
        public Boolean IsParent { get; set; }

        [DataMember]
        public String FieldHtml { get; set; }

        [DataMember]
        public RequirementCategoryContract RequirementCategory { get; set; }

        [DataMember]
        public RequirementItemContract RequirementItem { get; set; }

        [DataMember]
        public Boolean IsCategoryDataEntered { get; set; }

        /// <summary>
        /// Used to show/hide 'Enter Requirements' on the data entry screen
        /// </summary>
        [DataMember]
        public Boolean ShowAddRequirement
        {
            get { return _showAddRequirement; }
            set { _showAddRequirement = value; }
        }

        [DataMember]
        public Boolean ShowViewRequirement
        {
            get { return _showViewRequirement; }
            set { _showViewRequirement = value; }
        }

        /// <summary>
        /// Used to Manage normal Item 'Update' & 'Delete' buttons 
        /// </summary>
        [DataMember]
        public Boolean ShowItemEditDelete
        {
            get { return _showItemEditDelete; }
            set { _showItemEditDelete = value; }
        }
        //UAT-3511
        [DataMember]
        public Boolean ShowItemDelete
        {
            get { return _showItemDelete; }
            set { _showItemDelete = value; }
        }

        [DataMember]
        public Boolean IsPaymentItem
        {
            get { return _isPaymentItem; }
            set { _isPaymentItem = value; }
        }

        [DataMember]
        public String FieldHtmlItem { get; set; }

        [DataMember]
        public String FieldHtmlPaymentItem { get; set; }

        [DataMember]
        public String fieldHtmlDescription { get; set; }

        /// <summary>
        /// Property to identify whether the category is Required or not, 
        /// based on the Compliance Required and Date Range for Compliance Required.
        /// </summary>
        [DataMember]
        public Boolean IsComplianceRequired
        {
            get;
            set;
        }

        [DataMember]
        public String ItemRejectionReason
        {
            get;
            set;
        }

        //UAT - 3106
        [DataMember]
        public Boolean OptionalCategorySettings
        {
            get;
            set;
        }
        //UAT - 4165
        [DataMember]
        public Boolean IsEditableByApplicant
        {
            get;
            set;
        }
        public RotationRequirementUIContract()
        {
        }

        public RotationRequirementUIContract(RequirementCategoryContract requirementCategory, ApplicantRequirementCategoryDataContract categoryData,
                                             Boolean isAddRequirementsVisible, String incompleteCategoryStatus, Dictionary<Int32, Boolean> LstComplianceRqdCategoryMapping, Boolean IsOptionalcategorysettingEnabled, List<Int32> expRequirementItemsList)
        {

            //UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            this.IsComplianceRequired = IsCatComplianceRequired(requirementCategory.RequirementCategoryID, LstComplianceRqdCategoryMapping);
            this.OptionalCategorySettings = IsOptionalcategorysettingEnabled; //UAT 3106
            if (IsComplianceRequired)
            {
                this.ParentNodeID = INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_REQUIREMENT_CATEGORY_NODE;
            }
            else
            {
                this.ParentNodeID = INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_REQUIREMENT_CATEGORY_NODE; ;
            }

            //this.ParentNodeID = String.Empty;
            this.IsParent = true;
            this.NodeID = String.Format("P_{0}", requirementCategory.RequirementCategoryID);
            this.RequirementCategory = requirementCategory;

            //No Description and no label field for any except attribute (Package, Category, Item). Duplicate names should be allowed. 
            //this.Name = !String.IsNullOrEmpty(requirementCategory.RequirementCategoryLabel) ? requirementCategory.RequirementCategoryLabel :
            //                                                                                  requirementCategory.RequirementCategoryName;

            this.Name = requirementCategory.CategoryLabel.IsNullOrEmpty() ? requirementCategory.RequirementCategoryName : requirementCategory.CategoryLabel;

            this.ShowAddRequirement = isAddRequirementsVisible;
            this.ShowViewRequirement = !isAddRequirementsVisible;

            if (requirementCategory.LstRequirementItem.Where(d => expRequirementItemsList.Contains(d.RequirementItemID)).Any())//     expRequirementItemsList.Count > AppConsts.NONE)
            {
                //UAT 3792 Ability to turn off applicant editibility on rotation items/categories
                if (requirementCategory.LstRequirementItem.Where(d => expRequirementItemsList.Contains(d.RequirementItemID) && d.AllowItemDataEntry == true).Any())
                {
                    this.ShowAddRequirement = true;
                    this.ShowViewRequirement = false;
                }
                //else if (requirementCategory.LstRequirementItem.Where(d => expRequirementItemsList.Contains(d.RequirementItemID) && d.AllowItemDataEntry == false).Any())
                //{
                //    this.ShowAddRequirement = false;
                //    this.ShowViewRequirement = true;
                //}
            }
            //UAT 3792 Ability to turn off applicant editibility on rotation items/categories
            Boolean IsAllowItemDataEntry = requirementCategory.LstRequirementItem.All(x => x.AllowItemDataEntry == false);

            if (IsAllowItemDataEntry)
            {
                this.ShowAddRequirement = false;
                this.ShowViewRequirement = true;
            }

            if (!Convert.ToBoolean(requirementCategory.IsEditableByApplicant))
            {
                if (!requirementCategory.LstRequirementItem.Where(d => d.IsEditableByApplicant == true).Any())
                {
                    this.ShowAddRequirement = false;
                }
                this.ShowItemDelete = false;
                this.ShowItemEditDelete = false;
            }

            if (categoryData == null)
            {
                this.ReviewStatus = incompleteCategoryStatus;
                this.ImgReviewStatus = incompleteCategoryStatus;
                this.ImageReviewStatusPath = GetImagePathOfCategoryReviewStatus(ApplicantCategoryComplianceStatus.Incomplete.GetStringValue(), String.Empty);
            }
            else
            {
                this.ReviewStatus = categoryData.RequirementCategoryStatus;
                this.ImgReviewStatus = categoryData.RequirementCategoryStatus;
                this.ImageReviewStatusPath = GetImagePathOfCategoryReviewStatus(categoryData.RequirementCategoryStatusCode, categoryData.CategoryRuleStatusID);
            }

            if (categoryData.IsNullOrEmpty() ||
                (!categoryData.IsNullOrEmpty()
                    && (categoryData.ApplicantRequirementItemData.IsNullOrEmpty()
                        || categoryData.ApplicantRequirementItemData.Count() == 0))
                )
            {
                this.IsCategoryDataEntered = false;
            }
            else
                this.IsCategoryDataEntered = true;
        }

        public RotationRequirementUIContract(RequirementItemContract item, RequirementCategoryContract category, ApplicantRequirementItemDataContract itemData,
                                             List<Int32> expItemList, Boolean isClinicalRotationIsExpired, String packageComplianceStatus, bool isNeedToHideDocumentLink,
                                             Dictionary<Int32, Boolean> LstComplianceRqdCategoryMapping, List<RequirementItemPaymentContract> ItemPaymentList, Int32 clinicalRotationID, String QuizConfigSetting, List<Int32> expRequirementItemsList, Int32 TenantID = 0, Int32 currentUserID = 0)
        {
            this.ParentNodeID = String.Format("P_{0}", category.RequirementCategoryID);
            this.IsParent = false;
            this.NodeID = item.RequirementItemID.ToString();
            this.RequirementCategory = category;
            this.RequirementItem = item;
            this.Name = !String.IsNullOrEmpty(item.RequirementItemLabel) ? item.RequirementItemLabel : item.RequirementItemName;
            //this.Name = item.RequirementItemName;
            this._isNeedToHideDocumentLink = isNeedToHideDocumentLink;

            ApplicantRequirementItemDataId = itemData.RequirementItemDataID;

            //UAT-2165:
            this.IsComplianceRequired = IsCatComplianceRequired(category.RequirementCategoryID, LstComplianceRqdCategoryMapping);

            if (itemData == null)
            {
                this.ReviewStatus = RequirementItemStatus.INCOMPLETE.ToString();
            }
            else
            {
                this.ReviewStatus = itemData.RequirementItemStatus;

                if (itemData.RequirementItemStatusCode == RequirementItemStatus.APPROVED.GetStringValue()
                    //Bug Id: 9891:ADB Rotation Data Entry: When item gets expired then update/delete link doesn’t appear on applicant rotation details
                    //|| itemData.RequirementItemStatusCode == RequirementItemStatus.EXPIRED.GetStringValue() 
                    || isClinicalRotationIsExpired)
                {
                    this.ShowItemEditDelete = false;
                    this.ShowItemDelete = false; //UAT-3511
                }

                //UAT 3299
                if ((QuizConfigSetting == AppConsts.STR_ONE || QuizConfigSetting.IsNullOrEmpty()) && itemData.RequirementItemStatusCode == RequirementItemStatus.INCOMPLETE.GetStringValue()
                    && itemData.ItemMovementTypeCode == LkpItemMovementStatus.VIA_QUIZ_EVALUATION.GetStringValue())
                {
                    this.ShowItemEditDelete = false;
                    this.ShowItemDelete = false; //UAT-3511
                }

                #region UAT-3458
                if (expRequirementItemsList.Count > 0 && expRequirementItemsList.Contains(itemData.RequirementItemID))
                {
                    this.ShowItemEditDelete = true;
                    //this.ShowItemDelete = false; //UAT-3511
                }
                #endregion
                // Hide the 'Add requirements' and 'Exception' button when data is already added for the items
                ShowAddRequirement = false;

                //UAT 3792 Ability to turn off applicant editibility on rotation items/categories
                if (item.AllowItemDataEntry == false)
                {
                    this.ShowItemEditDelete = false;
                    this.ShowItemDelete = false;
                }

                //UAT-4165
                if (!Convert.ToBoolean(item.IsEditableByApplicant))
                {
                    this.ShowAddRequirement = false;
                    this.ShowItemDelete = false;
                    this.ShowItemEditDelete = false;
                }
                //UAT-2226- Allow the rejection reason to populate to the student when an admin leaves a rejection note for rotation requirements
                if (!itemData.RejectionReason.IsNullOrEmpty())
                {                    
                    this.ItemRejectionReason = itemData.RejectionReason?.Replace("###", "<br/>");
                }

                var fieldData = new List<ApplicantRequirementFieldDataContract>();

                if (itemData.ApplicantRequirementFieldData != null)
                {
                    var fieldDataTemp = itemData.ApplicantRequirementFieldData.OrderBy(o => o.RequirementFieldDisplayOrder).ToList();
                    if (fieldDataTemp != null && fieldDataTemp.Count > 0)
                    {
                        foreach (var fldDataItem in fieldDataTemp)
                        {
                            var fieldDataItem = fldDataItem;
                            Int32 fieldID = fldDataItem.RequirementFieldID;
                            fieldData.Add(fieldDataItem);
                        }
                    }
                }
                if (item.IsPaymentType && !itemData.IsNullOrEmpty()) //UAT-3077
                {
                    var itemOrderData = ItemPaymentList.Where(x => x.CategoryID == category.RequirementCategoryID && x.ItemID == item.RequirementItemID).FirstOrDefault();
                    String TotalPrice = String.Empty;
                    String OrderStatus = String.Empty;
                    String PkgName = String.Empty;
                    String CategoryName = String.Empty;
                    String ItemID = String.Empty;
                    String CategoryID = String.Empty;
                    String ItemName = String.Empty;
                    String PkgId = String.Empty;
                    String PkgSubscriptionId = String.Empty;
                    String OrderID = String.Empty;
                    String OrderNumber = String.Empty;
                    String OrderStatusCode = String.Empty;
                    String InvoiceNumber = String.Empty;
                    String OrganizationUserProfileID = String.Empty;
                    String ClinicalRotationID = clinicalRotationID.ToString();
                    String strTenantID = TenantID.ToString();
                    String strCurrentUserID = currentUserID.ToString();
                    if (!itemOrderData.IsNullOrEmpty())
                    {

                        OrderStatus = Convert.ToString(itemOrderData.OrderStatus);
                        PkgName = Convert.ToString(itemOrderData.PkgName);
                        CategoryName = Convert.ToString(itemOrderData.CategoryName);
                        ItemID = Convert.ToString(itemOrderData.ItemID);
                        CategoryID = Convert.ToString(itemOrderData.CategoryID);
                        ItemName = Convert.ToString(itemOrderData.ItemName);
                        PkgId = Convert.ToString(itemOrderData.PkgId);
                        PkgSubscriptionId = Convert.ToString(itemOrderData.PkgSubscriptionId);
                        OrderID = Convert.ToString(itemOrderData.orderID);
                        OrderNumber = Convert.ToString(itemOrderData.OrderNumber);
                        OrderStatusCode = itemOrderData.OrderStatusCode;
                        InvoiceNumber = itemOrderData.invoiceNumber;
                        OrganizationUserProfileID = Convert.ToString(itemOrderData.OrganizationUserProfileID);
                        Boolean IsPaid = false;

                        if (OrderStatusCode == ApplicantOrderStatus.Paid.GetStringValue()) // "OSPAD"
                        {
                            IsPaid = true;
                            TotalPrice = Convert.ToString(Math.Round(itemOrderData.TotalPrice, 2, MidpointRounding.AwayFromZero));
                        }
                        else
                            TotalPrice = Convert.ToString(Math.Round(itemOrderData.PaidAmount, 2, MidpointRounding.AwayFromZero));

                        this.FieldHtmlItem = GetFieldHtml(fieldData, item, GetItemPaymentHtml(TotalPrice, IsPaid, OrderStatus, PkgName, CategoryName, ItemID, CategoryID, ItemName, PkgId, PkgSubscriptionId, OrderID, OrderNumber, TotalPrice, InvoiceNumber, OrganizationUserProfileID, ClinicalRotationID, OrderStatusCode, strTenantID, strCurrentUserID));
                        this.FieldHtmlPaymentItem = GetItemPaymentHtml(TotalPrice, IsPaid, OrderStatus, PkgName, CategoryName, ItemID, CategoryID, ItemName, PkgId, PkgSubscriptionId, OrderID, OrderNumber, TotalPrice, InvoiceNumber, OrganizationUserProfileID, ClinicalRotationID, OrderStatusCode, strTenantID, strCurrentUserID);
                    }
                    else
                    {
                        this.FieldHtmlItem = GetFieldHtml(fieldData, item, GetItemPaymentHtml(String.Concat(Convert.ToString(Math.Round(item.Amount.Value, 2, MidpointRounding.AwayFromZero))), false, OrderStatus, PkgName, CategoryName, ItemID, CategoryID, ItemName, PkgId, PkgSubscriptionId, OrderID, OrderNumber, AppConsts.ZERO, InvoiceNumber, OrganizationUserProfileID, ClinicalRotationID, OrderStatusCode, strTenantID, strCurrentUserID));
                        this.FieldHtmlPaymentItem = GetItemPaymentHtml(String.Concat(Convert.ToString(Math.Round(item.Amount.Value, 2, MidpointRounding.AwayFromZero))), false, OrderStatus, PkgName, CategoryName, ItemID, CategoryID, ItemName, PkgId, PkgSubscriptionId, OrderID, OrderNumber, AppConsts.ZERO, InvoiceNumber, OrganizationUserProfileID, ClinicalRotationID, OrderStatusCode, strTenantID, strCurrentUserID);
                    }
                }
                else
                    this.FieldHtmlItem = GetFieldHtml(fieldData, item, String.Empty);

                if (item.IsPaymentType)
                {
                    this.IsPaymentItem = true;
                }
                //UAT 4380 
                if (item.IsNotNull() && item.LstRequirementField.IsNotNull())
                {
                    Boolean attributeDataEntryAllowed = item.LstRequirementField.Where(cond => cond.IsCustomSetting && cond.IsEditableByApplicant == true).IsNullOrEmpty() ? false : item.LstRequirementField.Where(cond => cond.IsCustomSetting && cond.IsEditableByApplicant == true && !cond.IsDeleted).Select(select => select.IsEditableByApplicant.Value).FirstOrDefault();

                    if (attributeDataEntryAllowed)
                    {
                        this.ShowAddRequirement = true;
                        this.ShowItemDelete = false;
                        this.ShowItemEditDelete = false;
                    }
                }
            }
        }
        //UAT 3106 Added new parameter CategoryRuleStatusID
        private String GetImagePathOfCategoryReviewStatus(String reviewStatus, String CategoryRuleStatusID)
        {
            if (!this.IsComplianceRequired)
            {
                //UAT 3106 
                if (!this.OptionalCategorySettings)
                {
                    return "~/Resources/Mod/Compliance/icons/optional.png";
                }
                else if (CategoryRuleStatusID.Trim() == AppConsts.STR_ONE)
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

        public String GetFieldHtml(List<ApplicantRequirementFieldDataContract> fieldsData, RequirementItemContract reqItem, String itemPaymentHtml)
        {
            if (fieldsData == null || fieldsData.Count == 0 && String.IsNullOrEmpty(itemPaymentHtml)) return String.Empty;

            else if (fieldsData == null || fieldsData.Count == 0 && !String.IsNullOrEmpty(itemPaymentHtml))
            {
                StringBuilder sbItemPaymentOnly = new StringBuilder();
                sbItemPaymentOnly.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                sbItemPaymentOnly.Append(itemPaymentHtml);
                sbItemPaymentOnly.Append("</table>");
                return sbItemPaymentOnly.ToString();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
            if (!String.IsNullOrEmpty(itemPaymentHtml))
            {
                sb.Append(itemPaymentHtml);
            }
            List<ApplicantRequirementFieldDataContract> lstFielddata = fieldsData.OrderBy(o => o.RequirementFieldDisplayOrder).ToList();

            foreach (ApplicantRequirementFieldDataContract fieldData in lstFielddata)
            {
                String fieldValue = fieldData.FieldValue;
               
                Int32 fieldId = fieldData.RequirementFieldID;
                if (reqItem.LstRequirementField.IsNotNull())
                {
                    RequirementFieldContract reqField = reqItem.LstRequirementField.FirstOrDefault(x => x.RequirementFieldID == fieldId);

                    if (fieldData.FieldDataTypeCode.ToLower() == RequirementFieldDataType.OPTIONS.GetStringValue().ToLower())
                    {
                        try
                        {
                            if (reqField.RequirementFieldData.LstRequirementFieldOptions == null)
                                fieldValue = String.Empty;
                            else
                            {
                                RequirementFieldOptionsData fieldOption = reqField.RequirementFieldData.LstRequirementFieldOptions.Where(opt => opt.RequirementFieldID == fieldId
                                                                                                                                && opt.OptionValue == fieldValue).FirstOrDefault();
                                if (fieldOption.IsNotNull())
                                    fieldValue = fieldOption.OptionText;
                                else
                                    fieldValue = String.Empty;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (fieldData.FieldDataTypeCode.ToLower() == RequirementFieldDataType.SIGNATURE.GetStringValue().ToLower())
                    {
                        try
                        {
                            string value = "No";
                            if (!string.IsNullOrEmpty(fieldValue) && fieldValue.ToLower() == "true")
                                value = "Yes";

                            fieldValue = value;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (fieldData.FieldDataTypeCode.ToLower() == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower())
                    {
                        if (!_isNeedToHideDocumentLink)
                        {
                            try
                            {
                                Int32 fieldDocumentCount = AppConsts.NONE;
                                if (fieldData.LstApplicantFieldDocumentMapping.IsNotNull())
                                {
                                    fieldDocumentCount = fieldData.LstApplicantFieldDocumentMapping.Count();
                                }
                                fieldValue = fieldDocumentCount + " document(s)";
                                String fileName = String.Empty;
                                String fileDescription = String.Empty;

                                sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'><span class='attr_val dtl_viewer' _p='{2}' _n='{3}' _i='{4}' _a='{0}' title='View document list'>{1}</span></td></tr>", !String.IsNullOrEmpty(reqField.RequirementFieldLabel) ? reqField.RequirementFieldName + ":" : reqField.RequirementFieldLabel + ":", fieldValue, ParentNodeID, NodeID, Name);
                                if (fieldDocumentCount > 0)
                                {
                                    GetDocumentDescriptionDetails(fieldData.LstApplicantFieldDocumentMapping);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    else
                    {
                        if (fieldData.FieldDataTypeCode.ToLower() == RequirementFieldDataType.DATE.GetStringValue().ToLower()
                            && !String.IsNullOrEmpty(fieldValue))
                        {
                            try
                            {
                                fieldValue = Convert.ToDateTime(fieldValue).ToShortDateString();
                            }
                            catch (Exception ex) { }
                        }
                        else if (fieldData.FieldDataTypeCode.ToLower() == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower()
                            && !String.IsNullOrEmpty(fieldValue)
                            && !_isNeedToHideDocumentLink)
                        {
                            string fieldValueViewDocExpired = fieldData.FieldValueViewDoc;
                            try
                            {
                                if (!fieldValueViewDocExpired.IsNullOrEmpty())
                                    fieldValue = fieldValueViewDocExpired;
                                else
                                {
                                    fieldValue = Convert.ToInt32(fieldValue) == AppConsts.NONE ? "No" : "Yes";
                                    //UAT-1615: If I am a student using the view document for a requirement in a rotation package, I should be able to access a copy of the completed form.
                                    if (!fieldData.LstApplicantFieldDocumentMapping.IsNullOrEmpty() && fieldValue.ToLower() == "yes")
                                    {
                                        var appDocumen = fieldData.LstApplicantFieldDocumentMapping.FirstOrDefault();
                                        if (!appDocumen.IsNullOrEmpty())
                                        {
                                            String documentLink = String.Format("<span class='attr_val' onclick=\"ShowSignedDocument('" + appDocumen.ApplicantDocumentId + "')\" >{0}</span>", appDocumen.FileName);
                                            fieldValue = fieldValue + " ( " + documentLink + " )";
                                        }
                                    }
                                }
                                sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}</td></tr>", !String.IsNullOrEmpty(reqField.RequirementFieldLabel) ? reqField.RequirementFieldName + ":" : reqField.RequirementFieldLabel + ":", fieldValue);
                            }
                            catch (Exception ex) { }
                        }
                        else if (fieldData.FieldDataTypeCode.ToLower() == RequirementFieldDataType.VIEW_VIDEO.GetStringValue().ToLower()
                           && !String.IsNullOrEmpty(fieldValue))
                        {
                            try
                            {
                                fieldValue = fieldValue + " " + "seconds";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    //UAT-1615: If I am a student using the view document for a requirement in a rotation package, I should be able to access a copy of the completed form.
                    //Added check for View_Document.
                    if (!(fieldData.FieldDataTypeCode.ToLower() == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower())
                        && !(fieldData.FieldDataTypeCode.ToLower() == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower()))
                    {
                        sb.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}</td></tr>", !String.IsNullOrEmpty(reqField.RequirementFieldLabel) ? reqField.RequirementFieldName + ":" : reqField.RequirementFieldLabel + ":", fieldValue);
                    }
                }
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        //UAT-3077
        public String GetItemPaymentHtml(String Amount, Boolean IsPaid, String OrderStatus, String PkgName, String CategoryName, String ItemID, String CategoryID, String ItemName, String PkgId, String PkgSubscriptionId, String OrderID, String OrderNumber, String TotalPrice, String InvoiceNumber, String OrganizationUserProfileID, String clinicalRotationID, String paymentStatusCode, String tenantID, String CurrentUserID)
        {
            StringBuilder sb = new StringBuilder();
            // sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");

            sb.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>Amount: </td><td class='td-three'>$" + Amount + "</td></tr>");

            if (IsPaid)
            {
                sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Order Number:</td><td class='td-three'>" + OrderNumber + "</td></tr>");
                sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Payment Status:</td><td class='td-three'>" + OrderStatus + "</td></tr>");
            }
            else
            {
                if (!String.IsNullOrEmpty(PkgSubscriptionId))
                {

                    sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Order Number:</td><td class='td-three'>" + OrderNumber + "</td></tr>");
                    sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Payment Status:</td><td class='td-three'>");
                    sb.AppendFormat(" " + OrderStatus);
                    if (!paymentStatusCode.Equals(ApplicantOrderStatus.Cancelled.GetStringValue()))
                        sb.AppendFormat("<a href='#' class='completePaymentLink' style='color:#0000ff;'  onclick=\"ItemPaymentClick('"
                                                     + PkgName + "','" + CategoryName + "','" + ItemID + "','" + CategoryID + "','" + ItemName + "','" + PkgId + "','" + PkgSubscriptionId + "','" + OrderID + "','" + OrderNumber + "','" + TotalPrice + "','" + InvoiceNumber + "','" + AppConsts.TRUE + "','" + OrganizationUserProfileID + "','" + AppConsts.ZERO + "','" + AppConsts.ONE + "','" + clinicalRotationID + "','" + tenantID + "','" + CurrentUserID + "')\" >{0}</a></br>", " (Complete your payment)");
                    sb.AppendFormat("</td></tr>");
                }
                else
                    sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Payment Status:</td><td class='td-three'>" + "Pending Payment" + "</td></tr>");
            }


            // sb.Append("</table>");
            return sb.ToString();
        }

        private void GetDocumentDescriptionDetails(IEnumerable<ApplicantFieldDocumentMappingContract> documentDetailData)
        {
            StringBuilder documentDescription = new StringBuilder();
            documentDescription.Append(@"<table border='0' cellpadding='0' id='documentDescription" + ParentNodeID + "-" + NodeID + "' cellspacing='0' style='display:none;'>");
            documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two'><b>Name</b></td><td class='td-three'><b>Description</b></td></tr>");
            foreach (var documentDetail in documentDetailData)
            {
                documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}</td></tr>", documentDetail.FileName, documentDetail.Description);
            }
            documentDescription.AppendFormat("</table>");
            fieldHtmlDescription = documentDescription.ToString();
        }

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        /// <summary>
        /// Returns whether the Current category is Optional or not, 
        /// based on the Compliance Required and Date Range for Compliance Required.
        /// </summary>
        /// <param name="requirementPackageCategoryID"></param>
        /// <param name="LstComplianceRqdCategoryMapping"></param>
        /// <returns></returns>
        private Boolean IsCatComplianceRequired(Int32 requirementCategoryID, Dictionary<Int32, Boolean> LstComplianceRqdCategoryMapping)
        {
            return LstComplianceRqdCategoryMapping.FirstOrDefault(x => x.Key == requirementCategoryID).Value;
        }
        #endregion
    }
}
