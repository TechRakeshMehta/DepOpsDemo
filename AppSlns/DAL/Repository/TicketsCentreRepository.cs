using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract.TicketsCentre;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using INTSOF.UI.Contract.TicketsCentre;
using INTSOF.UI.Contract.Templates;


namespace DAL.Repository
{
    public class TicketsCentreRepository : ClientBaseRepository, ITicketsCentreRepository
    {
        #region Variables

        #region public Variables

        #endregion

        #region Private Variables
        private ADB_LibertyUniversity_ReviewEntities _dbNavigation;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public TicketsCentreRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbNavigation = base.ClientDBContext;
        }
        #endregion
        #region Method Imeplentation
        DataTable ITicketsCentreRepository.GetTickets(int LOBID, CustomPagingArgsContract obj, Int32 ViewAllTicketsPermission, Int32 ClientID, Int32 UserID, Int64 workItemID
                                    , Int32 timeZoneOffset, TicketSearchContract ticketSearchContract)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_GetTickets", con);
                command.CommandType = CommandType.StoredProcedure;
                if (obj != null)
                {
                    //obj = new CustomPagingArgsContract();
                    command.Parameters.AddWithValue("@filteringSortingData", obj.XML);
                }
                //Comments By [SS]
                //command.Parameters.AddWithValue("@LOBID", LOBID);
                command.Parameters.AddWithValue("@ViewAllTicketsPermission", ViewAllTicketsPermission);
                command.Parameters.AddWithValue("@ClientID", ClientID);
                command.Parameters.AddWithValue("@UserID", UserID);
                //Comments By [SS]
                //command.Parameters.AddWithValue("@WorkItemID", workItemID);
                //command.Parameters.AddWithValue("@timeZoneOffset", timeZoneOffset);

