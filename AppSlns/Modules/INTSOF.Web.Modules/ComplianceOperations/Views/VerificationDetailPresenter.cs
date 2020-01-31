#region Namespaces

#region System Defined

using System;
using System.Text;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceRuleEngine;

#endregion

#endregion


namespace CoreWeb.ComplianceOperations.Views
{
    public class VerificationDetailPresenter : Presenter<IVerificationDetailView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public VerificationDetailPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {

        }

        /// <summary>
        /// call when View Initialized
        /// </summary>
        public override void OnViewInitialized()
        {
            GetApplicantComplianceData();
        }

        public void GetApplicantComplianceData()
        {
            ApplicantComplianceItemData applicantComplianceItemData = ComplianceDataManager.GetApplicantComplianceItemData(View.CurrentViewContext.ItemDataId, View.SelectedTenantId);
            View.CurrentViewContext.ApplicantComplianceItem = applicantComplianceItemData;
            if (applicantComplianceItemData.IsNotNull())
            {
                //get CompliancePackageId, ComplianceCategoryId, ComplianceItemId for executing buisness rules. 
                // View.CurrentViewContext.CurrentStatusCode = applicantComplianceItemData.lkpItemComplianceStatu.Code;
                View.CurrentViewContext.ComplianceItemId = applicantComplianceItemData.ComplianceItemID;
                View.CurrentViewContext.ComplianceCategoryId = applicantComplianceItemData.ApplicantComplianceCategoryData.ComplianceCategoryID;
                View.CurrentViewContext.CompliancePackageId = applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.CompliancePackageID;
                View.CurrentViewContext.ApplicantId = applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.OrganizationUserID.Value;
                List<Int32?> organisationUserIds = new List<Int32?>();
                organisationUserIds.Add(applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.OrganizationUserID);
                List<Int32?> loggedInUserIds = new List<Int32?>();
                loggedInUserIds.Add(View.CurrentLoggedInUserId);
                View.OrganizationUserData = ComplianceDataManager.GetOrganizationUsersByIds(organisationUserIds).FirstOrDefault();
                var organizationUserData = ComplianceDataManager.GetOrganizationUsersByIds(loggedInUserIds).FirstOrDefault();

                if (organizationUserData.IsNotNull())
                {
                    View.OrganizationUserName = organizationUserData.FirstName + " " + organizationUserData.LastName;
                }
                View.lstApplicantComplianceAttributeData = applicantComplianceItemData.ApplicantComplianceAttributeDatas.Where(x => !x.IsDeleted).ToList();

                var applicantComplianceAttribute = View.lstApplicantComplianceAttributeData.Where(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && !x.IsDeleted).FirstOrDefault();

                if (applicantComplianceAttribute == null)
                    View.lstApplicantComplianceDocumentMaps = null;
                else
                    View.lstApplicantComplianceDocumentMaps = applicantComplianceAttribute.ApplicantComplianceDocumentMaps.Where(x => x.IsDeleted == false).ToList();

                //To check if tenant is ADB admin
                if (View.TenantId == SecurityManager.DefaultTenantID)
                {
                    var assignmentOptions = ComplianceSetupManager.FetchAssignmentOptions(View.SelectedTenantId, applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.CompliancePackageID, applicantComplianceItemData.ApplicantComplianceCategoryData.ComplianceCategoryID, applicantComplianceItemData.ComplianceItemID);
                    if (assignmentOptions.ApprovalRequired == true)
                    {
                        var clientAdmin = assignmentOptions.AssignmentPropertiesReviewers.FirstOrDefault(x => x.lkpReviewerType.Code == LkpReviewerType.ClientAdmin && !x.IsDeleted);

                        if (clientAdmin.IsNull())
                        {
                            View.ClientAdminId = 0;
                        }
                        else
                        {
                            View.ClientAdminId = Convert.ToInt16(clientAdmin.ReviewerTypeID);
                        }
                    }
                    else
                        View.ClientAdminId = 0; // Handle the case when Approval required is reset to No, AFTER applicant has submitted the data.

                    if (assignmentOptions.TPReviewerUserID > 0)
                    {
                        View.ReviewerUserId = assignmentOptions.TPReviewerUserID.Value;
                    }
                }
                else
                {
                    View.ClientAdminId = 0;
                }
            }

            View.TenantData = SecurityManager.GetTenant(View.SelectedTenantId);
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return View.Tenant.TenantID.Equals(SecurityManager.DefaultTenantID);
            }
        }

        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.TenantId, TenantType.Compliance_Reviewer.GetStringValue());
            }
        }

        public Boolean IsSendForThirdPartyReview
        {
            get
            {
                AssignmentProperty assignmentProperty = GetAssignmentProperty();
                if (assignmentProperty != null && assignmentProperty.ReviewerTenantID != null && assignmentProperty.ReviewerTenantID > 0)
                    return true;

                return false;
            }
        }

        public AssignmentProperty GetAssignmentProperty()
        {
            //&& View.Tenant.lkpTenantType.TenantTypeCode == TenantType.Institution.GetStringValue()
            if (View.SelectedTenantId != SecurityManager.DefaultTenantID)
            {
                ApplicantComplianceItemData applicantComplianceItemData = ComplianceDataManager.GetApplicantComplianceItemData(
                    View.CurrentViewContext.ItemDataId,
                    View.SelectedTenantId);

                return ComplianceSetupManager.FetchAssignmentOptions(View.SelectedTenantId,
                    applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.CompliancePackageID,
                    applicantComplianceItemData.ApplicantComplianceCategoryData.ComplianceCategoryID,
                    applicantComplianceItemData.ComplianceItemID);

            }
            return null;

        }


        public void GetApplicantDocuments()
        {
            if (View.OrganizationUserData != null)
                View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocuments(View.OrganizationUserData.OrganizationUserID, View.SelectedTenantId);
        }


        //public void GetComplianceItemForControls()
        //{
        //    View.CurrentViewContext.ClientComplianceItem = ComplianceDataManager.GetDataEntryComplianceItem(View.CurrentViewContext.ItemId, View.TenantId);
        //}

        /// <summary>
        /// To update Item Data Status
        /// </summary>
        public void UpdateItemDataStatus()
        {
            Int32? reviewerTenantId = null;
            Int32? reviewerUserId = null;
            AssignmentProperty assignmentProperty = GetAssignmentProperty();
            if (assignmentProperty != null
                && assignmentProperty.ReviewerTenantID != null
                && assignmentProperty.ReviewerTenantID > 0
                && View.CurrentStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()))
                reviewerTenantId = assignmentProperty.ReviewerTenantID;

            if (assignmentProperty != null
                && assignmentProperty.ReviewerTenantID != null
                && assignmentProperty.ReviewerTenantID > 0
                && View.CurrentStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue())
                && assignmentProperty.TPReviewerUserID > 0)
            {
                reviewerTenantId = assignmentProperty.ReviewerTenantID;
                reviewerUserId = assignmentProperty.TPReviewerUserID;
            }
            View.CurrentStatusId = ComplianceDataManager.GetlkpItemComplianceStatus(View.CurrentStatusCode, View.SelectedTenantId);
            //ComplianceDataManager.UpdateItemDataStatus(View.ItemDataId, View.Comments, View.CurrentStatusId,
            //    View.CurrentLoggedInUserId, View.SelectedTenantId, reviewerTenantId, DateTime.Now, View.ClientAdminId, View.ReviewerUserId);
            //evaluatePostSubmitRules();
        }


        public void UpdateApplicantComplianceAttributeData()
        {
            View.UIInputException = String.Empty;

            if (View.IsUIValidationApplicable)
            {
                ApplicantComplianceItemData applicantComplianceItemData = ComplianceDataManager.GetApplicantComplianceItemData(View.CurrentViewContext.ItemDataId, View.SelectedTenantId);
                Int32 organizationUserId = applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.OrganizationUserID.Value;
                Int32 compliancePackageId = applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.CompliancePackageID;

                //View.UIInputException = ComplianceDataManager.ValidateUIInput(organizationUserId, compliancePackageId, View.lstApplicantComplianceAttributeData, applicantComplianceItemData, View.SelectedTenantId);
            }

            //if (String.IsNullOrEmpty(View.UIInputException))
            //    ComplianceDataManager.UpdateApplicantComplianceAttributeData(View.lstApplicantComplianceAttributeData, View.SelectedTenantId);
        }

        public void UpdateApplicantComplianceDocumentMaps(List<ApplicantComplianceDocumentMap> toAddDocumentMap, List<Int32> toDeleteApplicantComplianceDocumentMapIDs)
        {
            ComplianceDataManager.UpdateApplicantComplianceDocumentMaps(toAddDocumentMap, toDeleteApplicantComplianceDocumentMapIDs, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Entity.Tenant GetTenant()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.Tenant;
        }

        public void getNextRecordData()
        {
            Entity.CustomPagingArgs customPagingArgs = new Entity.CustomPagingArgs();
            IQueryable<vwComplianceItemDataQueue> resultQuery = null;
            List<vwComplianceItemDataQueue> lstApplicantComplianceItemData = null;
            Int32 assignedToUserId = -1;
            List<String> lstStatusCode = new List<String>();
            String reviewerTypeCode = String.Empty;
            Int32 clientId = View.TenantId;
            Int32 reviewerId = 0;
            if (clientId == SecurityManager.DefaultTenantID)
            {
                lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
                reviewerTypeCode = LkpReviewerType.Admin;
            }
            else if (IsThirdPartyTenant)
            {

                lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());
                reviewerId = View.TenantId;
            }
            else
            {
                reviewerTypeCode = LkpReviewerType.ClientAdmin;
                lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
            }


            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if ((clientId == SecurityManager.DefaultTenantID || IsThirdPartyTenant) && View.SelectedTenantId != 0)
            {
                clientId = View.SelectedTenantId;
            }

            if (!((clientId == SecurityManager.DefaultTenantID || IsThirdPartyTenant) && View.SelectedTenantId == 0))
            {
                if (View.WorkQueue == WorkQueueType.UserWorkQueue)
                {
                    resultQuery = ComplianceDataManager.GetApplicantComplianceItemData(clientId, lstStatusCode, View.CurrentLoggedInUserId);
                }
                else
                {
                    resultQuery = ComplianceDataManager.GetApplicantComplianceItemData(clientId, lstStatusCode, -1, reviewerTypeCode, false, reviewerId);
                }
                if (View.PackageId > 0)
                {
                    resultQuery = resultQuery.Where(x => x.PackageID == View.PackageId);

                    if (View.CategoryId > 0)
                    {
                        resultQuery = resultQuery.Where(x => x.CategoryID == View.CategoryId);
                    }
                }
                View.VerificationGridCustomPaging.PageSize = AppConsts.NONE;
                View.VerificationGridCustomPaging.DefaultSortExpression = "ApplicantComplianceItemId";
                View.VerificationGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS;
                resultQuery = customPagingArgs.ApplyFilterOrSort(resultQuery, View.VerificationGridCustomPaging);
                lstApplicantComplianceItemData = resultQuery.ToList();
                if (lstApplicantComplianceItemData != null && lstApplicantComplianceItemData.Count > 0)
                {
                    if (lstApplicantComplianceItemData.SkipWhile(x => x.ApplicantComplianceItemId != View.ItemDataId).Skip(1).FirstOrDefault() != null)
                    {
                        View.NextItemDataId = lstApplicantComplianceItemData.SkipWhile(x => x.ApplicantComplianceItemId != View.ItemDataId).Skip(1).FirstOrDefault().ApplicantComplianceItemId.Value;
                    }
                    else if (lstApplicantComplianceItemData.Count() == 1)
                    {
                        View.NextItemDataId = 0;
                    }
                    else
                    {
                        View.NextItemDataId = lstApplicantComplianceItemData.FirstOrDefault().ApplicantComplianceItemId.Value;
                    }
                }
            }

        }

        /// <summary>
        /// for executing buisness rules.
        /// </summary>
        public void evaluatePostSubmitRules()
        {
            List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();
            RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), View.SelectedTenantId).OT_ID),
                RuleObjectId = Convert.ToString(View.CurrentViewContext.CompliancePackageId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), View.SelectedTenantId).OT_ID),
                RuleObjectId = Convert.ToString(View.CurrentViewContext.ComplianceCategoryId),
                RuleObjectParentId = Convert.ToString(View.CurrentViewContext.CompliancePackageId)
            };

            RuleObjectMapping ruleObjectMappingForItem = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Item.GetStringValue(), View.SelectedTenantId).OT_ID),
                RuleObjectId = Convert.ToString(View.CurrentViewContext.ComplianceItemId),
                RuleObjectParentId = Convert.ToString(View.CurrentViewContext.ComplianceCategoryId)
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
                        RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_ATR.GetStringValue(), View.SelectedTenantId).OT_ID),
                        RuleObjectId = Convert.ToString(attributeData.ComplianceAttributeID),
                        RuleObjectParentId = Convert.ToString(View.CurrentViewContext.ComplianceItemId)
                    };
                    ruleObjectMappingList.Add(ruleObjectMappingForAttribute);
                }
            }
            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.ApplicantId, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }
    }
}




