using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public partial class AdminOrderReview : BaseUserControl, IAdminOrderReviewView
    {
        #region Variables

        #region Private Variables

        private ApplicantOrderCart _applicantOrderCart;
        private OrganizationUserProfile _orgUserProfile;
        private AdminOrderReviewPresenter _presenter = new AdminOrderReviewPresenter();
        private ApplicantOrderCart applicantOrderCart = new ApplicantOrderCart();
        private Guid LCNAttCode = new Guid("515BEF57-9072-4D2A-A97A-0C248BB045F9");////License Number
        private Guid MotherNameAttrCode = new Guid("3DA8912A-6337-4B8F-93C4-88BFC3032D2D");////Mother's Maiden Name
        private Guid IdentificationNumberAttrCode = new Guid("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211");////Identification Number

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"] != null)
                    return (Int32)(ViewState["TenantId"]);
                return
                   AppConsts.NONE;
            }
            set
            {
                if (ViewState["TenantId"] == null)
                    ViewState["TenantId"] = value;
            }
        }

        List<AttributeFieldsOfSelectedPackages> IAdminOrderReviewView.LstInternationCriminalSrchAttributes
        {
            get;
            set;
        }

        #endregion

        #region Public Properties

        public IAdminOrderReviewView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public AdminOrderReviewPresenter Presenter
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

        public List<BackgroundOrderData> lstBackgroundOrderData
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                return applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;
            }
        }

        #region E DRUG SCREENING PROPERTIES
        public Int32 EDrugScreenCustomFormId
        {
            get;
            set;
        }
        public Int32 EDrugScreenAttributeGroupId
        {
            get;
            set;
        }
        #endregion

        public List<BackgroundPackagesContract> lstPackages
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart.IsNotNull())
                {
                    if (applicantOrderCart.lstApplicantOrder.IsNotNull())
                    {
                        return applicantOrderCart.lstApplicantOrder[0].lstPackages;
                    }
                }
                return new List<BackgroundPackagesContract>();
            }
        }

        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }

        #region UAT-2855
        public Boolean IsAdminOrderScreen
        {
            get
            {
                if (ViewState["IsAdminOrderScreen"].IsNullOrEmpty())
                    return false;
                return Convert.ToBoolean(ViewState["IsAdminOrderScreen"]);
            }
            set
            {
                ViewState["IsAdminOrderScreen"] = value;
            }
        }
        #endregion

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                if (IsAdminOrderScreen)
                {
                    cmdbarSubmit.Visible = false;
                }
            }            
            _applicantOrderCart = GetApplicantOrderCart();
            if (!lstPackages.IsNullOrEmpty() && lstPackages.Count > 0)
                CreateCustomForm();
        }

        #region Private Methods       

        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }

        private void CreateCustomForm()
        {
            String packages = String.Empty;
            packages = GetPackageIdString();
            List<Int32> lstCustomForms = new List<Int32>();
            List<Int32> lstGroupIds = new List<Int32>();
            Presenter.GetAttributeFieldsOfSelectedPackages(packages);
            List<AttributeFieldsOfSelectedPackages> lstCriminalAttributes = CurrentViewContext.LstInternationCriminalSrchAttributes;
            PreviousAddressContract resHisoryProfile = _applicantOrderCart.lstPrevAddresses.FirstOrDefault(cond => cond.isCurrent == true);
            if (resHisoryProfile.IsNotNull() && resHisoryProfile.CountryId != AppConsts.COUNTRY_USA_ID)
            {
                if (!lstCriminalAttributes.IsNullOrEmpty())
                {
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == LCNAttCode.ToString() && x.IsAttributeDisplay))
                    {
                        divInternationalCriminalSearchAttributes.Visible = true;
                        divCriminalLicenseNumber.Visible = true;
                    }
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == MotherNameAttrCode.ToString() && x.IsAttributeDisplay))
                    {
                        divInternationalCriminalSearchAttributes.Visible = true;
                        divMothersName.Visible = true;
                    }
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == IdentificationNumberAttrCode.ToString() && x.IsAttributeDisplay))
                    {
                        divInternationalCriminalSearchAttributes.Visible = true;
                        divIdentificationNumber.Visible = true;
                    }
                }
            }
            if (!lstBackgroundOrderData.IsNullOrEmpty())
            {

                lstCustomForms = lstBackgroundOrderData.Where(x => x.CustomFormId != AppConsts.ONE).DistinctBy(x => x.CustomFormId).Select(x => x.CustomFormId).ToList();
                #region E DRUG SCREENING
                Presenter.GetEDrugAttributeGroupIdAndFormId();

                #endregion
                for (Int32 custId = 0; custId < lstCustomForms.Count; custId++)
                {
                    Presenter.GetAttributesForTheCustomForm(packages, lstCustomForms[custId]);
                    List<BackgroundOrderData> newLstBackGroundOrderData = new List<BackgroundOrderData>();
                    newLstBackGroundOrderData = lstBackgroundOrderData.Where(x => x.CustomFormId == lstCustomForms[custId]).Select(x => x).ToList();
                    lstGroupIds = newLstBackGroundOrderData.DistinctBy(x => x.BkgSvcAttributeGroupId).Select(x => x.BkgSvcAttributeGroupId).ToList();
                    for (Int32 grpId = 0; grpId < lstGroupIds.Count; grpId++)
                    {

                        if ((EDrugScreenAttributeGroupId > 0 && EDrugScreenCustomFormId > 0) && (lstGroupIds[grpId] == EDrugScreenAttributeGroupId && lstCustomForms[custId] == EDrugScreenCustomFormId))
                        {
                            WebCCF _webCCFForm = Page.LoadControl("~/BkgOperations/UserControl/WebCCF.ascx") as WebCCF;
                            _webCCFForm.IsReview = true;
                            _webCCFForm.IsOrderConfirmation = false;
                            _webCCFForm.CustomFormId = lstCustomForms[custId];
                            _webCCFForm.AttributeGroupId = lstGroupIds[grpId];
                            _webCCFForm.LstBackgroundOrderData = newLstBackGroundOrderData;
                            _webCCFForm.LstAttributeForCustomFormContract = lstCustomFormAttributes;
                            pnlReviewLoader.Controls.Add(_webCCFForm);
                        }
                        else
                        {
                            CustomFormHtlm _customForm = Page.LoadControl("~/BkgOperations/UserControl/CustomFormHtlm.ascx") as CustomFormHtlm;
                            _customForm.lstCustomFormAttributes = lstCustomFormAttributes;
                            _customForm.groupId = lstGroupIds[grpId];
                            //Total Number Of Instane for a particular group
                            _customForm.InstanceId = newLstBackGroundOrderData.Where(x => x.BkgSvcAttributeGroupId == lstGroupIds[grpId] && x.CustomFormId == lstCustomForms[custId]).Count();
                            _customForm.CustomFormId = lstCustomForms[custId];
                            _customForm.tenantId = CurrentViewContext.TenantId;
                            _customForm.lstBackgroundOrderData = newLstBackGroundOrderData;
                            _customForm.IsReadOnly = true;
                            if (IsAdminOrderScreen)
                            {
                                _customForm.ShowEditDetailButton = true;
                                _customForm.IsAdminOrderScreen = IsAdminOrderScreen;
                            }
                            else
                            {
                                _customForm.ShowEditDetailButton = false;
                            }

                            pnlReviewLoader.Controls.Add(_customForm);
                        }
                    }
                }
            }
        }

        private string GetPackageIdString()
        {
            String packages = String.Empty;
            if (!lstPackages.IsNullOrEmpty())
            {
                lstPackages.ForEach(x => packages += Convert.ToString(x.BPAId) + ",");
                //packages = "4";
                if (packages.EndsWith(","))
                    packages = packages.Substring(0, packages.Length - 1);
            }
            return packages;
        }

        #endregion

        #region Button Events

        protected void cmdbarSubmit_Back(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefeshPage();", true);
        }

        #endregion

    }


}