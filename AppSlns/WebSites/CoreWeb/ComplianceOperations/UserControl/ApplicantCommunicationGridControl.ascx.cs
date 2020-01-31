using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceOperations.Views;
using Entity;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ApplicantCommunicationGridControl : BaseUserControl, IApplicantCommunicationGridControlView
    {

        #region Variables

        #region Private Variables

        private ApplicantCommunicationGridControlPresenter _presenter = new ApplicantCommunicationGridControlPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public properties

        public ApplicantCommunicationGridControlPresenter Presenter
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

        public IApplicantCommunicationGridControlView CurrentViewContext
        {
            get { return this; }
        }
        public Int32 CurrentPageIndex
        {
            get
            {
                if (!rdlCommunicationMode.IsNullOrEmpty())
                {
                    if (string.Compare(rdlCommunicationMode.SelectedValue, "1") == 0)
                        return grdUserCommunicationGrid.MasterTableView.CurrentPageIndex + 1;
                    else
                        return grdUserEmailGrid.MasterTableView.CurrentPageIndex + 1;
                }
                else
                    return grdUserCommunicationGrid.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (!rdlCommunicationMode.IsNullOrEmpty())
                {
                    if (string.Compare(rdlCommunicationMode.SelectedValue, "1") == 0)
                        grdUserCommunicationGrid.MasterTableView.CurrentPageIndex = value - 1;
                    else
                        grdUserEmailGrid.MasterTableView.CurrentPageIndex = value - 1;
                }
                else
                    grdUserCommunicationGrid.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public Int32 VirtualPageCount
        {
            set
            {
                if (!rdlCommunicationMode.IsNullOrEmpty())
                {
                    if (string.Compare(rdlCommunicationMode.SelectedValue, "1") == 0)
                    {
                        grdUserCommunicationGrid.VirtualItemCount = value;
                        grdUserCommunicationGrid.MasterTableView.VirtualItemCount = value;
                    }
                    else
                    {
                        grdUserEmailGrid.VirtualItemCount = value;
                        grdUserEmailGrid.MasterTableView.VirtualItemCount = value;
                    }
                }
                else
                {
                    grdUserCommunicationGrid.VirtualItemCount = value;
                    grdUserCommunicationGrid.MasterTableView.VirtualItemCount = value;
                }
            }
        }

        public String DefaultSortExpression
        {
            get
            {
                if (ViewState["SortExpression"].IsNullOrEmpty())
                    return "ReceivedDateFormat";
                return Convert.ToString(ViewState["SortExpression"]);
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        public String DefaultEmailSortExpression
        {
            get
            {
                if (ViewState["SortExpression"].IsNullOrEmpty())
                    return "DispatchedDate";
                return Convert.ToString(ViewState["SortExpression"]);
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }



        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdUserCommunicationGrid.PageSize > 100 ? 100 : grdUserCommunicationGrid.PageSize;
                //UAT-3261: Badge Form Enhancements
                if (!rdlCommunicationMode.IsNullOrEmpty())
                {
                    if (string.Compare(rdlCommunicationMode.SelectedValue, "1") == 0)
                        return grdUserCommunicationGrid.PageSize;
                    else
                        return grdUserEmailGrid.PageSize;
                }
                return grdUserCommunicationGrid.PageSize;
            }
        }

        public Boolean IsSortDirectionDescending
        {
            set
            {
                ViewState["SortDirection"] = value;
            }
            get
            {
                if (ViewState["SortDirection"].IsNullOrEmpty())
                    return true;
                return Convert.ToBoolean(ViewState["SortDirection"]);
            }
        }

        public int UserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }
        }

        public int ApplicantDashboard
        {
            get;
            set;
        }

        public List<MessageDetail> lstUserMessageDetailList
        {
            get;
            set;
        }

        //UAT-3261: Badge Form Enhancements
        public List<EmailDetails> lstUserEmailDetailList
        {
            get;
            set;
        }

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }
        public String ControlUseType
        {
            get
            {
                if (ViewState["ControlUseType"] != null)
                    return (Convert.ToString(ViewState["ControlUseType"]));
                return String.Empty;
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }
        #region UAT-3261
        public List<Int32> SystemCommunicationDeliveryIds
        {
            get;
            set;
        }

        public Int32 SystemCommunicationId
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Private properties

        #endregion

        #endregion

        #region Events

        #region Page Events       

        protected void Page_Load(object sender, EventArgs e)
        {
            //change done for applicant dashboard redesign.
            if (Visiblity.IsNullOrEmpty() || Visiblity == true)
            {
                if (ControlUseType == AppConsts.DASHBOARD)
                {
                    dvGoToComnCntr.Style.Add("display", "block");
                    Dictionary<String, String> queryString;
                    queryString = new Dictionary<String, String> { { AppConsts.CHILD, @"~\Messaging\MessageQueue.ascx" } };
                    String navigationUrl = String.Format("/Messaging/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                    lnkViewAllRecentMessages.OnClientClick = "NavigationToCommunicatnCenter(" + "'" + navigationUrl + "');return false;";
                }
                else
                {
                    dvGoToComnCntr.Style.Add("display", "none");
                }
            }
           // grdUserCommunicationGrid.Focus();
        }

        #endregion

        #region Grid Events

        protected void grdUserCommunicationGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //change done for applicant dashboard redesign.
            if (Visiblity.IsNullOrEmpty() || Visiblity == true)
            {
                Presenter.GetUserMessages();
                grdUserCommunicationGrid.DataSource = lstUserMessageDetailList;
            }
        }


        protected void grdUserCommunicationGrid_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            DefaultSortExpression = e.SortExpression;
            if (e.NewSortOrder.Equals(GridSortOrder.None))
                IsSortDirectionDescending = true;
            else
                IsSortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
        }


        #endregion
        #endregion


        #region UAT-3261: Badge Form Enhancements
        protected void rdlCommunicationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.Compare(rdlCommunicationMode.SelectedValue, "1") == 0)
            {
                divCommMessageGrid.Style.Add("display", "block");
                divCommEmailGrid.Style.Add("display", "none");
            }
            else if (string.Compare(rdlCommunicationMode.SelectedValue, "2") == 0)
            {
                divCommMessageGrid.Style.Add("display", "none");
                divCommEmailGrid.Style.Add("display", "block");
                grdUserEmailGrid.Rebind();
            }
        }
        protected void grdUserEmailGrid_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            DefaultEmailSortExpression = e.SortExpression;
            if (e.NewSortOrder.Equals(GridSortOrder.None))
                IsSortDirectionDescending = true;
            else
                IsSortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
        }

        protected void grdUserEmailGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (string.Compare(rdlCommunicationMode.SelectedValue, "2") == 0)
            {
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    Presenter.GetUserEmails();
                    grdUserEmailGrid.DataSource = lstUserEmailDetailList;
                }
            }
            else
            {
                grdUserEmailGrid.DataSource = new List<EmailDetails>();
            }
        }
        protected void grdUserEmailGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Resend")
            {
                SystemCommunicationDeliveryIds = new List<int>();
                CurrentViewContext.SystemCommunicationId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[Convert.ToInt32(e.CommandArgument)]["SystemCommunicationId"]);
                if (!Presenter.IsFileMissing())
                {                    
                    Presenter.QueueReSendingEmails();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + Resources.Language.EMAILRSNTSUC + "','sucs');", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + Resources.Language.CNTRESNTEMAIL + "','info');", true);
                }
            }
        }
        #endregion   
    }
}