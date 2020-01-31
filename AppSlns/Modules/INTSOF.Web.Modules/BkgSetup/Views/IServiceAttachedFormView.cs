using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IServiceAttachedFormView
    {
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> ListTenants
        {
            set;
            get;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        List<ServiceAttachedFormContract> ListServiceAttachedForm
        {
            set;
            get;
        }

        List<Entity.ServiceAttachedForm> LstParentServiceAttachedForm
        {
            set;
            get;
        }

        Int32 ParentFormId
        {
            get;
            set;
        }
        String FormName
        {
            get;
            set;
        }

        Int32 SF_ID
        {
            get;
            set;
        }

        Entity.SystemDocument SystemDocumentToSaveUpdate
        {
            get;
            set;
        }

        String ServiceFormDispatchType
        {
            get;
            set;
        }

        Int16 ServiceFormType
        {
            get;
            set;
        }

        String TemplateName
        {
            get;
            set;
        }

        String TemplateSubject
        {
            get;
            set;
        }

        String TemplateContent
        {
            get;
            set;
        }

        String ReminderTemplateName
        {
            get;
            set;
        }

        String ReminderTemplateSubject
        {
            get;
            set;
        }

        String ReminderTemplateContent
        {
            get;
            set;
        }


        List<Entity.CommunicationTemplatePlaceHolder> TemplatePlaceHolders
        {
            get;
            set;
        }

        List<Entity.CommunicationTemplateEntity> ServiceFormCommunicationTemplateData
        {
            get;
            set;
        }

        Boolean IsUpdate
        {
            get;
            set;
        }

        Int32 SvcFormCommunicationTemplateID
        {
            get;
            set;
        }

        Int32 ReminderCommunicationTemplateID
        {
            get;
            set;
        }

        Boolean IsCorruptedDocument
        {
            get;
            set;
        }

        Boolean IsBkgServiceAttachedFormMappingExists
        {
            get;
            set;
        }
        //UAT-2480
        Boolean IsActiveVersionsPresent
        {
            get;
            set;
        }

        List<Entity.lkpLanguage> Languages
        {
            get;
            set;
        }

        Int32 DefaultLanguageId { get; }

        Int32 SelectedLanguageId { get; set; }
    }
}
