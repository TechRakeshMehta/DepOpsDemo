using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class AgencyLocationDepartmentDetails : BaseWebPage, IAgencyLocationDepartmentDetailsView
    {
        #region Variables
        private AgencyLocationDepartmentDetailsPresenter _presenter = new AgencyLocationDepartmentDetailsPresenter();
        #endregion

        #region Properties

        public AgencyLocationDepartmentDetailsPresenter Presenter
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

        private IAgencyLocationDepartmentDetailsView CurrentViewContext
        {
            get { return this; }
        }

        Boolean IAgencyLocationDepartmentDetailsView.IsLocationClick
        {
            get;
            set;
        }

        Int32 IAgencyLocationDepartmentDetailsView.AgencyRootNodeID
        {
            get;
            set;
        }

        Int32 IAgencyLocationDepartmentDetailsView.AgencyLocationID
        {
            get;
            set;
        }

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ucLocationDepartment.eventShowMessage += new LocationDepartment.ShowMessageHandler(ShowMessage);
                ucAgencyLocation.eventShowMessage += new AgencyLocation.ShowMessageHandler(ShowMessage);
                if (!this.IsPostBack)
                {
                    CaptureQueryString();
                    ManageUserControlsVisibility();
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

        #endregion

        #region Methods

        private void CaptureQueryString()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey("IsLocationClick"))
                {
                    CurrentViewContext.IsLocationClick = Convert.ToBoolean(args["IsLocationClick"]);
                }
                if (args.ContainsKey("AgencyRootNodeID"))
                {
                    CurrentViewContext.AgencyRootNodeID = Convert.ToInt32(args["AgencyRootNodeID"]);
                }
                if (args.ContainsKey("AgencyLocationID"))
                {
                    CurrentViewContext.AgencyLocationID = Convert.ToInt32(args["AgencyLocationID"]);
                }
            }
        }

        private void ManageUserControlsVisibility()
        {
            if (!CurrentViewContext.IsLocationClick)
            {
                ucAgencyLocation.Visible = true;
                ucAgencyLocation.AgencyRootNodeID = CurrentViewContext.AgencyRootNodeID;
                ucLocationDepartment.Visible = false;
            }
            else
            {
                ucLocationDepartment.AgencyLocationID = CurrentViewContext.AgencyLocationID;
                ucLocationDepartment.Visible = true;
                ucAgencyLocation.Visible = false;
            }
        }

        Boolean ShowMessage(object sender, StatusMessages msgType, String message)
        {
            if (!message.IsNullOrEmpty() && message.ToLower().Contains(AppConsts.HTML_XSS_INJECTION_ERROR_MSG))
            {
                message = Resources.Language.LOGINEXCEPTIONCSSANDHTML;
            }

            if (String.Compare(msgType.GetStringValue().ToLower(), StatusMessages.SUCCESS_MESSAGE.GetStringValue().ToLower()) == 0)
            {
                base.ShowSuccessMessage(message);
            }
            else if (String.Compare(msgType.GetStringValue().ToLower(), StatusMessages.INFO_MESSAGE.GetStringValue().ToLower()) == 0)
            {
                base.ShowInfoMessage(message);
            }
            else if (String.Compare(msgType.GetStringValue().ToLower(), StatusMessages.ERROR_MESSAGE.GetStringValue().ToLower()) == 0)
            {
                base.ShowErrorMessage(message);
            }
            return true;
        }

        #endregion

    }
}