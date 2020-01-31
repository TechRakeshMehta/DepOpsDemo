using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.UserControl
{
    public partial class PackagePaymentOptions : BaseUserControl, IPackagePaymentOptions
    {
        #region Variables

        #region Private Variables

        private PackagePaymentOptionsPresenter _presenter = new PackagePaymentOptionsPresenter();

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public PackagePaymentOptionsPresenter Presenter
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
        /// List of Payment Options, along with flag whether they are selected or not
        /// </summary>
        List<Tuple<Int32, String, Boolean>> IPackagePaymentOptions.PaymentOptions
        {
            set
            {
                List<Tuple<Int32, String, Boolean>> _lstPaymentOptns = value;
                chkPaymentOption.Items.Clear();
                foreach (var paymentOptn in _lstPaymentOptns)
                {
                    chkPaymentOption.Items.Add(new ListItem
                    {
                        Text = paymentOptn.Item2,
                        Value = Convert.ToString(paymentOptn.Item1),
                        Selected = paymentOptn.Item3
                    });
                }
            }
        }

        /// <summary>
        /// Gets the Id's of the Payment Options selected for Save/Update, by admin
        /// </summary>
        public List<Int32> SelectedPaymentOptions
        {
            get
            {
                List<Int32> _lst = new List<int>();
                foreach (ListItem item in chkPaymentOption.Items)
                {
                    if (item.Selected)
                    {
                        _lst.Add(Convert.ToInt32(item.Value));
                    }
                }
                return _lst;
            }
        }

        /// <summary>
        /// Id of the Selected Tenant
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        /// <summary>
        /// Code to identify whether we are using the control for Compliance Package or Background Package
        /// </summary>
        public String PackageTypeCode
        {
            get
            {
                return Convert.ToString(ViewState["PkgTypeCode"]);
            }
            set
            {
                ViewState["PkgTypeCode"] = value;
            }
        }

        /// <summary>
        /// Will be DPP_ID for Compliance Package 
        /// and BPHM_ID for Background Packae
        /// </summary>
        public Int32 PkgNodeMappingId
        {
            get
            {
                return Convert.ToInt32(ViewState["DPPId"]);
            }
            set
            {
                ViewState["DPPId"] = value;
            }
        }

        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.

        public Int32 PaymentApprovalID
        {
            get
            {
                return Convert.ToInt32(rbtnApprovalRequiredBeforePayment.SelectedValue);
            }
            set
            {
                rbtnApprovalRequiredBeforePayment.SelectedValue = value.ToString();
            }
        }

        public Int32 NotSpecifiedPaymentApprovalID
        {
            get;
            set;
        }

        List<lkpPaymentApproval> IPackagePaymentOptions.PaymentApprovalList
        {
            set
            {
                rbtnApprovalRequiredBeforePayment.DataSource = value;
                rbtnApprovalRequiredBeforePayment.DataBind();
                rbtnApprovalRequiredBeforePayment.SelectedValue = Convert.ToString(NotSpecifiedPaymentApprovalID);
            }
        }


        #endregion

        #region UAT-3268

        //public Boolean IsReqToQualifyInRotation
        //{
        //    get
        //    {
        //        return Convert.ToBoolean(ViewState["IsReqToQualifyInRotation"]);
        //    }
        //    set
        //    {
        //        ViewState["IsReqToQualifyInRotation"] = value;
        //    }
        //}
        //public Int32 AdditionalPriceSelectedPaymentOptionID
        //{
        //    get
        //    {
        //        if (rbtnAdditionalPricePaymentOption.SelectedValue != null)
        //        {
        //            return Convert.ToInt32(rbtnAdditionalPricePaymentOption.SelectedValue);
        //        }
        //        return AppConsts.NONE;
        //    }
        //}
        //public Int32 SelectedAdditionalPaymentTypeID
        //{
        //    set
        //    {
        //        rbtnAdditionalPricePaymentOption.SelectedValue = value.ToString();
        //    }
        //}
        //public List<lkpPaymentOption> AdditionalPaymentOptions
        //{

        //    get;
        //    set;
        //}
        #endregion
        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Binds the Payment Options for the Compliance or Background Package
        /// along with whether they should be checked or not in Edit Mode
        /// </summary>
        public void BindPaymentOptions()
        {
            Presenter.BindPaymentOptions();
            //UAT-3268
        //    HideShowAdditionalPanel();
        }

        /// <summary>
        /// Returns the Ids of the PaymentOptionIds selected for a particular Compliance or Background Package
        /// </summary>
        /// <returns></returns>
        public List<Int32> GetSelectedPaymentOptions()
        {
            return this.SelectedPaymentOptions;
        }

        /// <summary>
        /// For UAT-2073: Is Approval Required for Credit Card
        /// </summary>
        /// <returns></returns>
        public Int32 GetApprovalRequiredForCreditCard()
        {
            return this.PaymentApprovalID;
        }

        //#region UAT-3268 - If Selected package is to qualify in Rotation.

        //public void BindAdditionalPricePaymentOption()
        //{
        //    Presenter.BindAdditionalPricePaymentOption();
        //    rbtnAdditionalPricePaymentOption.DataSource = AdditionalPaymentOptions;
        //    rbtnAdditionalPricePaymentOption.DataBind();
        //}

        //public void HideShowAdditionalPanel()
        //{
        //    if (IsReqToQualifyInRotation)
        //    {
        //        BindAdditionalPricePaymentOption();
        //        lblPrimaryPricePaymentOption.Visible = true;
        //        pnlAdditionNode.Visible = true;
        //        lblAdditionalPricePaymentOption.Visible = true;
        //    }
        //    else
        //    {
        //        lblPrimaryPricePaymentOption.Visible = false;
        //        pnlAdditionNode.Visible = false;
        //        lblAdditionalPricePaymentOption.Visible = false;
        //    }
        //}

        //public Int32 GetSelectedPaymentOptionForAdditionalPrice()
        //{
        //    return this.AdditionalPriceSelectedPaymentOptionID;
        //}

        //#endregion

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}