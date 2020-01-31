#region Namespaces

#region System Defined Namespaces

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

#region User Defined Namespaces

using Telerik.Web.UI;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using System.Threading;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    /// <summary>
    /// Displays the Package Detail till Items.
    /// </summary>
    public partial class ViewPackageDetail : BaseUserControl, IViewPackageDetailView
    {
        #region Variables

        #region Private Variables

        private ViewPackageDetailPresenter _presenter=new ViewPackageDetailPresenter();
        //String _viewType = null;

        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Public Properties

        
        public ViewPackageDetailPresenter Presenter
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

        /// <summary>
        /// Gets the data of TreeListPackagesDetail.
        /// </summary>
        public List<GetPackageDetail> TreeListPackagesDetail
        {
            get;
            set;
        }

        public Int32 ManageTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["ManageTenantId"]);
            }
            set
            {
                ViewState["ManageTenantId"] = value;
            }
        }

        public Int32 PackageID
        {
            get;
            set;
        }

        public IViewPackageDetailView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String OrderRequestMode
        {
            get;
            set;
        }

        #endregion

        #region Private Properties



        #endregion

        #endregion

        #region Methods

        #region Page Load Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //_viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                Dictionary<String, String> queryString = new Dictionary<String, String>();

                //Decrypt the TenantId from Query String.
                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                }

                //Checks if the TenantId is present in Query String.
                if (queryString.ContainsKey("TenantId"))
                {
                    //Assigns the TenantId to property ManageTenantId.
                    if (!queryString["TenantId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ManageTenantId = Convert.ToInt32(queryString["TenantId"]);
                    }
                }

                //Checks if the PackageID is present in Query String.
                if (queryString.ContainsKey("PackageId"))
                {
                    //Assigns the TenantId to property ManageTenantId.
                    if (!queryString["PackageId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.PackageID = Convert.ToInt32(queryString["PackageId"]);
                    }
                }

                if (queryString.ContainsKey("OrderRequestType"))
                {
                    if (!queryString["OrderRequestType"].IsNullOrEmpty())
                    {
                        this.OrderRequestMode = Convert.ToString(queryString["OrderRequestType"]);
                    }
                }

                //Checks if page is PostBack or not.
                if (!this.IsPostBack)
                {
                    //Gets the assigned tree data from database and sets it in property AssignedTreeData.
                    Presenter.GetTreeData();
                    Presenter.OnViewInitialized();
                }
                Presenter.OnViewLoaded();

                base.SetPageTitle("Package Detail");
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

        #region Tree List Events

        /// <summary>
        /// Retrieve Block's Feature.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListPackageDetail_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                treeListPackageDetail.DataSource = TreeListPackagesDetail;
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

        protected void treeListPackageDetail_PreRender(object sender, EventArgs e)
        {
            try
            {
                treeListPackageDetail.ExpandAllItems();
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

        protected void treeListPackageDetail_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            try
            {
                // Below code disables the expand button.
                if (e.Item is TreeListDataItem)
                {
                    Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                    if (expandButton != null)
                    {
                        ((Button)expandButton).Enabled = false;
                        ((Button)expandButton).Visible = false;
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

        #endregion

        #region Button Methods

        protected void fsucCmdBarAssignment_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString;
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (Convert.ToString(applicantOrderCart.OrderRequestType) == OrderRequestType.RenewalOrder.GetStringValue() )
                {

                    queryString = new Dictionary<String, String>()
                                                         { 
                                                            {"OrderId",applicantOrderCart.PrevOrderId.ToString()},
                                                            { "Child",  ChildControls.RenewalOrder}
                                                         };
                    //Response.Redirect(String.Format("~/Main/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                    Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
                else
                {
                    queryString = new Dictionary<String, String>()
                                                         { 
                                                            { "Child",  @"~\ComplianceOperations\UserControl\PendingOrder.ascx"}
                                                         };
                    Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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

        #region PrivateMethods



        #endregion

        #endregion
    }
}

