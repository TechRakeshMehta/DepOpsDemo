using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using Entity;
using System.Collections.Generic;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using System.Linq;
using Telerik.Web.UI;
using System.Web.Services;
using Business.RepoManagers;
using INTSOF.UI.Contract.Messaging;


namespace CoreWeb.Messaging.Views
{
    public partial class MoveToFolders : System.Web.UI.Page, IMoveToFoldersView
    {
        private MoveToFoldersPresenter _presenter=new MoveToFoldersPresenter();
        private List<lkpMessageFolder> _folderList;

        /// <summary>
        /// Get the current userid
        /// </summary>
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
        /// Get and set the presenter.
        /// </summary>
        /// <summary>
        /// Get and set the folderlist
        /// </summary>
        public List<lkpMessageFolder> FolderList
        {
            get
            {
                return _folderList;
            }
            set
            {
                _folderList = value;
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            BindTreeView();
            if (Session[AppConsts.SESSION_QUEUE_TYPE_KEY].IsNull())
            {
                Presenter.GetQueueType();
            }
            String myscript = "var currentUserID = " + CurrentUserID.ToString() + "; var queueType = " + Session["QueueType"].ToString() + "; var __newFolderImg = '" + Page.ResolveUrl("~/Resources/Mod/Messaging/Images/folder.gif") + "';";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", myscript, true);
        }

        
        public MoveToFoldersPresenter Presenter
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

        /// <summary>
        /// Bind treeView.
        /// </summary>
        private void BindTreeView()
        {
            Presenter.GetFolders();
            //RadTreeView1.DataSource = Presenter.View.FolderList;            

            // RadTreeView1.DataSource = FolderList.Select(con => new { MessageFolderCode = String.Format("{0}#{1}", con.MessageFolderID, con.MessageFolderCode.IsNull() ? String.Empty : con.MessageFolderCode), MessageFolderName = con.MessageFolderName });
            treePersonalFolders.DataSource = FolderList.Select(con => new
            {
                MessageFolderCode = String.Format("{0}#{1}", con.MessageFolderID, con.Code),
                MessageFolderName = con.Name,
                MessageFolderID = con.MessageFolderID,
                MessageFolderParentID = con.MessageFolderParentID,
                ImageUrl = con.ImageUrl,
                IsDefaultFolder = con.IsDefault

            });

            treePersonalFolders.DataValueField = "MessageFolderCode";
            treePersonalFolders.DataTextField = "MessageFolderName";
            treePersonalFolders.DataFieldID = "MessageFolderID";
            treePersonalFolders.DataFieldParentID = "MessageFolderParentID";
            treePersonalFolders.DataBind();
        }

        protected void treeFolders_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            string ImageUrl = (e.Node.DataItem as dynamic).ImageUrl;
            Boolean IsDefaultFolder = (e.Node.DataItem as dynamic).IsDefaultFolder;
            String sourceFoldecode = Request["selectedFolder"];
            if (ImageUrl == null)
            {
                e.Node.ImageUrl = "~/Resources/Mod/Messaging/Images/folder.gif";
            }
            else
            {
                e.Node.ImageUrl = ImageUrl;
            }
            if (e.Node.Value.Split('#')[1] == lkpMessageFolderContext.PERSONALFOLDERS.GetStringValue() || e.Node.Value.Split('#')[1] == lkpMessageFolderContext.JUNK.GetStringValue() ||
                e.Node.Value.Split('#')[1] == lkpMessageFolderContext.INBOX.GetStringValue()|| IsDefaultFolder == false)
            {
                e.Node.Enabled = true;
            }
            else
            {
                e.Node.Enabled = false;
            }
            if (e.Node.Value.Split('#')[1] == lkpMessageFolderContext.PERSONALFOLDERS.GetStringValue())
            {
                e.Node.Expanded = true;
            }
           
        }

        /// <summary>
        /// Invoked to add new folder for a particular user.
        /// </summary>
        /// <param name="nodeText"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        [WebMethod]
        public static String AddNewFolder(String nodeText, Int32 currentUserId, Int32 userGroup, Int32 parentFolderID)
        {
            return MessageManager.AddNewFolder(nodeText.Trim(), currentUserId, userGroup, parentFolderID);
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
            MessageSearchContract messageSearchContract = new MessageSearchContract();
            messageSearchContract.SenderId = from;
            messageSearchContract.ToUserList = to;
            messageSearchContract.Subject = subject;
            messageSearchContract.MessageBody = System.Web.HttpUtility.HtmlEncode(body);
            List<MessageDetail> mails = MessageManager.GetQueue(QueueConstants.MESSAGEQUEUE, folderID, folderCode, queueTypeID, queueOwnerId, userGroup, false, DateTime.Now, messageSearchContract);
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
            filteredMessages = orderedMails.Skip(startIndex).Take(maximumRows).ToList();
            filteredMessages[0].TotalRecords = mails.Count;
            return filteredMessages;
        }
    }


}

