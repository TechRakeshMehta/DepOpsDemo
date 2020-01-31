#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;


#endregion

#region Application Defined

using DAL.Interfaces;
using Entity;
using INTSOF.Utils;
using INTSOF.UI.Contract.Templates;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

#endregion

#endregion

namespace DAL.Repository
{
    /// <summary>
    /// This Class is for managing the Templates that are to be used in different communication type external emails
    /// </summary>
    public class TemplatesRepository : BaseQueueRepository, ITemplates
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ADBMessageDB_DevEntities _dbNavigationMessages;

        #endregion

        #endregion

        #region Constructor

        public TemplatesRepository()
        {
            _dbNavigationMessages = base.ADB_MessageQueueContext;
            _dbNavigationMessages.ContextOptions.LazyLoadingEnabled = false;
        }

        #endregion


        /// <summary>
        /// Returns the list of templates, based on the type required
        /// </summary>
        /// <param name="isDeleted">Type of Tempaltes to fetch</param>
        /// <returns></returns>
        public List<SystemEventTemplatesContract> GetEventSpecificTemplates()
        {
            return _dbNavigationMessages.CommunicationTemplates.Where(ct => ct.IsDeleted == false).Join
                (_dbNavigationMessages.SystemEventSettings.Where(ses => ses.IsDeleted == false),
                 ct => ct.CommunicationTemplateID, ses => ses.CommunicationTemplateID,
                (ct, ses) => new SystemEventTemplatesContract
                {
                    CommunicationTemplateId = ct.CommunicationTemplateID,
                    CommunicationTypeName = ct.lkpCommunicationSubEvent.lkpCommunicationType.Name,
                    EventName = ct.lkpCommunicationSubEvent.lkpCommunicationEvent.Name,
                    SubEventName = ct.lkpCommunicationSubEvent.Name,
                    TemplateName = ct.Name,
                    Subject = ct.Subject,
                    EventSettings = ct.SystemEventSettings.FirstOrDefault()
                }).ToList();

        }

        public List<SystemEventTemplatesContract> GetTenantSpecificEventTemplates(Int32 tenantId)
        {

            return _dbNavigationMessages.CommunicationTemplates.Where(ct => ct.IsDeleted == false).Join
                (_dbNavigationMessages.SystemEventSettings.Where(ses => ses.IsDeleted == false && ses.TenantID == tenantId && ses.SES_NodeNotificationMappingID == null && !ses.InstHierarchyNodeID.HasValue),
                 ct => ct.CommunicationTemplateID, ses => ses.CommunicationTemplateID,
                (ct, ses) => new SystemEventTemplatesContract
                {
                    CommunicationTemplateId = ct.CommunicationTemplateID,
                    CommunicationTypeName = ct.lkpCommunicationSubEvent.lkpCommunicationType.Name,
                    EventName = ct.lkpCommunicationSubEvent.lkpCommunicationEvent.Name,
                    SubEventName = ct.lkpCommunicationSubEvent.Name,
                    TemplateName = ct.Name,
                    Subject = ct.Subject,
                    EventSettings = ct.SystemEventSettings.FirstOrDefault(),
                    CategoryId = ses.ComplianceCategoryID,
                    ItemId = ses.ComplianceItemID,
                    IsNotificationBlocked = ses.IsNotificationBlocked,
                    CommunicationLanguageId = ct.LanguageId
                }).ToList();
        }

        public List<CommunicationTemplate> GetOtherTemplates()
        {
            List<Int32> lstSystemEventSettings = _dbNavigationMessages.SystemEventSettings
                .Where(ses => ses.IsDeleted == false)
                .Select(ses => ses.CommunicationTemplateID).ToList();

            return _dbNavigationMessages.CommunicationTemplates
                .Include("lkpCommunicationSubEvent.lkpCommunicationEvent").Include("lkpCommunicationSubEvent.lkpCommunicationType")
                .Where(ct => ct.IsDeleted == false && ct.lkpCommunicationSubEvent.IsDeleted == false && !lstSystemEventSettings.Contains(ct.CommunicationTemplateID)).ToList();
        }

        /// <summary>
        /// Used to delete the selected template
        /// </summary>
        /// <param name="templateId">Id of the selected template</param>
        public void DeleteTemplate(Int32 templateId, Int32 currentUserId, Boolean isEventSpecific)
        {
            DateTime dtModificationDateTime = DateTime.Now;
            if (isEventSpecific)
            {
                SystemEventSetting _systemEventSetting = _dbNavigationMessages.SystemEventSettings
                    .Where(ses => ses.CommunicationTemplateID == templateId && ses.IsDeleted == false).FirstOrDefault();

                if (_systemEventSetting.IsNotNull())
                {
                    _systemEventSetting.IsDeleted = true;
                    _systemEventSetting.ModifiedBy = currentUserId;
                    _systemEventSetting.ModifiedOn = dtModificationDateTime;
                }
            }
            CommunicationTemplate communicationTemplate = _dbNavigationMessages.CommunicationTemplates.Where(cTemplate => cTemplate.CommunicationTemplateID.Equals(templateId)).FirstOrDefault();
            communicationTemplate.IsDeleted = true;
            communicationTemplate.ModifiedByID = currentUserId;
            communicationTemplate.ModifiedOn = dtModificationDateTime;

            _dbNavigationMessages.SaveChanges();
        }

