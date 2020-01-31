using CoreWeb.IntsofSecurityModel;
using CoreWeb.ReportsTableau.Views;
using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;

namespace CoreWeb.Reports.Views
{
    public partial class CategoryDataReportByComplioID : BaseUserControl, ICategoryDataReportByComplioIDView
    {
        SysXMembershipUser _user;
        String _userID = String.Empty;
        private CategoryDataReportByComplioIDPresenter _presenter = new CategoryDataReportByComplioIDPresenter();
        private Boolean IsSharedUserLoginURL;

        public CategoryDataReportByComplioIDPresenter Presenter
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

        public ICategoryDataReportByComplioIDView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String UserID
        {
            get
            {
                if (_userID.IsNullOrEmpty())
                {
                    if (_user.IsNotNull())
                    {
                        _userID = _user.UserId.ToString();
                    }
                }
                return _userID;
            }
            set { _userID = value; }
        }

        public List<Int32> SelectedTenantIDs
        {
            get
            {
                return cmbInstitute.Items.Where(x => x.Checked).Select(x => Convert.ToInt32(x.Value)).ToList();
            }

            set
            {
                foreach (Int32 item in value)
                {
                    cmbInstitute.Items.FirstOrDefault(x => x.Value == item.ToString()).Checked = true;
                }
            }
        }

        public List<Int32> SelectedAgencyIDs
        {
            get
            {
                return cmbAgency.Items.Where(x => x.Checked).Select(x => Convert.ToInt32(x.Value)).ToList();
            }

            set
            {
                foreach (Int32 item in value)
                {
                    cmbAgency.Items.FirstOrDefault(x => x.Value == item.ToString()).Checked = true;
                }
            }
        }

        public List<String> SelectedCategories
        {
            get
            {
                return cmbCategory.Items.Where(x => x.Checked).Select(x => x.Value).ToList();
            }
            set
            {
                foreach (String item in value)
                {
                    cmbCategory.Items.FirstOrDefault(x => x.Value == item.ToString()).Checked = true;
                }
            }
        }

        public List<String> SelectedItems
        {
            get
            {
                return cmbItem.Items.Where(x => x.Checked).Select(x => x.Value).ToList();
            }
            set
            {
                foreach (String item in value)
                {
                    cmbItem.Items.FirstOrDefault(x => x.Value == item.ToString()).Checked = true;
                }
            }
        }

        public DateTime? RotationStartDate
        {
            get
            {
                return dpRotationStartDate.SelectedDate;
            }
            set
            {
                if (value.HasValue)
                    dpRotationStartDate.SelectedDate = value.Value;
            }
        }

        public DateTime? RotationEndDate
        {
            get
            {
                return dpRotationEndDate.SelectedDate;
            }
            set
            {
                if (value.HasValue)
                    dpRotationEndDate.SelectedDate = value.Value;
            }
        }

        public String ComplioID
        {
            get
            {
                return txtComplioID.Text;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) txtComplioID.Text = value;
            }
        }

