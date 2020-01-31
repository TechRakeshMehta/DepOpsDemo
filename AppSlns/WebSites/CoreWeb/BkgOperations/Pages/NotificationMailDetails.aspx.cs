using System;
using System.Web.UI;
using Entity;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public partial class NotificationMailDetails : BaseWebPage, INotificationMailDetailsView
    {
        #region Variables

        private NotificationMailDetailsPresenter _presenter = new NotificationMailDetailsPresenter();

        #endregion

        #region Properties

        Int32 INotificationMailDetailsView.SystemCommunicationID
        {
            get
            {
                if (!Request.QueryString["SystemCommunicationID"].IsNull())
                {
                    return Convert.ToInt32(Request.QueryString["SystemCommunicationID"]);
                }
                return 0;
            }
        }

        public SystemCommunication SystemCommunicationDetail { get; set; }

        private NotificationMailDetailsPresenter Presenter
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

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Notification Mail Details";
            if (!Page.IsPostBack)
            {
                Presenter.GetSystemCommunicationForMailData();
            }
            BindMailData();
        }

        #endregion

        #endregion

        #region methods

        private void BindMailData()
        {
            if (SystemCommunicationDetail.IsNotNull())
            {
                divSenderName.InnerHtml = SystemCommunicationDetail.SenderName;
                divSenderEmailID.InnerHtml = SystemCommunicationDetail.SenderEmailID;
                divSubject.InnerHtml = SystemCommunicationDetail.Subject;
                divBody.InnerHtml = SystemCommunicationDetail.Content;
            }

        }

        #endregion
    }
}