        /// <summary>
        /// Saves the new Communication Template in the database
        /// </summary>
        /// <param name="communicationTemplate"></param>
        public Boolean SaveUpdateTemplate(SystemEventTemplatesContract communicationTemplateContract)
        {
            if (communicationTemplateContract.CommunicationTemplateId == 0)
            {
                DateTime _dtCreationDateTime = DateTime.Now;

                CommunicationTemplate _communicationTemplate = new CommunicationTemplate
                           {
                               CommunicationSubEventID = communicationTemplateContract.SubEventId,
                               Name = communicationTemplateContract.TemplateName,
                               Subject = communicationTemplateContract.Subject,
                               Description = communicationTemplateContract.TemplateDescription,
                               Content = communicationTemplateContract.TemplateContent,
                               IsDeleted = false,
                               CreatedByID = communicationTemplateContract.CurrentUserId,
                               CreatedOn = _dtCreationDateTime,
                               LanguageId = communicationTemplateContract.CommunicationLanguageId
                           };

                _dbNavigationMessages.CommunicationTemplates.AddObject(_communicationTemplate);

                if (communicationTemplateContract.EventSettings.IsNotNull())
                {
                    SystemEventSetting _systemEventSetting = new SystemEventSetting();

                    _systemEventSetting.TenantID = communicationTemplateContract.EventSettings.TenantID;
                    List<SystemEventSetting> sysEvntSttngInDB = new List<SystemEventSetting>();
                    if (communicationTemplateContract.EventSettings.ComplianceCategoryID > 0)
                    {
                        sysEvntSttngInDB = _dbNavigationMessages.SystemEventSettings
                                       .Where(cond => cond.CommunicationSubEventID == communicationTemplateContract.SubEventId && cond.IsDeleted != true
                                           && cond.ComplianceCategoryID == communicationTemplateContract.EventSettings.ComplianceCategoryID && cond.TenantID == communicationTemplateContract.EventSettings.TenantID).ToList();
                        if (communicationTemplateContract.EventSettings.ComplianceItemID > 0)
                        {
                            sysEvntSttngInDB = sysEvntSttngInDB.Where(cond => cond.ComplianceItemID == communicationTemplateContract.EventSettings.ComplianceItemID).ToList();
                        }
                    }
                    if (!sysEvntSttngInDB.IsNullOrEmpty())
                    {
                        return false;
                    }

                    if (communicationTemplateContract.EventSettings.ComplianceCategoryID > 0)
                        _systemEventSetting.ComplianceCategoryID = communicationTemplateContract.EventSettings.ComplianceCategoryID;

                    if (communicationTemplateContract.EventSettings.ComplianceItemID > 0)
                        _systemEventSetting.ComplianceItemID = communicationTemplateContract.EventSettings.ComplianceItemID;

                    _systemEventSetting.CommunicationTemplateID = _communicationTemplate.CommunicationTemplateID;
                    _systemEventSetting.CommunicationSubEventID = communicationTemplateContract.SubEventId;
                    _systemEventSetting.DaysBefore = communicationTemplateContract.EventSettings.DaysBefore;
                    _systemEventSetting.Frequency = communicationTemplateContract.EventSettings.Frequency;
                    _systemEventSetting.NoOfDays = communicationTemplateContract.EventSettings.NoOfDays;
                    _systemEventSetting.IsDeleted = false;
                    _systemEventSetting.CreatedBy = communicationTemplateContract.CurrentUserId;
                    _systemEventSetting.CreatedOn = _dtCreationDateTime;
                    //UAT-3656
                    _systemEventSetting.IsNotificationBlocked = communicationTemplateContract.EventSettings.IsNotificationBlocked;



                    _dbNavigationMessages.SystemEventSettings.AddObject(_systemEventSetting);
                }
            }
            else
            {
                // UPDATE CASE
                DateTime dtModificationDateTime = DateTime.Now;
                CommunicationTemplate cTemplate = _dbNavigationMessages.CommunicationTemplates.Where(cmnTemplate => cmnTemplate.CommunicationTemplateID.Equals(communicationTemplateContract.CommunicationTemplateId)).FirstOrDefault();

                // TEMPLATE UPDATE
                if (cTemplate.IsNotNull())
                {
                    cTemplate.Name = communicationTemplateContract.TemplateName;
                    cTemplate.Description = communicationTemplateContract.TemplateDescription;
                    cTemplate.Subject = communicationTemplateContract.Subject;
                    cTemplate.Content = communicationTemplateContract.TemplateContent;
                    cTemplate.ModifiedByID = communicationTemplateContract.CurrentUserId;
                    cTemplate.ModifiedOn = dtModificationDateTime;
                }

                //  SYSTEM EVENT SETTIGS UPDATE CASE
                if (communicationTemplateContract.EventSettings.IsNotNull() && communicationTemplateContract.EventSettings.SES_ID.IsNotNull() && communicationTemplateContract.EventSettings.SES_ID > AppConsts.NONE)
                {
                    SystemEventSetting _systemEventSettings = _dbNavigationMessages.SystemEventSettings
                            .Where(ses => ses.SES_ID == communicationTemplateContract.EventSettings.SES_ID).FirstOrDefault();

                    if (_systemEventSettings.IsNotNull())
                    {
                        _systemEventSettings.ModifiedBy = communicationTemplateContract.CurrentUserId;
                        _systemEventSettings.ModifiedOn = dtModificationDateTime;
                        _systemEventSettings.Frequency = communicationTemplateContract.EventSettings.Frequency;
                        _systemEventSettings.NoOfDays = communicationTemplateContract.EventSettings.NoOfDays;
                        _systemEventSettings.DaysBefore = communicationTemplateContract.EventSettings.DaysBefore;
                        _systemEventSettings.IsNotificationBlocked = communicationTemplateContract.EventSettings.IsNotificationBlocked;
                    }
                }
            }
            _dbNavigationMessages.SaveChanges();
            return true;
        }

