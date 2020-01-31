using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class AttributeDocumentFieldMappingPresenter : Presenter<IAttributeDocumentFieldMappingView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        /// <summary>
        /// Called when viwe is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        
        public void GetDocumentFieldMapping()
        {
            View.lstDocumentFieldMapping = ComplianceSetupManager.GetDocumentFieldMapping(View.SelectedTenantId, View.SystemDocumentId);
        }

        public Boolean UpdateDocumentFieldMapping(Int32 documentFieldMappingID, int documentFieldTypeID)
        {
            return ComplianceSetupManager.UpdateDocumentFieldMapping(View.SelectedTenantId, documentFieldMappingID, documentFieldTypeID, View.CurrentLoggedInUserId);
        }

        public void GetDocumentFieldTypes()
        {
            View.lstDocumentFieldType = ComplianceSetupManager.GetDocumentFieldTypes(View.SelectedTenantId);
        }
    }
}
