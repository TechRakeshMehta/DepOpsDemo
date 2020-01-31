using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.SearchUI.Views
{
    public partial class SupportPortalNotes : BaseUserControl, ISupportPortalNotesView
    {
        #region Variables

        #region Private Variables

        private SupportPortalNotesPresenter _presenter = new SupportPortalNotesPresenter();
        private Boolean _isReloadData = false;
        //private String _viewType;
        //private Int32 tenantId = 0;
        //private List<Int32> _selectedTenantIds = null;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public SupportPortalNotesPresenter Presenter
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

        public ISupportPortalNotesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 ISupportPortalNotesView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        public Int32 ApplicantOrganizationUserID
        {
            get
            {
                if (!ViewState["ApplicantOrganizationUserID"].IsNull())
                {
                    return (Int32)ViewState["ApplicantOrganizationUserID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ApplicantOrganizationUserID"] = value;
            }
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (!ViewState["SelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["SelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        }

        public Entity.OrganizationUser OrganizationUser { get; set; }

        List<BkgOrderQueueNotesContract> ISupportPortalNotesView.lstBkgOrderNotes
        {
            get
            {
                if (!(ViewState["lstBkgOrderNotes"] is List<BkgOrderQueueNotesContract>))
                {
                    ViewState["lstBkgOrderNotes"] = new List<BkgOrderQueueNotesContract>();
                }
                return (List<BkgOrderQueueNotesContract>)ViewState["lstBkgOrderNotes"];
            }
            set
            {
                ViewState["lstBkgOrderNotes"] = value;
            }
        }

        String ISupportPortalNotesView.CurrentLoggedInUserName
        {
            get
            {
                if (ViewState["CurrentLoggedInUserName"].IsNotNull())
                {
                    return Convert.ToString(ViewState["CurrentLoggedInUserName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["CurrentLoggedInUserName"] = value;
            }
        }

        public List<Int32> lstOrderIds
        {
            get
            {
                if (!(ViewState["lstOrderIds"] is List<Int32>))
                {
                    ViewState["lstOrderIds"] = new List<Int32>();
                }
                return (List<Int32>)ViewState["lstOrderIds"];
            }
            set
            {
                ViewState["lstOrderIds"] = value;
            }
        }

        public List<String> lstOrderNumber
        {
            get
            {
                if (!(ViewState["lstOrderNumber"] is List<String>))
                {
                    ViewState["lstOrderNumber"] = new List<String>();
                }
                return (List<String>)ViewState["lstOrderNumber"];
            }
            set
            {
                ViewState["lstOrderNumber"] = value;
            }
        }

        public List<SupportPortalOrderDetailContract> lstOrder
        {
            get
            {
                if (!(ViewState["lstOrder"] is List<SupportPortalOrderDetailContract>))
                {
                    ViewState["lstOrder"] = new List<SupportPortalOrderDetailContract>();
                }
                return (List<SupportPortalOrderDetailContract>)ViewState["lstOrder"];
            }
            set
            {
                ViewState["lstOrder"] = value;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    Presenter.GetCurrentLoggedInUserName();
                    CaptureQuerystringParameters();
                }
                ucApplicantNotes.IsReadOnly = true;
                ucApplicantNotes.SelectedTenantId = CurrentViewContext.SelectedTenantID;
                ucApplicantNotes.ApplicantUserID = CurrentViewContext.ApplicantOrganizationUserID;
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


        protected void grdBkgOrderNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetSupportPortalBkgOrderNotes();
                grdBkgOrderNotes.DataSource = CurrentViewContext.lstBkgOrderNotes;
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


        protected void grdBkgOrderNotes_InsertCommand(object sender, GridCommandEventArgs e)
        {

            try
            {
                BkgOrderQueueNotesContract supportPortalBkgOrderNotes = null;
                String updatedNote = (e.Item.FindControl("txtNotes") as WclTextBox).Text.Trim();

                Int32 OrderID = 0;
                var Orderdetails = (e.Item.FindControl("cmbOrderNumber") as WclComboBox);

                if (!Orderdetails.IsNullOrEmpty() && !Orderdetails.SelectedValue.IsNullOrEmpty())
                {
                    OrderID = Convert.ToInt32((e.Item.FindControl("cmbOrderNumber") as WclComboBox).SelectedValue);

                    if (!Presenter.SaveSupportPortalBkgOrderNotes(OrderID, updatedNote))
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                        grdBkgOrderNotes.Rebind();
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Note saved successfully.");
                    }
                }
                //else
                //{
                //    Panel pnlSupportPortalNote = (e.Item.FindControl("pnlSupportPortalNote") as Panel);
                //    if (!pnlSupportPortalNote.IsNullOrEmpty())
                //    {
                //        e.Canceled = true;

                //        WclComboBox cmbOrderNumber = (WclComboBox)pnlSupportPortalNote.FindControl("cmbOrderNumber") as WclComboBox;
                //        cmbOrderNumber.Focus();
                //        RequiredFieldValidator rfvOrderID = (RequiredFieldValidator)pnlSupportPortalNote.FindControl("rfvOrderID");
                //        //rfvOrderID.InitialValue = "--SELECT--";
                //        rfvOrderID.ValidationGroup = "grpValdNotesForm";
                //        rfvOrderID.Enabled = true;
                //        rfvOrderID.Visible = true;
                //        rfvOrderID.Display = ValidatorDisplay.Dynamic;
                //        rfvOrderID.CssClass = "errmsg";
                //        rfvOrderID.ErrorMessage = "Order Number is required.";
                //    }

                //}
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

        protected void grdBkgOrderNotes_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                BkgOrderQueueNotesContract supportPortalDetailNotes = null;
                String updatedNote = (e.Item.FindControl("txtNotes") as WclTextBox).Text.Trim();
                Int32 supportPortalNotesNoteID = Convert.ToInt32((e.Item.FindControl("txtNotesID") as WclTextBox).Text);
                Int32 orderID = Convert.ToInt32((e.Item.FindControl("txtOrderID") as WclTextBox).Text);

                if (!Presenter.UpdateSupportPortalBkgOrderNotes(supportPortalNotesNoteID, updatedNote, orderID))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage("Some error has occurred. Please try again.");
                    grdBkgOrderNotes.Rebind();
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Note updated successfully.");
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

        protected void grdBkgOrderNotes_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                BkgOrderQueueNotesContract supportPortalDetailNotes = null;
                Int32 supportPortalBkgOrderNoteID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("NotesID"));

                if (!Presenter.DeleteSupportPortalBkgOrderNote(supportPortalBkgOrderNoteID))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage("Some error has occurred. Please try again.");
                    grdBkgOrderNotes.Rebind();
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Note deleted successfully.");
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

        protected void grdBkgOrderNotes_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox cmbOrderNumber = editform.FindControl("cmbOrderNumber") as WclComboBox;
                    //CurrentViewContext.lstOrderIds = new List<int>();
                    //CurrentViewContext.lstOrderIds.Add(6948);
                    if (!cmbOrderNumber.IsNullOrEmpty())
                    {
                        if (!CurrentViewContext.lstOrder.IsNullOrEmpty() && CurrentViewContext.lstOrder.Count > AppConsts.NONE)
                        {
                            cmbOrderNumber.DataSource = CurrentViewContext.lstOrder.DistinctBy(sel=>sel.OrderId);
                        }
                        else
                        {
                            cmbOrderNumber.DataSource = new List<AdminOrderSearchContract>();
                        }
                        cmbOrderNumber.DataBind();
                        cmbOrderNumber.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
                        cmbOrderNumber.Focus();
                    }

                    if (e.Item is GridEditFormItem && !(e.Item is GridEditFormInsertItem))
                    {
                        Int32 orderID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderID"]);
                        if (orderID > AppConsts.NONE)
                        {
                            cmbOrderNumber.SelectedValue = orderID.ToString();
                            cmbOrderNumber.Enabled = false;
                        }
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

        protected void grdBkgOrderNotes_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
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

        /// <summary>
        /// Get data from query string.
        /// </summary>
        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey("OrganizationUserId"))
                {
                    CurrentViewContext.ApplicantOrganizationUserID = Convert.ToInt32(args["OrganizationUserId"]);
                }
                if (args.ContainsKey("TenantId"))
                {
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(args["TenantId"]);
                }
            }
        }
        #endregion

    }
}