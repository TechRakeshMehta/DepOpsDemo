using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public partial class RequirementRotations : BaseUserControl, IRequirementRotation
    {
        #region Private Variables

        private String _viewType;

        private List<ClientContactContract> _lstClientContacts = new List<ClientContactContract>();
        private RequirementRotationPresenter _presenter = new RequirementRotationPresenter();
        private List<AgencyDetailContract> _lstAgencies = new List<AgencyDetailContract>();
        private ClinicalRotationDetailContract _searchContract = new ClinicalRotationDetailContract();
        private List<ClinicalRotationDetailContract> _lstRotations = new List<ClinicalRotationDetailContract>();

        #endregion

        #region Properties

        #region Private Properties

        private RequirementRotationPresenter Presenter
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

        #region Public Properties

        /// <summary>
        /// Represents the Current View Context
        /// </summary>
        public IRequirementRotation CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        ///  Tenant-Id of the logged-in Applicant
        /// </summary>
        public Int32 TenantId
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
        /// List of the Agencies that belong to the Current Applicants' Tenant
        /// </summary>
        List<AgencyDetailContract> IRequirementRotation.lstAgency
        {
            get
            {
                return _lstAgencies;
            }
            set
            {
                _lstAgencies = value;
                _lstAgencies.Insert(AppConsts.NONE, new AgencyDetailContract
                {
                    AgencyID = AppConsts.NONE,
                    AgencyName = AppConsts.COMBOBOX_ITEM_SELECT
                });
                cmbAgency.DataSource = _lstAgencies;
                cmbAgency.DataBind();
            }
        }

        /// <summary>
        /// List of the ClientContacts that belong to the Current Applicants' Tenant
        /// </summary>
        List<ClientContactContract> IRequirementRotation.lstClientContacts
        {
            get
            {
                return _lstClientContacts;
            }
            set
            {
                _lstClientContacts = value;
                _lstClientContacts.Insert(AppConsts.NONE, new ClientContactContract
                {
                    ClientContactID = AppConsts.NONE,
                    Name = AppConsts.COMBOBOX_ITEM_SELECT
                });
                cmbClientContacts.DataSource = _lstClientContacts;
                cmbClientContacts.DataBind();
            }
        }

        /// <summary>
        /// ID of the Agency selected for Search
        /// </summary>
        Int32 IRequirementRotation.SelectedAgencyId
        {
            get
            {
                var _selectedValue = cmbAgency.SelectedValue;
                if (!String.IsNullOrEmpty(_selectedValue))
                {
                    return Convert.ToInt32(_selectedValue);
                }
                else
                {
                    return AppConsts.NONE;
                }
            }
            set
            {
                if (cmbAgency.Items.Count > AppConsts.NONE)
                {
                    cmbAgency.SelectedValue = Convert.ToString(value);
                }
            }
        }

        /// <summary>
        /// ID of the ClientContact, selected for Search
        /// </summary>
        String IRequirementRotation.ClientContactId
        {
            get
            {
                var _selectedValue = cmbClientContacts.SelectedValue;
                if (!String.IsNullOrEmpty(_selectedValue) && _selectedValue != AppConsts.ZERO)
                {
                    return _selectedValue;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                if (cmbClientContacts.Items.Count > AppConsts.NONE)
                {
                    cmbClientContacts.SelectedValue = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        String IRequirementRotation.StatusTypeCode
        {
            get
            {
                return rbtnListStatus.SelectedValue;
            }
            set
            {
                rbtnListStatus.SelectedValue = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ClinicalRotationDetailContract IRequirementRotation.SearchContract
        {
            get
            {
                //GetSearchParameters();
                ClinicalRotationDetailContract searchData = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_KEY_CLINICAL_ROTATION_SEARCH_DATA) as ClinicalRotationDetailContract;
                if (searchData.IsNotNull())
                {
                    return searchData;
                }
                return _searchContract;
            }
            //set
            //{
            //    Session["SearchContract"] = value;
            //    // _searchContract = value;
            //}
        }

        /// <summary>
        /// Represents the list of Rotations oto which the applicant belongs to.
        /// </summary>
        List<ClinicalRotationDetailContract> IRequirementRotation.lstApplicantRotations
        {
            set
            {
                _lstRotations = value;
            }
            get
            {
                return _lstRotations;
            }
        }

        public bool IsClinicalRotationClicked
        {
            get;
            set;
        }

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            BindAgencies();
            BindClientContacts();

            //IsClinicalRotationClicked true if user click on clinical rotation tab.In that case clear the sassion data.
            if (IsClinicalRotationClicked)
            {
                ClearSession();
            }
            else
            {
                SetPropertyFromSession();
            }
        }

        /// <summary>
        /// Method to Bind Agencies
        /// </summary>
        private void BindAgencies()
        {
            Presenter.GetAgencies();
        }

        /// <summary>
        /// Method to Bind ClientContacts
        /// </summary>
        private void BindClientContacts()
        {
            Presenter.GetClientContacts();
        }

        protected void grdRequirementRotations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetClinicalRotations();
                grdRequirementRotations.DataSource = CurrentViewContext.lstApplicantRotations;
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
        /// Get search parameters
        /// </summary>
        private void GetSearchParameters()
        {
            _searchContract.AgencyID = CurrentViewContext.SelectedAgencyId;
            _searchContract.TenantID = CurrentViewContext.TenantId;
            _searchContract.ContactIdList = CurrentViewContext.ClientContactId;
            _searchContract.StatusTypeCode = CurrentViewContext.StatusTypeCode;

            if (!txtDepartment.Text.Trim().IsNullOrEmpty())
                _searchContract.Department = txtDepartment.Text.Trim();

            if (!txtCourse.Text.Trim().IsNullOrEmpty())
                _searchContract.Course = txtCourse.Text.Trim();

            if (!txtProgram.Text.Trim().Trim().IsNullOrEmpty())
                _searchContract.Program = txtProgram.Text.Trim();

            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_CLINICAL_ROTATION_SEARCH_DATA, _searchContract);
            //CurrentViewContext.SearchContract = _searchContract;
        }

        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //ClearSession();
                GetSearchParameters();
                ResetGridFilters();
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

        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            cmbAgency.SelectedValue = AppConsts.ZERO;
            cmbClientContacts.SelectedValue = AppConsts.ZERO;
            txtCourse.Text = txtProgram.Text = txtDepartment.Text = String.Empty;
            rbtnListStatus.SelectedValue = "all";
            ClearSession();
            ResetGridFilters();
        }

        private void ResetGridFilters()
        {
            grdRequirementRotations.DataSource = null;
            grdRequirementRotations.Rebind();
        }

        protected void grdRequirementRotations_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    ClearSession();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    var _rotationId = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationId"]);
                    var _pkgSubId = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PkgSubscriptionId"]);
                    String menuId = "10";
                    Response.Redirect(AppConsts.DASHBOARD_URL + "?MenuId=" + menuId + "&ReqPkgSubscriptionId=" + _pkgSubId + "&ClinicalRotationId=" + _rotationId, true);
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

        #region Private Methods
        /// <summary>
        /// This method used to clear the session data.
        /// </summary>
        private void ClearSession()
        {
            Session.Remove(AppConsts.SESSION_KEY_CLINICAL_ROTATION_SEARCH_DATA);
        }

        private void SetPropertyFromSession()
        {
            ClinicalRotationDetailContract searchData = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_KEY_CLINICAL_ROTATION_SEARCH_DATA) as ClinicalRotationDetailContract;
            if (searchData.IsNotNull())
            {
                CurrentViewContext.SelectedAgencyId = searchData.AgencyID;
                CurrentViewContext.ClientContactId = searchData.ContactIdList;
                CurrentViewContext.StatusTypeCode = searchData.StatusTypeCode;
            }
        }
        #endregion
    }
}