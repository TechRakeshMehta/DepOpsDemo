using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using System.Web.Configuration;
using System.IO;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using CoreWeb.ClinicalRotation.Views;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RotationPackageCategoryDetailPopUp : BaseWebPage, IRequirementPackageCategoryDetail
    {
        #region Private Variable

        private RequirementPackageCategoryDetailPresenter _presenter = new RequirementPackageCategoryDetailPresenter();

        #endregion
        #region Properties


        public RequirementPackageCategoryDetailPresenter Presenter
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

        public IRequirementPackageCategoryDetail CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<RequirementCategoryContract> IRequirementPackageCategoryDetail.CategoryData { get; set; }

        Int32 IRequirementPackageCategoryDetail.RotationID { get; set; }

        Int32 IRequirementPackageCategoryDetail.TenantID { get; set; }

        Boolean IRequirementPackageCategoryDetail.IsStudentPackage { get; set; }

        #endregion

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdRequirementCategoryDetail.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdRequirementCategoryDetail.WclGridDataObject)).ColumnsToSkipEncoding.Add("ExplanatoryNotes");
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
            if (Request.QueryString["TenantID"] != null)
            {
                CurrentViewContext.TenantID = Convert.ToInt32(Request.QueryString["TenantID"]);
            }
            if (Request.QueryString["RotationID"] != null)
            {
                CurrentViewContext.RotationID = Convert.ToInt32(Request.QueryString["RotationID"]);
            }
            if (Request.QueryString["IsStudentPackage"] != null)
            {
                CurrentViewContext.IsStudentPackage = Convert.ToBoolean(Request.QueryString["IsStudentPackage"]);
            }
        }

        #endregion

        #region Grid Events

        protected void grdRequirementCategoryDetail_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetRotationPackageCategoryDetail();
            grdRequirementCategoryDetail.DataSource = CurrentViewContext.CategoryData;
        }

        #endregion

        protected void grdRequirementCategoryDetail_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                dataItem["ExplanatoryNotes"].Text = dataItem["ExplanatoryNotes"].Text.Replace(" -", String.Empty);
            }
        }
    }
}