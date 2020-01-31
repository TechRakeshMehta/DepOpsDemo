using System;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System.Collections.Generic;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ExceptionUserQueue : System.Web.UI.UserControl, IExceptionUserQueueView
    {
        #region Privare Variables

        private ExceptionUserQueuePresenter _presenter = new ExceptionUserQueuePresenter();
        private Int32 _tenantid;
        private String _tenantTypeCode = String.Empty;

        #endregion

        #region Public Properties

        public ExceptionUserQueuePresenter Presenter
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

        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        public String TenantTypeCode
        {
            get
            {
                if (_tenantTypeCode == String.Empty)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantTypeCode = user.TenantTypeCode;
                    }
                }
                return _tenantTypeCode;
            }
            set
            {
                _tenantTypeCode = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Page_Load event
        /// Redirect to UserExceptionDataQueue.ascx page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            GetUrl();
        }

        /// <summary>
        /// To get Queue URL
        /// </summary>
        public void GetUrl()
        {
            String _viewType = String.Empty;
            //Int32 queueID = Presenter.GetQueueID();
            String queueCode = Presenter.GetQueueCode();
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    //{ "QID", Convert.ToString( queueID) },
                                                                    { "Child",@"~\ComplianceOperations\UserExceptionDataQueue.ascx"},
                                                                    { "QCODE", queueCode}
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        #endregion
    }
}