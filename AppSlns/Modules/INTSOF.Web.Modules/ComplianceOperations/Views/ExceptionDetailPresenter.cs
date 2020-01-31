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
    public class ExceptionDetailPresenter : Presenter<IExceptionDetailView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ExceptionDetailPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            getExceptionData();
        }

        public void getExceptionData()
        {
            ApplicantComplianceItemData applicantComplianceItemData = ComplianceDataManager.GetApplicantComplianceItemData(View.CurrentViewContext.ItemDataId, View.SelectedTenantId);
            View.CurrentViewContext.ApplicantComplianceItem = applicantComplianceItemData;
            View.ApplicantComplianceCategoryID = applicantComplianceItemData.ApplicantComplianceCategoryID;
            if (applicantComplianceItemData.IsNotNull())
            {
                View.CurrentViewContext.ComplianceItemId = applicantComplianceItemData.ComplianceItemID;
                View.CurrentViewContext.ComplianceCategoryId = applicantComplianceItemData.ApplicantComplianceCategoryData.ComplianceCategoryID;
                View.CurrentViewContext.CompliancePackageId = applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.CompliancePackageID;
                View.CurrentViewContext.ApplicantId = applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.OrganizationUserID.Value;
                List<Int32?> organisationUserIds = new List<Int32?>();
                organisationUserIds.Add(applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.OrganizationUserID);
                View.OrganizationUserData = ComplianceDataManager.GetOrganizationUsersByIds(organisationUserIds).FirstOrDefault();
                List<ComplianceItem> tempItemList = ComplianceDataManager.GetAvailableDataEntryItems(applicantComplianceItemData.ApplicantComplianceCategoryData.PackageSubscription.CompliancePackageID, applicantComplianceItemData.ApplicantComplianceCategoryData.ComplianceCategoryID, View.OrganizationUserData.OrganizationUserID, View.SelectedTenantId).ToList();
                tempItemList.Insert(0, new ComplianceItem { ComplianceItemID = applicantComplianceItemData.ComplianceItemID, Name = applicantComplianceItemData.ComplianceItem.Name });
                View.itemIdList = tempItemList;
                View.ApplicantDocumnetIds = applicantComplianceItemData.ExceptionDocumentMappings.Where(cond => cond.ApplicantComplianceItemID == View.CurrentViewContext.ItemDataId && !cond.IsDeleted).Select(co => co.ApplicantDocumentID).ToList();
            }
            View.TenantData = SecurityManager.GetTenant(View.SelectedTenantId);
        }
        #region public Methods

        /// <summary>
        /// To update and save Item Data Status
        /// </summary>
        public Boolean UpdateAndSaveItemData()
        {

            Entity.OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            String currentLoggedInUserName = organizationUser.FirstName + " " + organizationUser.LastName;
            View.CurrentStatusId = ComplianceDataManager.GetlkpItemComplianceStatus(View.CurrentStatusCode, View.SelectedTenantId);
            return ComplianceDataManager.UpdateItemData(View.ItemDataId, View.Comments, View.CurrentStatusId, View.CurrentLoggedInUserId, View.SelectedTenantId, currentLoggedInUserName, View.ListOfIdToRemoveDocument, View.SelectedItemId, View.ListOfIdToAddDocument);

        }

        /// <summary>
        /// Bind document grid with document list
        /// </summary>
        public List<ApplicantDocument> BindDocumentGrid()
        {
            ApplicantComplianceItemData applicantComplianceItemData = ComplianceDataManager.GetApplicantComplianceItemData(View.CurrentViewContext.ItemDataId, View.SelectedTenantId);
            View.ApplicantDocumnetIds = applicantComplianceItemData.ExceptionDocumentMappings.Where(cond => cond.ApplicantComplianceItemID == View.CurrentViewContext.ItemDataId && !cond.IsDeleted).Select(co => co.ApplicantDocumentID).ToList();
           List<ApplicantDocument> applicantDocuments=ComplianceDataManager.GetApplicantDocuments(View.OrganizationUserData.OrganizationUserID, View.SelectedTenantId).OrderByDescending(x => View.ApplicantDocumnetIds.IndexOf(x.ApplicantDocumentID)).ToList();
           if (applicantDocuments.IsNotNull())
           {
               return ComplianceDataManager.GetApplicantDocumentsExceptEsigned(applicantDocuments, View.SelectedTenantId);
           }
           return null;
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Save the uploaded documents and map with exceptiondocumentmapping.
        /// </summary>
        /// <param name="applicantDocumentList">ApplicantDocumentList</param>
        /// <returns>Boolean</returns>
        public Boolean SaveApplicantUploadedDocuments(List<ApplicantDocument> applicantDocumentList)
        {
            return ComplianceDataManager.SaveApplicantUploadedDocuments(applicantDocumentList, View.ItemDataId, View.SelectedTenantId);
        }

        public void getNextRecordData()
        {
            Entity.CustomPagingArgs customPagingArgs = new Entity.CustomPagingArgs();
            IQueryable<vwComplianceItemDataQueue> resultQuery = null;
            List<vwComplianceItemDataQueue> lstApplicantComplianceItemData = null;
            Int32 clientId = View.TenantId;
            List<String> lstStatusCode = new List<String>();
            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if (clientId == SecurityManager.DefaultTenantID && View.SelectedTenantId != 0)
            {
                clientId = View.SelectedTenantId;
            }

            if (!(clientId == SecurityManager.DefaultTenantID && View.SelectedTenantId == 0))
            {
                lstStatusCode.Add(ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue());
                if (View.WorkQueue == WorkQueueType.UserWorkQueue)
                {
                    resultQuery = ComplianceDataManager.GetApplicantComplianceItemData(clientId, lstStatusCode, View.CurrentLoggedInUserId);
                }
                else
                {
                    resultQuery = ComplianceDataManager.GetApplicantComplianceItemData(clientId, lstStatusCode, -1);
                }
                if (View.PackageId > 0)
                {
                    resultQuery = resultQuery.Where(x => x.PackageID == View.PackageId);

                    if (View.CategoryId > 0)
                    {
                        resultQuery = resultQuery.Where(x => x.CategoryID == View.CategoryId);
                    }
                }
                View.ExceptionGridCustomPaging.PageSize = AppConsts.NONE;
                View.ExceptionGridCustomPaging.DefaultSortExpression = "ApplicantComplianceItemId";
                View.ExceptionGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS;
                resultQuery = customPagingArgs.ApplyFilterOrSort(resultQuery, View.ExceptionGridCustomPaging);
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
            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.CurrentViewContext.ApplicantId, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }
        #endregion
    }
}




