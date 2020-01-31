using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageSeriesDose : BaseWebPage, IManageSeriesDoseView
    {
        #region Variables

        #region Private Variables

        private ManageSeriesDosePresenter _presenter = new ManageSeriesDosePresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        public IManageSeriesDoseView CurrentViewContext
        {
            get { return this; }
        }

        public ManageSeriesDosePresenter Presenter
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
        /// Id of the Category to which the Dose belongs to
        /// </summary>
        Int32 IManageSeriesDoseView.SelectedCategoryId
        {
            get
            {
                return Convert.ToInt32(ViewState["CatId"]);
            }
            set
            {
                ViewState["CatId"] = value;
            }
        }

        /// <summary>
        /// Id of the Series to which the Dose belongs to
        /// </summary>
        Int32 IManageSeriesDoseView.SelectedSeriesId
        {
            get
            {
                return Convert.ToInt32(ViewState["SeriesId"]);
            }
            set
            {
                ViewState["SeriesId"] = value;
            }
        }

        /// <summary>
        /// ComplianceItems related to selected Categories
        /// </summary>
        List<ComplianceItem> IManageSeriesDoseView.lstComplianceItems
        {
            get;
            set;
        }

        /// <summary>
        /// ComplianceAttributes related to selected Items
        /// </summary>
        List<ComplianceItemAttribute> IManageSeriesDoseView.lstCompliancItemeAttributes
        {
            get
            {
                return ViewState["lstAttributes"].IsNull()
                                ? new List<ComplianceItemAttribute>()
                                : ViewState["lstAttributes"] as List<ComplianceItemAttribute>;
            }
            set
            {
                ViewState["lstAttributes"] = value;
            }
        }

        /// <summary>
        /// Id of the Selected Tenant 
        /// </summary>
        Int32 IManageSeriesDoseView.TenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        /// <summary>
        /// Id of the Item for which attributes are to be fetched
        /// </summary>
        Int32 IManageSeriesDoseView.SelectedComplianceItemId
        {
            get;
            set;
        }

        /// <summary>
        /// List of Items selected for Save
        /// </summary>
        List<Int32> IManageSeriesDoseView.lstSelectedComplianceItem
        {
            get
            {
                return cmbComplianceItems.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
            }
            set
            {
                cmbComplianceItems.Items.Where(item => value.Contains(Convert.ToInt32(item.Value))).ForEach(itm =>
                {
                    itm.Checked = true;
                });
            }

        }

        /// <summary>
        /// List of Attributes selected for Save
        /// </summary>
        Dictionary<Int32, Boolean> IManageSeriesDoseView.dicAttributes
        {
            get;
            set;
        }

        //List of Compliance Attributes
        List<ComplianceAttribute> IManageSeriesDoseView.lstComplianceAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the Current LoggedInUser
        /// </summary>
        Int32 IManageSeriesDoseView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// Return if it is Edit Mode of the Screen
        /// </summary>
        Boolean IManageSeriesDoseView.IsEditMode
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsEditMode"]);
            }
            set
            {
                ViewState["IsEditMode"] = value;
            }
        }

        /// <summary>
        /// list of selected attribute id are stored
        /// </summary>
        List<Int32> IManageSeriesDoseView.lstSelectedAttributes
        {
            get
            {
                return cmbAttributes.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
            }
            set
            {
                cmbAttributes.Items.Where(item => value.Contains(Convert.ToInt32(item.Value))).ForEach(item =>
                {
                    item.Checked = true;
                });
            }
        }

        /// <summary>
        /// get the selected Key Attrbiute
        /// </summary>
        Int32 IManageSeriesDoseView.SelectedKeyAttribute
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedKeyAttribute"]);
            }
            set
            {
                ViewState["SelectedKeyAttribute"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            CaptureQueryStringData();
            if (!IsPostBack)
            {
                ClearSession();

                BindComplianceItems();
                if (CurrentViewContext.IsEditMode)
                {
                    BindComplianceAttribute(true);
                }
                BindComplianceKeyAttribute();

            }
            if (CurrentViewContext.IsEditMode)
            {
                LoadMappingTable();
            }

            Boolean asynchronousRequest = System.Web.UI.ScriptManager.GetCurrent(this).IsInAsyncPostBack;
        }

        protected void cmbComplianceItems_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindComplianceAttribute(false);
            BindComplianceKeyAttribute();
        }

        protected void cmbAttributes_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindComplianceKeyAttribute();
        }

        protected void fsucCmdBar_SaveClick(object sender, EventArgs e)
        {
            CurrentViewContext.dicAttributes = new Dictionary<Int32, Boolean>();
            foreach (var attr in cmbAttributes.CheckedItems)
            {
                CurrentViewContext.dicAttributes.Add(Convert.ToInt32(attr.Value), attr.Value == cmbKeyAttributes.SelectedValue ? true : false);
            }

            Presenter.SaveSeriesData();
            CurrentViewContext.IsEditMode = true;
            ClearSession();
            LoadMappingTable();
        }

        protected void cmbKeyAttributes_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CurrentViewContext.SelectedKeyAttribute = Convert.ToInt32(e.Value);
        }

        #endregion

        #region Private Method
        /// <summary>
        /// Bind Compliance Items of selected Category
        /// </summary>
        private void BindComplianceItems()
        {
            Presenter.GetComplianceItems();
            cmbComplianceItems.DataSource = CurrentViewContext.lstComplianceItems;
            cmbComplianceItems.DataBind();
            Presenter.CheckComplinaceItemForEditMode();
        }

        /// <summary>
        /// Read data from Query String
        /// </summary>
        private void CaptureQueryStringData()
        {
            if (Request.QueryString["SeriesId"].IsNotNull() && Request.QueryString["TenantId"].IsNotNull() && Request.QueryString["CatId"].IsNotNull())
            {
                CurrentViewContext.SelectedCategoryId = Convert.ToInt32(Request.QueryString["CatId"]);
                CurrentViewContext.SelectedSeriesId = Convert.ToInt32(Request.QueryString["SeriesId"]);
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
            }
        }

        /// <summary>
        /// Genarate the Mapping table, based on the selected Items.
        /// </summary>
        private void LoadMappingTable()
        {
            pnl.Controls.Clear();
            Control seriesDoseControl = Page.LoadControl("~\\ComplianceAdministration\\UserControl\\SeriesDose.ascx");

            (seriesDoseControl as SeriesDose).SeriesId = CurrentViewContext.SelectedSeriesId;
            (seriesDoseControl as SeriesDose).SelectedTenantId = CurrentViewContext.TenantId;
            (seriesDoseControl as SeriesDose).ID = "ucSeriesDose_" + CurrentViewContext.SelectedSeriesId;
            pnl.Controls.Add(seriesDoseControl);
        }

        /// <summary>
        /// bind data in compliance attribute DropdownList
        /// </summary>
        private void BindComplianceAttribute(Boolean IsFirstTimePageLoad)
        {
            List<Int32> chkattrib = CurrentViewContext.lstSelectedAttributes;
            Presenter.GetComplianceAttributes();
            cmbAttributes.DataSource = CurrentViewContext.lstComplianceAttributes;
            cmbAttributes.DataBind();
            CurrentViewContext.lstSelectedAttributes = chkattrib;
            if (IsFirstTimePageLoad)
            {
                Presenter.ChcekComplianceAttributeForEditMode();
            }
        }

        /// <summary>
        /// Bind Key Attribute Drop down
        /// </summary>
        private void BindComplianceKeyAttribute()
        {
            Int32 selectedkeyAttr = CurrentViewContext.SelectedKeyAttribute;
            cmbKeyAttributes.DataSource = cmbAttributes.CheckedItems;
            cmbKeyAttributes.DataBind();
            cmbKeyAttributes.Items.Insert(0, new RadComboBoxItem("--SELECT--", "0"));
            if (!cmbKeyAttributes.Items.FirstOrDefault(keyattr => Convert.ToInt32(keyattr.Value) == selectedkeyAttr).IsNullOrEmpty())
            {
                cmbKeyAttributes.Items.FirstOrDefault(keyattr => Convert.ToInt32(keyattr.Value) == selectedkeyAttr).Selected = true;
            }
            else
            {
                cmbKeyAttributes.SelectedIndex = AppConsts.NONE;
                CurrentViewContext.SelectedKeyAttribute = AppConsts.NONE;
            }
        }

        /// <summary>
        /// Clear the Previous Session data of Mapping
        /// </summary>
        private void ClearSession()
        {
            if (Session[AppConsts.SHOTSERIES_SESSION_LISTATTRIBUTESCONTRACT].IsNotNull() && Session[AppConsts.SHOTSERIES_SESSION_LISTITEMCONTRACT].IsNotNull())
            {
                Session.Remove(AppConsts.SHOTSERIES_SESSION_LISTATTRIBUTESCONTRACT);
                Session.Remove(AppConsts.SHOTSERIES_SESSION_LISTITEMCONTRACT);
            }
        }

        #endregion 
    }
}