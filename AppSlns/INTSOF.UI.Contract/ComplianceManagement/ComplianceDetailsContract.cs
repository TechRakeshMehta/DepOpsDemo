using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceDetailsContract
    {
        public PackageSubscription Subscription { get; set; }
        public List<ComplianceDetail> ComplianceDetails { get; set; }

        /// <summary>
        /// Store the data of entire Category & Items of the Package from the Data entry screen, on OnLoad call.
        /// </summary>
        private List<ListCategoryEditableBies> _listEditableByData { get; set; }
        private Boolean IsExceptionNotAllowed(List<ListCategoryEditableBies> _listCategoryEditableBies, ApplicantComplianceCategoryData categoryData)
        {
            if (_listCategoryEditableBies.IsNotNull() && categoryData.IsNotNull())
            {
                var assignmentProperty = _listCategoryEditableBies
                       .FirstOrDefault(editableBy => editableBy.CategoryId == categoryData.ComplianceCategoryID);
                if (assignmentProperty.IsNotNull())
                {
                    return assignmentProperty.IsExceptionNotAllowed;
                }
            }
            return false;
        }

        public ComplianceDetailsContract(CompliancePackage clientCompliancePackage, PackageSubscription packageSubscription, List<Int32> expItemList, String incompleteItemName,
                                         List<ListCategoryEditableBies> lstEditableByData, List<Int32> expPendingItemLst, List<ItemSeriesItemContract> lstItemSeriesItems,
                                         Dictionary<Int32, Boolean> lstComplncRqdMapping, bool isNeedToHideDocumentLink, List<Int32> expNotApprdItemLst, Dictionary<Int32, String> dicCatExplanatoryNotes, String EnterRequirementText, List<ItemPaymentContract> itemPaymentList, String optionalCategoryClientSetting, Boolean IsDisabledBothCategoryAndItemExceptionsForTenant, List<ListCategoryEditableBies> _listCategoryEditableBies)
        {
            this._listEditableByData = lstEditableByData;
            Subscription = packageSubscription;
            ComplianceDetails = null;
            if (clientCompliancePackage.IsNull() || clientCompliancePackage.CompliancePackageCategories.IsNull())
            {
                return;
            }
            ComplianceDetails = new List<ComplianceDetail>();
            var categories = clientCompliancePackage.CompliancePackageCategories.Where(cpc => !cpc.CPC_IsDeleted && cpc.ComplianceCategory.IsActive).OrderBy(ordr => ordr.CPC_DisplayOrder);

            #region UAT-3083
            Boolean IsSubscriptionExpiredOrArchived = false;
            if (packageSubscription.lkpArchiveState.AS_Code != ArchiveState.Active.GetStringValue() || packageSubscription.ExpiryDate < DateTime.Now)
            {
                IsSubscriptionExpiredOrArchived = true;
            }
            #endregion

            foreach (CompliancePackageCategory category in categories)
            {
                ApplicantComplianceCategoryData categoryData = GetCategoryData(category.CPC_CategoryID);
                var _showAddException = true;
                var _showAddRequirements = true;
                //var _showItemEditDelete = true;
                #region UAT-1607:
                List<ItemSeriesItemContract> tempItemSeriesContractlst = lstItemSeriesItems.Where(cnd => cnd.ComplianceCategoryID == category.CPC_CategoryID).ToList();

                #endregion

                if (ShowAddButton(category, categoryData, tempItemSeriesContractlst, expItemList, expPendingItemLst, expNotApprdItemLst))
                    _showAddException = _showAddRequirements = true;
                else
                    _showAddException = _showAddRequirements = false;

                List<String> code = new List<string>();
                code.Add(ApplicantCategoryComplianceStatus.Approved.GetStringValue());
                code.Add(ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue());
                var _showApplyException = false;
                //UAT 3189 As a student, I should be able to apply for a category exception when all items in the category have been entered.   
                if (categoryData.IsNotNull() && !code.Contains(categoryData.lkpCategoryComplianceStatu.Code) && !_showAddRequirements)
                {
                    _showApplyException = true;
                }
                String exceptionExpiredCode = lkpCategoryExceptionStatus.EXCEPTION_EXPIRED.GetStringValue();
                if (IsExceptionNotAllowed(_listCategoryEditableBies, categoryData) || IsDisabledBothCategoryAndItemExceptionsForTenant || (categoryData.IsNotNull() && categoryData.lkpCategoryExceptionStatu.IsNotNull() && categoryData.lkpCategoryExceptionStatu.CES_Code != exceptionExpiredCode))
                {
                    _showApplyException = false;
                }
                String catExplanatoryNotes = dicCatExplanatoryNotes.ContainsKey(category.CPC_CategoryID) ? dicCatExplanatoryNotes[category.CPC_CategoryID] : string.Empty;
                ComplianceDetails.Add(new ComplianceDetail(category.ComplianceCategory, categoryData, _showAddRequirements, _showAddException, incompleteItemName, category.CPC_PackageID, lstComplncRqdMapping, isNeedToHideDocumentLink, catExplanatoryNotes, EnterRequirementText, itemPaymentList, optionalCategoryClientSetting, _showApplyException));

                if (categoryData.IsNotNull() && category.ComplianceCategory.ComplianceCategoryItems.IsNotNull())
                {
                    GetComplianceItems(category, categoryData, expItemList, packageSubscription.lkpPackageComplianceStatu.Code, expPendingItemLst, tempItemSeriesContractlst, lstComplncRqdMapping, isNeedToHideDocumentLink, EnterRequirementText, itemPaymentList, IsSubscriptionExpiredOrArchived);
                }
            }
        }

        /// <summary>
        /// Check if the 'Enter Requirements' & 'Apply for Expception' buttons can be enabled
        /// </summary>
        /// <param name="category"></param>
        /// <param name="categoryData"></param>
        /// <returns></returns>
        private Boolean ShowAddButton(CompliancePackageCategory category, ApplicantComplianceCategoryData categoryData, List<ItemSeriesItemContract> tempItemSeriesContractLst
                                      , List<Int32> expItemList, List<Int32> expPendingItemList, List<Int32> expNotApprdItemLst)
        {
            Int32 _submittedItemCount = 0;
            //UAT-1607:
            Boolean isSeriesAvailablePostApproval = false;
            if (!categoryData.IsNullOrEmpty()) // Submitted items count will be zero in case no category data is entered, else get the count
            {
                foreach (var itemData in categoryData.ApplicantComplianceItemDatas.Where(x => (x.lkpItemComplianceStatu == null
                                                                                   || (x.lkpItemComplianceStatu != null
                                                                                       && x.lkpItemComplianceStatu.Code != ApplicantItemComplianceStatus.Expired.GetStringValue())
                                                                                       )))
                {
                    if (!itemData.IsDeleted)
                    {
                        List<ListCategoryEditableBies> _temp = this._listEditableByData.Where(x => x.ComplianceItemId == itemData.ComplianceItemID
                              && x.CategoryId == categoryData.ComplianceCategoryID).ToList();
                        if (!_temp.IsNullOrEmpty() && (_temp.Count > 0))
                        {
                            //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                            //Added ItemDataEntry check to include those items which is not editable by applicant but allowed for ItemDataEntry.
                            if (_temp.Any(x => x.EditableByCode == LkpEditableBy.Applicant || x.ItemDataEntry == false))
                            {
                                _submittedItemCount++;
                            }
                        }
                    }
                }
            }

            Int32 _countItemsCanBeEdited = GetEditableItemsCount(category);

            //UAT-2028:Expired items should also show in the Enter Requirements item selection dropdown on the student screen
            List<Int32> lstCategoryItems = new List<Int32>();
            List<Int32> lstAllExpItemList = new List<Int32>();
            lstAllExpItemList.AddRange(expItemList);
            lstAllExpItemList.AddRange(expPendingItemList);
            lstAllExpItemList.AddRange(expNotApprdItemLst);
            if (!category.IsNullOrEmpty() && !category.ComplianceCategory.IsNullOrEmpty() && !category.ComplianceCategory.ComplianceCategoryItems.IsNullOrEmpty())
            {
                lstCategoryItems = category.ComplianceCategory.ComplianceCategoryItems.Where(cnd => !cnd.CCI_IsDeleted && cnd.CCI_IsActive).Select(slct => slct.CCI_ItemID).ToList();
            }
            var expItemListForCategory = lstAllExpItemList.Where(x => lstCategoryItems.Contains(x));
            //UAT-1607:
            isSeriesAvailablePostApproval = IsSeriesAvailablePostApproval(categoryData, tempItemSeriesContractLst, expItemList);

            //UAT-1607: Added a check (tempItemSeriesContract.IsNullOrEmpty() || !tempItemSeriesContract.IsSeriesAvailablePostApproval) [UAT-1607]
            // If NO item can be edited 
            if (_countItemsCanBeEdited == 0 && !isSeriesAvailablePostApproval && expItemListForCategory.IsNullOrEmpty())
                return false;
            else
            {
                //Added a check (tempItemSeriesContract.IsNullOrEmpty() || !tempItemSeriesContract.IsSeriesAvailablePostApproval) [UAT-1607]
                // If count of items for which data has been entered equals count of items that can be edited, then disable, else enable
                if (_submittedItemCount >= _countItemsCanBeEdited && !isSeriesAvailablePostApproval && expItemListForCategory.IsNullOrEmpty())
                    return false;
                else
                    return true;
            }
        }

        public ApplicantComplianceCategoryData GetCategoryData(Int32 CategoryID)
        {
            if (Subscription.ApplicantComplianceCategoryDatas == null)
                return null;
            return Subscription.ApplicantComplianceCategoryDatas.FirstOrDefault(x => x.ComplianceCategoryID == CategoryID && !x.IsDeleted);
        }

        public ApplicantComplianceItemData GetItemData(ApplicantComplianceCategoryData categoryData, Int32 itemID)
        {
            String exceptionExpiredCode = lkpCategoryExceptionStatus.EXCEPTION_EXPIRED.GetStringValue();
            String approvedByOverrideCode = lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue();
            String approvedByOverrideCodeDisable = lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE_DISABLE.GetStringValue();
            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
            if (categoryData == null || categoryData.ApplicantComplianceItemDatas == null)
                return null;
            return categoryData.ApplicantComplianceItemDatas.FirstOrDefault(x => x.ComplianceItemID == itemID && (!x.IsDeleted ||
                (x.ComplianceItem.Code == wholeCatGUID && categoryData.CategoryExceptionStatusID != null
                && categoryData.lkpCategoryExceptionStatu.CES_Code != exceptionExpiredCode//"AAAC"
                && categoryData.lkpCategoryExceptionStatu.CES_Code != approvedByOverrideCode
                 && categoryData.lkpCategoryExceptionStatu.CES_Code != approvedByOverrideCodeDisable))); //"AAAD"
        }

        private void GetComplianceItems(CompliancePackageCategory category, ApplicantComplianceCategoryData categoryData, List<Int32> expItemList, String packageComplianceStatus,
                                        List<Int32> expPendingItemLst, List<ItemSeriesItemContract> tempItemSeriesContractLst, Dictionary<Int32, Boolean> lstComplncRqdMapping, bool isNeedToHideDocument, String EnterRequirementText, List<ItemPaymentContract> itemPaymentList, Boolean IsSubscriptionExpiredOrArchived)
        {
            String exceptionExpiredCode = lkpCategoryExceptionStatus.EXCEPTION_EXPIRED.GetStringValue();
            String approvedByOverrideCode = lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue();
            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
            var ccItems = category.ComplianceCategory.ComplianceCategoryItems.Where(cci => (!cci.CCI_IsDeleted && cci.ComplianceItem.IsActive)
                || (cci.ComplianceItem.Code == wholeCatGUID && categoryData.CategoryExceptionStatusID != null
                && categoryData.lkpCategoryExceptionStatu.CES_Code != exceptionExpiredCode && categoryData.lkpCategoryExceptionStatu.CES_Code != approvedByOverrideCode))
                .OrderBy(x => x.CCI_DisplayOrder);

            foreach (ComplianceCategoryItem ccItem in ccItems.Where(cci => cci.ComplianceItem.Code == wholeCatGUID))
            {
                ApplicantComplianceItemData itemData = GetItemData(categoryData, ccItem.ComplianceItem.ComplianceItemID);
                if (itemData != null)
                    ComplianceDetails.Add(new ComplianceDetail(ccItem, itemData, expItemList, this._listEditableByData, packageComplianceStatus, expPendingItemLst, category.CPC_PackageID, tempItemSeriesContractLst, lstComplncRqdMapping, isNeedToHideDocument, EnterRequirementText, itemPaymentList, IsSubscriptionExpiredOrArchived));
            }

            foreach (ComplianceCategoryItem ccItem in ccItems.Where(cci => cci.ComplianceItem.Code != wholeCatGUID))
            {
                ApplicantComplianceItemData itemData = GetItemData(categoryData, ccItem.ComplianceItem.ComplianceItemID);
                if (itemData != null)
                    ComplianceDetails.Add(new ComplianceDetail(ccItem, itemData, expItemList, this._listEditableByData, packageComplianceStatus, expPendingItemLst, category.CPC_PackageID, tempItemSeriesContractLst, lstComplncRqdMapping, isNeedToHideDocument, EnterRequirementText, itemPaymentList, IsSubscriptionExpiredOrArchived));
            }
        }

        /// <summary>
        /// Returns count of items of the category that can be edited
        /// </summary>
        /// <param name="category"></param>
        private Int32 GetEditableItemsCount(CompliancePackageCategory category)
        {
            Int32 countItemsCanBeEdited = 0;
            var lstItems = this._listEditableByData
                   .Where(editableBy => editableBy.CategoryId == category.CPC_CategoryID).ToList();
            //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
            List<Int32> alreadyProcessedItemIds = new List<Int32>();

            foreach (var item in lstItems)
            {
                if (item.EditableByCode == LkpEditableBy.Applicant && !alreadyProcessedItemIds.Contains(item.ComplianceItemId)
                    || item.ItemDataEntry == false && !alreadyProcessedItemIds.Contains(item.ComplianceItemId)
                    )
                {
                    countItemsCanBeEdited++;
                    //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                    //Add complianceItem id in already processedItemIds list to restrict duplicate item id count.
                    if (!alreadyProcessedItemIds.Contains(item.ComplianceItemId))
                    {
                        alreadyProcessedItemIds.Add(item.ComplianceItemId);
                    }
                }
            }
            return countItemsCanBeEdited;
        }

        #region UAT-1607:
        private Boolean IsSeriesAvailablePostApproval(ApplicantComplianceCategoryData categoryData, List<ItemSeriesItemContract> tempItemSeriesContractLst, List<Int32> expItemList)
        {
            Boolean isSeriesAvailablePostApproval = false;
            Boolean isAnyCategoryItemIsPending = false;
            String itemApprovedStatusCode = ApplicantItemComplianceStatus.Approved.GetStringValue();
            String itemApprovedWithExceptionStatusCode = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
            List<Int32> itemSeriesItemIds = new List<Int32>();
            if (!categoryData.IsNullOrEmpty() && !categoryData.ApplicantComplianceItemDatas.IsNullOrEmpty())
            {
                if (!tempItemSeriesContractLst.IsNullOrEmpty())
                {
                    foreach (var itm in tempItemSeriesContractLst)
                    {
                        isAnyCategoryItemIsPending = categoryData.ApplicantComplianceItemDatas.Any(cnd => itm.ComplianceItemID.Contains(cnd.ComplianceItemID) && !cnd.IsDeleted
                                                                                                  && ((expItemList.Contains(cnd.ComplianceItemID))
                                                                                                      || (cnd.lkpItemComplianceStatu != null
                                                                                                      && cnd.lkpItemComplianceStatu.Code != itemApprovedStatusCode
                                                                                                      && cnd.lkpItemComplianceStatu.Code != itemApprovedWithExceptionStatusCode)
                                                                                                     )
                                                                                                  );
                        if (!isAnyCategoryItemIsPending)
                        {
                            if (categoryData.ApplicantComplianceItemDatas.Any(x => itm.ComplianceItemID.Contains(x.ComplianceItemID)) && itm.IsSeriesAvailablePostApproval)
                            {
                                isSeriesAvailablePostApproval = itm.IsSeriesAvailablePostApproval;
                                break;
                            }
                        }
                        else
                        {
                            isSeriesAvailablePostApproval = true;
                        }

                    }
                }
            }
            return isSeriesAvailablePostApproval;
        }
        #endregion
    }

    public class ComplianceDetail
    {
        #region Private Variables

        private Boolean _showItemEditDelete = true;
        private Boolean _showItemDelete = true; //UAT-3511
        private Boolean _showAddException;
        private Boolean _showAddRequirement;
        private Boolean _showApplyException;
        private List<ListCategoryEditableBies> _listEditableByContract;
        private Boolean _showEnterData = false;
        private Boolean _isItemAboutToExpire = false;
        private Boolean _isItemExpired = false;
        private String _updateButtonText = "Update";
        private String _updateExceptionText = "Update";
        private bool _isNeedToHideDocumentLink = false;
        private Boolean _OptionalCategoryClientSetting { get; set; } //UAT 3106
        #endregion

        #region Public Properties

        //UAT-2413
        public String EnterRequirementText
        {
            get;
            set;
        }

        #region UAT-2609
        public Int32 CategoryId { get; set; }
        public Boolean HideExceptionLinkForApprovedCat { get; set; }
        public Boolean IsUpdateButtonNeedForException { get; set; }

        #endregion

        public String EnterRequirementToolTip { get; set; }

        public int ApplicantComplianceItemId { get; set; }

        public String NodeID { get; set; }

        public String ParentNodeID { get; set; }

        public string ExceptionReason { get; set; }

        public String Name { get; set; }

        public String Notes { get; set; }

        public String ExplanatoryNotes { get; set; }

        public String ReviewStatus
        {
            get;
            set;
        }

        public String ReviewStatusCode
        {
            get;
            set;
        }

        public String ImgReviewStatus
        {
            get;
            set;
        }

        public String ImageReviewStatus
        {
            get;
            set;
        }

        public Boolean IsParent
        {
            get;
            set;
        }

        public String AttributeHtml { get; set; }

        public ComplianceCategory Category { get; set; }

        public ComplianceItem Item { get; set; }

        public Boolean ShowExceptionEditDelete { get; set; }
        public Boolean ShowExceptionAllTimeUpdate{ get; set; }

        /// <summary>
        /// Check if data is entered for any item of the category or not
        /// </summary>
        public Boolean IsCategoryDataEntered { get; set; }

        #region UAT-1386:Add submitted date for each item to the student data entry screen
        public String SubmissionDate { get; set; }
        #endregion

        #endregion

        /// <summary>
        /// Used to show/hide 'Enter Requirements' on the data entry screen
        /// </summary>
        public Boolean ShowAddRequirement
        {
            get { return _showAddRequirement; }
            set { _showAddRequirement = value; }
        }
        public Boolean ShowApplyException
        {
            get { return _showApplyException; }
            set { _showApplyException = value; }
        }
        /// <summary>
        /// Used to show/hide 'Apply Exceptions' on the data entry screen
        /// </summary>
        public Boolean ShowAddException
        {
            get { return _showAddException; }
            set { _showAddException = value; }
        }

        /// <summary>
        /// Used to Manage normal Item 'Update' & 'Delete' buttons 
        /// </summary>
        public Boolean ShowItemEditDelete
        {
            get { return _showItemEditDelete; }
            set { _showItemEditDelete = value; }
        }

        //UAT-3511
        public Boolean ShowItemDelete
        {
            get { return _showItemDelete; }
            set { _showItemDelete = value; }
        }

        public Boolean ShowEnterData
        {
            get { return _showEnterData; }
            set { _showEnterData = value; }
        }

        public String UpdateExceptionText
        {
            get { return _updateExceptionText; }
            set { _updateExceptionText = value; }
        }

        public String AttributeHtmlItem { get; set; }

        public String AttributeHtmlDescription { get; set; }

        public String VerificationComments { get; set; }

        public String StatusComments { get; set; }

        /// <summary>
        /// Property to identify whether the category is Required or not, 
        /// based on the Compliance Required and Date Range for Compliance Required.
        /// </summary>
        public Boolean IsComplianceRequired
        {
            get;
            set;
        }
        #region UAT-1607:
        public Boolean IsSeriesItem { get; set; }
        #endregion

        public Boolean IsPaymentItem { get; set; }

        public Boolean IsQuizItem { get; set; }

        public String GetAttributeHtml(List<ApplicantComplianceAttributeData> attributesData, String itemPaymentHtml)
        {
            if (attributesData == null || attributesData.Count == 0 && String.IsNullOrEmpty(itemPaymentHtml)) return String.Empty;

            else if (attributesData == null || attributesData.Count == 0 && !String.IsNullOrEmpty(itemPaymentHtml))
            {
                StringBuilder sbItemPaymentOnly = new StringBuilder();
                sbItemPaymentOnly.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                sbItemPaymentOnly.Append(itemPaymentHtml);
                sbItemPaymentOnly.Append("</table>");
                return sbItemPaymentOnly.ToString();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
            // String attributeHtml = "<table border='0' cellpadding='0' cellspacing='0'>";
            if (!String.IsNullOrEmpty(itemPaymentHtml))
            {
                sb.Append(itemPaymentHtml);
            }
            foreach (ApplicantComplianceAttributeData attributeData in attributesData)
            {
                String _attributeValue = attributeData.AttributeValue;

                if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower())
                {
                    try
                    {
                        Int32 _attId = attributeData.ComplianceAttributeID;

                        if (attributeData.ComplianceAttribute.ComplianceAttributeOptions == null)
                            _attributeValue = String.Empty;
                        else
                        {
                            ComplianceAttributeOption attrOption = attributeData.ComplianceAttribute.ComplianceAttributeOptions.Where(opt => opt.ComplianceItemAttributeID == _attId && opt.OptionValue == _attributeValue && !opt.IsDeleted).FirstOrDefault();
                            if (attrOption.IsNotNull())
                                _attributeValue = attrOption.OptionText;
                            else
                                _attributeValue = String.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                {
                    if (!_isNeedToHideDocumentLink)
                    {
                        try
                        {
                            Int32 atributeDocumentCount = attributeData.ApplicantComplianceDocumentMaps.Where(doc => !doc.IsDeleted).Count();
                            _attributeValue = atributeDocumentCount + " document(s)";
                            var documentDetailData = attributeData.ApplicantComplianceDocumentMaps.Where(mappedDocs => !mappedDocs.IsDeleted).Select(x => x.ApplicantDocument);
                            String fileName = String.Empty;
                            String fileDescription = String.Empty;

                            //attributeHtml += String.Format("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'><span class='attr_val details_viewer' _p='{2}' _n='{3}' _i='{4}' _a='{0}' title='View document list' >{1}</span></td></tr>", !String.IsNullOrEmpty(attributeData.ComplianceAttribute.AttributeLabel) ? attributeData.ComplianceAttribute.AttributeLabel + ":" : attributeData.ComplianceAttribute.Name + ":", _attributeValue, ParentNodeID, NodeID, Name) + "</table>";
                            sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'><span class='attr_val details_viewer' _p='{2}' _n='{3}' _i='{4}' _a='{0}' title='View document list' >{1}</span></td></tr>", !String.IsNullOrEmpty(attributeData.ComplianceAttribute.AttributeLabel) ? attributeData.ComplianceAttribute.AttributeLabel + ":" : attributeData.ComplianceAttribute.Name + ":", _attributeValue, ParentNodeID, NodeID, Name);
                            if (atributeDocumentCount > 0)
                            {
                                GetDocumentDescriptionDetails(documentDetailData);
                            }

                            //AttributeHtml = attributeHtml;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower())
                {
                    try
                    {
                        ///UAT: 4663
                        if (ApplicantItemComplianceStatus.Expired.GetStringValue().ToLower() == attributeData.ApplicantComplianceItemData.lkpItemComplianceStatu.Code.ToLower() && Convert.ToInt32( _attributeValue) == Convert.ToInt32(AppConsts.ZERO.ToString()) )
                        {
                            _attributeValue = attributeData.ApplicantComplianceItemData.lkpItemComplianceStatu.Description;
                        }
                       else if (_attributeValue == AppConsts.ZERO)
                        {
                            _attributeValue = AppConsts.NO;
                        }
                        else if (_attributeValue == AppConsts.ONE.ToString())
                        {
                            _attributeValue = AppConsts.YES;
                        }
                        else
                        {
                            _attributeValue = "N/A";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.Signature.GetStringValue().ToLower())
                {
                    try
                    {
                        string value = "No";
                        if (!string.IsNullOrEmpty(_attributeValue) && _attributeValue.ToLower() == "true")
                            value = "Yes";

                        _attributeValue = value;
                    }
                    catch (Exception ex)
                    {
                    }
                }
                //UAT-1738:Create new attribute type for data-synced documents and update data sync procedure
                else if (String.Compare(attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code, ComplianceAttributeDatatypes.Screening_Document.GetStringValue(),
                         true) == AppConsts.NONE)
                {
                    var documentDetailData = attributeData.ApplicantComplianceDocumentMaps.Where(mappedDocs => !mappedDocs.IsDeleted);

                    sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>", !String.IsNullOrEmpty(attributeData.ComplianceAttribute.AttributeLabel) ? attributeData.ComplianceAttribute.AttributeLabel + ":" : attributeData.ComplianceAttribute.Name + ":");
                    if (!documentDetailData.IsNullOrEmpty())
                    {
                        documentDetailData.ForEach(docMap =>
                        {
                            sb.AppendFormat("<a href='#' style='color:#0000ff;'  onclick=\"ViewScreeningDoc('"
                                             + docMap.ApplicantDocument.ApplicantDocumentID
                                             + "')\" >{0}</a></br>", docMap.ApplicantDocument.FileName);
                        });
                    }
                    sb.Append("</td></tr>");

                }
                else
                {
                    //if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Name.Equals(ComplianceAttributeDatatypes.Date.ToString())
                    if (String.Compare(attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code, ComplianceAttributeDatatypes.Date.GetStringValue(), true)
                         == AppConsts.NONE
                        && !String.IsNullOrEmpty(_attributeValue))
                    {
                        try
                        {
                            _attributeValue = Convert.ToDateTime(_attributeValue).ToShortDateString();
                        }
                        catch (Exception ex) { }
                    }
                }
                //Added a check of screening document for the implementation of UAT-1738:
                if (!(attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                    &&
                    String.Compare(attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code, ComplianceAttributeDatatypes.Screening_Document.GetStringValue(),
                    true) != AppConsts.NONE
                    )
                {
                    sb.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}</td></tr>", !String.IsNullOrEmpty(attributeData.ComplianceAttribute.AttributeLabel) ? attributeData.ComplianceAttribute.AttributeLabel + ":" : attributeData.ComplianceAttribute.Name + ":", _attributeValue);
                }
            }
            sb.Append("</table>");
            //ShowDetailDocument = "display: " + (ShowDetailDocument.Equals(String.Empty) ? "none" : ShowDetailDocument);
            return sb.ToString();
        }


        public String GetItemPaymentHtml(String amount, Boolean isPaid, String orderStatus, String pkgName, String categoryName, String itemID, String categoryID, String itemName, String pkgId, String pkgSubscriptionId, String orderID, String orderNumber, String totalPrice, String invoiceNumber, String organizationUserProfileID, String paymentStatusCode, Boolean IsSubscriptionExpiredOrArchived)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>Amount: </td><td class='td-three'>$" + amount + "</td></tr>");

            if (isPaid)
            {
                sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Order Number:</td><td class='td-three'>" + orderNumber + "</td></tr>");
                sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Payment Status:</td><td class='td-three'>" + orderStatus + "</td></tr>");
            }
            else
            {
                if (!String.IsNullOrEmpty(pkgSubscriptionId))
                {
                    sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Order Number:</td><td class='td-three'>" + orderNumber + "</td></tr>");
                    sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Payment Status:</td><td class='td-three'>");
                    sb.AppendFormat(" " + orderStatus);
                    //OSCNL
                    if (!paymentStatusCode.Equals(ApplicantOrderStatus.Cancelled.GetStringValue()))
                    {
                        if (!IsSubscriptionExpiredOrArchived)
                        {
                            sb.AppendFormat("<a href='#' class='completePaymentLink' style='color:#0000ff;'  onclick=\"ItemPaymentClick('"
                                                      + pkgName + "','" + categoryName + "','" + itemID + "','" + categoryID + "','" + itemName + "','" + pkgId + "','" + pkgSubscriptionId + "','" + orderID + "','" + orderNumber + "','" + totalPrice + "','" + invoiceNumber + "','" + AppConsts.FALSE + "','" + organizationUserProfileID + "','" + AppConsts.ZERO + "','" + AppConsts.ONE + "','" + AppConsts.ZERO + "')\" >{0}</a></br>", " (Complete your payment)");
                        }
                    }
                    sb.AppendFormat("</td></tr>");
                }
                else
                    sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>Payment Status:</td><td class='td-three'>" + "Pending Payment" + "</td></tr>");
            }
            return sb.ToString();
        }
        public String GetImagePathOfCategoryReviewStatus(String reviewStatus, String CategoryExceptionStatusCode, String ruleStatus)
        {
            //UAT -3106
            if (!this.IsComplianceRequired)
            {
                if (!this._OptionalCategoryClientSetting)
                {
                    return "~/Resources/Mod/Compliance/icons/optional.png";
                }
                else if (ruleStatus.Trim() != AppConsts.STR_ONE)
                {
                    return "~/Resources/Mod/Compliance/icons/optional.png";
                }
                else if (ruleStatus.Trim() == AppConsts.STR_ONE)
                {
                    return "~/Resources/Mod/Compliance/icons/yes16.png";
                }
            }

            if (!String.IsNullOrEmpty(reviewStatus))
            {
                if (!String.IsNullOrEmpty(CategoryExceptionStatusCode) && CategoryExceptionStatusCode == "AAAD" && ApplicantCategoryComplianceStatus.Approved.GetStringValue().Equals(reviewStatus))
                    return "~/Resources/Mod/Compliance/icons/yesx16.png";
                else if (ApplicantCategoryComplianceStatus.Approved.GetStringValue().Equals(reviewStatus))
                    return "~/Resources/Mod/Compliance/icons/yes16.png";
                else if (ApplicantCategoryComplianceStatus.Incomplete.GetStringValue().Equals(reviewStatus))
                    return "~/Resources/Mod/Compliance/icons/no16.png";
                else if (ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue().Equals(reviewStatus))
                    return "~/Resources/Mod/Compliance/icons/compliance_exception.png";
                else if (ApplicantCategoryComplianceStatus.Pending_Review.GetStringValue().Equals(reviewStatus))
                    return "~/Resources/Mod/Compliance/icons/attn16.png";
            }
            return "~/Resources/Mod/Compliance/icons/no16.png";
        }

        #region UAT-1413:Add note to data entry screen when student has editable upcoming expiration item
        /// <summary>
        /// Used to Manage normal Item about to expire 'Update' & 'Delete' buttons 
        /// </summary>
        public Boolean IsItemAboutToExpire
        {
            get { return _isItemAboutToExpire; }
            set { _isItemAboutToExpire = value; }
        }

        public Boolean IsItemExpired
        {
            get { return _isItemExpired; }
            set { _isItemExpired = value; }
        }

        public String UpdateButtonText
        {
            get { return _updateButtonText; }
            set { _updateButtonText = value; }
        }

        #endregion

        public ComplianceDetail()
        {

        }

        public ComplianceDetail(ComplianceCategory category, ApplicantComplianceCategoryData categoryData, Boolean isAddRequirementsVisible
                                , Boolean isAddExceptionVisible, String incompleteItemName, Int32 packageId, Dictionary<Int32, Boolean> LstComplncRqdMapping, Boolean isNeedToHideDocument, String catExplanatoryNotes, String EnterRequirementText, List<ItemPaymentContract> itemPaymentList, String OptionalCategoryClientSetting, Boolean isApplyExceptionVisible)
        {

            this.IsComplianceRequired = IsCatComplianceRequired(packageId, category, LstComplncRqdMapping);

            this._OptionalCategoryClientSetting = OptionalCategoryClientSetting == "1" ? true : false;//UAT 3106

            if (IsComplianceRequired)
            {
                this.ParentNodeID = INTSOF.Utils.AppConsts.DATA_ENTRY_REQUIRED_CATEGORY_NODE;
            }
            else
            {
                this.ParentNodeID = INTSOF.Utils.AppConsts.DATA_ENTRY_OPTIONAL_CATEGORY_NODE; ;
            }

            this.IsParent = true;
            this.NodeID = String.Format("P_{0}", category.ComplianceCategoryID);
            this.Category = category;
            this.Name = !String.IsNullOrEmpty(category.CategoryLabel) ? category.CategoryLabel : category.CategoryName;
            this.ExplanatoryNotes = catExplanatoryNotes;
            this.ShowAddRequirement = isAddRequirementsVisible;
            this.ShowAddException = isAddExceptionVisible;
            this._isNeedToHideDocumentLink = isNeedToHideDocument;
            this.ShowApplyException = isApplyExceptionVisible;
            //UAT-2413
            this.EnterRequirementText = EnterRequirementText;
            if (EnterRequirementText == AppConsts.ENTER_REQUIREMENT_TEXT)
            {
                this.EnterRequirementToolTip = "Click to enter data for a requirement in this category";
            }

            //UAT-2609
            this.CategoryId = category.ComplianceCategoryID;
            this.IsUpdateButtonNeedForException = true;

            //Implemented for UAT-1386:Add submitted date for each item to the student data entry screen
            this.SubmissionDate = String.Empty;

            if (categoryData == null)
            {
                this.Notes = String.Empty;
                this.ReviewStatus = incompleteItemName;
                this.ImgReviewStatus = incompleteItemName;
                this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus(ApplicantCategoryComplianceStatus.Incomplete.GetStringValue(), String.Empty, String.Empty);
                this.ReviewStatusCode = "INCM";
                //UAT-2609
                this.HideExceptionLinkForApprovedCat = false;
            }
            else
            {
                this.Notes = categoryData.Notes;
                String CategoryExceptionStatusCode = categoryData.lkpCategoryExceptionStatu == null ? String.Empty : categoryData.lkpCategoryExceptionStatu.CES_Code.ToString();
                //Changes as per UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                if (categoryData.lkpCategoryExceptionStatu.IsNotNull() && categoryData.lkpCategoryExceptionStatu.CES_Code == "AAAA"
                    && categoryData.lkpCategoryComplianceStatu.Code == "INCM")
                {
                    //Changes as per UAT-819: WB: Category Exception enhancements
                    this.ReviewStatus = "Pending Review"; //this.ReviewStatus = "Applied For Exception"; 
                    this.ImgReviewStatus = "Pending Review"; //categoryData.lkpCategoryComplianceStatu.Name;
                    this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus("PNDR", String.Empty, String.Empty); // GetImagePathOfCategoryReviewStatus(categoryData.lkpCategoryComplianceStatu.Code);
                    this.ReviewStatusCode = categoryData.lkpCategoryComplianceStatu.Code;
                    //UAT-2609
                    this.HideExceptionLinkForApprovedCat = false;
                }
                else
                {
                    if (categoryData.lkpCategoryExceptionStatu != null && categoryData.lkpCategoryExceptionStatu.CES_Code == "AAAD" && categoryData.lkpCategoryComplianceStatu.Code == "APRD")
                    {
                        this.ReviewStatus = "Approved Override";
                        this.ImgReviewStatus = "Approved Override";
                    }
                    else
                    {
                        this.ReviewStatus = categoryData.lkpCategoryComplianceStatu.Name;
                        this.ImgReviewStatus = categoryData.lkpCategoryComplianceStatu.Name;
                    }
                    this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus(categoryData.lkpCategoryComplianceStatu.Code, CategoryExceptionStatusCode, categoryData.RulesStatusID.ToString());
                    this.ReviewStatusCode = categoryData.lkpCategoryComplianceStatu.Code;
                    //UAT-2609
                    if (categoryData.lkpCategoryComplianceStatu.Code == "APRD" || categoryData.lkpCategoryComplianceStatu.Code == "APWE")
                    {
                        this.HideExceptionLinkForApprovedCat = true;
                    }
                    else
                    {
                        this.HideExceptionLinkForApprovedCat = false;
                    }
                }
            }

            if (categoryData.IsNullOrEmpty() ||
                (!categoryData.IsNullOrEmpty()
                    && (categoryData.ApplicantComplianceItemDatas.IsNullOrEmpty() ||
                        categoryData.ApplicantComplianceItemDatas.Where(itemData => !itemData.IsDeleted).Count() == 0))
                )
            {
                this.IsCategoryDataEntered = false;
            }
            else
                this.IsCategoryDataEntered = true;
        }

        public ComplianceDetail(ComplianceCategoryItem item, ApplicantComplianceItemData itemData, List<Int32> expItemList, List<ListCategoryEditableBies> lstEditableByData
            , String packageComplianceStatus, List<Int32> expPendingItemLst, Int32 packageId, List<ItemSeriesItemContract> tempItemSeriesContractlst, Dictionary<Int32, Boolean> lstComplncRqdMapping, Boolean isNeedToHideDocument, String EnterRequirementText, List<ItemPaymentContract> itemPaymentList, Boolean IsSubscriptionExpiredOrArchived)
        {
            this._listEditableByContract = lstEditableByData;
            this.ParentNodeID = String.Format("P_{0}", item.CCI_CategoryID);
            this.IsParent = false;
            this.NodeID = item.CCI_ItemID.ToString();
            this.Category = item.ComplianceCategory;
            this.Item = item.ComplianceItem;
            this.Name = !String.IsNullOrEmpty(item.ComplianceItem.ItemLabel) ? item.ComplianceItem.ItemLabel : item.ComplianceItem.Name;
            this._isNeedToHideDocumentLink = isNeedToHideDocument;
            //UAT-2609
            this.IsUpdateButtonNeedForException = true;

            //UAT-2413
            this.EnterRequirementText = EnterRequirementText;
            if (EnterRequirementText == AppConsts.ENTER_REQUIREMENT_TEXT)
            {
                this.EnterRequirementToolTip = "Click to enter data for a requirement in this category";
            }
            ExceptionReason = itemData.ExceptionReason;

            this.IsComplianceRequired = IsCatComplianceRequired(packageId, item.ComplianceCategory, lstComplncRqdMapping);


            //Implemented for UAT-1386:Add submitted date for each item to the student data entry screen
            if (!itemData.IsNullOrEmpty())
            {
                this.SubmissionDate = "Submitted Date: " + (itemData.SubmissionDate.IsNullOrEmpty() ? String.Empty : itemData.SubmissionDate.Value.ToShortDateString());
            }

            ApplicantComplianceItemId = itemData.ApplicantComplianceItemID;
            //this.ExplanatoryNotes = item.ExplanatoryNotes;
            //if (itemData == null || itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Incomplete.GetStringValue())
            // Item status can be incomplete when it is copied from one package to another
            // & data for some attributes may not be copied with mapping. So incomplete data is to be displayed. So new check is applied
            if (itemData == null)
            {
                this.Notes = this.AttributeHtml = String.Empty;
                this.ReviewStatus = ApplicantItemComplianceStatus.Incomplete.ToString();
                this.ReviewStatusCode = ApplicantItemComplianceStatus.Incomplete.GetStringValue();
            }
            else
            {

                this.Notes = itemData.Notes;

                //UAT-3028: Update "Pending Review" statuses for applicants

                if (itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Pending_Review.GetStringValue())
                {
                    this.ReviewStatus = "Pending ADB Review";
                }
                else if (itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue())
                {
                    this.ReviewStatus = "Pending Institution Review";
                }
                else if (itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue())
                {
                    this.ReviewStatus = "Exception Pending School Review";
                }
                else
                {
                    this.ReviewStatus = itemData.lkpItemComplianceStatu.Name;
                }
                this.ReviewStatusCode = itemData.lkpItemComplianceStatu.Code;
                #region UAT-1413:Add note to data entry screen when student has editable upcoming expiration item
                //Commented below code for UAT-1866:As an applicant, I should always be able to update items where there is an item expiration 
                //if ((expItemList.Count > 0 || expPendingItemLst.Count > 0)
                //     && (expItemList.Contains(itemData.ComplianceItemID) || expPendingItemLst.Contains(itemData.ComplianceItemID)))
                //{
                //    this.IsItemAboutToExpire = true;
                //    this.UpdateButtonText = "Expiring";
                //}
                if (itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Expired.GetStringValue())
                {
                    this.IsItemExpired = true;
                    this.UpdateButtonText = "Expired";
                }
                #endregion

                var attributeData = new List<ApplicantComplianceAttributeData>();
                var attributeDataTemp = itemData.ApplicantComplianceAttributeDatas.Where(x => !x.IsDeleted).ToList();
                if (attributeDataTemp != null && attributeDataTemp.Count > 0)
                {
                    foreach (var attDataItem in attributeDataTemp)
                    {
                        var attributeDataItem = attDataItem;
                        Int32 attID = attDataItem.ComplianceAttribute.ComplianceAttributeID;
                        Int32 displayOrder = attDataItem.ComplianceAttribute.ComplianceItemAttributes.FirstOrDefault(x => x.CIA_AttributeID == attID && x.CIA_ItemID == item.CCI_ItemID).CIA_DisplayOrder;
                        attributeDataItem.DisplayOrder = displayOrder;
                        if (attDataItem.ComplianceAttribute.IsActive)
                            attributeData.Add(attributeDataItem);
                    }

                    attributeData = attributeData.OrderBy(x => x.DisplayOrder).ThenBy(x => x.ComplianceAttributeID).ToList();
                }

                if (item.ComplianceItem.IsPaymentType.Value && !itemData.IsNullOrEmpty()) //UAT-3077
                {
                    var itemOrderData = itemPaymentList.Where(x => x.CategoryID == item.CCI_CategoryID && x.ItemID == item.CCI_ItemID).FirstOrDefault();
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
                        if (OrderStatusCode == ApplicantOrderStatus.Paid.GetStringValue()) //  "OSPAD"
                        {
                            IsPaid = true;
                            TotalPrice = Convert.ToString(Math.Round(itemOrderData.TotalPrice, 2, MidpointRounding.AwayFromZero));
                        }
                        else
                            TotalPrice = Convert.ToString(Math.Round(itemOrderData.PaidAmount, 2, MidpointRounding.AwayFromZero));

                        if (itemData.lkpItemComplianceStatu.Code != ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue()
                            && itemData.lkpItemComplianceStatu.Code != ApplicantItemComplianceStatus.Exception_Approved.GetStringValue()
                            && itemData.lkpItemComplianceStatu.Code != ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue()
                            && itemData.lkpItemComplianceStatu.Code != ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue()
                            && !isExceptionExpired(itemData))
                            this.AttributeHtmlItem = GetAttributeHtml(attributeData, GetItemPaymentHtml(TotalPrice, IsPaid, OrderStatus, PkgName, CategoryName, ItemID, CategoryID, ItemName, PkgId, PkgSubscriptionId, OrderID, OrderNumber, TotalPrice, InvoiceNumber, OrganizationUserProfileID, OrderStatusCode, IsSubscriptionExpiredOrArchived));
                        else
                            this.AttributeHtmlItem = GetAttributeHtml(attributeData, string.Empty);
                    }
                    else
                    {
                        if (itemData.lkpItemComplianceStatu.Code != ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue()
                            && itemData.lkpItemComplianceStatu.Code != ApplicantItemComplianceStatus.Exception_Approved.GetStringValue()
                            && itemData.lkpItemComplianceStatu.Code != ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue()
                            && itemData.lkpItemComplianceStatu.Code != ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue()
                            && !isExceptionExpired(itemData)
                            )
                            this.AttributeHtmlItem = GetAttributeHtml(attributeData, GetItemPaymentHtml(String.Concat(Convert.ToString(Math.Round(item.ComplianceItem.Amount.Value, 2, MidpointRounding.AwayFromZero))), false, OrderStatus, PkgName, CategoryName, ItemID, CategoryID, ItemName, PkgId, PkgSubscriptionId, OrderID, OrderNumber, AppConsts.ZERO, InvoiceNumber, OrganizationUserProfileID, OrderStatusCode, IsSubscriptionExpiredOrArchived));
                        else
                            this.AttributeHtmlItem = GetAttributeHtml(attributeData, string.Empty);
                    }
                }
                else
                    this.AttributeHtmlItem = GetAttributeHtml(attributeData, String.Empty);

                if (!String.IsNullOrEmpty(itemData.VerificationComments))
                {
                    //this.VerificationComments = itemData.VerificationComments.Replace(Environment.NewLine, "<br/>");
                    var verificationComments = itemData.VerificationComments.Replace(Environment.NewLine, "<br/>");
                    //To show only last reviewer’s comment to student 
                    var listVerificationComments = verificationComments.Split(new String[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries);
                    this.VerificationComments = listVerificationComments.LastOrDefault();
                }

                if (!String.IsNullOrEmpty(itemData.StatusComments))
                {
                    this.StatusComments = itemData.StatusComments.Replace(Environment.NewLine, "<br/>");
                }

                if (itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Approved.GetStringValue()
                    || itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue()
                    || itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()
                    || itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()
                    || !IsItemEditableByApplicant(itemData.ComplianceItemID, itemData.ApplicantComplianceCategoryData.ComplianceCategoryID))
                {
                    this.ShowItemEditDelete = false;
                    this.ShowItemDelete = false; //UAT-3511
                }
                if ((itemData.ApplicantComplianceCategoryData.lkpCategoryComplianceStatu.Code == ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                        || itemData.ApplicantComplianceCategoryData.lkpCategoryComplianceStatu.Code == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue())
                    && (itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue()
                        || itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Expired.GetStringValue()))
                {
                    this.IsUpdateButtonNeedForException = false;
                }


                if (((expItemList.Count > 0 && expItemList.Contains(itemData.ComplianceItemID))
                    ||
                        ((itemData.lkpItemComplianceStatu.IsNotNull() && itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()) &&
                        itemData.lkpItemMovementType.IsNotNull()
                         && (
                              itemData.lkpItemMovementType.Code == LkpItemMovementStatus.VIA_DATA_ENTRY_FROM_INCOMPLETE.GetStringValue()
                              || itemData.lkpItemMovementType.Code == LkpItemMovementStatus.VIA_DATA_ENTRY_FROM_REJECTED.GetStringValue()
                              || itemData.lkpItemMovementType.Code == LkpItemMovementStatus.VIA_VERIFICATION_FROM_INCOMPLETE_REVIEW_NOT_REQUIRED.GetStringValue()
                              || itemData.lkpItemMovementType.Code == LkpItemMovementStatus.VIA_EXPIRED.GetStringValue()
                              //Below chack is used to identify the data entry of item in case item status is set to 'Pending Review For Client' [UAT-1049:Admin Data Entry]
                              || itemData.lkpItemMovementType.Code == LkpItemMovementStatus.VIA_ADMIN_DATA_ENTRY.GetStringValue()
                            //|| (itemData.lkpItemMovementType.Code == LkpItemMovementStatus.VIA_DATA_ENTRY_FROM_PENDING_REVIEW_FOR_CLIENT_ADMIN.GetStringValue() && !item.ComplianceItem.IsNullOrEmpty() && item.ComplianceItem.IsPaymentType.Value)
                            )
                        )
                    )
                    //Commented code as per UAT-761: Update link not appearing for students' expiring items
                    //&& packageComplianceStatus != ApplicantPackageComplianceStatus.Compliant.GetStringValue()// MASTER CONDITION FOR ANY CASE WHEN UPDATE/DELETE SHOULD BE ALLOWED
                    && IsItemEditableByApplicant(itemData.ComplianceItemID, itemData.ApplicantComplianceCategoryData.ComplianceCategoryID))
                // MASTER CONDITION FOR ANY CASE WHEN UPDATE/DELETE SHOULD BE ALLOWED
                {
                    this.ShowItemEditDelete = true;
                    this.ShowItemDelete = false; //UAT-3511
                }

                //UAT 3299 Hide update and delete button when item status is incomplete and movement type is Quiz evaluation 
                if (itemData.lkpItemComplianceStatu.IsNotNull() && itemData.lkpItemMovementType.IsNotNull() && (
                    itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Incomplete.GetStringValue()
                    && itemData.lkpItemMovementType.Code == LkpItemMovementStatus.VIA_QUIZ_EVALUATION.GetStringValue()))
                {
                    this.IsQuizItem = true;
                    this.ShowItemEditDelete = false;
                    this.ShowItemDelete = false; //UAT-3511
                }

                //Show update and delete link for exception item expired.
                if (itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() ||
                    itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue() ||
                    isExceptionExpired(itemData))
                {
                    // Further check in case of exception, if applicant can delete the item or not
                    if (IsItemEditableByApplicant(itemData.ComplianceItemID, itemData.ApplicantComplianceCategoryData.ComplianceCategoryID))
                    {
                        ShowExceptionEditDelete = true;
                        ShowItemEditDelete = false;
                        ShowItemDelete = false; //UAT-3511
                    }
                }
                //UAT-1264: As a student, I should be able to enter the data for an item (or submit a new exception) when an item exception expires
                if (isExceptionExpired(itemData) && IsItemEditableByApplicant(itemData.ComplianceItemID, itemData.ApplicantComplianceCategoryData.ComplianceCategoryID))
                {
                    ShowEnterData = true;
                    UpdateExceptionText = "Update Exception Request";
                }
                //ShowAddExceptionRequirement = false;

                // Hide the 'Add requirements' and 'Exception' button when data is already added for the items
                ShowAddRequirement = false;
                ShowAddException = false;

                //if (!String.IsNullOrEmpty(ExceptionReason))
                //    this.AttributeHtml = String.Format("{0} document(s) attached.", itemData.ExceptionDocumentMappings.Where(mappedDocs => !mappedDocs.IsDeleted).Count());

                if (!String.IsNullOrEmpty(ExceptionReason) && !_isNeedToHideDocumentLink)
                {
                    Int32 atributeDocumentCount = itemData.ExceptionDocumentMappings.Where(mappedDocs => !mappedDocs.IsDeleted).DistinctBy(x => x.ApplicantDocumentID).Count();
                    this.AttributeHtml = String.Format("<table border='0' cellpadding='0' cellspacing='0'><tr class='clickable'><td class='td-one'></td><td class='td-two'>Documents:</td><td class='td-three'><span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='Document(s)' title='View document list' >{0} document(s)</span></td></tr></table>", atributeDocumentCount, ParentNodeID, NodeID, Name);
                    var documentDetailData = itemData.ExceptionDocumentMappings.Where(mappedDocs => !mappedDocs.IsDeleted).DistinctBy(x => x.ApplicantDocumentID).Select(x => x.ApplicantDocument);

                    if (atributeDocumentCount > 0)
                    {
                        GetDocumentDescriptionDetails(documentDetailData);
                    }
                }

                #region UAT-4926
                if (IsEnableUpdateAllTimeForItem(itemData.ComplianceItemID, itemData.ApplicantComplianceCategoryData.ComplianceCategoryID) && IsItemEditableByApplicant(itemData.ComplianceItemID, itemData.ApplicantComplianceCategoryData.ComplianceCategoryID))
                {
                    if (itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() ||
                   itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue() ||
                   itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue() ||
                   isExceptionExpired(itemData))
                    {
                        // ShowExceptionEditDelete = true;
                        ShowExceptionAllTimeUpdate = true;
                    }
                    else
                    {
                        ShowItemEditDelete = true;
                    }

                }
                #endregion

                #region UAT-1607:
                List<Int32> itemSeriesItemIds = new List<Int32>();
                if (!tempItemSeriesContractlst.IsNullOrEmpty())
                {
                    tempItemSeriesContractlst.ForEach(itm =>
                    {
                        itemSeriesItemIds.AddRange(itm.ComplianceItemID);
                    });
                }
                if (itemSeriesItemIds.Contains(item.CCI_ItemID))
                {
                    this.IsSeriesItem = true;
                }
                #endregion

                //UAT-3077
                if (item.ComplianceItem.IsPaymentType.Value)
                {
                    this.IsPaymentItem = true;
                }


            }
        }
        /// <summary>
        /// Check for IsEnableUpdateAllTime Setting 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private Boolean IsEnableUpdateAllTimeForItem(Int32 itemId, Int32 categoryId)
        {
            var lstItems = this._listEditableByContract
                  .Where(isEnabled => isEnabled.CategoryId == categoryId && isEnabled.ComplianceItemId == itemId).ToList();
            if (lstItems.IsNotNull() && lstItems.Count > 0)
            {
                if (lstItems.Any(enable => enable.IsEnableUpdateAllTime))
                    return true;
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Check whether exception item is expired.
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns></returns>
        private Boolean isExceptionExpired(ApplicantComplianceItemData itemData)
        {
            if (itemData.IsNotNull() && itemData.lkpItemComplianceStatu.Code == ApplicantItemComplianceStatus.Expired.GetStringValue() && itemData.lkpItemMovementType.IsNull())
            {
                return true;
            }
            return false;
        }

        private void GetDocumentDescriptionDetails(IEnumerable<ApplicantDocument> documentDetailData)
        {
            StringBuilder documentDescription = new StringBuilder();
            documentDescription.Append(@"<table border='0' cellpadding='0' id='documentDescription" + ParentNodeID + "-" + NodeID + "' cellspacing='0' style='display:none;'>");
            documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two'><b>Name</b></td><td class='td-three'><b>Description</b></td><td class='td-four'><b>Preview Document</b></td></tr>");
            foreach (var documentDetail in documentDetailData)
            {
                String anchorID = documentDetail.ApplicantDocumentID + "_" + Category.ComplianceCategoryID + "_" + Item.ComplianceItemID;
                documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}</td><td class='td-four'><a id='{3}' href='#' FileName='{0}' DocumentID='{2}'>View Document</a></td></tr>", documentDetail.FileName, documentDetail.Description, documentDetail.ApplicantDocumentID, anchorID);
            }
            //documentDescription.AppendFormat("<tr><td class='td-one'></td><td class='td-two'></td><td onclick=\"ShowHideDetails('" + ParentNodeID + "','" + NodeID + "')\" style='font-size:x-small; text-align:right; cursor:pointer' class='td-three'><u>Hide</u></td></tr>");
            documentDescription.AppendFormat("</table>");
            AttributeHtmlDescription = documentDescription.ToString();
        }

        /// <summary>
        /// Returns whether the Item Edit/Delete buttons should be visible or not for the applicant, based on the Assignment properties
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private Boolean IsItemEditableByApplicant(Int32 itemId, Int32 categoryId)
        {
            var lstItems = this._listEditableByContract
                  .Where(editableBy => editableBy.CategoryId == categoryId && editableBy.ComplianceItemId == itemId).ToList();

            //UAT-2418:Student's are able to update item after rejection even when editable by does not include applicants. 
            //if (lstItems.IsNotNull() && lstItems.Any(eb => String.IsNullOrEmpty(eb.EditableByCode)))
            if (lstItems.IsNotNull() && lstItems.Count > 0)
            {
                if (lstItems.Any(edit => edit.EditableByCode == LkpEditableBy.Applicant))
                    return true;
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns whether the Current category is Optional or not, 
        /// based on the Compliance Required and Date Range for Compliance Required.
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private Boolean IsCatComplianceRequired(Int32 packageId, ComplianceCategory category, Dictionary<Int32, Boolean> LstComplncRqdMapping)
        {

            CompliancePackageCategory currentCategory = category.CompliancePackageCategories.Where(cpc => cpc.CPC_PackageID == packageId
                                                                                              && cpc.CPC_IsDeleted == false)
                                                                                          .First();

            Boolean _isRequired = LstComplncRqdMapping.FirstOrDefault(x => x.Key == currentCategory.CPC_ID).Value;


            //DateTime _crntDateTime = DateTime.Now;

            //if ((currentCategory.CPC_ComplianceRqdStartDate.IsNull() && currentCategory.CPC_ComplianceRqdEndDate.IsNull())
            //    ||
            //    ((_crntDateTime.Month > Convert.ToDateTime(currentCategory.CPC_ComplianceRqdStartDate).Month
            //       || (
            //           _crntDateTime.Month == Convert.ToDateTime(currentCategory.CPC_ComplianceRqdStartDate).Month
            //          && _crntDateTime.Date >= Convert.ToDateTime(currentCategory.CPC_ComplianceRqdStartDate).Date

            //       ))
            //       && (_crntDateTime.Month < Convert.ToDateTime(currentCategory.CPC_ComplianceRqdEndDate).Month
            //       || (
            //            _crntDateTime.Month == Convert.ToDateTime(currentCategory.CPC_ComplianceRqdEndDate).Month
            //            && _crntDateTime.Date <= Convert.ToDateTime(currentCategory.CPC_ComplianceRqdEndDate).Date
            //            )
            //       || (
            //            _crntDateTime.Month > Convert.ToDateTime(currentCategory.CPC_ComplianceRqdEndDate).Month
            //            && Convert.ToDateTime(currentCategory.CPC_ComplianceRqdEndDate).Month < Convert.ToDateTime(currentCategory.CPC_ComplianceRqdStartDate).Month
            //            )
            //       ))
            //     )
            //{
            //    return _isRequired;
            //}
            //else if (currentCategory.CPC_ComplianceRqdStartDate.IsNotNull()
            //     && currentCategory.CPC_ComplianceRqdEndDate.IsNotNull()
            //     && (
            //        (
            //           _crntDateTime.Month < Convert.ToDateTime(currentCategory.CPC_ComplianceRqdStartDate).Month
            //            || (
            //            _crntDateTime.Month == Convert.ToDateTime(currentCategory.CPC_ComplianceRqdStartDate).Month
            //            && _crntDateTime.Date < Convert.ToDateTime(currentCategory.CPC_ComplianceRqdStartDate).Date
            //            )
            //    || (
            //        _crntDateTime.Month > Convert.ToDateTime(currentCategory.CPC_ComplianceRqdEndDate).Month
            //        || (
            //            _crntDateTime.Month == Convert.ToDateTime(currentCategory.CPC_ComplianceRqdEndDate).Month
            //            && _crntDateTime.Date > Convert.ToDateTime(currentCategory.CPC_ComplianceRqdEndDate).Date
            //            )
            //        )
            //        )
            //    )
            //    )
            //{
            //    _isRequired = !_isRequired;
            //}

            return _isRequired;
        }
    }

    public class ComplianceSaveResponse
    {
        public String UIValidationErrors { get; set; }
        public String StatusCode { get; set; }
        public Int32? StatusId { get; set; }
        public ApplicantComplianceItemData ItemData { get; set; }
        public Boolean SaveStatus { get; set; }
    }
}
