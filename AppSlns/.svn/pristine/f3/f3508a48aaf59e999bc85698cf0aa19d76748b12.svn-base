using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgNotes : BaseUserControl, IBkgNoteView
    {
        private String _viewType;
        private BkgNotePresenter _presenter = new BkgNotePresenter();

        public IBkgNoteView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public BkgNotePresenter Presenter
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

        Int32 IBkgNoteView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IBkgNoteView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        Int32 IBkgNoteView.OrderID
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
            }
        }

        /// <summary>
        /// Gets and sets the Address1
        /// </summary>
        String IBkgNoteView.NewNote
        {
            get
            {
                return txtNewNote.Text.Trim();
            }
            set
            {
                txtNewNote.Text = value;
            }
        }

        List<BkgOrderQueueNotesContract> IBkgNoteView.LstNotes { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    ApplyActionLevelPermission(ActionCollection, "Bkg Notes");
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
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Notes";
                base.OnInit(e);
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
        protected void grdNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {         
            //grdNotes.DataSource = Presenter.GetNotesByOrderId();
            Presenter.GetBkgOrderNote();
            grdNotes.DataSource = CurrentViewContext.LstNotes.IsNullOrEmpty() ? new List<BkgOrderQueueNotesContract>() : CurrentViewContext.LstNotes;
        }

        protected void CmdBarCancel_Click(Object sender, EventArgs e)
        {
            //Boolean status = Presenter.AddNote();
            Boolean status = Presenter.SaveBkgOrderNote();
            grdNotes.Rebind();
            if (status)
            {
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage("Note Added successfully.", MessageType.SuccessMessage);
            }
            else
            {
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage("Some error has occured while updating the order.", MessageType.Error);
            }
        }

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    ControlActionId = "Add",
                    ControlActionLabel = "Add Note",
                    SystemControl = btnAdd,
                    ScreenName = "Bkg Notes"
                });
                return actionCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
        }
       
    }
}