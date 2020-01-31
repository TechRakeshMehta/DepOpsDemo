using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgOperations.Views;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.UserControl
{
    public partial class ApplicantDetails : BaseUserControl, IApplicantDetails
    {
        #region Variables

        private ApplicantDetailsPresenter _presenter = new ApplicantDetailsPresenter();

        #endregion

        #region Properties

        #region Public Properties

        //public ApplicantDetailsPresenter Presenter
        //{
        //    get
        //    {
        //        this._presenter.View = this;
        //        return this._presenter;
        //    }
        //    set
        //    {
        //        this._presenter = value;
        //        this._presenter.View = this;
        //    }
        //}

        /// <summary>
        /// TenantId of the Applicant
        /// </summary>
        public Int32 TenantId { get; set; }

        /// <summary>
        /// PK of the Order table i.e. OrderID
        /// </summary>
        public Int32 MasterOrderId { get; set; }

        public String MasterOrderNumber { get; set; }

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SupplementOrderApplicantDataContract _applicantData = _presenter.GetApplicantData(this.MasterOrderId, this.TenantId);

                if (!_applicantData.IsNullOrEmpty())
                {
                    lblApplicantName.Text = _applicantData.ApplicantName.HtmlEncode();
                    lblInstituteName.Text = _applicantData.InstitutionName.HtmlEncode();
                    lblOrderNumber.Text = Convert.ToString(this.MasterOrderNumber).HtmlEncode();
                    lblOrderStatus.Text = _applicantData.BkgOrderStatus;
                }
            }
        }
    }
}