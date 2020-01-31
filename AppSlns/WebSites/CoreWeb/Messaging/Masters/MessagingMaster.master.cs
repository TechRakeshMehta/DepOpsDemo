using System;
using Microsoft.Practices.ObjectBuilder;
using Telerik.Web.UI;
using Entity;
using System.Collections.Generic;
using INTERSOFT.WEB.UI.WebControls;
using System.Linq;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using System.Web.Services;
using Business.RepoManagers;
using System.Web.Security;


namespace CoreWeb.Messaging.MasterPages
{
    public partial class MessagingMaster : System.Web.UI.MasterPage, IMessagingMasterView
    {
        SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
        private List<lkpMessageFolder> _folderList;
        private List<lkpCommunicationType> _communicationTypeList;
        private MessagingMasterPresenter _presenter = new MessagingMasterPresenter();
        private List<MessagingGroup> _groupFolderList;

        /// <summary>
        /// Get the current userid
        /// </summary>
        public Int32 CurrentUserID
        {
            get
            {
                return user.OrganizationUserId;
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

        public int TimeoutMinutes
        {
            get
            {
                return ((Int32)FormsAuthentication.Timeout.TotalSeconds - 600);
            }
        }

        /// <summary>
        /// Get or set the list of communication type
        /// </summary>
        public List<lkpCommunicationType> CommunicationTypeList
        {
            get
            {
                return _communicationTypeList;
            }
            set
            {
                _communicationTypeList = value;
            }
        }

        /// <summary>
        /// Get and set the presenter.
        /// </summary>
        /// <summary>
        /// Get and set the folderlist
        /// </summary>
        public List<MessagingGroup> GroupFolderList
        {
            get
            {
                return _groupFolderList;
            }
            set
            {
                _groupFolderList = value;
            }
        }

        public String UserName
        {
            get
            {
                return SysXWebSiteUtils.SessionService.SysXMembershipUser.UserName;
            }
        }
        public bool IsApplicant
        {
            get
            {
                return SecurityManager.GetOrganizationUser(CurrentUserID).IsApplicant.GetValueOrDefault(false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                hdntimeout.Value = TimeoutMinutes.ToString();
            }
            Presenter.OnViewLoaded();
            BindTreeView();
            //BindCommunicationTypes();
            BindGroupTree();
            BindMoveToFolders();
            if (IsApplicant)
            {
                Boolean IsApplicantAllowToSendMessages = Presenter.CheckApplicantClientSettings() ? true : false;
                if (!IsApplicantAllowToSendMessages)
                {
                    hdnIsApplicantAllowToSendMessages.Value = "false";
                    RadToolBarItem senditemToDisable = tlbMessageMain.FindItemByValue("Compose");
                    senditemToDisable.Enabled = false;
                    RadToolBarItem replyitemToDisableMngeTmplte = tlbMessageMain.FindItemByText("Reply");
                    replyitemToDisableMngeTmplte.Enabled = false;
                    RadToolBarItem replyAllitemToDisableMngeTmplte = tlbMessageMain.FindItemByText("Reply All");
                    replyAllitemToDisableMngeTmplte.Enabled = false;
                    RadToolBarItem forwarditemToDisableMngeTmplte = tlbMessageMain.FindItemByText("Forward");
                    forwarditemToDisableMngeTmplte.Enabled = false;
                }
                RadToolBarItem itemToDisable = tlbMessageMain.FindItemByValue("Rules");
                RadToolBarItem itemToDisableMngeTmplte = tlbMessageMain.FindItemByText("Manage Template");
                itemToDisableMngeTmplte.Visible = false;
                itemToDisable.Visible = false;
            }
        }


        public MessagingMasterPresenter Presenter
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

            var folderList = (from n in FolderList
                              let folderCount = Presenter.GetFolderCount(n.MessageFolderID, n.Code, CurrentUserID)
                              select new
                              {
                                  MessageFolderCode = String.Format("{0}#{1}", n.MessageFolderID, n.Code),
                                  MessageFolderName = String.Format("{0}{1}", n.Name, folderCount > 0 ? String.Format("({0})", Convert.ToString(folderCount)) : String.Empty),
                                  MessageFolderID = n.MessageFolderID,
                                  MessageFolderParentID = n.MessageFolderParentID,
                                  ImageUrl = n.ImageUrl,
                                  IsDefault = n.IsDefault
                              });

            //treePersonalFolders.DataSource = FolderList.Select(con => new
            //{
            //    MessageFolderCode = String.Format("{0}#{1}", con.MessageFolderID, con.Code),
            //    MessageFolderName = String.Format("{0}{1}", con.Name, Presenter.GetFolderCount(con.MessageFolderID, con.Code, CurrentUserID) > 0 ? String.Format("({0})", Convert.ToString(Presenter.GetFolderCount(con.MessageFolderID, con.Code, user.OrganizationUserId))) : String.Empty),
            //    MessageFolderID = con.MessageFolderID,
            //    MessageFolderParentID = con.MessageFolderParentID,
            //    ImageUrl = con.ImageUrl,
            //    IsDefault = con.IsDefault

            //});

            treePersonalFolders.DataSource = folderList;
            treePersonalFolders.DataValueField = "MessageFolderCode";
            treePersonalFolders.DataTextField = "MessageFolderName";
            treePersonalFolders.DataFieldID = "MessageFolderID";
            treePersonalFolders.DataFieldParentID = "MessageFolderParentID";
            treePersonalFolders.DataBind();
        }

        /// <summary>
        /// Bind the communication types
        /// </summary>
        private void BindCommunicationTypes()
        {
            Presenter.GetCommuncationTypes(new Guid(SysXWebSiteUtils.SessionService.UserId));
            RadToolBarSplitButton rsbtnCommunicationTypes = tlbMessageMain.Items.FindItemByValue("Compose") as RadToolBarSplitButton;


            var lstCommunicationType = CommunicationTypeList.Select(con => new { CommunicationTypeCode = String.Format("{0}#{1}", con.CommunicationTypeID, con.Code.IsNull() ? String.Empty : con.Code), CommunicationTypeName = con.Name });

            foreach (var communicationType in lstCommunicationType)
            {
                if (!communicationType.CommunicationTypeName.ToLower().Trim().Contains(AppConsts.COMBOBOX_ITEM_SELECT.ToLower()))
                    rsbtnCommunicationTypes.Buttons.Add(new RadToolBarButton { Text = communicationType.CommunicationTypeName, CommandName = "NewMessage", Value = communicationType.CommunicationTypeCode });
            }
        }

        /// <summary>
        /// Bind Move To Folders list
        /// </summary>
        private void BindMoveToFolders()
        {

            RadToolBarSplitButton rsbtnMovetoFolders = tlbMessageMain.Items.FindItemByValue("MoveToFolders") as RadToolBarSplitButton;
            var lstMoveToFolders = FolderList.Where(folder => folder.IsDefault == false && folder.IsDeleted == false).Select(folder => new { MoveToFolderCode = String.Format("{0}#{1}", folder.MessageFolderID, folder.Code.IsNull() ? String.Empty : folder.Code), FolderName = folder.Name, CreationDate = folder.CreatedOn, IsDefault = folder.IsDefault });
            lstMoveToFolders = lstMoveToFolders.OrderByDescending(dt => dt.CreationDate).Take(3);

            foreach (var folder in lstMoveToFolders)
            {
                rsbtnMovetoFolders.Buttons.Add(new RadToolBarButton { Text = folder.FolderName, CommandName = "MoveTo", Value = folder.MoveToFolderCode });
            }

            rsbtnMovetoFolders.Buttons.Add(new RadToolBarButton { IsSeparator = true });
            rsbtnMovetoFolders.Buttons.Add(new RadToolBarButton { Text = "More Folders...", CommandName = "MoreFolders" });
        }


        private void BindGroupTree()
        {
            Presenter.GetgroupFolders();
            treeGroupFolders.DataSource = GroupFolderList.Select(con => new
            {
                MessageGroupID = String.Format("{0}#{1}", con.MessagingGroupID, con.GroupCode),
                GroupName = String.Format("{0}{1}", con.GroupName, Presenter.GetGroupFolderCount(con.MessagingGroupID, con.GroupCode) > 0 ? String.Format("({0})", Convert.ToString(Presenter.GetGroupFolderCount(con.MessagingGroupID, con.GroupCode))) : String.Empty)
            });
            treeGroupFolders.DataValueField = "MessageGroupID";
            treeGroupFolders.DataTextField = "GroupName";
            treeGroupFolders.DataBind();
        }

        protected void treeFolders_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            string ImageUrl = (e.Node.DataItem as dynamic).ImageUrl;
            if (ImageUrl == null)
            {
                e.Node.ImageUrl = "~/Resources/Mod/Messaging/Images/folder.gif";
            }
            else
            {
                e.Node.ImageUrl = ImageUrl;
            }
            if (e.Node.Value.Split('#')[1] == lkpMessageFolderContext.PERSONALFOLDERS.GetStringValue())
            {
                e.Node.Expanded = true;
            }
        }

        protected void treeGroupFolders_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "~/Resources/Mod/Messaging/Images/folder.gif";
        }
    }
}