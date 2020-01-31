using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ItemDataExceptionModePresenter : Presenter<IItemDataExceptionModeView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ItemDataExceptionModePresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Save the uploaded documents and map with exceptiondocumentmapping.
        /// </summary>
        /// <param name="applicantDocumentList">ApplicantDocumentList</param>
        /// <returns>Boolean</returns>
        public Boolean SaveApplicantUploadedDocuments(List<ApplicantDocument> applicantDocumentList)
        {
            return ComplianceDataManager.SaveApplicantUploadedDocuments(applicantDocumentList, View.ApplicantItemDataId, View.SelectedTenantId_Global);
        }

        /// <summary>
        /// To update and save Item Data Status
        /// </summary>
        public Boolean UpdateAndSaveExceptionItemData(String recordActionType)
        {
            Boolean _status = false;

            PackageSubscription ps = ComplianceDataManager.GetPackageSubscriptionByID(View.SelectedTenantId_Global, View.CurrentPackageSubscriptionId);

            Entity.OrganizationUser _organizationUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            String _currentLoggedInUserName = _organizationUser.FirstName + " " + _organizationUser.LastName;
            String currentLoggedInUserInitials = _organizationUser.FirstName.Substring(0, 1) + (_organizationUser.MiddleName.IsNullOrEmpty() ? String.Empty : _organizationUser.MiddleName.Substring(0, 1)) + _organizationUser.LastName.Substring(0, 1);
            //Int32 _statusId = ComplianceDataManager.GetlkpItemComplianceStatus(View.StatusCode, View.SelectedTenantId_Global);
            List<lkpItemComplianceStatu> _lstItemStatus = LookupManager.GetLookUpData<lkpItemComplianceStatu>(View.SelectedTenantId_Global);
            Int32 _statusId = _lstItemStatus.Where(sts => sts.Code == View.StatusCode).FirstOrDefault().ItemComplianceStatusID;
            String _statusCode = _lstItemStatus.Where(sts => sts.Code == View.StatusCode).FirstOrDefault().Code;

            ComplianceDataManager.UpdateExceptionVerificationItemData(View.ApplicantItemDataId, View.ExceptionItemIdUpdated, View.Comments, _statusId, View.CurrentLoggedInUserId, _currentLoggedInUserName, null, View.SelectedTenantId_Global, View.lstAssignmentProperties, recordActionType, currentLoggedInUserInitials, _statusCode, View.ItemExpirationDate);

            //UAT-3951
            if (!View.SelectedRejectionReasonIds.IsNullOrEmpty())
            {
                ComplianceDataManager.SaveRejectionReasonAuditHistory(View.SelectedTenantId_Global, View.SelectedRejectionReasonIds, View.ApplicantItemDataId, View.CurrentLoggedInUserId);
            }

            EvaluatePostSubmitRules();

            if (ps.ComplianceStatusID.Value != 0)
            {
                String tenantName = String.Empty;
                Entity.Tenant tenant = SecurityManager.GetTenant(View.SelectedTenantId_Global);
                if (!tenant.IsNullOrEmpty())
                    tenantName = tenant.TenantName;
                //Send Mail
                //UAT-2908: Impacted due to (UAT-2955)
                //ComplianceDataManager.SendMailOnComplianceStatusChange(View.SelectedTenantId_Global, tenantName, ps.lkpPackageComplianceStatu.Name, ps.ComplianceStatusID.Value, View.CurrentPackageSubscriptionId, ps.Order.DeptProgramPackage.DeptProgramMapping.DPM_ID);
            }

            _status = true;
            return _status;
        }

        /// <summary>
        /// Remove the selected document from the exception 
        /// </summary>
        /// <param name="mappingId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="selectedTenantId"></param>
        public void RemoveExceptionDocumentMapping()
        {
            ComplianceDataManager.RemoveExceptionDocumentMapping(View.MappingId, View.CurrentLoggedInUserId, View.SelectedTenantId_Global);
        }

        public List<ComplianceItem> GetAvailableItemsForException()
        {
            return ComplianceDataManager.GetAvailableDataEntryItems(View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global, View.SelectedApplicantId_Global, View.SelectedTenantId_Global, View.CurrentItemId);
        }

        /// <summary>
        /// for executing buisness rules.
        /// </summary>
        public void EvaluatePostSubmitRules()
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
                RuleObjectId = Convert.ToString(View.CurrentViewContext.CurrentItemId),
                RuleObjectParentId = Convert.ToString(View.CurrentViewContext.SelectedComplianceCategoryId_Global)
            };
            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);
            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.CurrentViewContext.SelectedApplicantId_Global, View.CurrentLoggedInUserId, View.SelectedTenantId_Global);
        }


        public Int32 GetNewStatusId(String newStatusCode)
        {
            List<lkpItemComplianceStatu> _lstStatus = LookupManager.GetLookUpData<lkpItemComplianceStatu>(View.SelectedTenantId_Global);
            return _lstStatus.Where(sts => sts.Code == newStatusCode).FirstOrDefault().ItemComplianceStatusID;
        }

        /// <summary>
        /// To delete Applicant Item Attr Data and execute Category level Business Rules
        /// </summary>
        public void DeleteApplicantItemAttrData()
        {
            /*Call "RemoveExceptionData" function in place of "DeleteApplicantItemAttributeData"  to delete the applied exception for item  and also remove the exception documents mapping fo this item.
            [Change By: Sachin Singh:13/04/2015,Description:- Issue:-Document is not linked with any item then also already mapped message appears on document mapping screen.]*/
            var isDeleted = ComplianceDataManager.RemoveExceptionData(View.ApplicantItemDataId, View.CurrentLoggedInUserId, View.SelectedTenantId_Global); //ComplianceDataManager.DeleteApplicantItemAttributeData(View.ApplicantItemDataId, View.CurrentLoggedInUserId, View.SelectedTenantId_Global);

            if (isDeleted.IsNotNull() && isDeleted.SaveStatus == true)
                RuleManager.ExecuteCategoryLevelBusinessRules(View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global, View.CurrentLoggedInUserId,
                                                                View.SelectedApplicantId_Global, View.SelectedTenantId_Global);
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

        public Boolean IsDefaultTenant
        {
            get
            {
                return View.CurrentTenantId_Global.Equals(SecurityManager.DefaultTenantID);
            }
        }

    }
}




