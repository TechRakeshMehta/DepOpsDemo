
#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

#region UserDefined
using INTSOF.Utils;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

#endregion

#endregion


namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class ManageAgencyNode : BaseUserControl, IManageAgencyNodeView
    {
        #region Private Variables
        private ManageAgencyNodePresenter _presenter = new ManageAgencyNodePresenter();
        #endregion

        #region PublicProperties

        public IManageAgencyNodeView CurrentViewContext
        {
            get
            {
                return this;
            }

        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["ClientTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ClientTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ClientTenantID"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public List<AgencyNodeContract> lstGetNodeList
        {
            get;
            set;
        }

        String IManageAgencyNodeView.AgencyNodeName
        {
            get
            {
                if (!txtAgencyNodeName.Text.IsNullOrEmpty())
                {
                    return txtAgencyNodeName.Text;
                }
                return String.Empty;
            }
        }
        String IManageAgencyNodeView.Description
        {
            get
            {
                if (!txtDescription.Text.IsNullOrEmpty())
                {
                    return txtDescription.Text;
                }
                return String.Empty;
            }
        }

        public String SuccessMessage { get; set; }

        public String ErrorMessage { get; set; }

        public String InfoMessage { get; set; }

        public AgencyNodeContract NodeContract
        {
            get;
            set;
        }


        #endregion

        #region Custom paging parameters UAT-3652
        public Int32 CurrentPageIndex
        {
            get
            {
                return grAgencyNode.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grAgencyNode.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public int PageSize
        {
            get
            {
                return grAgencyNode.MasterTableView.PageSize;
            }
            set
            {
                grAgencyNode.MasterTableView.PageSize = value;
                grAgencyNode.PageSize = value;
            }
        }

        public int VirtualRecordCount
        {
            get
            {
                if (ViewState["ItemCount"] != null)
                    return Convert.ToInt32(ViewState["ItemCount"]);
                else
                    return 0;
            }
            set
            {
                ViewState["ItemCount"] = value;
                grAgencyNode.VirtualItemCount = value;
                grAgencyNode.MasterTableView.VirtualItemCount = value;
            }
        }

        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["GridCustomPaging"] == null)
                {
                    ViewState["GridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["GridCustomPaging"];
            }
            set
            {
                ViewState["GridCustomPaging"] = value;
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }


        #endregion


        #region Events

        #region Page Events

        /// <summary>
        /// Set the page title on bread crum. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = AppConsts.TITLE_MANAGE_AGENCY_NODE;
                base.SetPageTitle(AppConsts.TITLE_MANAGE_AGENCY_NODE);
                base.OnInit(e);
            }

            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    if (Presenter.IsAdminLoggedIn())
                    {
                        Presenter.OnViewLoaded();
                    }
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }

            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Grid Events

        protected void grAgencyNode_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                Presenter.GetNodeList();
                grAgencyNode.DataSource = CurrentViewContext.lstGetNodeList;
            }

            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }


        }

        protected void grAgencyNode_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.NodeContract = new AgencyNodeContract();
                CurrentViewContext.NodeContract.NodeName = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                //  CurrentViewContext.NodeContract.NodeLabel = (e.Item.FindControl("txtLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.NodeContract.NodeDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.NodeContract.CurrentLoggedInUser = CurrentViewContext.CurrentUserId;
                Presenter.SaveNodeDetail();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }

            }

            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grAgencyNode_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.NodeContract = new AgencyNodeContract();
                CurrentViewContext.NodeContract.NodeName = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                //CurrentViewContext.NodeContract.NodeLabel = (e.Item.FindControl("txtLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.NodeContract.NodeDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.NodeContract.NodeId = Convert.ToInt32((e.Item.FindControl("txtNodeId") as WclTextBox).Text);
                CurrentViewContext.NodeContract.CurrentLoggedInUser = CurrentViewContext.CurrentUserId;
                Presenter.UpdateNodeDetail();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);

                }
                if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }

            }

            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grAgencyNode_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {

                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.NodeContract = new AgencyNodeContract();
                CurrentViewContext.NodeContract.NodeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("NodeId"));
                CurrentViewContext.NodeContract.IsDeleted = true;
                CurrentViewContext.NodeContract.CurrentLoggedInUser = CurrentViewContext.CurrentUserId;
                Presenter.DeleteNode();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }

            }

            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #region UAT-3652
        protected void grAgencyNode_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #endregion


        #region Button Events

        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            try
            {
                grAgencyNode.MasterTableView.SortExpressions.Clear();
                grAgencyNode.CurrentPageIndex = 0;
                grAgencyNode.MasterTableView.CurrentPageIndex = 0;
                grAgencyNode.MasterTableView.IsItemInserted = false;
                grAgencyNode.MasterTableView.ClearEditItems();
                grAgencyNode.Rebind();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {

            try
            {
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                txtAgencyNodeName.Text = String.Empty;
                txtDescription.Text = String.Empty;
                grAgencyNode.MasterTableView.SortExpressions.Clear();
                grAgencyNode.CurrentPageIndex = 0;
                grAgencyNode.MasterTableView.CurrentPageIndex = 0;
                grAgencyNode.MasterTableView.IsItemInserted = false;
                grAgencyNode.MasterTableView.ClearEditItems();
                grAgencyNode.Rebind();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion
        #endregion

        #region Properties

        #region Presenter

        public ManageAgencyNodePresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }

            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        #endregion

        #endregion

    }
}