using System;
using Microsoft.Practices.ObjectBuilder;
using System.Web.Services;
using System.Collections.Generic;
using Entity;
using Business.RepoManagers;
using INTSOF.Utils;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;
using Telerik.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using System.Web.Script.Services;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.UI.Contract.Messaging;
using System.Web;

namespace CoreWeb.Messaging.Views
{
    public partial class Messaging : BaseWebPage, IMessagingView
    {
        private MessagingPresenter _presenter = new MessagingPresenter();
        private ISysXExceptionService _exceptionService = SysXWebSiteUtils.ExceptionService;

        public Int32 CurrentUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// Get the current userid
        /// </summary>
        public Int32 UserGroupID
        {
            get
            {
                return AppConsts.NONE;
            }
        }
        /// <summary>
        /// Get and set the flag.
        /// </summary>
        public Boolean Flag
        {
            get;
            set;
        }
        /// <summary>
        /// Get queueType
        /// </summary>
        public Int32 QueueType
        {
            set
            {
                Session["QueueType"] = value;
            }
        }

        public Int32 SytemCommunicationUserId
        {
            get
            {
                AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.SYSTEM_COMMUNICATION_USER_ID);
                Int32 systemCommunicationUserId = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    systemCommunicationUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }
                return systemCommunicationUserId;
            }
        }

        public Int32 BackgroundProcessUserId
        {
            get
            {
                AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    backgroundProcessUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }
                return backgroundProcessUserId;
            }
        }

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="ex"></param>
        private void LogError(String errorMessage, System.Exception ex)
        {
            _exceptionService.HandleError(errorMessage, ex);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                Shell.MasterPages.IDefaultMasterView master;
                var pageMaster = Page.Master; ;
                ContentPlaceHolder contentPlaceHolder = (ContentPlaceHolder)pageMaster.FindControl("DefaultContent");
                while (contentPlaceHolder == null)//in case there are nested master pages
                {
                    pageMaster = pageMaster.Master;
                    contentPlaceHolder = (ContentPlaceHolder)pageMaster.FindControl("DefaultContent");
                }

                master = pageMaster as Shell.MasterPages.IDefaultMasterView;
                master.HideTitleBars();


                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }
                Presenter.OnViewLoaded();
                if (Session["QueueType"].IsNull())
                {
                    Presenter.GetQueueType();
                }
                String myscript = "var currentUserID = " + CurrentUserID.ToString() + "; var queueType = " + Session["QueueType"].ToString()
                + "; var backgroundProcessUserId = " + BackgroundProcessUserId.ToString() + "; var systemCommunicationUserId = " + SytemCommunicationUserId.ToString()
                + "; var __newFolderImg = '" + Page.ResolveUrl("~/Resources/Mod/Messaging/Images/folder.gif") + "';";

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", myscript, true);

                //base.Title = "Messaging";                
                base.Title = Resources.Language.MESSAGING;
                base.BreadCrumbTitleKey = "Key_MESSAGING";
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }


        protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //RadGrid1.DataSource = new List<object>();
        }

        public MessagingPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("ChangeFlag"))
            {
                ImageButton imgMailFlag = (ImageButton)e.Item.FindControl("imgMailFlagImageButton");
                bool flagged = Convert.ToBoolean(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]);
                if (flagged.Equals(false))
                {
                    //SelectedMessageID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MessageDetail.MessageDetailID"]);
                    Flag = true;
                    //FolderList = MessagingFolder.FollowUp;
                    //this.Presenter.FollowUp();
                    imgMailFlag.ImageUrl = "/Resources/Mod/Messaging/Images/mailFlagRed.gif";
                }
                else
                {
                    //SelectedMessageID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MessageDetail.MessageDetailID"]);
                    Flag = false;
                    //FolderList = MessagingFolder.Inbox;
                    //this.Presenter.FollowUp();
                    imgMailFlag.ImageUrl = "/Resources/Mod/Messaging/Images/mailFlag.gif";
                }
            }
        }
        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
            {
                (e.Item.FindControl("imgMailFlagImageButton") as ImageButton).Attributes.Add("OnClick", "return ChgFlag(" + e.Item.ItemIndex.ToString() + ", this)");
                (e.Item.FindControl("imgRestoreButton") as ImageButton).Attributes.Add("OnClick", "return RestoreMessage(" + e.Item.ItemIndex.ToString() + ", this)");
            }

        }

        public static List<MessageDetail> SortedResults(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<MessageDetail> mails)
        {
            IOrderedEnumerable<MessageDetail> orderedMails = null;
            List<MessageDetail> filteredMessages = new List<MessageDetail>();
            String[] header = sortExpressions.Split(new char[] { ' ' });

            if (header[0].Equals("From", StringComparison.OrdinalIgnoreCase))
            {
                if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    orderedMails = mails.OrderByDescending(md => md.From);
                }
                else
                {
                    orderedMails = mails.OrderBy(md => md.From);
                }
            }
            else if (header[0].Equals("Subject", StringComparison.OrdinalIgnoreCase))
            {
                if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    orderedMails = mails.OrderByDescending(md => md.Subject);
                }
                else
                {
                    orderedMails = mails.OrderBy(md => md.Subject);
                }
            }
            else if (header[0].Equals("ReceivedDate", StringComparison.OrdinalIgnoreCase))
            {
                if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    orderedMails = mails.OrderByDescending(md => md.ReceivedDate);
                }
                else
                {
                    orderedMails = mails.OrderBy(md => md.ReceivedDate);
                }
            }
            else if (header[0].Equals("Size", StringComparison.OrdinalIgnoreCase))
            {
                if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    orderedMails = mails.OrderByDescending(md => md.Size);
                }
                else
                {
                    orderedMails = mails.OrderBy(md => md.Size);
                }
            }
            else if (header[0].Equals("To", StringComparison.OrdinalIgnoreCase))
            {
                if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    orderedMails = mails.OrderByDescending(md => md.To);
                }
                else
                {
                    orderedMails = mails.OrderBy(md => md.To);
                }
            }


            filteredMessages = orderedMails.Skip(startIndex).Take(maximumRows).ToList();
            filteredMessages[0].TotalRecords = mails.Count;
            return filteredMessages;
        }



        [WebMethod]
        public static List<MessageDetail> GetData(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<GridFilterExpression> filterExpressions, Int32 folderId, String folderCode, Int32 queueOwnerId, Int32 userGroup, Int32 queueTypeID, String from, String to, String subject, String body)
        {
            try
            {
                return GetMessageQueueData(startIndex, maximumRows, sortExpressions, filterExpressions, folderId, folderCode, queueOwnerId, userGroup, queueTypeID, from, to, subject, body);
            }
            catch (Exception ex)
            {
                List<MessageDetail> msg = new List<MessageDetail>();
                msg.Add(new MessageDetail() { From = ex.Message });
                return msg;
            }
        }

        private static List<MessageDetail> GetMessageQueueData(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<GridFilterExpression> filterExpressions, Int32 folderId, String folderCode, Int32 queueOwnerId, Int32 userGroup, Int32 queueTypeID, String from, String to, String subject, String body)
        {
            MessageSearchContract messageSearchContract = new MessageSearchContract();
            messageSearchContract.SenderId = from;
            messageSearchContract.ToUserList = to;
            messageSearchContract.Subject = subject;
            messageSearchContract.MessageBody = System.Web.HttpUtility.HtmlEncode(body);
            List<MessageDetail> mails = MessageManager.GetQueue(QueueConstants.MESSAGEQUEUE, folderId, folderCode, queueTypeID, queueOwnerId, userGroup, false, DateTime.Now, messageSearchContract);
            List<MessageDetail> page = new List<MessageDetail>();
            if (!mails.IsNull() && mails.Count > 0)
            {
                if (sortExpressions.IsNullOrEmpty())
                {
                    page = mails.Skip(startIndex).Take(maximumRows).ToList();
                    page[0].TotalRecords = mails.Count;

                }
                else
                {
                    page = SortedResults(startIndex, maximumRows, sortExpressions, mails);
                }

                if (filterExpressions.Count > 0)
                {
                    foreach (GridFilterExpression expression in filterExpressions)
                    {
                        page = page.FindAll(new CoreWeb.AppUtils.GridGenericFilterer<MessageDetail>(expression).Filter);
                    }
                }
                return page;
            }
            else
            {
                return new List<MessageDetail>();
            }
        }

        /// <summary>
        /// Get message content according to messageid, currentUserId and queuetype
        /// </summary>
        /// <param name="messageId">messageId</param>
        /// <param name="currentUserId">currentUserId</param>
        /// <param name="queueTypeID">queueTypeID</param>
        /// <returns></returns>
        [WebMethod]
        public static String GetMessageContent(String messageId, Int32 currentUserId, Int32 queueTypeID)
        {
            Guid currentMessageId = new Guid(messageId);
            ComplexObject message = MessageManager.GetCommonMessageContents(queueTypeID, currentMessageId);
            StringBuilder content = new StringBuilder();

            //COMMENTED TO REMOVE THE EXTRA BREAK TAG WHEN CONTENT IS LOADED IN THE TEXT EDITOR
            //content.Append("<br/> ");
            //content.Append("<br/> ");

            String eVaultDocumentID = String.Empty;
            String documentName = String.Empty;

            String originalDocumentName = String.Empty;

            GetADBMessageContentResult adbMessageContent = message as GetADBMessageContentResult;
            content.Append("<br/> " + SysXUtils.GetXmlDecodedString(adbMessageContent.MessageBody));
            content.Append("<br/><div class='sxseparator'></div>");
            eVaultDocumentID = adbMessageContent.EVaultDocumentID;
            documentName = adbMessageContent.DocumentName;
            originalDocumentName = adbMessageContent.OriginalDocumentName;
            String[] arrEVaultDocumentID = { };
            String[] arrDocumentName = { };

            if (!string.IsNullOrEmpty(eVaultDocumentID))
                arrEVaultDocumentID = eVaultDocumentID.Split(new char[] { ',' });
            if (!string.IsNullOrEmpty(documentName))
                arrDocumentName = documentName.Split(new char[] { ',' });

            if (!originalDocumentName.IsNullOrEmpty())
            {
                content.Append("<b>Attached Documents:</b></br>");

                content.Append("<a target=\"blank\"  href=\"" + documentName + "\">" + originalDocumentName + "</a><br/>");
            }
            return content.ToString();
        }

        /// <summary>
        /// Update Read status.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="queueTypeID"></param>
        /// <returns></returns>

        [WebMethod]
        public static Boolean UpdateReadStatus(String messageId, Int32 queueOwnerId, Int32 queueTypeID, Int32 isUnread)
        {
            Guid currentMessageId = new Guid(messageId);
            return MessageManager.UpdateReadStatus(currentMessageId, queueOwnerId, queueTypeID, Convert.ToBoolean(isUnread));
        }

        [WebMethod]
        public static Boolean CheckSubscribeToEmail(Int32 queueOwnerId)
        {
            return MessageManager.CheckSubscriptionStatus(queueOwnerId);
        }

        [WebMethod]
        public static Boolean UpdateSubscriptionStatus(Int32 queueOwnerId, Boolean status)
        {
            return MessageManager.UpdateSubscriptionStatus(queueOwnerId, status);
        }

        /// <summary>
        /// Update Followup status.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="queueTypeID"></param>
        /// <returns></returns>
        [WebMethod]
        public static Boolean UpdateFollowUpStatus(String messageId, Int32 queueOwnerId, Int32 queueTypeID, Int32 isFollowUp, String folderCode)
        {
            Guid currentMessageId = new Guid(messageId);
            return MessageManager.UpdateFollowUpStatus(currentMessageId, queueOwnerId, queueTypeID, Convert.ToBoolean(isFollowUp), folderCode);
        }

        /// <summary>
        /// Invoked to delete the message and update the respective folder.
        /// </summary>
        /// <param name="folderID">folderID</param>
        /// <param name="messageId">messageId</param>
        /// <param name="queueOwnerId">queueOwnerId</param>
        /// <param name="queueTypeID">queueTypeID</param>
        /// <returns></returns>
        [WebMethod]
        public static List<MessageDetail> DeleteMessageAndUpdateResult(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<GridFilterExpression> filterExpressions, Int32 folderID, String folderCode, String messageId, Int32 queueOwnerId, Int32 userGroup, Int32 queueTypeID, String from, String to, String subject, String body)
        {
            Guid currentMessageId = new Guid(messageId);
            Boolean isDeleted = MessageManager.DeleteMesssage(currentMessageId, queueOwnerId, folderCode, queueTypeID);
            return GetMessageQueueData(startIndex, maximumRows, sortExpressions, filterExpressions, folderID, folderCode, queueOwnerId, userGroup, queueTypeID, from, to, subject, body);
        }

        /// <summary>
        /// Delete the folder and its messages.
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="messageId"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="queueTypeID"></param>
        /// <returns>boolean</returns>
        [WebMethod]
        public static Boolean DeleteFolderAndMessage(Int32 folderId, Int32 queueOwnerId, Int32 queueTypeID)
        {
            return MessageManager.DeleteFolderAndMesssage(folderId, queueOwnerId, queueTypeID);
        }

        /// <summary>
        /// Invoked to add new folder for a particular user.
        /// </summary>
        /// <param name="nodeText"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static String AddNewFolder(String nodeText, Int32 currentUserId, Int32 userGroup, Int32 parentFolderID)
        {
            return MessageManager.AddNewFolder(nodeText.Trim(), currentUserId, userGroup, parentFolderID);
        }

        [WebMethod]
        public static List<MessageDetail> GetUserGroupQueueData(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<GridFilterExpression> filterExpressions, Int32 userGroupID, Int32 queueOwnerId, String from, String to, String subject, String body)
        {
            try
            {
                IQueryable<vw_groupMessages> mails = null;
                // IQueryable<vw_groupMessages> mails = MessageManager.GetQueueForGroupFolders(userGroupID);
                MessageSearchContract messageSearchContract = new MessageSearchContract();
                messageSearchContract.SenderId = from;
                messageSearchContract.ToUserList = to;
                messageSearchContract.Subject = subject;
                messageSearchContract.MessageBody = System.Web.HttpUtility.HtmlEncode(body);
                messageSearchContract.UserGroupID = userGroupID;
                mails = MessageManager.PerformSearchForGroupMessageQueues<vw_groupMessages>(messageSearchContract);
                IQueryable<vw_groupMessages> page = null;
                List<MessageDetail> filteredRecords = new List<MessageDetail>();
                if (!mails.IsNull() && mails.Count() > 0)
                {
                    if (sortExpressions.IsNullOrEmpty())
                    {
                        page = mails.Skip(startIndex).Take(maximumRows);
                        //page[0].TotalRecords = mails.Count;
                    }
                    else
                    {
                        page = ApplypagingSortingOnGroupMessages(startIndex, maximumRows, sortExpressions, mails);
                    }

                    filteredRecords = MessageManager.GetADBGroupMessageDetails(page.ToList());
                    filteredRecords[0].TotalRecords = mails.Count();
                    return filteredRecords;
                }
                else
                {
                    return new List<MessageDetail>();
                }
            }
            catch (Exception ex)
            {
                List<MessageDetail> msg = new List<MessageDetail>();
                msg.Add(new MessageDetail() { From = ex.Message });
                return msg;
            }
        }

        [WebMethod]
        public static List<MessageDetail> RestoreMessageAndUpdateResult(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<GridFilterExpression> filterExpressions, Int32 folderID, String folderCode, String messageId, Int32 queueOwnerId, Int32 userGroup, Int32 queueTypeID, String from, String to, String subject, String body)
        {
            Guid currentMessageId = new Guid(messageId);
            Boolean isMessageRestored = MessageManager.RestoreMessage(currentMessageId, queueOwnerId, queueTypeID);
            return GetMessageQueueData(startIndex, maximumRows, sortExpressions, filterExpressions, folderID, folderCode, queueOwnerId, userGroup, queueTypeID, from, to, subject, body);
            //List<MessageDetail> mails = MessageManager.GetQueue(QueueConstants.MESSAGEQUEUE, folderID, folderCode, queueTypeID, queueOwnerId, userGroup, false, DateTime.Now);
            //List<MessageDetail> page = new List<MessageDetail>();
            //if (!mails.IsNull() && mails.Count > 0)
            //{
            //    if (sortExpressions.IsNullOrEmpty())
            //    {
            //        page = mails.Skip(startIndex).Take(maximumRows).ToList();
            //        page[0].TotalRecords = mails.Count;

            //    }
            //    else
            //    {
            //        page = SortedResults(startIndex, maximumRows, sortExpressions, mails);
            //    }

            //    if (filterExpressions.Count > 0)
            //    {
            //        foreach (GridFilterExpression expression in filterExpressions)
            //        {
            //            page = page.FindAll(new CoreWeb.AppUtils.GridGenericFilterer<MessageDetail>(expression).Filter);
            //        }
            //    }
            //    return page;
            //}
            //else
            //{
            return new List<MessageDetail>();
            //}
        }

        /// <summary>
        /// Invoked to move the messages from one folder to another.
        /// </summary>
        /// <param name="folderID">folderID</param>
        /// <param name="moveToFolderId">moveToFolderId</param>
        /// <param name="messageId">messageId</param>
        /// <param name="queueOwnerId">queueOwnerId</param>
        /// <param name="queueTypeID">queueTypeID</param>
        /// <returns></returns>
        [WebMethod]
        public static List<MessageDetail> SetMoveToFolderAndUpdateResult(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<GridFilterExpression> filterExpressions, Int32 folderID, String folderCode, Int32 moveToFolderId, String moveToFolderCode, String messageId, Int32 queueOwnerId, Int32 userGroup, Int32 queueTypeID, String from, String to, String subject, String body)
        {
            Guid currentMessageId = new Guid(messageId);
            Boolean isDeleted = MessageManager.SetMoveToFolder(currentMessageId, queueOwnerId, folderCode, moveToFolderId, moveToFolderCode, queueTypeID);
            return GetMessageQueueData(startIndex, maximumRows, sortExpressions, filterExpressions, folderID, folderCode, queueOwnerId, userGroup, queueTypeID, from, to, subject, body);
            //List<MessageDetail> mails = MessageManager.GetQueue(QueueConstants.MESSAGEQUEUE, folderID, folderCode, queueTypeID, queueOwnerId, userGroup, false, DateTime.Now);
            //List<MessageDetail> page = new List<MessageDetail>();
            //if (!mails.IsNull() && mails.Count > 0)
            //{
            //    if (sortExpressions.IsNullOrEmpty())
            //    {
            //        page = mails.Skip(startIndex).Take(maximumRows).ToList();
            //        page[0].TotalRecords = mails.Count;

            //    }
            //    else
            //    {
            //        page = SortedResults(startIndex, maximumRows, sortExpressions, mails);
            //    }


            //    if (filterExpressions.Count > 0)
            //    {
            //        foreach (GridFilterExpression expression in filterExpressions)
            //        {
            //            page = page.FindAll(new CoreWeb.AppUtils.GridGenericFilterer<MessageDetail>(expression).Filter);
            //        }
            //    }
            //    return page;
            //}
            //else
            //{
            //  return new List<MessageDetail>();
            //}
        }

        private static String GetFolderName(String folderCode)
        {
            switch (folderCode)
            {
                case "MSGINB":
                    return "Inbox";
                case "MSGDEL":
                    return "Deleted Items";
                case "MSGDRF":
                    return "Drafts";
                default:
                    return "";
            }
        }


        [WebMethod]
        public static String UpdateFolderCount(String FolderName, Int32 folderID, String foldercode, Int32 queueOwnerId)
        {
            Int32 count = MessageManager.GetMessageCount(folderID, foldercode, queueOwnerId);
            FolderName = MessageManager.GetFolderName(folderID, foldercode);
            String folderName = String.Format("{0}{1}", FolderName, count > 0 ? String.Format("({0})", count) : String.Empty);
            String updateFolderParameter = folderName + "-" + folderID + "-" + foldercode;
            return updateFolderParameter;
        }

        [WebMethod]
        public static String CheckIfFolderNeedToBeRestored(Guid messageId, Int32 userId, Int32 queueTypeID)
        {
            Boolean result = MessageManager.CheckIfFolderNeedToBeRestored(messageId, userId, queueTypeID);
            String restoredFolder = MessageManager.FoldersToBeRestored(messageId, userId);
            return String.Format("{0}#{1}#{2}", result, messageId, restoredFolder);
        }

        /// <summary>
        /// Invoked to delete group message and update the respective folder.
        /// </summary>
        /// <param name="folderID">folderID</param>
        /// <param name="messageId">messageId</param>
        /// <param name="queueOwnerId">queueOwnerId</param>
        /// <param name="queueTypeID">queueTypeID</param>
        /// <returns></returns>
        [WebMethod]
        public static List<MessageDetail> DeleteGroupMessageAndUpdateResult(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<GridFilterExpression> filterExpressions, String messageId, Int32 userGroupID, Int32 queueOwnerId, String from, String to, String subject, String body)
        {
            Guid currentMessageId = new Guid(messageId);
            Boolean isDeleted = MessageManager.DeleteADBgroupMesssage(currentMessageId, userGroupID);
            return GetUserGroupQueueData(startIndex, maximumRows, sortExpressions, filterExpressions, userGroupID, queueOwnerId, from, to, subject, body);
        }
        public static IQueryable<vw_groupMessages> ApplypagingSortingOnGroupMessages(Int32 startIndex, Int32 maximumRows, String sortExpressions, IQueryable<vw_groupMessages> messageQueue)
        {
            IQueryable<vw_groupMessages> orderedMails = null;
            IQueryable<vw_groupMessages> filteredMessages = null;
            String[] header = sortExpressions.Split(new char[] { ' ' });

            if (header[0].Equals("From", StringComparison.OrdinalIgnoreCase))
            {
                if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    orderedMails = messageQueue.OrderByDescending(md => md.From);
                }
                else
                {
                    orderedMails = messageQueue.OrderBy(md => md.From);
                }
            }
            else if (header[0].Equals("Subject", StringComparison.OrdinalIgnoreCase))
            {
                if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    orderedMails = messageQueue.OrderByDescending(md => md.Subject);
                }
                else
                {
                    orderedMails = messageQueue.OrderBy(md => md.Subject);
                }
            }
            else if (header[0].Equals("ReceivedDate", StringComparison.OrdinalIgnoreCase))
            {
                if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    orderedMails = messageQueue.OrderByDescending(md => md.ReceiveDate);
                }
                else
                {
                    orderedMails = messageQueue.OrderBy(md => md.ReceiveDate);
                }
            }
            else if (header[0].Equals("Size", StringComparison.OrdinalIgnoreCase))
            {
                //    if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                //    {
                //        orderedMails = messageQueue.OrderByDescending(md => md.si);
                //    }
                //    else
                //    {
                //        orderedMails = messageQueue.OrderBy(md => md.Size);
                //    }
            }
            else if (header[0].Equals("To", StringComparison.OrdinalIgnoreCase))
            {
                if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
                {
                    orderedMails = messageQueue.OrderByDescending(md => md.Message.GetNodeContent("MessageSize"));
                }
                else
                {
                    orderedMails = messageQueue.OrderBy(md => md.Message.GetNodeContent("ToUserList"));
                }
            }


            filteredMessages = orderedMails.Skip(startIndex).Take(maximumRows);
            // filteredMessages[0].TotalRecords = mails.Count;
            return filteredMessages;
        }
        [WebMethod]
        public static Boolean SetGridPageSize(Int32 pageSize)
        {
            //HttpContext.Current.Profile.SetPropertyValue("PageSize", pageSize);
            //HttpContext.Current.Profile.Save();
            HttpContext.Current.Session["GRID_PAGE_SIZE"] = pageSize;
            return true;
        }
    }

}

