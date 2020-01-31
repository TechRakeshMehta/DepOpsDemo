using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using System.Linq;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Contracts;
using System.Web;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ReconciliationItemDataReviewerPrsenter : Presenter<IReconciliationItemDataReviewerView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                // return View.Tenant.TenantID.Equals(SecurityManager.DefaultTenantID);
                return View.CurrentTenantId_Global.Equals(SecurityManager.DefaultTenantID);
            }
        }

        public Int32 DefaultTenantId
        {
            get
            {
                return SecurityManager.DefaultTenantID;
            }
        }
        public Boolean IsSendForThirdPartyReview(AssignmentProperty assignmentProperty)
        {
            if (assignmentProperty != null && assignmentProperty.ReviewerTenantID != null && assignmentProperty.ReviewerTenantID > 0)
                return true;

            return false;
        }
        public Boolean IsSendForThirdPartyReview(ListItemAssignmentProperties assignmentProperty)
        {
            if (assignmentProperty != null && assignmentProperty.ReviewerTenantId != null && assignmentProperty.ReviewerTenantId > 0)
                return true;

            return false;
        }

        public Int32 GetNewStatusId(String newStatusCode)
        {
            List<lkpItemComplianceStatu> _lstStatus = LookupManager.GetLookUpData<lkpItemComplianceStatu>(View.SelectedTenantId_Global);
            return _lstStatus.Where(sts => sts.Code == newStatusCode).FirstOrDefault().ItemComplianceStatusID;
        }


        /// <summary>
        /// Gets the Tenant details for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Entity.Tenant GetTenant()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.Tenant;
        }

        public void ValidateApplicantData(List<ApplicantComplianceItemData> lstItemData, List<ApplicantComplianceAttributeData> lstAttributeData)
        {
            View.UIInputException = String.Empty;

            if (View.IsUIValidationApplicable)
            {
                ApplicantComplianceItemData applicantComplianceItemData = new ApplicantComplianceItemData();
                applicantComplianceItemData.ComplianceItemID = View.ComplianceItemId;
                applicantComplianceItemData.ApplicantComplianceItemID = View.ApplicantItemDataId; // This can be ZERO as Incomplete items can now be added from Verification Details Screen
                View.UIInputException = ComplianceDataManager.ValidateUIInput(View.SelectedApplicantId_Global, View.SelectedCompliancePackageId_Global, View.lstApplicantComplianceAttributeData, View.SelectedComplianceCategoryId_Global, applicantComplianceItemData, View.CurrentPackageSubscriptionId, false, View.SelectedTenantId_Global);
            }
        }

        /// <summary>
        /// Save/Update Applicant data from Verification details screen and Update item status
        /// </summary>
        public ApplicantComplianceItemData SaveApplicantData(String currentTenantTypeCode, String recordActionType)
        {
            Boolean IsReconciliationDataSaved = false;
            ApplicantComplianceItemData _applicantItemData = new ApplicantComplianceItemData();
            //View.lstApplicantComplianceAttributeData.ForEach(attr => attr.ModifiedOn = _dtCurrentDateTime);

            PackageSubscription ps = ComplianceDataManager.GetPackageSubscriptionByID(View.SelectedTenantId_Global, View.CurrentPackageSubscriptionId);

            //ComplianceDataManager.UpdateApplicantComplianceAttributeData(View.lstApplicantComplianceAttributeData, View.SelectedTenantId_Global);
            ApplicantComplianceCategoryData categoryData = new ApplicantComplianceCategoryData
            {
                ApplicantComplianceCategoryID = View.ApplicantCategoryDataId > AppConsts.NONE ? View.ApplicantCategoryDataId : AppConsts.NONE,
                PackageSubscriptionID = View.CurrentPackageSubscriptionId,
                ComplianceCategoryID = View.SelectedComplianceCategoryId_Global,
                //Notes = View.CategoryDataContract.Notes // To be added
            };

            ApplicantComplianceItemData itemData = new ApplicantComplianceItemData
            {
                ApplicantComplianceItemID = View.ApplicantItemDataId > AppConsts.NONE ? View.ApplicantItemDataId : AppConsts.NONE,
                ComplianceItemID = View.ComplianceItemId
                // Notes = View.ItemDataContract.Notes// To be added
            };
            itemData = ComplianceDataManager.SetItemReviewerTypeProperties(itemData, View.lstAssignmentProperties);
            itemData.VerificationComments = View.Comments;
            itemData.StatusComments = View.StatusComments;
            //Int32 _newStatusId = ComplianceDataManager.GetlkpItemComplianceStatus(View.NewItemStatus, View.SelectedTenantId_Global);

            var _loggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var _exceptionService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            VerificationDetailsContract verificationDetailsContract = new VerificationDetailsContract
            {
                applicantCategoryData = categoryData,
                applicantItemData = itemData,
                lstApplicantData = View.lstApplicantComplianceAttributeData,
                createdModifiedById = View.CurrentLoggedInUserId,
                newStatus = View.IncompleteItemNewStatusId ?? View.AttemptedItemStatusId,
                reviewerTypeId = View.ReviewerTypeId,
                reviewerTenantId = View.ReviewerTenantId,
                thirdPartyReviewerUserId = View.TPReviewerUserId,
                applicantId = View.SelectedApplicantId_Global,
                isAdminReviewRequired = View.IsAdminReviewRequired,
                newItemStatusCode = View.IncompleteItemNewStatusCode ?? View.AttemptedItemStatus,
                currentTenantTypeCode = currentTenantTypeCode,
                packageId = View.SelectedCompliancePackageId_Global
            };

            ComplianceDataManager.SaveApplicanteDataVerificationDetails(verificationDetailsContract, recordActionType, View.SelectedTenantId_Global, out _applicantItemData
                                                                        ,IsReconciliationDataSaved,null);

            this.EvaluatePostSubmitRules();

            if (ps.ComplianceStatusID.Value != 0)
            {
                String tenantName = String.Empty;
                Entity.Tenant tenant = SecurityManager.GetTenant(View.SelectedTenantId_Global);
                if (!tenant.IsNullOrEmpty())
                    tenantName = tenant.TenantName;
                //Send Mail
                ComplianceDataManager.SendMailOnComplianceStatusChange(View.SelectedTenantId_Global, tenantName, ps.lkpPackageComplianceStatu.Name, ps.ComplianceStatusID.Value, View.CurrentPackageSubscriptionId, ps.Order.DeptProgramPackage.DeptProgramMapping.DPM_ID);

                //Send Notification On Item Status Changed To Review Status
                ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(false, View.SelectedTenantId_Global, View.CurrentPackageSubscriptionId, View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global, View.ComplianceItemId, View.CurrentLoggedInUserId, View.SelectedApplicantId_Global, View.ItemComplianceStatusCode);
            }
            return _applicantItemData;
        }

        /// <summary>
        /// To delete Applicant Item Attr Data and execute Category level Business Rules
        /// </summary>
        public void DeleteApplicantItemAttrData()
        {
            var isDeleted = ComplianceDataManager.DeleteApplicantItemAttributeData(View.ApplicantItemDataId, View.CurrentLoggedInUserId, View.SelectedTenantId_Global, String.Empty, View.SelectedApplicantId_Global);

            if (isDeleted.SaveStatus == true)
                RuleManager.ExecuteCategoryLevelBusinessRules(View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global, View.CurrentLoggedInUserId,
                                                                View.SelectedApplicantId_Global, View.SelectedTenantId_Global);
        }

        /// <summary>
        /// for executing buisness rules.
        /// </summary>
        private void EvaluatePostSubmitRules()
        {
            List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();
            RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), View.SelectedTenantId_Global).OT_ID),
                RuleObjectId = Convert.ToString(View.CurrentViewContext.SelectedCompliancePackageId_Global),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), View.SelectedTenantId_Global).OT_ID),
                RuleObjectId = Convert.ToString(View.CurrentViewContext.SelectedComplianceCategoryId_Global),
                RuleObjectParentId = Convert.ToString(View.CurrentViewContext.SelectedCompliancePackageId_Global)
            };

            RuleObjectMapping ruleObjectMappingForItem = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Item.GetStringValue(), View.SelectedTenantId_Global).OT_ID),
                RuleObjectId = Convert.ToString(View.CurrentViewContext.ComplianceItemId),
                RuleObjectParentId = Convert.ToString(View.CurrentViewContext.SelectedComplianceCategoryId_Global)
            };
            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);
            if (View.lstApplicantComplianceAttributeData.Count > 0)
            {
                foreach (ApplicantComplianceAttributeData attributeData in View.lstApplicantComplianceAttributeData)
                {
                    RuleObjectMapping ruleObjectMappingForAttribute = new RuleObjectMapping
                    {
                        RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_ATR.GetStringValue(), View.SelectedTenantId_Global).OT_ID),
                        RuleObjectId = Convert.ToString(attributeData.ComplianceAttributeID),
                        RuleObjectParentId = Convert.ToString(View.CurrentViewContext.ComplianceItemId)
                    };
                    ruleObjectMappingList.Add(ruleObjectMappingForAttribute);
                }
            }
            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.SelectedApplicantId_Global, View.CurrentLoggedInUserId, View.SelectedTenantId_Global);
        }

        #region UAT:719 Check Exceptions turned off for a Category/Item
        /// <summary>
        /// To check wheather exception is allowed turned off for a Category/item
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean IsAllowExceptionOnCategory()
        {
            return ComplianceDataManager.IsAllowExceptionOnCategory(View.SelectedTenantId_Global, View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global);
        }
        #endregion

        public ViewDocumentDetailsContract GetViewDocumentDetailContract(ApplicantItemVerificationData attributeData)
        {
            return ComplianceDataManager.GetViewDocumentDetailContract(View.SelectedTenantId_Global, attributeData);
        }
    }
}
