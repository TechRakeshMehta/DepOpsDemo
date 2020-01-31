using Entity;
using INTSOF.UI.Contract.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Templates.Views
{
    public interface IAgencyHierarchySpecificTemplateView
    {
        /// <summary>
        /// Id of the template selected
        /// </summary>
        Int32 TemplateId { get; set; }

        /// <summary>
        /// List of Communication Languages.
        /// </summary>
        List<lkpLanguage> lstCommunicationLanguages { get; set; }

        /// <summary>
        /// List of Agency hierarchy root nodes
        /// </summary>
        List<Entity.SharedDataEntity.AgencyHierarchy> lstAgencyHierarchyRootNodes { get; set; }

        /// <summary>
        /// List of communication types
        /// </summary>
        List<lkpCommunicationType> lstCommunicationTypes { get; set; }

        /// <summary>
        /// list of Events related to the Communication Type
        /// </summary>
        List<lkpCommunicationEvent> lstCommunicationEvents { get; set; }

        /// <summary>
        /// List of the Sub-Events of the selected Communication type & Communication event
        /// </summary>
        List<lkpCommunicationSubEvent> lstCommunicationSubEvents { get; set; }

        /// <summary>
        /// Template details in edit mode
        /// </summary>
        SystemEventTemplatesContract SystemEventTemplate { get; set; }

        /// <summary>
        /// List of agency hierarchy root node ids.
        /// </summary>
        List<Int32?> lstAgencyHierarchyRootNodeID { get; set; }

        /// USe this id to get the related sub events & save the values in the Database
        /// </summary>
        Int32 CommunicationLanguageId { get; set; }

        /// <summary>
        /// USe this id to get the related events & save the values in the Database
        /// </summary>
        Int32? CommunicationTypeId { get; set; }

        /// <summary>
        /// Use this id to get the related sub events & save the values in the Database
        /// </summary>
        Int32? EventId { get; set; }

        /// <summary>
        /// Use this id to save the values in the Database
        /// </summary>
        Int32? SubEventId { get; set; }

        /// <summary>
        /// Name of the Template
        /// </summary>
        String TemplateName { get; set; }

        /// <summary>
        /// Description of the Template
        /// </summary>
        String TemplateDescription { get; set; }

        /// <summary>
        /// Subjec of the Template
        /// </summary>
        String TemplateSubject { get; set; }

        /// <summary>
        /// Content of the Template
        /// </summary>
        String TemplateContent { get; set; }

        /// <summary>
        /// List of the Place holders of the selected Sub-event
        /// </summary>
        List<CommunicationTemplatePlaceHolder> TemplatePlaceHolders { get; set; }

        List<Int32> SubEventIdsWithTemplates { get; set; }

        List<String> lstCommunicationSubEventCodes { get; set; }

        Int32 CurrentUserId{get;}

        List<Int32> lstSESId { get; set; }
    }
}
