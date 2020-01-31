using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class AgencyLocationDepartment : BaseUserControl, IAgencyLocationDepartmentView
    {

        #region Variables

        #region Private Variables
        private AgencyLocationDepartmentPresenter _presenter = new AgencyLocationDepartmentPresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private IAgencyLocationDepartmentView CurrentViewContext
        {
            get { return this; }
        }

        #endregion

        #region Public Properties

        public AgencyLocationDepartmentPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        Guid IAgencyLocationDepartmentView.UserId
        {
            get
            {
                return base.SysXMembershipUser.UserId;
            }
        }

        Int32 IAgencyLocationDepartmentView.AgencyRootNodeID
        {
            get
            {
                if (!ViewState["AgencyRootNodeID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["AgencyRootNodeID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AgencyRootNodeID"] = value;
            }
        }

        String IAgencyLocationDepartmentView.AgencyRootNode
        {
            get
            {
                if (!ViewState["AgencyRootNode"].IsNullOrEmpty())
                    return ViewState["AgencyRootNode"].ToString();
                return String.Empty;
            }
            set
            {
                ViewState["AgencyRootNode"] = value;
            }
        }

        List<AgencyLocationDepartmentContract> IAgencyLocationDepartmentView.lstAgencyLocations
        {
            get
            {
                if (!ViewState["DicAgencyLocations"].IsNullOrEmpty())
                    return (List<AgencyLocationDepartmentContract>)(ViewState["DicAgencyLocations"]);
                return new List<AgencyLocationDepartmentContract>();
            }
            set
            {
                ViewState["DicAgencyLocations"] = value;
            }
        }

        Int32 IAgencyLocationDepartmentView.AgencyLocationID
        {
            get
            {
                if (!ViewState["AgencyLocationID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["AgencyLocationID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AgencyLocationID"] = value;
            }
        }


        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Agency Location Setup";
                base.SetPageTitle("Agency Location Setup");
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
            try
            {
                if (!this.IsPostBack)
                {
                    //SetAgencyLocationHiearchy();
                    SetAgencyRootNode();
                    ifrDetails.Src = GetIframeSrcUrl(false);
                   
                }
                BindRepeater();
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

        #endregion

        #region Button Events

        protected void btnLocation_ServerClick(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.Control btnLocation = ((System.Web.UI.Control)(sender));
                RepeaterItem item = (RepeaterItem)(btnLocation.NamingContainer);
                HiddenField hdnLocationId = item.FindControl("hdnLocationId") as HiddenField;
                CurrentViewContext.AgencyLocationID = hdnLocationId.Value.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(hdnLocationId.Value);
                ifrDetails.Src = GetIframeSrcUrl(true);
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

        protected void btnAgencyRootNode_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ifrDetails.Src = GetIframeSrcUrl(false);
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

        #region Methods

        #region Private Methods


        private void SetAgencyRootNode()
        {
            GetAgencyRootNode();
            hdnAgencyRootNode.Value = Convert.ToString(!CurrentViewContext.AgencyRootNodeID.IsNullOrEmpty() ? CurrentViewContext.AgencyRootNodeID : AppConsts.NONE);
            btnAgencyRootNode.InnerText = !CurrentViewContext.AgencyRootNode.IsNullOrEmpty() ? CurrentViewContext.AgencyRootNode : String.Empty;
        }

        private void BindRepeater()
        {
            Presenter.GetAgencyLocations();
            rptrAgencyLocation.DataSource = CurrentViewContext.lstAgencyLocations;
            rptrAgencyLocation.DataBind();
        }
       
        private void GetAgencyRootNode()
        {
            Presenter.GetAgencyRootNode();
        }

        private String GetIframeSrcUrl(Boolean isLocationClick)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                        {
                                            {"AgencyRootNodeID",CurrentViewContext.AgencyRootNodeID.ToString()},
                                            {"IsLocationClick", isLocationClick.ToString()},
                                            {"AgencyLocationID", CurrentViewContext.AgencyLocationID.ToString()},
                                        };
            return String.Format("~/PlacementMatching/Pages/AgencyLocationDepartmentDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}