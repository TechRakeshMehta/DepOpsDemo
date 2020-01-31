using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;

namespace CoreWeb.BkgSetup.Views
{
    public class ContentEditorPresenter : Presenter<IContentEditorView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void SaveContentData()
        {
            Int32 contentTypeId = BackgroundSetupManager.GetContentType(View.TenantId, lkpContentTypeEnum.Applicant_Invite_Landing_Page.GetStringValue());
            Int32 contentRecordTypeId = BackgroundSetupManager.GetContentRecordType(View.TenantId, lkpContentRecordTypeEnum.Institution_Hierarchy.GetStringValue());
            PageContent objContentData = new PageContent();
            objContentData.PC_ContentRecordID = View.DeptProgramMappingID;
            objContentData.PC_ContentRecordTypeID = contentRecordTypeId;
            objContentData.PC_ContentTypeID = contentTypeId;
            objContentData.PC_Content = View.Content;
            objContentData.PC_CreatedBy = View.CurrentUserId;
            objContentData.PC_CreatedOn = DateTime.Now;
            if (objContentData.IsNotNull())
            {
                if (BackgroundSetupManager.SaveContentData(View.TenantId, objContentData))
                {
                    View.SuccessMessage = "Content saved successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                }
            }
              
        }

        public void GetContentData()
        {
            View.ContentData = BackgroundSetupManager.GetContentData(View.TenantId, View.DeptProgramMappingID);
            if (!View.ContentData.IsNullOrEmpty())
            {
               View.Content = View.ContentData.PC_Content;
            }
        }
    }
}
