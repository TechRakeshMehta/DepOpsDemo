using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgOperations.Views
{
    public partial class ServiceLevelDetailsForOrder : BaseUserControl, IServiceLevelDetailsForOrderView
    {

        #region Variables

        #region Protected Variables

        protected String ImagePath = "~/images/small";
        protected String ImagePathMedium = "~/images/medium";

        #endregion

        #region Private Variables

        private ServiceLevelDetailsForOrderPresenter _presenter = new ServiceLevelDetailsForOrderPresenter();
        private Int32 _tenantid;

        #endregion

        #endregion

        #region Properties

        #region Private Properties


        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        private IServiceLevelDetailsForOrderView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private ServiceLevelDetailsForOrderPresenter Presenter
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

        public Int32 SelectedTenantId
        {
            get;
            set;
        }

        public Int32 OrderID
        {
            get;
            set;
        }

        List<ServiceLevelDetailsForOrderContract> IServiceLevelDetailsForOrderView.LstServiceDetails
        {
            get
            {
                return ViewState["lstServiceGrpDetails"] as List<ServiceLevelDetailsForOrderContract>;
            }

            set
            {
                ViewState["lstServiceGrpDetails"] = value as List<ServiceLevelDetailsForOrderContract>;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events


        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    Presenter.GetServiceLevelDetailsForOrder();
                    hfTenantId.Value = CurrentViewContext.SelectedTenantId.ToString();
                    hfOrderID.Value = CurrentViewContext.OrderID.ToString();
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

        #endregion

        #region Grid Related Events

        protected void grdPackage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdPackage.DataSource = CurrentViewContext.LstServiceDetails.DistinctBy(col => col.PackageID);
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

        protected void grdServiceGrp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32? packageID = Convert.ToInt32(parentItem.GetDataKeyValue("PackageID"));
                (sender as RadGrid).DataSource = CurrentViewContext.LstServiceDetails.Where(col => col.PackageID == packageID).DistinctBy(col => col.PackageServiceGroupID);
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

        protected void grdServices_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32? serviceGroupID = Convert.ToInt32(parentItem.GetDataKeyValue("ServiceGroupID"));
                Int32? bkgPkgSvcGrpID = Convert.ToInt32(parentItem.GetDataKeyValue("PackageServiceGroupID"));
                (sender as RadGrid).DataSource = CurrentViewContext.LstServiceDetails.Where(col => col.PackageServiceGroupID == bkgPkgSvcGrpID).DistinctBy(col => col.ServiceID);
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

        protected void grdServiceGrp_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Image imgStatusServiceGrp = e.Item.FindControl("imgStatusServiceGrp") as Image;
                    ServiceLevelDetailsForOrderContract serviceLevelDetailsForOrderContract = (ServiceLevelDetailsForOrderContract)e.Item.DataItem;
                    Int32 svcgrpID = serviceLevelDetailsForOrderContract.ServiceGroupID;
                    Int32 bkgPackageServiceGrpId = serviceLevelDetailsForOrderContract.PackageServiceGroupID;
                    List<ServiceLevelDetailsForOrderContract> selectedLstServiceLevelDetailsForOrder = CurrentViewContext.LstServiceDetails
                                                                                            .Where(cond => cond.PackageServiceGroupID == bkgPackageServiceGrpId).ToList();

                    if (selectedLstServiceLevelDetailsForOrder.FirstOrDefault().IsServiceGroupStatusComplete)
                    {
                        if (selectedLstServiceLevelDetailsForOrder.Any(cond => cond.IsServiceFlagged))
                        {
                            imgStatusServiceGrp.ImageUrl = ImagePath + "/Red.gif";
                            imgStatusServiceGrp.AlternateText = "Red";
                        }
                        else
                        {
                            imgStatusServiceGrp.ImageUrl = ImagePath + "/Green.gif";
                            imgStatusServiceGrp.AlternateText = "Green";
                        }
                    }
                    else
                    {
                        HideServiceGroupControls(e, imgStatusServiceGrp);
                    }

                    if (selectedLstServiceLevelDetailsForOrder.Any(cond => cond.ServiceTypeCode.Equals(BkgServiceType.OPERATIONSUPPORTAUTOCOMPLETE.GetStringValue())))
                    {
                        Image imgServiceGroupPDF = e.Item.FindControl("imgServiceGroupPDF") as Image;
                        imgServiceGroupPDF.ImageUrl = ImagePath + "/Blank.gif";
                        imgServiceGroupPDF.Visible = false;
                    }

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

        protected void grdServices_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    Image imgStatus = e.Item.FindControl("imgStatus") as Image;
                    ServiceLevelDetailsForOrderContract serviceLevelDetailsForOrderContract = (ServiceLevelDetailsForOrderContract)e.Item.DataItem;
                    Int32 svcID = serviceLevelDetailsForOrderContract.ServiceID;
                    Int32 bkgPkgSvcGrpId = serviceLevelDetailsForOrderContract.PackageServiceGroupID;
                    List<ServiceLevelDetailsForOrderContract> selectedLstServiceLevelDetailsForOrder = CurrentViewContext.LstServiceDetails
                                                                                            .Where(cond => cond.PackageServiceGroupID == bkgPkgSvcGrpId
                                                                                            && cond.ServiceID == svcID).ToList();  // svcID check added in UAT-4377// 

                    if (selectedLstServiceLevelDetailsForOrder.FirstOrDefault().IsServiceCompleted)
                    {
                        if (selectedLstServiceLevelDetailsForOrder.Any(cond => cond.IsServiceFlagged))
                        {
                            imgStatus.ImageUrl = ImagePath + "/Red.gif";
                            imgStatus.AlternateText = "Red";
                        }
                        else
                        {
                            imgStatus.ImageUrl = ImagePath + "/Green.gif";
                            imgStatus.AlternateText = "Green";
                        }
                    }
                    else
                    {
                        imgStatus.ImageUrl = ImagePath + "/Blank.gif";
                        imgStatus.AlternateText = String.Empty;
                        imgStatus.Visible = false;
                    }
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

        private void HideServiceGroupControls(GridItemEventArgs e, Image imgStatusServiceGrp)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            imgStatusServiceGrp.ImageUrl = ImagePath + "/Blank.gif";
            Image imgServiceGroupPDF = e.Item.FindControl("imgServiceGroupPDF") as Image;
            imgServiceGroupPDF.ImageUrl = ImagePath + "/Blank.gif";
            HyperLink hlPackageGroupDocument = e.Item.FindControl("hlPackageGroupDocument") as HyperLink;
            hlPackageGroupDocument.Enabled = false;
            imgServiceGroupPDF.Visible = false;
            imgStatusServiceGrp.Visible = false;
            hlPackageGroupDocument.Visible = false;
        }

        protected void grdPackage_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdServiceGrp = parentItem.ChildItem.FindControl("grdServiceGrp") as RadGrid;
                    grdServiceGrp.MasterTableView.HierarchyDefaultExpanded = false;
                    grdServiceGrp.Rebind();
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

        protected void grdServiceGrp_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdServices = parentItem.ChildItem.FindControl("grdServices") as RadGrid;
                    grdServices.Rebind();
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
    }
}