using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ReviewCriteriaHierarchyMapping : BaseUserControl, IReviewCriteriaHierarchyMappingView
    {
        #region Private Variables
        private ReviewCriteriaHierarchyMappingPresenter _presenter = new ReviewCriteriaHierarchyMappingPresenter();
        #endregion

        #region Properties
        public ReviewCriteriaHierarchyMappingPresenter Presenter
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

        public IReviewCriteriaHierarchyMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        Int32 IReviewCriteriaHierarchyMappingView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 IReviewCriteriaHierarchyMappingView.TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IReviewCriteriaHierarchyMappingView.DefaultTenantId
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

        Int32 IReviewCriteriaHierarchyMappingView.DeptProgramMappingID
        {
            get
            {
                return (Int32)(ViewState["DepProgramMappingID"]);
            }
            set
            {
                ViewState["DepProgramMappingID"] = value;
            }
        }


        List<BkgReviewCriteria> IReviewCriteriaHierarchyMappingView.ReviewCriteriaList
        {
            get;
            set;
        }

        List<Int32> IReviewCriteriaHierarchyMappingView.ReviewCriteriaIDList
        {
            get;
            set;
        }

        List<BkgReviewCriteriaHierarchyMapping> IReviewCriteriaHierarchyMappingView.MappedReviewCriteriaList
        {
            get;
            set;
        }

        List<Int32> IReviewCriteriaHierarchyMappingView.MappedReviewCriteriaIds
        {
            get
            {
                if (ViewState["mappedReviewCriteriaIds"].IsNotNull())
                    return (List<Int32>)(ViewState["mappedReviewCriteriaIds"]);
                return new List<Int32>();
            }
            set
            {
                ViewState["mappedReviewCriteriaIds"] = value;
            }
        }
        #endregion

        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (Request.QueryString["SelectedTenantId"].IsNotNull())
                        CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    Presenter.OnViewInitialized();
                    if (Request.QueryString["Id"].IsNotNull())
                        CurrentViewContext.DeptProgramMappingID = Convert.ToInt32(Request.QueryString["Id"]);
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

        #region Grid Events

        protected void grdMappedReviewCriteria_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetMappedReviewCriteria();
                grdMappedReviewCriteria.DataSource = CurrentViewContext.MappedReviewCriteriaList;
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

        protected void grdMappedReviewCriteria_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                WclComboBox ddlReviewCriteria = ((e.Item.FindControl("ddlReviewCriteria") as WclComboBox));
                CurrentViewContext.ReviewCriteriaIDList = new List<Int32>();
                if (ddlReviewCriteria.IsNotNull())
                {
                    for (Int32 i = 0; i < ddlReviewCriteria.Items.Count; i++)
                    {
                        if (ddlReviewCriteria.Items[i].Checked)
                        {
                            CurrentViewContext.ReviewCriteriaIDList.Add(Convert.ToInt32(ddlReviewCriteria.Items[i].Value));
                        }
                    }

                    Boolean saveStatus = Presenter.SaveMapping();
                    if (!saveStatus)
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Some error has occurred.Please try again");
                    }
                    else if (saveStatus)
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Mapping saved successfully.");
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


        protected void grdMappedReviewCriteria_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Int32 BRCHM_ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BRCHM_ID"]);
                Boolean saveStatus = Presenter.DeleteMapping(BRCHM_ID);
                if (!saveStatus)
                {
                    e.Canceled = true;
                    base.ShowErrorMessage("Some Error has occurred.Please try again.");
                }
                else if (saveStatus)
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Mapping deleted successfully");
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

        protected void grdMappedReviewCriteria_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem gridEditFormItem = (GridEditFormItem)e.Item;
                    WclComboBox ddlHierPerUser = (WclComboBox)gridEditFormItem.FindControl("ddlReviewCriteria");

                    Presenter.GetReviewCriteria();

                    if (CurrentViewContext.ReviewCriteriaList.IsNotNull())
                    {
                        ddlHierPerUser.DataSource = CurrentViewContext.ReviewCriteriaList;
                        ddlHierPerUser.DataBind();
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

        #endregion
        #endregion

        #endregion
    }
}