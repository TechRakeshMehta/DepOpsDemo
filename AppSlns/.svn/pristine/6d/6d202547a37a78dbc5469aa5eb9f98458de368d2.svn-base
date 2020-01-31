using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.PackageBundleManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.CommonOperations.Views
{
    public partial class Announcement : BaseUserControl, IAnnouncementView
    {
        #region Private Variables
        private AnnouncementPresenter _presenter = new AnnouncementPresenter();
        private String _viewType;
        private AnnouncementContract _viewContract = null;
        private Int32 tenantId = 0;

        #endregion

        #region Properties

        #region Public Properties

        public AnnouncementPresenter Presenter
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

        public IAnnouncementView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IAnnouncementView.ErrorMessage
        {

            get;
            set;
        }

        String IAnnouncementView.SuccessMessage
        {
            get;
            set;
        }

        Int32 IAnnouncementView.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }

        Boolean IAnnouncementView.IsAdminLoggedIn
        {
            get;
            set;
        }

        List<AnnouncementContract> IAnnouncementView.AnnouncementDetails
        {
            get;
            set;
        }

        AnnouncementContract IAnnouncementView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new AnnouncementContract();
                }
                return _viewContract;
            }
        }

        #endregion
        #endregion

        #region Events

        #region Page Events

        /// <summary>
        ///  set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdAnnouncement.WclGridDataObject = new GridObjectDataContainer();                
                ((GridObjectDataContainer)(grdAnnouncement.WclGridDataObject)).ColumnsToSkipEncoding.Add("AnnouncementText");
                base.OnInit(e);
                base.Title = "Announcement";
                base.SetPageTitle("Announcement");
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
                //Set Module Title
                BasePage basePage = base.Page as BasePage;
                if (basePage != null)
                {
                    basePage.SetModuleTitle("Manage Announcements");
                }

                if (!this.IsPostBack)
                {

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

        #region Grid Events

        protected void grdAnnouncement_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAnnouncementDetail();
                grdAnnouncement.DataSource = CurrentViewContext.AnnouncementDetails;
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

        protected void grdAnnouncement_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);

                    WclTextBox txtAnnouncementName = (e.Item.FindControl("txtAnnouncementName") as WclTextBox);
                    WclEditor rdEditorNotes = (e.Item.FindControl("rdEditorNotes") as WclEditor);

                    AnnouncementContract announcementContract = e.Item.DataItem as AnnouncementContract;

                    if (announcementContract != null)
                    {
                        txtAnnouncementName.Text = announcementContract.AnnouncementName;
                        rdEditorNotes.Content = announcementContract.AnnouncementText;
                    }
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

        protected void grdAnnouncement_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.ViewContract.AnnouncementID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AnnouncementID"]);
                    }

                    CurrentViewContext.ViewContract.AnnouncementName = (e.Item.FindControl("txtAnnouncementName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.AnnouncementText = (e.Item.FindControl("rdEditorNotes") as WclEditor).Content.Trim();

                    if (Presenter.SaveUpdateAnnouncement())
                    {
                        if (e.CommandName == RadGrid.UpdateCommandName)
                        {
                            base.ShowSuccessMessage("Announcement updated successfully.");
                        }
                        else
                        {
                            base.ShowSuccessMessage("Announcement saved successfully.");
                        }
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowErrorMessage("some error has occured due to which Announcement can not be saved.");
                    }
                }

                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.ViewContract.AnnouncementID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AnnouncementID"]);
                    Presenter.DeleteAnnouncement();
                    base.ShowSuccessMessage("Announcement deleted successfully.");
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

        #region Button Events

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        #endregion
        #endregion
    }
}