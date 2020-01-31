#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;


#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using INTSOF.UI.Contract.Templates;
using System.Data;

#endregion

#endregion

namespace Business.RepoManagers
{
    public class TemplatesManager
    {
        /// <summary>
        /// Gets the list of templates
        /// </summary>
        /// <param name="isDeleted">Type of templates i.e. Deleted/Not delete</param>
        /// <returns>List of the Templates</returns>
        public static List<SystemEventTemplatesContract> GetEventSpecificTemplates()
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().GetEventSpecificTemplates();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<SystemEventTemplatesContract> GetTenantSpecificEventTemplates(Int32 tenantId)
        {
            try
            {
                List<Entity.ClientEntity.ComplianceCategory> _lstCategories = ComplianceSetupManager.GetComplianceCategories(tenantId, false);
                List<Entity.ClientEntity.ComplianceItem> _lstItems = ComplianceSetupManager.GetComplianceItems(tenantId, false);
                List<SystemEventTemplatesContract> _lstSystemEventTemplatesContract = BALUtils.GetTemplatesRepoInstance().GetTenantSpecificEventTemplates(tenantId);

                foreach (var systemEventTemplatesContract in _lstSystemEventTemplatesContract)
                {
                    Entity.ClientEntity.ComplianceCategory cCategory = _lstCategories.
                                 Where(cat => cat.ComplianceCategoryID == systemEventTemplatesContract.CategoryId).FirstOrDefault();

                    if (cCategory.IsNotNull())
                        // systemEventTemplatesContract.CategoryName = cCategory.CategoryLabel.IsNullOrEmpty() ? cCategory.CategoryName : cCategory.CategoryLabel;
                        systemEventTemplatesContract.CategoryName = cCategory.CategoryName; //UAT-2582

                    Entity.ClientEntity.ComplianceItem cItem = _lstItems.
                                 Where(item => item.ComplianceItemID == systemEventTemplatesContract.ItemId).FirstOrDefault();

                    if (cItem.IsNotNull())
                        //systemEventTemplatesContract.ItemName = cItem.ItemLabel.IsNullOrEmpty() ? cItem.Name : cItem.ItemLabel;
                        systemEventTemplatesContract.ItemName = cItem.Name; //UAT-2582

                }


                return _lstSystemEventTemplatesContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<CommunicationTemplate> GetOtherTemplates()
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().GetOtherTemplates();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Used to Call the delete the selected template from repository
        /// </summary>
        /// <param name="templateId">Id of the selected template</param>
        public static void DeleteTemplate(Int32 templateId, Int32 currentUserId, Boolean isEventSpecific)
        {
            try
            {
                BALUtils.GetTemplatesRepoInstance().DeleteTemplate(templateId, currentUserId, isEventSpecific);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }

        }

        public static Boolean SaveUpdateTemplate(SystemEventTemplatesContract communicationTemplateContract)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().SaveUpdateTemplate(communicationTemplateContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static SystemEventTemplatesContract GetTemplateDetails(Int32 templateId)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().GetTemplateDetails(templateId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #region Node Level Template Operations

        public static List<NodeTemplatesContract> GetNodeTemplates(Int32 subEventId, Int32 tenantId)
        {
            try
            {
                DataTable _dtTemplates = BALUtils.GetTemplatesRepoInstance().GetNodeTemplates(subEventId);

                if (_dtTemplates.Rows.Count > 0)
                {
                    IEnumerable<DataRow> rows = _dtTemplates.AsEnumerable();
                    return rows.Select(x => new NodeTemplatesContract
                    {

                        CommunicationTemplateId = x["CommunicationTemplateId"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(x["CommunicationTemplateId"]),
                        CommunicationTypeId = Convert.ToInt32(x["CommunicationTypeId"]),
                        CommunicationTypeName = x["CommunicationTypeName"].ToString(),
                        EventId = x["EventId"] is DBNull ? AppConsts.NONE : Convert.ToInt32(x["EventId"]),
                        EventName = x["EventName"].ToString(),
                        SubEventId = Convert.ToInt32(x["SubEventId"]),
                        SubEventName = x["SubEventName"].ToString(),
                        Subject = x["Subject"].ToString(),
                        TemplateContent = x["TemplateContent"].ToString(),
                        TemplateDescription = x["TemplateDescription"].ToString(),
                        TemplateName = x["TemplateName"].ToString(),
                        Frequency = x["Frequency"] is DBNull ? AppConsts.NONE : Convert.ToInt32(x["Frequency"]),
                        NoOfDays = x["NoOfDays"] is DBNull ? AppConsts.NONE : Convert.ToInt32(x["NoOfDays"]),
                    }).ToList();
                }
                else
                    return new List<NodeTemplatesContract>();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static SystemEventTemplatesContract GetNodeTemplateByNotificationMappingId(Int32 nodeNotificationMappingId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().GetNodeTemplateByNotificationMappingId(nodeNotificationMappingId, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Int32 SaveUpdateNodeNotificationTemplates(SystemEventTemplatesContract communicationTemplateContract)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().SaveUpdateNodeNotificationTemplates(communicationTemplateContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        #region 3704:- Agency Hierarchy Specific Templates

        public static List<SystemEventTemplatesContract> GetAgencyHierarchySpecificEventTemplates()
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().GetAgencyHierarchySpecificEventTemplates();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static SystemEventTemplatesContract GetAgencyHierarchyTemplateDetail(Int32 templateId)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().GetAgencyHierarchyTemplateDetail(templateId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUpdateAgencyHierarchyTemplate(SystemEventTemplatesContract communicationTemplateContract)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().SaveUpdateAgencyHierarchyTemplate(communicationTemplateContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean DeleteAgencyHierarchyTemplate(Int32 templateId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().DeleteAgencyHierarchyTemplate(templateId, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<Int32?> GetAlreadyMappedAgencyHierarchyWithSubEvnt(Int32 templateId , Int32? subEventId)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().GetAlreadyMappedAgencyHierarchyWithSubEvnt(templateId,subEventId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        #region Admin Entry Portal Node - Inst. NodeLevel Templates

        public static Int32 GetSubEventIdByCode(String subEventCode)
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpCommunicationSubEvent>().Where(c => c.IsDeleted != true && c.Code == subEventCode).Select(sel => sel.CommunicationSubEventID).FirstOrDefault();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<CommunicationTemplate> GetTemplates(Int32 subEventId)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().GetTemplates(subEventId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        public static List<SystemEventSetting> GetSystemEventSettings(Int32 subEventId)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().GetSystemEventSettings(subEventId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUpdateTemplate(CommunicationTemplate communicationTemplate)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().SaveUpdateAdminEntryNodeTemplate(communicationTemplate);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveUpdateSystemEventSetting(SystemEventSetting systemEventSetting)
        {
            try
            {
                return BALUtils.GetTemplatesRepoInstance().SaveUpdateSystemEventSetting(systemEventSetting);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion
    }
}
