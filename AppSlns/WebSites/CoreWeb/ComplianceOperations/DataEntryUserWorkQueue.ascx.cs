#region Namespace

#region System Defined
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;
#endregion

#region Project Specific
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.Shell;
using Entity.ClientEntity;
using CoreWeb.IntsofSecurityModel;
using WebSiteUtils.SharedObjects;
using Business.RepoManagers;
using INTERSOFT.WEB.UI.WebControls;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DataEntryUserWorkQueue : BaseUserControl, IDataEntryUserWorkQueueView
    {
        private DataEntryUserWorkQueuePresenter _presenter = new DataEntryUserWorkQueuePresenter();
        #region Properties
        Int32 IDataEntryUserWorkQueueView.TenantId
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    return user.TenantId.HasValue ? user.TenantId.Value : AppConsts.NONE;
                }
                return AppConsts.NONE;
            }
        }

        public DataEntryUserWorkQueuePresenter Presenter
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
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                String title = "Pending Document User Work Queue";
                base.Title = title;
                base.SetPageTitle(title);
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
                String title = "Pending Document User Work Queue";
                base.SetPageTitle(title);
                if (Presenter.IsDefaultTenant)
                {
                    ucDataEntryQueue.QueueType = DataEntryQueueType.DATA_ENTRY_USER_WORK_QUEUE.GetStringValue();
                    dvDataEntryQueue.Visible = true;
                }
                else
                {
                    base.ShowInfoMessage("You do not have permission to access this feature.");
                    dvDataEntryQueue.Visible = false;
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

        #region Methods

        #region Private Methods
        ///// <summary>
        ///// To get Queue URL
        ///// </summary>
        //private void GetUrl()
        //{
        //    String _viewType = String.Empty;
        //    String queueCode = DataEntryQueueType.DATA_ENTRY_USER_WORK_QUEUE.GetStringValue();
        //    Dictionary<String, String> queryString = new Dictionary<String, String>
        //                                                         { 
        //                                                            //{ "QID", Convert.ToString( queueID) },
        //                                                            { "Child",@"~\ComplianceOperations\UserControl\DataEntryQueue.ascx"},
        //                                                            { "QCODE", queueCode}
        //                                                         };
        //    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
        //    Response.Redirect(url, true);
        //}
        #endregion
        #endregion
    }
}