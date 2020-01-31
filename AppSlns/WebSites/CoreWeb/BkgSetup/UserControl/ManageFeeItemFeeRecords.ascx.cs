using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{

    public partial class ManageFeeItemFeeRecords : BaseUserControl, IManageFeeItemFeeRecordView
    {

        #region Variables

        #region Private Variables

        private ManageFeeItemFeeRecordsPresenter _presenter = new ManageFeeItemFeeRecordsPresenter();
        private FeeRecordContract _viewContract;
        #endregion
        #endregion

        #region Properties

        #region Public Properties

        public int SelectedFeeItemId
        {
            get
            {
                if (!ViewState["FeeItemId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["FeeItemId"]);
                }
                return 0;
            }
            set
            {
                ViewState["FeeItemId"] = value;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        public ManageFeeItemFeeRecordsPresenter Presenter
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

        public IManageFeeItemFeeRecordView CurrentViewContext
        {
            get { return this; }
        }

        #endregion

        #region Private Properties


        FeeRecordContract IManageFeeItemFeeRecordView.ViewContract
        {
            get
            {
                if (_viewContract == null)
                {
                    _viewContract = new FeeRecordContract();
                }

                return _viewContract;
            }

        }

        int IManageFeeItemFeeRecordView.currentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        string IManageFeeItemFeeRecordView.ErrorMessage
        {
            get;
            set;
        }

        string IManageFeeItemFeeRecordView.InfoMessage
        {
            get;
            set;
        }

        string IManageFeeItemFeeRecordView.SuccessMessage
        {
            get;
            set;
        }


        List<LocalFeeRecordsInfo> IManageFeeItemFeeRecordView.ListServiceItemFeeRecord
        {
            get;
            set;
        }

        List<Entity.State> IManageFeeItemFeeRecordView.ListAllState
        {
            get;
            set;
        }

        List<Entity.County> IManageFeeItemFeeRecordView.ListCountyByStateId
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
            Presenter.OnViewInitialized();
            // For Fixed no separate fee record Bug#6863
            if (Presenter.CheckIfFeeItemIsFixedType())
                divAddFeeRecords.Visible = false;
            else
            {
                divAddFeeRecords.Visible = true;
            }
            ApplyActionLevelPermission(ActionCollection, "Manage Fee Record");
        }

        #endregion

        #region Button Events

        protected void btnAddFeeRecord_Click(object sender, EventArgs e)
        {
            try
            {
                ShowAddItemBlock();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.Amount = Convert.ToDecimal(txtAmount.Text.Trim());
                CurrentViewContext.ViewContract.FieldValue = divCounty.Visible ? cmbCounty.SelectedValue :  cmbState.SelectedValue;                
                Presenter.SaveServiceItemFeeRecord();
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    ClearControls();
                    grdFeeRecord.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearControls();
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

        protected void grdFeeRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetServiceItemFeeRecordList();
            grdFeeRecord.DataSource = CurrentViewContext.ListServiceItemFeeRecord;
        }

        protected void grdFeeRecord_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 serviceItemFeeRecordID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("LocalSFRID"));
                Presenter.DeleteServiceItemFeeRecord(serviceItemFeeRecordID);
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    ClearControls();
                    grdFeeRecord.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        //protected void grdFeeRecord_ItemCreated(object sender, GridItemEventArgs e)
        //{
        //    if (e.Item is GridDataItem && e.Item.DataItem != null)
        //    {
        //        LocalFeeRecordsInfo localFeeRecordsInfo = e.Item.DataItem as LocalFeeRecordsInfo;
        //        String sSIFTCode = "";
        //        GridDataItem dataItem = e.Item as GridDataItem;
        //        if (localFeeRecordsInfo.LocalSFRID > 0)
        //        {
        //            sSIFTCode = Presenter.GetCurrentlkpServiceItemFeeTypeSIFTCode(localFeeRecordsInfo.LocalSFRID, false);
        //        }
        //        else if(localFeeRecordsInfo.GlobalSIFR_ID.HasValue && localFeeRecordsInfo.GlobalSIFR_ID > 0)
        //        {
        //            sSIFTCode = Presenter.GetCurrentlkpServiceItemFeeTypeSIFTCode(localFeeRecordsInfo.GlobalSIFR_ID.Value, true);
        //        }

        //        dataItem["DeleteColumn"].Controls[0].Visible = !localFeeRecordsInfo.ISGLobal;

        //        if (localFeeRecordsInfo.IsNotNull())
        //        {
        //            if (sSIFTCode != ServiceItemFeeType.FIXED_FEE.GetStringValue())
        //            {
        //                Int32 fieldValue = Convert.ToInt32(localFeeRecordsInfo.LocalSFRFieldValue);
        //                Label lblState = e.Item.FindControl("lblState") as Label;
        //                Label lblCounty = e.Item.FindControl("lblCounty") as Label;
        //                if (Presenter.IfFieldValueStateOrCounty())
        //                {
        //                    Presenter.GetAllState(false);
        //                    lblState.Text = CurrentViewContext.ListAllState.Where(obj => obj.StateID == fieldValue).FirstOrDefault().StateName;
        //                }
        //                else
        //                {
        //                    lblCounty.Text = Presenter.GetCountyByCountyId(fieldValue).CountyName;
        //                }
        //            }
        //            else if (sSIFTCode == ServiceItemFeeType.FIXED_FEE.GetStringValue())
        //            {
        //                btnAddFeeRecord.Enabled = false;
        //                dataItem["DeleteColumn"].Controls[0].Visible = false;
        //            }                    
        //        }

        //    }
        //}

        #endregion

        #region Dropdown Events

        protected void cmbState_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!Presenter.IfFieldValueStateOrCounty())
            {
                Presenter.GetCountyByStateId(Convert.ToInt32(cmbState.SelectedValue));
                BindCombo(cmbCounty, CurrentViewContext.ListCountyByStateId);
                divCounty.Visible = true;
            }
            else
            {
                txtGlobalFee.Text = Presenter.GetGlobalFeeAmount(cmbState.SelectedValue);
            }
        }


        protected void cmbCounty_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            txtGlobalFee.Text = Presenter.GetGlobalFeeAmount(cmbCounty.SelectedValue, cmbState.SelectedValue);
        }


        #endregion


        #endregion

        #region Methods

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
        }

        private void ShowAddItemBlock()
        {
            Presenter.GetAllState(true);
            BindCombo(cmbState, CurrentViewContext.ListAllState);
            divCounty.Visible = false;
            divFeeRecord.Visible = divButtonSave.Visible = true;
        }

        private void ClearControls()
        {
            txtAmount.Text = txtGlobalFee.Text = "" ;
            cmbState.SelectedValue = cmbCounty.SelectedValue = "-1";
            divCounty.Visible = divButtonSave.Visible = divFeeRecord.Visible = false;
        }

        #endregion

        #region Apply Permissions

        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
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
                                if (x.FeatureAction.CustomActionId == "SaveFeeRecord")
                                {
                                    btnSave.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteFeeRecord")
                                {
                                    grdFeeRecord.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }

        #endregion
    }
}