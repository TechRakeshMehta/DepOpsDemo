using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageMasterServiceItemRule : BaseUserControl, IManageMasterStateSearchView
    {
        #region VARIABLES

        #region PRIVATE VARIABLES
        private ManageMasterStateSearchPresenter _presenter = new ManageMasterStateSearchPresenter();
        #endregion

        #region PUBLIC VARIABLES
        #endregion

        #endregion

        #region PROPERTIES 
        
        #region PRIVATE PROPERTIES
        #endregion

        #region PUBLIC PROPERTIES

        public ManageMasterStateSearchPresenter Presenter
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

        public Int32 DefaultTenantId
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

        public Int32 CurrentLoggedInUserId
        {
            get 
            { 
                return SysXWebSiteUtils.SessionService.OrganizationUserId; 
            }
        }

        public String ErrorMessage { get; set; }

        public List<Entity.State> lstState { get; set; }

        public List<BkgPackageStateSearchContract> lstStateSearchContract
        {
            get
            {
                List<BkgPackageStateSearchContract> lstStateSearchContract = new List<BkgPackageStateSearchContract>();
                if (ViewState["lstStateSearchContract"].IsNotNull())
                {
                    lstStateSearchContract = (List<BkgPackageStateSearchContract>)ViewState["lstStateSearchContract"];
                }
                return lstStateSearchContract;
            }
            set
            {
                ViewState["lstStateSearchContract"] = value;
            }
        }

        public Boolean IsStateSearchChecked { get; set; }

        public Boolean IsCountySearchChecked { get; set; }

        public Int32 StateID { get; set; }

        public List<Entity.BkgMasterStateSearch> lstMasterStateSearch
        {
            get
            {
                return (List<Entity.BkgMasterStateSearch>)ViewState["lstMasterStateSearch"];
            }
            set
            {
                ViewState["lstMasterStateSearch"] = value;
            }
        }
        #endregion
        #endregion

        #region EVENTS

        #region PAGE EVENTS

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Master State Search Critera";
                base.SetPageTitle("Manage Master State Search Criteria");

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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Presenter.OnViewInitialized();
                Presenter.GetMasterStateSearchCriteria();
                BindStates(); 
            }
        }

        #endregion

        #region REPEATER EVENTS
        protected void rptStateCounty_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField stateID = (HiddenField)e.Item.FindControl("hdnStateID");
            if (stateID.IsNotNull() && Convert.ToInt32(stateID.Value) > 0)
            {
                CheckBox chkStateSearch = (CheckBox)e.Item.FindControl("chkStateSearch");
                CheckBox chkCountySearch = (CheckBox)e.Item.FindControl("chkCountySearch");
                if (lstMasterStateSearch.IsNotNull() && lstMasterStateSearch.Count > 0)
                {
                    chkStateSearch.Checked = lstMasterStateSearch.Where(x => x.BMSS_StateID == Convert.ToInt32(stateID.Value)).Select(x=>x.BMSS_IsStateSearch).FirstOrDefault().IsNotNull() ?
                        Convert.ToBoolean(lstMasterStateSearch.Where(x => x.BMSS_StateID == Convert.ToInt32(stateID.Value)).Select(x => x.BMSS_IsStateSearch).FirstOrDefault()) : false;

                    chkCountySearch.Checked = lstMasterStateSearch.Where(x => x.BMSS_StateID == Convert.ToInt32(stateID.Value)).Select(x => x.BMSS_IsCountySearch).FirstOrDefault().IsNotNull() ?
                        Convert.ToBoolean(lstMasterStateSearch.Where(x => x.BMSS_StateID == Convert.ToInt32(stateID.Value)).Select(x => x.BMSS_IsCountySearch).FirstOrDefault()) : false;
                }
            }
        }

        protected void rptStateCounty_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        #endregion

        #region BUTTON EVENTS

        protected void btnEditMasterStateSearchCriteria_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptStateCounty.Items)
            {
                CheckBox chkStateSearch = (CheckBox)item.FindControl("chkStateSearch");
                chkStateSearch.Enabled = true;

                CheckBox chkCountySearch = (CheckBox)item.FindControl("chkCountySearch");
                chkCountySearch.Enabled = true;

                //CheckBox chkAllState = (CheckBox)item.FindControl("chkAllState");
                //chkAllState.Enabled = true;

                //CheckBox chkAllCounty = (CheckBox)item.FindControl("chkAllCounty");
                //chkAllCounty.Enabled = true;
            }
            (rptStateCounty.Controls[0].Controls[0].FindControl("chkAllState") as CheckBox).Enabled = true;
            (rptStateCounty.Controls[0].Controls[0].FindControl("chkAllCounty") as CheckBox).Enabled = true;
            btnSaveStateSearchCriteria.Visible = true;
            btnCancelStateSearchCriteria.Visible = true;
        }

        protected void btnSaveStateSearchCriteria_Click(object sender, EventArgs e)
        {
            List<BkgPackageStateSearchContract> lstStateSearchContractObj = new List<BkgPackageStateSearchContract>();
            foreach (RepeaterItem item in rptStateCounty.Items)
            {
                HiddenField hdnStateID = (HiddenField)item.FindControl("hdnStateID");
                CheckBox chkStateSearch = (CheckBox)item.FindControl("chkStateSearch");
                CheckBox chkCountySearch = (CheckBox)item.FindControl("chkCountySearch");

                BkgPackageStateSearchContract stateSearchContractItem = new BkgPackageStateSearchContract();
                //stateSearchContractItem.BkgPackageID = packageId;
                stateSearchContractItem.StateID = Convert.ToInt32(hdnStateID.Value);
                stateSearchContractItem.IsStateSearchChecked = chkStateSearch.Checked;
                stateSearchContractItem.IsCountySearchChecked = chkCountySearch.Checked;
                lstStateSearchContractObj.Add(stateSearchContractItem);
                lstStateSearchContract = lstStateSearchContractObj;
            }
            Presenter.SaveMasterStateSearchCriteria();
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Master state search criteria updated successfully.");
                ResetStateCountyRepeater();
            }
        }

        protected void btnCancelStateSearchCriteria_Click(object sender, EventArgs e)
        {
            Presenter.GetMasterStateSearchCriteria();
            BindStates();
            ResetStateCountyRepeater();
        }
        
        #endregion

        #endregion

        #region METHODS

        #region PRIVATE METHODS

        private void BindStates()
        {
            Presenter.GetStateList();
            rptStateCounty.DataSource = lstState;
            rptStateCounty.DataBind();
        }

        #endregion

        #region PUBLIC METHODS

        public void ResetStateCountyRepeater()
        {
            foreach (RepeaterItem item in rptStateCounty.Items)
            {
                CheckBox chkStateSearch = (CheckBox)item.FindControl("chkStateSearch");
                chkStateSearch.Enabled = false;

                CheckBox chkCountySearch = (CheckBox)item.FindControl("chkCountySearch");
                chkCountySearch.Enabled = false;
            }
            (rptStateCounty.Controls[0].Controls[0].FindControl("chkAllState") as CheckBox).Enabled = false;
            (rptStateCounty.Controls[0].Controls[0].FindControl("chkAllCounty") as CheckBox).Enabled = false;
            btnSaveStateSearchCriteria.Visible = false;
            btnCancelStateSearchCriteria.Visible = false;
        }
        
        #endregion

        #endregion




    }
}