        /// <summary>
        /// Get the list of category ids for which templates have been already defined.
        /// </summary>
        /// <returns></returns>
        public List<Int32?> GetUsedCategories(Int32 tenantId)
        {
            return _dbNavigationMessages.SystemEventSettings
                .Where(ses => ses.IsDeleted == false && ses.ComplianceCategoryID != null && ses.ComplianceItemID == null
                    && ses.TenantID == tenantId).Select(ses => ses.ComplianceCategoryID).ToList();
        }

        /// <summary>
        /// Get template defined at category level in template maintenance form, on selection of category
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public SystemEventSetting GetCategoryLevelTemplate(Int32 categoryId, Int32 subEventId, Int32 tenantId)
        {
            SystemEventSetting _systemEventSetting = _dbNavigationMessages.SystemEventSettings.Include("CommunicationTemplate.lkpCommunicationSubEvent")
                .Where(ses => ses.ComplianceCategoryID == categoryId && ses.ComplianceItemID == null
                    && ses.TenantID == tenantId && ses.CommunicationSubEventID == subEventId && ses.IsDeleted == false)
                .FirstOrDefault();

            return _systemEventSetting;
        }

        public SystemEventTemplatesContract GetTemplateDetails(Int32 templateId)
        {
            SystemEventTemplatesContract _systemEventTemplatesContract = new SystemEventTemplatesContract();

            CommunicationTemplate _cTemplate = _dbNavigationMessages.CommunicationTemplates
                .Include("lkpCommunicationSubEvent").Include("SystemEventSettings")
                       .Where(ct => ct.CommunicationTemplateID == templateId).FirstOrDefault();

            _systemEventTemplatesContract.TemplateName = _cTemplate.Name;
            _systemEventTemplatesContract.Subject = _cTemplate.Subject;
            _systemEventTemplatesContract.TemplateDescription = _cTemplate.Description;
            _systemEventTemplatesContract.TemplateContent = _cTemplate.Content;
            _systemEventTemplatesContract.CommunicationLanguageId = _cTemplate.LanguageId;

            _systemEventTemplatesContract.EventId = _cTemplate.lkpCommunicationSubEvent.CommunicationEventID;
            _systemEventTemplatesContract.CommunicationTypeId = _cTemplate.lkpCommunicationSubEvent.CommunicationTypeID;
            _systemEventTemplatesContract.SubEventId = _cTemplate.lkpCommunicationSubEvent.CommunicationSubEventID;

            if (_cTemplate.SystemEventSettings.IsNotNull() && _cTemplate.SystemEventSettings.FirstOrDefault().IsNotNull())
            {
                SystemEventSetting _sysEventSettings = _cTemplate.SystemEventSettings.FirstOrDefault();
                _systemEventTemplatesContract.EventSettings = new SystemEventSetting
               {
                   TenantID = _sysEventSettings.TenantID,
                   ComplianceCategoryID = _sysEventSettings.ComplianceCategoryID,
                   ComplianceItemID = _sysEventSettings.ComplianceItemID,
                   NoOfDays = _sysEventSettings.NoOfDays,
                   Frequency = _sysEventSettings.Frequency,
                   SES_ID = _sysEventSettings.SES_ID,
                   SES_NodeNotificationMappingID = _sysEventSettings.SES_NodeNotificationMappingID,
                   DaysBefore = _sysEventSettings.DaysBefore,
                   IsNotificationBlocked = _sysEventSettings.IsNotificationBlocked
               };
            }
            return _systemEventTemplatesContract;
        }

