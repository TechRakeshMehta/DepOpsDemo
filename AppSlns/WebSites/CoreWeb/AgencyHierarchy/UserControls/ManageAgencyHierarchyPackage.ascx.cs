using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class ManageAgencyHierarchyPackage : BaseUserControl, IManageAgencyHierarchyPackageView
    {
        #region Handlers
        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;
        #endregion

        #region [Private Variables]

        private ManageAgencyHierarchyPackagePresenter _presenter = new ManageAgencyHierarchyPackagePresenter();

        #endregion

        #region [Properties]

        #region [Public Properties]

        public ManageAgencyHierarchyPackagePresenter Presenter
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

        public IManageAgencyHierarchyPackageView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IManageAgencyHierarchyPackageView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IManageAgencyHierarchyPackageView.AgencyHierarchyId
        {
            get
            {
                return Convert.ToInt32(ViewState["AgencyHierarchyId"]);
            }
            set
            {
                ViewState["AgencyHierarchyId"] = value;
            }
        }

        public Int32 SelectedRootNodeID
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedRootNodeID"]);
            }
            set
            {
                ViewState["SelectedRootNodeID"] = value;
            }
        }

        List<RequirementPackageContract> IManageAgencyHierarchyPackageView.lstRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> IManageAgencyHierarchyPackageView.lstAgencyHierarchyPackages
        {
            get;
            set;
        }

        AgencyHierarchyPackageContract IManageAgencyHierarchyPackageView.agencyHierarchyPackageContract
        {
            get;
            set;
        }

        Int32 IManageAgencyHierarchyPackageView.CurrentPageIndex
        {
            get
            {
                return grdAgencyHirarchyPackage.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (value > 0)
                {
                    grdAgencyHirarchyPackage.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        Int32 IManageAgencyHierarchyPackageView.PageSize
        {
            get
            {
                return grdAgencyHirarchyPackage.MasterTableView.PageSize;
            }
            set
            {
                grdAgencyHirarchyPackage.MasterTableView.PageSize = value;
            }
        }

        Int32 IManageAgencyHierarchyPackageView.VirtualRecordCount
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
                grdAgencyHirarchyPackage.VirtualItemCount = value;
                grdAgencyHirarchyPackage.MasterTableView.VirtualItemCount = value;
            }
        }

        CustomPagingArgsContract IManageAgencyHierarchyPackageView.GridCustomPaging
        {
            get
            {
                if (ViewState["_gridCustomPaging"] == null)
                {
                    ViewState["_gridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["_gridCustomPaging"];
            }
            set
            {
                ViewState["_gridCustomPaging"] = value;
                CurrentViewContext.VirtualRecordCount = value.VirtualPageCount;
                CurrentViewContext.PageSize = value.PageSize;
                CurrentViewContext.CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        public Int32 NodeId { get; set; }
        #endregion

        #region [Private Properties]

        #endregion

        #endregion

        #region [Page Events]
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    CurrentViewContext.AgencyHierarchyId = NodeId;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }
        #endregion

        #region [Controls Events]

        #region [Combobox Events]

        #endregion

        #region [Grid Events]
        protected void grdAgencyHirarchyPackage_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                AgencyHierarchyPackageContract agencyHierarchyPackageContract = new AgencyHierarchyPackageContract();
                agencyHierarchyPackageContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchyPackageContract.AgencyHierarchyPackageID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyPackageID"]);
                agencyHierarchyPackageContract.RequirementPackageID= Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementPackageID"]); //UAT-4657
                CurrentViewContext.agencyHierarchyPackageContract = agencyHierarchyPackageContract;
                #region UAT-4657
                String ShowAlert = Presenter.IsPackageVersionInProgress(agencyHierarchyPackageContract.RequirementPackageID);
                if (ShowAlert == "Package")
                {
                    eventShowMessage(sender, StatusMessages.INFO_MESSAGE, "Currently package digestion is in progress. " +
                        "Please try again after sometime!");
                }
                else if (ShowAlert == "Category")
                {
                    eventShowMessage(sender, StatusMessages.INFO_MESSAGE, "Category digestion of this package is pending." +
                       "Please try again after sometime!");
                }
                #endregion
                else
                {
                    if (Presenter.DeleteAgencyHierarchyPackageMapping())
                        eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Package Deleted Successfully.");
                    else
                        eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while package un-mapping with selected node.");
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHirarchyPackage_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            

            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Boolean isNewPackage = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsNewPackage"]);                    
                    //RadButton btnCopyPkg = ((RadButton)e.Item.FindControl("btbPackageCopy"));
                    //#region UAT-3494
                    //String RequirementPackageCodeType = ((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementPackageCodeType"]).ToString();
                    //if (RequirementPackageCodeType == RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue())                    
                    //    btnCopyPkg.Visible = true;                    
                    //else
                    //    btnCopyPkg.Visible = false;
                    //#endregion

                    #region UAT-4402
                    string packageCategoryCount = ((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempPackageCategoryCount"]).ToString();
                    string requirementPackageID = ((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementPackageID"]).ToString();
                    

                    HtmlAnchor lnkNameOfPackageCategorys = ((HtmlAnchor)e.Item.FindControl("lnkPackageCategoryName"));
                    (e.Item.FindControl("lnkPackageCategoryName") as HtmlAnchor).InnerText = Convert.ToString(packageCategoryCount);
                    if (!string.IsNullOrEmpty((e.Item.FindControl("lnkPackageCategoryName") as HtmlAnchor).InnerText))
                    {
                        
                        Dictionary<String, String> queryString = new Dictionary<String, String>();
                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"RequirementPackageID", requirementPackageID.ToString() }
                        };
                        lnkNameOfPackageCategorys.Attributes.Add("args", queryString.ToEncryptedQueryString());
                    }
                    else
                    {
                        lnkNameOfPackageCategorys.Visible = false;
                    }
                    #endregion

                    if (!isNewPackage)
                    {
                        dataItem["EditCommandColumn"].Controls[0].Visible = false;
                    }
                    else
                    {
                        dataItem["EditCommandColumn"].Controls[0].Visible = true;
                    }
                }
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox cmbPackage = editform.FindControl("cmbPackage") as WclComboBox;

                    RequirementPackageContract requirementPackageContract = e.Item.DataItem as RequirementPackageContract;

                    if (requirementPackageContract != null)
                    {
                        cmbPackage.SelectedValue = requirementPackageContract.RequirementPackageID.ToString();
                    }
                    BindPackagesForAddEditForm(cmbPackage, requirementPackageContract);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHirarchyPackage_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {

                AgencyHierarchyPackageContract agencyHierarchyPackageContract = new AgencyHierarchyPackageContract();
                agencyHierarchyPackageContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                if (e.CommandName == "PerformInsert")
                {
                    WclComboBox cmbPackage = e.Item.FindControl("cmbPackage") as WclComboBox;
                    if (cmbPackage.IsNotNull() && !cmbPackage.SelectedValue.IsNullOrEmpty())
                    {

                        agencyHierarchyPackageContract.RequirementPackageID = Convert.ToInt32(cmbPackage.SelectedValue);
                        agencyHierarchyPackageContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyId;
                        CurrentViewContext.agencyHierarchyPackageContract = agencyHierarchyPackageContract;
                        if (Presenter.SaveUpdateAgencyHierarchyPackageMapping())
                            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Package Successfully Added.");
                        else
                            eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while adding package to selected node.");
                    }
                }
                else if (e.CommandName == "Edit")
                {
                    //UAT-4657
                    String ShowAlert = Presenter.IsPackageVersionInProgress(Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementPackageID"]));
                    if (ShowAlert == "Package")
                    {
                        e.Canceled = true;
                        eventShowMessage(sender, StatusMessages.INFO_MESSAGE, "Currently package digestion is in progress. " +
                                                    "Please try again after sometime!");
                    }
                    else if (ShowAlert == "Category")
                    {
                        e.Canceled = true;
                        eventShowMessage(sender, StatusMessages.INFO_MESSAGE, "Category digestion of this package is pending." +
                            "Please try again after sometime!");
                    }
                    else
                    {
                        e.Canceled = true;
                        RedirectOnPackageCategoryMapping(Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyPackageID"]), Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementPackageID"]));
                    }
                }
                //else if (e.CommandName == "PackageReplace") //UAT-3494
                //{

                //    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                //    Int32 requirementPackageID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("RequirementPackageID"));
                //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopUpToReplacePackage('" + Convert.ToString(requirementPackageID) + "','" + CurrentViewContext.AgencyHierarchyId + "',true);", true);
                //}
                if (e.CommandName.IsNullOrEmpty())
                {
                    grdAgencyHirarchyPackage.MasterTableView.GetColumn("TempPackageCategoryCount").Display = true;
                }
                if (e.CommandName == "Cancel")
                {
                    grdAgencyHirarchyPackage.MasterTableView.GetColumn("TempPackageCategoryCount").Display = false;
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHirarchyPackage_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                CurrentViewContext.GridCustomPaging.CurrentPageIndex = CurrentViewContext.CurrentPageIndex;
                CurrentViewContext.GridCustomPaging.PageSize = CurrentViewContext.PageSize;
                CurrentViewContext.GridCustomPaging = CurrentViewContext.GridCustomPaging;
                Presenter.GetAgencyHirarchyPackages();
                grdAgencyHirarchyPackage.DataSource = CurrentViewContext.lstAgencyHierarchyPackages;

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHirarchyPackage_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHirarchyPackage_Init(object sender, EventArgs e)
        {
            GridFilterMenu menu = grdAgencyHirarchyPackage.FilterMenu;

            if (grdAgencyHirarchyPackage.clearFilterMethod == null)
                grdAgencyHirarchyPackage.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);

            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
        #endregion

        #endregion

        #region [Private Methods]
        private void BindPackagesForAddEditForm(RadComboBox cmbPackageOnAddEditForm, RequirementPackageContract requirementPackageContract)
        {
            Presenter.GetRequiremetPackages(requirementPackageContract);
            cmbPackageOnAddEditForm.DataSource = CurrentViewContext.lstRequirementPackage;
            cmbPackageOnAddEditForm.DataBind();
            cmbPackageOnAddEditForm.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            cmbPackageOnAddEditForm.Focus();
        }

        private void RedirectOnPackageCategoryMapping(Int32 agencyHierarchyID, Int32 requirementPackageID)
        {
            //Int32 SelectedRootNodeID = Maste
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                         { 
                                                            {"Child", @"~/RotationPackages/UserControl/PackageCategoryMapping.ascx"},
                                                           {"RequirementPackageID",Convert.ToString(requirementPackageID)},
                                                           {"AgencyHierarchyId",Convert.ToString(CurrentViewContext.AgencyHierarchyId)},
                                                           {"SelectedRootNodeID",Convert.ToString(SelectedRootNodeID)}
                                                         };
            string url = String.Format("../../RotationPackages/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RedirectControls('" + url + "');", true);
        }
        #endregion


        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;
            CurrentViewContext.GridCustomPaging.FilterColumns = null;
            CurrentViewContext.GridCustomPaging.FilterOperators = null;
            CurrentViewContext.GridCustomPaging.FilterValues = null;
            CurrentViewContext.GridCustomPaging.FilterTypes = null;
        }

        //#region UAT-3494
        //protected void btnRebindGrid_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // GetSessionDataForFilters();
        //        grdAgencyHirarchyPackage.Rebind();
        //        eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Requirement package replaced successfully.");
        //        //base.ShowSuccessMessage("Requirement replaced copied successfully.");
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}
        //#endregion

    }
}