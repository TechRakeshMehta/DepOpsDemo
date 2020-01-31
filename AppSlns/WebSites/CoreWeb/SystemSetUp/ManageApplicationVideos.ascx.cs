using Entity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.SystemSetUp.Views
{
    public partial class ManageApplicationVideos : BaseUserControl, IManageVideos
    {
        #region Variables

        #region Private Variables

        private ManageVideosPresenter _presenter = new ManageVideosPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        public ManageVideosPresenter Presenter
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

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 IManageVideos.DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageVideos CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<ApplicationVideo> IManageVideos.lstApplicationVideos
        {
            get;
            set;
        }

        List<lkpVideoType> IManageVideos.lstVideoType
        {
            get;
            set;
        }

        String IManageVideos.VideoTitle
        {
            get;
            set;
        }

        Int32 IManageVideos.VideoTypeID
        {
            get;
            set;
        }

        String IManageVideos.VideoDirectLink
        {
            get;
            set;
        }

        String IManageVideos.VideoEmbedLink
        {
            get;
            set;
        }

        String IManageVideos.ErrorMessage
        {
            get;
            set;
        }

        String IManageVideos.SuccessMessage
        {
            get;
            set;
        }

        Int32 IManageVideos.ApplicationVideoID
        {
            get;
            set;
        }

        String IManageVideos.Description
        {
            get;
            set;
        }

        Boolean IManageVideos.IsDisplayDirectLink
        {
            get;
            set;
        }

        Boolean IManageVideos.IsDisplayDescription
        {
            get;
            set;
        }
        #endregion

        #region  Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Videos";
                lblManageApplicationVideos.Text = base.Title;
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
                }

                Presenter.OnViewLoaded();
                base.SetPageTitle("Manage Videos");
                lblSuccess.Visible = false;
                lblSuccess.Text = String.Empty;
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

        protected void grdManageApplicationVideo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetApplicationVideos();
                grdManageApplicationVideo.DataSource = CurrentViewContext.lstApplicationVideos;
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

        protected void grdManageApplicationVideo_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem gridEditFormItem = (GridEditFormItem)e.Item;
                    WclComboBox ddlVideoType = (WclComboBox)gridEditFormItem.FindControl("ddlVideoType");

                    Presenter.GetVideoType();

                    if (CurrentViewContext.lstVideoType.IsNotNull())
                    {
                        Entity.lkpVideoType organizationUser = new Entity.lkpVideoType
                        {
                            VT_ID = AppConsts.NONE,
                            VT_Name = AppConsts.COMBOBOX_ITEM_SELECT
                        };

                        CurrentViewContext.lstVideoType.Insert(AppConsts.NONE, organizationUser);


                        ddlVideoType.DataSource = CurrentViewContext.lstVideoType;
                        ddlVideoType.DataBind();
                    }

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        ApplicationVideo applicationVideo = (ApplicationVideo)e.Item.DataItem;
                        if (!applicationVideo.IsNull())
                        {
                            if (CurrentViewContext.lstVideoType.IsNotNull())
                            {
                                ddlVideoType.SelectedValue = Convert.ToString(applicationVideo.lkpVideoType.VT_ID);
                            }

                            if (applicationVideo.APV_IsDisplayDirectLink.IsNull() || applicationVideo.APV_IsDisplayDirectLink == false)
                            {
                                ((WclButton)gridEditFormItem.FindControl("rbtnDisplayDLinkYes")).Checked = false;
                                ((WclButton)gridEditFormItem.FindControl("rbtnDisplayDLinkNo")).Checked = true;
                            }
                            else
                            {
                                ((WclButton)gridEditFormItem.FindControl("rbtnDisplayDLinkYes")).Checked = true;
                                ((WclButton)gridEditFormItem.FindControl("rbtnDisplayDLinkNo")).Checked = false;
                            }

                            if (applicationVideo.APV_IsDisplayDescription.IsNull() || applicationVideo.APV_IsDisplayDescription == false)
                            {
                                ((WclButton)gridEditFormItem.FindControl("rbtnDisplayDescYes")).Checked = false;
                                ((WclButton)gridEditFormItem.FindControl("rbtnDisplayDescNo")).Checked = true;
                            }
                            else
                            {
                                ((WclButton)gridEditFormItem.FindControl("rbtnDisplayDescYes")).Checked = true;
                                ((WclButton)gridEditFormItem.FindControl("rbtnDisplayDescNo")).Checked = false;
                            }
                        }
                    }
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

        protected void grdManageApplicationVideo_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.VideoTitle = Convert.ToString(((e.Item.FindControl("txtVideoTitle") as WclTextBox).Text));
                CurrentViewContext.VideoTypeID = Convert.ToInt32(((e.Item.FindControl("ddlVideoType") as WclComboBox).SelectedValue));
                CurrentViewContext.VideoDirectLink = Convert.ToString(((e.Item.FindControl("txtVideoDirectLink") as WclTextBox).Text));
                CurrentViewContext.VideoEmbedLink = Convert.ToString(((e.Item.FindControl("txtVideoEmbedLink") as WclTextBox).Text));
                CurrentViewContext.Description = Convert.ToString(((e.Item.FindControl("txtDescription") as WclTextBox).Text));
                CurrentViewContext.IsDisplayDirectLink = Convert.ToBoolean(((e.Item.FindControl("rbtnDisplayDLinkYes") as WclButton).Checked));
                CurrentViewContext.IsDisplayDescription = Convert.ToBoolean(((e.Item.FindControl("rbtnDisplayDescYes") as WclButton).Checked));

                Presenter.SaveApplicationVideos();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdManageApplicationVideo_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ApplicationVideoID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["APV_ID"]);
                CurrentViewContext.VideoTitle = Convert.ToString(((e.Item.FindControl("txtVideoTitle") as WclTextBox).Text));
                CurrentViewContext.VideoTypeID = Convert.ToInt32(((e.Item.FindControl("ddlVideoType") as WclComboBox).SelectedValue));
                CurrentViewContext.VideoDirectLink = Convert.ToString(((e.Item.FindControl("txtVideoDirectLink") as WclTextBox).Text));
                CurrentViewContext.VideoEmbedLink = Convert.ToString(((e.Item.FindControl("txtVideoEmbedLink") as WclTextBox).Text));
                CurrentViewContext.Description = Convert.ToString(((e.Item.FindControl("txtDescription") as WclTextBox).Text));
                CurrentViewContext.IsDisplayDirectLink = Convert.ToBoolean(((e.Item.FindControl("rbtnDisplayDLinkYes") as WclButton).Checked));
                CurrentViewContext.IsDisplayDescription = Convert.ToBoolean(((e.Item.FindControl("rbtnDisplayDescYes") as WclButton).Checked));

                Presenter.UpdateApplicationVideo();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdManageApplicationVideo_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ApplicationVideoID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["APV_ID"]);
                Presenter.DeleteApplicationVideo();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        #region Dropdown Events

        #endregion

        #region Button Events

        #endregion

        #endregion

        #region Methods

        #endregion
    }
}