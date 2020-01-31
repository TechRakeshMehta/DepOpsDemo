using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class PreviousAddressControl : BaseUserControl
    {

        #region Variables

        #region Private Variables
        private PreviousAddressControlPresenter _presenter = new PreviousAddressControlPresenter();
        private Boolean _isFalsePostBack = false;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public PreviousAddressControlPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                this._presenter = value;
            }
        }

        public Boolean ShowEditColumn
        {
            set
            {
                grdResidentialHistory.Columns.FindByUniqueName("EditCommandColumn").Visible = value;
            }
        }

        public Boolean ShowDeleteColumn
        {
            set
            {
                grdResidentialHistory.Columns.FindByUniqueName("DeleteColumn").Visible = value;
            }
        }

        public List<PreviousAddressContract> ResidentialHistoryList
        {
            get
            {
                if (!(ViewState["ResidentialHistoryList"] is List<PreviousAddressContract>))
                {
                    // need to fix the memory and added to viewstate
                    ViewState["ResidentialHistoryList"] = new List<PreviousAddressContract>();
                }
                return (List<PreviousAddressContract>)ViewState["ResidentialHistoryList"];
            }
            set
            {
                List<PreviousAddressContract> previousAddressContract = value.IsNotNull()
                                                                        ? value.OrderBy(cond => cond.ResHistorySeqOrdID).ToList()
                                                                        : new List<PreviousAddressContract>();
                int seqCount = AppConsts.TWO;
                for (var i = 0; i < previousAddressContract.Where(cond => cond.isCurrent == false && cond.isDeleted == false).Count(); i++)
                {
                    previousAddressContract[i].ResHistorySeqOrdID = seqCount;
                    previousAddressContract[i].isUpdated = true;
                    seqCount++;
                }
                ViewState["ResidentialHistoryList"] = previousAddressContract;
            }
        }

        public Int32? MaxOccurance
        {
            get
            {
                if (!(ViewState["MaxOccurance"].IsNull()))
                {
                    return (Int32)ViewState["MaxOccurance"];
                }
                return null;
            }
            set
            {
                ViewState["MaxOccurance"] = value;
            }
        }
        public Boolean IsEditProfile
        {
            get
            {
                if ((ViewState["IsEditProfile"].IsNullOrEmpty()))
                {
                    ViewState["IsEditProfile"] = false;
                }
                return (Boolean)ViewState["IsEditProfile"];
            }
            set
            {
                ViewState["IsEditProfile"] = value;
            }
        }

        public Boolean HideAddNewButton
        {
            get;
            set;
        }

        public List<PreviousAddressContract> ResidentialHitoryTempList
        {
            get
            {
                if (!(ViewState["ResidentialHitoryTempList"] is List<PreviousAddressContract>))
                {
                    // need to fix the memory and added to viewstate
                    ViewState["ResidentialHitoryTempList"] = new List<PreviousAddressContract>();
                }
                return (List<PreviousAddressContract>)ViewState["ResidentialHitoryTempList"];
            }
            set
            {
                ViewState["ResidentialHitoryTempList"] = value;
            }
        }


        public Boolean IsApplicantOrderScreen
        {
            get
            {
                if (ViewState["IsApplicantOrderScreen"].IsNullOrEmpty())
                    return false;
                return Convert.ToBoolean(ViewState["IsApplicantOrderScreen"]);
            }
            set
            {
                ViewState["IsApplicantOrderScreen"] = value;
            }
        }

        public Int32 MaxNumberOfYearforResidence
        {
            get
            {
                if (ViewState["MaxNumberOfYearforResidence"].IsNullOrEmpty())
                    return -2;
                return Convert.ToInt32(ViewState["MaxNumberOfYearforResidence"]);
            }
            set
            {
                ViewState["MaxNumberOfYearforResidence"] = value;
            }
        }

        public Boolean IsFalsePostBack
        {
            get
            {
                return _isFalsePostBack;
            }
            set
            {
                _isFalsePostBack = value;
            }
        }


        public Boolean ShowCriminalAttribute_MotherName
        {
            set
            {
                hdnShowCriminalAttribute_MotherName.Value = value.ToString();
            }
            get
            {
                if (hdnShowCriminalAttribute_MotherName.Value.IsNullOrEmpty())
                {
                    return false;
                }
                return Convert.ToBoolean(hdnShowCriminalAttribute_MotherName.Value);
            }
        }

        public Boolean ShowCriminalAttribute_License
        {
            set
            {
                hdnShowCriminalAttribute_License.Value = value.ToString();
            }
            get
            {
                if (hdnShowCriminalAttribute_License.Value.IsNullOrEmpty())
                {
                    return false;
                }
                return Convert.ToBoolean(hdnShowCriminalAttribute_License.Value);
            }
        }

        public Boolean ShowCriminalAttribute_Identification
        {
            set
            {
                hdnShowCriminalAttribute_Identification.Value = value.ToString();
            }
            get
            {
                if (hdnShowCriminalAttribute_Identification.Value.IsNullOrEmpty())
                {
                    return false;
                }
                return Convert.ToBoolean(hdnShowCriminalAttribute_Identification.Value);
            }
        }


        #endregion

        #endregion

        #region Events

        protected void grdResidentialHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (IsApplicantOrderScreen)
                {
                    if (MaxNumberOfYearforResidence == -1 || MaxNumberOfYearforResidence == -2)
                    {
                        lblChkShowHide.InnerHtml = "I have a residential history.";
                    }
                    else
                    {
                        lblChkShowHide.InnerHtml = "I have lived in additional locations during the last " + MaxNumberOfYearforResidence + " year(s).";
                    }
                }
                else
                {
                    chkShowHideResidenceHistory.Visible = false;
                    divResidentialShowHide.Style["display"] = "block";
                }
                if (ResidentialHistoryList.IsNull())
                    ResidentialHistoryList = new List<PreviousAddressContract>();

                //grdResidentialHistory.DataSource = ResidentialHistoryList.Where(x => x.isDeleted == false && x.isCurrent == false)
                //                                   .OrderByDescending(cond => cond.ResidenceStartDate).ToList();

                List<PreviousAddressContract> addressSourceTemp = ResidentialHistoryList.Where(x => x.isDeleted == false && x.isCurrent == false)
                                                                                    .OrderBy(cond => cond.ResHistorySeqOrdID).ToList();

                //Created a list of previous address that contains only addresses count equal to max. occurence of package.//UAT 605
                List<PreviousAddressContract> addressSource = new List<PreviousAddressContract>();
                if (!MaxOccurance.IsNullOrEmpty())
                {
                    Int32? maxTempOccurance = MaxOccurance == AppConsts.NONE ? MaxOccurance : MaxOccurance.Value - AppConsts.ONE;
                    addressSource = ResidentialHistoryList.Where(x => x.isDeleted == false && x.isCurrent == false)
                                                                                        .OrderBy(cond => cond.ResHistorySeqOrdID).Take(maxTempOccurance.Value).ToList();//UAT605

                }

                //UAT-4540 
                if (!MaxNumberOfYearforResidence.IsNullOrEmpty() && MaxNumberOfYearforResidence != -1 && MaxNumberOfYearforResidence != -2)
                {
                    addressSourceTemp = ResidentialHistoryList.Where(x => x.isDeleted == false && x.isCurrent == false
                                                                        && x.ResidenceEndDate >= DateTime.Now.AddYears(-MaxNumberOfYearforResidence))
                                                                                    .OrderBy(cond => cond.ResHistorySeqOrdID).ToList();

                    if (!MaxOccurance.IsNullOrEmpty())
                    {
                        Int32? maxTempOccurance = MaxOccurance == AppConsts.NONE ? MaxOccurance : MaxOccurance.Value - AppConsts.ONE;

                        addressSource = ResidentialHistoryList.Where(x => x.isDeleted == false && x.isCurrent == false
                                                                        && x.ResidenceEndDate >= DateTime.Now.AddYears(-MaxNumberOfYearforResidence))
                                                                                            .OrderBy(cond => cond.ResHistorySeqOrdID).Take(maxTempOccurance.Value).ToList();
                    }
                }

                //END UAT-4540 

                if (IsEditProfile || (MaxOccurance == AppConsts.NONE) || MaxOccurance.IsNullOrEmpty())
                {
                    ResidentialHitoryTempList = ResidentialHistoryList;
                    grdResidentialHistory.DataSource = addressSourceTemp;
                }
                else
                {
                    ResidentialHitoryTempList = addressSource;
                    grdResidentialHistory.DataSource = addressSource;
                }
                //ResidentialHistoryList.Count() + AppConsts.ONE - > 1 is added to capture the current residential History also. 
                //as Current Residential History is also part of the Applicant Residentail History. Thus we have to add "1" in ResidentialHistoryList.
                //if (MaxOccurance > AppConsts.NONE && (addressSourceTemp.Count() + AppConsts.ONE) >= MaxOccurance)// commented code for UAT-605 and added below checks.
                if ((!MaxOccurance.IsNullOrEmpty() && MaxOccurance > AppConsts.NONE) && (((addressSource.Count() + AppConsts.ONE) >= MaxOccurance.Value) || ((addressSourceTemp.Count() + AppConsts.ONE) >= MaxOccurance.Value)))
                {
                    grdResidentialHistory.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                }
                else
                {
                    if (MaxOccurance.IsNullOrEmpty() || (MaxOccurance != AppConsts.NONE))
                        grdResidentialHistory.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
                    else if (MaxOccurance == AppConsts.NONE)
                        grdResidentialHistory.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

                }
                if (HideAddNewButton)
                {
                    grdResidentialHistory.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                }

                if (ResidentialHitoryTempList.Where(x => !x.isDeleted).Select(x => x).Count() > 0)
                {
                    divResidentialShowHide.Style["display"] = "block";
                    chkShowHideResidenceHistory.Checked = true;
                    chkShowHideResidenceHistory.Disabled = true;
                }
                else
                {
                    if (chkShowHideResidenceHistory.Checked)
                        divResidentialShowHide.Style["display"] = "block";
                    else
                    {
                        if (IsApplicantOrderScreen)
                            divResidentialShowHide.Style["display"] = "none";
                        else
                            divResidentialShowHide.Style["display"] = "block";
                    }
                    chkShowHideResidenceHistory.Disabled = false;
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

        protected void grdResidentialHistory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                PreviousAddressContract previousAddress = null;
                if (ResidentialHistoryList.IsNull())
                    ResidentialHistoryList = new List<PreviousAddressContract>();
                if (e.CommandName == "PerformInsert")
                {
                    CoreWeb.CommonControls.Views.LocationInfo _locInfo = (e.Item.FindControl("locationTenant") as CoreWeb.CommonControls.Views.LocationInfo);

                    if (!IsValidAddress(_locInfo.MasterZipcodeID.Value, _locInfo.RSLCountryId))
                    {
                        base.ShowErrorInfoMessage("Please select a valid ZipCode.");
                        return;
                    }

                    previousAddress = new PreviousAddressContract();
                    previousAddress.TempId = Guid.NewGuid().ToString();
                    previousAddress.Address1 = String.Empty; //(e.Item.FindControl("txtAddress1") as WclTextBox).Text.Trim();
                    previousAddress.Address2 = String.Empty; //(e.Item.FindControl("txtAddress2") as WclTextBox).Text.Trim();

                    previousAddress.ZipCodeID = _locInfo.MasterZipcodeID.Value;
                    if (_locInfo.MasterZipcodeID.Value > 0)
                    {
                        previousAddress.CountyName = _locInfo.RSLCountyName;
                    }
                    else
                    {
                        previousAddress.CountryId = _locInfo.RSLCountryId;
                    }
                    previousAddress.Zipcode = _locInfo.RSLZipCode;
                    previousAddress.CityName = _locInfo.RSLCityName;
                    previousAddress.StateName = _locInfo.RSLStateName;
                    previousAddress.Country = _locInfo.RSLCountryName;
                    previousAddress.ResidenceStartDate = (e.Item.FindControl("dpResFrm") as WclDatePicker).SelectedDate;
                    previousAddress.ResidenceEndDate = (e.Item.FindControl("dpResTill") as WclDatePicker).SelectedDate;

                    if (_locInfo.RSLCountryId != AppConsts.COUNTRY_USA_ID && _locInfo.RSLCountryId != AppConsts.NONE && IsApplicantOrderScreen)
                    {
                        if (ShowCriminalAttribute_Identification)
                        {
                            previousAddress.IdentificationNumber = (e.Item.FindControl("txtIdentificationNumber_grd") as WclTextBox).Text.Trim();
                        }
                        if (ShowCriminalAttribute_License)
                        {
                            previousAddress.LicenseNumber = (e.Item.FindControl("txtCriminalLicenseNumber_grd") as WclTextBox).Text.Trim();
                        }
                        if (ShowCriminalAttribute_MotherName)
                        {
                            previousAddress.MotherName = (e.Item.FindControl("txtMotherName_grd") as WclTextBox).Text.Trim();
                        }
                    }

                    if (IsApplicantOrderScreen)
                    {
                        if (
                             ((MaxNumberOfYearforResidence != -1) && (MaxNumberOfYearforResidence != -2))
                             &&
                             (DateTime.Now.AddYears(-MaxNumberOfYearforResidence) >= previousAddress.ResidenceEndDate.Value)
                           )

                        //(((DateTime.Now.Year - MaxNumberOfYearforResidence) >= previousAddress.ResidenceEndDate.Value.Year) &&
                        //(DateTime.Now.Month >= previousAddress.ResidenceEndDate.Value.Month) && DateTime.Now.Day > previousAddress.ResidenceEndDate.Value.Day))
                        {
                            (e.Item.FindControl("lblErrorMessage") as Label).Text = "Please enter resident history of previous " + MaxNumberOfYearforResidence + " year(s) only.";
                            e.Canceled = true;
                            return;
                        }
                    }

                    Int32 resHistorySeqOrdID = ResidentialHistoryList.Where(cond => cond.isDeleted == false && cond.isCurrent == false).Count() > AppConsts.NONE
                                                ? (ResidentialHistoryList.Where(cond => cond.isDeleted == false && cond.isCurrent == false)
                                                .Max(co => co.ResHistorySeqOrdID) + AppConsts.ONE)
                                                : AppConsts.TWO;

                    previousAddress.ResHistorySeqOrdID = resHistorySeqOrdID;
                    previousAddress.isNew = true;
                    ResidentialHistoryList.Add(previousAddress);
                    e.Canceled = false;
                }
                if (e.CommandName == "Update")
                {
                    CoreWeb.CommonControls.Views.LocationInfo _locInfo = (e.Item.FindControl("locationTenant") as CoreWeb.CommonControls.Views.LocationInfo);
                    if (!IsValidAddress(_locInfo.MasterZipcodeID.Value, _locInfo.RSLCountryId))
                    {
                        base.ShowErrorInfoMessage("Please select a valid ZipCode.");
                        return;
                    }

                    Int32 ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
                    //UAT-705
                    if (IsApplicantOrderScreen)
                    {
                        DateTime? endDate = (e.Item.FindControl("dpResTill") as WclDatePicker).SelectedDate;
                        if (((MaxNumberOfYearforResidence != -1) && (MaxNumberOfYearforResidence != -2)) &&
                            (
                                (DateTime.Now.AddYears(-MaxNumberOfYearforResidence) >= endDate.Value))
                            )
                        //((DateTime.Now.Year - MaxNumberOfYearforResidence) >= endDate.Value.Year) &&
                        //(DateTime.Now.Month >= endDate.Value.Month) && DateTime.Now.Day > endDate.Value.Day)
                        {
                            (e.Item.FindControl("lblErrorMessage") as Label).Text = "Please enter resident history of previous " + MaxNumberOfYearforResidence + " year(s) only.";
                            e.Canceled = true;
                            return;
                        }
                    }

                    if (ID > 0)
                    {
                        previousAddress = ResidentialHistoryList.FirstOrDefault(x => x.ID.Equals(ID));
                    }
                    else
                    {
                        String tempId = (e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempId"].ToString().Trim();
                        var resHistory = ResidentialHistoryList.Where(x => x.TempId == tempId);
                        if (resHistory.IsNotNull())
                        {
                            previousAddress = resHistory.FirstOrDefault();
                        }
                    }
                    if (previousAddress.IsNotNull())
                    {
                        previousAddress.Address1 = String.Empty; //(e.Item.FindControl("txtAddress1") as WclTextBox).Text.Trim();
                        previousAddress.Address2 = String.Empty; //(e.Item.FindControl("txtAddress2") as WclTextBox).Text.Trim();

                        previousAddress.ZipCodeID = _locInfo.MasterZipcodeID.Value;
                        if (_locInfo.MasterZipcodeID.Value > 0)
                        {
                            previousAddress.CountyName = _locInfo.RSLCountyName;
                        }
                        else
                        {
                            previousAddress.CountyName = null;
                            previousAddress.CountryId = _locInfo.RSLCountryId;
                        }
                        previousAddress.Zipcode = _locInfo.RSLZipCode;
                        previousAddress.CityName = _locInfo.RSLCityName;
                        previousAddress.StateName = _locInfo.RSLStateName;
                        previousAddress.Country = _locInfo.RSLCountryName;
                        previousAddress.ResidenceStartDate = (e.Item.FindControl("dpResFrm") as WclDatePicker).SelectedDate;
                        previousAddress.ResidenceEndDate = (e.Item.FindControl("dpResTill") as WclDatePicker).SelectedDate;
                        if (_locInfo.RSLCountryId != AppConsts.COUNTRY_USA_ID && _locInfo.RSLCountryId != AppConsts.NONE && IsApplicantOrderScreen)
                        {
                            if (ShowCriminalAttribute_Identification)
                            {
                                previousAddress.IdentificationNumber = (e.Item.FindControl("txtIdentificationNumber_grd") as WclTextBox).Text.Trim();
                            }
                            if (ShowCriminalAttribute_License)
                            {
                                previousAddress.LicenseNumber = (e.Item.FindControl("txtCriminalLicenseNumber_grd") as WclTextBox).Text.Trim();
                            }
                            if (ShowCriminalAttribute_MotherName)
                            {
                                previousAddress.MotherName = (e.Item.FindControl("txtMotherName_grd") as WclTextBox).Text.Trim();
                            }
                        }
                        if (!previousAddress.isNew)
                            previousAddress.isUpdated = true;
                    }
                    e.Canceled = false;
                }
                if (e.CommandName == "Delete")
                {
                    Int32 ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
                    if (ID > 0)
                    {
                        previousAddress = ResidentialHistoryList.FirstOrDefault(x => x.ID.Equals(ID));
                        if (previousAddress.IsNotNull())
                        {
                            previousAddress.isDeleted = true;
                            SetResHistorySeqOrderID(ResidentialHistoryList, previousAddress.ResHistorySeqOrdID);
                        }
                    }
                    else
                    {
                        String tempId = (e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempId"].ToString().Trim();
                        var resHistory = ResidentialHistoryList.Where(x => x.TempId == tempId);
                        if (resHistory.IsNotNull())
                        {
                            previousAddress = resHistory.FirstOrDefault();
                            if (previousAddress.IsNotNull() && previousAddress.isNew == true)
                            {
                                ResidentialHistoryList.Remove(previousAddress);
                                SetResHistorySeqOrderID(ResidentialHistoryList, previousAddress.ResHistorySeqOrdID);
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
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdResidentialHistory_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {

                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclDatePicker _dpResFrom = (WclDatePicker)editform.FindControl("dpResFrm");
                    _dpResFrom.MinDate = Convert.ToDateTime("01-01-1900");
                    _dpResFrom.MaxDate = DateTime.Now;
                    WclDatePicker _dpResTill = (WclDatePicker)editform.FindControl("dpResTill");
                    _dpResTill.MinDate = Convert.ToDateTime("01-01-1900");
                    _dpResTill.MaxDate = DateTime.Now;

                    CompareValidator compValidate = (CompareValidator)editform.FindControl("cfvResTill");
                    compValidate.ControlToValidate = _dpResTill.ID;
                    compValidate.ControlToCompare = _dpResFrom.ID;
                    compValidate.Operator = ValidationCompareOperator.GreaterThanEqual;
                    compValidate.ErrorMessage = "Resident until Date should not be less than Move in Date";

                    Label _lblEHPrevAddress = (Label)editform.FindControl("lblEHPrevAddress");

                    HtmlGenericControl divInternationalCriminalSearchAttributes = (HtmlGenericControl)editform.FindControl("divInternationalCriminalSearchAttributes_grd");
                    divInternationalCriminalSearchAttributes.Style.Add("display", "none");
                    ((HtmlGenericControl)editform.FindControl("divMothersName_grd")).Style.Add("display", "none");
                    ((HtmlGenericControl)editform.FindControl("divIdentificationNumber_grd")).Style.Add("display", "none");
                    ((HtmlGenericControl)editform.FindControl("divCriminalLicenseNumber_grd")).Style.Add("display", "none");
                    CoreWeb.CommonControls.Views.LocationInfo _locInfo = (editform.FindControl("locationTenant") as CoreWeb.CommonControls.Views.LocationInfo);
                    if (IsApplicantOrderScreen)
                    {
                        _locInfo.ShowCriminalAttribute_Identification = ShowCriminalAttribute_Identification;
                        _locInfo.ShowCriminalAttribute_License = ShowCriminalAttribute_License;
                        _locInfo.ShowCriminalAttribute_MotherName = ShowCriminalAttribute_MotherName;
                    }
                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                    {
                        // insert item
                        _lblEHPrevAddress.Text = GetLblEHPrevAddress(true, AppConsts.NONE);
                    }
                    else
                    {
                        var dataItem = ((PreviousAddressContract)((e.Item).DataItem));
                        if (dataItem.IsNotNull())
                        {

                            _locInfo.MasterZipcodeID = dataItem.ZipCodeID;
                            if (dataItem.ZipCodeID == 0)
                            {
                                _locInfo.RSLStateName = dataItem.StateName;
                                _locInfo.RSLCityName = dataItem.CityName;
                                _locInfo.RSLZipCode = dataItem.Zipcode;
                                _locInfo.RSLCountryId = dataItem.CountryId;
                                if (dataItem.CountryId != AppConsts.COUNTRY_USA_ID && dataItem.CountryId != AppConsts.NONE && IsApplicantOrderScreen)
                                {
                                    if (ShowCriminalAttribute_Identification)
                                    {
                                        divInternationalCriminalSearchAttributes.Style.Add("display", "block");
                                        ((HtmlGenericControl)editform.FindControl("divIdentificationNumber_grd")).Style.Add("display", "block");
                                        ((WclTextBox)editform.FindControl("txtIdentificationNumber_grd")).Text = dataItem.IdentificationNumber;
                                        HtmlInputHidden hdnIsIdentificationRequired = (Parent.FindControl("hdnIsIdentificationRequired")) as HtmlInputHidden;
                                        if (!hdnIsIdentificationRequired.IsNullOrEmpty() && hdnIsIdentificationRequired.Value == "True")
                                        {
                                            ((RequiredFieldValidator)editform.FindControl("rfvIdentificationNumber_grd")).Enabled = true;
                                            ((HtmlGenericControl)editform.FindControl("spnIdentificationNumber_grd")).Style.Add("display", "");
                                        }
                                    }
                                    if (ShowCriminalAttribute_License)
                                    {
                                        divInternationalCriminalSearchAttributes.Style.Add("display", "block");
                                        ((HtmlGenericControl)editform.FindControl("divCriminalLicenseNumber_grd")).Style.Add("display", "block");
                                        ((WclTextBox)editform.FindControl("txtCriminalLicenseNumber_grd")).Text = dataItem.LicenseNumber;
                                        HtmlInputHidden hdnLicenseRequired = (Parent.FindControl("hdnLicenseRequired")) as HtmlInputHidden;
                                        if (!hdnLicenseRequired.IsNullOrEmpty() && hdnLicenseRequired.Value == "True")
                                        {
                                            ((RequiredFieldValidator)editform.FindControl("rfvCriminalLicenseNumber_grd")).Enabled = true;
                                            ((HtmlGenericControl)editform.FindControl("spnCriminalLicenseNumber_grd")).Style.Add("display", "");
                                        }
                                    }
                                    if (ShowCriminalAttribute_MotherName)
                                    {
                                        divInternationalCriminalSearchAttributes.Style.Add("display", "block");
                                        ((HtmlGenericControl)editform.FindControl("divMothersName_grd")).Style.Add("display", "block");
                                        ((WclTextBox)editform.FindControl("txtMotherName_grd")).Text = dataItem.MotherName;
                                        HtmlInputHidden hdnIsMotherNameRequired = (Parent.FindControl("hdnIsMotherNameRequired")) as HtmlInputHidden;
                                        if (!hdnIsMotherNameRequired.IsNullOrEmpty() && hdnIsMotherNameRequired.Value == "True")
                                        {
                                            ((RequiredFieldValidator)editform.FindControl("rfvMotherName_grd")).Enabled = true;
                                            ((HtmlGenericControl)editform.FindControl("spnMotherName_grd")).Style.Add("display", "");
                                        }
                                    }
                                }
                            }
                            _locInfo.SetValueInControls = true;
                        }

                        Int32 ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
                        if (ID > 0)
                        {
                            var previousAddress = ResidentialHistoryList.FirstOrDefault(x => x.ID.Equals(ID));
                            _lblEHPrevAddress.Text = GetLblEHPrevAddress(false, (previousAddress.ResHistorySeqOrdID - AppConsts.ONE));
                        }
                        else
                        {
                            String tempId = (e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempId"].ToString().Trim();
                            var resHistory = ResidentialHistoryList.Where(x => x.TempId == tempId);
                            if (resHistory.IsNotNull())
                            {
                                var _resHistory = resHistory.FirstOrDefault();
                                _lblEHPrevAddress.Text = GetLblEHPrevAddress(false, (_resHistory.ResHistorySeqOrdID - AppConsts.ONE));
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
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        private static void SetResHistorySeqOrderID(List<PreviousAddressContract> previousAddressContractList, Int32 deletedRecordID)
        {
            for (Int16 resHist = 0; resHist < previousAddressContractList.Count(); resHist++)
            {
                if (previousAddressContractList[resHist].ResHistorySeqOrdID > deletedRecordID)
                {
                    previousAddressContractList[resHist].ResHistorySeqOrdID = previousAddressContractList[resHist].ResHistorySeqOrdID - AppConsts.ONE;
                    previousAddressContractList[resHist].isUpdated = true;
                }
            }
        }

        private String GetLblEHPrevAddress(Boolean isInsertMode, Int32 resHistorySeqOrderID)
        {
            if (isInsertMode)
            {
                int resHistoryListCount = ResidentialHistoryList.Where(cond => cond.isDeleted == false).Count();
                return GetLableContent(resHistoryListCount + AppConsts.ONE);
            }
            else
            {
                return GetLableContent(resHistorySeqOrderID);
            }
        }

        private String GetLableContent(Int32 resHistoryCount)
        {
            switch (resHistoryCount)
            {
                case AppConsts.ONE:
                    return "Residence - Previous";
                //case AppConsts.TWO:
                //    return "Residence - Previous";
                default:
                    return String.Format("Residence - {0} before Current", resHistoryCount);
            }
        }

        /// <summary>
        /// Method to avoid registration with 0 ZipCodeId for USA address - UAT 934
        /// </summary>
        private Boolean IsValidAddress(Int32 zipCodeId, Int32 countryId)
        {
            if (zipCodeId == AppConsts.NONE && countryId == AppConsts.COUNTRY_USA_ID)
                return false;
            return true;
        }

        #endregion

        #endregion

    }
}