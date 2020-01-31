using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


namespace CoreWeb.BkgOperations.Views
{
    public partial class AdminCreateOrderDetails : BaseUserControl, IAdminCreateOrderDetailsView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private AdminCreateOrderDetailsPresenter _presenter = new AdminCreateOrderDetailsPresenter();
        private String _viewType;
        private Guid LCNAttCode = new Guid("515BEF57-9072-4D2A-A97A-0C248BB045F9");////License Number
        private Guid LCSAttCode = new Guid("1ADA97AE-9100-4BE6-B829-C914B7FA8750");//Driver's License State

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        Int32 IAdminCreateOrderDetailsView.SelectedTenantId
        {
            get
            {
                if (ViewState["SelectedTenantId"].IsNullOrEmpty())
                    return AppConsts.NONE;
                else
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        Int32 IAdminCreateOrderDetailsView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 IAdminCreateOrderDetailsView.OrderID
        {
            get
            {
                if (ViewState["OrderID"].IsNullOrEmpty())
                    return AppConsts.NONE;
                else
                    return Convert.ToInt32(ViewState["OrderID"]);
            }
            set
            {
                ViewState["OrderID"] = value;
            }
        }

        Int32 IAdminCreateOrderDetailsView.BkgOrderId
        {
            get
            {
                if (ViewState["BkgOrderId"].IsNullOrEmpty())
                    return AppConsts.NONE;
                else
                    return Convert.ToInt32(ViewState["BkgOrderId"]);
            }
            set
            {
                ViewState["BkgOrderId"] = value;
            }
        }
        List<Entity.lkpGender> IAdminCreateOrderDetailsView.GenderList
        {
            set
            {
                cmbGender.DataSource = value.Where(cond => cond.LanguageID == LanguageTranslateUtils.GetCurrentLanguageFromSession().LanguageID);
                cmbGender.DataBind();
            }
        }

        String IAdminCreateOrderDetailsView.FirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }
        String IAdminCreateOrderDetailsView.MiddleName
        {
            get
            {
                if (chkMiddleNameRequired.Checked)
                {
                    return String.Empty;
                }
                else
                {
                    return txtMiddleName.Text;
                }
            }
            set
            {
                txtMiddleName.Text = value;
                chkMiddleNameRequired.Checked = value.IsNullOrEmpty() ? false : true;
            }
        }
        String IAdminCreateOrderDetailsView.LastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }
        Boolean IAdminCreateOrderDetailsView.IsInternationalPhoneNumber
        {
            get
            {
                return chkPrimaryPhone.Checked;
            }
            set
            {
                chkPrimaryPhone.Checked = value;
            }
        }
        String IAdminCreateOrderDetailsView.PhoneNumber
        {
            get
            {
                if (chkPrimaryPhone.Checked)
                    return txtUnmaskedPrimaryPhone.Text;
                else
                    return txtPhoneNumber.Text;
            }
            set
            {
                if (CurrentViewContext.IsInternationalPhoneNumber)
                    txtUnmaskedPrimaryPhone.Text = value;
                else
                    txtPhoneNumber.Text = value;
            }
        }
        String IAdminCreateOrderDetailsView.SSN
        {
            get
            {
                return txtSSN.Text;
            }
            set
            {
                txtSSN.Text = value;
            }
        }
        DateTime IAdminCreateOrderDetailsView.DOB
        {
            get
            {
                return dpkrDOB.SelectedDate.Value;
            }
            set
            {
                dpkrDOB.SelectedDate = value;
            }
        }
        String IAdminCreateOrderDetailsView.Email
        {
            get
            {
                return txtEmail.Text;
            }
            set
            {
                txtEmail.Text = value;
            }
        }
        String IAdminCreateOrderDetailsView.Address1
        {
            get
            {
                return txtAddress1.Text;
            }
            set
            {
                txtAddress1.Text = value;
            }
        }
        String IAdminCreateOrderDetailsView.Address2
        {
            get
            {
                return txtAddress2.Text;
            }
            set
            {
                txtAddress2.Text = value;
            }
        }
        Int32 IAdminCreateOrderDetailsView.ZipId
        {
            get
            {
                if (locationTenant.ZipId == 0 && ViewState["ZipId"].IsNotNull())
                    return Convert.ToInt32(ViewState["ZipId"]);
                return locationTenant.MasterZipcodeID.Value;
            }
            set
            {
                ViewState["ZipId"] = locationTenant.MasterZipcodeID.Value;
            }
        }
        List<PersonAliasContract> IAdminCreateOrderDetailsView.PersonAliasList
        {
            get
            {
                return ucPersonAlias.PersonAliasList;
            }
            set
            {
                ucPersonAlias.PersonAliasList = value;
            }
        }

        String IAdminCreateOrderDetailsView.StateName
        {
            get
            {
                return locationTenant.RSLStateName;
            }
            set
            {
                locationTenant.RSLStateName = value;
            }
        }

        String IAdminCreateOrderDetailsView.CityName
        {
            get
            {
                return locationTenant.RSLCityName;
            }
            set
            {
                locationTenant.RSLCityName = value;
            }
        }

        String IAdminCreateOrderDetailsView.PostalCode
        {
            get
            {
                return locationTenant.RSLZipCode;
            }
            set
            {
                locationTenant.RSLZipCode = value;
            }
        }

        Int32 IAdminCreateOrderDetailsView.CountryId
        {
            get
            {
                return locationTenant.RSLCountryId;
            }
            set
            {
                locationTenant.RSLCountryId = value;
            }
        }

        List<BackgroundPackagesContract> IAdminCreateOrderDetailsView.lstBkgPackage
        {
            get
            {
                if (ViewState["lstBkgPackage"].IsNullOrEmpty())
                    return new List<BackgroundPackagesContract>();
                else
                    return (ViewState["lstBkgPackage"] as List<BackgroundPackagesContract>);
            }
            set
            {
                ViewState["lstBkgPackage"] = value;
            }
        }

        OrganizationUser IAdminCreateOrderDetailsView.OrganizationUser
        {
            get
            {
                if (ViewState["OrganizationUser"].IsNullOrEmpty())
                    return new OrganizationUser();
                else
                    return (ViewState["OrganizationUser"] as OrganizationUser);
            }
            set
            {
                ViewState["OrganizationUser"] = value;
            }
        }

        OrganizationUserProfile IAdminCreateOrderDetailsView.OrganizationUserProfile
        {
            get
            {
                if (ViewState["OrganizationUserProfile"].IsNullOrEmpty())
                    return new OrganizationUserProfile();
                else
                    return (ViewState["OrganizationUserProfile"] as OrganizationUserProfile);
            }
            set
            {
                ViewState["OrganizationUserProfile"] = value;
            }
        }

        Int32 IAdminCreateOrderDetailsView.SelectedNodeId
        {
            get
            {
                if (ViewState["SelectedNodeId"].IsNotNull())
                    return Convert.ToInt32(ViewState["SelectedNodeId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedNodeId"] = value;
            }
        }

        Int32 IAdminCreateOrderDetailsView.HierarchyNodeID
        {
            get
            {
                if (ViewState["HierarchyNodeID"].IsNotNull())
                    return Convert.ToInt32(ViewState["HierarchyNodeID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["HierarchyNodeID"] = value;
            }
        }

        Boolean IAdminCreateOrderDetailsView.AttestFCRAPrevisions
        {
            get
            {
                return chkAttestToFCRA.Checked;
            }
            set
            {
                chkAttestToFCRA.Checked = value;
            }
        }

        Boolean IAdminCreateOrderDetailsView.IsCustomFormDetailsSave
        {
            get
            {
                if (ViewState["IsCustomFormDetailsSave"].IsNotNull())
                    return Convert.ToBoolean(ViewState["IsCustomFormDetailsSave"]);
                return false;
            }
            set
            {
                ViewState["IsCustomFormDetailsSave"] = value;
            }
        }
        Int32 IAdminCreateOrderDetailsView.Gender
        {
            get
            {
                return Convert.ToInt32(cmbGender.SelectedValue);
            }
            set
            {
                if (!value.IsNullOrEmpty())
                    cmbGender.SelectedValue = Convert.ToString(value);
            }
        }

        List<Int32> IAdminCreateOrderDetailsView.lstSelectedPackageIds
        {
            get
            {
                if (ViewState["lstSelectedPackageIds"].IsNotNull())
                    return ViewState["lstSelectedPackageIds"] as List<Int32>;
                return new List<Int32>();
            }
            set
            {
                ViewState["lstSelectedPackageIds"] = value;
            }
        }

        String IAdminCreateOrderDetailsView.ClientMachineIP
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["ClientMachineIP"]);
            }
        }

