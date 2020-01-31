using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class PlacementSpecialty : BaseUserControl, ISpecialtyView
    {
        SpecialtyViewPresenter _presenter = new SpecialtyViewPresenter();
        public SpecialtyViewPresenter Presenter
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
        List<SpecialtyContract> ISpecialtyView.lstSpecialties
        {
            get;
            set;
        }

        public ISpecialtyView CurrentViewContext
        {
            get
            {
                return this;
            }

        }

        SpecialtyContract ISpecialtyView.specialtyContract { get; set; }
        Int32 ISpecialtyView.CurrentUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }
        String ISpecialtyView.SuccessMsg { get; set; }
        String ISpecialtyView.ErrorMsg { get; set; }
        String ISpecialtyView.InfoMsg { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {

        }


        #region Grid Event
        protected void grSpecialty_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.specialtyContract = new SpecialtyContract();
                CurrentViewContext.specialtyContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.specialtyContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                Presenter.InsertSpecialty();
                if (!CurrentViewContext.SuccessMsg.IsNullOrEmpty())
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMsg);
                }
                else if (!CurrentViewContext.ErrorMsg.IsNullOrEmpty())
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMsg);
                }
                else
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.InfoMsg);
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

        protected void grSpecialty_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.specialtyContract = new SpecialtyContract();
                CurrentViewContext.specialtyContract.SpecialtyID = Convert.ToInt32((e.Item.FindControl("txtSpecialtyID") as WclTextBox).Text.Trim());
                CurrentViewContext.specialtyContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.specialtyContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                if (Presenter.UpdateSpecialty())
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Specialty updated successfully.");
                }
                else
                {
                    e.Canceled = true;
                    base.ShowErrorMessage("Some error occurred.Please try again.");
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

        protected void grSpecialty_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 specialtyID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("SpecialtyId")); ;
                if (Presenter.DeleteSpecialty(specialtyID))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Specialty deleted successfully.");
                }
                else
                {
                    e.Canceled = true;
                    base.ShowErrorMessage("Some error occurred.Please try again.");
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

        protected void grSpecialty_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetSpecialties();
            grSpecialty.DataSource = CurrentViewContext.lstSpecialties;
        }
        #endregion
    }
}