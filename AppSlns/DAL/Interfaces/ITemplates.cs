using Entity;
using INTSOF.UI.Contract.Templates;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Interfaces
{
    public interface ITemplates
    {
        void DeleteTemplate(Int32 templateId, Int32 currentUserId, Boolean isEventSpecific);

        List<SystemEventTemplatesContract> GetEventSpecificTemplates();

        List<SystemEventTemplatesContract> GetTenantSpecificEventTemplates(Int32 tenantId);

        List<CommunicationTemplate> GetOtherTemplates();

        Boolean SaveUpdateTemplate(SystemEventTemplatesContract communicationTemplateContract);

        /// <summary>
        /// Get the list of category ids for which templates have been already defined.
        /// </summary>
        /// <returns></returns>
        List<Int32?> GetUsedCategories(Int32 tenantId);

        /// <summary>
        /// Get template defined at category level in template maintenance form, on selection of category
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        SystemEventSetting GetCategoryLevelTemplate(Int32 categoryId, Int32 subEventId, Int32 tenantId);

        SystemEventTemplatesContract GetTemplateDetails(Int32 templateId);


        #region Deadline Type Template Operations

        DataTable GetNodeTemplates(Int32 subEventId);

        SystemEventTemplatesContract GetNodeTemplateByNotificationMappingId(Int32 nodeNotificationMappingId, Int32 tenantId);

        Int32 SaveUpdateNodeNotificationTemplates(SystemEventTemplatesContract communicationTemplateContract);

        #endregion

        //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
        Dictionary<String, String> GetInvitationEmailContent(Int32 TenantID, String CommSubEventCode);
        //3704 :- Get Agency hierarchy Specific Templates
        List<SystemEventTemplatesContract> GetAgencyHierarchySpecificEventTemplates();
        SystemEventTemplatesContract GetAgencyHierarchyTemplateDetail(Int32 templateId);
        Boolean SaveUpdateAgencyHierarchyTemplate(SystemEventTemplatesContract communicationTemplateContract);
        Boolean DeleteAgencyHierarchyTemplate(Int32 templateId, Int32 currentUserId);
        List<Int32?> GetAlreadyMappedAgencyHierarchyWithSubEvnt(Int32 templateId, Int32? subEvntId);

        #region Admin Entry Portal Node - Inst. NodeLevel Templates
        List<CommunicationTemplate> GetTemplates(Int32 subEventId);
        List<SystemEventSetting> GetSystemEventSettings(Int32 subEventId);
        Boolean SaveUpdateAdminEntryNodeTemplate(CommunicationTemplate communicationTemplate);
        Boolean SaveUpdateSystemEventSetting(SystemEventSetting systemEventSetting);

        #endregion
    }
}
