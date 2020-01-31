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

using Entity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.ComplianceOperation;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageInvoiceGroups : BaseUserControl, IManageInvoiceGroupsView
    {

        #region  Variables

        #region Private Variables

        private ManageInvoiceGroupsPresenter _presenter = new ManageInvoiceGroupsPresenter();

        private Int32 _tenantId;
        #endregion

        #endregion

        #region Properties

        public ManageInvoiceGroupsPresenter Presenter
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

        public IManageInvoiceGroupsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IManageInvoiceGroupsView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IManageInvoiceGroupsView.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        String IManageInvoiceGroupsView.ErrorMessage
        {
            get;
            set;
        }

        String IManageInvoiceGroupsView.SuccessMessage
        {
            get;
            set;
        }

        List<InvoiceGroupContract> IManageInvoiceGroupsView.lstInvoiceGroups
        {
            get;
            set;
        }

        List<Tenant> IManageInvoiceGroupsView.lstTenants
        {
            get;
            set;
        }

        List<Entity.ClientEntity.DeptProgramMapping> IManageInvoiceGroupsView.lstNodes
        {
            get;
            set;
        }

        List<lkpReportColumn> IManageInvoiceGroupsView.lstReportColumns
        {
            get;
            set;
        }

        Int32 IManageInvoiceGroupsView.InvoiceGroupID
        {
            get;
            set;
        }

        List<InvoiceGroupNodes> IManageInvoiceGroupsView.LstInvoiceGroupNodes
        {
            get
            {
                if (ViewState["InvoiceGroupNodes"].IsNotNull())
                {
                    return (List<InvoiceGroupNodes>)ViewState["InvoiceGroupNodes"];
                }
                else
                {
                    return new List<InvoiceGroupNodes>();
                }
            }
            set
            {
                ViewState["InvoiceGroupNodes"] = value;
            }
        }


        #endregion

        #region  Page Events

        protected override void OnInit(EventArgs e)
        {
            base.Title = "Manage Invoice Groups";
            base.SetPageTitle("Manage Invoice Groups");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

            }
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// To bind grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdInvoiceGroups_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetInvoiceGroupDetails();
                grdInvoiceGroups.DataSource = CurrentViewContext.lstInvoiceGroups;
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
        /// Save, Update and Delete records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdInvoiceGroups_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                        CurrentViewContext.InvoiceGroupID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("InvoiceGroupID"));
                    }

                    var invoiceGroupName = (e.Item.FindControl("txtInvoiceGroupeName") as WclTextBox).Text.Trim();
                    var invoiceGroupDescription = (e.Item.FindControl("txtInvoiceGroupDescription") as WclTextBox).Text.Trim();
                    WclComboBox cmbTenant = (e.Item.FindControl("cmbTenant") as WclComboBox);
                    WclComboBox cmbNode = (e.Item.FindControl("cmbNode") as WclComboBox);
                    WclComboBox cmbReportColumn = (e.Item.FindControl("cmbReportColumn") as WclComboBox);
                    List<Int32> selectedNewMappedTenantIDs = new List<Int32>();
                    List<String> selectedNewMappedNodeIDs = new List<String>();
                    List<Int32> selectedNewMappedReportColumnIDs = new List<Int32>();

                    Dictionary<int, bool> dicTenant = new Dictionary<int, bool>();

                    foreach (var item in cmbTenant.Items.Where(itm => itm.Checked))
                    {
                        selectedNewMappedTenantIDs.Add(Convert.ToInt32(item.Value));
                        dicTenant.Add(Convert.ToInt32(item.Value), false);
                    }

                    if (!cmbNode.Items.Any())
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Please load nodes.");
                        return;
                    }

                    foreach (var item in cmbNode.Items.Where(itm => itm.Checked))
                    {
                        selectedNewMappedNodeIDs.Add(item.Value);

                        Int32 tenantID = 0;
                        Int32.TryParse(item.Value.Split('_')[0], out tenantID);

                        if (dicTenant.ContainsKey(tenantID))
                        {
                            if (dicTenant[tenantID] == false)
                                dicTenant[tenantID] = true;
                        }
                        else
                        {
                            e.Canceled = true;
                            base.ShowInfoMessage("Please load nodes again.");
                            return;
                        }
                    }

                    foreach (var item in dicTenant)
                    {
                        if (item.Value == false)
                        {
                            if (CurrentViewContext.LstInvoiceGroupNodes.IsNullOrEmpty())
                            {
                                e.Canceled = true;
                                base.ShowInfoMessage("Please load nodes.");
                                return;
                            }
                            else
                            {
                                var rootNode = CurrentViewContext.LstInvoiceGroupNodes.FirstOrDefault(cond => cond.TenantID == item.Key && cond.ParentNodeValue == "");
                                if (rootNode.IsNotNull())
                                    selectedNewMappedNodeIDs.Add(rootNode.NodeValue);
                                else
                                {
                                    e.Canceled = true;
                                    base.ShowInfoMessage("Please load nodes again.");
                                    return;
                                }
                            }
                        }
                    }

                    foreach (var item in cmbReportColumn.Items.Where(itm => itm.Checked))
                    {
                        selectedNewMappedReportColumnIDs.Add(Convert.ToInt32(item.Value));
                    }

                    if (Presenter.SaveInvoiceGroup(invoiceGroupName, invoiceGroupDescription, selectedNewMappedTenantIDs, selectedNewMappedNodeIDs, selectedNewMappedReportColumnIDs))
                    {
                        e.Canceled = false;
                        if (e.CommandName == RadGrid.UpdateCommandName)
                        {
                            base.ShowSuccessMessage("Invoice Group updated successfully.");
                        }
                        else
                        {
                            base.ShowSuccessMessage("Invoice Group saved successfully.");
                        }
                    }
                    else
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Some error occurred. Please try again.");
                    }
                }

                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    CurrentViewContext.InvoiceGroupID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("InvoiceGroupID"));

                    if (Presenter.DeleteInvoiceGroup())
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Invoice Group deleted successfully.");
                    }
                    else
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Some error occured. Please try again.");
                        grdInvoiceGroups.Rebind();
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

        protected void grdInvoiceGroups_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                //if (e.Item is GridDataItem)
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                    {
                        //insert item
                        //GridEditFormItem editform = (e.Item as GridEditFormItem);
                    }
                    else
                    {
                        //edit item
                        GridEditFormItem editform = (e.Item as GridEditFormItem);
                        GridDataItem item = e.Item as GridDataItem;
                        WclComboBox cmbNode = (WclComboBox)editform.FindControl("cmbNode");
                        WclComboBox cmbTenant = (WclComboBox)editform.FindControl("cmbTenant");
                        WclComboBox cmbReportColumn = (WclComboBox)editform.FindControl("cmbReportColumn");
                        String institutionIDs = Convert.ToString(editform.GetDataKeyValue("InstitutionIDs"));
                        String institutionHierarchyIDs = Convert.ToString(editform.GetDataKeyValue("InstitutionHierarchyIDs"));
                        String reportColumnIDs = Convert.ToString(editform.GetDataKeyValue("ReportColumnIDs"));
                        var lstInstitutionIDs = institutionIDs.Split(',');
                        var lstInstitutionHierarchyIDs = institutionHierarchyIDs.Split(',');
                        var lstReportColumnIDs = reportColumnIDs.Split(',');

                        foreach (RadComboBoxItem cmbItem in cmbTenant.Items)
                        {
                            if (lstInstitutionIDs.Contains(cmbItem.Value))
                                cmbItem.Checked = true;
                        }

                        //Bind Nodes
                        BindNodes(cmbTenant, cmbNode);

                        foreach (RadComboBoxItem cmbItem in cmbNode.Items)
                        {
                            if (lstInstitutionHierarchyIDs.Contains(cmbItem.Value))
                                cmbItem.Checked = true;
                        }
                        foreach (RadComboBoxItem cmbItem in cmbReportColumn.Items)
                        {
                            if (lstReportColumnIDs.Contains(cmbItem.Value))
                                cmbItem.Checked = true;
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
        /// ItemCreated event to bind grid contols
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdInvoiceGroups_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    //Get tenants
                    WclComboBox cmbTenant = (WclComboBox)editform.FindControl("cmbTenant");
                    Presenter.GetTenants();
                    //Get Report Columns
                    WclComboBox cmbReportColumn = (WclComboBox)editform.FindControl("cmbReportColumn");
                    Presenter.GetReportColumns();

                    BindComboBox(cmbTenant, CurrentViewContext.lstTenants);
                    BindComboBox(cmbReportColumn, CurrentViewContext.lstReportColumns);
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

        /// <summary>
        /// ItemChecked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbTenant_ItemChecked(object sender, RadComboBoxItemEventArgs e)
        {
            try
            {
                WclComboBox cmbTenant = sender as WclComboBox;
                BindNodes(cmbTenant, cmbTenant.Parent.NamingContainer.FindControl("cmbNode") as WclComboBox);
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

        #region Button Events

        /// <summary>
        /// Event to load nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLoadNodes_Click(object sender, EventArgs e)
        {
            WclButton btnLoadNodes = sender as WclButton;
            WclComboBox cmbTenant = btnLoadNodes.Parent.NamingContainer.FindControl("cmbTenant") as WclComboBox;
            WclComboBox cmbNode = btnLoadNodes.Parent.NamingContainer.FindControl("cmbNode") as WclComboBox;

            BindNodes(cmbTenant, cmbNode);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Bind ComboBox
        /// </summary>
        /// <param name="cmbBox"></param>
        /// <param name="dataSource"></param>
        private void BindComboBox(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
        }

        /// <summary>
        /// To bind Nodes
        /// </summary>
        /// <param name="cmbTenant"></param>
        /// <param name="cmbNode"></param>
        private void BindNodes(WclComboBox cmbTenant, WclComboBox cmbNode)
        {
            List<String> lstSelectedHierarchyIDs = new List<String>();

            foreach (RadComboBoxItem item in cmbTenant.Items)
            {
                int tenantID = Convert.ToInt32(item.Value);

                if (item.Checked)
                {
                    if (!CurrentViewContext.LstInvoiceGroupNodes.Any(cond => cond.TenantID == tenantID))
                    {
                        Presenter.GetNodes(tenantID);
                        List<InvoiceGroupNodes> lstInvoiceGroupNodes = new List<InvoiceGroupNodes>();

                        foreach (var DPM in CurrentViewContext.lstNodes)
                        {
                            lstInvoiceGroupNodes.Add(new InvoiceGroupNodes
                            {
                                NodeLabel = DPM.DPM_Label,
                                NodeValue = tenantID.ToString() + "_" + DPM.DPM_ID.ToString(),
                                TenantID = tenantID,
                                ParentNodeValue = DPM.DPM_ParentNodeID.ToString()
                            });
                        }
                        if (CurrentViewContext.LstInvoiceGroupNodes.IsNullOrEmpty())
                        {
                            CurrentViewContext.LstInvoiceGroupNodes = lstInvoiceGroupNodes;
                        }
                        else
                        {
                            var tempLstInvoiceGroupNodes = CurrentViewContext.LstInvoiceGroupNodes;
                            tempLstInvoiceGroupNodes.AddRange(lstInvoiceGroupNodes);
                            CurrentViewContext.LstInvoiceGroupNodes = tempLstInvoiceGroupNodes;
                        }
                    }
                }
                else
                {
                    if (CurrentViewContext.LstInvoiceGroupNodes.Any(cond => cond.TenantID == tenantID))
                    {
                        var lstInvoiceGroupNodes = CurrentViewContext.LstInvoiceGroupNodes.Where(cond => cond.TenantID == tenantID).ToList();
                        foreach (var invoiceGroupNodes in lstInvoiceGroupNodes)
                        {
                            CurrentViewContext.LstInvoiceGroupNodes.Remove(invoiceGroupNodes);
                        }
                    }
                }
            }
            if (cmbNode.IsNotNull())
            {
                foreach (RadComboBoxItem cmbItem in cmbNode.Items.Where(itm => itm.Checked))
                {
                    lstSelectedHierarchyIDs.Add(cmbItem.Value);
                }

                BindComboBox(cmbNode, CurrentViewContext.LstInvoiceGroupNodes);

                if (!lstSelectedHierarchyIDs.IsNullOrEmpty())
                {
                    foreach (RadComboBoxItem cmbItem in cmbNode.Items)
                    {
                        if (lstSelectedHierarchyIDs.Contains(cmbItem.Value))
                            cmbItem.Checked = true;
                    }
                }
            }
        }

        #endregion
    }
}