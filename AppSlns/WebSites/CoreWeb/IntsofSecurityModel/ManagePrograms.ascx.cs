
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
    public partial class ManagePrograms : BaseUserControl, IManageProgramsView
    {
        #region Private Variables
        private ManageProgramsPresenter _presenter=new ManageProgramsPresenter();
        private ManageProgramsContract _viewContract;
        private String _viewType;
        private IQueryable<lkpGradeLevel> _AllGrades;
        private Int32 OrgID;
        private Int32 _currentUserTenantId;
        #endregion

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Manage Program";

                lblManageProgram.Text = base.Title;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.OrganizationID = Request.QueryString["Id"].IsNull() ? AppConsts.NONE : Convert.ToInt32(Request.QueryString["Id"]);
                CurrentViewContext.SelectedOrganizationId = CurrentViewContext.ViewContract.OrganizationID;
                if (!this.IsPostBack)
                {
                    //CurrentViewContext.TenantId = Presenter.GetTenantId();
                    CurrentViewContext.ViewContract.OrganizationID = Request.QueryString["Id"].IsNull() ? AppConsts.NONE : Convert.ToInt32(Request.QueryString["Id"]);
                    OrgID = CurrentViewContext.ViewContract.OrganizationID;
                    Presenter.OnViewInitialized();
                }

                Presenter.OnViewLoaded();
                //base.SetPageTitle("Programs");
                HideMessages();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region Properties

        #region Presenter
        
        public ManageProgramsPresenter Presenter
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
        Boolean IManageProgramsView.IsAdmin
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
        Int32 IManageProgramsView.CurrentUserId
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
        String IManageProgramsView.SuccessMessage
        {
            get;
            set;
        }

        public Int32 DepProgramMappingId
        { get; set; }

        /// <summary>
        /// TenantID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for tenant's id.
        /// </value>
        Int32 IManageProgramsView.TenantId
        {
            //get
            //{
            //    return Convert.ToInt32(ViewState["TenantID"]);
            //}
            //set
            //{
            //    ViewState["TenantID"] = value;
            //}

             get
            {
                if (_currentUserTenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _currentUserTenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _currentUserTenantId;
            }
            set { _currentUserTenantId = value; }
        }

        /// <summary>
        /// Selected OrganizationId.
        /// </summary>
        /// <value>
        /// Gets or sets the value for Selected OrganizationId.
        /// </value>
        Int32 IManageProgramsView.SelectedOrganizationId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedOrganizationId"]);
            }
            set
            {
                ViewState["SelectedOrganizationId"] = value;
            }
        }


        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageProgramsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        IQueryable<lkpGradeLevel> IManageProgramsView.AllGrades
        {
            get
            {
                return _AllGrades;
            }
            set
            {
                _AllGrades = value;
            }
        }

        ManageProgramsContract IManageProgramsView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageProgramsContract();
                }

                return _viewContract;
            }
        }


        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        /// <remarks></remarks>
        String IManageProgramsView.ErrorMessage
        {
            get;
            set;
        }

        //public IQueryable<DeptProgramMapping> OrganizationPrograms
        //{
        //    get
        //    {
        //        return null;
        //    }
        //    set
        //    {
        //        grdPrograms.DataSource = value;
        //    }
        //}

        public IQueryable<Entity.ClientEntity.DeptProgramMapping> ProgramList
        {
            get;
            //{
            //    return null;
            //}
            set;
            //{
            //    grdPrograms.DataSource = value;
            //}
        }
        /// <summary>
        /// Get or Set Payment Options
        /// </summary>
        public IQueryable<Entity.ClientEntity.lkpPaymentOption> AllPaymentOption
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of selected payment option.
        /// </summary>
        public List<Int32> SelectedPaymentOptionIds
        {
            get;
            set;
        }


        #endregion
        #endregion

        #region Grid Events

        protected void grdPrograms_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingProgramDetails(CurrentViewContext.ViewContract.OrganizationID);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
        }
        protected void grdPrograms_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                WclComboBox cmbGrades = e.Item.FindControl("cmbGrades") as WclComboBox;
               WclNumericTextBox ntxtMgmtFee= e.Item.FindControl("ntxtMgmtFee") as WclNumericTextBox;
                CheckBoxList chkPaymentOption = (CheckBoxList)(e.Item.FindControl("chkPaymentOption"));
                CurrentViewContext.ViewContract.ProgramStudy = (e.Item.FindControl("txtProgramName") as WclTextBox).Text.Trim();
                //CurrentViewContext.ViewContract.RenewalTerm = Convert.ToInt16((e.Item.FindControl("txtRenewalTerm") as WclTextBox).Text.Trim());
                if (!String.IsNullOrEmpty(ntxtMgmtFee.Text))
                    CurrentViewContext.ViewContract.ManagementFee = Convert.ToDecimal((ntxtMgmtFee).Text.Trim());
                else
                    CurrentViewContext.ViewContract.ManagementFee = null;
                CurrentViewContext.ViewContract.DurationMonth = Convert.ToInt32((e.Item.FindControl("txtDuration") as WclTextBox).Text.Trim());
                CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                if (cmbGrades.SelectedValue != String.Empty)
                {
                    CurrentViewContext.ViewContract.GradeLevelID = Convert.ToInt16(cmbGrades.SelectedValue);
                }
                CurrentViewContext.SelectedPaymentOptionIds = chkPaymentOption.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToList();
                Presenter.ProgramSave();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    lblInfoMessage.ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Information);
                    lblInfoMessage.Visible = true;
                    //base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    lblInfoMessage.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
                    lblInfoMessage.Visible = true;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    //base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
        }
        protected void grdPrograms_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

                //CurrentViewContext.ErrorMessage = (e.Item.FindControl("lblErrorMessage") as Label).Text;            
                WclComboBox cmbGrades = e.Item.FindControl("cmbGrades") as WclComboBox;
                CheckBoxList chkPaymentOption = (CheckBoxList)(e.Item.FindControl("chkPaymentOption"));
                WclNumericTextBox ntxtMgmtFee = e.Item.FindControl("ntxtMgmtFee") as WclNumericTextBox;
                CurrentViewContext.ViewContract.ProgramId = Convert.ToInt16((e.Item.FindControl("txtProgramID") as WclTextBox).Text.Trim());
                CurrentViewContext.ViewContract.ProgramStudy = (e.Item.FindControl("txtProgramName") as WclTextBox).Text.Trim();
                //Int16 i = Convert.ToInt16((e.Item.FindControl("txtRenewalTerm") as WclTextBox).Text.Trim());
                //CurrentViewContext.ViewContract.RenewalTerm = i;
                if (!String.IsNullOrEmpty(ntxtMgmtFee.Text))
                {
                    CurrentViewContext.ViewContract.ManagementFee = Convert.ToDecimal(ntxtMgmtFee.Text.Trim());
                }
                else
                {
                    CurrentViewContext.ViewContract.ManagementFee = null;
                }
                CurrentViewContext.ViewContract.DurationMonth = Convert.ToInt32((e.Item.FindControl("txtDuration") as WclTextBox).Text.Trim());
                CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                if (cmbGrades.SelectedValue != String.Empty)
                {
                    CurrentViewContext.ViewContract.GradeLevelID = Convert.ToInt16(cmbGrades.SelectedValue);
                }
                CurrentViewContext.SelectedPaymentOptionIds = chkPaymentOption.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToList();
                CurrentViewContext.DepProgramMappingId = Convert.ToInt16((e.Item.FindControl("txtDepProgramId") as WclTextBox).Text.Trim());
                Presenter.ProgramUpdate();
                ViewState["prefix"] = null;
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    lblInfoMessage.ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Information);
                    lblInfoMessage.Visible = true;
                    //base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    lblInfoMessage.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
                    lblInfoMessage.Visible = true;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    //base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }

        }

        protected void grdPrograms_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
            }
            
                //Hide filter when exportig to pdf or word
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdPrograms);
            }
        }
        protected void grdPrograms_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.DepProgramMappingId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DPM_ID"));
                CurrentViewContext.ViewContract.DeleteFlag = false;
                Presenter.DeleteProgramDetails();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    lblInfoMessage.ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Information);
                    lblInfoMessage.Visible = true;
                    //base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    lblInfoMessage.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
                    lblInfoMessage.Visible = true;
                    //base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
        }
        protected void grdPrograms_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox ddlGradeGroup = (WclComboBox)editform.FindControl("cmbGrades");
                    CheckBoxList chkPaymentOption = (CheckBoxList)editform.FindControl("chkPaymentOption");
                    CurrentViewContext.ViewContract.OrganizationID = CurrentViewContext.SelectedOrganizationId;

                    var listGradeLevel = Presenter.RetrievingGradeDetails().ToList();
                    listGradeLevel.Insert(0, new lkpGradeLevel { GradeLevelID = 0, Description = "--SELECT--" });
                    ddlGradeGroup.DataSource = listGradeLevel;
                    ddlGradeGroup.DataValueField = "GradeLevelID";
                    ddlGradeGroup.DataTextField = "Description";
                    ddlGradeGroup.DataBind();

                    Presenter.GetAllPaymentOption();
                    chkPaymentOption.DataSource = CurrentViewContext.AllPaymentOption;
                    chkPaymentOption.DataBind();
                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        Int32 gradeLevelId = Convert.ToInt32(editform.GetDataKeyValue("AdminProgramStudy.GradeLevelID"));
                        if (gradeLevelId != 0)
                            ddlGradeGroup.SelectedValue = gradeLevelId.ToString();
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdPrograms_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridEditableItem editform = (GridEditableItem)e.Item;
                    CheckBoxList chkPaymentOption = (CheckBoxList)editform.FindControl("chkPaymentOption");
                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        Int32 depProgramMappingId = Convert.ToInt32(editform.GetDataKeyValue("DPM_ID"));
                        List<Int32> tempIds = Presenter.MappedPaymentOptionIds(depProgramMappingId);

                        if (tempIds.Count > AppConsts.NONE)
                        {
                            foreach (Int32 id in tempIds)
                            {
                                chkPaymentOption.Items.FindByValue(id.ToString()).Selected = true;
                            }
                        }
                    }

                }
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Int32 depProgramMappingId = Convert.ToInt32(dataItem.GetDataKeyValue("DPM_ID"));
                    if (Presenter.IsProgramMapped(depProgramMappingId))
                    {
                        ImageButton deleteColumn = dataItem["DeleteColumn"].Controls[0] as ImageButton;
                        deleteColumn.Visible = false;
                    }

                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblInfoMessage.ShowMessage(ex.Message, MessageType.Error);
                lblInfoMessage.Visible = true;
                //base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Methods

        #region public Method
        #endregion

        #region Private Methods
        /// <summary>
        /// Method to hide Message label.
        /// </summary>
        private void HideMessages()
        {
            lblSuccess.Visible = false;
            lblSuccess.Text = String.Empty;
            lblInfoMessage.Visible = false;
            lblInfoMessage.Text = String.Empty;
        }
        #endregion

        #endregion
    }
}