        public String CustomAttribute
        {
            get
            {
                return txtCustomAttribute.Text;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) txtCustomAttribute.Text = value;
            }
        }

        public List<String> SelectedReviewStatus
        {
            get
            {
                return cmbReviewStatus.Items.Where(x => x.Checked).Select(x => x.Value).ToList();
            }
            set
            {
                foreach (String item in value)
                {
                    cmbReviewStatus.Items.FirstOrDefault(x => x.Value == item).Checked = true;
                }
            }
        }

        public List<String> SelectedUserType
        {
            get
            {
                return cmbUserType.Items.Where(x => x.Checked).Select(x => x.Value).ToList();
            }
            set
            {
                foreach (String item in value)
                {
                    cmbUserType.Items.FirstOrDefault(x => x.Value == item).Checked = true;
                }
            }
        }

        public Dictionary<Int32, String> Tenants
        {
            set
            {
                cmbInstitute.DataSource = value.OrderBy(x => x.Value); cmbInstitute.DataBind();
            }
        }
        public Dictionary<Int32, String> Agencies
        {
            set
            {
                cmbAgency.DataSource = value.OrderBy(x => x.Value); cmbAgency.DataBind();
            }
        }
        public Dictionary<String, String> Categories
        {
            set
            {
                cmbCategory.DataSource = value.OrderBy(x => x.Value); cmbCategory.DataBind();
            }
        }
        public Dictionary<String, String> Items
        {
            set
            {
                cmbItem.DataSource = value.OrderBy(x => x.Value); cmbItem.DataBind();
            }
        }
        public Dictionary<String, String> ReviewStatus
        {
            set
            {
                cmbReviewStatus.DataSource = value.OrderBy(x => x.Value); cmbReviewStatus.DataBind();
            }
        }
        public Dictionary<String, String> UserType
        {
            set
            {
                cmbUserType.DataSource = value.OrderBy(x => x.Value); cmbUserType.DataBind();
            }
        }
        public Dictionary<String, String> SavedSearches
        {
            set
            {
                if (value.IsNotNull())
                {
                    cmbSavedReportSearches.DataSource = value.OrderBy(x => x.Value);
                    cmbSavedReportSearches.DataBind();
                    cmbSavedReportSearches.AddFirstEmptyItem();
                }
            }
        }

        public String SelectedSearch
        {
            get
            {
                return cmbSavedReportSearches.SelectedValue;
            }
            set
            {
                cmbSavedReportSearches.SelectedValue = value;
            }
        }

        public String SearchName
        {
            get
            {
                return txtSearchName.Text;
            }
            set
            {
                txtSearchName.Text = value;
            }
        }

        public String SearchDescription
        {
            get
            {
                return txtSearchDescription.Text;
            }
            set
            {
                txtSearchDescription.Text = value;
            }
        }


        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.Title = "Category Data Report With Complio ID";
            base.SetPageTitle("Category Data Report With Complio ID");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

                if (!Page.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }
                Presenter.OnViewLoaded();
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

        #region DropDown Events

        protected void cmbSavedSearches_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmbSavedReportSearches.SelectedValue != AppConsts.ZERO && !cmbSavedReportSearches.SelectedValue.IsNullOrEmpty())
                {
                    Presenter.FillSearchCriteria();
                }
                else
                {
                    txtSearchName.Text = String.Empty;
                    txtSearchDescription.Text = String.Empty;
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

        private void ToggleInstituteDataEnabledStatus(Boolean enabled = false)
        {
            cmbAgency.Enabled = enabled;
            cmbCategory.Enabled = enabled;
            cmbItem.Enabled = enabled;
            MainCommandBar.Enabled = enabled;
        }

        #endregion

        #region Grid Events


        #endregion

        #region Button Events

        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTenants_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.BindAgencies();
                cmbAgency.ClearCheckedItems();
                Presenter.BindCategories();
                Presenter.BindItems();
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
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgencies_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.BindCategories();
                cmbCategory.ClearCheckedItems();
                Presenter.BindItems();
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
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCategories_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.BindItems();
                cmbItem.ClearCheckedItems();
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
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            try
            {
                grdReportData.Rebind();
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
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearchSave_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.SaveSearchCriteria();
                base.ShowSuccessMessage("Search Criteria saved successfully.");
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
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetControls();
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

        private void ResetControls()
        {
            cmbInstitute.Items.ForEach(x => x.Checked = false);
            Presenter.BindAgencies();
            Presenter.BindCategories();
            Presenter.BindItems();
            Presenter.BindReviewStatusAndUserType();
            dpRotationStartDate.Clear();
            dpRotationEndDate.Clear();
            txtComplioID.Text = "";
            txtCustomAttribute.Text = "";
            grdReportData.Rebind();
        }


        #endregion

        protected void grdReportData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdReportData.DataSource = Presenter.GetCategoryDataReportWithComplioIDContracts();
        }
    }
}