        List<AdminCreateOrderContract> IAdminCreateOrderDetailsView.AdminCreateOrderContract
        {
            get
            {
                if (ViewState["AdminCreateOrderContract"].IsNotNull())
                    return ViewState["AdminCreateOrderContract"] as List<AdminCreateOrderContract>;
                return new List<AdminCreateOrderContract>();
            }
            set
            {
                ViewState["AdminCreateOrderContract"] = value;
            }
        }

        List<ApplicantDocumentContract> IAdminCreateOrderDetailsView.lstApplicantDocumentContract
        {
            get
            {
                if (ViewState["lstApplicantDocumentContract"] != null)
                    return (ViewState["lstApplicantDocumentContract"] as List<ApplicantDocumentContract>);
                return new List<ApplicantDocumentContract>();
            }
            set
            {
                ViewState["lstApplicantDocumentContract"] = value;
            }
        }
        List<ApplicantDocument> IAdminCreateOrderDetailsView.lstApplicantDocument
        {
            get
            {
                if (ViewState["lstApplicantDocument"] != null)
                    return (ViewState["lstApplicantDocument"] as List<ApplicantDocument>);
                return new List<ApplicantDocument>();
            }
            set
            {
                ViewState["lstApplicantDocument"] = value;
            }
        }

        List<PreviousAddressContract> IAdminCreateOrderDetailsView.ResidentialHistoryList
        {
            get
            {
                return PrevResident.ResidentialHistoryList;
            }
            set
            {
                PrevResident.ResidentialHistoryList = value;
            }
        }

        List<PreviousAddressContract> IAdminCreateOrderDetailsView.ResidentialHistoryListAll
        {
            get
            {
                if (ViewState["ResidentialHistoryListAll"].IsNullOrEmpty())
                    return new List<PreviousAddressContract>();
                return ViewState["ResidentialHistoryListAll"] as List<PreviousAddressContract>;
            }
            set
            {
                CurrentViewContext.ResidentialHistoryList = value.Where(cond => cond.isCurrent == false).ToList();
                ViewState["ResidentialHistoryListAll"] = value;
            }
        }

        Boolean IAdminCreateOrderDetailsView.IsOrderReadyForTransmit
        {
            get
            {
                if (ViewState["IsOrderReadyForTransmit"].IsNullOrEmpty())
                    return false;
                return Convert.ToBoolean(ViewState["IsOrderReadyForTransmit"]);
            }
            set
            {
                ViewState["IsOrderReadyForTransmit"] = value;
            }
        }

        //DateTime? IAdminCreateOrderDetailsView.DateResidentFrom
        //{
        //    get
        //    {
        //        return dpCurResidentFrom.SelectedDate;
        //    }
        //}