                command.Parameters.AddWithValue("@locationIDs", ticketSearchContract.LocationIDs);
                command.Parameters.AddWithValue("@TicketStatusID", ticketSearchContract.TicketStatusID);
                command.Parameters.AddWithValue("@TicketSeverityID", ticketSearchContract.TicketSeverityID);
                command.Parameters.AddWithValue("@TicketTypeID", ticketSearchContract.TicketTypeID);
                command.Parameters.AddWithValue("@IsEnroller", ticketSearchContract.IsEnroller);
                command.Parameters.AddWithValue("@EnrollerPermissionCode_FullACCess", LkpPermission.FullAccess.GetStringValue().ToLower());
                
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 1)
                {
                    if (obj != null && ds.Tables[0].Rows.Count > 0)
                    {
                        obj.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        obj.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                    return ds.Tables[1];
                }
            }

            return new DataTable();
        }
        Int32 ITicketsCentreRepository.AddTicket(TicketsContract TicketContract, DataTable ListClientSrvcStepMappingID, DataTable SendToList, DataTable ListTicketDocuments, Int32 LoggedInUserID)
        {
            try
            {
                EntityConnection connection = _dbNavigation.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {

                    SqlCommand command = new SqlCommand("ams.usp_SaveTicket", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TicketIssueID", TicketContract.TicketIssueID);
                    command.Parameters.AddWithValue("@TicketIssueSrvcMappingID", TicketContract.TicketIssueSrvcMappingID);
                    //command.Parameters.AddWithValue("@TicketIssueSrvcStepMappingID", TicketContract.TicketIssueSrvcStepMappingID);
                    command.Parameters.AddWithValue("@ClientID", TicketContract.ClientId);
                    command.Parameters.AddWithValue("@ClientSrvcMappingID", TicketContract.ClientSrvcMappingID);
                    command.Parameters.AddWithValue("@TicketSummary", TicketContract.TicketSummaryEncypted);
                    command.Parameters.AddWithValue("@TicketDetails", TicketContract.TicketDetailEncypted);
                    command.Parameters.AddWithValue("@SeverityID", TicketContract.SeverityId);
                    command.Parameters.AddWithValue("@TicketStatusID", TicketContract.TicketStatusId);
                    command.Parameters.AddWithValue("@TicketIssueTypeID", TicketContract.TicketIssueTypeID);
                    command.Parameters.AddWithValue("@LocationIdsToMap", TicketContract.LocationIds);
                    command.Parameters.AddWithValue("@UserID", LoggedInUserID);
                    if (TicketContract.WorkItemID.IsNotNull())
                        command.Parameters.AddWithValue("@WorkItemID", TicketContract.WorkItemID);
                    else
                        command.Parameters.AddWithValue("@WorkItemID", AppConsts.NONE);
                    command.Parameters.AddWithValue("@AssignToUserID", TicketContract.AssignToUserID);

                    command.Parameters.AddWithValue("@IsTicketNoteSaved", TicketContract.IsTicketNotesSaved);

                    SqlParameter param_UDDTTicketIssueSrvcStepMapping = new SqlParameter();
                    param_UDDTTicketIssueSrvcStepMapping.ParameterName = "@UDDT_TicketIssueSrvcStepMapping";
                    param_UDDTTicketIssueSrvcStepMapping.Value = ListClientSrvcStepMappingID;
                    param_UDDTTicketIssueSrvcStepMapping.SqlDbType = SqlDbType.Structured;
                    command.Parameters.Add(param_UDDTTicketIssueSrvcStepMapping);

                    SqlParameter param_UDTTSendEmailToUsers = new SqlParameter();
                    param_UDTTSendEmailToUsers.ParameterName = "@UDTT_SendEmailToUsers";
                    param_UDTTSendEmailToUsers.Value = SendToList;
                    param_UDTTSendEmailToUsers.SqlDbType = SqlDbType.Structured;
                    command.Parameters.Add(param_UDTTSendEmailToUsers);

                    SqlParameter param_UDTTDocuments = new SqlParameter();
                    param_UDTTDocuments.ParameterName = "@UDTT_Documents";
                    param_UDTTDocuments.Value = ListTicketDocuments;
                    param_UDTTDocuments.SqlDbType = SqlDbType.Structured;
                    command.Parameters.Add(param_UDTTDocuments);

                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        return Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    }
                }
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
            return 0;
        }
        //Comments By [SS]
        //List<Tenant> ITicketsCentreRepository.GetListOfClients()
        //{
        //    return _dbNavigation.Tenants.Where(x => x.IsActive && !x.IsDeleted && x.lkpTenantType.TenantTypeCode.Equals("TTYCLI")).OrderBy(x => x.TenantName).ToList();

        //}
        //Comment By [SS]
        //List<ClientUsers> ITicketsCentreRepository.GetClientUsers(Int32 UserId, Int32 LoggedInClientID, Int32? SelectedClientID)
        //{
        //    var users = from orguser in _dbNavigation.OrganizationUsers
        //                join org in _dbNavigation.Organizations on orguser.OrganizationID equals org.OrganizationID
        //                join tnt in _dbNavigation.Tenants on org.TenantID equals tnt.TenantID

        //                where org.IsActive && org.IsDeleted == false && orguser.IsActive && orguser.IsDeleted == false && ((LoggedInClientID == 1 && (tnt.TenantID == LoggedInClientID || tnt.TenantID == SelectedClientID.Value)) || (LoggedInClientID != 1 && (tnt.TenantID == LoggedInClientID || tnt.TenantID == 1)))
        //                select new ClientUsers()
        //                {
        //                    UserId = orguser.OrganizationUserID,
        //                    UserName = orguser.FirstName + " " + orguser.LastName
        //                };
        //    return users.OrderBy(x => x.UserName).ToList();
        //}
        List<ClientSrvcStepMapping> ITicketsCentreRepository.GetServiceStepsList(Int32 SelectedClientServiceMappingId, Int32 SelectedTenantId)
        {

            Int64 serviceID = _dbNavigation.ClientSrvcMappings.Where(cnd => cnd.ClientID == SelectedTenantId && cnd.ClientSrvcMappingID == SelectedClientServiceMappingId && !cnd.IsDeleted && !cnd.CUService.IsDeleted).Select(cnd1 => cnd1.ServiceID).FirstOrDefault();
            if (serviceID > 0)
            {
                return _dbNavigation.ClientSrvcStepMappings
                    .Where(cnd => cnd.ServiceStepMapping.SrvcStepMappingID == cnd.SrvcStepMappingID
                            && cnd.ClientID == SelectedTenantId
                            && cnd.ServiceStepMapping.ServiceID == serviceID
                            && !cnd.ServiceStepMapping.CUService.IsDeleted
                            && !cnd.IsDeleted
                            && !cnd.ServiceStepMapping.IsDeleted
                        ).ToList();
            }
            return new List<ClientSrvcStepMapping>();
        }
        //List<ClientSrvcStepMapping> ITicketsCentreRepository.GetWorkItemStepsList(Int32 SelectedClientServiceMappingId, Int32 SelectedTenantId, Int32 WorkItemId)
        //{
        //    return _dbNavigation.ClientSrvcMappings.Where(a => a.IsDeleted == false && a.ClientID == SelectedTenantId)
        //        .Join
        //         (
        //             _dbNavigation.ServiceStepMappings.Where(a => a.IsDeleted == false),
        //             csm => csm.ServiceID,
        //             ssm => ssm.ServiceID,
        //             (csm, ssm) => new { csm, ssm }
        //         )
        //         .Join(
        //       _dbNavigation.ClientSrvcStepMappings.Where(a => a.IsDeleted == false),
        //           Combined => Combined.ssm.SrvcStepMappingID,
        //           cssm => cssm.SrvcStepMappingID,
        //           (Combined, cssm) => new { Combined, cssm }
        //       )
        //       .Join(
        //           _dbNavigation.WorkItemDetails.Where(a => a.IsDeleted == false),
        //           Combined2 => Combined2.cssm.ClientSrvcStepMappingID,
        //           wid => wid.ClientSrvcStepMappingID,
        //            (Combined2, wid) => new { Combined2, wid }
        //       ).Where(a => a.wid.WorkItemID == WorkItemId).Select(sel => sel.Combined2.cssm).OrderBy(or => or.SequenceNumber)
        //       .ToList();
        //}
        bool ITicketsCentreRepository.DeleteTicket(Int64 TicketIssueId, Int32 UserId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            try
            {
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("ams.usp_DeleteTicket", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TicketIssueId", TicketIssueId);
                    command.Parameters.AddWithValue("@CurrentLoggedInUserID", UserId);
                    
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() == "1")
                            return true;
                        else return false;
                    }
                }
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
            return false;
        }
        DataSet ITicketsCentreRepository.GetTicketDetailById(long TicketIssueId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_GetTicketDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TicketIssueId", TicketIssueId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return ds;
            }
        }
        #endregion
        #region TicketNotes
        List<TicketsContract> ITicketsCentreRepository.GetTicketNotesList(Int64 TicketIssueID)
        {

            Entity.ClientEntity.AppConfiguration appConfiguration = _dbNavigation.AppConfigurations.Where(cond => cond.AC_Key == AppConsts.BACKGROUND_PROCESS_USER_ID).FirstOrDefault();
            Int32 bkgProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

            List<Int32> noteHistoryUsers = new List<Int32>();
            List<Entity.OrganizationUser> lstALlOrganizationUsers = new List<Entity.OrganizationUser>();
            List<TicketsContract> notesHistory = new List<TicketsContract>();

            List<TicketIssuesNote> notesHistoryDB = _dbNavigation.TicketIssuesNotes.Where(x => !x.IsDeleted && x.TicketIssueID == TicketIssueID).ToList();

            if (!notesHistoryDB.IsNullOrEmpty())
            {
                noteHistoryUsers = notesHistoryDB.Select(slct => slct.CreatedBy).ToList();
                lstALlOrganizationUsers = SecurityContext.OrganizationUsers.Where(cnd => noteHistoryUsers.Contains(cnd.OrganizationUserID)).ToList();

                notesHistory = notesHistoryDB.Where(x => x.TicketIssueID == TicketIssueID)
                     .Join(lstALlOrganizationUsers.Where(y => y.IsDeleted.Equals(false)), x => x.CreatedBy, y => y.OrganizationUserID, (x, y) => new { x, y })
                     .OrderByDescending(xx => xx.x.CreatedOn)
                     .Select(cond => new TicketsContract
                     {
                         UserName = cond.y.FirstName + " " + cond.y.LastName,
                         TicketNotes = cond.x.Notes,
                         CreatedOn = cond.x.CreatedOn,
                     }).ToList();
            }
            List<TicketsContract> systemNotes = _dbNavigation.TicketIssuesNotes.Where(x => !x.IsDeleted && x.TicketIssueID == TicketIssueID && x.CreatedBy == bkgProcessUserId)
                             .Select(cond => new TicketsContract
                             {
                                 UserName = "System",
                                 TicketNotes = cond.Notes,
                                 CreatedOn = cond.CreatedOn,
                             }).ToList();

            if (!notesHistory.IsNullOrEmpty())
            {
                notesHistory.AddRange(systemNotes);
            }
            else
            {
                notesHistory = systemNotes;
            }

            return notesHistory.ToList();
        }

        Boolean ITicketsCentreRepository.SaveTicketNotes(TicketIssuesNote ticketIssuesNote)
        {
            _dbNavigation.TicketIssuesNotes.AddObject(ticketIssuesNote);
            TicketIssue ticketIssue = _dbNavigation.TicketIssues.FirstOrDefault(xx => xx.TicketIssueID == ticketIssuesNote.TicketIssueID && xx.IsDeleted == false);
            if (ticketIssue != null)
            {
                ticketIssue.ModifiedOn = DateTime.Now;
                ticketIssue.ModifiedBy = ticketIssuesNote.CreatedBy;
            }
            if (_dbNavigation.SaveChanges() > 0)
                return true;
            return false;
        }

        #endregion

        #region Ticket Documents
        Boolean ITicketsCentreRepository.DeleteDocument(Int64 ticketID, Int32 documentID, Int16 entityTypeID, Int32 loggedInClientID)
        {
            DocumentEntityMapping docEntityMapping = new DocumentEntityMapping();
            docEntityMapping = _dbNavigation.DocumentEntityMappings.Where(x => x.EntityTypeID == entityTypeID && x.EntityID == ticketID && x.DocumentID == documentID && x.IsDeleted == false).FirstOrDefault();
            TicketDocument cuDocument = new TicketDocument();
            cuDocument = _dbNavigation.TicketDocuments.Where(x => x.DocumentID == documentID && x.IsDeleted == false).FirstOrDefault();
            if (docEntityMapping.IsNotNull() && cuDocument.IsNotNull())
            {
                docEntityMapping.IsDeleted = true;
                docEntityMapping.ModifiedOn = DateTime.Now;
                docEntityMapping.ModifiedBy = loggedInClientID;

                cuDocument.IsDeleted = true;
                cuDocument.ModifiedBy = loggedInClientID;
                cuDocument.ModifiedOn = DateTime.Now;

                _dbNavigation.SaveChanges();
                return true;
            }
            return false;
        }

        TicketDocument ITicketsCentreRepository.GetTicketDocument(Int32 documentID, Int32 documentTypeID)
        {
            return _dbNavigation.TicketDocuments.FirstOrDefault(cnd => cnd.DocumentID == documentID && !cnd.IsDeleted && cnd.DocumentTypeID == documentTypeID);
        }
        #endregion

        //Comment By [SS]
        //Boolean ITicketsCentreRepository.UpdateWorkItemStatus(int WorkItemID, Int32 statusID, int loggedInUserID)
        //{
        //    WorkItemTransition WITransitionObj = _dbNavigation.WorkItemTransitions.Where(x => x.WorkItemID == WorkItemID && x.IsDeleted == false).FirstOrDefault();
        //    if (WITransitionObj != null)
        //    {
        //        WITransitionObj.WorkItemStatusID = statusID;
        //        WITransitionObj.ModifiedOn = DateTime.Now;
        //        WITransitionObj.ModifiedBy = loggedInUserID;

        //        _dbNavigation.SaveChanges();

        //        return true;
        //    }
        //    return false;
        //}

        #region  Bug 19211
        //Comment By [SS]
        //List<TicketIssueSrvcMapping> ITicketsCentreRepository.IsTicketCreatedForServices(List<Int64> lstClientSrvcMappingIDs)
        //{
        //    return _dbNavigation.TicketIssueSrvcMappings.Where(cnd => lstClientSrvcMappingIDs.Contains(cnd.ClientSrvcMappingID) && cnd.IsDeleted == false).ToList();
        //}
        #endregion

        #region Automatic Ticket Creation
        //Comments BY [SS]
        //Boolean ITicketsCentreRepository.CreateAutomaticTicket(TicketsContract TicketContract, DataTable ListTicketDocuments, Int32 LoggedInUserID)
        //{
        //    try
        //    {
        //        EntityConnection connection = _dbNavigation.Connection as EntityConnection;
        //        using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //        {

        //            SqlCommand command = new SqlCommand("usp_SaveAutomaticTicket", con);
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@TicketSummary", TicketContract.TicketSummary);
        //            command.Parameters.AddWithValue("@TicketDetails", TicketContract.TicketDetail);
        //            command.Parameters.AddWithValue("@SeverityID", TicketContract.SeverityId);
        //            command.Parameters.AddWithValue("@TicketStatusID", TicketContract.TicketStatusId);
        //            command.Parameters.AddWithValue("@TicketIssueTypeID", TicketContract.TicketIssueTypeID);
        //            command.Parameters.AddWithValue("@CurrentLoggedInUserID", LoggedInUserID);
        //            command.Parameters.AddWithValue("@EmailConversationID", TicketContract.EmailConversationId);
        //            command.Parameters.AddWithValue("@RepliedEmailContent", TicketContract.RepliedEmailContent);
        //            command.Parameters.AddWithValue("@TicketEmailLOBMappingID", TicketContract.TicketEmailLOBMappingID);

        //            SqlParameter param_UDTTDocuments = new SqlParameter();
        //            param_UDTTDocuments.ParameterName = "@UDTT_Documents";
        //            param_UDTTDocuments.Value = ListTicketDocuments;
        //            param_UDTTDocuments.SqlDbType = SqlDbType.Structured;
        //            command.Parameters.Add(param_UDTTDocuments);

        //            SqlDataAdapter adp = new SqlDataAdapter();
        //            adp.SelectCommand = command;
        //            DataSet ds = new DataSet();
        //            adp.Fill(ds);
        //            if (ds.Tables.Count > 0)
        //            {
        //                return Convert.ToBoolean(ds.Tables[0].Rows[0][0]);
        //            }
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //    return false;
        //}

        #endregion

        #region Ticket Center Settings 
        //Comment By [SS]
        //List<TicketEmailLOBMapping> ITicketsCentreRepository.GetTicketCenterSettings(Int32 LOBID)
        //{
        //    return LOBID == AppConsts.NONE ? _dbNavigation.TicketEmailLOBMappings.Where(x => !x.IsDeleted).ToList()
        //        : _dbNavigation.TicketEmailLOBMappings.Where(x => !x.IsDeleted && x.BusniessChannelTypeID == LOBID).ToList();
        //}
        //Boolean ITicketsCentreRepository.SaveTicketEmailLOBMapping(TicketEmailLOBMapping ticketEmailLOBMapping)
        //{
        //    _dbNavigation.TicketEmailLOBMappings.AddObject(ticketEmailLOBMapping);
        //    return _dbNavigation.SaveChanges() > 0 ? true : false;
        //}
        //Boolean ITicketsCentreRepository.UpdateTicketEmailLOBMapping(TicketEmailLOBMapping ticketEmailLOBMapping, Int32 TicketEmailLOBMappingID)
        //{
        //    TicketEmailLOBMapping updateTicketEmailLOBMapping = _dbNavigation.TicketEmailLOBMappings.FirstOrDefault(x => !x.IsDeleted && x.TicketEmailLOBMappingID == TicketEmailLOBMappingID);
        //    updateTicketEmailLOBMapping.TicketEmailSetting.EmailAddress = ticketEmailLOBMapping.TicketEmailSetting.EmailAddress;
        //    updateTicketEmailLOBMapping.TicketEmailSetting.AccountName = ticketEmailLOBMapping.TicketEmailSetting.AccountName;
        //    updateTicketEmailLOBMapping.TicketEmailSetting.Password = ticketEmailLOBMapping.TicketEmailSetting.Password;
        //    updateTicketEmailLOBMapping.TicketEmailSetting.ModifiedBy = updateTicketEmailLOBMapping.ModifiedBy = ticketEmailLOBMapping.TicketEmailSetting.ModifiedBy;
        //    updateTicketEmailLOBMapping.TicketEmailSetting.ModifiedOn = updateTicketEmailLOBMapping.ModifiedOn = DateTime.Now;
        //    return _dbNavigation.SaveChanges() > 0 ? true : false;
        //}

        //Boolean ITicketsCentreRepository.DeleteTicketSetting(Int32 TicketEmailLOBMappingID, Int32 CurrentUserId)
        //{
        //    TicketEmailLOBMapping ticketEmailLOBMapping = _dbNavigation.TicketEmailLOBMappings.FirstOrDefault(x => !x.IsDeleted && x.TicketEmailLOBMappingID == TicketEmailLOBMappingID);
        //    ticketEmailLOBMapping.IsDeleted = true;
        //    ticketEmailLOBMapping.ModifiedBy = CurrentUserId;
        //    ticketEmailLOBMapping.ModifiedOn = DateTime.Now;
        //    ticketEmailLOBMapping.TicketEmailSetting.IsDeleted = true;
        //    ticketEmailLOBMapping.TicketEmailSetting.ModifiedOn = DateTime.Now;
        //    ticketEmailLOBMapping.TicketEmailSetting.ModifiedBy = CurrentUserId;
        //    return _dbNavigation.SaveChanges() > 0 ? true : false;
        //}
        //Boolean ITicketsCentreRepository.CheckEmailAlreadyExists(String email, Boolean isUpdate, Int32 TicketEmailLOBMappingID)
        //{
        //    String savedEmail = String.Empty;
        //    if (isUpdate)
        //        savedEmail = _dbNavigation.TicketEmailLOBMappings.FirstOrDefault(x => !x.IsDeleted && x.TicketEmailLOBMappingID == TicketEmailLOBMappingID && !x.TicketEmailSetting.IsDeleted).TicketEmailSetting.EmailAddress;

        //    return savedEmail == email && isUpdate ? true : !_dbNavigation.TicketEmailLOBMappings.Any(x => !x.IsDeleted && x.TicketEmailSetting.EmailAddress == email && !x.TicketEmailSetting.IsDeleted);
        //}
        #endregion

        #region Encrypt TimeSheet Data
        List<TicketIssue> ITicketsCentreRepository.GetAllTickets()
        {
            return _dbNavigation.TicketIssues.Where(x => !x.IsDeleted).ToList();
        }
        List<TicketIssuesNote> ITicketsCentreRepository.GetAllTicketNotes()
        {
            return _dbNavigation.TicketIssuesNotes.Where(x => !x.IsDeleted).ToList();
        }
        void ITicketsCentreRepository.SaveTickets(List<TicketIssue> lstTicket, List<TicketIssuesNote> lstNotes)
        {
            foreach (TicketIssue ticket in lstTicket)
            {
                TicketIssue tic = _dbNavigation.TicketIssues.Where(x => x.TicketIssueID == ticket.TicketIssueID).FirstOrDefault();
                if (tic.IsNotNull())
                {
                    tic.TicketSummary = ticket.TicketSummary;
                    tic.TicketDetail = ticket.TicketDetail;
                }
            }
            foreach (TicketIssuesNote ticket in lstNotes)
            {
                TicketIssuesNote tic = _dbNavigation.TicketIssuesNotes.Where(x => x.TicketIssuesNoteID == ticket.TicketIssuesNoteID).FirstOrDefault();
                if (tic.IsNotNull())
                {
                    tic.Notes = ticket.Notes;
                }
            }
            _dbNavigation.SaveChanges();
        }
        #endregion

        #region Services And Service Steps

        List<ClientSrvcMapping> ITicketsCentreRepository.GetAllClientServiceMapping(Int32 tenantID, Int32? LOBId)
        {
            List<ClientSrvcMapping> lstClientSrvcMapping = _dbNavigation.ClientSrvcMappings.Where(a => a.ClientID == tenantID && a.IsDeleted == false).ToList();
            if (LOBId != null)
            {
                //Commented by [SS]
                //lstClientSrvcMapping = lstClientSrvcMapping.Where(x => x.CUService.ServiceBusinessChannelMappings.Any(a => a.BussinessChannelTypeID == LOBId.Value && a.IsDeleted == false)).ToList();
                
            }
            return lstClientSrvcMapping;
        }

        #endregion

    }
}
