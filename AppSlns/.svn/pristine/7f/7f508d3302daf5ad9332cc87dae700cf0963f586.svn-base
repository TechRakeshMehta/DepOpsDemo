using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.BkgOperations.Views
{
    public partial class ChangeBkgOrderColorStatus : BaseWebPage, IChangeBkgOrderColorStatusView
    {
        #region Private variables
        private ChangeBkgOrderColorStatusPresenter _presenter = new ChangeBkgOrderColorStatusPresenter();
        #endregion

        #region Public Properties

        public ChangeBkgOrderColorStatusPresenter Presenter
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

        public IChangeBkgOrderColorStatusView CurrentViewContext
        {
            get { return this; }
        }

        Int32 IChangeBkgOrderColorStatusView.OrderID
        {
            get
            {
                if (ViewState["OrderID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["OrderID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OrderID"] = value;
            }
        }

        Int32 IChangeBkgOrderColorStatusView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IChangeBkgOrderColorStatusView.TenantID
        {
            get
            {
                if (ViewState["TenantId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IChangeBkgOrderColorStatusView.SelectedColorFlag
        {
            get
            {
                if (!rcbInstitutionStatusColorIcons.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(rcbInstitutionStatusColorIcons.SelectedValue);
                return AppConsts.NONE;
            }
            set
            {
                if (!rcbInstitutionStatusColorIcons.Items.IsNullOrEmpty() && rcbInstitutionStatusColorIcons.Items.Any(x => x.Value == Convert.ToString(value)))
                {
                    rcbInstitutionStatusColorIcons.SelectedValue = Convert.ToString(value);
                }
            }
        }

        public Boolean IsEditMode
        {
            get
            {
                if (ViewState["IsEditMode"].IsNotNull())
                {
                    return Convert.ToBoolean(ViewState["IsEditMode"]);
                }
                return false;
            }
            set
            {
                ViewState["IsEditMode"] = value;
            }
        }
        #endregion

        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (!Request.QueryString["OrderID"].IsNull())
                    {
                        CurrentViewContext.OrderID = Convert.ToInt32(Request.QueryString["OrderID"]);
                    }

                    if (!Request.QueryString["TenantId"].IsNull())
                    {
                        CurrentViewContext.TenantID = Convert.ToInt32(Request.QueryString["TenantId"]);
                    }
                    BindDropDown();
                    if (!Request.QueryString["InstitutionColorStatusID"].IsNull())
                    {
                        CurrentViewContext.SelectedColorFlag = Convert.ToInt32(Request.QueryString["InstitutionColorStatusID"]);
                    }
                    if (CurrentViewContext.SelectedColorFlag > AppConsts.NONE)
                    {
                        Page.Title = "Update Background Order Color Flag";
                        IsEditMode = true;
                    }
                    else
                    {
                        Page.Title = "Add Background Order Color Flag";
                        IsEditMode = false;
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

        #endregion

        #region Button Events
        protected void fsucFeatureActionList_SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.UpdateBkgOrderColorFlag())
                {
                    hdnSaveStatus.Value = "True";
                    if (IsEditMode)
                    {
                        base.ShowSuccessMessage("Background order color flag updated successfully.");
                    }
                    else
                    {
                        base.ShowSuccessMessage("Background order color flag saved successfully.");
                    }
                }
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "key", "ClosePopup();", true);
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

        private void BindDropDown()
        {
            rcbInstitutionStatusColorIcons.DataSource = Presenter.GetInstitutionStatusColor();
            rcbInstitutionStatusColorIcons.DataTextField = "OFL_Tooltip";
            rcbInstitutionStatusColorIcons.DataValueField = "IOF_ID";
            rcbInstitutionStatusColorIcons.DataBind();

        }
        #endregion
        #endregion
    }
}