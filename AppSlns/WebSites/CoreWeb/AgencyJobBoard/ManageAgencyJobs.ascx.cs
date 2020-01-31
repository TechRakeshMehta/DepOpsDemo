using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.AgencyJobBoard.Views
{
    public partial class ManageAgencyJobs : BaseUserControl, IManageAgencyJobsView
    {
        #region Variables

        #region Private Variables

        private ManageAgencyJobsPresenter _presenter = new ManageAgencyJobsPresenter();
        private Boolean _isCorruptedFileUploaded = false;

        #endregion

        #endregion

        #region Properties

        #region Public

        public ManageAgencyJobsPresenter Presenter
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

        public IManageAgencyJobsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<AgencyJobContract> LstAgencyJobTemplate
        {
            get
            {
                if (!ViewState["AgencyJobContract"].IsNull())
                {
                    return (ViewState["AgencyJobContract"]) as List<AgencyJobContract>;
                }
                return new List<AgencyJobContract>();
            }
            set
            {
                ViewState["AgencyJobContract"] = value;
            }
        }
        //UAT-3071
        public List<DefinedRequirementContract> LstJobFieldType
        {
            get
            {
                if (!ViewState["AgencyJobContract"].IsNull())
                {
                    return (ViewState["AgencyJobContract"]) as List<DefinedRequirementContract>;
                }
                return new List<DefinedRequirementContract>();
            }
            set
            {
                ViewState["AgencyJobContract"] = value;
            }
        }

        public List<AgencyJobContract> LstAgencyJobPosting
        {
            get
            {
                if (!ViewState["AgencyJobContract"].IsNull())
                {
                    return (ViewState["AgencyJobContract"]) as List<AgencyJobContract>;
                }
                return new List<AgencyJobContract>();
            }
            set
            {
                ViewState["AgencyJobContract"] = value;
            }
        }

        public Boolean IsJobTemplate
        {
            get
            {
                if (rblJobBoardType.SelectedValue == "AAAA")
                    return true;
                else
                    return false;
            }
        }

        Int32 IManageAgencyJobsView.OrganisationUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IManageAgencyJobsView.CurrentLoggedInUserId
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"]);
                }
                else
                {
                    return SysXWebSiteUtils.SessionService.OrganizationUserId;
                }
            }
        }

        Int32 IManageAgencyJobsView.MappedAgencyHierarchyRootNodeId
        {
            get
            {
                if (!ViewState["MappedAgencyHierarchyRootNodeId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["MappedAgencyHierarchyRootNodeId"]);
                }
                return 0;
            }
            set
            {
                ViewState["MappedAgencyHierarchyRootNodeId"] = value;
            }
        }

        public AgencyJobContract AgencyJob
        {
            get;
            set;
        }

        String IManageAgencyJobsView.FilePath { get; set; }

        String IManageAgencyJobsView.OriginalFileName { get; set; }

        AgencyLogoContract IManageAgencyJobsView.AgencyLogo
        {
            get
            {
                if (!ViewState["AgencyLogo"].IsNull())
                {
                    return ViewState["AgencyLogo"] as AgencyLogoContract;
                }
                return new AgencyLogoContract();
            }
            set
            {
                ViewState["AgencyLogo"] = value;
            }
        }

        List<Int32> IManageAgencyJobsView.SelectedAgencyPostIDs
        {
            get
            {
                if (ViewState["SelectedAgencyPostIDs"] != null)
                {
                    return (List<Int32>)ViewState["SelectedAgencyPostIDs"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["SelectedAgencyPostIDs"] = value;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdManageAgencyJobs.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdManageAgencyJobs.WclGridDataObject)).ColumnsToSkipEncoding.Add("JobDescription");
                base.Title = "Manage Agency Jobs";
                base.SetPageTitle("Manage Agency Jobs");
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
                }
                if (CurrentViewContext.IsJobTemplate)
                    grdManageAgencyJobs.MasterTableView.CommandItemSettings.AddNewRecordText = "Add New Job Template";
                Presenter.OnViewLoaded();
                ShowAgencyLogo();
                HideShowControls();
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

        protected void grdManageAgencyJobs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.IsJobTemplate)
                {
                    Presenter.GetAgencyJobTemplate();
                    grdManageAgencyJobs.DataSource = CurrentViewContext.LstAgencyJobTemplate;
                }
                else
                {
                    Presenter.GetAgencyJobPosting();
                    grdManageAgencyJobs.DataSource = CurrentViewContext.LstAgencyJobPosting;
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

        protected void grdManageAgencyJobs_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.AgencyJob = new AgencyJobContract();
                CurrentViewContext.AgencyJob.AgencyJobID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("AgencyJobID"));

                if (CurrentViewContext.IsJobTemplate)
                {
                    if (Presenter.DeleteAgencyJobTemplate())
                    {
                        base.ShowSuccessMessage("Agency job template deleted successfully");
                    }
                }
                else
                {
                    if (Presenter.DeleteAgencyJobPosting())
                    {
                        base.ShowSuccessMessage("Agency job posting deleted successfully");
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

        protected void grdManageAgencyJobs_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
                {
                    AgencyJobContract currentItem = e.Item.DataItem as AgencyJobContract;
                    //Changes by Chandan Hasija
                    WclComboBox ddlFieldType = e.Item.FindControl("ddlFieldType") as WclComboBox;
                    //--------------------
                    RadioButtonList rblJobType = e.Item.FindControl("rblJobType") as RadioButtonList;
                    WclComboBox drpdwnTemplate = e.Item.FindControl("drpdwnTemplate") as WclComboBox;
                    HtmlGenericControl dvTemplate = e.Item.FindControl("dvTemplate") as HtmlGenericControl;
                    HtmlGenericControl dvTemplateContainer = e.Item.FindControl("dvTemplateContainer") as HtmlGenericControl;
                    HtmlGenericControl dvTemplateText = e.Item.FindControl("dvTemplateText") as HtmlGenericControl;
                    RequiredFieldValidator rfvTemplateName = e.Item.FindControl("rfvTemplateName") as RequiredFieldValidator;
                    if (CurrentViewContext.IsJobTemplate)
                    {
                        dvTemplateText.Visible = true;
                        dvTemplate.Visible = false;                       
                    }
                    else
                    {
                        dvTemplateText.Visible = false;
                        rfvTemplateName.Enabled = false;
                        dvTemplate.Visible = true;
                        Presenter.GetAgencyJobTemplate();
                        drpdwnTemplate.DataSource = LstAgencyJobTemplate;
                        drpdwnTemplate.DataBind();
                    }
                    //UAT-3071
                    Presenter.GetJobFieldType();
                    ddlFieldType.DataSource = CurrentViewContext.LstJobFieldType;
                    ddlFieldType.DataBind();
                    ddlFieldType.Items.Insert(0, new RadComboBoxItem { Text = "--SELECT--", Value = "0" });
                    ddlFieldType.SelectedIndex = currentItem.IsNullOrEmpty()?0:currentItem.FieldTypeID;// == null ? 0 : currentItem.FieldTypeID;
                    //Edit Mode
                    if (!currentItem.IsNullOrEmpty())
                    {
                        rblJobType.SelectedValue = currentItem.AgencyJobTypeCode;
                        if (!CurrentViewContext.IsJobTemplate)
                        {
                            dvTemplate.Visible = false;
                            drpdwnTemplate.Enabled = false;
                            dvTemplateContainer.Visible = false;
                        }
                    }
                }
                else
                {
                    if (CurrentViewContext.IsJobTemplate)
                    {
                        (grdManageAgencyJobs.MasterTableView.GetColumn("Status") as GridBoundColumn).Visible = false;
                        (grdManageAgencyJobs.MasterTableView.GetColumn("TemplateName") as GridBoundColumn).Visible = true;
                        (grdManageAgencyJobs.MasterTableView.GetColumn("TemplateName") as GridBoundColumn).Display = true;
                        (grdManageAgencyJobs.MasterTableView.GetColumn("PublishDate") as GridBoundColumn).Visible = false;

                    }
                    else
                    {
                        (grdManageAgencyJobs.MasterTableView.GetColumn("TemplateName") as GridBoundColumn).Visible = false;
                        (grdManageAgencyJobs.MasterTableView.GetColumn("Status") as GridBoundColumn).Visible = true;
                        (grdManageAgencyJobs.MasterTableView.GetColumn("PublishDate") as GridBoundColumn).Visible = true;
                        (grdManageAgencyJobs.MasterTableView.GetColumn("Status") as GridBoundColumn).Display = true;
                        (grdManageAgencyJobs.MasterTableView.GetColumn("PublishDate") as GridBoundColumn).Display = true;
                    }
                }

                if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
                {
                    var commandBar = e.Item.FindControl("fsucCmdBarAgencyJobs");

                    if (commandBar != null)
                    {
                        (commandBar.FindControl("btnExtra") as INTERSOFT.WEB.UI.WebControls.WclButton).ValidationGroup = "grpValdManageAgencyJobs";
                        if (CurrentViewContext.IsJobTemplate)
                        {
                            (commandBar.FindControl("btnExtra") as INTERSOFT.WEB.UI.WebControls.WclButton).Text = "Save";
                            (commandBar.FindControl("btnSubmit") as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = false;
                        }
                        else
                        {
                            (commandBar.FindControl("btnExtra") as INTERSOFT.WEB.UI.WebControls.WclButton).Text = "Draft";
                            (commandBar.FindControl("btnSubmit") as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = true;
                        }

                        if (!CurrentViewContext.IsJobTemplate && !((e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)))
                        {
                            HiddenField hdnStatusCode = e.Item.FindControl("hdnAgencyJobStatusCode") as HiddenField;
                            String agencyJobStatusCode = hdnStatusCode.Value.Trim();
                            if (string.Compare(agencyJobStatusCode.ToLower(), AgencyJobStatus.Archived.GetStringValue().ToLower()) == 0
                                || string.Compare(agencyJobStatusCode.ToLower(), AgencyJobStatus.DraftAndArchived.GetStringValue().ToLower()) == 0
                                || string.Compare(agencyJobStatusCode.ToLower(), AgencyJobStatus.Published.GetStringValue().ToLower()) == 0)
                            {
                                (commandBar.FindControl("btnSubmit") as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = false;
                                (commandBar.FindControl("btnExtra") as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = false;
                            }
                        }
                    }
                }

                //Marking Check-Box Checked
                if ((e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item)) && !CurrentViewContext.IsJobTemplate)
                {
                    List<Int32> selectedItems = CurrentViewContext.SelectedAgencyPostIDs;
                    String agencyJobStatusCode = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusCode"]);

                    if (string.Compare(agencyJobStatusCode.ToLower(), AgencyJobStatus.Archived.GetStringValue().ToLower()) == 0
                          || string.Compare(agencyJobStatusCode.ToLower(), AgencyJobStatus.DraftAndArchived.GetStringValue().ToLower()) == 0)
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                        checkBox.Enabled = false;
                    }
                    else
                    {
                        if (selectedItems.IsNotNull())
                        {
                            String agencyJobPostID = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyJobID"]);
                            if (!String.IsNullOrEmpty(agencyJobPostID))
                            {
                                Int32 requestId = Convert.ToInt32(agencyJobPostID);
                                if (selectedItems.Exists(cond => cond == requestId))
                                {
                                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                    checkBox.Checked = true;
                                }
                            }
                        }
                    }
                }

                if (e.Item.ItemType.Equals(GridItemType.Footer) && !CurrentViewContext.IsJobTemplate)
                {
                    Int32 checkBoxCount = 0;
                    Int32 rowCount = grdManageAgencyJobs.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdManageAgencyJobs.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                            if (checkBox.IsNotNull() && checkBox.Visible)
                            {
                                checkBoxCount++;
                                if (checkBox.Checked)
                                {
                                    checkCount++;
                                }
                            }
                        }
                        if (checkBoxCount == checkCount && checkBoxCount > 0)
                        {
                            GridHeaderItem item = grdManageAgencyJobs.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                            checkBox.Checked = true;
                        }
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

        protected void grdManageAgencyJobs_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (GridDataItem item in grdManageAgencyJobs.MasterTableView.Items)
                {
                    string descriptionContent = item["JobDescription"].Text;
                    WclEditor editor = new WclEditor();
                    editor.Content = descriptionContent;

                    if (!string.IsNullOrEmpty(editor.Text) && editor.Text.Length > 200)
                    {
                        item["JobDescription"].Text = string.Concat(editor.Text.Substring(0, 200), "...");
                    }
                    else
                    {
                        item["JobDescription"].Text = editor.Text;
                    }

                    item["JobDescription"].ToolTip = editor.Text;
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

        #region [Button Events]

        /// <summary>
        /// Clear button click for logo 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClearLogo_Click(object sender, EventArgs e)
        {
            try
            {
                CloseGridForm();
                Presenter.ClearLogo();
                Presenter.GetAgencyLogo();
                ShowAgencyLogo();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Change button click for logo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(Object sender, EventArgs e)
        {
            try
            {
                CloseGridForm();
                CheckAndSaveAgencyLogo();
                if (_isCorruptedFileUploaded)
                {
                    String corruptedFileMessage = "Your profile picture is not uploaded. Please try again.";
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + corruptedFileMessage + "');", true);
                }
                else if (Presenter.SaveUpdateAgencyLogo())
                {
                    Presenter.GetAgencyLogo();
                    ShowAgencyLogo();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Draft Button Click || Save Button Click (Case Of Template)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarAgencyJobs_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                var container = ((((sender as INTERSOFT.WEB.UI.WebControls.WclButton).Parent).Parent.Parent));
                CreateAgencyJobContract(container);
                SaveUpdateAgencyJob(AgencyJobStatus.Draft.GetStringValue());
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

        /// <summary>
        /// Post Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarAgencyJobs_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                var container = ((((sender as INTERSOFT.WEB.UI.WebControls.WclButton).Parent).Parent.Parent));
                CreateAgencyJobContract(container);
                SaveUpdateAgencyJob(AgencyJobStatus.Published.GetStringValue());
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

        /// <summary>
        /// Cancel Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarAgencyJobs_CancelClick(object sender, EventArgs e)
        {
            try
            {
                CloseGridForm();
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

        protected void fsucCmdBarAgencyJobs_SaveClick(object sender, EventArgs e)
        {
            try
            {
                var pageTitle = String.Empty;
                var container = ((((sender as INTERSOFT.WEB.UI.WebControls.WclButton).Parent).Parent.Parent));

                var commandBar = container.FindControl("fsucCmdBarAgencyJobs");

                (commandBar.FindControl("btnExtra") as INTERSOFT.WEB.UI.WebControls.WclButton).ValidationGroup = "grpValdManageAgencyJobs";
                if (CurrentViewContext.IsJobTemplate)
                {
                    pageTitle = "Preview Job Template";
                    (commandBar.FindControl("btnExtra") as INTERSOFT.WEB.UI.WebControls.WclButton).Text = "Save";
                    (commandBar.FindControl("btnSubmit") as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = false;
                }
                else
                {
                    (commandBar.FindControl("btnExtra") as INTERSOFT.WEB.UI.WebControls.WclButton).Text = "Draft";
                    (commandBar.FindControl("btnSubmit") as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = true;
                }
                if (!CurrentViewContext.IsJobTemplate)
                {
                    pageTitle = "Preview Job Post";
                    HiddenField hdnStatusCode = container.FindControl("hdnAgencyJobStatusCode") as HiddenField;
                    String agencyJobStatusCode = hdnStatusCode.Value.Trim();
                    if (string.Compare(agencyJobStatusCode.ToLower(), AgencyJobStatus.Archived.GetStringValue().ToLower()) == 0
                        || string.Compare(agencyJobStatusCode.ToLower(), AgencyJobStatus.DraftAndArchived.GetStringValue().ToLower()) == 0
                        || string.Compare(agencyJobStatusCode.ToLower(), AgencyJobStatus.Published.GetStringValue().ToLower()) == 0)
                    {
                        (commandBar.FindControl("btnSubmit") as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = false;
                        (commandBar.FindControl("btnExtra") as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = false;
                    }
                }
                var AgencyJob = new AgencyJobContract();
                AgencyJob.JobTitle = (container.FindControl("txtJobTitle") as WclTextBox).Text.Trim();
                AgencyJob.Instructions = (container.FindControl("txtInstructions") as WclTextBox).Text.Trim();
                AgencyJob.Company = (container.FindControl("txtCompany") as WclTextBox).Text.Trim();
                AgencyJob.Location = (container.FindControl("txtLocation") as WclTextBox).Text.Trim();
                AgencyJob.JobDescription = (container.FindControl("txtDescription") as WclEditor).Content.Trim();
                AgencyJob.AgencyHierarchyID = CurrentViewContext.MappedAgencyHierarchyRootNodeId;
                if (Session["PreviewAgencyJobData"].IsNotNull())
                    Session.Remove("PreviewAgencyJobData");

                if (Session["PreviewAgencyJobData"].IsNotNull())
                    Session.Remove("PreviewAgencyJobData");

                Session["PreviewAgencyJobData"] = AgencyJob;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopUp('" + pageTitle + "');", true);
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

        /// <summary>
        ///Archive button click for job posts. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.SelectedAgencyPostIDs.IsNullOrEmpty())
                {
                    if (Presenter.ArchiveJobPosts())
                    {
                        base.ShowSuccessMessage("Agency job post(s) archived successfully.");
                        CurrentViewContext.SelectedAgencyPostIDs = new List<int>();
                        CloseGridForm();
                    }
                }
                else
                {
                    base.ShowInfoMessage("Please select job post(s).");
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

        #region Dropdown Events

        protected void drpdwnTemplate_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Int32 SelectedTemplateID = Convert.ToInt32(e.Value);
                AgencyJobContract agencyJobTemplateDetails = Presenter.GetTemplateDetailsByID(SelectedTemplateID);

                GridEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as GridEditFormInsertItem;
                WclTextBox txtJobTitle = insertItem.FindControl("txtJobTitle") as WclTextBox;
                WclTextBox txtInstructions = insertItem.FindControl("txtInstructions") as WclTextBox;
                WclTextBox txtCompany = insertItem.FindControl("txtCompany") as WclTextBox;
                WclTextBox txtLocation = insertItem.FindControl("txtLocation") as WclTextBox;
                WclEditor txtDescription = insertItem.FindControl("txtDescription") as WclEditor;
                WclTextBox txtHowToApply = insertItem.FindControl("txtHowToApply") as WclTextBox;
                RadioButtonList rblJobType = insertItem.FindControl("rblJobType") as RadioButtonList;
                WclComboBox ddlFieldType = insertItem.FindControl("ddlFieldType") as WclComboBox;

                ddlFieldType.SelectedIndex = agencyJobTemplateDetails.FieldTypeID;
                txtJobTitle.Text = agencyJobTemplateDetails.JobTitle;
                txtCompany.Text = agencyJobTemplateDetails.Company;
                txtDescription.Content = agencyJobTemplateDetails.JobDescription;
                txtHowToApply.Text = agencyJobTemplateDetails.HowToApply;
                txtInstructions.Text = agencyJobTemplateDetails.Instructions;
                txtLocation.Text = agencyJobTemplateDetails.Location;
                rblJobType.SelectedValue = agencyJobTemplateDetails.AgencyJobTypeCode;
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

        protected void chkSelectItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                    return;

                isChecked = checkBox.Checked;

                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Int32 agencyJobPostID = (Int32)dataItem.GetDataKeyValue("AgencyJobID");

                if (CurrentViewContext.SelectedAgencyPostIDs.IsNull())
                    CurrentViewContext.SelectedAgencyPostIDs = new List<Int32>();

                if (isChecked)
                {
                    if (!CurrentViewContext.SelectedAgencyPostIDs.Exists(t => t == agencyJobPostID))
                    {
                        CurrentViewContext.SelectedAgencyPostIDs.Add(agencyJobPostID);
                    }
                }
                else
                {
                    if (CurrentViewContext.SelectedAgencyPostIDs != null && CurrentViewContext.SelectedAgencyPostIDs.Exists(t => t == agencyJobPostID))
                        CurrentViewContext.SelectedAgencyPostIDs.Remove(agencyJobPostID);
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

        protected void rblJobBoardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.IsJobTemplate)
                    grdManageAgencyJobs.MasterTableView.CommandItemSettings.AddNewRecordText = "Add New Job Template";
                else
                    grdManageAgencyJobs.MasterTableView.CommandItemSettings.AddNewRecordText = "Add New Post";

                CloseGridForm();
                CurrentViewContext.SelectedAgencyPostIDs = new List<int>();
                HideShowControls();
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

        #region [Methods]

        private void CheckAndSaveAgencyLogo()
        {
            try
            {
                if (uploadControl.UploadedFiles.Count > 0)
                {
                    String oldAgencyLogoPath = string.Empty;
                    Boolean aWSUseS3 = false;

                    if (!CurrentViewContext.AgencyLogo.IsNullOrEmpty() && CurrentViewContext.AgencyLogo.AgencyLogoID > 0)
                        oldAgencyLogoPath = CurrentViewContext.AgencyLogo.LogoPath;

                    if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                        aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);

                    if (!string.IsNullOrEmpty(oldAgencyLogoPath))
                        DeleteOriginalFile(oldAgencyLogoPath, aWSUseS3);

                    List<String> extensions = new List<String>();
                    extensions.Add(".jpg");
                    extensions.Add(".jpeg");
                    extensions.Add(".tiff");
                    extensions.Add(".bmp");
                    extensions.Add(".bitmap");
                    extensions.Add(".png");

                    String fileExtension = Path.GetExtension(uploadControl.UploadedFiles[0].FileName);
                    if (extensions.Contains(fileExtension.ToLower()))
                    {
                        CurrentViewContext.OriginalFileName = uploadControl.UploadedFiles[0].FileName;
                        String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss");
                        String fileName = "AgencyLogo_" + Guid.NewGuid() + '_' + CurrentViewContext.MappedAgencyHierarchyRootNodeId.ToString() + "_" + date + Path.GetExtension(uploadControl.UploadedFiles[0].FileName);

                        String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                        if (tempFilePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for TemporaryFileLocation in config", null);
                            return;
                        }
                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }
                        //tempFilePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\" + @"Pics\";
                        tempFilePath += @"AgencyLogo\";

                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);

                        tempFilePath = Path.Combine(tempFilePath, fileName);
                        uploadControl.UploadedFiles[0].SaveAs(tempFilePath);
                        CurrentViewContext.FilePath = WebConfigurationManager.AppSettings[AppConsts.SYSTEM_DOCUMENT_LOCATION];
                        //Check whether use AWS S3, true if need to use
                        if (aWSUseS3 == false)
                        {
                            if (CurrentViewContext.FilePath.IsNullOrEmpty())
                            {
                                base.LogError("Please provide path for " + AppConsts.SYSTEM_DOCUMENT_LOCATION + " in config", null);
                                return;
                            }
                            if (!CurrentViewContext.FilePath.EndsWith(@"\"))
                            {
                                CurrentViewContext.FilePath += @"\";
                            }
                            //FilePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\";
                            //FilePath = FilePath + @"Pics\";

                            CurrentViewContext.FilePath += @"AgencyLogo\";

                            if (!Directory.Exists(CurrentViewContext.FilePath))
                                Directory.CreateDirectory(CurrentViewContext.FilePath);

                            CurrentViewContext.FilePath = Path.Combine(CurrentViewContext.FilePath, fileName);
                            MoveFile(tempFilePath, CurrentViewContext.FilePath);
                        }
                        else
                        {
                            if (CurrentViewContext.FilePath.IsNullOrEmpty())
                            {
                                base.LogError("Please provide path for " + AppConsts.SYSTEM_DOCUMENT_LOCATION + " in config", null);
                                return;
                            }
                            if (!CurrentViewContext.FilePath.EndsWith("//"))
                            {
                                CurrentViewContext.FilePath += "//";
                            }
                            //AWS code to save document to S3 location
                            AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                            //String destFolder = FilePath + "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")/" + @"Pics/";
                            String destFolder = CurrentViewContext.FilePath + @"AgencyLogo/";
                            String returnFilePath = objAmazonS3.SaveDocument(tempFilePath, fileName, destFolder);
                            try
                            {
                                if (!String.IsNullOrEmpty(tempFilePath))
                                    File.Delete(tempFilePath);
                            }
                            catch (Exception) { }
                            if (returnFilePath.IsNullOrEmpty())
                            {
                                _isCorruptedFileUploaded = true;
                            }
                            CurrentViewContext.FilePath = returnFilePath; //Path.Combine(destFolder, fileName);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void DeleteOriginalFile(string oldPhotoPath, Boolean aWSUseS3)
        {
            if (aWSUseS3 == false)
            {
                if (System.IO.File.Exists(oldPhotoPath))
                {
                    System.IO.File.Copy(oldPhotoPath, String.Concat(oldPhotoPath.Substring(0, oldPhotoPath.LastIndexOf(".")), "_Deleted", oldPhotoPath.Substring(oldPhotoPath.LastIndexOf("."))), true);
                    System.IO.File.Delete(oldPhotoPath);
                }
            }
            else
            {
                AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                objAmazonS3Documents.DeleteDocument(oldPhotoPath);
            }
        }

        private void MoveFile(String sourceFilePath, String destinationFilePath)
        {
            if (!String.IsNullOrEmpty(sourceFilePath))
            {
                File.Copy(sourceFilePath, destinationFilePath);
            }
            try
            {
                if (!String.IsNullOrEmpty(sourceFilePath))
                    File.Delete(sourceFilePath);
            }
            catch (Exception) { }
        }

        private void ShowAgencyLogo()
        {
            if (!CurrentViewContext.AgencyLogo.IsNullOrEmpty() && CurrentViewContext.AgencyLogo.AgencyLogoID > 0)
            {
                lblNameInitials.Visible = false;
                imgCntrl.Visible = true;
                imgCntrl.ImageUrl = RenderLogo(CurrentViewContext.AgencyLogo.LogoPath);
                btnClearLogo.Visible = true;
            }
            else
            {
                lblNameInitials.Visible = true;
                lblNameInitials.Text = "Agency logo not available";
                imgCntrl.Visible = false;
                btnClearLogo.Visible = false;
            }
            ManageCollapsedStateOfAgencyLogo();
        }

        private string RenderLogo(String documentPath)
        {
            Boolean aWSUseS3 = false;

            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);

            if (!aWSUseS3)
            {
                if (File.Exists(documentPath))
                {
                    String extension = Path.GetExtension(documentPath);
                    FileStream myFileStream = new FileStream(documentPath, FileMode.Open);
                    long FileSize = myFileStream.Length;
                    byte[] buffer = new byte[(int)FileSize];
                    myFileStream.Read(buffer, 0, (int)FileSize);
                    myFileStream.Close();
                    myFileStream.Dispose();
                    string base64 = Convert.ToBase64String(buffer);
                    return string.Format("data:{0};base64,{1}", GetContentType(extension), base64);
                }
            }
            else
            {
                AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                byte[] documentContent = objAmazonS3Documents.RetrieveDocument(documentPath);

                if (!documentContent.IsNullOrEmpty())
                {
                    String extension = Path.GetExtension(documentPath);
                    string base64 = Convert.ToBase64String(documentContent);
                    return string.Format("data:{0};base64,{1}", GetContentType(extension), base64);
                }
            }
            return string.Empty;
        }

        private String GetContentType(String fileExtension)
        {
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".swf":
                    return "application/x-shockwave-flash";
                case ".gif":
                    return "image/gif";
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".txt":
                    return "text/plain";
                case ".xls":
                case ".xlsx":
                case ".csv":
                    return "application/vnd.ms-excel";
                default:
                    return "application/octet-stream";
            }
        }

        private void ManageCollapsedStateOfAgencyLogo()
        {
            if (hdnIsCollapsed.Value == "0")
            {
                dvSection.Attributes["class"] = "section";
                hAgencyLogo.Attributes["class"] = "header-color mhdr";
                dvContent.Attributes["class"] = "content";
                dvContent.Style["display"] = "block";
            }
            else
            {
                dvSection.Attributes["class"] = "section collapsed";
                hAgencyLogo.Attributes["class"] = "header-color mhdr colps";
                dvContent.Attributes["class"] = "content";
                dvContent.Style["display"] = "none";
            }
        }

        private void HideShowControls()
        {
            if (CurrentViewContext.IsJobTemplate)
            {
                grdManageAgencyJobs.MasterTableView.GetColumn("AssignItems").Display = false;
                Archive.Visible = false;
            }
            else
            {
                grdManageAgencyJobs.MasterTableView.GetColumn("AssignItems").Display = true;
                grdManageAgencyJobs.MasterTableView.GetColumn("AssignItems").Visible = true;
                Archive.Visible = true;
            }


        }

        private void CreateAgencyJobContract(dynamic container)
        {
            if (container != null)
            {
                CurrentViewContext.AgencyJob = new AgencyJobContract();
                CurrentViewContext.AgencyJob.JobTitle = (container.FindControl("txtJobTitle") as WclTextBox).Text.Trim();
                CurrentViewContext.AgencyJob.Instructions = (container.FindControl("txtInstructions") as WclTextBox).Text.Trim();
                CurrentViewContext.AgencyJob.Company = (container.FindControl("txtCompany") as WclTextBox).Text.Trim();
                CurrentViewContext.AgencyJob.Location = (container.FindControl("txtLocation") as WclTextBox).Text.Trim();
                CurrentViewContext.AgencyJob.HowToApply = (container.FindControl("txtHowToApply") as WclTextBox).Text.Trim();
                CurrentViewContext.AgencyJob.JobDescription = (container.FindControl("txtDescription") as WclEditor).Content.Trim();
                CurrentViewContext.AgencyJob.AgencyJobTypeCode = Convert.ToString((container.FindControl("rblJobType") as RadioButtonList).SelectedValue);
                CurrentViewContext.AgencyJob.AgencyHierarchyID = CurrentViewContext.MappedAgencyHierarchyRootNodeId;
               
                CurrentViewContext.AgencyJob.FieldTypeID = (container.FindControl("ddlFieldType") as WclComboBox).SelectedIndex;

                Int32 agencyJobID = 0;
                HiddenField hdnAgencyJobID = container.FindControl("hdnAgencyJobID") as HiddenField;
                Int32.TryParse(hdnAgencyJobID.Value.Trim(), out agencyJobID);

                CurrentViewContext.AgencyJob.AgencyJobID = agencyJobID;

                if (CurrentViewContext.IsJobTemplate)
                    CurrentViewContext.AgencyJob.TemplateName = (container.FindControl("txtTemplateName") as WclTextBox).Text.Trim();
            }
        }

        private void SaveUpdateAgencyJob(string agencyJobStatusCode)
        {
            if (!CurrentViewContext.AgencyJob.IsNullOrEmpty())
            {
                if (!CurrentViewContext.IsJobTemplate)
                {
                    CurrentViewContext.AgencyJob.StatusCode = agencyJobStatusCode;
                }

                if (CurrentViewContext.IsJobTemplate)
                {
                    if (Presenter.SaveAgencyJobTemplate())
                    {
                        if (CurrentViewContext.AgencyJob.AgencyJobID > 0)
                        {
                            base.ShowSuccessMessage("Agency job template updated successfully");
                        }
                        else
                        {
                            base.ShowSuccessMessage("Agency job template saved successfully");
                        }
                        CloseGridForm();
                    }
                }
                else
                {
                    if (agencyJobStatusCode == AgencyJobStatus.Published.GetStringValue())
                    {
                        CurrentViewContext.AgencyJob.PublishDate = DateTime.UtcNow;
                    }
                    else
                    {
                        CurrentViewContext.AgencyJob.PublishDate = null;
                    }
                   
                    if (Presenter.SaveAgencyJobPosting())
                    {
                        if (CurrentViewContext.AgencyJob.AgencyJobID > 0)
                        {
                            base.ShowSuccessMessage("Agency job post updated successfully");
                        }
                        else
                        {
                            base.ShowSuccessMessage("Agency job post saved successfully");
                        }
                        CloseGridForm();
                    }
                }
            }
        }

        private void CloseGridForm()
        {
            grdManageAgencyJobs.MasterTableView.IsItemInserted = false;
            grdManageAgencyJobs.MasterTableView.ClearEditItems();
            grdManageAgencyJobs.MasterTableView.Rebind();
        }

        #endregion 

    }
}