        public SystemEventTemplatesContract GetNodeTemplateByNotificationMappingId(Int32 nodeNotificationMappingId, Int32 tenantId)
        {
            SystemEventTemplatesContract _systemEventTemplatesContract = new SystemEventTemplatesContract();

            SystemEventSetting _systemEventSettings = _dbNavigationMessages.SystemEventSettings.Include("CommunicationTemplate.lkpCommunicationSubEvent")
                                                      .Where(ses => ses.SES_NodeNotificationMappingID == nodeNotificationMappingId
                                                      && ses.IsDeleted == false && ses.TenantID == tenantId).FirstOrDefault();

            if (_systemEventSettings.IsNullOrEmpty())
                return new SystemEventTemplatesContract();

            _systemEventTemplatesContract.TemplateName = _systemEventSettings.CommunicationTemplate.Name;
            _systemEventTemplatesContract.Subject = _systemEventSettings.CommunicationTemplate.Subject;
            _systemEventTemplatesContract.TemplateDescription = _systemEventSettings.CommunicationTemplate.Description;
            _systemEventTemplatesContract.TemplateContent = _systemEventSettings.CommunicationTemplate.Content;
            _systemEventTemplatesContract.CommunicationTemplateId = _systemEventSettings.CommunicationTemplateID;

            _systemEventTemplatesContract.EventId = _systemEventSettings.CommunicationTemplate.lkpCommunicationSubEvent.CommunicationEventID;
            _systemEventTemplatesContract.CommunicationTypeId = _systemEventSettings.CommunicationTemplate.lkpCommunicationSubEvent.CommunicationTypeID;
            _systemEventTemplatesContract.SubEventId = _systemEventSettings.CommunicationTemplate.lkpCommunicationSubEvent.CommunicationSubEventID;

            if (_systemEventSettings.IsNotNull())
            {
                _systemEventTemplatesContract.EventSettings = new SystemEventSetting
               {
                   TenantID = _systemEventSettings.TenantID,
                   NoOfDays = _systemEventSettings.NoOfDays,
                   Frequency = _systemEventSettings.Frequency,
                   SES_ID = _systemEventSettings.SES_ID,
                   SES_NodeNotificationMappingID = _systemEventSettings.SES_NodeNotificationMappingID
               };
            }
            return _systemEventTemplatesContract;
        }

        public DataTable GetNodeTemplates(Int32 subEventId)
        {
            EntityConnection connection = _dbNavigationMessages.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetNodeTemplates", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SubEventId", subEventId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }

