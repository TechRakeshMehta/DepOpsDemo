using Entity.ClientEntity;
//using NSTOF.EncryptionDecryption.Utils;
using INTSOF.UI.Contract.TicketsCentre;
using INTSOF.UI.Contract.Templates;
using INTSOF.UI.Contract.TicketsCentre;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Business.RepoManagers
{
    public static class TicketsCentreManager
    {

        #region Methods
        #region public Methods
        /// <summary>
        /// Get Ticket List 
        /// </summary>
        /// <param name="LOB"> line of business</param>
        /// <param name="pageingContract">pageingContract</param>
        /// <returns>List of Tickets</returns>
        /// 
        public static Int32 DefaultClientID
        {
            get
            {
                return 1;
            }
        }

        public static List<TicketsContract> GetTickets(Int32 tenantID, int LOBID, CustomPagingArgsContract obj, Int32 ViewAllTicketsPermission, Int32 ClientID
                                                      , Int32 UserID, Int64 workItemID, Boolean IsEncryptionApplied, TicketSearchContract ticketSearchContract)
        {
            try
            {
                //Comments BY [SS]
                //if (IsEncryptionApplied)
                //{
                //    obj.FilterValues[obj.FilterColumns.FindIndex(x => x.StartsWith("TicketSummary"))] = AESEncryptDecrypt.Encrypt(Convert.ToString(obj.FilterValues[obj.FilterColumns.FindIndex(x => x.StartsWith("TicketSummary"))]));
                //}
                //return ConvertDTToList(BALUtils.GetTicketsCentreRepoInstance(tenantID).GetTickets(LOBID, obj, ViewAllTicketsPermission, ClientID, UserID, workItemID, TimeZoneOffset));
                return ConvertDTToList(BALUtils.GetTicketsCentreRepoInstance(tenantID).GetTickets(LOBID, obj, ViewAllTicketsPermission, ClientID, UserID
                                                                                                  , workItemID, 0, ticketSearchContract));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Int32 AddTicket(Int32 tenantID, TicketsContract TicketContract, Int32 CurrentLoggedInUserID, string screenName)
        {
            try
            {
                if (TicketContract.IsNotNull())
                {
                    //TicketContract.TicketSummaryEncypted = AESEncryptDecrypt.Encrypt(TicketContract.TicketSummaryEncypted);
                    //TicketContract.TicketDetailEncypted = AESEncryptDecrypt.Encrypt(TicketContract.TicketDetailEncypted);
                    //List<lkpTicketIssueType> listTicketIssueTypes = LookupManager.GetLookUpData<lkpTicketIssueType>().ToList();
                    //Comments By [SS]
                    //if (screenName == ScreenName.ManageWorkQueue.GetStringValue())
                    //{
                    //    TicketContract.TicketIssueTypeID = listTicketIssueTypes.Any(x => x.Code == TicketIssueType.WorkItem.GetStringValue()) ?
                    //        listTicketIssueTypes.Where(x => x.Code == TicketIssueType.WorkItem.GetStringValue()).FirstOrDefault().TicketIssueTypeID : AppConsts.NONE;
                    //}
                    //else
                    //{
                    //TicketContract.TicketIssueTypeID = listTicketIssueTypes.Any(x => x.Code == TicketIssueType.APPLICANT_SUPPORT.GetStringValue()) ?
                    //    listTicketIssueTypes.Where(x => x.Code == TicketIssueType.APPLICANT_SUPPORT.GetStringValue()).FirstOrDefault().TicketIssueTypeID : AppConsts.NONE;
                    //}

                    //Comments By [SS]
                    DataTable tdTicketIssueSrvcStepMapping = ConvertToUDDTTicketIssueSrvcStepMapping(TicketContract.TicketIssueSrvcStepMapping);
                    DataTable dtSendEmailToUsers = ConvertToUDTTSendEmailToUsers(TicketContract.SendToList);
                    DataTable dtUDTTDocuments = ConvertToUDTTDocuments(TicketContract.AttachedDocuments);

                    return BALUtils.GetTicketsCentreRepoInstance(tenantID).AddTicket(
                                    TicketContract
                                    , tdTicketIssueSrvcStepMapping
                                    , dtSendEmailToUsers
                                    , dtUDTTDocuments
                                    , CurrentLoggedInUserID);
                }
                return 0;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        //Comments By [SS]
        //public static List<Tenant> GetClientList(Int32 clientID)
        //{
        //    try
        //    {
        //        List<Tenant> clientList = new List<Tenant>();
        //        clientList = ServiceStepManager.GetClientList();
        //        if (clientList.IsNotNull() && clientList.Count > 0)
        //        {
        //            if (clientID != DefaultClientID)
        //            {
        //                clientList = clientList.Where(x => x.TenantID == clientID).ToList();
        //            }
        //            else
        //            {
        //                clientList.RemoveAll(x => x.TenantID == DefaultClientID);//.se.ToList();
        //            }
        //            return clientList;
        //        }

        //        return new List<Tenant>();
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        public static List<ClientSrvcMappingContract> GetServiceList(Int32 tenantID, Int32? LOBId)
        {
            try
            {
                List<ClientSrvcMappingContract> serviceList = new List<ClientSrvcMappingContract>();
                serviceList = GetAllClientServicesMapping(tenantID, null);
                if (serviceList.IsNotNull() && serviceList.Count > 0)
                    return serviceList.OrderBy(x => x.ServcieName).ToList();
                return new List<ClientSrvcMappingContract>();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<TicketIssueSrvcStepMappingContract> GetServiceStepsList(Int32 ClientServiceMappingID, Int32 tenantID, Int32 WorkItemID, string screenName)
        {
            try
            {
                List<ClientSrvcStepMapping> serviceStepList = new List<ClientSrvcStepMapping>();
                List<TicketIssueSrvcStepMappingContract> ticketServiceStepList = new List<TicketIssueSrvcStepMappingContract>();
                //Commented By [SS]
                //if (screenName == ScreenName.ManageWorkQueue.ToString())
                //{
                //    serviceStepList = BALUtils.GetTicketsCentreRepoInstance().GetWorkItemStepsList(ClientServiceMappingID, ClientID, WorkItemID);
                //}
                //else
                //{
                serviceStepList = BALUtils.GetTicketsCentreRepoInstance(tenantID).GetServiceStepsList(ClientServiceMappingID, tenantID);
                //}

                if (serviceStepList.IsNotNull() && serviceStepList.Count > 0)
                {
                    ticketServiceStepList = serviceStepList.Select(x => new TicketIssueSrvcStepMappingContract()
                    {
                        ClientSrvcStepMappingID = x.ClientSrvcStepMappingID,
                        ServcieStepName = x.ServiceStepMapping.ServiceStep.Name
                    }).OrderBy(x => x.ServcieStepName).ToList();
                }
                return ticketServiceStepList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<lkpTicketSeverity> GetTicketSeverityList(Int32 tenantID)
        {
            try
            {
                List<lkpTicketSeverity> severityList = new List<lkpTicketSeverity>();
                severityList = LookupManager.GetLookUpData<lkpTicketSeverity>(tenantID).OrderBy(x => x.Name).ToList();
                if (severityList.IsNotNull() && severityList.Count > 0)
                    return severityList;
                return new List<lkpTicketSeverity>();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<lkpTicketStatu> GetTicketStatusList(Int32 tenantID)
        {
            try
            {
                List<lkpTicketStatu> statusList = new List<lkpTicketStatu>();
                statusList = LookupManager.GetLookUpData<lkpTicketStatu>(tenantID).OrderBy(x => x.Name).ToList();
                if (statusList.IsNotNull() && statusList.Count > 0)
                    return statusList;
                return new List<lkpTicketStatu>();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<ClientUsers> GetClientUsers(Int32 UserId, Int32 LoggedInClientID, Int32? SelectedClientID, Int32 LOBId)
        {
            try
            {
                //List<ClientUsers> lstUsers = new List<ClientUsers>();
                //lstUsers = BALUtils.GetTicketsCentreRepoInstance().GetClientUsers(UserId, LoggedInClientID, SelectedClientID);
                //if (lstUsers.IsNotNull() && lstUsers.Count > 0)
                //    return lstUsers;
                //return new List<ClientUsers>();

                //Commented BY [SS]
                //List<OrganizationUser> listOrganizationUser = BALUtils.GetOMSServiceRepoInstance().GetContactTypeListBasedOnLOB(LOBId);
                //List<OrganizationUser> data = listOrganizationUser.Where(cond => cond.Organization.TenantID == DefaultClientID || cond.Organization.TenantID == SelectedClientID).ToList();
                List<Entity.OrganizationUser> listOrganizationUser = SecurityManager.GetOganisationUsersByTanentId(SelectedClientID.Value);
                List<Entity.OrganizationUser> data = listOrganizationUser.Where(cond => cond.Organization.TenantID == DefaultClientID || cond.Organization.TenantID == SelectedClientID).ToList();
                List<ClientUsers> lstUsers = new List<ClientUsers>();
                foreach (var item in data)
                {
                    ClientUsers clientUsers = new ClientUsers();
                    clientUsers.UserId = item.OrganizationUserID;
                    clientUsers.UserName = item.FirstName + " " + item.LastName;
                    lstUsers.Add(clientUsers);
                }
                return lstUsers;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<lkpTicketIssueType> GetTicketIssueTypeList(Int32 tenantID)
        {
            try
            {
                List<lkpTicketIssueType> ticketTypeList = new List<lkpTicketIssueType>();
                ticketTypeList = LookupManager.GetLookUpData<lkpTicketIssueType>(tenantID).OrderBy(x => x.Name).ToList();
                if (ticketTypeList.IsNotNull() && ticketTypeList.Count > 0)
                    return ticketTypeList;
                return new List<lkpTicketIssueType>();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 GetDocumentType(Int32 tenantID)
        {
            List<lkpDocumentType> listDocumentType = LookupManager.GetLookUpData<lkpDocumentType>(tenantID).ToList();
            return listDocumentType.Where(x => x.DMT_Code == DocumentType.TICKET_ISSUE_DOCUMENT.GetStringValue()).FirstOrDefault().DMT_ID;
        }
        public static Int32 GetEntityType(Int32 tenantID)
        {
            List<lkpClientEntityType> listEntityType = LookupManager.GetLookUpData<lkpClientEntityType>(tenantID).ToList();
            return listEntityType.Where(x => x.Code == ClientEntityType.TicketIssue.GetStringValue()).FirstOrDefault().ClientEntityTypeID;
        }
        public static bool DeleteTicket(Int32 tenantID, Int64 TicketIssueId, Int32 UserId)
        {
            try
            {
                //List<Int64> lstTicketDocumentIds = ConvertDSToContract(BALUtils.GetTicketsCentreRepoInstance().GetTicketDetailById(TicketIssueId)).AttachedDocuments.Select(x=>x.DocumentID).ToList();
                if (BALUtils.GetTicketsCentreRepoInstance(tenantID).DeleteTicket(TicketIssueId, UserId))
                {
                    //foreach (Int64 docId in lstTicketDocumentIds)
                    //{
                    //    DeleteDocumentFromServer(docId);
                    //}
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static TicketsContract GetTicketDetailById(Int32 tenantID, Int64 TicketIssueId)
        {
            try
            {
                return ConvertDSToContract(BALUtils.GetTicketsCentreRepoInstance(tenantID).GetTicketDetailById(TicketIssueId));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        #region Private  Methods

        //private static Boolean DeleteDocumentFromServer(Int64 DocumentID)
        //{
        //    String FilePath = DocumentManager.GetDocumentDetail(DocumentID).FilePath;
        //    return CommonFileManager.DeleteDocument(FilePath, "");
        //}
        private static List<TicketsContract> ConvertDTToList(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            List<TicketsContract> lstTicketContract = new List<TicketsContract>();
            foreach (var x in rows)
            {
                TicketsContract ticketsContract = new TicketsContract();
                ticketsContract.TicketIssueID = Convert.ToInt64(x["TicketIssueID"]);
                ticketsContract.ClientName = Convert.ToString(x["ClientName"]);
                ticketsContract.Service = Convert.ToString(x["Service"]);
                ticketsContract.TicketSummary = Convert.ToString(x["TicketSummary"]);
                ticketsContract.TicketDetail = Convert.ToString(x["TicketDetail"]);

                //Comments By [SS]
                //try
                //{
                //    //ticketsContract.TicketSummary = AESEncryptDecrypt.Decrypt(Convert.ToString(x["TicketSummary"]));
                //    //ticketsContract.TicketDetail = AESEncryptDecrypt.Decrypt(Convert.ToString(x["TicketDetail"]));


                //}
                //catch (Exception ex)
                //{
                //    ticketsContract.TicketSummary = "Decrption error";
                //    ticketsContract.TicketDetail = "Decrption error";
                //    BALUtils.LogError("Error in Ticket issue no " + ticketsContract.TicketIssueID + " " + BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                //}

                ticketsContract.SeverityName = Convert.ToString(x["SeverityName"]);
                ticketsContract.CreatedOn = Convert.ToDateTime(x["CreatedOn"]);
                ticketsContract.AssignedTo = Convert.ToString(x["AssignedTo"]);
                ticketsContract.TicketStatus = Convert.ToString(x["TicketStatus"]);
                ticketsContract.ModifiedByUserName = Convert.ToString(x["ModifiedByUserName"]);
                ticketsContract.UpdatedOn = x["UpdatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["UpdatedOn"]);
                ticketsContract.CreatedOnDatePart = x["CreatedOnDatePart"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["CreatedOnDatePart"]);
                ticketsContract.ModifiedOnDatePart = x["ModifiedOnDatePart"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["ModifiedOnDatePart"]);
                ticketsContract.CreatedByUserName = Convert.ToString(x["CreatedByUserName"]);
                ticketsContract.TicketType = Convert.ToString(x["TicketType"]);
                ticketsContract.LocationName = Convert.ToString(x["LocationName"]);
                lstTicketContract.Add(ticketsContract);
            }
            return lstTicketContract;
            //return rows.Select(x => new TicketsContract
            //{
            //    TicketIssueID = Convert.ToInt64(x["TicketIssueID"]),
            //    ClientName = Convert.ToString(x["ClientName"]),
            //    Service = Convert.ToString(x["Service"]),
            //    TicketSummary = AESEncryptDecrypt.Decrypt(Convert.ToString(x["TicketSummary"])),
            //    TicketDetail = AESEncryptDecrypt.Decrypt(Convert.ToString(x["TicketDetail"])),
            //    SeverityName = Convert.ToString(x["SeverityName"]),
            //    CreatedOn = Convert.ToDateTime(x["CreatedOn"]),
            //    AssignedTo = Convert.ToString(x["AssignedTo"]),
            //    TicketStatus = Convert.ToString(x["TicketStatus"]),
            //    ModifiedByUserName = Convert.ToString(x["ModifiedByUserName"]),
            //    UpdatedOn = x["UpdatedOn"].GetType().Name == "DBNull" ? (DateTime?)null : Convert.ToDateTime(x["UpdatedOn"]),
            //    CreatedOnDatePart = x["CreatedOnDatePart"].GetType().Name == "DBNull" ? (DateTime?)null : Convert.ToDateTime(x["CreatedOnDatePart"]),
            //    ModifiedOnDatePart = x["ModifiedOnDatePart"].GetType().Name == "DBNull" ? (DateTime?)null : Convert.ToDateTime(x["ModifiedOnDatePart"]),
            //    CreatedByUserName = Convert.ToString(x["CreatedByUserName"]),

            //}).ToList();
        }

        private static DataTable ConvertToUDDTTicketIssueSrvcStepMapping(List<TicketIssueSrvcStepMappingContract> ListClientServiceStepMappings)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TicketIssueSrvcStepMappingID", typeof(int));
            dataTable.Columns.Add("ClientSrvcStepMappingID", typeof(int));
            if (ListClientServiceStepMappings.IsNotNull() && ListClientServiceStepMappings.Count > 0)
            {
                foreach (var item in ListClientServiceStepMappings)
                {
                    dataTable.Rows.Add(item.TicketIssueSrvcStepMappingID, item.ClientSrvcStepMappingID);
                }
            }
            return dataTable;
        }

        private static DataTable ConvertToUDTTSendEmailToUsers(List<ClientUsers> lstSendTo)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TicketIssueNotificationUserID", typeof(int));
            dataTable.Columns.Add("SendToUserId", typeof(int));
            if (lstSendTo.IsNotNull() && lstSendTo.Count > 0)
            {
                foreach (var item in lstSendTo)
                {
                    dataTable.Rows.Add(0, item.UserId);
                }
            }
            return dataTable;
        }

        private static DataTable ConvertToUDTTDocuments(List<DocumentsContract> lstDocuments)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("DocumentID", typeof(int));
            dataTable.Columns.Add("ClientID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("OriginalName", typeof(string));
            dataTable.Columns.Add("FilePath", typeof(string));
            dataTable.Columns.Add("DocumentTypeID", typeof(int));
            dataTable.Columns.Add("FileType", typeof(string));
            dataTable.Columns.Add("FileVersion", typeof(string));
            dataTable.Columns.Add("FileSize", typeof(int));
            dataTable.Columns.Add("DocumentFolderID", typeof(int));
            dataTable.Columns.Add("PublishedBy", typeof(int));
            dataTable.Columns.Add("PublishedOn", typeof(DateTime));
            dataTable.Columns.Add("CreatedBy", typeof(int));
            dataTable.Columns.Add("CreatedOn", typeof(DateTime));
            dataTable.Columns.Add("ModifiedBy", typeof(int));
            dataTable.Columns.Add("ModifiedOn", typeof(DateTime));
            dataTable.Columns.Add("EntityTypeID", typeof(int));
            dataTable.Columns.Add("EntityID", typeof(int));
            dataTable.Columns.Add("EntityAttributeMappingID", typeof(int));
            if (lstDocuments.IsNotNull() && lstDocuments.Count > 0)
            {
                foreach (var item in lstDocuments)
                {
                    dataTable.Rows.Add(item.DocumentID, item.ClientID, item.Name, item.OriginalName, item.FilePath, item.DocumentTypeID, item.FileType
                                        , item.FileVersion, item.FileSize, item.DocumentFolderID, item.PublishedBy, item.PublishedOn
                                        , item.CreatedBy, item.CreatedOn, item.ModifiedBy, item.ModifiedOn, item.EntityTypeID, item.EntityID, item.EntityAttributeMappingID);
                }
            }
            return dataTable;
        }
        private static TicketsContract ConvertDSToContract(DataSet dsTicketInfo)
        {
            TicketsContract ticketsContract = new TicketsContract();
            if (dsTicketInfo.Tables[0].Rows.Count > 0)
            {
                DataTable tbBasicInfo = dsTicketInfo.Tables[0];
                //ticketsContract.TicketSummary = AESEncryptDecrypt.Decrypt(tbBasicInfo.Rows[0]["TicketSummary"].ToString());
                //ticketsContract.TicketDetail = AESEncryptDecrypt.Decrypt(tbBasicInfo.Rows[0]["TicketDetail"].ToString());
                ticketsContract.TicketSummary = Convert.ToString(tbBasicInfo.Rows[0]["TicketSummary"]);
                ticketsContract.TicketDetail = Convert.ToString(tbBasicInfo.Rows[0]["TicketDetail"]);
                ticketsContract.TicketIssueID = (tbBasicInfo.Rows[0]["TicketIssueID"].ToString() == "") ? 0 : Convert.ToInt32(tbBasicInfo.Rows[0]["TicketIssueID"]);
                ticketsContract.TicketIssueSrvcMappingID = (tbBasicInfo.Rows[0]["TicketIssueSrvcMappingID"].ToString() == "") ? 0 : Convert.ToInt32(tbBasicInfo.Rows[0]["TicketIssueSrvcMappingID"]);
                ticketsContract.ClientSrvcMappingID = (tbBasicInfo.Rows[0]["ClientSrvcMappingID"].ToString() == "") ? 0 : Convert.ToInt32(tbBasicInfo.Rows[0]["ClientSrvcMappingID"]);
                ticketsContract.ClientId = (tbBasicInfo.Rows[0]["ClientID"].ToString() == "") ? 0 : Convert.ToInt32(tbBasicInfo.Rows[0]["ClientID"]);
                ticketsContract.SeverityId = (tbBasicInfo.Rows[0]["TicketSeverityID"].ToString() == "") ? 0 : Convert.ToInt32(tbBasicInfo.Rows[0]["TicketSeverityID"]);
                ticketsContract.AssignToUserID = (tbBasicInfo.Rows[0]["AssignedToID"].ToString() == "") ? 0 : Convert.ToInt32(tbBasicInfo.Rows[0]["AssignedToID"]);
                ticketsContract.TicketStatusId = (tbBasicInfo.Rows[0]["TicketStatusID"].ToString() == "") ? 0 : Convert.ToInt32(tbBasicInfo.Rows[0]["TicketStatusID"]);
                ticketsContract.TicketIssueTypeID = (tbBasicInfo.Rows[0]["TicketTypeId"].ToString() == "") ? 0 : Convert.ToInt32(tbBasicInfo.Rows[0]["TicketTypeId"]);
                ticketsContract.TicketIssueTypeCode = tbBasicInfo.Rows[0]["TicketTypeCode"].ToString();
                ticketsContract.CreatedByUserName = tbBasicInfo.Rows[0]["CreatedByUserName"].ToString();

                ticketsContract.LocationIds = tbBasicInfo.Rows[0]["MappedLocationIds"].ToString();
                ticketsContract.CreatedOn = Convert.ToDateTime(tbBasicInfo.Rows[0]["CreatedDate"]);

                if (dsTicketInfo.Tables[1].Rows.Count > 0)
                {
                    //Comments By [SS]
                    List<TicketIssueSrvcStepMappingContract> lstClientSrvcStepMappingContract = new List<TicketIssueSrvcStepMappingContract>();
                    DataTable tbstepInfo = dsTicketInfo.Tables[1];
                    for (int i = 0; i < tbstepInfo.Rows.Count; i++)
                    {
                        TicketIssueSrvcStepMappingContract clientSrvcStepMappingContract = new TicketIssueSrvcStepMappingContract();
                        clientSrvcStepMappingContract.ClientSrvcStepMappingID = (tbstepInfo.Rows[i]["ClientSrvcStepMappingID"].ToString() == "") ? 0
                                                                                : Convert.ToInt32(tbstepInfo.Rows[i]["ClientSrvcStepMappingID"]);
                        clientSrvcStepMappingContract.TicketIssueSrvcStepMappingID = (tbstepInfo.Rows[i]["TicketIssueSrvcStepMappingID"].ToString() == "") ? 0
                                                                                : Convert.ToInt32(tbstepInfo.Rows[i]["TicketIssueSrvcStepMappingID"]);
                        clientSrvcStepMappingContract.TicketIssueID = ticketsContract.TicketIssueID;
                        lstClientSrvcStepMappingContract.Add(clientSrvcStepMappingContract);
                    }
                    ticketsContract.TicketIssueSrvcStepMapping = lstClientSrvcStepMappingContract;
                }
                if (dsTicketInfo.Tables[2].Rows.Count > 0)
                {
                    DataTable tbDocInfo = dsTicketInfo.Tables[2];
                    List<DocumentsContract> lstAttachedDocuments = new List<DocumentsContract>();
                    for (int i = 0; i < tbDocInfo.Rows.Count; i++)
                    {
                        DocumentsContract AttachedDocuments = new DocumentsContract();
                        AttachedDocuments.DocumentID = (tbDocInfo.Rows[i]["DocumentID"].ToString() == "") ? 0 : Convert.ToInt32(tbDocInfo.Rows[i]["DocumentID"]);
                        AttachedDocuments.ClientID = (tbDocInfo.Rows[i]["ClientID"].ToString() == "") ? 0 : Convert.ToInt32(tbDocInfo.Rows[i]["ClientID"]);
                        AttachedDocuments.Name = tbDocInfo.Rows[i]["DocName"].ToString();
                        AttachedDocuments.OriginalName = tbDocInfo.Rows[i]["OriginalName"].ToString();
                        AttachedDocuments.FilePath = tbDocInfo.Rows[i]["FilePath"].ToString();
                        lstAttachedDocuments.Add(AttachedDocuments);
                        ticketsContract.AttachedDocuments = lstAttachedDocuments;
                    }
                }
                if (dsTicketInfo.Tables[3].Rows.Count > 0)
                {
                    //Comments By [SS]
                    DataTable dsSendToInfo = dsTicketInfo.Tables[3];
                    List<ClientUsers> SendToList = new List<ClientUsers>();

                    for (int i = 0; i < dsSendToInfo.Rows.Count; i++)
                    {
                        ClientUsers ClientUsers = new ClientUsers();
                        ClientUsers.UserId = (dsSendToInfo.Rows[i]["UserID"].ToString() == "") ? 0 : Convert.ToInt32(dsSendToInfo.Rows[i]["UserId"]);
                        ClientUsers.UserName = dsSendToInfo.Rows[i]["UserName"].ToString();
                        ClientUsers.TicketIssueNotificationUserID = (dsSendToInfo.Rows[i]["TicketIssueNotificationUserID"].ToString() == "") ? 0 : Convert.ToInt32(dsSendToInfo.Rows[i]["TicketIssueNotificationUserID"]);
                        SendToList.Add(ClientUsers);
                        ticketsContract.SendToList = SendToList;

                    }
                }
            }
            return ticketsContract;
        }
        #endregion
        #endregion
        #region TicketNotes
        public static List<TicketsContract> GetTicketNotesList(Int32 tenantID, Int64 TicketIssueID)
        {
            try
            {
                List<TicketsContract> result = BALUtils.GetTicketsCentreRepoInstance(tenantID).GetTicketNotesList(TicketIssueID);
                //Comments BY [SS]
                //foreach (TicketsContract ticket in result)
                //{
                //    //ticket.TicketNotes = AESEncryptDecrypt.Decrypt(ticket.TicketNotes);
                //    //ticket.CreatedOn = GetUserTime(ticket.CreatedOn);
                //    //ticket.UpdatedOn = ticket.UpdatedOn.HasValue ? GetUserTime(ticket.UpdatedOn.Value) : (DateTime?)null;
                //    //ticket.ModifiedOnDatePart = ticket.ModifiedOnDatePart.HasValue ? GetUserTime(ticket.ModifiedOnDatePart.Value) : (DateTime?)null;
                //}
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveTicketNotes(Int32 tenantID, TicketIssuesNote ticketIssuesNote)
        {
            try
            {
                //ticketIssuesNote.Notes = AESEncryptDecrypt.Encrypt(ticketIssuesNote.Notes);
                Boolean result = BALUtils.GetTicketsCentreRepoInstance(tenantID).SaveTicketNotes(ticketIssuesNote);
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Ticket Documents
        public static Boolean DeleteDocument(Int32 tenantID, Int64 ticketID, Int32 documentID, Int32 loggedInClientID)
        {
            try
            {
                if (BALUtils.GetTicketsCentreRepoInstance(tenantID).DeleteDocument(ticketID, documentID, Convert.ToInt16(GetEntityType(tenantID)), loggedInClientID))
                {
                    //return DeleteDocumentFromServer(Convert.ToInt64(documentID));
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static TicketDocument GetTicketDocument(Int32 tenantID, Int32 documentID, String documentType)
        {
            try
            {

                Int32 documentTypeID = LookupManager.GetLookUpData<lkpDocumentType>(tenantID).FirstOrDefault(cnd => cnd.DMT_Code == documentType && !cnd.DMT_IsDeleted).DMT_ID;

                return BALUtils.GetTicketsCentreRepoInstance(tenantID).GetTicketDocument(documentID, documentTypeID);


            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        //public static void SendEmail(TicketsContract ticketContract, int ticketID, string actionName)
        //{
        //    try
        //    {
        //        List<int> lstSendToUsers = new List<int>();
        //        string ticketAssignToUserName = string.Empty;
        //        //ticketContract.TicketSummary = AESEncryptDecrypt.Decrypt(ticketContract.TicketSummary);
        //        //ticketContract.TicketDetail = AESEncryptDecrypt.Decrypt(ticketContract.TicketDetail);
        //        if (ticketContract.SendToList.IsNotNull() && ticketContract.SendToList.Count > 0)
        //            lstSendToUsers = ticketContract.SendToList.Select(x => x.UserId).ToList();

        //        if (ticketContract.AssignToUserID > AppConsts.NONE)
        //        {
        //            lstSendToUsers.Add(ticketContract.AssignToUserID);
        //            List<int> AssignToUserID = new List<int>();
        //            AssignToUserID.Add(ticketContract.AssignToUserID);
        //            List<Entity.OrganizationUser> assignToUser = MessageManager.GetOrganizationUsers(AssignToUserID);
        //            if (assignToUser != null)
        //            {
        //                ticketAssignToUserName = assignToUser.FirstOrDefault().FirstName + " " + assignToUser.FirstOrDefault().LastName;
        //            }
        //        }


        //        if (lstSendToUsers.Count > 0)
        //        {
        //            List<Entity.OrganizationUser> lstOrganizationUsers = MessageManager.GetOrganizationUsers(lstSendToUsers);
        //            for (int users = 0; users < lstOrganizationUsers.Count; users++)
        //            {
        //                String messageToEmail = lstOrganizationUsers.Where(orgUser => orgUser.OrganizationUserID == Convert.ToInt32(lstSendToUsers[users])).FirstOrDefault().aspnet_Users.aspnet_Membership.Email;
        //                String messageToName = string.Concat(lstOrganizationUsers.Where(orgUser => orgUser.OrganizationUserID == Convert.ToInt32(lstSendToUsers[users])).FirstOrDefault().FirstName, " ", lstOrganizationUsers.Where(orgUser => orgUser.OrganizationUserID == Convert.ToInt32(lstSendToUsers[users])).FirstOrDefault().LastName);


        //                Dictionary<String, Object> dictMailData = new Dictionary<String, Object>();
        //                CommunicationMockUpData mockData = new CommunicationMockUpData();

        //                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, messageToName);
        //                dictMailData.Add(EmailFieldConstants.TENANT_ID, ticketContract.ClientId);
        //                dictMailData.Add(EmailFieldConstants.TICKET_ID, ticketID);
        //                dictMailData.Add(EmailFieldConstants.TICKET_SUMMARY, ticketContract.TicketSummary);
        //                dictMailData.Add(EmailFieldConstants.TICKET_PRIORITY, ticketContract.SeverityName);
        //                dictMailData.Add(EmailFieldConstants.TICKET_STATUS, ticketContract.TicketStatus);
        //                dictMailData.Add(EmailFieldConstants.TICKET_ASSIGN_TO, ticketAssignToUserName);

        //                mockData.EmailID = messageToEmail;
        //                mockData.UserName = messageToName;
        //                mockData.ReceiverOrganizationUserID = Convert.ToInt32(lstSendToUsers[users]);

        //                if (!lstSendToUsers[users].IsNullOrEmpty())
        //                {
        //                    CommunicationManager.SaveNotificationMailContent(actionName == PageAction.Add.GetStringValue() ?
        //                                                            CommunicationSubEvents.NOTIFICATION_FOR_TICKET : CommunicationSubEvents.NOTIFICATION_FOR_TICKET_UPDATE
        //                                                            , dictMailData
        //                                                            , mockData
        //                                                            , ticketContract.ClientId
        //                                                            , null
        //                                                            , null);
        //                }
        //            }
        //        }

        //        //return BALUtils.GetAppointmentRepoInstance().GetAppointmentAttendees(clientId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

        //    }
        //    //return null;
        //}

        //public static void UpdateWorkItemStatus(int WorkItemID, int loggedInUserID)
        //{
        //    try
        //    {
        //        Int32 statusID = AppConsts.NONE;
        //        lkpWorkItemStatu workItemStatusObj = LookupManager.GetLookUpData<lkpWorkItemStatu>().Where(x => x.Code == "AAAD" && x.IsDeleted == false).FirstOrDefault();
        //        if (workItemStatusObj != null)
        //            statusID = workItemStatusObj.WorkItemStatusID;

        //        if (statusID > 0)
        //            BALUtils.GetTicketsCentreRepoInstance().UpdateWorkItemStatus(WorkItemID, statusID, loggedInUserID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

        //    }
        //}

        #region  Bug 19211
        //public static List<TicketIssueSrvcMapping> IsTicketCreatedForServices(List<Int64> lstClientSrvcMappingIDs)
        //{
        //    try
        //    {
        //        return BALUtils.GetTicketsCentreRepoInstance().IsTicketCreatedForServices(lstClientSrvcMappingIDs);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

        //    }
        //    return new List<TicketIssueSrvcMapping>();
        //}
        #endregion

        //#region Automatic Ticket Creation
        //public static Boolean CreateAutomaticTicket(TicketsContract TicketContract, Int32 currentLoggedInUserID)
        //{
        //    try
        //    {
        //        if (!TicketContract.IsNullOrEmpty())
        //        {
        //            List<lkpTicketIssueType> listTicketIssueTypes = LookupManager.GetLookUpData<lkpTicketIssueType>().ToList();


        //            TicketContract.TicketIssueTypeID = listTicketIssueTypes.Any(x => x.Code == TicketIssueType.Automatic.GetStringValue()) ?
        //                listTicketIssueTypes.FirstOrDefault(x => x.Code == TicketIssueType.Automatic.GetStringValue()).TicketIssueTypeID : AppConsts.NONE;

        //            DataTable dtUDTTDocuments = ConvertToUDTTDocuments(TicketContract.AttachedDocuments);

        //            return BALUtils.GetTicketsCentreRepoInstance().CreateAutomaticTicket(TicketContract, dtUDTTDocuments, currentLoggedInUserID);
        //        }
        //        return false;
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //#endregion

        #region Ticket Center Settings

        //public static List<TicketEmailLOBMapping> GetTicketCenterSettings(Int32 LOBID = AppConsts.NONE)
        //{
        //    try
        //    {
        //        return BALUtils.GetTicketsCentreRepoInstance().GetTicketCenterSettings(LOBID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //public static Boolean SaveUpdateTicketEmailLOBMapping(TicketEmailLOBMapping ticketEmailLOBMapping, Int32 TicketEmailLOBMappingID)
        //{
        //    try
        //    {
        //        ticketEmailLOBMapping.TicketEmailSetting.Password = AESEncryptDecrypt.Encrypt(ticketEmailLOBMapping.TicketEmailSetting.Password);
        //        return TicketEmailLOBMappingID == AppConsts.NONE ? BALUtils.GetTicketsCentreRepoInstance().SaveTicketEmailLOBMapping(ticketEmailLOBMapping) :
        //            BALUtils.GetTicketsCentreRepoInstance().UpdateTicketEmailLOBMapping(ticketEmailLOBMapping, TicketEmailLOBMappingID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //public static Boolean DeleteTicketSetting(Int32 TicketEmailLOBMappingID, Int32 CurrentUserId)
        //{
        //    try
        //    {
        //        return BALUtils.GetTicketsCentreRepoInstance().DeleteTicketSetting(TicketEmailLOBMappingID, CurrentUserId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //public static Boolean CheckEmailAlreadyExists(String email, Boolean isUpdate, Int32 TicketEmailLOBMappingID)
        //{
        //    try
        //    {
        //        return BALUtils.GetTicketsCentreRepoInstance().CheckEmailAlreadyExists(email, isUpdate, TicketEmailLOBMappingID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        #endregion

        public static List<ClientSrvcMappingContract> GetAllClientServicesMapping(Int32 tenantID, Int32? LOBId = null)
        {
            try
            {
                ClientSrvcMappingContract clientSrvcMappingContract = null;
                List<ClientSrvcMappingContract> lstClientSrvcMappingContract = new List<ClientSrvcMappingContract>();
                List<ClientSrvcMapping> clientSrvcMapping = BALUtils.GetTicketsCentreRepoInstance(tenantID).GetAllClientServiceMapping(tenantID, LOBId);
                foreach (var row in clientSrvcMapping)
                {
                    clientSrvcMappingContract = new ClientSrvcMappingContract();
                    clientSrvcMappingContract.ClientSrvcMappingID = row.ClientSrvcMappingID;
                    clientSrvcMappingContract.ServcieName = row.CUService.Name;
                    lstClientSrvcMappingContract.Add(clientSrvcMappingContract);
                }
                return lstClientSrvcMappingContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }


    }
}
