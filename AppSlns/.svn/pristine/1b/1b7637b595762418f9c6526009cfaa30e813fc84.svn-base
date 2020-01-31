#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Web.UI.WebControls;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.SysXSecurityModel;

#endregion

#endregion



namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class ManageGrade : BaseUserControl, IManageGradeView
    {
        #region Private Variables
        private ManageGradePresenter _presenter=new ManageGradePresenter();
        private ManageGradeContract _viewContract;
        //private String _viewType;
        #endregion

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            //try
            //{
            //    base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
            //    //_viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            //    base.OnInit(e);
            //    base.Title = "Manage Grade";
            //}
            //catch (SysXException ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
            //catch (System.Exception ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //try
            //{
            //    Dictionary<String, String> args = new Dictionary<String, String>();
            //    if (!this.IsPostBack)
            //    {

            //        if (!Request.QueryString["args"].IsNull())
            //        {
            //            args.ToDecryptedQueryString(Request.QueryString["args"]);
            //            CurrentViewContext.OrganizationId = args.ContainsKey("OrganizationId") ? Int32.Parse(args["OrganizationId"]) : AppConsts.NONE;
            //            CurrentViewContext.TenantId = args.ContainsKey("TenantId") ? Int32.Parse(args["TenantId"]) : AppConsts.NONE;
            //        }
            //        Presenter.OnViewInitialized();

            //    }

            //    Presenter.OnViewLoaded();
            //    //base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_PROGRAMS));
            //    lblSuccess.Visible = false;
            //    lblSuccess.Text = String.Empty;
            //}
            //catch (SysXException ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
            //catch (System.Exception ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}

        }

        #endregion

        #region Properties

        #region Presenter
        
        public ManageGradePresenter Presenter
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
        #endregion

        #region View Properties
       
        /// <summary>
        /// IsAdmin.
        /// </summary>
        /// <value>
        /// Gets or sets the value for user is admin or not?
        /// </value>
        Boolean IManageGradeView.IsAdmin
        {
            get
            {
                return base.IsSysXAdmin;
            }
        }

        /// <summary>
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for current user's id.
        /// </value>
        Int32 IManageGradeView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IManageGradeView.SuccessMessage
        {
            get;
            set;
        }


        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageGradeView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        ManageGradeContract IManageGradeView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageGradeContract();
                }

                return _viewContract;
            }
        }

        List<lkpGradeLevelGroup> IManageGradeView.AllGradeGroups
        {
            set;
            get;
        }

        /// <summary>
        /// OrganizationId.
        /// </summary>
        /// <value>
        /// Gets or sets the value for OrganizationId
        /// </value>
        public Int32 OrganizationId
        {
            get
            {
                return Convert.ToInt32(ViewState["OrganizationId"]);
            }
            set
            {
                ViewState["OrganizationId"] = value;
            }
        }

        public Int32 TenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantID"]);
            }
            set
            {
                ViewState["TenantID"] = value;
            }
        }

        /// <summary>
        /// Parent OrganizationId.
        /// </summary>
        /// <value>
        /// Gets or sets the value for Parent OrganizationId
        /// </value>
        public Int32 ParentOrganizationId
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentOrganizationId"]);
            }
            set
            {
                ViewState["ParentOrganizationId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        /// <remarks></remarks>
        String IManageGradeView.ErrorMessage
        {
            get;
            set;
        }

        //Int16 IManageGradeView.GroupID
        //{
        //    get
        //    {
        //        return 0;
        //    }
        //    set
        //    {

        //    }
        //}


        IQueryable<lkpGradeLevel> IManageGradeView.AllGrades
        {
            get
            {
                return null;
            }
            set
            {
                grdGrades.DataSource = value;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Grid Events
        protected void grdGrades_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //try
            //{
            //    Presenter.RetrievingGradeDetails();
            //}
            //catch (SysXException ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
            //catch (System.Exception ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
        }
        protected void grdGrades_InsertCommand(object sender, GridCommandEventArgs e)
        {
            //try
            //{
            //    CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtGradeName") as WclTextBox).Text.Trim();
            //    CurrentViewContext.ViewContract.GredeLevelGroupDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
            //    //CurrentViewContext.ViewContract.GradeLevelGroupID = Convert.ToInt16((e.Item.FindControl("cmbGradeGroup") as WclComboBox).SelectedValue.Trim());
            //    //CurrentViewContext.ViewContract.SEQ = Convert.ToInt32((e.Item.FindControl("txtGrpDispOdr") as WclNumericTextBox).Text.Trim());

            //    Presenter.GradeSave();
            //    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
            //    {
            //        e.Canceled = true;
            //        //base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
            //        base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
            //    }
            //    else
            //    {
            //        e.Canceled = false;
            //        base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
            //    }
                
            //}
            //catch (SysXException ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
            //catch (System.Exception ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}

        }
        protected void grdGrades_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            //try
            //{
            //    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            //    CurrentViewContext.ViewContract.Id = Convert.ToInt16(gridEditableItem.GetDataKeyValue("GradeLevelID"));
            //    CurrentViewContext.ViewContract.DeleteFlag = false;
            //    Presenter.DeleteGradeDetails();
            //    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
            //    //lblSuccess.Visible = true;
            //    //lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
            //}
            //catch (SysXException ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
            //catch (System.Exception ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
        }
        protected void grdGrades_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            //try
            //{
            //    CurrentViewContext.ViewContract.Id = Convert.ToInt16((e.Item.FindControl("txtGradeID") as WclTextBox).Text.Trim());
            //    CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtGradeName") as WclTextBox).Text.Trim();

            //    CurrentViewContext.ViewContract.GredeLevelGroupDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
            //    //CurrentViewContext.ViewContract.GradeLevelGroupID = Convert.ToInt16((e.Item.FindControl("cmbGradeGroup") as WclComboBox).SelectedValue.Trim());
            //    //CurrentViewContext.ViewContract.SEQ = Convert.ToInt32((e.Item.FindControl("txtGrpDispOdr") as WclNumericTextBox).Text.Trim());

            //    Presenter.GradeUpdate();
            //    ViewState["prefix"] = null;
            //    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
            //    {
            //        e.Canceled = true;
            //        //base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
            //        base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
            //    }
            //    else
            //    {
            //        e.Canceled = false;
            //        base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
            //    }
               

                //lblSuccess.Visible = true;
                //lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
            //}
            //catch (SysXException ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
            //catch (System.Exception ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
        }
        protected void grdGrades_ItemCreated(object sender, GridItemEventArgs e)
        {
            //try
            //{
                //if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                //{
                //    GridEditFormItem editform = (GridEditFormItem)e.Item;
                //    WclComboBox ddlGradeGroup = (WclComboBox)editform.FindControl("cmbGradeGroup");

                //    ddlGradeGroup.DataSource = this._presenter.RetrievingGradeGroups();
                //    ddlGradeGroup.DataValueField = "GradeLevelGroupID";
                //    ddlGradeGroup.DataTextField = "Description";
                //    ddlGradeGroup.DataBind();

                //    if(ddlGradeGroup.Items.Count > 0)
                //        ddlGradeGroup.SelectedValue = Convert.ToString(CurrentViewContext.ViewContract.GradeLevelGroupID);
                //}
            //}
            //catch (SysXException ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
            //catch (System.Exception ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}


        }

        protected void grdGrades_ItemCommand(object sender, GridCommandEventArgs e)
        {
            ////Setting Current Context
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = (GridDataItem)e.Item;

            //    //CurrentViewContext.ViewContract.Description = item["Description"].Text.Trim();
            //    //CurrentViewContext.ViewContract.GredeLevelGroupDescription = item["GradeLevelGroupDescription"].Text.Trim();
            //    //CurrentViewContext.ViewContract.GradeLevelGroupID = Convert.ToInt16(item["GradeLevelGroupID"].Text.Trim());
            //    //CurrentViewContext.ViewContract.SEQ = Convert.ToInt16(item["SEQ"].Text.Trim());
            //}
            ////Hide filter when exportig to pdf or word
            //if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
            //    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            //{
            //    base.ConfigureExport(grdGrades);

            //}

        }


        protected void grdGrades_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //try
            //{
            //    if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            //    {
            //        lkpGradeLevel gradeLevel = (lkpGradeLevel)e.Item.DataItem;
            //        if (e.Item is GridDataItem)
            //        {
            //            GridDataItem dataItem = e.Item as GridDataItem;
            //            if (Presenter.IsGradeLinked(gradeLevel.GradeLevelID))
            //            {
            //                ImageButton deleteColumn = dataItem["DeleteColumn"].Controls[0] as ImageButton;
            //                deleteColumn.Visible = false;
            //            }

            //        }
            //    }
            
            //}
            //catch (SysXException ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
            //catch (System.Exception ex)
            //{
            //    base.LogError(ex);
            //    base.ShowErrorMessage(ex.Message);
            //}
        }

        #endregion

        #endregion
    }
}