        public String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }

        Int32 IAdminCreateOrderDetailsView.MinResidentailHistoryOccurances
        {
            get
            {
                if (ViewState["MinResidentailHistoryOccurances"] != null)
                {
                    return (Int32)ViewState["MinResidentailHistoryOccurances"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["MinResidentailHistoryOccurances"] = value;
            }
        }

        Int32? IAdminCreateOrderDetailsView.MaxResidentailHistoryOccurances
        {
            get
            {
                if (ViewState["MaxResidentailHistoryOccurances"] != null)
                {
                    return (Int32)ViewState["MaxResidentailHistoryOccurances"];
                }
                //return AppConsts.NONE;UAT-605
                return null;
            }
            set
            {
                ViewState["MaxResidentailHistoryOccurances"] = value;
            }
        }

        Int32 IAdminCreateOrderDetailsView.MinPersonalAliasOccurances
        {
            get
            {
                if (ViewState["MinPersonalAliasOccurances"] != null)
                {
                    return (Int32)ViewState["MinPersonalAliasOccurances"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["MinPersonalAliasOccurances"] = value;
            }
        }

        Int32? IAdminCreateOrderDetailsView.MaxPersonalAliasOccurances
        {
            get
            {
                if (ViewState["MaxPersonalAliasOccurances"] != null)
                {
                    return (Int32)ViewState["MaxPersonalAliasOccurances"];
                }
                //return AppConsts.NONE;UAT-605
                return null;
            }
            set
            {
                ViewState["MaxPersonalAliasOccurances"] = value;
            }
        }

        List<BackgroundOrderData> IAdminCreateOrderDetailsView.lstBkgOrderData
        {
            get
            {
                if (!ViewState["lstBkgOrderData"].IsNullOrEmpty())
                    return ViewState["lstBkgOrderData"] as List<BackgroundOrderData>;
                return new List<BackgroundOrderData>();
            }
            set
            {
                ViewState["lstBkgOrderData"] = value;
            }
        }


        #region MVR Info

        List<Entity.State> IAdminCreateOrderDetailsView.ListStates
        {
            get;
            set;
        }

        List<AttributeFieldsOfSelectedPackages> IAdminCreateOrderDetailsView.lstMvrAttGrp
        {
            get
            {
                if (ViewState["lstMvrAttGrp"] != null)
                    return (List<AttributeFieldsOfSelectedPackages>)ViewState["lstMvrAttGrp"];
                return null;
            }
            set
            {
                ViewState["lstMvrAttGrp"] = value;
            }
        }

        List<AttributeFieldsOfSelectedPackages> IAdminCreateOrderDetailsView.LstInternationCriminalSrchAttributes
        {
            get
            {
                if (ViewState["LstInternationCriminalSrchAttributes"] != null)
                    return (List<AttributeFieldsOfSelectedPackages>)ViewState["LstInternationCriminalSrchAttributes"];
                return null;
            }
            set
            {
                ViewState["LstInternationCriminalSrchAttributes"] = value;
            }
        }
        #endregion

        #endregion

        #region Private Properties

        private IAdminCreateOrderDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private AdminCreateOrderDetailsPresenter Presenter
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

        #endregion

        #endregion

        #region Events

        #region Page Event

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Order Detail";
                base.SetPageTitle("Order Detail");
                lblOrderDetail.Text = base.Title;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
                    ucPersonAlias.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                    iframe.Src = String.Empty;
                    CaptureQuerystringParameters();
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    btnBindPackage.Style["display"] = "none";
                    CurrentViewContext.IsOrderReadyForTransmit = false;
                    PrevResident.IsApplicantOrderScreen = true;
                    Presenter.GetAdminOrderDetails();
                    //ShowHideInternationalPhoneNumberControl();
                    LoadControls();
                    hdnTenantId.Value = Convert.ToString(CurrentViewContext.SelectedTenantId);
                    String InfoMessage = Presenter.CheckOrderAvailabilityForTrasmit();
                    if (!InfoMessage.IsNullOrEmpty())
                    {
                        fsucCmdBarButton.SaveButton.Enabled = false;
                        fsucCmdBarButton.SubmitButton.Enabled = false;
                        btnNext.Enabled = false;
                        base.ShowInfoMessage(InfoMessage);
                    }
                }
                ShowHideInternationalPhoneNumberControl();
                MvrControls();
                lblinstituteHierarchy.Text = hdnHierarchyLabel.Value.HtmlEncode();
                rngvDOB.MaximumValue = DateTime.Now.Date.AddYears(-1).ToShortDateString();
                rngvDOB.MinimumValue = Convert.ToDateTime("01-01-1900").ToShortDateString();
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

        #region Button Events

        /// <summary>
        /// Save Order Details Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty())
                {
                    ShowHidePersonalAliasDetails();
                    ShowHideResidentialsHistory();
                }
                if (CurrentViewContext.MinResidentailHistoryOccurances > AppConsts.NONE &&
                ((PrevResident.ResidentialHitoryTempList.Where(cond => cond.isDeleted == false).Count() + AppConsts.ONE) < CurrentViewContext.MinResidentailHistoryOccurances))
                {
                    base.ShowInfoMessage("Please enter at least " + CurrentViewContext.MinResidentailHistoryOccurances + " Residence(s) for this Order.");
                    return;
                }

                if (CurrentViewContext.MinPersonalAliasOccurances > AppConsts.NONE &&
                ((ucPersonAlias.PersonAliasList.Count()) < CurrentViewContext.MinPersonalAliasOccurances))
                {
                    base.ShowInfoMessage("Please enter at least " + CurrentViewContext.MinPersonalAliasOccurances + " Alias/Maiden Name(s) for this Order.");
                    return;
                }

                AddUpdateOrderDetails(true);
                if (Presenter.AdminOrderIsReadyToTransmit())
                {
                    CurrentViewContext.IsOrderReadyForTransmit = true;
                }

                LoadControls();
                base.ShowSuccessMessage("Order Details saved successfully.");
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

        /// <summary>
        /// On close of insitution hierarchy popup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBindPackage_Click(object sender, EventArgs e)
        {
            try
            {
                BindPackageDropDown();
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

        /// <summary>
        /// Cancel button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                 {
                                                               { "Child",  AppConsts.ADMIN_CREATE_ORDER},
                                                              // { "ID", CurrentViewContext.ClinicalRotationID.ToString() },
                                                               {"CancelClick","true"}
                                                            };
                String url = String.Format("~/BkgOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
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

        /// <summary>
        /// Delete admin order details (Delete button clicked)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_DeleteClick(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.DeleteAdminOrderDetails())
                {
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                        {  
                                            { "Child",  AppConsts.ADMIN_CREATE_ORDER},
                                            {"CancelClick","true"},
                                            {"ButtonClick","delete"}
                                        };
                    String url = String.Format("~/BkgOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
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

        /// <summary>
        /// Trasmit button click (Trasmit the admin order)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty())
                {
                    ShowHidePersonalAliasDetails();
                    ShowHideResidentialsHistory();
                }
                if (CurrentViewContext.MinResidentailHistoryOccurances > AppConsts.NONE &&
                ((PrevResident.ResidentialHitoryTempList.Where(cond => cond.isDeleted == false).Count() + AppConsts.ONE) < CurrentViewContext.MinResidentailHistoryOccurances))
                {
                    base.ShowInfoMessage("Please enter at least " + CurrentViewContext.MinResidentailHistoryOccurances + " Residence(s) for this Order.");
                    return;
                }

                if (CurrentViewContext.MinPersonalAliasOccurances > AppConsts.NONE &&
                ((ucPersonAlias.PersonAliasList.Count()) < CurrentViewContext.MinPersonalAliasOccurances))
                {
                    base.ShowInfoMessage("Please enter at least " + CurrentViewContext.MinPersonalAliasOccurances + " Alias/Maiden Name(s) for this Order.");
                    return;
                }

                AddUpdateOrderDetails(true);
                if (Presenter.AdminOrderIsReadyToTransmit())
                {
                    Presenter.TransmmitAdminOrders();
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                        {  
                                            { "Child",  AppConsts.ADMIN_CREATE_ORDER},
                                            {"CancelClick","true"},
                                            {"ButtonClick","transmit"}
                                        };
                    String url = String.Format("~/BkgOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                else
                {
                    base.ShowInfoMessage("Please enter the required information to transmit the order.");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUploadAdditional_Click(object sender, EventArgs e)
        {
            String docTypeCode = DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue();
            UploadAllDocuments(docTypeCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUploadDandA_Click(object sender, EventArgs e)
        {
            String docTypeCode = DocumentType.Disclosure_n_Release.GetStringValue();
            UploadAllDocuments(docTypeCode);
        }

        /// <summary>
        /// Next Button click (Save applicant details/Package mapping and Load the custom forms)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty())
                {
                    ShowHidePersonalAliasDetails();
                    ShowHideResidentialsHistory();
                }
                AddUpdateOrderDetails(false);
                //SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
                //LoadControls();
                //if (Presenter.AdminOrderIsReadyToTransmit())
                //{
                //    CurrentViewContext.IsOrderReadyForTransmit = true;
                //}
                //base.ShowSuccessMessage("Order Details save successfully.");
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

        /// <summary>
        /// Refesh iframe for custom form in and out view mode to edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefeshPage_Click(object sender, EventArgs e)
        {
            //Save Custom Form data
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
            {
            //    List<BackgroundOrderData> lstBkgOrderData = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;
            //    //if (CurrentViewContext.IsOrderReadyForTransmit)
            //    //{
            //    //    BackgroundOrderData lastBkgOrderData = lstBkgOrderData.LastOrDefault();
            //    //    CurrentViewContext.IsOrderReadyForTransmit = false;
            //    //    Int32 MVRCustomFormId = (_presenter.GetCustomFormIDBYCode() > 0) ? _presenter.GetCustomFormIDBYCode() : AppConsts.NONE;
            //    //    applicantOrderCart.AdminOrderSteps = lstBkgOrderData.Where(cond => cond.CustomFormId != MVRCustomFormId).DistinctBy(x => x.CustomFormId).Count() - 1;
            //    //    iframe.Src = String.Format(@"\BkgOperations\Pages\CustomFormPage.aspx?SelectedTenantId=" + CurrentViewContext.SelectedTenantId + "&CustomFormId=" + lastBkgOrderData.CustomFormId + "&IsPrevious=1");
            //    //}
            //    //else
            //    //{
            //    //    CurrentViewContext.IsOrderReadyForTransmit = true;
            //    //    iframe.Src = String.Format(@"\BkgOperations\Pages\AdminOrderReviewMain.aspx?TenantId=" + CurrentViewContext.SelectedTenantId);
            //    //}
            //    //BackgroundOrderData lastBkgOrderData = lstBkgOrderData.LastOrDefault();
            //    //CurrentViewContext.IsOrderReadyForTransmit = false;
            //    //Int32 MVRCustomFormId = (_presenter.GetCustomFormIDBYCode() > 0) ? _presenter.GetCustomFormIDBYCode() : AppConsts.NONE;
            //    //applicantOrderCart.AdminOrderSteps = lstBkgOrderData.Where(cond => cond.CustomFormId != MVRCustomFormId).DistinctBy(x => x.CustomFormId).Count() - 1;
            //    //iframe.Src = String.Format(@"\BkgOperations\Pages\CustomFormPage.aspx?SelectedTenantId=" + CurrentViewContext.SelectedTenantId + "&CustomFormId=" + lastBkgOrderData.CustomFormId + "&IsPrevious=1");                
                iframe.Src = String.Format(@"\BkgOperations\Pages\AdminOrderReviewMain.aspx?TenantId=" + CurrentViewContext.SelectedTenantId);
            }
        }


        protected void btnCheckIsMvrPkgChecked_Click(object sender, EventArgs e)
        {

            Int32 chkdItemsCount = cmbBkgPackages.CheckedItems.Count();
            List<Int32> lstNewSelectedPkgIds = cmbBkgPackages.CheckedItems.Select(sel => sel.Value).Select(Int32.Parse).ToList();
            List<Int32> lstOldSelectedPkgIds = CurrentViewContext.lstSelectedPackageIds;
            List<Int32> matchingPackageIds = lstNewSelectedPkgIds.Intersect(lstOldSelectedPkgIds).ToList();
            List<Int32> removedPackageIds = lstOldSelectedPkgIds.Count > lstNewSelectedPkgIds.Count ? lstOldSelectedPkgIds.Except(lstNewSelectedPkgIds).ToList() : null;
            if (!CurrentViewContext.lstApplicantDocumentContract.IsNullOrEmpty())
            {
                if (matchingPackageIds.IsNullOrEmpty() || chkdItemsCount == AppConsts.NONE)
                {
                    Presenter.DeleteApplicantDocuments();
                    grdApplicantDoc.Rebind();
                }
            }
            CurrentViewContext.lstSelectedPackageIds = cmbBkgPackages.CheckedItems.Select(sel => sel.Value).Select(Int32.Parse).ToList();

            if (lstNewSelectedPkgIds.Except(lstOldSelectedPkgIds).Any() || lstOldSelectedPkgIds.Except(lstNewSelectedPkgIds).Any())
            {
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
                // LoadCustomForm();
                pnlCustomFormLoad.Visible = false;
            }

            CurrentViewContext.lstSelectedPackageIds = cmbBkgPackages.CheckedItems.Select(sel => sel.Value).Select(Int32.Parse).ToList();
            Presenter.GetAttributeFieldsOfSelectedPackages();
            if (CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty())
            {
                //pnlMvrInfo.Visible = false;
                //cmbState.ClearSelection();
                //txtLicenseNO.Text = String.Empty;
                chkIsMVRInfo.Checked = false;
                chkIsMVRInfo.Enabled = false;
                MvrControls();
            }
            else
            {

                chkIsMVRInfo.Enabled = true;


                //pnlMvrInfo.Visible = true;
                //dvDriverLicenseState.Visible = true;
                //dvDriverLicenseNo.Visible = true;
                //Presenter.GetAllStates();
                //if (CurrentViewContext.ListStates.IsNotNull())
                //{
                //    BindDLState();
                //}
            }

           

            // SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
        }
        #endregion

        #region Package ComboBox Events

        /// <summary>
        /// selected index change of package combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbBkgPackages_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Int32 chkdItemsCount = cmbBkgPackages.CheckedItems.Count();
                //List<Int32> lstNewSelectedPkgIds = cmbBkgPackages.CheckedItems.Select(sel => sel.Value).Select(Int32.Parse).ToList();
                //List<Int32> lstOldSelectedPkgIds = CurrentViewContext.lstSelectedPackageIds;
                //List<Int32> matchingPackageIds = lstNewSelectedPkgIds.Intersect(lstOldSelectedPkgIds).ToList();
                //List<Int32> removedPackageIds = lstOldSelectedPkgIds.Count > lstNewSelectedPkgIds.Count ? lstOldSelectedPkgIds.Except(lstNewSelectedPkgIds).ToList() : null;
                //if (!CurrentViewContext.lstApplicantDocumentContract.IsNullOrEmpty())
                //{
                //    if (matchingPackageIds.IsNullOrEmpty() || chkdItemsCount == AppConsts.NONE)
                //    {
                //        Presenter.DeleteApplicantDocuments();
                //        grdApplicantDoc.Rebind();
                //    }
                //}
                //CurrentViewContext.lstSelectedPackageIds = cmbBkgPackages.CheckedItems.Select(sel => sel.Value).Select(Int32.Parse).ToList();

                //if (matchingPackageIds.Count != lstOldSelectedPkgIds.Count)
                //{
                //    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
                //    // LoadCustomForm();
                //    pnlCustomFormLoad.Visible = false;
                //}

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

        #region Grid Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantDoc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetApplicantDocuments();
                grdApplicantDoc.DataSource = CurrentViewContext.lstApplicantDocumentContract;
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

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Get data from query string.
        /// </summary>
        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey("SelectedTenantId"))
                {
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(args["SelectedTenantId"]);
                }
                if (args.ContainsKey("OrderID"))
                {
                    CurrentViewContext.OrderID = Convert.ToInt32(args["OrderID"]);
                }
                if (args.ContainsKey("IsCustomFormDetailsSave"))
                {
                    CurrentViewContext.IsCustomFormDetailsSave = Convert.ToBoolean(args["IsCustomFormDetailsSave"]);
                }
            }
        }

        /// <summary>
        /// Load Controls for edit mode
        /// </summary>
        private void LoadControls()
        {
            if (!CurrentViewContext.AdminCreateOrderContract.IsNullOrEmpty() && CurrentViewContext.AdminCreateOrderContract.Count > AppConsts.NONE)
            {
                hdnDepartmntPrgrmMppng.Value = Convert.ToString(CurrentViewContext.AdminCreateOrderContract.FirstOrDefault().SelectedNodeID);
                hdnHierarchyLabel.Value = CurrentViewContext.AdminCreateOrderContract.FirstOrDefault().NodeLabel;
                CurrentViewContext.AttestFCRAPrevisions = CurrentViewContext.AdminCreateOrderContract.FirstOrDefault().AttestFCRAPrevisions.IsNullOrEmpty()
                                    ? false : CurrentViewContext.AdminCreateOrderContract.FirstOrDefault().AttestFCRAPrevisions;

                BindPackageDropDown();
                CurrentViewContext.lstSelectedPackageIds = CurrentViewContext.AdminCreateOrderContract.Where(cond => cond.BkgPackageHierarchyMappingID > AppConsts.NONE).Select(sel => sel.BkgPackageHierarchyMappingID).ToList();

                cmbBkgPackages.Items.Where(cond => CurrentViewContext.lstSelectedPackageIds.Contains(Convert.ToInt32(cond.Value))).ForEach(x =>
                {
                    x.Checked = true;
                });
                if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty())
                {
                    ShowHidePersonalAliasDetails();
                    ShowHideResidentialsHistory();
                }
                CurrentViewContext.FirstName = CurrentViewContext.OrganizationUserProfile.FirstName;
                ValidateMiddleName(CurrentViewContext.OrganizationUserProfile.MiddleName);
                CurrentViewContext.LastName = CurrentViewContext.OrganizationUserProfile.LastName;
                CurrentViewContext.IsInternationalPhoneNumber = CurrentViewContext.OrganizationUserProfile.IsInternationalPhoneNumber;
                ShowHideInternationalPhoneNumberControl();
                CurrentViewContext.PhoneNumber = CurrentViewContext.OrganizationUserProfile.PhoneNumber;
                CurrentViewContext.Gender = CurrentViewContext.OrganizationUserProfile.Gender.Value;
                CurrentViewContext.Email = CurrentViewContext.OrganizationUserProfile.PrimaryEmailAddress;
                CurrentViewContext.Address1 = CurrentViewContext.OrganizationUser.AddressHandle.Addresses.FirstOrDefault().Address1;
                CurrentViewContext.Address2 = CurrentViewContext.OrganizationUser.AddressHandle.Addresses.FirstOrDefault().Address2;
                CurrentViewContext.SSN = CurrentViewContext.AdminCreateOrderContract.FirstOrDefault().SSN;

                if (!CurrentViewContext.OrganizationUserProfile.DOB.IsNullOrEmpty())
                {
                    CurrentViewContext.DOB = CurrentViewContext.OrganizationUserProfile.DOB.Value;
                }

                //PersonalAlias
                if (CurrentViewContext.OrganizationUser.PersonAlias.IsNotNull())
                {
                    CurrentViewContext.PersonAliasList = CurrentViewContext.OrganizationUser.PersonAlias.Where(x => x.PA_IsDeleted == false).Select(cond => new PersonAliasContract
                    {
                        FirstName = cond.PA_FirstName,
                        LastName = cond.PA_LastName,
                        MiddleName = cond.PA_MiddleName,
                        ID = cond.PA_ID
                    }).ToList();
                }

                //Location binding
                var addressFields = CurrentViewContext.OrganizationUser.AddressHandle.Addresses.FirstOrDefault();
                List<Entity.ClientEntity.AddressExt> AddressExts = CurrentViewContext.OrganizationUser.AddressHandle.Addresses.FirstOrDefault().AddressExts.ToList();
                locationTenant.MasterZipcodeID = addressFields.ZipCodeID;
                if (addressFields.ZipCodeID == 0 && addressFields.AddressExts.IsNotNull() && addressFields.AddressExts.Count > 0)
                {
                    Entity.ClientEntity.AddressExt addressExt = AddressExts.FirstOrDefault();
                    locationTenant.RSLCountryId = addressExt.AE_CountryID;
                    locationTenant.RSLStateName = addressExt.AE_StateName;
                    locationTenant.RSLCityName = addressExt.AE_CityName;
                    locationTenant.RSLZipCode = addressExt.AE_ZipCode;
                }

                //UploadDocument Panel Show hide
                if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty())
                {
                    pnlUploadDocument.Visible = true;
                    grdApplicantDoc.Visible = true;
                    grdApplicantDoc.Rebind();
                }
                else
                {
                    pnlUploadDocument.Visible = false;
                    grdApplicantDoc.Visible = false;
                }

                //If Need To Show Custom Form Data
                if (!CurrentViewContext.lstBkgOrderData.IsNullOrEmpty()
                    && !CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty()
                    && CurrentViewContext.lstSelectedPackageIds.Count() > AppConsts.NONE)
                {
                    //Bind Custom Forms
                    if (!IsOnlyMVRCustomForm())
                    {
                        pnlCustomFormLoad.Visible = true;
                        LoadCustomForm();
                        hdnIsCustomDataSaved.Value = "true";
                    }
                    else
                    {
                        pnlCustomFormLoad.Visible = false;
                    }
                }
                else
                {
                    pnlCustomFormLoad.Visible = false;
                }

                //Bind MVR Custom Form
                if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty() && CurrentViewContext.lstSelectedPackageIds.Count() > AppConsts.NONE)
                {
                    Presenter.GetAttributeFieldsOfSelectedPackages();
                    if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Count() > AppConsts.NONE
                        && CurrentViewContext.lstMvrAttGrp.Select(sel => sel.AttributeGrpId).FirstOrDefault() > AppConsts.NONE)
                    {
                        pnlMvrInfo.Visible = true;

                        if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Any(x => x.BSA_Code == LCNAttCode.ToString()))
                        {
                            dvDriverLicenseNo.Visible = true;
                        }
                        if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Any(x => x.BSA_Code == LCSAttCode.ToString()))
                        {
                            dvDriverLicenseState.Visible = true;
                            Presenter.GetAllStates();
                            if (CurrentViewContext.ListStates.IsNotNull())
                            {
                                BindDLState();
                            }
                        }
                        GetMvrData();
                    }
                    else
                    {
                        pnlMvrInfo.Visible = false;
                    }
                }
                else
                {
                    pnlMvrInfo.Visible = false;
                }

            }

        }

        /// <summary>
        /// Validate middle name if required
        /// </summary>
        /// <param name="middleName"></param>
        private void ValidateMiddleName(String middleName)
        {

            if (middleName.IsNullOrEmpty())
            {
                chkMiddleNameRequired.Checked = true;
                txtMiddleName.Text = NoMiddleNameText;
                txtMiddleName.Enabled = false;
                rfvMiddleName.Enabled = false;
                spnMiddleName.Style["display"] = "none";
            }
            else
            {
                chkMiddleNameRequired.Checked = false;
                txtMiddleName.Text = middleName;
                txtMiddleName.Enabled = true;
                rfvMiddleName.Enabled = true;
                spnMiddleName.Style["display"] = "";
                spnMiddleName.Visible = true;
            }
        }

        /// <summary>
        /// bind backgorund package for selected node id.
        /// </summary>
        private void BindPackageDropDown()
        {
            String DPMIds = hdnDepartmntPrgrmMppng.Value;
            CurrentViewContext.SelectedNodeId = hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(hdnDepartmntPrgrmMppng.Value);
            Presenter.GetBkgPackageDetailsForAdminOrder(DPMIds);
            if (!CurrentViewContext.lstBkgPackage.IsNullOrEmpty())
            {
                CurrentViewContext.HierarchyNodeID = CurrentViewContext.lstBkgPackage.FirstOrDefault().InsitutionHierarchyNodeID;
            }
            cmbBkgPackages.DataSource = CurrentViewContext.lstBkgPackage;
            cmbBkgPackages.DataBind();
        }

        /// <summary>
        /// Load custom details acc tp packges.
        /// </summary>
        private void LoadCustomForm()
        {
            SetCustomFormDetails();
            pnlCustomFormLoad.Visible = true;
            String url = String.Empty;
            //if (CurrentViewContext.IsOrderReadyForTransmit)
            //{
            //    url = String.Format(@"\BkgOperations\Pages\AdminOrderReviewMain.aspx?TenantId=" + CurrentViewContext.SelectedTenantId);
            //}
            //else
            //{
            //    url = String.Format(@"/BkgOperations/Pages/CustomFormPage.aspx?SelectedTenantId=" + CurrentViewContext.SelectedTenantId);
            //}
            url = String.Format(@"\BkgOperations\Pages\AdminOrderReviewMain.aspx?TenantId=" + CurrentViewContext.SelectedTenantId);
            //url = String.Format(@"/BkgOperations/Pages/CustomFormPage.aspx?SelectedTenantId=" + CurrentViewContext.SelectedTenantId + "&IsNewCustomForm=false&IsAdminEditMode=true");
            iframe.Src = String.Format(@url);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDLState()
        {
            cmbState.Items.Clear();
            cmbState.DataSource = CurrentViewContext.ListStates;
            cmbState.DataBind();
        }

        /// <summary>
        /// Save User,Package,custom form details.
        /// </summary>
        /// <param name="IsSaveButtonClick"></param>
        private void AddUpdateOrderDetails(Boolean IsSaveButtonClick)
        {
            Presenter.AddUpdateUser();
            if (!CurrentViewContext.OrganizationUser.IsNullOrEmpty() && CurrentViewContext.OrganizationUser.OrganizationUserID > AppConsts.NONE)
            {
                if ((CurrentViewContext.SelectedNodeId > AppConsts.NONE && CurrentViewContext.HierarchyNodeID > AppConsts.NONE)
                    || CurrentViewContext.OrderID > AppConsts.NONE)
                {
                    Presenter.AddUpdateOrderDetails();
                    if (!IsSaveButtonClick && CurrentViewContext.SelectedNodeId <= AppConsts.NONE && CurrentViewContext.HierarchyNodeID <= AppConsts.NONE)
                    {
                        base.ShowInfoMessage("Please select package to proceed");
                    }
                }
                else
                {
                    Presenter.SaveDefaultOrderDetail();
                    if (!IsSaveButtonClick)
                    {
                        base.ShowInfoMessage("Please select package to proceed");
                    }
                }
                if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty() && CurrentViewContext.OrderID > AppConsts.NONE && IsSaveButtonClick)
                {
                    GetCustomFormDetails();
                    if (!CurrentViewContext.lstBkgOrderData.IsNullOrEmpty())
                    {
                        Presenter.SaveCustomFormDetails();
                    }
                }
                if (!CurrentViewContext.lstApplicantDocumentContract.IsNullOrEmpty())
                {
                    Presenter.SaveGenericDocumentMapping();
                }
                //ShowHideResidentialsHistory();
                //ShowHidePersonalAliasDetails();
                Presenter.GetAdminOrderDetails();
                if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty() && CurrentViewContext.lstSelectedPackageIds.Count() > AppConsts.NONE)
                {
                    pnlUploadDocument.Visible = true;
                    grdApplicantDoc.Visible = true;
                    grdApplicantDoc.Rebind();
                    if (!IsOnlyMVRCustomForm())
                    {
                        LoadCustomForm();
                        pnlCustomFormLoad.Visible = true;

                    }
                    else
                    {
                        pnlCustomFormLoad.Visible = false;

                    }

                    Presenter.GetAttributeFieldsOfSelectedPackages();
                    if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Count() > AppConsts.NONE
                        && CurrentViewContext.lstMvrAttGrp.Select(sel => sel.AttributeGrpId).FirstOrDefault() > AppConsts.NONE)
                    {
                        //hdnIsCustomDataSaved.Value = "true";
                        pnlMvrInfo.Visible = true;

                        if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Any(x => x.BSA_Code == LCNAttCode.ToString()))
                        {
                            dvDriverLicenseNo.Visible = true;
                        }
                        if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Any(x => x.BSA_Code == LCSAttCode.ToString()))
                        {
                            dvDriverLicenseState.Visible = true;
                            Presenter.GetAllStates();
                            if (CurrentViewContext.ListStates.IsNotNull())
                            {
                                BindDLState();
                            }
                        }
                    }
                    else
                    {
                        pnlMvrInfo.Visible = false;
                    }

                }
                else
                {
                    pnlUploadDocument.Visible = false;
                    grdApplicantDoc.Visible = false;
                    pnlCustomFormLoad.Visible = false;
                    pnlMvrInfo.Visible = false;
                }
            }
            else
            {
                base.ShowInfoMessage("Please Enter Applicant details First.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetMvrData()
        {
            Int32 MVRCustomFormId = (_presenter.GetCustomFormIDBYCode() > 0) ? _presenter.GetCustomFormIDBYCode() : AppConsts.NONE;

            BackgroundOrderData lstCustomData = CurrentViewContext.lstBkgOrderData.Where(cond => cond.CustomFormId == MVRCustomFormId).FirstOrDefault();

            if (!lstCustomData.IsNullOrEmpty())
            {
                chkIsMVRInfo.Checked = true;
                if (lstCustomData.CustomFormData.Keys.Contains(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID))
                {
                    cmbState.Text = lstCustomData.CustomFormData.GetValue(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID);
                    //cmbState.Enabled = false;
                }

                if (lstCustomData.CustomFormData.Keys.Contains(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID))
                {
                    txtLicenseNO.Text = lstCustomData.CustomFormData.GetValue(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID);
                    //txtLicenseNO.Enabled = false;
                }
                if (cmbState.Text == "" && (txtLicenseNO.Text == "" || txtLicenseNO.Text == "N/A"))
                {
                    chkIsMVRInfo.Checked = false;
                }
            }
            else
            {
                chkIsMVRInfo.Checked = false;
            }

            MvrControls();
            //if (!chkIsMVRInfo.Checked)
            //{
            //    cmbState.ClearSelection();
            //    cmbState.Enabled = false;
            //    txtLicenseNO.Text = "N/A";
            //    txtLicenseNO.Enabled = false;
            //    //txtLicenseNO.ReadOnly = true;
            //    rfvLicenseState.Enabled = false;
            //    rfvtxtLicenseNO.Enabled = false;
            //}

        }

        ///
        private void MvrControls()
        {
            if (pnlMvrInfo.Visible == true)
            {
                if (chkIsMVRInfo.Checked)
                {
                    txtLicenseNO.Enabled = true;
                    cmbState.Enabled = true;
                    rfvLicenseState.Enabled = true;
                    rfvtxtLicenseNO.Enabled = true;
                    spnLicenseState.Style["display"] = "inline";
                    spnLicenseNumber.Style["display"] = "inline";
                    //spnLicenseNumber.Visible = true;
                }
                else
                {
                    cmbState.ClearSelection();
                    cmbState.Text = "";
                    cmbState.Enabled = false;
                    txtLicenseNO.Text = "N/A";
                    txtLicenseNO.Enabled = false;
                    //txtLicenseNO.ReadOnly = true;
                    rfvLicenseState.Enabled = false;
                    rfvtxtLicenseNO.Enabled = false;
                    spnLicenseState.Style["display"] = "none";
                    spnLicenseNumber.Style["display"] = "none";
                    //spnLicenseState.Style.Add("display", "none");
                    //spnLicenseNumber.Style.Add("display", "none");
                }
            }
        }


        /// <summary>
        /// set custom form details in session.
        /// </summary>
        private void SetCustomFormDetails()
        {
            ApplicantOrderCart applicantOrderCart = null;
            ApplicantOrder applicantOrder = null;

            if (applicantOrderCart.IsNullOrEmpty())
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                applicantOrder = new ApplicantOrder();
            }
            else
            {
                applicantOrder = applicantOrderCart.lstApplicantOrder[0];
            }

            applicantOrder.OrganizationUserProfile = CurrentViewContext.OrganizationUserProfile;
            applicantOrder.OrderId = CurrentViewContext.AdminCreateOrderContract.FirstOrDefault().OrderID;
            applicantOrder.lstPackages = CurrentViewContext.lstBkgPackage.Where(cond => CurrentViewContext.lstSelectedPackageIds.Contains(cond.BPHMId)).ToList();
            //If custom form details exists
            if (!CurrentViewContext.lstBkgOrderData.IsNullOrEmpty())
            {
                Int32 MVRCustomFormId = (_presenter.GetCustomFormIDBYCode() > 0) ? _presenter.GetCustomFormIDBYCode() : AppConsts.NONE;
                applicantOrder.lstBackgroundOrderData = CurrentViewContext.lstBkgOrderData.Where(cond => cond.CustomFormId != MVRCustomFormId).ToList();
            }

            applicantOrderCart.lstApplicantOrder.Add(applicantOrder);

            applicantOrderCart.lstPersonAlias = CurrentViewContext.PersonAliasList;
            applicantOrderCart.lstPrevAddresses = CurrentViewContext.ResidentialHistoryListAll;
            applicantOrderCart.TenantId = CurrentViewContext.SelectedTenantId;
            applicantOrderCart.AdminOrderSteps = 0;
            applicantOrderCart.IsAdminOrderCart = true;
            //UAt-3056
            if (!hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty())
            {
                applicantOrderCart.SelectedHierarchyNodeID = Convert.ToInt32(hdnDepartmntPrgrmMppng.Value);
            }

            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
        }

        /// <summary>
        /// Is only MVR Type of package is selected
        /// </summary>
        /// <returns></returns>
        private Boolean IsOnlyMVRCustomForm()
        {
            return Presenter.IsOnlyMVRPackage();
        }

        /// <summary>
        /// Get Data of Custom forms and mvr to save.
        /// </summary>
        private void GetCustomFormDetails()
        {
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            ApplicantOrder applicantOrder = null;
            List<BackgroundOrderData> lstBkgOrderData = null;
            Boolean IsCustomDataNeedToSave = hdnIsCustomDataSaved.Value.IsNullOrEmpty() ? false : Convert.ToBoolean(hdnIsCustomDataSaved.Value);

            if (applicantOrderCart.IsNullOrEmpty())
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                applicantOrder = new ApplicantOrder();
                lstBkgOrderData = new List<BackgroundOrderData>();
            }
            else
            {
                applicantOrder = applicantOrderCart.lstApplicantOrder[0];
                lstBkgOrderData = applicantOrder.lstBackgroundOrderData;                
            }

            applicantOrder.MVRIsValidDriverLicenseAndState = false;
            Int32 mvrBkgSvcAttributeGroupId = 0;
            if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Count > 0 && CurrentViewContext.lstMvrAttGrp.Select(x => x.AttributeGrpId).FirstOrDefault() > 0
                )
            {
                 mvrBkgSvcAttributeGroupId = Convert.ToInt32(CurrentViewContext.lstMvrAttGrp.Select(x => x.AttributeGrpId).FirstOrDefault());
                applicantOrder.MVRIsValidDriverLicenseAndState = true;

                if (CurrentViewContext.lstBkgOrderData.IsNullOrEmpty()
                    || !CurrentViewContext.lstBkgOrderData.Any(col => col.BkgSvcAttributeGroupId == mvrBkgSvcAttributeGroupId))
                {
                    if (applicantOrder.lstBackgroundOrderData.IsNullOrEmpty())
                    {
                        applicantOrder.lstBackgroundOrderData = new List<BackgroundOrderData>();
                    }
                    BackgroundOrderData backgroundOrderDataMVR = new BackgroundOrderData();
                    backgroundOrderDataMVR.InstanceId = AppConsts.ONE;
                    backgroundOrderDataMVR.CustomFormId = (_presenter.GetCustomFormIDBYCode() > 0) ? _presenter.GetCustomFormIDBYCode() : AppConsts.NONE;
                    backgroundOrderDataMVR.BkgSvcAttributeGroupId = Convert.ToInt32(CurrentViewContext.lstMvrAttGrp.Select(x => x.AttributeGrpId).FirstOrDefault());
                    backgroundOrderDataMVR.CustomFormData = new Dictionary<Int32, String>();
                    Int32 mappingID = 0;
                    if (dvDriverLicenseNo.Visible)
                    {
                        mappingID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID;
                        backgroundOrderDataMVR.CustomFormData.Add(mappingID, txtLicenseNO.Text);
                        applicantOrder.MVRDvrLicenseNumberID = mappingID;
                    }
                    if (dvDriverLicenseState.Visible)
                    {
                        mappingID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID;
                        backgroundOrderDataMVR.CustomFormData.Add(mappingID, cmbState.Text);
                        applicantOrder.MVRDvrLicenseNumberStateID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID;
                    }
                    applicantOrder.lstBackgroundOrderData.Insert(AppConsts.NONE, backgroundOrderDataMVR);
                }
                else
                {
                    Int32 mappingID = 0;

                    if (applicantOrder.lstBackgroundOrderData.IsNullOrEmpty())
                    {
                        applicantOrder.lstBackgroundOrderData = new List<BackgroundOrderData>();
                    }

                    BackgroundOrderData MVRBkgOrderData = CurrentViewContext.lstBkgOrderData.Where(col => col.BkgSvcAttributeGroupId == mvrBkgSvcAttributeGroupId).FirstOrDefault();
                    if (MVRBkgOrderData.CustomFormData.Keys.Contains(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID))
                    {
                        mappingID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID;
                        MVRBkgOrderData.CustomFormData[mappingID] = chkIsMVRInfo.Checked ? txtLicenseNO.Text : String.Empty;
                    }
                    if (MVRBkgOrderData.CustomFormData.Keys.Contains(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID))
                    {
                        mappingID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID;
                        MVRBkgOrderData.CustomFormData[mappingID] = chkIsMVRInfo.Checked ? cmbState.Text : String.Empty;
                    }
                    applicantOrder.lstBackgroundOrderData.Insert(AppConsts.NONE, MVRBkgOrderData);
                }

            }
            CurrentViewContext.lstBkgOrderData = new List<BackgroundOrderData>();
            if (!applicantOrder.lstBackgroundOrderData.IsNullOrEmpty())
            {
                foreach (BackgroundOrderData bkgOrderData in applicantOrder.lstBackgroundOrderData.ToList())
                {
                    if (bkgOrderData.BkgSvcAttributeGroupId == mvrBkgSvcAttributeGroupId || bkgOrderData.CustomFormData.Any(cond => cond.Value != "" && cond.Value != null))
                    {
                        CurrentViewContext.lstBkgOrderData.Add(bkgOrderData);
                    }
                }
            }
            //CurrentViewContext.lstBkgOrderData = applicantOrder.lstBackgroundOrderData;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docTypeCode"></param>
        private void UploadAllDocuments(String docTypeCode)
        {
            String filePath = String.Empty;
            Boolean aWSUseS3 = false;
            Boolean isCorruptedFileUploaded = false;
            StringBuilder corruptedFileMessage = new StringBuilder();
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
            StringBuilder docMessage = new StringBuilder();

            if (!WebConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(WebConfigurationManager.AppSettings["AWSUseS3"]);
            }
            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            if (aWSUseS3 == false)
            {
                if (filePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                    return;
                }
                if (!filePath.EndsWith("\\"))
                {
                    filePath += "\\";
                }

                filePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\";

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            Int32 ApplicantDocTypeID = Presenter.GetlkpDocTypeMapping(docTypeCode);
            CurrentViewContext.lstApplicantDocument = new List<ApplicantDocument>();
            if (DocumentType.Disclosure_n_Release.GetStringValue() == docTypeCode)
            {
                //if (!Presenter.IsDandAAlreadyUploaded(ApplicantDocTypeID))
                //{
                //    if ((uploadDandA.UploadedFiles.Count) == AppConsts.ONE)
                //    {
                foreach (UploadedFile item in uploadDandA.UploadedFiles)
                {
                    ApplicantDocument ApplicantDoc = new ApplicantDocument();
                    String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);
                    item.SaveAs(newTempFilePath);

                    byte[] fileBytes = File.ReadAllBytes(newTempFilePath);
                    String documentName = Presenter.IsDocumentAlreadyUploaded(item.FileName, item.ContentLength, fileBytes, docTypeCode);
                    if (!documentName.IsNullOrEmpty())
                    {
                        docMessage.Append("You have already updated " + item.FileName + " document as " + documentName + ". \\n");
                        continue;
                    }

                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        String destFilePath = Path.Combine(filePath, fileName);
                        File.Copy(newTempFilePath, destFilePath);
                        ApplicantDoc.DocumentPath = destFilePath;
                    }

                    else
                    {
                        if (filePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                            return;
                        }
                        if (!filePath.EndsWith("//"))
                        {
                            filePath += "//";
                        }
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = filePath + "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")/";
                        String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                        if (returnFilePath.IsNullOrEmpty())
                        {
                            isCorruptedFileUploaded = true;
                            corruptedFileMessage.Append("Your file " + item.FileName + " is not uploaded. \\n");
                            continue;
                        }
                        ApplicantDoc.DocumentPath = returnFilePath;
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(newTempFilePath))
                            File.Delete(newTempFilePath);
                    }
                    catch (Exception) { }
                    ApplicantDoc.FileName = item.FileName;
                    ApplicantDoc.Description = item.GetFieldValue("TextBox");
                    ApplicantDoc.DocumentType = ApplicantDocTypeID;
                    ApplicantDoc.Size = item.ContentLength;
                    ApplicantDoc.OriginalDocMD5Hash = Presenter.GetMd5Hash(fileBytes);
                    ApplicantDoc.CreatedByID = CurrentViewContext.CurrentLoggedInUserId;
                    ApplicantDoc.CreatedOn = DateTime.Now;
                    ApplicantDoc.IsDeleted = false;
                    ApplicantDoc.OrganizationUserID = CurrentViewContext.OrganizationUser.OrganizationUserID;
                    CurrentViewContext.lstApplicantDocument.Add(ApplicantDoc);
                }

                if (!CurrentViewContext.lstApplicantDocument.IsNullOrEmpty())
                {
                    if (Presenter.SaveApplicantDocumentDetails())
                    {
                        //Presenter.CallParallelTaskPdfConversion();
                        base.ShowSuccessMessage(" D&A document(s) uploaded successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("D&A document(s) does not uploaded successfully. Please try again");
                    }
                }

                if ((docMessage.Length > 0 && !(docMessage.ToString().IsNullOrEmpty())) || (corruptedFileMessage.Length > 0 && !(corruptedFileMessage.ToString().IsNullOrEmpty())))
                {
                    String ErrorMessage = docMessage.ToString() + ' ' + corruptedFileMessage.ToString();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + ErrorMessage + "');", true);
                }

                //    else
                //    {
                //        base.ShowInfoMessage("Please select one D & A document for the applicant.");
                //    }
                //}
                //else
                //{
                //    base.ShowInfoMessage("D and A document is already uploaded for this applicant.");
                //}
            }
            if (docTypeCode == DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue())
            {
                foreach (UploadedFile item in uploadAdditional.UploadedFiles)
                {
                    ApplicantDocument ApplicantDoc = new ApplicantDocument();
                    String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);
                    item.SaveAs(newTempFilePath);

                    byte[] fileBytes = File.ReadAllBytes(newTempFilePath);
                    String documentName = Presenter.IsDocumentAlreadyUploaded(item.FileName, item.ContentLength, fileBytes, docTypeCode);
                    if (!documentName.IsNullOrEmpty())
                    {
                        docMessage.Append("You have already updated " + item.FileName + " document as " + documentName + ". \\n");
                        continue;
                    }

                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        String destFilePath = Path.Combine(filePath, fileName);
                        File.Copy(newTempFilePath, destFilePath);
                        ApplicantDoc.DocumentPath = destFilePath;
                    }

                    else
                    {
                        if (filePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                            return;
                        }
                        if (!filePath.EndsWith("//"))
                        {
                            filePath += "//";
                        }
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = filePath + "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")/";
                        String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                        if (returnFilePath.IsNullOrEmpty())
                        {
                            isCorruptedFileUploaded = true;
                            corruptedFileMessage.Append("Your file " + item.FileName + " is not uploaded. \\n");
                            continue;
                        }
                        ApplicantDoc.DocumentPath = returnFilePath;
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(newTempFilePath))
                            File.Delete(newTempFilePath);
                    }
                    catch (Exception) { }
                    ApplicantDoc.FileName = item.FileName;
                    ApplicantDoc.Description = item.GetFieldValue("TextBox");
                    ApplicantDoc.DocumentType = ApplicantDocTypeID;
                    ApplicantDoc.Size = item.ContentLength;
                    ApplicantDoc.OriginalDocMD5Hash = Presenter.GetMd5Hash(fileBytes);
                    ApplicantDoc.CreatedByID = CurrentViewContext.CurrentLoggedInUserId;
                    ApplicantDoc.CreatedOn = DateTime.Now;
                    ApplicantDoc.IsDeleted = false;
                    ApplicantDoc.OrganizationUserID = CurrentViewContext.OrganizationUser.OrganizationUserID;

                    CurrentViewContext.lstApplicantDocument.Add(ApplicantDoc);

                }

                if (!CurrentViewContext.lstApplicantDocument.IsNullOrEmpty())
                {
                    if (Presenter.SaveApplicantDocumentDetails())
                    {
                        //Presenter.CallParallelTaskPdfConversion();
                        base.ShowSuccessMessage("Document(s) uploaded successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Document(s) does not uploaded successfully. Please try again");
                    }
                }

                if ((docMessage.Length > 0 && !(docMessage.ToString().IsNullOrEmpty())) || (corruptedFileMessage.Length > 0 && !(corruptedFileMessage.ToString().IsNullOrEmpty())))
                {
                    String ErrorMessage = docMessage.ToString() + ' ' + corruptedFileMessage.ToString();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + ErrorMessage + "');", true);
                }
            }
            grdApplicantDoc.Rebind();
        }

        /// <summary>
        /// show hide residential history details and set max min occurance
        /// </summary>
        private void ShowHideResidentialsHistory()
        {
            if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty())
            {
                List<BackgroundPackagesContract> lstPackages = CurrentViewContext.lstBkgPackage.Where(cond => CurrentViewContext.lstSelectedPackageIds.Contains(cond.BPHMId)).ToList();
                List<PackageGroupContract> lstPackageGroupContract = Presenter.CheckShowResidentialHistory(CurrentViewContext.SelectedTenantId, lstPackages.Select(col => col.BPAId).ToList());

                if (lstPackageGroupContract.IsNull() || (lstPackageGroupContract.IsNotNull() && lstPackageGroupContract.Count == 0))
                {
                    dvResHistory.Style["display"] = "none"; 
                    PrevResident.IsApplicantOrderScreen = true;
                    PrevResident.MaxOccurance = AppConsts.NONE;
                    PrevResident.MaxNumberOfYearforResidence = -1;
                    CurrentViewContext.MinResidentailHistoryOccurances = AppConsts.NONE;
                    CurrentViewContext.MaxResidentailHistoryOccurances = AppConsts.NONE;
                }
                else
                {
                    dvResHistory.Style["display"] = "block";
                    if (!lstPackages.IsNullOrEmpty() && lstPackages.Count > 0)
                    {
                        dvResHistory.Visible = true;
                        Presenter.GetMinMaxResidentailHistoryOccurances(CurrentViewContext.SelectedTenantId, lstPackages.Select(col => col.BPAId).ToList());
                        PrevResident.MaxOccurance = CurrentViewContext.MaxResidentailHistoryOccurances;
                        PrevResident.IsApplicantOrderScreen = true;
                        if (lstPackages.Any(x => x.MaxNumberOfYearforResidence == -1))
                        {
                            PrevResident.MaxNumberOfYearforResidence = -1;
                        }
                        else
                        {
                            var maxresidenceAddress = lstPackages.Where(x => lstPackageGroupContract.Select(t => t.PackageId).Contains(x.BPAId)).OrderByDescending(y => y.MaxNumberOfYearforResidence).ToList();
                            if (!maxresidenceAddress.IsNullOrEmpty())
                            {
                                PrevResident.MaxNumberOfYearforResidence = maxresidenceAddress[0].MaxNumberOfYearforResidence;
                            }
                        }
                        PrevResident.RebindGrid();
                    }
                }
            }
        }

        /// <summary>
        /// set max min of personal alias details.
        /// </summary>
        private void ShowHidePersonalAliasDetails()
        {
            if (!CurrentViewContext.lstSelectedPackageIds.IsNullOrEmpty() && !CurrentViewContext.lstBkgPackage.IsNullOrEmpty())
            {
                List<BackgroundPackagesContract> lstPackages = CurrentViewContext.lstBkgPackage.Where(cond => CurrentViewContext.lstSelectedPackageIds.Contains(cond.BPHMId)).ToList();
                Presenter.GetMinMaxPaersonalAliasOccurances(CurrentViewContext.SelectedTenantId, lstPackages.Select(sel => sel.BPAId).ToList());
                ucPersonAlias.MaxOccurance = CurrentViewContext.MaxPersonalAliasOccurances.IsNullOrEmpty() ? AppConsts.ONE : CurrentViewContext.MaxPersonalAliasOccurances;
                dvPersonalAlias.Style["display"] = "block";
            }
        }

        /// <summary>
        /// show hide phone number textbox basis of internation setting
        /// </summary>
        private void ShowHideInternationalPhoneNumberControl()
        {
            if (CurrentViewContext.IsInternationalPhoneNumber)
            {
                dvUnmaskedPrimaryPhone.Style["display"] = "block";
                dvMaskedPrimaryPhone.Style["display"] = "none";
                revTxtMobile.Enabled = false;
                rfvTxtMobile.Enabled = false;
                rfvTxtMobileUnmasked.Enabled = true;
                revTxtMobilePrmyNonMasking.Enabled = true;
            }
            else
            {

                dvUnmaskedPrimaryPhone.Style["display"] = "none";
                dvMaskedPrimaryPhone.Style["display"] = "block";
                revTxtMobile.Enabled = true;
                rfvTxtMobile.Enabled = true;
                rfvTxtMobileUnmasked.Enabled = false;
                revTxtMobilePrmyNonMasking.Enabled = false;
            }
        }

        #endregion

        #endregion
    }
}