            return new DataTable();
        }

        public Int32 SaveUpdateNodeNotificationTemplates(SystemEventTemplatesContract communicationTemplateContract)
        {
            CommunicationTemplate _communicationTemplate = null;
            if (communicationTemplateContract.CommunicationTemplateId == 0)
            {
                DateTime _dtCreationDateTime = DateTime.Now;

                _communicationTemplate = new CommunicationTemplate
               {
                   CommunicationSubEventID = communicationTemplateContract.SubEventId,
                   Name = communicationTemplateContract.TemplateName,
                   Subject = communicationTemplateContract.Subject,
                   Description = communicationTemplateContract.TemplateDescription,
                   Content = communicationTemplateContract.TemplateContent,
                   IsDeleted = false,
                   CreatedByID = communicationTemplateContract.CurrentUserId,
                   CreatedOn = _dtCreationDateTime,
                   LanguageId = communicationTemplateContract.CommunicationLanguageId
               };

                _dbNavigationMessages.CommunicationTemplates.AddObject(_communicationTemplate);


                SystemEventSetting _systemEventSetting = new SystemEventSetting();

                _systemEventSetting.TenantID = communicationTemplateContract.EventSettings.TenantID;

                _systemEventSetting.CommunicationTemplateID = _communicationTemplate.CommunicationTemplateID;
                _systemEventSetting.CommunicationSubEventID = communicationTemplateContract.SubEventId;
                _systemEventSetting.Frequency = communicationTemplateContract.EventSettings.Frequency;
                _systemEventSetting.NoOfDays = communicationTemplateContract.EventSettings.NoOfDays;
                _systemEventSetting.IsDeleted = false;
                _systemEventSetting.CreatedBy = communicationTemplateContract.CurrentUserId;
                _systemEventSetting.CreatedOn = _dtCreationDateTime;
                _systemEventSetting.SES_NodeNotificationMappingID = communicationTemplateContract.EventSettings.SES_NodeNotificationMappingID;

                _dbNavigationMessages.SystemEventSettings.AddObject(_systemEventSetting);
                _dbNavigationMessages.SaveChanges();
                return _communicationTemplate.CommunicationTemplateID;
            }
            else
            {
                // UPDATE CASE
                DateTime dtModificationDateTime = DateTime.Now;
                _communicationTemplate = _dbNavigationMessages.CommunicationTemplates.Include("SystemEventSettings")
                    .Where(cmnTemplate => cmnTemplate.CommunicationTemplateID.Equals(communicationTemplateContract.CommunicationTemplateId)).FirstOrDefault();

                // TEMPLATE UPDATE
                if (_communicationTemplate.IsNotNull())
                {
                    _communicationTemplate.Name = communicationTemplateContract.TemplateName;
                    _communicationTemplate.Description = communicationTemplateContract.TemplateDescription;
                    _communicationTemplate.Subject = communicationTemplateContract.Subject;
                    _communicationTemplate.Content = communicationTemplateContract.TemplateContent;
                    _communicationTemplate.ModifiedByID = communicationTemplateContract.CurrentUserId;
                    _communicationTemplate.ModifiedOn = dtModificationDateTime;
                }

                SystemEventSetting _systemEventSettings = _communicationTemplate.SystemEventSettings.Where(ses => ses.CommunicationTemplateID ==
                    _communicationTemplate.CommunicationTemplateID && ses.IsDeleted == false && ses.TenantID == communicationTemplateContract.EventSettings.TenantID)
                    .FirstOrDefault();

                if (_systemEventSettings.IsNotNull())
                {
                    _systemEventSettings.ModifiedBy = communicationTemplateContract.CurrentUserId;
                    _systemEventSettings.ModifiedOn = dtModificationDateTime;
                    _systemEventSettings.Frequency = communicationTemplateContract.EventSettings.Frequency;
                    _systemEventSettings.NoOfDays = communicationTemplateContract.EventSettings.NoOfDays;
                    _systemEventSettings.SES_NodeNotificationMappingID = communicationTemplateContract.EventSettings.SES_NodeNotificationMappingID;
                }
                _dbNavigationMessages.SaveChanges();
                return communicationTemplateContract.CommunicationTemplateId;
            }
        }

        //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
        Dictionary<String, String> ITemplates.GetInvitationEmailContent(Int32 TenantID, String CommSubEventCode)
        {
            Int32 CommSubEventID = _dbNavigationMessages.lkpCommunicationSubEvents.Where(cond => cond.Code == CommSubEventCode && cond.IsDeleted != true).Select(sel => sel.CommunicationSubEventID).FirstOrDefault();
            Int32 communicationTemplateID = 0;
            CommunicationTemplate objCommunicationTemplate;
            SystemEventSetting objSystemEventSettings = _dbNavigationMessages.SystemEventSettings.Where(cond => cond.CommunicationSubEventID == CommSubEventID && cond.TenantID == TenantID && cond.IsDeleted != true).FirstOrDefault();
            if (!objSystemEventSettings.IsNullOrEmpty())
            {
                communicationTemplateID = objSystemEventSettings.CommunicationTemplateID;
            }
            if (communicationTemplateID != AppConsts.NONE)
            {
                objCommunicationTemplate = _dbNavigationMessages.CommunicationTemplates.Where(con => con.CommunicationTemplateID == communicationTemplateID && con.CommunicationSubEventID == CommSubEventID && con.IsDeleted != true).FirstOrDefault();
            }
            else
            {
                objCommunicationTemplate = _dbNavigationMessages.CommunicationTemplates.Where(con => con.CommunicationSubEventID == CommSubEventID && con.IsDeleted != true).FirstOrDefault();
            }
            var _dic = new Dictionary<String, String>();
            if (!objCommunicationTemplate.IsNullOrEmpty())
            {
                _dic.Add(objCommunicationTemplate.Subject, objCommunicationTemplate.Content);
            } return _dic;
        }

        #region UAT-3704

        //UAT-3704 :- Agency hierarchy specific templates
        List<SystemEventTemplatesContract> ITemplates.GetAgencyHierarchySpecificEventTemplates()
        {
            List<SystemEventTemplatesContract> lstAgencyHierarchyTemplates = new List<SystemEventTemplatesContract>();

            EntityConnection connection = _dbNavigationMessages.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            //new SqlParameter("@AgencyHierarchyRootNodeID",agencyHierarchyRootNodeId),
                            //new SqlParameter("@FromDate", fromDate),
                            //new SqlParameter("@ToDate", toDate)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchySpecificEventTemplates", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SystemEventTemplatesContract systemEventTemplatesContract = new SystemEventTemplatesContract();

                            systemEventTemplatesContract.CommunicationTemplateId = dr["CommunicationTemplateID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["CommunicationTemplateID"]);
                            systemEventTemplatesContract.EventId = dr["CommunicationEventID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["CommunicationEventID"]);
                            systemEventTemplatesContract.SubEventId = dr["CommunicationSubEventID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["CommunicationSubEventID"]);
                            systemEventTemplatesContract.CommunicationTypeId = dr["CommunicationTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["CommunicationTypeID"]);
                            systemEventTemplatesContract.CommunicationTypeName = dr["CommunicationTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CommunicationTypeName"]);
                            systemEventTemplatesContract.EventName = dr["EventName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["EventName"]);
                            systemEventTemplatesContract.SubEventName = dr["SubEventName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SubEventName"]);
                            systemEventTemplatesContract.TemplateName = dr["TemplateName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TemplateName"]);
                            systemEventTemplatesContract.Subject = dr["Subject"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Subject"]);
                            systemEventTemplatesContract.AgencyHierarchyID = dr["AgencyHierarchyID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyHierarchyID"]);
                            systemEventTemplatesContract.AgencyHierarchy = dr["AgencyHierarchy"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyHierarchy"]);

                            lstAgencyHierarchyTemplates.Add(systemEventTemplatesContract);
                        }
                    }
                }
            }
            return lstAgencyHierarchyTemplates;
        }

        SystemEventTemplatesContract ITemplates.GetAgencyHierarchyTemplateDetail(Int32 templateId)
        {
            SystemEventTemplatesContract _systemEventTemplatesContract = new SystemEventTemplatesContract();

            CommunicationTemplate _cTemplate = _dbNavigationMessages.CommunicationTemplates
                .Include("lkpCommunicationSubEvent").Include("SystemEventSettings")
                       .Where(ct => ct.CommunicationTemplateID == templateId).FirstOrDefault();

            _systemEventTemplatesContract.TemplateName = _cTemplate.Name;
            _systemEventTemplatesContract.Subject = _cTemplate.Subject;
            _systemEventTemplatesContract.TemplateDescription = _cTemplate.Description;
            _systemEventTemplatesContract.TemplateContent = _cTemplate.Content;
            _systemEventTemplatesContract.CommunicationLanguageId = _cTemplate.LanguageId;
            _systemEventTemplatesContract.EventId = _cTemplate.lkpCommunicationSubEvent.CommunicationEventID;
            _systemEventTemplatesContract.CommunicationTypeId = _cTemplate.lkpCommunicationSubEvent.CommunicationTypeID;
            _systemEventTemplatesContract.SubEventId = _cTemplate.lkpCommunicationSubEvent.CommunicationSubEventID;

            _systemEventTemplatesContract.lstAgencyHierarcEventSettings = new List<SystemEventSetting>();
            _systemEventTemplatesContract.lstAgencyHierarcEventSettings = (_cTemplate.SystemEventSettings.IsNotNull() && _cTemplate.SystemEventSettings.ToList().IsNotNull()) ? _cTemplate.SystemEventSettings.ToList() : new List<SystemEventSetting>();

            return _systemEventTemplatesContract;
        }


        public Boolean SaveUpdateAgencyHierarchyTemplate(SystemEventTemplatesContract communicationTemplateContract)
        {
            //List<SystemEventSetting> lstAlreadyAddedSettingFrSubEvent = _dbNavigationMessages.SystemEventSettings.Where(con => con.IsDeleted != true && con.CommunicationSubEventID == communicationTemplateContract.SubEventId && con.TenantID == null && con.AgencyHierarchyID != null).ToList();

            //if (!lstAlreadyAddedSettingFrSubEvent.IsNullOrEmpty() && communicationTemplateContract.CommunicationTemplateId == 0)
            //{
            //    List<Int32?> lstHierarchyIdsMapped = lstAlreadyAddedSettingFrSubEvent.Select(sel => sel.AgencyHierarchyID).ToList();

            //    foreach (SystemEventSetting agencyHierarcEventSettings in communicationTemplateContract.lstAgencyHierarcEventSettings)
            //    {
            //        if (lstHierarchyIdsMapped.Contains(agencyHierarcEventSettings.AgencyHierarchyID))
            //            return false;
            //    }
            //}

            // Add new //
            if (communicationTemplateContract.CommunicationTemplateId == 0)
            {
                DateTime _dtCreationDateTime = DateTime.Now;

                CommunicationTemplate _communicationTemplate = new CommunicationTemplate
                           {
                               CommunicationSubEventID = communicationTemplateContract.SubEventId,
                               Name = communicationTemplateContract.TemplateName,
                               Subject = communicationTemplateContract.Subject,
                               Description = communicationTemplateContract.TemplateDescription,
                               Content = communicationTemplateContract.TemplateContent,
                               IsDeleted = false,
                               CreatedByID = communicationTemplateContract.CurrentUserId,
                               CreatedOn = _dtCreationDateTime,
                               LanguageId = communicationTemplateContract.CommunicationLanguageId
                           };

                _dbNavigationMessages.CommunicationTemplates.AddObject(_communicationTemplate);

                if (communicationTemplateContract.lstAgencyHierarcEventSettings.IsNotNull())
                {
                    foreach (SystemEventSetting agencyHierarcEventSettings in communicationTemplateContract.lstAgencyHierarcEventSettings)
                    {
                        SystemEventSetting _systemEventSetting = new SystemEventSetting();
                        _systemEventSetting.TenantID = agencyHierarcEventSettings.TenantID;
                        _systemEventSetting.ComplianceCategoryID = agencyHierarcEventSettings.ComplianceCategoryID;
                        _systemEventSetting.ComplianceItemID = agencyHierarcEventSettings.ComplianceItemID;
                        _systemEventSetting.CommunicationTemplateID = _communicationTemplate.CommunicationTemplateID;
                        _systemEventSetting.CommunicationSubEventID = communicationTemplateContract.SubEventId;
                        _systemEventSetting.DaysBefore = agencyHierarcEventSettings.DaysBefore;
                        _systemEventSetting.Frequency = agencyHierarcEventSettings.Frequency;
                        _systemEventSetting.NoOfDays = agencyHierarcEventSettings.NoOfDays;
                        _systemEventSetting.IsDeleted = false;
                        _systemEventSetting.CreatedBy = communicationTemplateContract.CurrentUserId;
                        _systemEventSetting.CreatedOn = _dtCreationDateTime;
                        _systemEventSetting.IsNotificationBlocked = agencyHierarcEventSettings.IsNotificationBlocked;
                        _systemEventSetting.AgencyHierarchyID = agencyHierarcEventSettings.AgencyHierarchyID;

                        _dbNavigationMessages.SystemEventSettings.AddObject(_systemEventSetting);
                    }
                }
            }

            else
            {
                // UPDATE CASE
                DateTime dtModificationDateTime = DateTime.Now;
                CommunicationTemplate cTemplate = _dbNavigationMessages.CommunicationTemplates.Where(cmnTemplate => cmnTemplate.CommunicationTemplateID.Equals(communicationTemplateContract.CommunicationTemplateId)).FirstOrDefault();

                // TEMPLATE UPDATE
                if (cTemplate.IsNotNull())
                {
                    cTemplate.Name = communicationTemplateContract.TemplateName;
                    cTemplate.Description = communicationTemplateContract.TemplateDescription;
                    cTemplate.Subject = communicationTemplateContract.Subject;
                    cTemplate.Content = communicationTemplateContract.TemplateContent;
                    cTemplate.ModifiedByID = communicationTemplateContract.CurrentUserId;
                    cTemplate.ModifiedOn = dtModificationDateTime;
                }

                //  SYSTEM EVENT SETTIGS UPDATE CASE

                //communicationTemplateContract.lstAgencyHierarcEventSettings

                List<SystemEventSetting> lstExistingSystemEventSettings = _dbNavigationMessages.SystemEventSettings.Where(con => con.IsDeleted != true && con.TenantID == null && con.CommunicationTemplateID == communicationTemplateContract.CommunicationTemplateId).ToList();

                if (!communicationTemplateContract.lstAgencyHierarcEventSettings.IsNullOrEmpty())
                {
                    foreach (SystemEventSetting agencyHierarchySystemEventSetting in communicationTemplateContract.lstAgencyHierarcEventSettings)
                    {
                        SystemEventSetting _systemEventSettings = new SystemEventSetting();
                        if (agencyHierarchySystemEventSetting.SES_ID > AppConsts.NONE)
                        {
                            //to update existing
                            _systemEventSettings = _dbNavigationMessages.SystemEventSettings.Where(ses => ses.SES_ID == agencyHierarchySystemEventSetting.SES_ID).FirstOrDefault();
                            if (_systemEventSettings.IsNotNull())
                            {
                                _systemEventSettings.TenantID = agencyHierarchySystemEventSetting.TenantID;
                                _systemEventSettings.ComplianceCategoryID = agencyHierarchySystemEventSetting.ComplianceCategoryID;
                                _systemEventSettings.ComplianceItemID = agencyHierarchySystemEventSetting.ComplianceItemID;
                                _systemEventSettings.CommunicationTemplateID = cTemplate.CommunicationTemplateID;
                                _systemEventSettings.CommunicationSubEventID = communicationTemplateContract.SubEventId;
                                _systemEventSettings.DaysBefore = agencyHierarchySystemEventSetting.DaysBefore;
                                _systemEventSettings.Frequency = agencyHierarchySystemEventSetting.Frequency;
                                _systemEventSettings.NoOfDays = agencyHierarchySystemEventSetting.NoOfDays;
                                _systemEventSettings.IsDeleted = false;
                                _systemEventSettings.ModifiedBy = communicationTemplateContract.CurrentUserId;
                                _systemEventSettings.ModifiedOn = dtModificationDateTime;
                                _systemEventSettings.IsNotificationBlocked = agencyHierarchySystemEventSetting.IsNotificationBlocked;
                                _systemEventSettings.AgencyHierarchyID = agencyHierarchySystemEventSetting.AgencyHierarchyID;
                            }

                            lstExistingSystemEventSettings = lstExistingSystemEventSettings.Where(con => con.SES_ID != agencyHierarchySystemEventSetting.SES_ID).ToList();
                        }
                        else
                        {
                            //to add new 
                            _systemEventSettings.TenantID = agencyHierarchySystemEventSetting.TenantID;
                            _systemEventSettings.ComplianceCategoryID = agencyHierarchySystemEventSetting.ComplianceCategoryID;
                            _systemEventSettings.ComplianceItemID = agencyHierarchySystemEventSetting.ComplianceItemID;
                            _systemEventSettings.CommunicationTemplateID = cTemplate.CommunicationTemplateID;
                            _systemEventSettings.CommunicationSubEventID = communicationTemplateContract.SubEventId;
                            _systemEventSettings.DaysBefore = agencyHierarchySystemEventSetting.DaysBefore;
                            _systemEventSettings.Frequency = agencyHierarchySystemEventSetting.Frequency;
                            _systemEventSettings.NoOfDays = agencyHierarchySystemEventSetting.NoOfDays;
                            _systemEventSettings.IsDeleted = false;
                            _systemEventSettings.CreatedBy = communicationTemplateContract.CurrentUserId;
                            _systemEventSettings.CreatedOn = dtModificationDateTime;
                            _systemEventSettings.IsNotificationBlocked = agencyHierarchySystemEventSetting.IsNotificationBlocked;
                            _systemEventSettings.AgencyHierarchyID = agencyHierarchySystemEventSetting.AgencyHierarchyID;

                            _dbNavigationMessages.SystemEventSettings.AddObject(_systemEventSettings);
                        }
                    }

                }

                //To be removed system event settings
                if (!lstExistingSystemEventSettings.IsNullOrEmpty() & lstExistingSystemEventSettings.Count() > AppConsts.NONE)
                {
                    foreach (SystemEventSetting agencyHierarchySystemEventSetting in lstExistingSystemEventSettings)
                    {
                        agencyHierarchySystemEventSetting.IsDeleted = true;
                        agencyHierarchySystemEventSetting.ModifiedOn = dtModificationDateTime;
                        agencyHierarchySystemEventSetting.ModifiedBy = communicationTemplateContract.CurrentUserId;
                    }
                }

            }

            if (_dbNavigationMessages.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean ITemplates.DeleteAgencyHierarchyTemplate(Int32 templateId, Int32 currentUserId)
        {
            DateTime dtModificationDateTime = DateTime.Now;

            List<SystemEventSetting> lstSystemEventSetting = _dbNavigationMessages.SystemEventSettings
                .Where(con => con.CommunicationTemplateID == templateId && con.IsDeleted == false && con.TenantID == null && con.AgencyHierarchyID != null).ToList();

            foreach (SystemEventSetting systemEventSetting in lstSystemEventSetting)
            {
                systemEventSetting.IsDeleted = true;
                systemEventSetting.ModifiedBy = currentUserId;
                systemEventSetting.ModifiedOn = dtModificationDateTime;
            }

            CommunicationTemplate communicationTemplate = _dbNavigationMessages.CommunicationTemplates.Where(cTemplate => cTemplate.CommunicationTemplateID.Equals(templateId)).FirstOrDefault();
            communicationTemplate.IsDeleted = true;
            communicationTemplate.ModifiedByID = currentUserId;
            communicationTemplate.ModifiedOn = dtModificationDateTime;

            if (_dbNavigationMessages.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        List<Int32?> ITemplates.GetAlreadyMappedAgencyHierarchyWithSubEvnt(Int32 templateId, Int32? subEvntId)
        {
            List<Int32?> lstAgencyHierarchyMapped = new List<Int32?>();
            //List<SystemEventSetting> lstAlreadyAddedSettingFrSubEvent 
            if (!templateId.IsNullOrEmpty() && templateId > AppConsts.NONE)
                lstAgencyHierarchyMapped = _dbNavigationMessages.SystemEventSettings
                                        .Where(con => con.IsDeleted != true && con.CommunicationSubEventID == subEvntId && con.TenantID == null && con.AgencyHierarchyID != null && con.CommunicationTemplateID != templateId).Select(sel => sel.AgencyHierarchyID).ToList();
            else
                lstAgencyHierarchyMapped = _dbNavigationMessages.SystemEventSettings
                                        .Where(con => con.IsDeleted != true && con.CommunicationSubEventID == subEvntId && con.TenantID == null && con.AgencyHierarchyID != null).Select(sel => sel.AgencyHierarchyID).ToList();
            
            return lstAgencyHierarchyMapped;
        }
        #endregion

        #region Admin Entry Portal Node - Inst. NodeLevel Templates

        List<CommunicationTemplate> ITemplates.GetTemplates(Int32 subEventId)
        {
            return _dbNavigationMessages.CommunicationTemplates.Where(con => con.IsDeleted != true && con.CommunicationSubEventID == subEventId).ToList();
        }

        List<SystemEventSetting> ITemplates.GetSystemEventSettings(Int32 subEventId)
        {
            return _dbNavigationMessages.SystemEventSettings.Where(con => con.IsDeleted != true && con.CommunicationSubEventID == subEventId).ToList();
        }

        Boolean ITemplates.SaveUpdateAdminEntryNodeTemplate(CommunicationTemplate communicationTemplate)
        {
            if (communicationTemplate.CommunicationTemplateID <= AppConsts.NONE)
            {
                _dbNavigationMessages.CommunicationTemplates.AddObject(communicationTemplate);
            }

            if (_dbNavigationMessages.SaveChanges() > AppConsts.NONE)
                return true;
            return false;

        }

        Boolean ITemplates.SaveUpdateSystemEventSetting(SystemEventSetting systemEventSetting)
        {
            if (systemEventSetting.SES_ID <= AppConsts.NONE)
            {
                _dbNavigationMessages.SystemEventSettings.AddObject(systemEventSetting);
            }

            if (_dbNavigationMessages.SaveChanges() > AppConsts.NONE)
                return true;
            return false;

        }

        #endregion
    }
}
