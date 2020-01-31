using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using System.Text;
using System.Collections;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using INTSOF.UI.Contract.FingerPrintSetup;
using Telerik.Web.UI;
using System.Globalization;
using INTSOF.UI.Contract.Globalization;
using System.Web.Configuration;
using CoreWeb.AdminEntryPortal.Views;
#endregion

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryApplicantInformation : BaseUserControl, IAdminEntryApplicantInformationView
    {
        #region Variables

        #region Private Variables

        private AdminEntryApplicantInformationPresenter _presenter = new AdminEntryApplicantInformationPresenter();
        private Int32 _tenantid;
        private Guid LCSAttCode = new Guid("1ADA97AE-9100-4BE6-B829-C914B7FA8750");//Driver's License State
        private Guid LCNAttCode = new Guid("515BEF57-9072-4D2A-A97A-0C248BB045F9");////License Number
        private Guid MotherNameAttrCode = new Guid("3DA8912A-6337-4B8F-93C4-88BFC3032D2D");////Mother's Maiden Name
        private Guid IdentificationNumberAttrCode = new Guid("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211");////Identification Number
        private ApplicantOrderCart applicantOrderCart = new ApplicantOrderCart();
        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private Int32 NodeId
        {
            get
            {
                if (ViewState["NodeId"] != null)
                    return (Int32)(ViewState["NodeId"]);

                return AppConsts.NONE;
            }
            set
            {
                if (ViewState["NodeId"] == null)
                    ViewState["NodeId"] = value;
            }
        }

        private Int32 customFormdataId
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart.lstCustomFormData.IsNullOrEmpty())
                {
                    return 0;
                }
                return applicantOrderCart.lstCustomFormData.FirstOrDefault().customFormId;
            }
        }


        public List<BackgroundOrderData> lstBackgroundOrderData
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (!applicantOrderCart.IsNullOrEmpty())
                    return applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;
                return new List<BackgroundOrderData>();
            }
        }

        /// <summary>
        /// DeptProgramId of the selected Package from Pending order.
        /// Used to get the Custom attribute data of the applicant for this level
        /// </summary>
        private Int32 DeptProgId
        {
            get
            {
                if (ViewState["DeptProgId"] != null)
                    return (Int32)(ViewState["DeptProgId"]);

                return AppConsts.NONE;
            }
            set
            {
                if (ViewState["DeptProgId"] == null)
                    ViewState["DeptProgId"] = value;
            }
        }

        /// <summary>
        /// To Store Department Program Package ID. Will be used to fetch DPM_ID to get custom attributes.
        /// </summary>
        private Int32? DPP_ID
        {
            get
            {
                if (ViewState["DPPID"] != null)
                    return (Int32)(ViewState["DPPID"]);

                return null;
            }
            set
            {
                if (ViewState["DPPID"] == null)
                    ViewState["DPPID"] = value;
            }
        }

        /// <summary>
        /// To Store Bkg Package Hierarchy Mapping ID. Will be used to fetch DPM_ID to get custom attributes.
        /// </summary>
        private Int32? BPHM_ID
        {
            get
            {
                if (ViewState["BPHMID"] != null)
                    return (Int32)(ViewState["BPHMID"]);

                return null;
            }
            set
            {
                if (ViewState["BPHMID"] == null)
                    ViewState["BPHMID"] = value;
            }
        }

        /// <summary>
        /// Will be used to check the custom attributes are fetched from order flow or any other screen.
        /// </summary>
        private Boolean IsOrder
        {
            get
            {
                if (ViewState["IsOrder"] != null)
                    return (Boolean)(ViewState["IsOrder"]);

                return false;
            }
            set
            {
                if (ViewState["IsOrder"] == null)
                    ViewState["IsOrder"] = value;
            }
        }

        Boolean IAdminEntryApplicantInformationView.IsSSNDisabled
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsSSNDisabled"] ?? false);
            }
            set
            {
                ViewState["IsSSNDisabled"] = value;
            }
        }
        //Boolean IAdminEntryApplicantInformationView.IsSuffixDropDownType
        //{
        //    get
        //    {
        //        if (ViewState["IsSuffixDropDownType"] != null)
        //            return (Boolean)(ViewState["IsSuffixDropDownType"]);

        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["IsSuffixDropDownType"] = value;
        //    }

        //}

        #region CBI CABS
        //Boolean IAdminEntryApplicantInformationView.IsLocationServiceTenant
        //{
        //    get
        //    {
        //        if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IsLocationServiceTenant"])))
        //            return (Boolean)ViewState["IsLocationServiceTenant"];
        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["IsLocationServiceTenant"] = value;
        //        hdnIsLocationTenant.Value = Convert.ToString(value);
        //    }
        //}

        //List<Entity.lkpSuffix> IAdminEntryApplicantInformationView.lstSuffixes
        //{
        //    get
        //    {
        //        if (!ViewState["lstSuffixes"].IsNullOrEmpty())
        //            return (List<Entity.lkpSuffix>)ViewState["lstSuffixes"];
        //        return new List<Entity.lkpSuffix>();
        //    }
        //    set
        //    {
        //        ViewState["lstSuffixes"] = value;
        //    }
        //}

        List<Entity.lkpAdminEntrySuffix> IAdminEntryApplicantInformationView.lstSuffixes
        {
            get
            {
                if (!ViewState["lstSuffixes"].IsNullOrEmpty())
                    return (List<Entity.lkpAdminEntrySuffix>)ViewState["lstSuffixes"];
                return new List<Entity.lkpAdminEntrySuffix>();
            }
            set
            {
                ViewState["lstSuffixes"] = value;
            }
        }

        //Int32? IAdminEntryApplicantInformationView.SelectedSuffixID
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}

        #endregion

        #region UAT-2169:Send Middle Name and Email address to clearstar in Complio

        private String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = String.Empty;
                //if (!CurrentViewContext.IsLocationServiceTenant)  // Check for location service tenant, if it is a loaction service tenant then no middle name text should be blank rather than '-----'.
                noMiddleNameText = AppConsts.NO_MIDDLE_NAME_TEXT_AEP;
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }
        #endregion

        #endregion

        #region Public Properties


        public AdminEntryApplicantInformationPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        public Entity.ClientEntity.OrganizationUser OrganizationUser
        {
            get
            {
                if (ViewState["OrganizationUser"] != null)
                    return (Entity.ClientEntity.OrganizationUser)ViewState["OrganizationUser"];
                return null;

            }
            set
            {
                ViewState["OrganizationUser"] = value;
                if (value != null)
                    AddressHandleId = value.AddressHandleID;
            }
        }

        public String CurrentEmailAddress
        {
            get
            {
                if (ViewState["CurrentEmailAddress"].IsNotNull())
                    return ViewState["CurrentEmailAddress"].ToString().Trim().ToLower();
                return String.Empty;
            }
            set
            {
                ViewState["CurrentEmailAddress"] = value.Trim();
            }
        }

        public String VerifiedEmailAddress
        {
            get
            {
                if (ViewState["VerifiedEmailAddress"].IsNotNull())
                    return ViewState["VerifiedEmailAddress"].ToString().Trim().ToLower();
                return String.Empty;
            }
            set
            {
                ViewState["VerifiedEmailAddress"] = value.Trim();
            }
        }

        public IAdminEntryApplicantInformationView CurrentViewContext
        {
            get { return this; }
        }

        public Guid? AddressHandleId
        {
            get
            {
                if (ViewState["AddressHandleId"] != null)
                    return (Guid)ViewState["AddressHandleId"];
                return Guid.NewGuid();
            }
            set
            {
                ViewState["AddressHandleId"] = value;
            }
        }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        public String NextPagePath { get; set; }

        public List<TypeCustomAttributes> lst { get; set; }

        public List<AttributeFieldsOfSelectedPackages> lstMvrAttGrp
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

        List<AttributeFieldsOfSelectedPackages> IAdminEntryApplicantInformationView.LstInternationCriminalSrchAttributes
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

        Int32 IAdminEntryApplicantInformationView.MinResidentailHistoryOccurances
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

        Int32? IAdminEntryApplicantInformationView.MaxResidentailHistoryOccurances
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

        public String UserFirstName
        {
            get
            {
                return ucPersonAlias.UserFirstName;
            }
            set
            {
                ucPersonAlias.UserFirstName = value;
            }
        }

        public String UserLastName
        {
            get
            {
                return ucPersonAlias.UserLastName;
            }
            set
            {
                ucPersonAlias.UserLastName = value;
            }
        }

        public String UserMiddleName
        {
            get
            {
                return ucPersonAlias.UserMiddleName;
            }
            set
            {
                ucPersonAlias.UserMiddleName = value;
            }
        }

        Boolean IAdminEntryApplicantInformationView.IsPersonalInformationGroupExist { get; set; }

        String IAdminEntryApplicantInformationView.PersonalInformationInstructionText { get; set; }

        String IAdminEntryApplicantInformationView.ResidentialHistoryInstructionText { get; set; }

        #region Min Max Occurance For Personal Alias
        Int32 IAdminEntryApplicantInformationView.MinPersonalAliasOccurances
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

        Int32? IAdminEntryApplicantInformationView.MaxPersonalAliasOccurances
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
        #endregion
        public List<Entity.State> ListStates { get; set; }

        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set
            {
                _tenantid = value;
            }
        }


        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        public String OrderType
        {
            get
            {
                return Convert.ToString(ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE]);
            }
            set
            {
                ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE] = value;
            }
        }

        //UAT-781
        public String DecryptedSSN { get; set; }

        //UAT 1438
        public Boolean IsUserGroupCustomAttributeExist
        {
            get
            {
                if (ViewState["IsUserGroupCustomAttributeExist"] != null)
                {
                    return Convert.ToBoolean(ViewState["IsUserGroupCustomAttributeExist"]);
                }
                return false;
            }
            set
            {
                ViewState["IsUserGroupCustomAttributeExist"] = value;
            }
        }

        public IQueryable<UserGroup> lstUserGroups { get; set; }

        public IList<UserGroup> lstUserGroupsForUser { get; set; }

        //UAT-3455
        public IList<UserGroup> lstUsrGrpSavedValues { get; set; }

        #region UAT-1578 : Addition of SMS notification
        //Boolean IAdminEntryApplicantInformationView.IsReceiveTextNotification
        //{
        //    get
        //    {
        //        return Convert.ToBoolean(rdbTextNotification.SelectedValue);
        //    }
        //    set
        //    {
        //        rdbTextNotification.SelectedValue = Convert.ToString(value);
        //    }
        //}
        String IAdminEntryApplicantInformationView.PhoneNumber
        {
            get;
            //{
            //    return txtPhoneNumber.Text;
            //}
            set;
            //{
            //    txtPhoneNumber.Text = value;
            //}
        }
        //Entity.OrganisationUserTextMessageSetting IAdminEntryApplicantInformationView.OrganisationUserTextMessageSettingData { get; set; }
        #endregion

        public Int32 BkgPackageID
        {
            get
            {
                if (ViewState["BkgPackageID"] != null)
                    return (Int32)(ViewState["BkgPackageID"]);

                return AppConsts.NONE;
            }
            set
            {
                if (ViewState["BkgPackageID"] == null)
                    ViewState["BkgPackageID"] = value;
            }
        }

        public Int32 OrderNodeID
        {
            get
            {
                if (ViewState["OrderNodeID"] != null)
                    return (Int32)(ViewState["OrderNodeID"]);

                return AppConsts.NONE;
            }
            set
            {
                if (ViewState["OrderNodeID"] == null)
                    ViewState["OrderNodeID"] = value;
            }
        }

        public Int32 HierarchyNodeID
        {
            get
            {
                if (ViewState["HierarchyNodeID"] != null)
                    return (Int32)(ViewState["HierarchyNodeID"]);

                return AppConsts.NONE;
            }
            set
            {
                if (ViewState["HierarchyNodeID"] == null)
                    ViewState["HierarchyNodeID"] = value;
            }
        }

        public Int32 BulkOrderUploadID
        {
            get
            {
                if (ViewState["BulkOrderUploadID"] != null)
                    return (Int32)(ViewState["BulkOrderUploadID"]);

                return AppConsts.NONE;
            }
            set
            {
                if (ViewState["BulkOrderUploadID"] == null)
                    ViewState["BulkOrderUploadID"] = value;
            }
        }

        //UAT-3455
        Boolean IAdminEntryApplicantInformationView.IsMultipleValsSelected
        {
            get
            {
                if (!ViewState["IsMultipleValsSelected"].IsNullOrEmpty())
                    return (Boolean)ViewState["IsMultipleValsSelected"];
                return false;
            }
            set
            {
                caOtherDetails.IsMultipleValsSelected = value;
                ViewState["IsMultipleValsSelected"] = value;
            }
        }

        Boolean IAdminEntryApplicantInformationView.IsMultiSelectionAllowed
        {
            get
            {
                if (!ViewState["IsMultiSelectionAllowed"].IsNullOrEmpty())
                    return (Boolean)ViewState["IsMultiSelectionAllowed"];
                return false;
            }
            set
            {
                ViewState["IsMultiSelectionAllowed"] = value;
            }
        }

        List<Int32> IAdminEntryApplicantInformationView.lstPreviousSelectedUserGroupIds
        {
            get
            {
                if (!ViewState["lstPreviousSelectedUserGroupIds"].IsNullOrEmpty())
                    return (List<Int32>)ViewState["lstPreviousSelectedUserGroupIds"];
                return null;
            }
            set
            {
                ViewState["lstPreviousSelectedUserGroupIds"] = value;
            }
        }

        public Boolean IsHavingSSN
        {
            get
            {
                if (!rblSSN.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToBoolean(rblSSN.SelectedValue.ToLower());

                }
                return false;
            }
            set
            {
                rblSSN.SelectedValue = value.ToString().ToLower();
            }
        }

        //public String UserSuffix
        //{
        //    get
        //    {
        //        return ucPersonAlias.UserSuffix;
        //    }
        //    set
        //    {
        //        ucPersonAlias.UserSuffix = value;
        //    }
        //}


        public List<Entity.lkpLanguage> CommLanguage
        {

            set
            {
                cmbCommLang.DataSource = value;
                cmbCommLang.DataBind();
            }

        }
        Int32? IAdminEntryApplicantInformationView.SelectedCommLang { get; set; }

        String IAdminEntryApplicantInformationView.LanguageCode
        {
            get
            {
                LanguageContract languageContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!languageContract.IsNullOrEmpty())
                {
                    return languageContract.LanguageCode;
                }
                return Languages.ENGLISH.GetStringValue();
            }
        }

        Int32 IAdminEntryApplicantInformationView.OrderID { get; set; }

        public Int32 CurrentAddressId
        {
            get
            {
                if (!ViewState["CurrentAddressId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["CurrentAddressId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["CurrentAddressId"] = value;
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
                hdnLanguageCode.Value = CurrentViewContext.LanguageCode;
                base.Title = Resources.Language.ORDER;
                base.BreadCrumbTitleKey = "Key_ORDER";
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
            if (!this.IsPostBack)
            {
                CaptureQueryString();
                //Set MinDate and MaxDate for DOB
                //Release 158 CBI

                ucPersonAlias.SelectedTenantId = CurrentViewContext.TenantId;
                ucPersonAlias.PageType = PersonAliasPageType.ApplicantProfile.GetStringValue();
                divInternationalCriminalSearchAttributes.Style.Add("display", "none");
                divMothersName.Style.Add("display", "none");
                divCriminalLicenseNumber.Style.Add("display", "none");
                divIdentificationNumber.Style.Add("display", "none");
                dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                rngvDOB.MaximumValue = DateTime.Now.Date.AddYears(-1).ToShortDateString();
                rngvDOB.MinimumValue = Convert.ToDateTime("01-01-1900").ToShortDateString();
                //rngvDOB.ErrorMessage = "Date of birth should not be less than a year.";
                rngvDOB.ErrorMessage = Resources.Language.DOBNOTLESSTHANYEAR;
                //Set MinDate and MaxDate for ResidentFrom
                dpCurResidentFrom.MinDate = Convert.ToDateTime("01-01-1900");
                dpCurResidentFrom.MaxDate = DateTime.Now;
                Presenter.OnViewInitialized();
                #region CBI CABS
                //Presenter.IsLocationServiceTenant();
                //if (CurrentViewContext.IsLocationServiceTenant)
                //{
                //    //Presenter.IsDropDownSuffixType();
                //    chkUpdatePersonalDetailsDiv.Visible = false;
                //    chkUpdatePersonalDetails.Checked = true;
                //    rngvDOB.MaximumValue = DateTime.Now.Date.AddYears(-10).ToShortDateString();

                //    rngvDOB.ErrorMessage = Resources.Language.DOBNOTLESSTHAT10YEAR;
                //    dpkrDOB.DateInput.ClientEvents.OnKeyPress = "OnKeyPress";
                //    dvCommLang.Style.Add("Display", "");   //UAT-3824              
                //    //UAT-3860
                //    dvAddress2.Visible = false;

                //    lblAddress1.Text = Resources.Language.ADDRESS;
                //    rfvAddress1.ErrorMessage = Resources.Language.ADDRESSREQ;
                //}
                //else
                //{
                lblAddress1.Text = Resources.Language.ADDRESS1;
                rfvAddress1.ErrorMessage = Resources.Language.ADDRESS1REQ;
                //}
                //locationTenant.IsLocationServiceTenant = CurrentViewContext.IsLocationServiceTenant;//UAT-3910

                AddSuffixDropdownAndDesignChange();
                #endregion

                #region UAT-1834: NYU Migration 2 of 3: Applicant Complete Order Process

                //comment if not required

                if (!Session[AppConsts.BULK_ORDER_UPLOAD_DATA].IsNullOrEmpty())
                {
                    Entity.ClientEntity.BulkOrderUpload bulkOrderUploadSession = Session[AppConsts.BULK_ORDER_UPLOAD_DATA] as Entity.ClientEntity.BulkOrderUpload;

                    if (bulkOrderUploadSession.IsNotNull())
                    {
                        CurrentViewContext.BulkOrderUploadID = bulkOrderUploadSession.BOU_ID;
                        CurrentViewContext.BkgPackageID = bulkOrderUploadSession.BOU_PackageID ?? AppConsts.NONE;
                        CurrentViewContext.OrderNodeID = bulkOrderUploadSession.BOU_OrderNodeID ?? AppConsts.NONE;
                        CurrentViewContext.HierarchyNodeID = bulkOrderUploadSession.BOU_HierarchyNodeID ?? AppConsts.NONE;
                        Session.Remove(AppConsts.BULK_ORDER_UPLOAD_DATA);

                        if (CurrentViewContext.BulkOrderUploadID > AppConsts.NONE && CurrentViewContext.BkgPackageID > AppConsts.NONE && CurrentViewContext.OrderNodeID > AppConsts.NONE
                                && CurrentViewContext.HierarchyNodeID > AppConsts.NONE)
                        {
                            StartOrder();
                        }
                    }
                }

                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);

                if (applicantOrderCart == null)
                {
                    applicantOrderCart = new ApplicantOrderCart();
                    applicantOrderCart.GetApplicantOrder();
                }

                #endregion


                RedirectInvalidOrder(applicantOrderCart);
                //applicantOrderCart.IsAccountUpdated = chkUpdatePersonalDetails.Checked;
                CurrentViewContext.OrderType = applicantOrderCart.OrderRequestType;


                #region License Number,License State for MVR
                //get The AttributegrpId For MVR grp,According hide the Divs on basis of Code
                if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0)
                {
                    CurrentViewContext.BkgPackageID = CurrentViewContext.BkgPackageID > AppConsts.NONE ? CurrentViewContext.BkgPackageID : applicantOrderCart.lstApplicantOrder[0].lstPackages[0].BPAId;
                    Presenter.GetAttributeFieldsOfSelectedPackages(GetPackageIdString(applicantOrderCart));
                    if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Count > 0 &&
                        CurrentViewContext.lstMvrAttGrp.Select(x => x.AttributeGrpId).FirstOrDefault() > 0)
                    {

                        if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Any(x => x.BSA_Code == LCNAttCode.ToString()))
                        {
                            divMVRInfo.Visible = true;
                            dvDriverLicenseNo.Visible = true;
                        }
                        if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Any(x => x.BSA_Code == LCSAttCode.ToString()))
                        {
                            dvDriverLicenseState.Visible = true;
                            Presenter.GetAllStates();
                            if (CurrentViewContext.ListStates.IsNotNull())
                            {
                                divMVRInfo.Visible = true;
                                BindCombo(cmbState, CurrentViewContext.ListStates);
                            }
                        }
                    }

                    if (!CurrentViewContext.LstInternationCriminalSrchAttributes.IsNullOrEmpty())
                    {
                        if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == LCNAttCode.ToString() && x.IsAttributeDisplay))
                        {
                            locationTenant.ShowCriminalAttribute_License = true;
                            PrevResident.ShowCriminalAttribute_License = true;
                            if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == LCNAttCode.ToString() && x.IsAttributeRequired))
                            {
                                hdnLicenseRequired.Value = "True";
                            }
                        }
                        if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == MotherNameAttrCode.ToString() && x.IsAttributeDisplay))
                        {
                            locationTenant.ShowCriminalAttribute_MotherName = true;
                            PrevResident.ShowCriminalAttribute_MotherName = true;
                            if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == MotherNameAttrCode.ToString() && x.IsAttributeRequired))
                            {
                                hdnIsMotherNameRequired.Value = "True";
                            }
                        }
                        if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == IdentificationNumberAttrCode.ToString() && x.IsAttributeDisplay))
                        {
                            locationTenant.ShowCriminalAttribute_Identification = true;
                            PrevResident.ShowCriminalAttribute_Identification = true;
                            if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == IdentificationNumberAttrCode.ToString() && x.IsAttributeRequired))
                            {
                                hdnIsIdentificationRequired.Value = "True";
                            }
                        }
                    }

                    #region Show Residentail History Check


                    applicantOrderCart.IsResidentialHistoryVisible = true;
                    PrevResident.IsApplicantOrderScreen = true;
                    List<PackageGroupContract> lstPackageGroupContract = Presenter.CheckShowResidentialHistory(CurrentViewContext.TenantId, applicantOrderCart.lstApplicantOrder[0].lstPackages.Select(col => col.BPAId).ToList());
                    if (lstPackageGroupContract.IsNull() || (lstPackageGroupContract.IsNotNull() && lstPackageGroupContract.Count == 0))
                    {
                        dvResHistory.Visible = false;
                        //chkShowResidential.Visible = false;
                        dvResHistoryText.Visible = false;
                        applicantOrderCart.IsResidentialHistoryVisible = false;
                    }
                    else
                    {
                        if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0)
                        {
                            var pkgResiHistoryDataList = applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(x => lstPackageGroupContract.Select(t => t.PackageId).Contains(x.BPAId)).ToList();
                            var maxNoOfYearResidence = pkgResiHistoryDataList.Max(xx => xx.MaxNumberOfYearforResidence);
                            //End: UAT-4540

                            //if (applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(x => x.MaxNumberOfYearforResidence == -1))                            
                            if (pkgResiHistoryDataList.Any(x => x.MaxNumberOfYearforResidence == -1))
                            {
                                PrevResident.MaxNumberOfYearforResidence = -1;
                                //chkShowResidential.Text = "I have Residential History.";
                            }
                            else
                            {
                                var maxresidenceAddress = applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(x => lstPackageGroupContract.Select(t => t.PackageId).Contains(x.BPAId)).OrderByDescending(y => y.MaxNumberOfYearforResidence).ToList();
                                if (!maxresidenceAddress.IsNullOrEmpty())
                                {
                                    PrevResident.MaxNumberOfYearforResidence = maxresidenceAddress[0].MaxNumberOfYearforResidence;
                                    //  chkShowResidential.Text = "I have Residential History in " + PrevResident.MaxNumberOfYearforResidence + " number of years";
                                }
                            }
                        }
                    }
                    if (!CurrentViewContext.IsPersonalInformationGroupExist)
                    {
                        divPersonalInfoInst.Visible = false;
                    }
                    Presenter.ShowInstructionTextForResiHistory(CurrentViewContext.TenantId, applicantOrderCart.lstApplicantOrder[0].lstPackages.Select(col => col.BPAId).ToList());

                    if (dvResHistory.Visible && dvResHistoryText.Visible)
                    {
                        litresHistoryText.Text = CurrentViewContext.ResidentialHistoryInstructionText;
                        if ((litresHistoryText.Text.Trim().IsNullOrEmpty()))
                        {
                            dvResHistoryText.Visible = false;
                        }
                    }
                    if (divPersonalInfoInst.Visible)
                    {
                        litPersonalInfoInst.Text = CurrentViewContext.PersonalInformationInstructionText;
                        if ((litPersonalInfoInst.Text.Trim().IsNullOrEmpty()))
                        {
                            divPersonalInfoInst.Visible = false;
                        }
                    }

                    #endregion

                    #region Min/Max occurance
                    //Get the Max & Min Occurances for different attribute Group
                    Presenter.GetMinMaxPaersonalAliasOccurances(CurrentViewContext.TenantId, applicantOrderCart.lstApplicantOrder[0].lstPackages
                                                                                                                                        .Select(col => col.BPAId).ToList());

                    Presenter.GetMinMaxResidentailHistoryOccurances(CurrentViewContext.TenantId, applicantOrderCart.lstApplicantOrder[0].lstPackages
                                                                                                                                        .Select(col => col.BPAId).ToList());
                    #endregion

                    #region HideRedidentialHistory
                    //Added check to hide residential histories on the basis of max occurances.UAT-605
                    if (CurrentViewContext.MaxResidentailHistoryOccurances > 0 && CurrentViewContext.MaxResidentailHistoryOccurances - AppConsts.ONE == AppConsts.NONE)
                    {
                        dvResHistory.Visible = false;
                        //chkShowResidential.Visible = false;
                    }
                    #endregion
                }
                // only complio package 
                else
                {
                    dvResHistory.Visible = false;
                    dvBackgroundReport.Visible = false;
                    chkSendBkgReport.Checked = false;
                }
                #endregion

                BindData(applicantOrderCart);

                String _currentStep = " (" + Resources.Language.STEP + " " + (applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) +
                                                   " " + Resources.Language.OF + " " + applicantOrderCart.GetTotalOrderSteps() + ")";

                base.SetPageTitle(_currentStep);

                (this.Page as CoreWeb.AdminEntryPortal.Views.Default).SetModuleTitle(Resources.Language.CMPLTORDER);

                Presenter.GetSSNSetting();

                #region UAT-2169:Send Middle Name and Email address to clearstar in Complio
                hdnNoMiddleNameText.Value = NoMiddleNameText;
                #endregion


                //UAT 3573
                //if (applicantOrderCart.IsLocationServiceTenant)
                //{
                //    dvBackgroundReport.Style.Add("display", "none");
                //    chkSendBkgReport.Checked = false;
                //}

            }
            if (locationTenant.RSLCountryId != AppConsts.COUNTRY_USA_ID && locationTenant.RSLCountryId != AppConsts.NONE)
            {
                HideShowInternationalCriminalAttributes();
            }
            Presenter.OnViewLoaded();
            //base.SetPageTitle("Order");
            UserFirstName = txtFirstName.Text;
            UserLastName = txtLastName.Text;
            //UAT-1612:
            if (chkMiddleNameRequired.Checked)
            {
                ValidateMiddleName(String.Empty);
            }
            else
            {
                ValidateMiddleName(txtMiddleName.Text);
            }

            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
            UserMiddleName = txtMiddleName.Text;
            //CBI || CABS
            //if (CurrentViewContext.IsLocationServiceTenant)
            //{
            //    UserSuffix = txtSuffix.Text;
            //    revAddress1.ErrorMessage = Resources.Language.ADDRESSASCIIINVALIDCODE;
            //    revAddress1.Enabled = true;
            //}
            GenerateCustomAttributes();

            INTERSOFT.WEB.UI.WebControls.WclButton btnSave = (fsucCmdBar1.FindControl("btnSave") as INTERSOFT.WEB.UI.WebControls.WclButton);
            if (btnSave.IsNotNull())
                btnSave.CausesValidation = false;

            // fsucCmdBar1.ClearButton.ToolTip = "Continue to next step";
            fsucCmdBar1.ClearButton.ToolTip = Resources.Language.CONTINUENXTSTP;
            fsucCmdBar1.ClearButton.ValidationGroup = "grpFormSubmit";

            divSSN.Visible = !(CurrentViewContext.IsSSNDisabled);


            SetButtonText();

            //UAT-1578
            //BindSMSNotificationDetails(null, false);
            caOtherDetails.IsUserGroupSlctdValuesdisabled = true;
            caOtherDetails.IsApplicantProfileScreen = true;//UAT-3455
            //UAT-4997 //UAT 4829
            caOtherDetails.NeedTocheckCustomAttributeEditableSetting = true;

            //UAT-3545 CBI || CABS
            ValidatePersonalInformation();
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Event for Forward navigation i.e. Accept/Proceed/Next Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_SubmitClick(object sender, EventArgs e)
        {

            //if (CurrentViewContext.IsLocationServiceTenant && (!ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() || !ucPersonAlias.NewLastNameAlias.IsNullOrEmpty() || !ucPersonAlias.NewMiddleNameAlias.IsNullOrEmpty()) && hdnConfirmSave.Value == "0")
            if ((!ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() || !ucPersonAlias.NewLastNameAlias.IsNullOrEmpty() || !ucPersonAlias.NewMiddleNameAlias.IsNullOrEmpty()) && hdnConfirmSave.Value == "0")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "confirmClick();", true);
                return;
            }
            else
            {
                var _validateMessage = ValidatePreviousAliases();
                ValidateUserGroupSelection();
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                //base.ShowInfoMessage("Secondary Confirm Email Address is required.");
                base.ShowInfoMessage(Resources.Language.SCNDRYCNFRMEMAILADDRSREQ);

                //base.ShowInfoMessage("Alias/Maiden First Name is required if Alias/Maiden Last Name is entered.");
                base.ShowInfoMessage(Resources.Language.ALIASFRSTNMEREQIFLSTNME);
                //base.ShowInfoMessage("Alias/Maiden Last Name is required if Alias/Maiden First Name is entered.");
                base.ShowInfoMessage(Resources.Language.ALIASLSTNMEREQIFFRSTNME);
                //UAT 3573
                String _validationMessage = ValidatePageData();

                if (!String.IsNullOrEmpty(_validateMessage))
                {
                    base.ShowInfoMessage(_validateMessage);
                    return;
                }
                //base.ShowInfoMessage("Duplicate names cannot be added.");
                base.ShowInfoMessage(Resources.Language.DPLNAMECNTADD);

                //UAT 3573
                if (!_validationMessage.IsNullOrEmpty())
                {
                    base.ShowInfoMessage(_validationMessage);
                    return;
                }

                //base.ShowInfoMessage("Please enter at least " + CurrentViewContext.MinPersonalAliasOccurances + " Alias/Maiden Name(s) for this Order.");
                base.ShowInfoMessage(Resources.Language.PLZENTERLEAST + " " + CurrentViewContext.MinPersonalAliasOccurances + " " + Resources.Language.ALSMDNNAMEFORORD);
                if (!txtSecondaryEmail.Text.IsNullOrEmpty() && txtConfirmSecEmail.Text.IsNullOrEmpty())
                {
                    base.ShowInfoMessage("Secondary Confirm Email Address is required.");
                    return;
                }

                //base.ShowInfoMessage("Please enter at least " + CurrentViewContext.MinResidentailHistoryOccurances + " Residence(s) for this Order.");
                base.ShowInfoMessage(Resources.Language.PLZENTERLEAST + " " + CurrentViewContext.MinResidentailHistoryOccurances + " " + Resources.Language.RSDNCFORORD);

                //if (CurrentViewContext.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsLegalNameChange)
                //{
                //    if (ucPersonAlias.PersonAliasList.Count() < AppConsts.ONE)
                //    {
                //        base.ShowInfoMessage(Resources.Language.ONEALIASREQUD);
                //        //base.ShowInfoMessage("Please enter at least 1 Alias / Maiden Name(s) for this Order.");
                //        return;
                //    }
                //}

                if (ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() && !ucPersonAlias.NewLastNameAlias.IsNullOrEmpty())
                {
                    base.ShowInfoMessage("Alias/Maiden First Name is required if Alias/Maiden Last Name is entered.");
                    return;
                }
                if (!ucPersonAlias.NewFirstNameAlias.IsNullOrEmpty() && ucPersonAlias.NewLastNameAlias.IsNullOrEmpty())
                {
                    base.ShowInfoMessage("Alias/Maiden Last Name is required if Alias/Maiden First Name is entered.");
                    return;
                }

                //base.ShowErrorInfoMessage("Please select a valid ZipCode.");
                base.ShowErrorInfoMessage(Resources.Language.PLSSELVALIDZIP);
                if (ucPersonAlias.HasDuplicateNames)
                {

                    //base.ShowErrorInfoMessage("Please enter valid Date of Birth.");
                    base.ShowErrorInfoMessage(Resources.Language.PLZENTRVLDDOB);
                    //base.ShowInfoMessage("Duplicate names cannot be added.");
                    base.ShowInfoMessage(Resources.Language.DPLNAMECNTADD);
                    return;
                }

                //base.ShowErrorInfoMessage("Please select one user group only.");
                base.ShowErrorInfoMessage(Resources.Language.PLSSLCTONLYONEUSERGRP);
                if (CurrentViewContext.MinPersonalAliasOccurances > AppConsts.NONE &&
                    ((ucPersonAlias.PersonAliasList.Count()) < CurrentViewContext.MinPersonalAliasOccurances))
                {
                    base.ShowInfoMessage("Please enter at least " + CurrentViewContext.MinPersonalAliasOccurances + " Alias/Maiden Name(s) for this Order.");
                    return;
                }

                //CurrentViewContext.MinResidentailHistoryOccurances - AppConsts.ONE -> 1 is subtracted to capture the current Residenatial History also.
                if (CurrentViewContext.MinResidentailHistoryOccurances > AppConsts.NONE &&
                    ((PrevResident.ResidentialHitoryTempList.Where(cond => cond.isDeleted == false).Count() + AppConsts.ONE) < CurrentViewContext.MinResidentailHistoryOccurances))
                {
                    base.ShowInfoMessage("Please enter at least " + CurrentViewContext.MinResidentailHistoryOccurances + " Residence(s) for this Order.");
                    return;
                }

                if (!IsValidAddress())
                {
                    //base.ShowErrorInfoMessage("Please select a valid ZipCode.");
                    base.ShowErrorInfoMessage(Resources.Language.PLSSELVALIDZIP);
                    return;
                }

                //UAT-2112 : Screening DOB missing issue
                if (dpkrDOB.SelectedDate.IsNullOrEmpty())
                {
                    //base.ShowErrorInfoMessage("Please enter valid Date of Birth.");
                    base.ShowErrorInfoMessage(Resources.Language.PLZENTRVLDDOB + ".");
                    return;
                }

                //if (!txtFirstName.IsNullOrEmpty() || !txtMiddleName.IsNullOrEmpty() || !txtLastName.IsNullOrEmpty() || !txtSuffix.IsNullOrEmpty())
                if (!txtFirstName.IsNullOrEmpty() || !txtMiddleName.IsNullOrEmpty() || !txtLastName.IsNullOrEmpty() || !cmbSuffix.SelectedItem.Text.IsNullOrEmpty())
                {
                    //if (CurrentViewContext.IsLocationServiceTenant)
                    //{
                    Int32 suffixLength = cmbSuffix.SelectedItem.Index > 0 ? cmbSuffix.SelectedItem.Text.Length : 0;
                    Int32 totalLength = (txtFirstName.Text.Length) + (txtMiddleName.Text.Length) + (txtLastName.Text.Length) + suffixLength;
                    if (totalLength < AppConsts.THREE)
                    {
                        //base.ShowErrorInfoMessage("Total length for Full Name should be atleast 3 characters.");
                        base.ShowErrorInfoMessage(Resources.Language.TTLLENFULLNAMEATLEASTTHREECHARS);
                        return;
                    }
                    //}
                }


                //UAT-3455
                if (caOtherDetails.IsMultipleValsSelected)
                {
                    errMsgUserGroup.Visible = true;
                    base.ShowErrorInfoMessage("Please select one user group only.");
                    return;
                }

                applicantOrderCart.lstApplicantOrder[0].SelectedCommLang = Convert.ToInt32(cmbCommLang.SelectedValue);
                //Entity.ClientEntity.OrganizationUserProfile organizationUserProfile = new Entity.ClientEntity.OrganizationUserProfile();
                Entity.ClientEntity.OrganizationUserProfile organizationUserProfile = OrganizationUser.OrganizationUserProfiles.FirstOrDefault();
                //Entity.ClientEntity.OrganizationUserProfile organizationUserProfile = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile;

                if (!organizationUserProfile.IsNullOrEmpty())
                {
                    organizationUserProfile.FirstName = txtFirstName.Text;
                    organizationUserProfile.LastName = txtLastName.Text;
                    //if (CurrentViewContext.IsSuffixDropDownType)
                    organizationUserProfile.UserTypeID = cmbSuffix.SelectedIndex == 0 ? (Int32?)null : Convert.ToInt32(cmbSuffix.SelectedValue);
                    //else
                    //    organizationUserProfile.UserTypeID = Presenter.GetSuffixIdBasedOnSuffixText(CurrentViewContext.UserSuffix);
                }

                #region UAT-1612 : As an applicant, my middle name should be required.
                //organizationUserProfile.MiddleName = txtMiddleName.Text;
                if (chkMiddleNameRequired.Checked)
                {
                    organizationUserProfile.MiddleName = String.Empty;
                }
                else
                {
                    organizationUserProfile.MiddleName = txtMiddleName.Text;
                }
                #endregion
                organizationUserProfile.Gender = Convert.ToInt32(cmbGender.SelectedValue);
                organizationUserProfile.DOB = dpkrDOB.SelectedDate;
                //organizationUserProfile.PrimaryEmailAddress = txtPrimaryEmail.Text;
                //
                organizationUserProfile.PrimaryEmailAddress = lblPrimaryEmail.Text;
                organizationUserProfile.SecondaryEmailAddress = txtSecondaryEmail.Text;

                //UAT-2447
                organizationUserProfile.IsInternationalPhoneNumber = chkPrimaryPhone.Checked;
                organizationUserProfile.IsInternationalSecondaryPhone = chkSecondaryPhone.Checked;
                if (chkSecondaryPhone.Checked)
                {
                    organizationUserProfile.SecondaryPhone = txtUnmaskedSecondaryPhone.Text;
                }
                else
                {
                    organizationUserProfile.SecondaryPhone = txtSecondaryPhone.Text;
                }

                organizationUserProfile.SSN = txtSSN.Text;
                //organizationUserProfile.SSNL4 = txtLastSSNAny.Text;
                applicantOrderCart.lstApplicantOrder[0].IsHavingSSN = CurrentViewContext.IsHavingSSN;

                //UAT-2447
                if (chkPrimaryPhone.Checked)
                {
                    organizationUserProfile.PhoneNumber = txtUnmaskedPrimaryPhone.Text;
                }
                else
                {
                    organizationUserProfile.PhoneNumber = txtPrimaryPhone.Text;
                }

                organizationUserProfile.AddressHandleID = AddressHandleId;
                organizationUserProfile.IsActive = true;

                organizationUserProfile.AddressHandle = new AddressHandle
                {
                    AddressHandleID = AddressHandleId == null ? Guid.NewGuid() : AddressHandleId.Value
                };


                organizationUserProfile.AddressHandle.Addresses = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Address>();
                organizationUserProfile.AddressHandle.Addresses.Add(new Address
                {
                    AddressHandleID = AddressHandleId.Value,
                    Address1 = txtAddress1.Text,
                    Address2 = txtAddress2.Text,
                    ZipCodeID = locationTenant.MasterZipcodeID,
                    AddressID = CurrentViewContext.CurrentAddressId
                });

                if (applicantOrderCart != null)
                {
                    String clientMachineIP = Request.UserHostAddress;
                    applicantOrderCart.AddOrganizationUserProfile(organizationUserProfile, true, clientMachineIP);
                    applicantOrderCart.AddCustomAttributeValues((caOtherDetails).GetCustomAttributeValues());
                    //UAT 1438: Enhancement to allow students to select a User Group. 
                    applicantOrderCart.IsUserGroupCustomAttributeExist = IsUserGroupCustomAttributeExist;
                    applicantOrderCart.lstCustomAttributeUserGroupIDs = (caOtherDetails).GetUserGroupCustomAttributeValues();
                    //applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport = chkSendBkgReport.Checked;
                    applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport = false;
                    applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState = false;

                    #region UAT-1578 : Addition of SMS notification
                    //applicantOrderCart.lstApplicantOrder[0].IsReceiveTextNotification = CurrentViewContext.IsReceiveTextNotification;
                    applicantOrderCart.lstApplicantOrder[0].PhoneNumber = CurrentViewContext.PhoneNumber;
                    #endregion

                    #region Mvr Field Set in The Session

                    if (chkIsMVRInfo.Checked && !applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0
                        && !CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Count > 0 && CurrentViewContext.lstMvrAttGrp.Select(x => x.AttributeGrpId).FirstOrDefault() > 0
                        )
                    {
                        Int32 mvrBkgSvcAttributeGroupId = Convert.ToInt32(CurrentViewContext.lstMvrAttGrp.Select(x => x.AttributeGrpId).FirstOrDefault());
                        applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState = true;
                        if (applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty()
                            || !applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.Any(col => col.BkgSvcAttributeGroupId == mvrBkgSvcAttributeGroupId))
                        {
                            if (applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty())
                            {
                                applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData = new List<BackgroundOrderData>();
                            }

                            BackgroundOrderData backgroundOrderDataMVR = new BackgroundOrderData();
                            backgroundOrderDataMVR.InstanceId = AppConsts.ONE;
                            backgroundOrderDataMVR.CustomFormId = (_presenter.GetCustomFormIDBYCode() > 0) ? _presenter.GetCustomFormIDBYCode() : AppConsts.NONE;
                            backgroundOrderDataMVR.BkgSvcAttributeGroupId = Convert.ToInt32(CurrentViewContext.lstMvrAttGrp.Select(x => x.AttributeGrpId).FirstOrDefault());
                            backgroundOrderDataMVR.CustomFormData = new Dictionary<Int32, String>();
                            Int32 mappingID = 0;
                            if (dvDriverLicenseNo.Visible && !txtLicenseNO.Text.IsNullOrEmpty())
                            {
                                mappingID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID;
                                backgroundOrderDataMVR.CustomFormData.Add(mappingID, txtLicenseNO.Text);
                                applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID = mappingID;
                            }
                            if (dvDriverLicenseState.Visible && !cmbState.Text.IsNullOrEmpty())
                            {
                                mappingID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID;
                                backgroundOrderDataMVR.CustomFormData.Add(mappingID, cmbState.Text);
                                applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID;
                            }

                            applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.Insert(AppConsts.NONE, backgroundOrderDataMVR);
                        }
                        else
                        {
                            Int32 mappingID = 0;
                            if (applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.Keys.Contains(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID))
                            {
                                mappingID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID;
                                applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData[mappingID] = txtLicenseNO.Text;
                            }
                            if (applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.Keys.Contains(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID))
                            {
                                mappingID = CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID;
                                applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData[mappingID] = cmbState.Text;
                            }
                        }
                    }

                    #endregion

                    PreviousAddressContract currentAddress = new PreviousAddressContract();
                    currentAddress.Address1 = txtAddress1.Text;
                    currentAddress.Address2 = txtAddress2.Text;
                    currentAddress.ZipCodeID = locationTenant.MasterZipcodeID.Value;
                    currentAddress.Zipcode = locationTenant.RSLZipCode;
                    currentAddress.CityName = locationTenant.RSLCityName;
                    currentAddress.StateName = locationTenant.RSLStateName;
                    currentAddress.Country = locationTenant.RSLCountryName;
                    currentAddress.CountryId = locationTenant.RSLCountryId;
                    currentAddress.MotherName = txtMotherName.Text.Trim();
                    currentAddress.IdentificationNumber = txtIdentificationNumber.Text.Trim();
                    currentAddress.LicenseNumber = txtCriminalLicenseNumber.Text.Trim();
                    if (locationTenant.MasterZipcodeID.Value > 0)
                    {
                        currentAddress.CountyName = locationTenant.RSLCountyName;
                    }
                    currentAddress.ResidenceStartDate = dpCurResidentFrom.SelectedDate;
                    currentAddress.isCurrent = true;
                    currentAddress.ResHistorySeqOrdID = AppConsts.ONE;

                    PrevResident.ResidentialHitoryTempList.Add(currentAddress);

                    foreach (var prevAddress in PrevResident.ResidentialHistoryList.Where(fx => fx.isDeleted == true && fx.isCurrent == false).ToList())
                    {
                        if (!PrevResident.ResidentialHitoryTempList.Any(x => x.ID.Equals(prevAddress.ID)))
                            PrevResident.ResidentialHitoryTempList.Add(prevAddress);
                    }

                    applicantOrderCart.lstPrevAddresses = PrevResident.ResidentialHitoryTempList;

                    applicantOrderCart.lstPersonAlias = new List<PersonAliasContract>();

                    Int32 _sequenceNo = AppConsts.NONE;

                    foreach (var _alias in ucPersonAlias.PersonAliasTempListMaxOcc)
                    {
                        _sequenceNo += 1;
                        _alias.AliasSequenceId = _sequenceNo;
                        applicantOrderCart.lstPersonAlias.Add(_alias);
                    }

                    applicantOrderCart.IncrementOrderStepCount();
                    //applicantOrderCart.IsAccountUpdated = chkUpdatePersonalDetails.Checked;
                    applicantOrderCart.AddOrderStageTrackID(OrderStages.ApplicantProfileCompleted);

                    #region UAT-2625:Add ability to choose 18 and above, 17 or under, and all ages as options on D&A association
                    SetApplicantAgeGroup(applicantOrderCart);
                    #endregion
                    applicantOrderCart.lstApplicantOrder[0].UpdatePersonalDetails = true;
                    applicantOrderCart.TenantId = CurrentViewContext.TenantId;
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    if (applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
                    {
                        queryString = new Dictionary<String, String>
                                                                     {
                                                                        {AppConsts.CHILD,  ChildControls.AdminEntryApplicantDisclaimerPage} //UAT-5184
                                                                     };
                        Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                    }
                    else
                    {

                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD, ChildControls.AdminEntryCustomFormLoad},
                                                                 };

                        if (customFormdataId > 0 && !lstBackgroundOrderData.IsNullOrEmpty())
                        {
                            applicantOrderCart.IsEditMode = true;
                            queryString.Add("IsEdit", "true");
                            queryString.Add("CustomFormId", Convert.ToString(customFormdataId));
                        }
                        Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                    }
                }
            }
        }

        private void ValidateUserGroupSelection()
        {
            CurrentViewContext.IsMultiSelectionAllowed = Presenter.IsMultiUserGroupSelectionAllowed(); // UAT-4731 : Restrict Applicant To One User Group In Order Process
            //UAT-3455
            caOtherDetails.ClearMultiSelectedValues(CurrentViewContext.IsMultiSelectionAllowed);
        }



        protected void cmdbarSubmit_CancelClick(object sender, EventArgs e)
        {
            try
            {
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
                //UAT-1560
                Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
                Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "OrderId", applicantOrderCart.OrderId.ToString()},
                                                                    { "TenantId",applicantOrderCart.TenantId.ToString() }
                                                                 };
                string url = String.Format("~/AdminEntryPortal/Default.aspx?ucid={0}&args={1}", null, queryString.ToEncryptedQueryString());
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

        #endregion

        #region DropDown Events



        #endregion

        #region RadioButton Event

        protected void rblSSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.IsHavingSSN)
                {
                    txtSSN.Text = String.Empty;
                    txtSSN.Enabled = true;
                    rfvSSN.Enabled = true;
                    revtxtSSN.Enabled = true;
                    //if (CurrentViewContext.IsLocationServiceTenant)
                    //{
                    //    rgvSSNCBI.Enabled = true;
                    //    txtSSN.Visible = true;
                    //    lblSSN.Visible = true;
                    //    revtxtSSN.Enabled = false;
                    //}
                }
                else
                {
                    txtSSN.Text = AppConsts.DefaultSSN;
                    rfvSSN.Enabled = false;
                    txtSSN.Enabled = false;
                    //if (CurrentViewContext.IsLocationServiceTenant)
                    //{
                    //    rgvSSNCBI.Enabled = false;
                    //    txtSSN.Visible = false;
                    //    lblSSN.Visible = false;
                    //    revtxtSSN.Enabled = false;
                    //}
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

        #region Methods

        #region Public Methods


        #endregion

        #region Private Methods

        private void GenerateVerCodeSendEmail(Boolean isDifferentEmail = false)
        {
            Random random = new Random();
            String value = random.Next(100000, 999999).ToString();
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_EMAIL_VERIFICATION, value);
            if (Presenter.SendVerificationCodeForEmailChange(value, CurrentViewContext.OrganizationUser, lblPrimaryEmail.Text.Trim().ToLower()))
            {
                VerifiedEmailAddress = lblPrimaryEmail.Text.Trim().ToLower();
                if (!isDifferentEmail)
                {
                    //base.ShowInfoMessage("An email has been sent with a verification code to your email, " + lblPrimaryEmail.Text.Trim().ToLower() + ". Please enter the verification code.");
                    base.ShowInfoMessage(Resources.Language.EMAILSNTWITHVRFCTNCODE + ", " + lblPrimaryEmail.Text.Trim().ToLower() + ". " + Resources.Language.PLZENTRVRFCTNCOE);
                }
                else
                {
                    //base.ShowInfoMessage("Primary email entered is different from verified email." + "<br />" + "An email has been sent with a verification code to your email, " + lblPrimaryEmail.Text.Trim().ToLower() + ". Please enter the verification code.");
                    base.ShowInfoMessage(Resources.Language.PRMRYMAILENTRISDFRNT + "<br />" + Resources.Language.EMAILSNTWITHVRFCTNCODE + ", " + lblPrimaryEmail.Text.Trim().ToLower() + ". " + Resources.Language.PLZENTRVRFCTNCOE);
                }
            }
            else
            {
                //base.ShowInfoMessage("Some error has occured.Please contact administrator.");
                base.ShowInfoMessage(Resources.Language.ERROCCNTCTADMNSTR);
            }
        }

        private void BindGender()
        {
            cmbGender.DataSource = Presenter.GetGender().Where(cond => cond.LanguageID == LanguageTranslateUtils.GetCurrentLanguageFromSession().LanguageID);
            cmbGender.DataBind();
            cmbGender.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(Resources.Language.SELECTWITHHYPENS, "0"));
        }

        private void BindOrganizationUserData()
        {
            #region UAT-781 ENCRYPTED SSN
            //Commented for Admin Entry Portal
            //Presenter.GetDecryptedSSN(CurrentViewContext.OrganizationUser.OrganizationUserID, false);
            Presenter.GetDecryptedSSN(CurrentViewContext.OrganizationUser.OrganizationUserProfiles.FirstOrDefault().OrganizationUserProfileID, true);
            OrganizationUserProfile OrganizationUserProfileData = CurrentViewContext.OrganizationUser.OrganizationUserProfiles.FirstOrDefault(x => !x.IsDeleted);
            #endregion
            //UAT-1578 : Addition of SMS notification
            //BindSMSNotificationDetails();
            txtFirstName.Text = UserFirstName = OrganizationUserProfileData.FirstName;
            #region UAT-1612 : As an applicant, my middle name should be required.
            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
            txtMiddleName.Text = UserMiddleName = OrganizationUserProfileData.MiddleName;
            #endregion
            txtLastName.Text = UserLastName = OrganizationUserProfileData.LastName;

            //if (CurrentViewContext.IsSuffixDropDownType)
            //{
            BindSuffixDropdown();
            //if (!UserSuffix.IsNullOrEmpty())
            //{
            //    var item = cmbSuffix.FindItemByText(UserSuffix, true);
            //    if (!item.IsNullOrEmpty())
            //    {
            //        cmbSuffix.Items.FindItemByText(UserSuffix, true).Selected = true;
            //    }
            //}
            //}
            //else
            //{
            //    txtSuffix.Text = UserSuffix = CurrentViewContext.lstSuffixes.Where(a => a.Suffix == UserSuffix && !a.IsSystem).Any() ? UserSuffix : String.Empty;
            //}

            txtSSN.Text = CurrentViewContext.DecryptedSSN; //UAT-781
            //txtLastSSNAny.Text = txtSSN.Text;
            rblSSN.SelectedValue = CurrentViewContext.IsHavingSSN.ToString();
            dpkrDOB.SelectedDate = OrganizationUserProfileData.DOB;
            cmbGender.SelectedValue = OrganizationUserProfileData.Gender.IsNullOrEmpty() ? "0" : OrganizationUserProfileData.Gender.Value.ToString();
            txtPrimaryPhone.Text = OrganizationUserProfileData.PhoneNumber;
            //UAT-3824
            cmbCommLang.SelectedValue = CurrentViewContext.SelectedCommLang.HasValue ? Convert.ToString(CurrentViewContext.SelectedCommLang.Value) : "0";


            //UAT-2447
            ShowHidePhoneControls(OrganizationUserProfileData.IsInternationalPhoneNumber, 1);
            ShowHidePhoneControls(OrganizationUserProfileData.IsInternationalSecondaryPhone, 2);
            chkSecondaryPhone.Checked = OrganizationUserProfileData.IsInternationalSecondaryPhone;
            chkPrimaryPhone.Checked = OrganizationUserProfileData.IsInternationalPhoneNumber;

            if (OrganizationUserProfileData.IsInternationalPhoneNumber)
            {
                txtUnmaskedPrimaryPhone.Text = OrganizationUserProfileData.PhoneNumber;
            }
            else
            {
                txtPrimaryPhone.Text = OrganizationUserProfileData.PhoneNumber;
            }
            if (OrganizationUserProfileData.IsInternationalSecondaryPhone)
            {
                txtUnmaskedSecondaryPhone.Text = OrganizationUserProfileData.SecondaryPhone;
            }
            else
            {
                txtSecondaryPhone.Text = OrganizationUserProfileData.SecondaryPhone;
            }

            lblPrimaryEmail.Text = OrganizationUserProfileData.PrimaryEmailAddress.Trim();
            CurrentEmailAddress = OrganizationUserProfileData.PrimaryEmailAddress.Trim();
            txtSecondaryEmail.Text = OrganizationUserProfileData.SecondaryEmailAddress;
            txtConfirmSecEmail.Text = OrganizationUserProfileData.SecondaryEmailAddress;

            if (OrganizationUserProfileData.AddressHandle != null
                && OrganizationUserProfileData.AddressHandle.Addresses != null
                && OrganizationUserProfileData.AddressHandle.Addresses.Count > 0)
            {
                Entity.ClientEntity.Address address = OrganizationUserProfileData.AddressHandle.Addresses.ToList()[0];
                txtAddress1.Text = address.Address1;
                txtAddress2.Text = address.Address2;
                CurrentViewContext.CurrentAddressId = address.AddressID;
                //locationTenant.MasterZipcodeID = address.ZipCodeID;
                if (address.ZipCodeID == 0 && address.AddressExts.IsNotNull() && address.AddressExts.Count > 0)
                {
                    Entity.ClientEntity.AddressExt addressExt = address.AddressExts.FirstOrDefault();
                    //locationTenant.RSLCountryId = addressExt.AE_CountryID;
                    //UAT-3910
                    //if (CurrentViewContext.IsLocationServiceTenant)
                    //{
                    //    locationTenant.RSLCountryId = addressExt.AE_County == null ? AppConsts.NONE : Convert.ToInt32(addressExt.AE_County);
                    //}
                    //else
                    //{
                    locationTenant.RSLCountryId = addressExt.AE_CountryID;
                    //}
                    locationTenant.RSLStateName = addressExt.AE_StateName;
                    locationTenant.RSLCityName = addressExt.AE_CityName;
                    locationTenant.RSLZipCode = addressExt.AE_ZipCode;

                }
            }
            //Show Residing From/To
            //var resHisoryAddress = CurrentViewContext.OrganizationUser.ResidentialHistories.FirstOrDefault(obj => obj.RHI_IsDeleted == false && obj.RHI_IsCurrentAddress == true);

            var resHistoryAddress = CurrentViewContext.OrganizationUser.OrganizationUserProfiles.FirstOrDefault().ResidentialHistoryProfiles.FirstOrDefault(x => x.RHIP_IsDeleted == false
            && x.RHIP_IsCurrentAddress == true);

            if (resHistoryAddress.IsNotNull())
            {
                dpCurResidentFrom.SelectedDate = resHistoryAddress.RHIP_ResidenceStartDate;
                txtMotherName.Text = resHistoryAddress.RHIP_MotherMaidenName;
                txtCriminalLicenseNumber.Text = resHistoryAddress.RHIP_DriverLicenseNumber;
                txtIdentificationNumber.Text = resHistoryAddress.RHIP_IdentificationNumber;

            }
            //Commented for Admin Entry Portal
            //PrevResident.ResidentialHistoryList = Presenter.GetResidentialHistories(CurrentViewContext.OrganizationUser.OrganizationUserID);
            PrevResident.ResidentialHistoryList = Presenter.GetResidentialHistories(CurrentViewContext.OrganizationUser.OrganizationUserProfiles.FirstOrDefault().OrganizationUserProfileID);
            PrevResident.MaxOccurance = CurrentViewContext.MaxResidentailHistoryOccurances;

            if (CurrentViewContext.OrganizationUser.PersonAlias.Where(con => !con.PA_IsDeleted).IsNotNull())
            {
                ucPersonAlias.PersonAliasList = CurrentViewContext.OrganizationUser.PersonAlias.Where(x => x.PA_IsDeleted == false).Select(cond => new PersonAliasContract
                {
                    FirstName = cond.PA_FirstName,
                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                    MiddleName = cond.PA_MiddleName,
                    LastName = cond.PA_LastName,
                    ID = cond.PA_ID,
                    //CBI || CABS
                    //Suffix = !CurrentViewContext.IsLocationServiceTenant.IsNullOrEmpty() && CurrentViewContext.IsLocationServiceTenant ? (!cond.PersonAliasExtensions.IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix.IsNullOrEmpty() ? cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix : String.Empty) : String.Empty,
                    Suffix = String.Empty,

                }).OrderBy(cond => cond.AliasSequenceId).ToList();


                //UAT-1305 Only one Alias name input should be coming for packages when no Alias service item is defined or no Quantity group for Alias Item is setup.
                //Set the MaxOccurance to 1 as the MaxPersonalAliasOccurances is null for all the services other than PersonAlias.
                ucPersonAlias.MaxOccurance = CurrentViewContext.MaxPersonalAliasOccurances.IsNullOrEmpty() ? AppConsts.ONE : CurrentViewContext.MaxPersonalAliasOccurances;
            }
        }
        private void BindOrganizationUserProfileData(ApplicantOrderCart applicantOrderCart)
        {
            OrganizationUserProfile organizationUserProfile = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile;
            //UAT-1578 : Addition of SMS notification
            //BindSMSNotificationDetails(applicantOrderCart);
            txtFirstName.Text = UserFirstName = organizationUserProfile.FirstName;

            #region UAT-1612 : As an applicant, my middle name should be required.
            txtMiddleName.Text = UserMiddleName = organizationUserProfile.MiddleName;
            #endregion

            txtLastName.Text = UserLastName = organizationUserProfile.LastName;
            //if (CurrentViewContext.IsSuffixDropDownType)
            //{
            BindSuffixDropdown();

            if (!organizationUserProfile.UserTypeID.IsNullOrEmpty() && organizationUserProfile.UserTypeID > AppConsts.NONE)
            {
                cmbSuffix.SelectedItem.Value = Convert.ToString(organizationUserProfile.UserTypeID);
                cmbSuffix.Items.Where(x => x.Value.Equals(cmbSuffix.SelectedItem.Value.ToString()))
                            .ForEach(x => x.Selected = true);
                //cmbSuffix.SelectedItem.Text = suffix
            }

            //if (!UserSuffix.IsNullOrEmpty())
            //{
            //    var item = cmbSuffix.FindItemByText(UserSuffix, true);
            //    if (!item.IsNullOrEmpty())
            //    {
            //        cmbSuffix.Items.FindItemByText(UserSuffix, true).Selected = true;
            //    }
            //}
            //}
            txtSSN.Text = organizationUserProfile.SSN;
            CurrentViewContext.IsHavingSSN = applicantOrderCart.lstApplicantOrder[0].IsHavingSSN;
            dpkrDOB.SelectedDate = organizationUserProfile.DOB;
            cmbGender.SelectedValue = organizationUserProfile.Gender.Value.ToString();
            cmbCommLang.SelectedValue = applicantOrderCart.lstApplicantOrder[0].SelectedCommLang.HasValue ? Convert.ToString(applicantOrderCart.lstApplicantOrder[0].SelectedCommLang) : "0";
            //UAT-2447
            chkPrimaryPhone.Checked = organizationUserProfile.IsInternationalPhoneNumber;
            chkSecondaryPhone.Checked = organizationUserProfile.IsInternationalSecondaryPhone;
            if (organizationUserProfile.IsInternationalPhoneNumber)
            {
                txtUnmaskedPrimaryPhone.Text = organizationUserProfile.PhoneNumber;
            }
            else
            {
                txtPrimaryPhone.Text = organizationUserProfile.PhoneNumber;
            }
            if (organizationUserProfile.IsInternationalSecondaryPhone)
            {
                txtUnmaskedSecondaryPhone.Text = organizationUserProfile.SecondaryPhone;
            }
            else
            {
                txtSecondaryPhone.Text = organizationUserProfile.SecondaryPhone;
            }

            ShowHidePhoneControls(organizationUserProfile.IsInternationalPhoneNumber, 1);
            ShowHidePhoneControls(organizationUserProfile.IsInternationalSecondaryPhone, 2);

            lblPrimaryEmail.Text = organizationUserProfile.PrimaryEmailAddress;
            txtSecondaryEmail.Text = organizationUserProfile.SecondaryEmailAddress;
            txtConfirmSecEmail.Text = organizationUserProfile.SecondaryEmailAddress;
            //chkSendBkgReport.Checked = applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport;
            chkSendBkgReport.Checked = false;
            chkIsMVRInfo.Checked = applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState;
            PrevResident.MaxOccurance = CurrentViewContext.MaxResidentailHistoryOccurances;

            //UAT-1305 Only one Alias name input should be coming for packages when no Alias service item is defined or no Quantity group for Alias Item is setup.
            //Set the MaxOccurance to 1 as the MaxPersonalAliasOccurances is null for all the services other than PersonAlias.
            ucPersonAlias.MaxOccurance = CurrentViewContext.MaxPersonalAliasOccurances.IsNullOrEmpty() ? AppConsts.ONE : CurrentViewContext.MaxPersonalAliasOccurances;
            #region Extract values of lIcense No.,license state For Mvr Grp
            if (!CurrentViewContext.lstMvrAttGrp.IsNullOrEmpty() && CurrentViewContext.lstMvrAttGrp.Count > 0 && CurrentViewContext.lstMvrAttGrp.Select(x => x.AttributeGrpId).FirstOrDefault() > 0
                && !applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0
                && !applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.Count > 0)
            {
                if (applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.Keys.Contains(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID))
                {
                    cmbState.Text = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.GetValue(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID);
                }
                if (applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.Keys.Contains(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID))
                {
                    txtLicenseNO.Text = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.GetValue(CurrentViewContext.lstMvrAttGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID);
                }
            }

            if (!applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState)
            {
                cmbState.ClearSelection();
                cmbState.Enabled = false;
                txtLicenseNO.Text = "N/A";
                txtLicenseNO.Enabled = false;
                rfvLicenseState.Enabled = false;
                rfvtxtLicenseNO.Enabled = false;
            }

            #endregion

            if (applicantOrderCart.lstPrevAddresses.IsNotNull() && applicantOrderCart.lstPrevAddresses.Count > 0)
            {
                PreviousAddressContract currentAddress = applicantOrderCart.lstPrevAddresses.Where(obj => obj.isCurrent == true).FirstOrDefault();
                if (currentAddress.IsNotNull())
                {
                    txtAddress1.Text = currentAddress.Address1;
                    txtAddress2.Text = currentAddress.Address2;
                    CurrentViewContext.CurrentAddressId = currentAddress.ID;
                    locationTenant.MasterZipcodeID = currentAddress.ZipCodeID;
                    if (currentAddress.ZipCodeID == 0)
                    {
                        locationTenant.RSLCountryId = currentAddress.CountryId;
                        locationTenant.RSLStateName = currentAddress.StateName;
                        locationTenant.RSLCityName = currentAddress.CityName;
                        locationTenant.RSLZipCode = currentAddress.Zipcode;
                    }
                    dpCurResidentFrom.SelectedDate = currentAddress.ResidenceStartDate;
                    txtCriminalLicenseNumber.Text = currentAddress.LicenseNumber;
                    txtMotherName.Text = currentAddress.MotherName;
                    txtIdentificationNumber.Text = currentAddress.IdentificationNumber;
                }
                PrevResident.ResidentialHistoryList = applicantOrderCart.lstPrevAddresses.Where(obj => obj.isCurrent != true).ToList();
            }
            if (applicantOrderCart.lstPersonAlias.IsNotNull())
            {
                ucPersonAlias.PersonAliasList = applicantOrderCart.lstPersonAlias;
            }
            //chkUpdatePersonalDetails.Checked = applicantOrderCart.lstApplicantOrder[0].UpdatePersonalDetails;
        }

        private void BindData(ApplicantOrderCart applicantOrderCart)
        {
            //ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            RedirectIfIncorrectOrderStage(applicantOrderCart);
            BindGender();
            //UAT-3824
            Presenter.GetCommLang(CurrentViewContext.OrganizationUser.UserID);
            if (applicantOrderCart != null
                && applicantOrderCart.lstApplicantOrder != null
                && applicantOrderCart.lstApplicantOrder.Count > 0
                && applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile != null
                )
            {
                BindOrganizationUserProfileData(applicantOrderCart);
            }
            else
            {
                BindOrganizationUserData();
            }
            ManageSSN();
            this.NodeId = applicantOrderCart.NodeId;
            this.DeptProgId = Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID);
            if (this.lst.IsNull())
                this.lst = new List<TypeCustomAttributes>();

            this.lst = applicantOrderCart.GetCustomAttributeValues();

            if (!applicantOrderCart.DPP_Id.IsNullOrEmpty())
                this.DPP_ID = applicantOrderCart.DPP_Id;
            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0)
                this.BPHM_ID = applicantOrderCart.lstApplicantOrder[0].lstPackages[0].BPHMId;
        }

        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart applicantOrderCart)
        {
            Presenter.GetNextPagePathByOrderStageID(applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (CurrentViewContext.NextPagePath.IsNotNull())
            {
                Response.Redirect(CurrentViewContext.NextPagePath);
            }
            else
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.ApplicantProfile);
            }
        }

        void GenerateCustomAttributes()
        {
            // Generate the control using database, but set the values from the session
            caOtherDetails.TenantId = CurrentViewContext.TenantId;
            caOtherDetails.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
            caOtherDetails.MappingRecordId = this.NodeId;
            caOtherDetails.ValueRecordId = this.DeptProgId;
            caOtherDetails.DataSourceModeType = DataSourceMode.Ids;
            //caOtherDetails.Title = "Other Details";
            caOtherDetails.Title = Resources.Language.OTHRDTLS;
            caOtherDetails.ControlDisplayMode = DisplayMode.Controls;
            caOtherDetails.CurrentLoggedInUserId = base.CurrentUserId;
            caOtherDetails.ValidationGroup = "grpFormSubmit";
            caOtherDetails.IsReadOnly = false;
            if (this.lst.IsNotNull() && this.lst.Count() > 0)
                caOtherDetails.AttributeValues = lst;

            //To get the custom attributes by Department Program Package ID and Bkg Package Hierarchy Mapping ID
            caOtherDetails.IsOrder = true;
            caOtherDetails.DPP_ID = this.DPP_ID;
            caOtherDetails.BPHM_ID = this.BPHM_ID;

            //UAT 1438: Enhancement to allow students to select a User Group
            caOtherDetails.ShowUserGroupCustomAttribute = true;
            if (Presenter.IsUserGroupCustomAttributeExist(CustomAttributeUseTypeContext.Hierarchy.GetStringValue(), this.DeptProgId))
            {
                IsUserGroupCustomAttributeExist = true;
                Presenter.GetAllUserGroup();
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                //Means fresh order start. Get the list of usergroups.
                if (applicantOrderCart.IsNotNull())
                {
                    if (applicantOrderCart.lstCustomAttributeUserGroupIDs.IsNullOrEmpty())
                    {
                        //Get applicants Usergroups from database.
                        Presenter.GetUserGroupsForUser();
                        lstUsrGrpSavedValues = lstUserGroupsForUser;
                        applicantOrderCart.lstUsrGrpSavedValues = lstUserGroupsForUser;
                    }
                    else
                    {
                        //Get applicants Usergroups from session.
                        lstUserGroupsForUser = lstUserGroups.Where(x => (applicantOrderCart.lstCustomAttributeUserGroupIDs.Contains(x.UG_ID))).ToList();
                    }
                }
                //Commented to get userGroups by Node id (UAT-3942)
                //caOtherDetails.lstUserGroups = lstUserGroups; 
                if (!lstUserGroups.IsNullOrEmpty())  //UAT-3942
                    caOtherDetails.lstUserGroups = lstUserGroups.Where(x => x.UserGroupHierarchyMappings.Any(a => a.UGHM_HierarchyNodeID == this.DeptProgId));   ////UAT-3942

                caOtherDetails.lstUserGroupsForUser = lstUserGroupsForUser;
                //UAT-3455
                CurrentViewContext.lstPreviousSelectedUserGroupIds = applicantOrderCart.lstPreviousSelectedUserGroupIds; //UAT-3455
                caOtherDetails.lstPreviousSelectedUserGroupIds = applicantOrderCart.lstPreviousSelectedUserGroupIds; //UAT-3455

                if (!applicantOrderCart.lstUsrGrpSavedValues.IsNullOrEmpty())
                {
                    caOtherDetails.lstPreviousSelectedUserGroupIds = applicantOrderCart.lstUsrGrpSavedValues.Where(cond => !cond.UG_IsDeleted).Select(cond => cond.UG_ID).ToList();
                    applicantOrderCart.lstPreviousSelectedUserGroupIds = caOtherDetails.lstPreviousSelectedUserGroupIds;
                }
                else
                {
                    caOtherDetails.lstPreviousSelectedUserGroupIds = null;
                    applicantOrderCart.lstPreviousSelectedUserGroupIds = null;
                }
            }
        }

        /// <summary>
        /// Method to avoid registration with 0 ZipCodeId for USA address - UAT 934
        /// </summary>
        private Boolean IsValidAddress()
        {
            if (locationTenant.MasterZipcodeID == AppConsts.NONE && locationTenant.RSLCountryId == AppConsts.COUNTRY_USA_ID)
                return false;
            return true;
        }

        #endregion

        #endregion

        private string GetPackageIdString(ApplicantOrderCart applicantOrderCart)
        {
            String packages = String.Empty;
            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                applicantOrderCart.lstApplicantOrder[0].lstPackages.ForEach(x => packages += Convert.ToString(x.BPAId) + ",");
                //packages = "4";
                if (packages.EndsWith(","))
                    packages = packages.Substring(0, packages.Length - 1);
            }
            return packages;
        }

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
        }


        /// <summary>
        /// Redirect the user to dashboard, if applicant order cart is empty
        /// </summary>
        /// <param name="applicantOrder"></param>
        private void RedirectInvalidOrder(ApplicantOrderCart applicantOrderCart)
        {
            if (applicantOrderCart.IsNullOrEmpty())
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "OrderId", "0"},
                                                                    { "TenantId","0" }
                                                                 };
                string url = String.Format("~/AdminEntryPortal/Default.aspx?ucid={0}&args={1}", null, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }

        }

        /// <summary>
        /// Set the button Text for 'Previous', 'Next' or 'Restart' etc, based on the type of Order
        /// </summary>
        private void SetButtonText()
        {
            fsucCmdBar1.ClearButtonText = Resources.Language.NEXT;
        }


        /// <summary>
        /// Prepare the list of Aliases which were empty, before the Validations were applied
        /// </summary>
        /// <returns></returns>
        private String ValidatePreviousAliases()
        {
            Boolean _isAliasValidated = true;
            var _msg = String.Empty;
            StringBuilder _sbAliases = new StringBuilder();
            _sbAliases.Append(Resources.Language.PLZENTRALSLASTNAME);

            if (ucPersonAlias.PersonAliasTempListMaxOcc.IsNullOrEmpty())
                return _msg;

            foreach (var _alias in ucPersonAlias.PersonAliasTempListMaxOcc)
            {
                if (_alias.LastName.IsNullOrEmpty())
                {
                    _isAliasValidated = false;
                    _sbAliases.Append("'" + _alias.FirstName + "', ");
                }
            }

            if (!_isAliasValidated)
            {
                _msg = Convert.ToString(_sbAliases);
                _msg = _msg.Substring(0, _msg.LastIndexOf(','));
            }
            return _msg;
        }

        //#region UAT-1578 : Addition of SMS notification
        ///// <summary>
        ///// method used to bind the text notification details of the user
        ///// </summary>
        ///// <param name="applicantOrderCart">applicantOrderCart use to bind data from session</param>
        //private void BindSMSNotificationDetails(ApplicantOrderCart applicantOrderCart = null, Boolean isFreshLoading = true)
        //{
        //    if (isFreshLoading)
        //    {
        //        String phoneNumber = String.Empty;
        //        if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
        //        {
        //            //CurrentViewContext.IsReceiveTextNotification = applicantOrderCart.lstApplicantOrder[0].IsReceiveTextNotification;
        //            phoneNumber = applicantOrderCart.lstApplicantOrder[0].PhoneNumber;
        //        }
        //        else
        //        {
        //            Presenter.GetUserSMSNotificationData(CurrentViewContext.OrganizationUser.OrganizationUserID);

        //            if (!CurrentViewContext.OrganisationUserTextMessageSettingData.IsNullOrEmpty()
        //                && CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_ID > AppConsts.NONE
        //                )
        //            {
        //                //if (CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_ReceiveTextNotification)
        //                //    CurrentViewContext.IsReceiveTextNotification = true;
        //                //else
        //                //    CurrentViewContext.IsReceiveTextNotification = false;

        //                phoneNumber = CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_MobileNumber;
        //            }
        //        }

        //        //CurrentViewContext.PhoneNumber = phoneNumber;

        //        #region UAT-3601 : cell number for text notification should be populated with Profile Phone Number by default but user can change it.
        //        if (phoneNumber.IsNullOrEmpty())
        //        {
        //            CurrentViewContext.PhoneNumber = CurrentViewContext.OrganizationUser.PhoneNumber;
        //        }
        //        else
        //        {
        //            CurrentViewContext.PhoneNumber = phoneNumber;
        //        }
        //        #endregion

        //        //if (CurrentViewContext.IsReceiveTextNotification)
        //        //{
        //        //    CurrentViewContext.IsReceiveTextNotification = true;
        //        //    //divHideShowPhoneNumber.Style["display"] = "block";
        //        //    rfvPhoneNumber.Enabled = true;
        //        //    //spnPhoneNumberReq.Style["display"] = "";
        //        //}
        //        //else
        //        //{
        //        //    CurrentViewContext.IsReceiveTextNotification = false;
        //        //    //divHideShowPhoneNumber.Style["display"] = "none";
        //        //    //CurrentViewContext.PhoneNumber = "";
        //        //    rfvPhoneNumber.Enabled = false;
        //        //    //spnPhoneNumberReq.Style["display"] = "none";
        //        //}
        //    }
        //    //else
        //    //{
        //    //    if (!CurrentViewContext.IsReceiveTextNotification)
        //    //    {
        //    //        //divHideShowPhoneNumber.Style["display"] = "none";
        //    //        rfvPhoneNumber.Enabled = false;
        //    //        //spnPhoneNumberReq.Style["display"] = "none";
        //    //    }
        //    //    else
        //    //    {
        //    //        //divHideShowPhoneNumber.Style["display"] = "block";
        //    //        rfvPhoneNumber.Enabled = true;
        //    //        //spnPhoneNumberReq.Style["display"] = "";
        //    //    }
        //    //}

        //}
        //#endregion

        #region UAT-1612 : As an applicant, my middle name should be required.
        private void ValidateMiddleName(String middleName)
        {
            if (middleName.IsNullOrEmpty())
            {
                divMiddleNameCheckBox.Visible = true;
                chkMiddleNameRequired.Checked = true;
                txtMiddleName.Text = NoMiddleNameText;
                txtMiddleName.Enabled = false;
                rfvMiddleName.Enabled = false;
                spnMiddleName.Style["display"] = "none";
                //if (CurrentViewContext.IsLocationServiceTenant)
                //{
                //    txtMiddleName.Attributes.Add("PlaceHolder", "");
                //    txtMiddleName.ToolTip = "";
                //}
            }
            else
            {
                divMiddleNameCheckBox.Visible = true;
                chkMiddleNameRequired.Checked = false;
                txtMiddleName.Text = middleName;
                txtMiddleName.Enabled = true;
                rfvMiddleName.Enabled = true;
                spnMiddleName.Style["display"] = "";
                spnMiddleName.Visible = true;
            }
        }
        #endregion

        private void HideShowInternationalCriminalAttributes()
        {
            if (!CurrentViewContext.LstInternationCriminalSrchAttributes.IsNullOrEmpty())
            {
                if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == LCNAttCode.ToString() && x.IsAttributeDisplay))
                {
                    divInternationalCriminalSearchAttributes.Style.Add("display", "block");
                    divCriminalLicenseNumber.Style.Add("display", "block");
                    if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == LCNAttCode.ToString() && x.IsAttributeRequired))
                    {
                        rfvCriminalLicenseNumber.Enabled = true;
                        spnCriminalLicenseNumber.Style.Add("display", "");
                        hdnLicenseRequired.Value = "True";
                    }
                }
                if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == MotherNameAttrCode.ToString() && x.IsAttributeDisplay))
                {
                    divInternationalCriminalSearchAttributes.Style.Add("display", "block");
                    divMothersName.Style.Add("display", "block");
                    if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == MotherNameAttrCode.ToString() && x.IsAttributeRequired))
                    {
                        rfvMotherName.Enabled = true;
                        spnMotherName.Style.Add("display", "");
                        hdnIsMotherNameRequired.Value = "True";
                    }
                }
                if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == IdentificationNumberAttrCode.ToString() && x.IsAttributeDisplay))
                {
                    divInternationalCriminalSearchAttributes.Style.Add("display", "block");
                    divIdentificationNumber.Style.Add("display", "block");
                    if (CurrentViewContext.LstInternationCriminalSrchAttributes.Any(x => x.BSA_Code == IdentificationNumberAttrCode.ToString() && x.IsAttributeRequired))
                    {
                        rfvIdentificationNumber.Enabled = true;
                        spnIdentificationNumber.Style.Add("display", "");
                        hdnIsIdentificationRequired.Value = "True";
                    }
                }
            }
        }

        /// <summary>
        /// Start user order 
        /// 1. To purchase bulk order
        /// 2. There is no compliance package to select, for the applicant
        /// </summary>
        private void StartOrder()
        {
            applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            if (applicantOrderCart == null)
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.GetApplicantOrder();
            }
            applicantOrderCart.IsBulkOrder = true;
            applicantOrderCart.BulkOrderUploadID = CurrentViewContext.BulkOrderUploadID;

            // This check is required as when user navigates back from the Applicant Profile screen, 
            // then query string will be NULL and GetNavigationFrom() will set this property to ApplicantDashBoard,
            // even if the user had started from the ApplicantLanding page
            if (String.IsNullOrEmpty(applicantOrderCart.PendingOrderNavigationFrom))
            {
                applicantOrderCart.PendingOrderNavigationFrom = GetNavigationFrom();
            }

            if (!String.IsNullOrEmpty(applicantOrderCart.EDrugScreeningRegistrationId))
            {
                applicantOrderCart.EDrugScreeningRegistrationId = null;
            }

            SetSelectedHierarchyData();

            var _isBundleSelected = false;
            applicantOrderCart.lstSelectedPkgBundleId = null;//UAT-3283
            // applicantOrderCart.SelectedPkgBundleId = null;//UAT-3283
            if (applicantOrderCart.IsNotNull() && applicantOrderCart.OrderRequestType != OrderRequestType.ChangeSubscription.GetStringValue() && !_isBundleSelected)
            {
                #region Add Background Packages to Session object

                AddBackgroundPackageDataToSession();

                #endregion
            }

            applicantOrderCart.AddOrderStageTrackID(OrderStages.ApplicantProfile);

            // If No package is selected, then stop navigation
            if (!applicantOrderCart.IsCompliancePackageSelected && applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                //base.ShowInfoMessage("Please select any Package to continue.");
                base.ShowInfoMessage(Resources.Language.PLSSLCTPCKGS);
                return;
            }

            Int32 _customFormSteps = AppConsts.NONE;
            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                _customFormSteps = GetTotalCustomForms(applicantOrderCart.lstApplicantOrder[0].lstPackages.ToList());
            }
            applicantOrderCart.SetTotalOrderSteps(AppConsts.FOUR + _customFormSteps);

            applicantOrderCart.IncrementOrderStepCount();

            Int32 selectedHierarchyNodeId = CurrentViewContext.OrderNodeID;
            applicantOrderCart.NodeId = Presenter.GetLastNodeInstitutionId(selectedHierarchyNodeId);
            applicantOrderCart.SelectedHierarchyNodeID = selectedHierarchyNodeId;

            var ifInvoiceOnlyPymnOptn = Presenter.IfInvoiceIsOnlyPaymentOptions(selectedHierarchyNodeId);
            applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel = ifInvoiceOnlyPymnOptn;

            #region UAT-1560: WB: We should be able to add documents that need to be signed to the order process
            var isAdditionalDocumentExist = Presenter.GetAdditionalDocuments(applicantOrderCart.lstApplicantOrder[0].lstPackages, applicantOrderCart.SelectedHierarchyNodeID.Value,
                                             applicantOrderCart.CompliancePackages, applicantOrderCart.IsCompliancePackageSelected);
            applicantOrderCart.IsAdditionalDocumentExist = isAdditionalDocumentExist;
            if (isAdditionalDocumentExist)
            {
                applicantOrderCart.SetTotalOrderSteps(AppConsts.FIVE + _customFormSteps);
            }
            #endregion
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);

        }

        /// <summary>
        /// Returns the enum Code of the screen, from which has user has navigated from. 
        /// If it is empty, then defaults to the Dashboard
        /// </summary>
        /// <returns></returns>
        private String GetNavigationFrom()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                if (args.ContainsKey(AppConsts.PENDING_ORDER_NAVIGATION_FROM))
                    return Convert.ToString(args[AppConsts.PENDING_ORDER_NAVIGATION_FROM]);

                return PendingOrderNavigationFrom.ApplicantDashboard.GetStringValue();
            }
            return PendingOrderNavigationFrom.ApplicantDashboard.GetStringValue();
        }

        /// <summary>
        /// Set the data in properties for the entire selected hierarchy
        /// </summary>
        private void SetSelectedHierarchyData()
        {
            ClearHierarchyData();
            SetSelectedNodeData();
        }

        /// <summary>
        /// Resets hierarchy nodes from order cart.
        /// </summary>
        private void ClearHierarchyData()
        {
            if (applicantOrderCart.IsNotNull())
            {
                applicantOrderCart.alNodeIds = null;
                applicantOrderCart.DefaultNodeId = null;
            }
        }

        private void SetSelectedNodeData()
        {
            //Get the default node id from the default Institution.  
            applicantOrderCart.DefaultNodeId = Presenter.GetDefaultNodeId(TenantId);

            #region Set Selected hierarchy node id in Session

            if (applicantOrderCart.IsNotNull())
            {
                if (applicantOrderCart.alNodeIds.IsNull())
                    applicantOrderCart.alNodeIds = new ArrayList();

                Int32 OrderNodeID = CurrentViewContext.OrderNodeID;
                if (!applicantOrderCart.alNodeIds.Contains(Convert.ToString(OrderNodeID)))
                    applicantOrderCart.alNodeIds.Add(Convert.ToString(OrderNodeID));

            }

            #endregion
        }

        /// <summary>
        /// Add the data of Background Packages to the Session cart
        /// </summary>
        private void AddBackgroundPackageDataToSession()
        {
            List<BackgroundPackagesContract> _lstBackgroundPackages = new List<BackgroundPackagesContract>();

            //Get and set background order mapping data
            #region Generate Exclusive and Non Exclusive Bkg Packages' List

            BackgroundPackagesContract bkgPackage = Presenter.GetBackgroundPackage();
            _lstBackgroundPackages.Add(bkgPackage);

            #endregion

            ApplicantOrder _applicantOrder = new ApplicantOrder();
            if (applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
            {
                applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                applicantOrderCart.lstApplicantOrder.Add(new ApplicantOrder
                {
                    lstPackages = _lstBackgroundPackages,
                });
            }
            else
            {
                applicantOrderCart.lstApplicantOrder[0].lstPackages = _lstBackgroundPackages;
            }

            if (_lstBackgroundPackages.Count() > 0)
            {
                applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();
            }
        }

        /// <summary>
        /// Gets the total custom forms for the selected Background packages selected,
        /// to add the Total count to Session
        /// </summary>
        private Int32 GetTotalCustomForms(List<BackgroundPackagesContract> lstBkgPackages)
        {
            String _packageIds = String.Empty;
            if (!lstBkgPackages.IsNullOrEmpty())
            {
                lstBkgPackages.ForEach(pkgId => _packageIds += Convert.ToString(pkgId.BPAId) + ",");

                if (_packageIds.EndsWith(","))
                    _packageIds = _packageIds.Substring(0, _packageIds.Length - 1);
            }

            if (!String.IsNullOrEmpty(_packageIds))
                return _presenter.GetCustomFormsCount(_packageIds);

            return AppConsts.NONE;
        }

        private void ShowHidePhoneControls(Boolean IsInternationalNumber, Int32 ControlType)
        {
            if (ControlType == AppConsts.ONE)
            {
                if (IsInternationalNumber)
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
            if (ControlType == AppConsts.TWO)
            {
                if (IsInternationalNumber)
                {
                    dvUnMaskedSecondaryPhone.Style["display"] = "block";
                    dvMaskedSecondaryPhone.Style["display"] = "none";
                }

                else
                {
                    dvUnMaskedSecondaryPhone.Style["display"] = "none";
                    dvMaskedSecondaryPhone.Style["display"] = "block";
                }
            }
        }


        #region UAT-2625:Add ability to choose 18 and above, 17 or under, and all ages as options on D&A association


        #region sprint 147 UAT-3414

        private void SetApplicantAgeGroup(ApplicantOrderCart applicantOrderCart)
        {
            String ageGroup = String.Empty;
            DateTime? applicantDOB = !dpkrDOB.SelectedDate.IsNullOrEmpty() ? dpkrDOB.SelectedDate.Value : (DateTime?)null;
            DateTime? currentAge = applicantDOB.HasValue ? applicantDOB : null;
            if (!applicantDOB.IsNullOrEmpty() && DateTime.Now >= currentAge.Value.AddYears(18))
            {
                ageGroup = DisclosureDocumentAgeGroup.EIGHTEEN_AND_ABOVE.GetStringValue();
            }
            else
            {
                ageGroup = DisclosureDocumentAgeGroup.SEVENTEEN_OR_UNDER.GetStringValue();
            }
            applicantOrderCart.DisclosureAgeGroupType = ageGroup;
        }

        #endregion

        #endregion

        #region UAT-3545 CBI || CABS

        #region Properties

        List<ValidateRegexDataContract> IAdminEntryApplicantInformationView.lstValidateRegexDataContract
        {
            get
            {
                if (!ViewState["lstValidateRegexDataContract"].IsNullOrEmpty())
                    return ViewState["lstValidateRegexDataContract"] as List<ValidateRegexDataContract>;
                return new List<ValidateRegexDataContract>();
            }
            set
            {
                ViewState["lstValidateRegexDataContract"] = value;
            }
        }
        #endregion

        private void ValidatePersonalInformation()
        {
            GetPersonalInformationExpressions();
            String controlTovalidate = String.Empty;
            if (!CurrentViewContext.lstValidateRegexDataContract.IsNullOrEmpty())
            {
                foreach (ValidateRegexDataContract validateRegexData in CurrentViewContext.lstValidateRegexDataContract)
                {
                    switch (validateRegexData.BAGM_Code)
                    {
                        case ("88A26BC1-FE90-4103-8E83-D0133479DC00"):
                            controlTovalidate = txtFirstName.ID.ToString();
                            if (!validateRegexData.ValidateExpression.IsNullOrEmpty())
                                DivRegexFirstName.Controls.Add(SetRegularExpressionFieldValidator(controlTovalidate, validateRegexData.ValidateExpression, validateRegexData.ValidationMessage, true));
                            break;

                        case ("0260B1EA-2834-4CBD-B494-15EE84BB7016"):
                            controlTovalidate = txtMiddleName.ID.ToString();
                            if (!validateRegexData.ValidateExpression.IsNullOrEmpty())
                                DivRegexMiddleName.Controls.Add(SetRegularExpressionFieldValidator(controlTovalidate, validateRegexData.ValidateExpression, validateRegexData.ValidationMessage, true));
                            break;

                        case ("D4798877-38D4-4418-92B8-B9912EAB02E4"):
                            controlTovalidate = txtLastName.ID.ToString();
                            if (!validateRegexData.ValidateExpression.IsNullOrEmpty())
                                DivRegexLastName.Controls.Add(SetRegularExpressionFieldValidator(controlTovalidate, validateRegexData.ValidateExpression, validateRegexData.ValidationMessage, true));
                            break;

                        case ("62CAD6E3-AE98-4981-BF50-A25661506EB5"):
                            controlTovalidate = txtSSN.ID.ToString();
                            if (!validateRegexData.ValidateExpression.IsNullOrEmpty())
                                DivRegexSSN.Controls.Add(SetRegularExpressionFieldValidator(controlTovalidate, validateRegexData.ValidateExpression, validateRegexData.ValidationMessage, true));
                            break;

                        //case ("9D76FDE5-0E5B-429B-BE1B-2150E2582FD3"):
                        //    controlTovalidate = txtPhoneNumber.ID.ToString();
                        //    if (!validateRegexData.ValidateExpression.IsNullOrEmpty())
                        //        DivRegexPhoneNumber.Controls.Add(SetRegularExpressionFieldValidator(controlTovalidate, validateRegexData.ValidateExpression, validateRegexData.ValidationMessage, true));
                        //    break;

                        case ("326F6BCF-54F3-4820-8950-CB78D78F638C"):
                            controlTovalidate = txtSecondaryEmail.ID.ToString();
                            if (!validateRegexData.ValidateExpression.IsNullOrEmpty())
                                DivRegexSecondaryEmail.Controls.Add(SetRegularExpressionFieldValidator(controlTovalidate, validateRegexData.ValidateExpression, validateRegexData.ValidationMessage, true));
                            break;
                    }
                }
            }
        }

        private void GetPersonalInformationExpressions()
        {
            Presenter.GetPersonalInformationExpressions();
        }

        private HtmlGenericControl SetRegularExpressionFieldValidator(String controlTovalidate, String validationExpression, String ValidationMessage, Boolean enable = true)
        {
            HtmlGenericControl regularExpressionDiv = new HtmlGenericControl("div");
            regularExpressionDiv.Attributes.Add("class", "vldx");
            RegularExpressionValidator regularExpression = new RegularExpressionValidator();
            regularExpression.ControlToValidate = controlTovalidate;
            if (controlTovalidate == txtSSN.ID.ToString())
            {
                regularExpression.ID = "rev_" + controlTovalidate;
            }
            else
            {
                regularExpression.ID = "rev" + controlTovalidate;
            }
            if (!ValidationMessage.IsNullOrEmpty())
            {
                regularExpression.ErrorMessage = ValidationMessage.ToString();
            }
            else
            {
                //regularExpression.ErrorMessage = "Incorrect Format";
                regularExpression.ErrorMessage = Resources.Language.INCRTFORMAT;
            }
            regularExpression.ValidationGroup = "grpFormSubmit";
            regularExpression.CssClass = "errmsg";
            regularExpression.Enabled = enable;
            regularExpression.Display = ValidatorDisplay.Dynamic;
            regularExpression.ValidationExpression = validationExpression;
            regularExpressionDiv.Controls.Add(regularExpression);
            return regularExpressionDiv;
        }

        private String ValidatePageData()
        {
            StringBuilder xmlStringData = new StringBuilder();
            xmlStringData.Append("<Attributes>");
            xmlStringData.Append("<Attribute><AttributeName>DOB</AttributeName><AttributeValue>" + Convert.ToString(dpkrDOB.SelectedDate, CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue())) + "</AttributeValue></Attribute>");
            xmlStringData.Append("</Attributes>");

            return Presenter.validatePageData(xmlStringData);
        }

        private void AddSuffixDropdownAndDesignChange()
        {
            //if (CurrentViewContext.IsLocationServiceTenant)
            //{
            dvSpnFirstName.Style.Add("Width", "10%");
            dvSpnMiddleName.Style.Add("Width", "10%");
            dvSpnLastName.Style.Add("Width", "10%");

            dvFirstName.Style.Add("Width", "17%");
            dvMiddleName.Style.Add("Width", "17%");
            dvLastName.Style.Add("Width", "17%");

            dvSuffix.Style.Add("display", "inline-block");
            Presenter.GetSuffixes();
            //if (CurrentViewContext.IsSuffixDropDownType)
            //{
            cmbSuffix.Visible = true;
            BindSuffixDropdown();
            //txtSuffix.Visible = false;
            //}
            //else
            //{
            //    cmbSuffix.Visible = false;
            //    txtSuffix.Visible = true;
            //}
            //}

            //else
            //{
            //    dvSuffix.Style.Add("display", "none");
            //}
        }

        /// <summary>
        /// Bind suffix dropdown, only in case of location service tenant.
        /// </summary>
        private void BindSuffixDropdown()
        {
            //cmbSuffix.DataSource = CurrentViewContext.lstSuffixes.Where(a => a.IsSystem);
            cmbSuffix.DataSource = CurrentViewContext.lstSuffixes;
            cmbSuffix.DataBind();
            cmbSuffix.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(Resources.Language.SELECTSUFFIX, "0"));
        }
        #endregion

        private void ManageSSN()
        {
            String AppSSN = txtSSN.Text.Trim();
            if (AppSSN == AppConsts.DefaultSSN)
            {
                rblSSN.SelectedValue = "false";
                rfvSSN.Enabled = false;
                txtSSN.Enabled = false;
                //if (CurrentViewContext.IsLocationServiceTenant)
                //{
                //rgvSSNCBI.Enabled = false;
                //txtSSN.Visible = false;
                //lblSSN.Visible = false;
                //}
            }
            else
            {
                rblSSN.SelectedValue = "true";
                txtSSN.Enabled = true;
                rfvSSN.Enabled = true;
                //if (CurrentViewContext.IsLocationServiceTenant)
                //{
                //    rgvSSNCBI.Enabled = true;
                //    txtSSN.Visible = true;
                //    lblSSN.Visible = true;
                //    revtxtSSN.Enabled = false;
                //}
            }
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            //if (CurrentViewContext.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsSSNRequired)
            //{
            //    rblSSN.SelectedValue = "true";
            //    rblSSN.Enabled = false;
            //    rgvSSNCBI.Enabled = true;
            //    txtSSN.Visible = true;
            //    lblSSN.Visible = true;
            //    txtSSN.Enabled = true;
            //    rfvSSN.Enabled = true;
            //    revtxtSSN.Enabled = false;
            //    if (AppSSN == AppConsts.DefaultSSN)
            //    {
            //        txtSSN.Text = String.Empty;
            //    }

            //}
        }
        private void CaptureQueryString()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("OrderId"))
                {
                    CurrentViewContext.OrderID = Convert.ToInt32(args["OrderId"]);
                }

            }
        }
    }
}