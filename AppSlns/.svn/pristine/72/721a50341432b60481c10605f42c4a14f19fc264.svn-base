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
    public partial class DataEntryAssignmentQueue : BaseUserControl, IDataEntryAssignmentQueueView
    {
        private DataEntryAssignmentQueuePresenter _presenter = new DataEntryAssignmentQueuePresenter();

        #region Properties
        public DataEntryAssignmentQueuePresenter Presenter
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

        Int32 IDataEntryAssignmentQueueView.TenantId
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
        #endregion

        #region Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                String title = "Pending Documents Assignment Queue";
                base.Title = title;
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
                String title = "Pending Documents Assignment Queue";
                base.SetPageTitle(title);
                if (Presenter.IsDefaultTenant)
                {
                    ucDataEntryQueue.QueueType = DataEntryQueueType.DATA_ENTRY_ASSIGNMENT_QUEUE.GetStringValue();
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

        #region Private Methods
        ///// <summary>
        ///// To get Queue URL
        ///// </summary>
        //private void GetUrl()
        //{
        //    String _viewType = String.Empty;
        //    String queueCode = DataEntryQueueType.DATA_ENTRY_ASSIGNMENT_QUEUE.GetStringValue();
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
    }
}