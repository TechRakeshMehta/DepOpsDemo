#region NameSpace
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
#endregion

namespace CoreWeb.BkgOperations.Views
{
    public partial class DisclosureAndReleaseForm : BaseUserControl, IDisclosureAndReleaseFormView
    {
        #region Private Variables
        private DisclosureAndReleaseFormPresenter _presenter = new DisclosureAndReleaseFormPresenter();
        #endregion

        #region Properties

        #region Private Properties

        IDisclosureAndReleaseFormView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IDisclosureAndReleaseFormView.MasterOrderID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID);
                return 0;
            }
        }

        Int32 IDisclosureAndReleaseFormView.SelectedTenantID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID);
                return 0;
            }
        }

        Int32 IDisclosureAndReleaseFormView.loggedInUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }

        }
        List<Entity.ClientEntity.ApplicantDocument> IDisclosureAndReleaseFormView.lstDnRDocuments
        {
            get;
            set;
        }
        String IDisclosureAndReleaseFormView.SetTenantID
        {
            set
            {
                hdnTenantId.Value = value.ToString();
            }
        }

        List<SystemDocBkgSvcMapping> IDisclosureAndReleaseFormView.lstApplicantDocs
        {
            get
            {
                if (!ViewState["lstApplicantDocs"].IsNullOrEmpty())
                    return (List<SystemDocBkgSvcMapping>)ViewState["lstApplicantDocs"];
                return new List<SystemDocBkgSvcMapping>();
            }
            set
            {
                ViewState["lstApplicantDocs"] = value;
            }
        }

        #endregion

        #region Public Properties

        public DisclosureAndReleaseFormPresenter Presenter
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

        #endregion

        #region Page Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            //UAT-3745
            GetApplicantAdditionalDoc();
        }

        #endregion

        #region Grid Events
        /// <summary>
        /// Bind Grid grdDNR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDNR_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetDisclosureReleaseDoc();
                grdDNR.DataSource = CurrentViewContext.lstDnRDocuments;
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

        #region UAT-3745

        private void GetApplicantAdditionalDoc()
        {
            Presenter.GetApplicantAdditionalDoc();
            ManageAdditionalDocsVisibility();
        }

        private void ManageAdditionalDocsVisibility()
        {
            if (!CurrentViewContext.lstApplicantDocs.IsNullOrEmpty())
                dvAdditionalDocs.Style.Add("display", "block");
                
            else
                dvAdditionalDocs.Style.Add("display", "none");
        }

        protected void grdAdditionalDocuments_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (GridDataItem dataItem in grdAdditionalDocuments.Items)
                {
                    GridTableView grdTableView = (GridTableView)dataItem.OwnerTableView;
                    for (int rowIndex = grdTableView.Items.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridDataItem row = grdTableView.Items[rowIndex];
                        GridDataItem previousRow = grdTableView.Items[rowIndex + 1];
                        if (row["BkgServiceName"].Text == previousRow["BkgServiceName"].Text)
                        {
                            row["BkgServiceName"].RowSpan = previousRow["BkgServiceName"].RowSpan < 2 ? 2 : previousRow["BkgServiceName"].RowSpan + 1;
                            previousRow["BkgServiceName"].Visible = false;
                            previousRow["BkgServiceName"].Text = "&nbsp;";
                        }
                    }
                }
                grdAdditionalDocuments.ClientSettings.EnableRowHoverStyle = false;
                grdAdditionalDocuments.ClientSettings.Selecting.AllowRowSelect = false;
                grdAdditionalDocuments.ClientSettings.EnableAlternatingItems = false;
                // grdAdditionalDocuments.GridLines = GridLines.None;
            }
            catch (SystemException ex)
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

        protected void grdAdditionalDocuments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdAdditionalDocuments.DataSource = CurrentViewContext.lstApplicantDocs;
            }
            catch (SystemException ex)
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