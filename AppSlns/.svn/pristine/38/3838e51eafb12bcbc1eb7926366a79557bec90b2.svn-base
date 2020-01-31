using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
using CoreWeb.Shell;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.UserControl.Views
{
    public partial class AdditionalDocumentsMapping : BaseUserControl, IAdditionalDocumentsMappingView
    {
        #region VARIABLES
        private AdditionalDocumentsMappingPresenter _presenter = new AdditionalDocumentsMappingPresenter();

        #endregion

        #region PROPERTIES

        public AdditionalDocumentsMappingPresenter Presenter
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

        public IAdditionalDocumentsMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String PageType
        {
            get
            {
                return Convert.ToString(ViewState["PageType"]);
            }
            set
            {
                ViewState["PageType"] = value;
            }
        }

        String IAdditionalDocumentsMappingView.ErrorMessage
        {
            get;
            set;
        }

        String IAdditionalDocumentsMappingView.SuccessMessage
        {
            get;
            set;
        }

        String IAdditionalDocumentsMappingView.InfoMessage
        {
            get;
            set;
        }

        Int32 IAdditionalDocumentsMappingView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 IAdditionalDocumentsMappingView.TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IAdditionalDocumentsMappingView.DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        Int32 IAdditionalDocumentsMappingView.DocMappingID
        {
            get;
            set;
        }

        List<GenericSystemDocumentMappingContract> IAdditionalDocumentsMappingView.lstGenericSystemDocumentMapping
        {
            get;
            set;
        }

        Int32 IAdditionalDocumentsMappingView.RecordID
        {
            get;
            set;
        }

        Int32 IAdditionalDocumentsMappingView.RecortTypeID
        {
            get;
            set;
        }

        List<SystemDocument> IAdditionalDocumentsMappingView.lstAdditionalDocuments
        {
            get;
            set;
        }

        List<Int32> IAdditionalDocumentsMappingView.SelectedAdditionalDocumentsID
        {
            get;
            set;
        }

        String IAdditionalDocumentsMappingView.RecortTypeCode
        {
            get;
            set;
        }

        List<Int32> IAdditionalDocumentsMappingView.lstMappedSysDocIDs
        {
            get
            {
                return (ViewState["lstMappedSysDocIDs"]) as List<Int32>;
            }
            set
            {
                ViewState["lstMappedSysDocIDs"] = value;
            }
        }

        #endregion

        #region PAGE EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Request.QueryString["TenantId"]) > AppConsts.NONE)
                {
                    CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                }
                else if (Convert.ToInt32(Request.QueryString["SelectedTenantId"]) > AppConsts.NONE)
                {
                    CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                }

                Presenter.OnViewInitialized();

                CurrentViewContext.RecortTypeCode = Convert.ToString(Request.QueryString["RecordTypeCode"]);

                if (String.Compare(CurrentViewContext.RecortTypeCode, RecordType.Institution_Node.GetStringValue(), true) == AppConsts.NONE && Convert.ToInt32(Request.QueryString["Id"]) > AppConsts.NONE)
                {
                    CurrentViewContext.RecordID = Convert.ToInt32(Request.QueryString["Id"]);
                }
                else if (String.Compare(CurrentViewContext.RecortTypeCode, RecordType.Compliance_Package.GetStringValue(), true) == AppConsts.NONE && Convert.ToInt32(Request.QueryString["CompliancePkgId"]) > AppConsts.NONE)
                {
                    CurrentViewContext.RecordID = Convert.ToInt32(Request.QueryString["CompliancePkgId"]);
                }
                else if (String.Compare(CurrentViewContext.RecortTypeCode, RecordType.Background_Package.GetStringValue(), true) == AppConsts.NONE && Convert.ToInt32(Request.QueryString["BPAId"]) > AppConsts.NONE)
                {
                    CurrentViewContext.RecordID = Convert.ToInt32(Request.QueryString["BPAId"]);
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

        private void BindAdditionalDocuments()
        {
        }

        #endregion

        #region CONTROLS EVENTS
        protected void grdAdditionalDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.TenantId > AppConsts.NONE)
                {
                    Presenter.GetGenericSystemDocumentMapping();
                    grdAdditionalDocuments.DataSource = CurrentViewContext.lstGenericSystemDocumentMapping;
                }
                else
                {
                    grdAdditionalDocuments.DataSource = new List<GenericSystemDocumentMappingContract>();
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

        protected void grdAdditionalDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    WclComboBox cmbAdditionalDocuments = e.Item.FindControl("cmbAdditionalDocuments") as WclComboBox;
                    Presenter.GetAdditionalDocuments();
                    cmbAdditionalDocuments.DataSource = CurrentViewContext.lstAdditionalDocuments;
                    cmbAdditionalDocuments.DataBind();
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

        protected void grdAdditionalDocuments_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                WclComboBox cmbAdditionalDocuments = (e.Item.FindControl("cmbAdditionalDocuments") as WclComboBox);
                if (cmbAdditionalDocuments.CheckedItems.Count != AppConsts.NONE)
                {
                    CurrentViewContext.SelectedAdditionalDocumentsID = cmbAdditionalDocuments.CheckedItems.Select(x => Convert.ToInt32(x.Value)).ToList();
                    Presenter.SaveAdditionalDocumentMapping();
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    grdAdditionalDocuments.Rebind();
                }
                else
                {
                    (this.Page as BaseWebPage).ShowInfoMessage("Please select at least one document to map.");
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

        protected void grdAdditionalDocuments_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.DocMappingID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SystemDocMappingID"]);
                Presenter.DeleteAdditionalDocumentMapping();
                (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                grdAdditionalDocuments.Rebind();
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

    }
}