using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class SharedUserInvitationDetailContarct
    {
        public PackageSubscription Subscription { get; set; }
        public List<SharedInvitationComplianceDetail> SharedInvitationComplianceDetails { get; set; }

        public SharedUserInvitationDetailContarct(CompliancePackage clientCompliancePackage, PackageSubscription packageSubscription, String incompleteItemName, List<Int32> SharedComplainceCategoryIdList, Boolean optionalCategoryClientSettingValue)
        {
            Subscription = packageSubscription;
            SharedInvitationComplianceDetails = null;
            if (clientCompliancePackage.IsNull() || clientCompliancePackage.CompliancePackageCategories.IsNull())
            {
                return;
            }
            SharedInvitationComplianceDetails = new List<SharedInvitationComplianceDetail>();
            var categories = clientCompliancePackage.CompliancePackageCategories.Where(cpc => !cpc.CPC_IsDeleted && cpc.ComplianceCategory.IsActive && SharedComplainceCategoryIdList.Contains(cpc.CPC_CategoryID)).OrderBy(ordr => ordr.CPC_DisplayOrder);//&& SharedComplainceCategoryIdList.Contains(cpc.CPC_CategoryID)
            foreach (CompliancePackageCategory category in categories)
            {
                ApplicantComplianceCategoryData categoryData = GetCategoryData(category.CPC_CategoryID);

                SharedInvitationComplianceDetails.Add(new SharedInvitationComplianceDetail(category.ComplianceCategory, categoryData, incompleteItemName, category.CPC_PackageID, optionalCategoryClientSettingValue));

                if (categoryData.IsNotNull() && category.ComplianceCategory.ComplianceCategoryItems.IsNotNull())
                {
                    GetComplianceItems(category, categoryData, packageSubscription.lkpPackageComplianceStatu.Code, packageSubscription.PackageSubscriptionID);
                }
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
            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
            if (categoryData == null || categoryData.ApplicantComplianceItemDatas == null)
                return null;
            return categoryData.ApplicantComplianceItemDatas.FirstOrDefault(x => x.ComplianceItemID == itemID && (!x.IsDeleted ||
                (x.ComplianceItem.Code == wholeCatGUID && categoryData.CategoryExceptionStatusID != null
                 && categoryData.lkpCategoryExceptionStatu.CES_Code != exceptionExpiredCode//"AAAC"
                 && categoryData.lkpCategoryExceptionStatu.CES_Code != approvedByOverrideCode))); //"AAAD"
        }

        private void GetComplianceItems(CompliancePackageCategory category, ApplicantComplianceCategoryData categoryData, String packageComplianceStatus, Int32 packageSubscription)
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
                    SharedInvitationComplianceDetails.Add(new SharedInvitationComplianceDetail(ccItem, itemData, packageSubscription, category.CPC_PackageID));
            }

            foreach (ComplianceCategoryItem ccItem in ccItems.Where(cci => cci.ComplianceItem.Code != wholeCatGUID))
            {
                ApplicantComplianceItemData itemData = GetItemData(categoryData, ccItem.ComplianceItem.ComplianceItemID);
                if (itemData != null)
                    SharedInvitationComplianceDetails.Add(new SharedInvitationComplianceDetail(ccItem, itemData, packageSubscription, category.CPC_PackageID));
            }
        }

        #region Get Compliance Data From Snapshot
        public SharedUserInvitationDetailContarct(DataSet immunizationData, String incompleteItemName, List<lkpCategoryExceptionStatu> lstCategoryExceptionStatus,
                                                   List<lkpCategoryComplianceStatu> lstCategoryComplianceStatus, List<lkpPackageComplianceStatu> lstPackageCompStatus,
                         List<lkpItemComplianceStatu> lstItemComplianceStatus, List<lkpComplianceAttributeDatatype> lstComplianceAttributeDatatType, Boolean optionalCategoryClientSettingValue)
        {
            DataTable packageSubscriptionData = immunizationData.Tables[0];
            DataTable packageCategoriesData = immunizationData.Tables[1];
            DataTable categoryItemdata = immunizationData.Tables[2];
            Int32 packageSubscriptionId = Convert.ToInt32(packageSubscriptionData.Rows[0]["PackageSubscriptionID"]);

            Int32 packageCompStatusId = (packageSubscriptionData.Rows[0]["PackageStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(packageSubscriptionData.Rows[0]["PackageStatusID"]);
            var packageCompStatus = lstPackageCompStatus.FirstOrDefault(cnd => cnd.PackageComplianceStatusID == packageCompStatusId);

            SharedInvitationComplianceDetails = null;
            if ((packageSubscriptionData.IsNull()) || packageCategoriesData.IsNull())
            {
                return;
            }
            SharedInvitationComplianceDetails = new List<SharedInvitationComplianceDetail>();
            var categoryData = packageCategoriesData.AsEnumerable().OrderBy(ordr => ordr["CategoryDisplayOrder"]);//&& SharedComplainceCategoryIdList.Contains(cpc.CPC_CategoryID)
            foreach (DataRow category in categoryData)
            {
                //ApplicantComplianceCategoryData categoryData = GetCategoryData(category.CPC_CategoryID);

                SharedInvitationComplianceDetails.Add(new SharedInvitationComplianceDetail(category, categoryItemdata, incompleteItemName, lstCategoryExceptionStatus,
                                                                                           lstCategoryComplianceStatus, optionalCategoryClientSettingValue));

                if (categoryData.IsNotNull() && category.IsNotNull())
                {
                    GetComplianceItemsFromSnapshot(category, categoryItemdata, immunizationData, packageCompStatus.Code, packageSubscriptionId, lstCategoryExceptionStatus,
                                       lstCategoryComplianceStatus, lstItemComplianceStatus, lstComplianceAttributeDatatType);
                }
            }
        }

        private void GetComplianceItemsFromSnapshot(DataRow categoryDatarow, DataTable categoryItemData, DataSet immunizationData, String packageComplianceStatus, Int32 packageSubscription,
                                        List<lkpCategoryExceptionStatu> lstCategoryExceptionStatus, List<lkpCategoryComplianceStatu> lstCategoryComplianceStatus,
                                        List<lkpItemComplianceStatu> lstItemComplianceStatus, List<lkpComplianceAttributeDatatype> lstComplianceAttributeDataType
                                       )
        {
            String exceptionExpiredCode = lkpCategoryExceptionStatus.EXCEPTION_EXPIRED.GetStringValue();
            String approvedByOverrideCode = lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue();
            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
            Int32 categoryId = (categoryDatarow["CategoryID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(categoryDatarow["CategoryID"]);
            Int32 compStatusId = (categoryDatarow["CategoryStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(categoryDatarow["CategoryStatusID"]);
            Int32 catExcepStatusId = (categoryDatarow["CategoryExceptionStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(categoryDatarow["CategoryExceptionStatusID"]);

            var catComplianceStatus = lstCategoryComplianceStatus.FirstOrDefault(cnd => cnd.CategoryComplianceStatusID == compStatusId && !cnd.IsDeleted);
            var catExceptionStatus = lstCategoryExceptionStatus.FirstOrDefault(cnd => cnd.CES_ID == catExcepStatusId && !cnd.CES_IsDeleted);

            //Get Category Exception Record 
            var ccExceptionItems = categoryItemData.AsEnumerable().Where(cci => (Convert.ToString(cci["ItemCode"]) == AppConsts.WHOLE_CATEGORY_GUID
                                                               && catExceptionStatus != null
                                                               && catExceptionStatus.CES_Code != exceptionExpiredCode && catExceptionStatus.CES_Code != approvedByOverrideCode)
                                                               && Convert.ToString(cci["PackageCategoryID"]) == Convert.ToString(categoryDatarow["PackageCategoryID"]))
                                                               .OrderBy(x => x["ItemDisplayOrder"]);

            //For category level exception 
            foreach (DataRow itemCatException in ccExceptionItems)
            {
                if (itemCatException["ItemDataID"].GetType().Name != "DBNull")
                    SharedInvitationComplianceDetails.Add(new SharedInvitationComplianceDetail(itemCatException, immunizationData, categoryDatarow, packageSubscription, categoryId, lstItemComplianceStatus, lstComplianceAttributeDataType));
            }

            foreach (DataRow itemData in categoryItemData.AsEnumerable().Where(x => Convert.ToString(x["PackageCategoryID"]) == Convert.ToString(categoryDatarow["PackageCategoryID"])
                                                                 && Convert.ToString(x["ItemCode"]) != AppConsts.WHOLE_CATEGORY_GUID).OrderBy(ordr => ordr["ItemDisplayOrder"])
                    )
            {
                //ApplicantComplianceItemData itemData = GetItemData(categoryData, ccItem.ComplianceItem.ComplianceItemID);
                if (itemData["ItemDataID"].GetType().Name != "DBNull")
                    SharedInvitationComplianceDetails.Add(new SharedInvitationComplianceDetail(itemData, immunizationData, categoryDatarow, packageSubscription, categoryId, lstItemComplianceStatus, lstComplianceAttributeDataType));
            }
        }
        #endregion
    }

    public class SharedInvitationComplianceDetail
    {
        #region Public Properties

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

        //public Boolean ShowExceptionEditDelete { get; set; }

        /// <summary>
        /// Check if data is entered for any item of the category or not
        /// </summary>
        public Boolean IsCategoryDataEntered { get; set; }

        #endregion

        public String AttributeHtmlItem { get; set; }

        public String AttributeHtmlDescription { get; set; }

        //public String StatusComments { get; set; }

        public Int32 FirstItemDocumentId { get; set; }

        /// <summary>
        /// Property to identify whether the category is Required or not, 
        /// based on the Compliance Required and Date Range for Compliance Required.
        /// </summary>
        public Boolean IsComplianceRequired
        {
            get;
            set;
        }

        public Boolean OptionalCategoryClientSetting
        {
            get;
            set;
        }

        public String GetAttributeHtml(List<ApplicantComplianceAttributeData> attributesData, Int32 packageSubscriptionId)
        {
            if (attributesData == null || attributesData.Count == 0) return String.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
            //String attributeHtml = "<table border='0' cellpadding='0' cellspacing='0'>";

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
                else if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                {
                    try
                    {
                        Int32 atributeDocumentCount = attributeData.ApplicantComplianceDocumentMaps.Where(doc => !doc.IsDeleted).Count();
                        _attributeValue = atributeDocumentCount + " document(s)";
                        var documentDetailData = attributeData.ApplicantComplianceDocumentMaps.Where(mappedDocs => !mappedDocs.IsDeleted).Select(x => x.ApplicantDocument);
                        String fileName = String.Empty;
                        String fileDescription = String.Empty;

                        sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>", !String.IsNullOrEmpty(attributeData.ComplianceAttribute.AttributeLabel) ? attributeData.ComplianceAttribute.AttributeLabel + ":" : attributeData.ComplianceAttribute.Name + ":");

                        if (atributeDocumentCount > 0)
                        {
                            foreach (var documentDetail in documentDetailData)
                            {
                                if (this.FirstItemDocumentId == AppConsts.NONE)
                                {
                                    this.FirstItemDocumentId = documentDetail.ApplicantDocumentID;
                                }
                                sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}' _ps='{4}'  onclick=\"ShowDocument('" + documentDetail.ApplicantDocumentID + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span></br>", documentDetail.FileName, ParentNodeID, NodeID, Convert.ToString(documentDetail.ApplicantDocumentID), Convert.ToString(packageSubscriptionId));
                            }
                        }
                        sb.Append("</td></tr>");

                    }
                    catch (Exception ex)
                    {
                    }
                }
                //Regarding UAT-1738: adding new attribute type: screening document.
                //If sharing is done by applicant.
                else if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.Screening_Document.GetStringValue().ToLower())
                {
                    try
                    {
                        Int32 atributeDocumentCount = attributeData.ApplicantComplianceDocumentMaps.Where(doc => !doc.IsDeleted).Count();
                        _attributeValue = atributeDocumentCount + " document(s)";
                        var documentDetailData = attributeData.ApplicantComplianceDocumentMaps.Where(mappedDocs => !mappedDocs.IsDeleted).Select(x => x.ApplicantDocument);
                        String fileName = String.Empty;
                        String fileDescription = String.Empty;

                        sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>", !String.IsNullOrEmpty(attributeData.ComplianceAttribute.AttributeLabel) ? attributeData.ComplianceAttribute.AttributeLabel + ":" : attributeData.ComplianceAttribute.Name + ":");

                        if (atributeDocumentCount > 0)
                        {
                            foreach (var documentDetail in documentDetailData)
                            {
                                sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}' _ps='{4}'  onclick=\"ViewScreeningDocument('" + documentDetail.ApplicantDocumentID + "')\" >{0}</span></br>", documentDetail.FileName, ParentNodeID, NodeID, Convert.ToString(documentDetail.ApplicantDocumentID), Convert.ToString(packageSubscriptionId));
                            }
                        }
                        sb.Append("</td></tr>");

                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower())
                {
                    try
                    {
                        Int32 atributeDocumentCount = attributeData.ApplicantComplianceDocumentMaps.Where(doc => !doc.IsDeleted).Count();
                        _attributeValue = atributeDocumentCount + " document(s)";
                        var documentDetailData = attributeData.ApplicantComplianceDocumentMaps.Where(mappedDocs => !mappedDocs.IsDeleted).Select(x => x.ApplicantDocument);
                        String fileName = String.Empty;
                        String fileDescription = String.Empty;

                        sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>", !String.IsNullOrEmpty(attributeData.ComplianceAttribute.AttributeLabel) ? attributeData.ComplianceAttribute.AttributeLabel + ":" : attributeData.ComplianceAttribute.Name + ":");

                        if (atributeDocumentCount > 0)
                        {
                            foreach (var documentDetail in documentDetailData)
                            {
                                if (this.FirstItemDocumentId == AppConsts.NONE)
                                {
                                    this.FirstItemDocumentId = documentDetail.ApplicantDocumentID;
                                }
                                sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}' _ps='{4}'  onclick=\"ShowDocument('" + documentDetail.ApplicantDocumentID + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span></br>", documentDetail.FileName, ParentNodeID, NodeID, Convert.ToString(documentDetail.ApplicantDocumentID), Convert.ToString(packageSubscriptionId));
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
                if ((attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() != ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                     && (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() != ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower())
                    && (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() != ComplianceAttributeDatatypes.Screening_Document.GetStringValue().ToLower()))
                {
                    sb.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}</td></tr>", !String.IsNullOrEmpty(attributeData.ComplianceAttribute.AttributeLabel) ? attributeData.ComplianceAttribute.AttributeLabel + ":" : attributeData.ComplianceAttribute.Name + ":", _attributeValue);
                }
            }
            sb.Append("</table>");
            //ShowDetailDocument = "display: " + (ShowDetailDocument.Equals(String.Empty) ? "none" : ShowDetailDocument);
            return sb.ToString();
        }

        public String GetImagePathOfCategoryReviewStatus(String reviewStatus, String CatExceptionStatus = null, String ruleStatus = null)
        {
            //UAT 3106
            if (!this.IsComplianceRequired)
            {
                if (!this.OptionalCategoryClientSetting)
                {
                    return "~/Resources/Mod/Compliance/icons/optional.png";
                }
                else if (!ruleStatus.IsNullOrEmpty() && ruleStatus.Trim() != AppConsts.STR_ONE)
                {
                    return "~/Resources/Mod/Compliance/icons/optional.png";
                }
                else if (!ruleStatus.IsNullOrEmpty() && ruleStatus.Trim() == AppConsts.STR_ONE)
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
                if (CatExceptionStatus.IsNotNull() && CatExceptionStatus == "AAAD" && ApplicantCategoryComplianceStatus.Approved.GetStringValue().Equals(reviewStatus))
                    return "~/Resources/Mod/Compliance/icons/compliance_exception.png";
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

        public SharedInvitationComplianceDetail(ComplianceCategory category, ApplicantComplianceCategoryData categoryData, String incompleteItemName, Int32 packageId, Boolean OptionalCategoryClientSetting)
        {
            this.ParentNodeID = String.Empty;
            this.IsParent = true;
            this.NodeID = String.Format("P_{0}", category.ComplianceCategoryID);
            this.Category = category;
            this.Name = !String.IsNullOrEmpty(category.CategoryLabel) ? category.CategoryLabel : category.CategoryName;

            this.IsComplianceRequired = IsCatComplianceRequired(packageId, category);

            this.OptionalCategoryClientSetting = OptionalCategoryClientSetting;

            if (categoryData == null)
            {
                this.Notes = String.Empty;
                this.ReviewStatus = incompleteItemName;
                this.ImgReviewStatus = incompleteItemName;
                this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus(ApplicantCategoryComplianceStatus.Incomplete.GetStringValue());
            }
            else
            {
                this.Notes = categoryData.Notes;
                //Changes as per UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                if (categoryData.lkpCategoryExceptionStatu.IsNotNull() && categoryData.lkpCategoryExceptionStatu.CES_Code == "AAAA"
                    && categoryData.lkpCategoryComplianceStatu.Code == "INCM")
                {
                    //Changes as per UAT-819: WB: Category Exception enhancements
                    this.ReviewStatus = "Pending Review"; //this.ReviewStatus = "Applied For Exception"; 
                    this.ImgReviewStatus = "Pending Review"; //categoryData.lkpCategoryComplianceStatu.Name;
                    this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus("PNDR"); // GetImagePathOfCategoryReviewStatus(categoryData.lkpCategoryComplianceStatu.Code);
                }
                else
                {
                    if (categoryData.lkpCategoryExceptionStatu.IsNotNull() && categoryData.lkpCategoryExceptionStatu.CES_Code == "AAAD")
                    {
                        this.ImgReviewStatus = "Approved Override";
                        this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus(categoryData.lkpCategoryComplianceStatu.Code, categoryData.lkpCategoryExceptionStatu.CES_Code, Convert.ToString(categoryData.RulesStatusID));
                    }
                    else
                    {
                        this.ImgReviewStatus = categoryData.lkpCategoryComplianceStatu.Name;
                        this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus(categoryData.lkpCategoryComplianceStatu.Code, Convert.ToString(categoryData.RulesStatusID));
                    }
                    this.ReviewStatus = categoryData.lkpCategoryComplianceStatu.Name;

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

        public SharedInvitationComplianceDetail(ComplianceCategoryItem item, ApplicantComplianceItemData itemData, Int32 packageSubscriptionId, Int32 packageId)
        {
            this.ParentNodeID = String.Format("P_{0}", item.CCI_CategoryID);
            this.IsParent = false;
            this.NodeID = item.CCI_ItemID.ToString();
            this.Category = item.ComplianceCategory;
            this.Item = item.ComplianceItem;
            this.Name = !String.IsNullOrEmpty(item.ComplianceItem.ItemLabel) ? item.ComplianceItem.ItemLabel : item.ComplianceItem.Name;


            this.IsComplianceRequired = IsCatComplianceRequired(packageId, item.ComplianceCategory);

            ExceptionReason = itemData.ExceptionReason;
            StringBuilder sb = new StringBuilder();
            ApplicantComplianceItemId = itemData.ApplicantComplianceItemID;
            if (itemData == null)
            {
                this.Notes = this.AttributeHtml = String.Empty;
                this.ReviewStatus = ApplicantItemComplianceStatus.Incomplete.ToString();
            }
            else
            {

                this.Notes = itemData.Notes;
                this.ReviewStatus = itemData.lkpItemComplianceStatu.Name;
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
                        attributeData.Add(attributeDataItem);
                    }

                    attributeData = attributeData.OrderBy(x => x.DisplayOrder).ThenBy(x => x.ComplianceAttributeID).ToList();
                }

                this.AttributeHtmlItem = GetAttributeHtml(attributeData, packageSubscriptionId);

                if (!String.IsNullOrEmpty(ExceptionReason))
                {
                    sb.Append("<table border='0' cellpadding='0' cellspacing='0'><tr class='clickable'><td class='td-one'></td><td class='td-two'>Documents:</td><td class='td-three'>");
                    var exceptionDocument = itemData.ExceptionDocumentMappings.Where(mappedDocs => !mappedDocs.IsDeleted)
                                                     .Select(x => x.ApplicantDocument).ToList();

                    foreach (var documentDetail in exceptionDocument)
                    {
                        if (this.FirstItemDocumentId == AppConsts.NONE)
                        {
                            this.FirstItemDocumentId = documentDetail.ApplicantDocumentID;
                        }
                        sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}' _ps='{4}'  onclick=\"ShowDocument('" + documentDetail.ApplicantDocumentID + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span></br>", documentDetail.FileName, ParentNodeID, NodeID, Convert.ToString(documentDetail.ApplicantDocumentID), Convert.ToString(packageSubscriptionId));
                    }
                    sb.Append("</td></tr></table>");
                    this.AttributeHtml = sb.ToString();
                }
            }
        }

        #region Get Compliance Data From Snapshot
        public SharedInvitationComplianceDetail(DataRow categoryDatarow, DataTable categoryItemData, String incompleteItemName, List<lkpCategoryExceptionStatu> lstCategoryExceptionStatus, List<lkpCategoryComplianceStatu> lstCategoryComplianceStatus, Boolean optionalCategoryClientSettingValue)
        {
            this.IsComplianceRequired = Convert.ToBoolean(categoryDatarow["IsComplianceRequired"]);
            this.ParentNodeID = String.Empty;
            this.IsParent = true;
            this.NodeID = String.Format("P_{0}", categoryDatarow["CategoryID"].ToString());
            #region Assign Category
            //Set Compliance Category
            ComplianceCategory category = new ComplianceCategory();
            category.CategoryName = Convert.ToString(categoryDatarow["CategoryName"]);
            category.ComplianceCategoryID = Convert.ToInt32(categoryDatarow["CategoryID"]);
            category.Description = String.Empty;

            this.Category = category;
            #endregion
            this.Name = !(categoryDatarow["CategoryLabel"].ToString().IsNullOrEmpty()) ? categoryDatarow["CategoryLabel"].ToString() : categoryDatarow["CategoryName"].ToString();
            Int32 compStatusId = (categoryDatarow["CategoryStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(categoryDatarow["CategoryStatusID"]);
            Int32 catExcepStatusId = (categoryDatarow["CategoryExceptionStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(categoryDatarow["CategoryExceptionStatusID"]);
            Int32 CatRuleStatusID = (categoryDatarow["CategoryRuleStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(categoryDatarow["CategoryRuleStatusID"]); //UAT - 3106
            var catComplianceStatus = lstCategoryComplianceStatus.FirstOrDefault(cnd => cnd.CategoryComplianceStatusID == compStatusId && !cnd.IsDeleted);
            var catExceptionStatus = lstCategoryExceptionStatus.FirstOrDefault(cnd => cnd.CES_ID == catExcepStatusId && !cnd.CES_IsDeleted);


            Boolean isItemDataEntered = categoryItemData.AsEnumerable().Any(cnd => cnd["PackageCategoryID"] == categoryDatarow["PackageCategoryID"]);

            this.OptionalCategoryClientSetting = optionalCategoryClientSettingValue;

            if (categoryDatarow["CategoryDataID"].GetType().Name == "DBNull")
            {
                this.Notes = String.Empty;
                this.ReviewStatus = incompleteItemName;
                this.ImgReviewStatus = incompleteItemName;
                this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus(ApplicantCategoryComplianceStatus.Incomplete.GetStringValue());
            }
            else
            {
                this.Notes = categoryDatarow["CategoryNotes"].ToString();
                //Changes as per UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                if (catExceptionStatus.IsNotNull() && catExceptionStatus.CES_Code == "AAAA"
                    && catComplianceStatus.Code == "INCM")
                {
                    //Changes as per UAT-819: WB: Category Exception enhancements
                    this.ReviewStatus = "Pending Review"; //this.ReviewStatus = "Applied For Exception"; 
                    this.ImgReviewStatus = "Pending Review"; //categoryData.lkpCategoryComplianceStatu.Name;
                    this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus("PNDR"); // GetImagePathOfCategoryReviewStatus(categoryData.lkpCategoryComplianceStatu.Code);
                }
                else if (catComplianceStatus.IsNotNull())
                {
                    if (catExceptionStatus.IsNotNull() && catExceptionStatus.CES_Code == "AAAD" && catComplianceStatus.Name == "Approved")
                    {
                        this.ImgReviewStatus = "Approved Override";
                        this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus(catComplianceStatus.Code, catExceptionStatus.CES_Code,CatRuleStatusID.ToString()); //UAT 3106 
                    }
                    else
                    {
                        this.ImgReviewStatus = catComplianceStatus.Name;
                        this.ImageReviewStatus = GetImagePathOfCategoryReviewStatus(catComplianceStatus.Code, null, CatRuleStatusID.ToString()); //UAT 3106
                    }
                    this.ReviewStatus = catComplianceStatus.Name;

                }
            }

            if (categoryDatarow["CategoryDataID"].GetType().Name == "DBNull" ||
                (categoryDatarow["CategoryDataID"].GetType().Name != "DBNull"
                    && (!isItemDataEntered))
                )
            {
                this.IsCategoryDataEntered = false;
            }
            else
                this.IsCategoryDataEntered = true;
        }

        public SharedInvitationComplianceDetail(DataRow itemDataRow, DataSet immunizationData, DataRow categoryData, Int32 packageSubscriptionId, Int32 categoryId, List<lkpItemComplianceStatu> lstItemComplianceStatus, List<lkpComplianceAttributeDatatype> lstComplianceAttDataType)
        {
            DataTable itemAttributeData = immunizationData.Tables[3];
            DataTable attDocdata = immunizationData.Tables[4];
            var exceptionDocData = immunizationData.Tables[5].AsEnumerable().Where(x => Convert.ToString(x["CategoryItemID"]) == Convert.ToString(itemDataRow["CategoryItemID"]));
            Int32 itemId = (itemDataRow["ItemID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(itemDataRow["ItemID"]);
            this.ParentNodeID = String.Format("P_{0}", categoryId);
            this.IsParent = false;
            this.NodeID = itemId.ToString();
            //Set Compliance Category
            ComplianceCategory category = new ComplianceCategory();
            category.CategoryName = Convert.ToString(categoryData["CategoryName"]);
            category.ComplianceCategoryID = Convert.ToInt32(categoryId);
            category.Description = String.Empty;
            this.Category = category;
            //this.Item = item.ComplianceItem;
            this.Name = !(itemDataRow["ItemLabel"].ToString().IsNullOrEmpty()) ? itemDataRow["ItemLabel"].ToString() : itemDataRow["ItemName"].ToString();
            ExceptionReason = itemDataRow["ExceptionReason"].ToString();
            StringBuilder sb = new StringBuilder();
            ApplicantComplianceItemId = (itemDataRow["ItemDataID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(itemDataRow["ItemDataID"]);
            Int32 itemStatusId = (itemDataRow["ItemStatusID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(itemDataRow["ItemStatusID"]);
            var itemComplianceStatus = lstItemComplianceStatus.FirstOrDefault(cnd => cnd.ItemComplianceStatusID == itemStatusId && !cnd.IsDeleted);
            if (itemDataRow["ItemDataID"].GetType().Name == "DBNull")
            {
                this.Notes = this.AttributeHtml = String.Empty;
                this.ReviewStatus = ApplicantItemComplianceStatus.Incomplete.ToString();
            }
            else
            {

                //this.Notes = itemData.Notes;
                this.ReviewStatus = itemComplianceStatus.Name;
                var attributeData = new List<DataRow>();
                var attributeDataTemp = itemAttributeData;
                if (attributeDataTemp != null && attributeDataTemp.Rows.Count > 0)
                {
                    foreach (DataRow attDataItem in attributeDataTemp.AsEnumerable().Where(x => Convert.ToString(x["CategoryItemID"]) == Convert.ToString(itemDataRow["CategoryItemID"]) && x["AttributeDataID"].GetType().Name != "DBNull"))
                    {
                        var attributeDataItem = attDataItem;
                        Int32 displayOrder = 0;
                        String attID = (attDataItem["AttributeID"]).GetType().Name == "DBNull" ? "0" : Convert.ToString(attDataItem["AttributeID"]);
                        var attributeDataItemRecord = itemAttributeData.AsEnumerable().FirstOrDefault(x => Convert.ToString(x["AttributeID"]) == attID && Convert.ToString(x["CategoryItemID"]) == Convert.ToString(itemDataRow["CategoryItemID"]));
                        if (attributeDataItemRecord.IsNotNull() && attributeDataItemRecord["AttributeDisplayOrder"].GetType().Name != "DBNull")
                            displayOrder = Convert.ToInt32(attributeDataItemRecord["AttributeDisplayOrder"]);
                        attributeDataItem["AttributeDisplayOrder"] = displayOrder;
                        attributeData.Add(attributeDataItem);
                    }

                    attributeData = attributeData.OrderBy(x => x["AttributeDisplayOrder"]).ThenBy(x => x["AttributeID"]).ToList();
                }

                this.AttributeHtmlItem = GetAttributeHtmlFromSnapshot(attributeData, attDocdata, packageSubscriptionId, lstComplianceAttDataType);

                if (!String.IsNullOrEmpty(ExceptionReason))
                {
                    sb.Append("<table border='0' cellpadding='0' cellspacing='0'><tr class='clickable'><td class='td-one'></td><td class='td-two'>Documents:</td><td class='td-three'>");

                    foreach (var documentDetail in exceptionDocData)
                    {
                        String fileName = Convert.ToString(documentDetail["FileName"]);
                        Int32 excDocumentId = (documentDetail["ApplicantDocumentID"]).GetType().Name == "DBNull" ? 0 : Convert.ToInt32(documentDetail["ApplicantDocumentID"]);
                        if (this.FirstItemDocumentId == AppConsts.NONE)
                        {
                            this.FirstItemDocumentId = excDocumentId;
                        }
                        sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}' _ps='{4}'  onclick=\"ShowDocument('" + excDocumentId + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span></br>", fileName, ParentNodeID, NodeID, Convert.ToString(excDocumentId), Convert.ToString(packageSubscriptionId));
                    }
                    sb.Append("</td></tr></table>");
                    this.AttributeHtml = sb.ToString();
                }
            }
        }

        public String GetAttributeHtmlFromSnapshot(List<DataRow> attributesData, DataTable attributeDocData, Int32 packageSubscriptionId, List<lkpComplianceAttributeDatatype> lstComplianceAttributeDataType)
        {
            if (attributesData == null || attributesData.Count == 0) return String.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
            String attributeHtml = "<table border='0' cellpadding='0' cellspacing='0'>";

            foreach (DataRow attributeData in attributesData)
            {
                Int32 attributeDataTypeId = (attributeData["AttributeDataTypeID"]).GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(attributeData["AttributeDataTypeID"]);
                var attDataType = lstComplianceAttributeDataType.FirstOrDefault(cnd => cnd.ComplianceAttributeDatatypeID == attributeDataTypeId);
                String _attributeValue = attributeData["AttributeValue"].ToString();
                String attName = !Convert.ToString(attributeData["AttributeLabel"]).IsNullOrEmpty() ? attributeData["AttributeLabel"].ToString() : attributeData["AttributeName"].ToString();

                if (attDataType.Code.ToLower() == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower())
                {
                    try
                    {
                        Int32 _attId = Convert.ToInt32(attributeData["AttributeID"]);

                        if (attributeData["AttributeOptionID"].GetType().Name == "DBNull")
                            _attributeValue = String.Empty;
                        else
                        {
                            //ComplianceAttributeOption attrOption = attributeData.ComplianceAttribute.ComplianceAttributeOptions.Where(opt => opt.ComplianceItemAttributeID == _attId && opt.OptionValue == _attributeValue && !opt.IsDeleted).FirstOrDefault();
                            if (attributeData["AttributeOptionText"].GetType().Name != "DBNull")
                                _attributeValue = attributeData["AttributeOptionText"].ToString();
                            else
                                _attributeValue = String.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (attDataType.Code.ToLower() == ComplianceAttributeDatatypes.Signature.GetStringValue().ToLower())
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
                else if (attDataType.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                {
                    try
                    {
                        //var applicantDocument = attributeDocData.AsEnumerable().Where(x => Convert.ToString(x["ItemAttributeID"]) == Convert.ToString(attributeData["ItemAttributeID"]));
                        var applicantDocument = attributeDocData.AsEnumerable().Where(x => Convert.ToString(x["AttributeDataID"]) == Convert.ToString(attributeData["AttributeDataID"]));
                        String fileDescription = String.Empty;

                        sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>", attName + ":");

                        foreach (var documentDetail in applicantDocument)
                        {
                            String fileName = Convert.ToString(documentDetail["FileName"]);
                            Int32 applicantDocumentId = documentDetail["ApplicantDocumentID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(documentDetail["ApplicantDocumentID"]);
                            if (this.FirstItemDocumentId == AppConsts.NONE)
                            {
                                this.FirstItemDocumentId = applicantDocumentId;
                            }
                            sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}' _ps='{4}'  onclick=\"ShowDocument('" + applicantDocumentId + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span></br>", fileName, ParentNodeID, NodeID, applicantDocumentId, Convert.ToString(packageSubscriptionId));
                        }
                        sb.Append("</td></tr>");
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (attDataType.Code.ToLower() == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower())
                {
                    try
                    {
                        //var applicantDocument = attributeDocData.AsEnumerable().Where(x => Convert.ToString(x["ItemAttributeID"]) == Convert.ToString(attributeData["ItemAttributeID"]));
                        var applicantDocument = attributeDocData.AsEnumerable().Where(x => Convert.ToString(x["AttributeDataID"]) == Convert.ToString(attributeData["AttributeDataID"]));
                        String fileDescription = String.Empty;
                        String IsAttributeValueTrue = String.Empty;

                        if (!_attributeValue.IsNullOrEmpty() && _attributeValue == "1" && applicantDocument.Count() > 0) //UAT-3532
                            IsAttributeValueTrue = " (";

                        sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}" + IsAttributeValueTrue, attName + ":", Convert.ToInt32(_attributeValue) == AppConsts.NONE ? "No" : "Yes");

                        if (!_attributeValue.IsNullOrEmpty() && _attributeValue == "1" && applicantDocument.Count() > 0)  //UAT-3532
                        {
                            foreach (var documentDetail in applicantDocument)
                            {
                                String fileName = Convert.ToString(documentDetail["FileName"]);
                                Int32 applicantDocumentId = documentDetail["ApplicantDocumentID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(documentDetail["ApplicantDocumentID"]);
                                if (this.FirstItemDocumentId == AppConsts.NONE)
                                {
                                    this.FirstItemDocumentId = applicantDocumentId;
                                }
                                sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}' _ps='{4}'  onclick=\"ShowDocument('" + applicantDocumentId + "','" + Convert.ToString(packageSubscriptionId) + "')\" >{0}</span>)</br>", fileName, ParentNodeID, NodeID, applicantDocumentId, Convert.ToString(packageSubscriptionId));
                            }
                        }
                        sb.Append("</td></tr>");
                    }
                    catch (Exception ex)
                    {
                    }
                }
                //Regarding UAT-1738: adding new attribute type: screening document.
                //Getting attribute data from snapshot.
                else if (attDataType.Code.ToLower() == ComplianceAttributeDatatypes.Screening_Document.GetStringValue().ToLower())
                {
                    try
                    {
                        var applicantDocument = attributeDocData.AsEnumerable().Where(x => Convert.ToString(x["AttributeDataID"]) == Convert.ToString(attributeData["AttributeDataID"]));
                        String fileDescription = String.Empty;

                        sb.AppendFormat("<tr class='clickable'><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>", attName + ":");

                        foreach (var documentDetail in applicantDocument)
                        {
                            String fileName = Convert.ToString(documentDetail["FileName"]);
                            Int32 applicantDocumentId = documentDetail["ApplicantDocumentID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(documentDetail["ApplicantDocumentID"]);

                            sb.AppendFormat("<span class='attr_val details_viewer' _p='{1}' _n='{2}' _i='{3}' _a='{0}' _ps='{4}'  onclick=\"ViewScreeningDocument('" + applicantDocumentId + "')\" >{0}</span></br>", fileName, ParentNodeID, NodeID, applicantDocumentId, Convert.ToString(packageSubscriptionId));
                        }
                        sb.Append("</td></tr>");
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    //if (attDataType.Name.Equals(ComplianceAttributeDatatypes.Date.ToString())
                    if (String.Compare(attDataType.Code, ComplianceAttributeDatatypes.Date.GetStringValue(), true) == AppConsts.NONE
                        && !String.IsNullOrEmpty(_attributeValue))
                    {
                        try
                        {
                            _attributeValue = Convert.ToDateTime(_attributeValue).ToShortDateString();
                        }
                        catch (Exception ex) { }
                    }
                }
                if ((attDataType.Code.ToLower() != ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                    && (attDataType.Code.ToLower() != ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower())
                    && (attDataType.Code.ToLower() != ComplianceAttributeDatatypes.Screening_Document.GetStringValue().ToLower()))
                {
                    sb.AppendFormat("<tr><td class='td-one'></td><td class='td-two'>{0}</td><td class='td-three'>{1}</td></tr>", attName + ":", _attributeValue);
                }
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Returns whether the Current category is Optional or not, 
        /// based on the Compliance Required and Date Range for Compliance Required.
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private Boolean IsCatComplianceRequired(Int32 packageId, ComplianceCategory category)
        {
            #region UAT:4237.Change here...

            CompliancePackageCategory currentCategory = category.CompliancePackageCategories.Where(cpc => cpc.CPC_PackageID == packageId
                                                                                              && cpc.CPC_IsDeleted == false)
                                                                                          .First();
            Boolean _isRequired = currentCategory.CPC_ComplianceRequired;
            DateTime _crntDateTime = DateTime.Now;
            DateTime StartDate = Convert.ToDateTime(currentCategory.CPC_ComplianceRqdStartDate).Date;
            DateTime EndDate = Convert.ToDateTime(currentCategory.CPC_ComplianceRqdEndDate).Date;
            DateTime ModifedCurrentDate = DateTime.Now.Date;
            if (currentCategory.CPC_ComplianceRqdStartDate.IsNotNull()
               && currentCategory.CPC_ComplianceRqdEndDate.IsNotNull())
            {
                string TempStartDate = "2000-" + StartDate.Month + "-" + StartDate.Day;
                string TempEndDate = "2000-" + EndDate.Month + "-" + EndDate.Day;
                string TempCurrentDate = "2000-" + _crntDateTime.Month + "-" + _crntDateTime.Day;
                StartDate = Convert.ToDateTime(TempStartDate);
                EndDate = Convert.ToDateTime(TempEndDate);
                ModifedCurrentDate = Convert.ToDateTime(TempCurrentDate);
                if (EndDate < StartDate)
                {
                    EndDate = EndDate.AddYears(1);
                }
                if (ModifedCurrentDate < StartDate)
                {
                    ModifedCurrentDate = ModifedCurrentDate.AddYears(1);
                }
            }

            if (currentCategory.CPC_ComplianceRqdStartDate.IsNull()
              || currentCategory.CPC_ComplianceRqdEndDate.IsNull() ||
              ModifedCurrentDate >= StartDate && ModifedCurrentDate < EndDate)
            {
                return _isRequired;
            }
            else
            {
                return _isRequired = !_isRequired;
            } 
            #endregion

            #region Comment code
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
            #endregion
        }

        #endregion
    }

    [Serializable]
    public class SharedInvitationBackgroundDetail
    {
        public Int32 BkgPackageId { get; set; }
        public String BkgPackageName { get; set; }
        public Int32 BkgSvcGroupId { get; set; }
        public String BkgSvcGroupName { get; set; }
        public Int32 MasterOrderID { get; set; }
        public Boolean IsColorFlagVisible { get; set; }
        public Boolean IsResultPDFVisible { get; set; }
        public String ColorFlagPath { get; set; }
        public Boolean IsHeaderVisible { get; set; }
        public Boolean IsFlagStatusVisible { get; set; }
        public String FlagStatusImagePath { get; set; }
        public String InstitutionHierarchy { get; set; }
        public String MasterOrderNumber { get; set; }
    }
}
