using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity.ClientEntities;
using Business.RepoManagers;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ItemDetailsPresenter : Presenter<IItemDetailsView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ItemDetailsPresenter([CreateNew] IComplianceOperationsController controller)
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

        // TODO: Handle other view events and set state in the view

        public void SaveApplicantComplianceAttributeData()
        {
            List<ApplicantComplianceAttributeData> applicantData = new List<ApplicantComplianceAttributeData>();


            ApplicantComplianceCategoryData categoryData = new ApplicantComplianceCategoryData
            {
                ApplicantComplianceCategoryDataID = View.CategoryDataContract.ApplicantComplianceCategoryId > 0 ? View.CategoryDataContract.ApplicantComplianceCategoryId : 0,
                PackageSubscriptionID = View.CategoryDataContract.PackageSubscriptionId,
                ComplianceCategoryID = View.CategoryDataContract.ComplianceCategoryId,
                ReviewStatusTypeID = View.CategoryDataContract.ReviewStatusTypeId,
                Notes = View.CategoryDataContract.Notes
            };

            ApplicantComplianceItemData itemData = new ApplicantComplianceItemData
            {
                ApplicantComplianceItemDataID = View.ItemDataContract.ApplicantComplianceItemId > 0 ? View.ItemDataContract.ApplicantComplianceItemId : 0,
                ComplianceItemID = View.ItemDataContract.ComplianceItemId,
                ReviewStatusTypeID = View.ItemDataContract.ReviewStatusTypeId,
                Notes = View.ItemDataContract.Notes
            };

            foreach (var attData in View.lstAttributesData)
            {
                applicantData.Add(new ApplicantComplianceAttributeData
                {
                    ApplicantComplianceAttributeDataID = attData.ApplicantComplianceAttributeId > 0 ? attData.ApplicantComplianceAttributeId : 0,
                    ComplianceItemAttributeID = attData.ComplianceItemAttributeId,
                    AttributeValue = attData.AttributeValue,
                });
            }
            //  View.SaveStatus = ApplicantManager.SaveApplicantComplianceAttributeData(categoryData, itemData, applicantData, View.CurrentLoggedInUserId, View.AttributeDocuments, View.CategoryDataContract.ReviewStatusTypeCode, View.ItemDataContract.ReviewStatusTypeCode, View.TenantId);
        }
    }
}




