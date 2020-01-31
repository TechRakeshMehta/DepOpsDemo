#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using System.Web.UI.WebControls;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class NodeNotificationSettings : BaseUserControl, INodeNotificationSettingsView
    {
        public delegate void NotifyStatusDelegate(String message, Boolean isSuccess);
        public event NotifyStatusDelegate NotifyStatusChange;


        #region Variables

        #region Private Variables

        private NodeNotificationSettingsPresenter _presenter = new NodeNotificationSettingsPresenter();
        private String _viewType = String.Empty;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public NodeNotificationSettingsPresenter Presenter
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

        public INodeNotificationSettingsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public List<UserGroup> lstUserGroups
        {
            get;
            set;
        }

        public List<NodeDeadline> lstNodeDeadlines
        {
            get;
            set;
        }
        public List<Entity.lkpCommunicationSubEvent> lstSubEvent
        {
            get;
            set;
        }

        public List<Entity.ExternalCopyUser> lstExternalUserBCC
        {
            get;
            set;
        }
        // public List<Int32> checkedGroups { get; set; }

        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["SelectedTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        }

        public Int32 HierarchyNodeID
        {
            get
            {
                if (ViewState["HierarchyNodeID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["HierarchyNodeID"]);
                }
                return 0;
            }
            set
            {
                ViewState["HierarchyNodeID"] = value;
            }
        }

        public Int32 ParentID
        {
            get
            {
                if (ViewState["ParentID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ParentID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ParentID"] = value;
            }
        }

        public String PermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["PermissionCode"]);
            }
            set
            {
                ViewState["PermissionCode"] = value;
            }
        }

        public String NodeLabel
        {
            get
            {
                if (ViewState["NodeLabel"].IsNotNull())
                {
                    return Convert.ToString(ViewState["NodeLabel"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["NodeLabel"] = value;
                lblNodeTitle.Text = value.ToString();
            }
        }

        public Int32? NagFrequency
        {
            get
            {
                if (!txtNagFrequency.Text.Trim().IsNullOrEmpty())
                {
                    return Convert.ToInt32(txtNagFrequency.Text.Trim());
                }
                return 0;
            }
            set
            {
                txtNagFrequency.Text = value.ToString();
            }
        }

        /// <summary>
        /// UAT-1743: As an admin, I should be able to turn off nag email notifications once setting a frequency
        /// </summary>
        public Boolean IsActive
        {
            get
            {
                return Convert.ToBoolean(chkActive.SelectedValue.Trim());
            }
            set
            {
                chkActive.SelectedValue = Convert.ToString(value).ToLower();
            }
        }


        public Int32? SavedNagFrequency
        {
            get
            {
                if (!hdnSavedNagFrequency.Value.Trim().IsNullOrEmpty())
                {
                    return Convert.ToInt32(hdnSavedNagFrequency.Value.Trim());
                }
                return 0;
            }
            set
            {
                hdnSavedNagFrequency.Value = value.ToString();
            }
        }

        public List<HierarchyContactMapping> lstHierarchyContactMapping
        {
            get;
            set;
        }

        public List<InsContact> contactStatus
        {
            get;
            set;
        }

        public InstitutionContact institutionContactById
        {
            get;
            set;
        }

        public List<Entity.HierarchyNotificationMapping> lstNotifications
        {
            get;
            set;
        }

        public List<Entity.lkpCopyType> copyType
        {
            get;
            set;
        }

        public List<Entity.lkpCommunicationSubEvent> subEvent
        {
            get;
            set;
        }



        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ApplyActionLevelPermission(ActionCollection, "Node Notification Settings");
                BusinessRuleImplementations();
                Presenter.GetNodeNotificationMappingByNodeID();
                if (CurrentViewContext.SavedNagFrequency.IsNotNull() && CurrentViewContext.SavedNagFrequency > 0)
                {
                    fsucCmdBarNagEmailNotify.SubmitButton.Enabled = true;
                }
                else
                {
                    fsucCmdBarNagEmailNotify.SubmitButton.Enabled = false;
                }
            }
            lblContactMessage.Visible = false;
        }

        #endregion

        #region Grid Related Events

        /// <summary>
        /// Set data source to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNodeDeadline_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetNodeDeadlines();
                grdNodeDeadline.DataSource = CurrentViewContext.lstNodeDeadlines;
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
        /// Grid ItemDataBound event to bind controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNodeDeadline_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                WclComboBox ddlUserGroup = gridEditableItem.FindControl("ddlUserGroup") as WclComboBox;
                List<Int32> mappingIds = new List<Int32>();
                if (!(e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem))
                {
                    var nodeDeadlineId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ND_NodeNotificationMappingId"));
                    mappingIds = Presenter.GetCheckedUsergroupIds(nodeDeadlineId);
                }
                Presenter.GetAllUserGroups();
                ddlUserGroup.DataSource = CurrentViewContext.lstUserGroups;
                ddlUserGroup.DataBind();
                ddlUserGroup.Items.Where(condition => mappingIds.Contains(Convert.ToInt32(condition.Value)))
                    .ForEach(condition => condition.Checked = true);
            }
        }

        /// <summary>
        /// Redirect the user to the detail page.
        /// Sets the filetrs when filtering is applied.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNodeDeadline_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region Detail screen navigation

                if (e.CommandName.Equals("ViewDetail"))
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String _ndId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ND_ID"].ToString();
                    String _ndNodeNotificationMappingId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ND_NodeNotificationMappingId"].ToString();
                    GridDataItem item = (GridDataItem)e.Item;
                    String frequency = item["ND_Frequency"].Text;
                    String daysBeforeDeadline = item["ND_DaysBeforeDeadline"].Text;
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(CurrentViewContext.SelectedTenantID) },
                                                                    { "Frequency", frequency },
                                                                    { "NoOfDays", daysBeforeDeadline },
                                                                    { "SubEventCode", CommunicationSubEvents.NOTIFICATION_FOR_DEADLINE.GetStringValue() },
                                                                    { "NodeNotificationTypeCode", lkpNodeNotificationTypesContext.DEADLINE.GetStringValue() },
                                                                    { "HierarchyNodeID", Convert.ToString(CurrentViewContext.HierarchyNodeID) },
                                                                    { "NodeNotificationMappingId", _ndNodeNotificationMappingId },
                                                                    { "TemplateNodeLevel", TemplateNodeLevels.CURRENTLEVEL.GetStringValue() },
                                                                    { "PermissionCode", CurrentViewContext.PermissionCode },
                                                                    { "ParentID", Convert.ToString(CurrentViewContext.ParentID) }
                                                                 };
                    Response.Redirect("~/Templates/Pages/NodeNotifications.aspx?args=" + queryString.ToEncryptedQueryString());
                }

                #endregion
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
        /// Grid Insert command to insert data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNodeDeadline_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                NodeNotificationSettingsContract nodeNotificationSettingsContract = new NodeNotificationSettingsContract();
                nodeNotificationSettingsContract.NodeDeadlineName = (e.Item.FindControl("txtNodeDeadlineName") as WclTextBox).Text.Trim();
                nodeNotificationSettingsContract.NodeDeadlineDescription = (e.Item.FindControl("txtNodeDeadlineDescription") as WclTextBox).Text.Trim();
                nodeNotificationSettingsContract.DeadlineDate = (e.Item.FindControl("dpkrDeadlineDate") as WclDatePicker).SelectedDate;
                var frequency = (e.Item.FindControl("txtFrequency") as WclNumericTextBox).Text.Trim();
                var daysBeforeDeadline = (e.Item.FindControl("txtDaysBeforeDeadline") as WclNumericTextBox).Text.Trim();
                var ddlUserGroup = (e.Item.FindControl("ddlUserGroup") as WclComboBox);

                if (!frequency.IsNullOrEmpty())
                {
                    nodeNotificationSettingsContract.Frequency = Convert.ToInt32(frequency);
                }
                if (!daysBeforeDeadline.IsNullOrEmpty())
                {
                    nodeNotificationSettingsContract.DaysBeforeDeadline = Convert.ToInt32(daysBeforeDeadline);
                }
                List<Int32> _lstUserGroupChecked = new List<Int32>();
                foreach (RadComboBoxItem item in ddlUserGroup.Items)
                {
                    if (item.Checked)
                        _lstUserGroupChecked.Add(Convert.ToInt32(item.Value));
                }
                if (Presenter.SaveNodeDeadline(nodeNotificationSettingsContract, _lstUserGroupChecked))
                {
                    e.Canceled = false;
                    if (NotifyStatusChange.IsNotNull())
                        NotifyStatusChange("Node Deadline is saved successfully.", true);
                }
                else
                {
                    e.Canceled = true;
                    //base.ShowInfoMessage("An error occured while saving Node Deadline. Please try again.");
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                if (NotifyStatusChange.IsNotNull())
                    NotifyStatusChange(ex.Message, false);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                if (NotifyStatusChange.IsNotNull())
                    NotifyStatusChange(ex.Message, false);
            }
        }

        /// <summary>
        /// Grid Update command to update data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNodeDeadline_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                var nodeDeadlineId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ND_ID"));
                NodeNotificationSettingsContract nodeNotificationSettingsContract = new NodeNotificationSettingsContract();
                nodeNotificationSettingsContract.NodeDeadlineName = (e.Item.FindControl("txtNodeDeadlineName") as WclTextBox).Text.Trim();
                nodeNotificationSettingsContract.NodeDeadlineDescription = (e.Item.FindControl("txtNodeDeadlineDescription") as WclTextBox).Text.Trim();
                nodeNotificationSettingsContract.DeadlineDate = (e.Item.FindControl("dpkrDeadlineDate") as WclDatePicker).SelectedDate;
                var frequency = (e.Item.FindControl("txtFrequency") as WclNumericTextBox).Text.Trim();
                var daysBeforeDeadline = (e.Item.FindControl("txtDaysBeforeDeadline") as WclNumericTextBox).Text.Trim();
                var ddlUserGroup = (e.Item.FindControl("ddlUserGroup") as WclComboBox);

                if (!frequency.IsNullOrEmpty())
                {
                    nodeNotificationSettingsContract.Frequency = Convert.ToInt32(frequency);
                }
                if (!daysBeforeDeadline.IsNullOrEmpty())
                {
                    nodeNotificationSettingsContract.DaysBeforeDeadline = Convert.ToInt32(daysBeforeDeadline);
                }
                List<Int32> _lstUserGroupChecked = new List<Int32>();
                foreach (RadComboBoxItem item in ddlUserGroup.Items)
                {
                    if (item.Checked)
                        _lstUserGroupChecked.Add(Convert.ToInt32(item.Value));
                }
                if (Presenter.UpdateNodeDeadline(nodeNotificationSettingsContract, nodeDeadlineId, _lstUserGroupChecked))
                {
                    e.Canceled = false;
                    if (NotifyStatusChange.IsNotNull())
                        NotifyStatusChange("Node Deadline is updated successfully.", true);
                }
                else
                {
                    e.Canceled = true;
                }
            }

            catch (SysXException ex)
            {
                base.LogError(ex);
                if (NotifyStatusChange.IsNotNull())
                    NotifyStatusChange(ex.Message, false);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                if (NotifyStatusChange.IsNotNull())
                    NotifyStatusChange(ex.Message, false);
            }
        }

        /// <summary>
        /// Grid Delete command to delete data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNodeDeadline_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                var nodeDeadlineId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ND_ID"));
                var nodeNotificationMappingId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ND_NodeNotificationMappingId"));
                if (Presenter.DeleteNodeDeadline(nodeDeadlineId, nodeNotificationMappingId))
                {
                    e.Canceled = false;
                    if (NotifyStatusChange.IsNotNull())
                        NotifyStatusChange("Node Deadline is deleted successfully.", true);
                }
                else
                {
                    e.Canceled = true;
                    //base.ShowInfoMessage("An error occured while deleting Node Deadline. Please try again.");

                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                if (NotifyStatusChange.IsNotNull())
                    NotifyStatusChange(ex.Message, false);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                if (NotifyStatusChange.IsNotNull())
                    NotifyStatusChange(ex.Message, false);
            }
        }

        #region Notification Grid

        protected void grdNotifications_ItemCreated(object sender, GridItemEventArgs e)
        {

            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox cmbCopyType = (WclComboBox)editform.FindControl("cmbCopyType");
                    WclComboBox cmbSubEvent = (WclComboBox)editform.FindControl("cmbSubEvent");

                    Presenter.BindComboBox();

                    if (cmbCopyType.IsNotNull() && CurrentViewContext.copyType.IsNotNull())
                    {
                        List<Entity.lkpCopyType> modifiedCopyType = CurrentViewContext.copyType;
                        modifiedCopyType.Add(new Entity.lkpCopyType { CT_Id = 0, CT_Name = "--SELECT--" });
                        cmbCopyType.DataSource = modifiedCopyType.OrderBy(x => x.CT_Id);
                        cmbCopyType.DataBind();

                    }
                    else
                    {
                        cmbCopyType.Items.Insert(0, new RadComboBoxItem("--SELECT--"));
                    }

                    if (cmbSubEvent.IsNotNull() && CurrentViewContext.subEvent.IsNotNull())
                    {
                        List<Entity.lkpCommunicationSubEvent> modifiedCommunicationSubEvent = CurrentViewContext.subEvent;
                        modifiedCommunicationSubEvent.Add(new Entity.lkpCommunicationSubEvent { CommunicationSubEventID = 0, Name = "--SELECT--" });
                        cmbSubEvent.DataSource = modifiedCommunicationSubEvent.OrderBy(x => x.CommunicationSubEventID);
                        cmbSubEvent.DataBind();
                    }
                    else
                    {
                        cmbSubEvent.Items.Insert(0, new RadComboBoxItem("--SELECT--"));
                    }

                    if (!(e.Item.DataItem is GridInsertionObject))
                    {
                        Entity.HierarchyNotificationMapping hierarchyNotificationMapping = (Entity.HierarchyNotificationMapping)e.Item.DataItem;
                        if (hierarchyNotificationMapping.IsNotNull())
                        {
                            cmbSubEvent.SelectedValue = Convert.ToString(hierarchyNotificationMapping.lkpCommunicationSubEvent.CommunicationSubEventID);
                            cmbCopyType.SelectedValue = Convert.ToString(hierarchyNotificationMapping.lkpCopyType.CT_Id);
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

        protected void grdNotifications_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                string id = parentItem.GetDataKeyValue("InstitutionContact.ICO_ID").ToString();
                Int32 InstitutionHierarchyNodeID = Convert.ToInt32(parentItem.GetDataKeyValue("ICM_ID"));
                Presenter.GetNotificationData(InstitutionHierarchyNodeID);
                (sender as RadGrid).DataSource = CurrentViewContext.lstNotifications;
                ApplyPermissionForNotificationGrid(sender as RadGrid);
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

        protected void grdNotifications_DeleteCommand(object sender, GridCommandEventArgs e)
        {

            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 notificationMappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("HNM_ID"));
                Presenter.DeleteCommunicationSubEvent(notificationMappingID);
                NotifyStatusChange("Notification deleted sucessfully.", true);
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

        protected void grdNotifications_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Boolean checkUnique = false;
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 notificationMappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("HNM_ID"));
                HierarchyNotificationMappingContract hierarchyNotificationMappingContract = new HierarchyNotificationMappingContract();
                WclComboBox cmbCopyType = (e.Item.FindControl("cmbCopyType") as WclComboBox);
                WclComboBox cmbSubEvent = (e.Item.FindControl("cmbSubEvent") as WclComboBox);
                hierarchyNotificationMappingContract.CommunicationSubEventID = Convert.ToInt32(cmbSubEvent.SelectedValue);
                hierarchyNotificationMappingContract.CopyTypeID = Convert.ToInt16(cmbCopyType.SelectedValue);
                Label lblNotificationSuccess = e.Item.FindControl("lblNotificationSuccess") as Label;
                if (!CheckDuplicate(e, checkUnique))
                {
                    Presenter.UpdateCommunicationSubEvent(hierarchyNotificationMappingContract, notificationMappingID);
                    NotifyStatusChange("Notification updated sucessfully.", true);
                }
                else
                {
                    e.Canceled = true;
                    lblNotificationSuccess.Visible = true;
                    lblNotificationSuccess.ShowMessage("Notification already exists.", MessageType.Error);
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

        protected void grdNotifications_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Boolean checkUnique = false;
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32 contactID = Convert.ToInt32(parentItem.GetDataKeyValue("InstitutionContact.ICO_ID"));
                Int32 mappingID = Convert.ToInt32(parentItem.GetDataKeyValue("ICM_ID"));
                HierarchyNotificationMappingContract hierarchyNotificationMappingContract = new HierarchyNotificationMappingContract();

                WclComboBox cmbCopyType = (e.Item.FindControl("cmbCopyType") as WclComboBox);
                WclComboBox cmbSubEvent = (e.Item.FindControl("cmbSubEvent") as WclComboBox);
                hierarchyNotificationMappingContract.CommunicationSubEventID = Convert.ToInt32(cmbSubEvent.SelectedValue);
                hierarchyNotificationMappingContract.CopyTypeID = Convert.ToInt16(cmbCopyType.SelectedValue);

                Label lblNotificationSuccess = e.Item.FindControl("lblNotificationSuccess") as Label;
                if (!CheckDuplicate(e, checkUnique))
                {

                    Presenter.SaveCommunicationSubEvent(hierarchyNotificationMappingContract, mappingID);
                    NotifyStatusChange("Notification has been added successfully.", true);
                }
                else
                {
                    e.Canceled = true;
                    lblNotificationSuccess.Visible = true;
                    lblNotificationSuccess.ShowMessage("Notification already exists.", MessageType.Error);
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

        #region Contact Grid

        protected void grdContacts_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;

                    WclComboBox cmbContactStatus = (WclComboBox)editform.FindControl("cmbContactStatus");
                    Presenter.BindContactStatus();

                    if (cmbContactStatus.IsNotNull() && CurrentViewContext.contactStatus.IsNotNull())
                    {
                        List<InsContact> modifiedContactStatus = CurrentViewContext.contactStatus;
                        modifiedContactStatus.Add(new InsContact { ICO_ID = 0, ICO_Name = "New" });
                        cmbContactStatus.DataSource = modifiedContactStatus.OrderBy(x => x.ICO_ID);
                        cmbContactStatus.DataBind();

                    }
                }

                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    HierarchyContactMapping currentItem = e.Item.DataItem as HierarchyContactMapping;
                    if (currentItem != null)
                    {

                        (e.Item.FindControl("cmbContactStatus") as WclComboBox).Visible = false;
                        (e.Item.FindControl("rfvContactStatus") as RequiredFieldValidator).Enabled = false;
                        (e.Item.FindControl("divContact") as System.Web.UI.HtmlControls.HtmlGenericControl).Visible = false;
                    }
                    else
                    {
                        (e.Item.FindControl("cmbContactStatus") as WclComboBox).Visible = true;
                        (e.Item.FindControl("rfvContactStatus") as RequiredFieldValidator).Enabled = true;
                        (e.Item.FindControl("divContact") as System.Web.UI.HtmlControls.HtmlGenericControl).Visible = true;

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

        protected void cmbContactStatus_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            //var combo = (RadComboBox)sender;
            //combo.Items.Insert(1, new RadComboBoxItem("New", AppConsts.ZERO));
        }

        protected void cmbContactStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox cmbContactStatus = sender as WclComboBox;
                WclTextBox txtFirstName = cmbContactStatus.Parent.NamingContainer.FindControl("txtFirstName") as WclTextBox;
                WclTextBox txtLastName = cmbContactStatus.Parent.NamingContainer.FindControl("txtLastName") as WclTextBox;
                WclTextBox txtTitle = cmbContactStatus.Parent.NamingContainer.FindControl("txtTitle") as WclTextBox;
                WclTextBox txtPrimaryEmailAddress = cmbContactStatus.Parent.NamingContainer.FindControl("txtPrimaryEmailAddress") as WclTextBox;
                WclMaskedTextBox txtPhone = cmbContactStatus.Parent.NamingContainer.FindControl("txtPhone") as WclMaskedTextBox;
                WclTextBox txtAddress1 = cmbContactStatus.Parent.NamingContainer.FindControl("txtAddress1") as WclTextBox;
                WclTextBox txtAddress2 = cmbContactStatus.Parent.NamingContainer.FindControl("txtAddress2") as WclTextBox;
                WclMaskedTextBox txtZipCode = cmbContactStatus.Parent.NamingContainer.FindControl("txtZipCode") as WclMaskedTextBox;
                if (cmbContactStatus.SelectedValue != AppConsts.ZERO && cmbContactStatus.SelectedValue.IsNotNull())
                {

                    Presenter.GetInstitutionContactListById(Convert.ToInt32(cmbContactStatus.SelectedValue));
                    if (CurrentViewContext.institutionContactById.IsNotNull())
                    {
                        InstitutionContact lstContact = CurrentViewContext.institutionContactById;
                        txtFirstName.Text = lstContact.ICO_FirstName;
                        txtLastName.Text = lstContact.ICO_LastName;
                        txtTitle.Text = lstContact.ICO_Title;
                        txtPrimaryEmailAddress.Text = lstContact.ICO_PrimaryEmailAddress;
                        txtPhone.Text = lstContact.ICO_PrimaryPhone;
                        txtAddress1.Text = lstContact.ICO_Address1;
                        txtAddress2.Text = lstContact.ICO_Address2;
                        txtZipCode.Text = lstContact.ICO_ZipCodeID.ToString();
                    }
                }
                else
                {
                    txtFirstName.Text = String.Empty;
                    txtLastName.Text = String.Empty;
                    txtTitle.Text = String.Empty;
                    txtPrimaryEmailAddress.Text = String.Empty;
                    txtPhone.Text = String.Empty;
                    txtAddress1.Text = String.Empty;
                    txtAddress2.Text = String.Empty;
                    txtZipCode.Text = String.Empty;

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

        protected void grdContacts_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdNotifications = parentItem.ChildItem.FindControl("grdNotifications") as RadGrid;
                    foreach (GridItem item in e.Item.OwnerTableView.Items)
                    {
                        if (item.Expanded && item != e.Item)
                        {
                            grdNotifications.MasterTableView.IsItemInserted = false;
                            grdNotifications.MasterTableView.ClearEditItems();
                            item.Expanded = false;
                        }
                    }
                    grdNotifications.Rebind();
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

        #region External User Grid Event

        /// <summary>
        /// Sets the list of filters to be hide in the grid. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdContacts_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdContacts.FilterMenu;
            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.GreaterThanOrEqualTo.ToString() || menu.Items[i].Text == GridKnownFunction.LessThanOrEqualTo.ToString()
                    || menu.Items[i].Text == GridKnownFunction.LessThan.ToString() || menu.Items[i].Text == GridKnownFunction.GreaterThan.ToString() || menu.Items[i].Text == GridKnownFunction.GreaterThan.ToString()
                    || menu.Items[i].Text == GridKnownFunction.IsEmpty.ToString()
                    )
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Grid ItemDataBound event to bind controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdContacts_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                     GridDataItem dataItem = (GridDataItem)e.Item;
                     if (e.Item.OwnerTableView.Columns.FindByUniqueNameSafe("ICO_PrimaryPhone") != null)
                    {
                        dataItem["ICO_PrimaryPhone"].Text = Presenter.GetFormattedPhoneNumber(Convert.ToString(dataItem["ICO_PrimaryPhone"].Text));
                    }
                }

                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    WclComboBox cmbCommunicationSubEvent = gridEditableItem.FindControl("cmbCommunicationSubEvent") as WclComboBox;
                    if (cmbCommunicationSubEvent.IsNotNull())
                    {
                        cmbCommunicationSubEvent.Items.Clear();
                        _presenter.GetCommunicationSubEventsType();
                        cmbCommunicationSubEvent.DataSource = CurrentViewContext.lstSubEvent;
                        cmbCommunicationSubEvent.DataBind();
                        cmbCommunicationSubEvent.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--", "0"));
                    }
                    if (!(e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem))
                    {
                        Int32 communicationSubEventID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ECU_CommunicationSubEventID"));
                        if (communicationSubEventID.IsNotNull() && communicationSubEventID > 0)
                        {
                            cmbCommunicationSubEvent.SelectedValue = communicationSubEventID.ToString();
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
        /// <summary>
        /// Set data source to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdContacts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetInstitutionContactUserData();
                grdContacts.DataSource = CurrentViewContext.lstHierarchyContactMapping;
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
        /// Grid Insert command to insert data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdContacts_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

                InstitutionContactContract institutionContactContract = new InstitutionContactContract();
                institutionContactContract.FirstName = (e.Item.FindControl("txtFirstName") as WclTextBox).Text.Trim();
                institutionContactContract.LastName = (e.Item.FindControl("txtLastName") as WclTextBox).Text.Trim();
                institutionContactContract.Title = (e.Item.FindControl("txtTitle") as WclTextBox).Text.Trim();
                institutionContactContract.EmailAddress = (e.Item.FindControl("txtPrimaryEmailAddress") as WclTextBox).Text.Trim();
                institutionContactContract.PrimaryPhone = (e.Item.FindControl("txtPhone") as WclMaskedTextBox).Text.Trim();
                institutionContactContract.Address1 = (e.Item.FindControl("txtAddress1") as WclTextBox).Text.Trim();
                institutionContactContract.Address2 = (e.Item.FindControl("txtAddress2") as WclTextBox).Text.Trim();

                if (!(e.Item.FindControl("txtZipCode") as WclMaskedTextBox).Text.Trim().Equals(String.Empty))
                {
                    institutionContactContract.ZipCodeID = Convert.ToInt32((e.Item.FindControl("txtZipCode") as WclMaskedTextBox).Text.Trim());
                }
                else
                {
                    institutionContactContract.ZipCodeID = (Int32?)null;
                }



                WclComboBox cmbContactStatus = (e.Item.FindControl("cmbContactStatus") as WclComboBox);
                Boolean isNew = false;
                Int32 contactID = Convert.ToInt32(cmbContactStatus.SelectedValue);
                //check for email exists or not
                if (Presenter.IsContactExists(institutionContactContract.EmailAddress.Trim(), contactID))
                {
                    lblContactMessage.Visible = true;
                    lblContactMessage.ShowMessage("Contact with same email id is already exist.", MessageType.Error);
                    e.Canceled = true;
                    return;
                }

                if (cmbContactStatus.SelectedValue == AppConsts.ZERO)
                {
                    isNew = true;
                    if (Presenter.SaveContact(institutionContactContract, isNew))
                    {
                        if (!(SysXWebSiteUtils.SessionService).BusinessChannelType.IsNull() &&
             (SysXWebSiteUtils.SessionService).BusinessChannelType.BusinessChannelTypeID == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
                        {

                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                        }
                        e.Canceled = false;
                        if (NotifyStatusChange.IsNotNull())
                            NotifyStatusChange("Contact is saved successfully.", true);

                    }
                }
                else
                {
                    isNew = false;
                    if (Presenter.SaveContact(institutionContactContract, isNew, Convert.ToInt32(cmbContactStatus.SelectedValue)))
                    {
                        if (!(SysXWebSiteUtils.SessionService).BusinessChannelType.IsNull() &&
             (SysXWebSiteUtils.SessionService).BusinessChannelType.BusinessChannelTypeID == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
                        {

                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                        }
                        e.Canceled = false;
                        if (NotifyStatusChange.IsNotNull())
                            NotifyStatusChange("Contact is saved successfully.", true);

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
        /// <summary>
        /// Grid Update command to update data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdContacts_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 contactID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("InstitutionContact.ICO_ID"));
                InstitutionContactContract institutionContactContract = new InstitutionContactContract();
                institutionContactContract.FirstName = (e.Item.FindControl("txtFirstName") as WclTextBox).Text.Trim();
                institutionContactContract.LastName = (e.Item.FindControl("txtLastName") as WclTextBox).Text.Trim();
                institutionContactContract.Title = (e.Item.FindControl("txtTitle") as WclTextBox).Text.Trim();
                institutionContactContract.EmailAddress = (e.Item.FindControl("txtPrimaryEmailAddress") as WclTextBox).Text.Trim();
                institutionContactContract.PrimaryPhone = (e.Item.FindControl("txtPhone") as WclMaskedTextBox).Text.Trim();
                institutionContactContract.Address1 = (e.Item.FindControl("txtAddress1") as WclTextBox).Text.Trim();
                institutionContactContract.Address2 = (e.Item.FindControl("txtAddress2") as WclTextBox).Text.Trim();

                if ((e.Item.FindControl("txtZipCode") as WclMaskedTextBox).Text.Trim() != "")
                {
                    institutionContactContract.ZipCodeID = Convert.ToInt32((e.Item.FindControl("txtZipCode") as WclMaskedTextBox).Text.Trim());
                }
                else
                {
                    institutionContactContract.ZipCodeID = (Int32?)null;
                }

                //check for email exists or not
                if (Presenter.IsContactExists(institutionContactContract.EmailAddress.Trim(), contactID))
                {
                    lblContactMessage.Visible = true;
                    lblContactMessage.ShowMessage("Contact with same email id is already exist.", MessageType.Error);
                    e.Canceled = true;
                    return;
                }

                WclComboBox cmbContactStatus = (e.Item.FindControl("cmbContactStatus") as WclComboBox);



                if (Presenter.UpdateContact(institutionContactContract, contactID))
                {
                    if (!(SysXWebSiteUtils.SessionService).BusinessChannelType.IsNull() &&
               (SysXWebSiteUtils.SessionService).BusinessChannelType.BusinessChannelTypeID == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
                    {

                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    e.Canceled = false;
                    if (NotifyStatusChange.IsNotNull())
                        NotifyStatusChange("Contact is updated successfully.", true);
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

        /// Grid Delete command to delete data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdContacts_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 contactID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("InstitutionContact.ICO_ID"));
                if (Presenter.DeleteContact(contactID))
                {
                    if (!(SysXWebSiteUtils.SessionService).BusinessChannelType.IsNull() &&
     (SysXWebSiteUtils.SessionService).BusinessChannelType.BusinessChannelTypeID == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    e.Canceled = false;
                    if (NotifyStatusChange.IsNotNull())
                        NotifyStatusChange("Contact is deleted successfully.", true);
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

        /// <summary>
        /// To save Nag Email Notifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarNagEmailNotify_SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.SaveNagEmailNotifications())
                {
                    CurrentViewContext.SavedNagFrequency = CurrentViewContext.NagFrequency;
                    fsucCmdBarNagEmailNotify.SubmitButton.Enabled = true;

                    if (NotifyStatusChange.IsNotNull())
                        NotifyStatusChange("Nag Email Notifications is saved successfully.", true);
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

        protected void fsucCmdBarNagEmailNotify_NagEmailTempClick(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            Int32 rootNodeNotificationMappingID = Presenter.GetRootNodeNotificationMappingID();
            queryString = new Dictionary<String, String>
                                                         { 
                                                           { "SelectedTenantId", Convert.ToString(CurrentViewContext.SelectedTenantID) },
                                                           { "Frequency", Convert.ToString(CurrentViewContext.SavedNagFrequency) },
                                                           { "SubEventCode", CommunicationSubEvents.NOTIFICATION_FOR_NAG_EMAIL.GetStringValue() },
                                                           { "NodeNotificationTypeCode", lkpNodeNotificationTypesContext.NAGEMAILS.GetStringValue() },
                                                           { "HierarchyNodeID", Convert.ToString(CurrentViewContext.HierarchyNodeID) },
                                                           { "RootNodeNotificationMappingId", Convert.ToString(rootNodeNotificationMappingID) },
                                                           { "TemplateNodeLevel", TemplateNodeLevels.ROOTLEVEL.GetStringValue() },
                                                           { "PermissionCode", CurrentViewContext.PermissionCode },
                                                           { "ParentID", Convert.ToString(CurrentViewContext.ParentID) }
                                                         };
            Response.Redirect("~/Templates/Pages/NodeNotifications.aspx?args=" + queryString.ToEncryptedQueryString());
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// To bind user groups
        /// </summary>
        private void BindUserGroups()
        {
            //Presenter.GetAllUserGroups();
            //ddlUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            //ddlUserGroup.DataBind();
        }

        /// <summary>
        /// Business Rule Implementation
        /// </summary>
        private void BusinessRuleImplementations()
        {
            //If Business Channel Type= AMS only show the Notification Control
            if (!(SysXWebSiteUtils.SessionService).BusinessChannelType.IsNull() &&
                (SysXWebSiteUtils.SessionService).BusinessChannelType.BusinessChannelTypeID == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
            {
                divNagEmailNotfication.Visible = false;
                divDeadlineNotification.Visible = false;
                lblDeadlineNotifications.Visible = false;
                grdNodeDeadline.NeedDataSource -= grdNodeDeadline_NeedDataSource;

            }

        }

        /// <summary>
        /// Method to apply permission on notification grid of contacts grid.
        /// </summary>
        /// <param name="grdNotificationGrid">notification grid</param>
        private void ApplyPermissionForNotificationGrid(RadGrid grdNotificationGrid)
        {
            List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
            base.ApplyActionLevelPermission(actionCollection, "Node Notification Settings");
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "AddNotification")
                                {
                                    grdNotificationGrid.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "EditNotification")
                                {
                                    grdNotificationGrid.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteNotification")
                                {
                                    grdNotificationGrid.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }
                }
                    );
            }
        }

        #endregion

        /// <summary>
        /// Duplicate check for notification grid
        /// </summary>
        /// <param name="e"></param>
        /// <param name="isDuplicate"></param>
        /// <returns></returns>
        private Boolean CheckDuplicate(GridCommandEventArgs e, Boolean isDuplicate)
        {
            String cmbCopyTypeKey = (e.Item.FindControl("cmbCopyType") as WclComboBox).SelectedValue;
            String cmbSubEventKey = (e.Item.FindControl("cmbSubEvent") as WclComboBox).SelectedValue;
            foreach (Telerik.Web.UI.DataKey key in e.Item.OwnerTableView.DataKeyValues)
            {
                if (((e.Item.ItemIndex > AppConsts.MINUS_ONE) && (key["HNM_ID"] != e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HNM_ID"])) || (e.Item.ItemIndex == AppConsts.MINUS_ONE))
                {
                    if ((key["lkpCopyType.CT_Id"] != null) && (key["lkpCopyType.CT_Id"].ToString() == cmbCopyTypeKey))
                    {
                        if ((key["lkpCommunicationSubEvent.CommunicationSubEventID"] != null) && (key["lkpCommunicationSubEvent.CommunicationSubEventID"].ToString() == cmbSubEventKey))
                        {
                            isDuplicate = true;
                            break;
                        }
                    }
                }
            }
            return isDuplicate;
        }



        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "AddContact";
                objClsFeatureAction.CustomActionLabel = "Add Contact";
                objClsFeatureAction.ScreenName = "Node Notification Settings";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Edit";
                objClsFeatureAction.CustomActionLabel = "Edit Contact";
                objClsFeatureAction.ScreenName = "Node Notification Settings";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "Delete Contact";
                objClsFeatureAction.ScreenName = "Node Notification Settings";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "AddNotification";
                objClsFeatureAction.CustomActionLabel = "Add Notfication";
                objClsFeatureAction.ScreenName = "Node Notification Settings";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "EditNotification";
                objClsFeatureAction.CustomActionLabel = "Edit Notification";
                objClsFeatureAction.ScreenName = "Node Notification Settings";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "DeleteNotification";
                objClsFeatureAction.CustomActionLabel = "Delete Notification";
                objClsFeatureAction.ScreenName = "Node Notification Settings";
                actionCollection.Add(objClsFeatureAction);
                return actionCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }
        private void ApplyPermisions()
        {
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "AddContact")
                                {
                                    grdContacts.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdContacts.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdContacts.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }
    }
}