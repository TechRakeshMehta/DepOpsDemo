using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using System.Linq;
using INTSOF.UI.Contract.Templates;

namespace CoreWeb.Templates.Views
{
    public interface ITemplatesMaintenanceFormEventSpecificView
    { /// <summary>
        /// List of communication types
        /// </summary>
        List<lkpCommunicationType> CommunicationTypes
        {
            get;
            set;
        }

        /// <summary>
        /// list of Events related to the Communication Type
        /// </summary>
        IEnumerable<lkpCommunicationEvent> CommunicationEvents
        {
            get;
            set;
        }

        /// <summary>
        /// List of the Sub-Events of the selected Communication type & Communication event
        /// </summary>
        List<lkpCommunicationSubEvent> CommunicationSubEvents
        {
            get;
            set;
        }

        List<lkpLanguage> CommunicationLanguages
        {
            get;
            set;
        }

        /// <summary>
        /// List of the Place holders of the selected Sub-event
        /// </summary>
        List<CommunicationTemplatePlaceHolder> TemplatePlaceHolders
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the template selected
        /// </summary>
        Int32 TemplateId
        {
            get;
            set;
        }

        /// <summary>
        /// USe this id to get the related sub events & save the values in the Database
        /// </summary>
        Int32 CommunicationLanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// USe this id to get the related events & save the values in the Database
        /// </summary>
        Int32? CommunicationTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Use this id to get the related sub events & save the values in the Database
        /// </summary>
        Int32? EventId
        {
            get;
            set;
        }

        /// <summary>
        /// Use this id to save the values in the Database
        /// </summary>
        Int32? SubEventId
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the Template
        /// </summary>
        String TemplateName
        {
            get;
            set;
        }

        /// <summary>
        /// Subjec of the Template
        /// </summary>
        String TemplateSubject
        {
            get;
            set;
        }

        /// <summary>
        /// Description of the Template
        /// </summary>
        String TemplateDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Content of the Template
        /// </summary>
        String TemplateContent
        {
            get;
            set;
        }

        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Represents the current context of the Template Maintenance Form
        /// </summary>
        ITemplatesMaintenanceFormEventSpecificView CurrentContext
        {
            get;
        }

        /// <summary>
        /// Get list of tenants for items specific templates
        /// </summary>
        List<Tenant> lstTenants
        {
            get;
            set;
        }

        List<Entity.ClientEntity.ComplianceCategory> lstCategories
        { get; set; }

        List<Entity.ClientEntity.ComplianceItem> lstItems
        { get; set; }

        /// <summary>
        /// Id of the selected Tenant for the item specific template
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        List<String> lstCommunicationSubEventCodes
        {
            get;
            set;
        }

        Int32 SelectedCategoryId
        { get; set; }

        Int32 SelectedItemId
        { get; set; }

        Int32? Frequency
        { get; set; }

        Int32? NoOfDays
        { get; set; }

        /// <summary>
        /// Template details in edit mode
        /// </summary>
        SystemEventTemplatesContract SystemEventTemplate { get; set; }

        Int32 ItemIdEdited { get; set; }

        Int32 SESId { get; set; }

        String DaysBefore
        {
            get;
            set;
        }
        //UAT-1793 : Should not be able to create duplicate templates in the common template section of the System Template screen.
        List<Int32> SubEventIdsWithTemplates { get; set; }

        //UAT-3656
        Boolean IsNotificationBlocked { get; set; }
    }
}




