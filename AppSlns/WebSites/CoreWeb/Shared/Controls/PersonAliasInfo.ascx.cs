using Business.RepoManagers;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.Shell.Views
{
    public partial class PersonAliasInfo : BaseUserControl, IPersonAliasInfoView// System.Web.UI.UserControl
    {
        #region Variables
        private Boolean _isFalsePostBack = false;
        private PersonAliasInfoPresenter _presenter = new PersonAliasInfoPresenter();
        #endregion
        #region Properties

        string suffixDefaultText { get { return Resources.Language.SELECTSUFFIX; } }
        public PersonAliasInfoPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this; ;
            }
        }

        public IPersonAliasInfoView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Boolean IsSuffixDropDownType
        {
            get
            {
                if (ViewState["IsSuffixDropDownType"] != null)
                    return (Boolean)(ViewState["IsSuffixDropDownType"]);
                return false;
            }
            set
            {
                ViewState["IsSuffixDropDownType"] = value;
            }
        }

        public Boolean HasDuplicateNames
        {
            get
            {
                List<String> nameList = new List<String>();
                if (chkShowHideAlias.Checked == true)
                {
                    if (!String.IsNullOrWhiteSpace(txtNewFirstName.Text))
                    {
                        if (!IsLocationServiceTenant)
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            nameList.Add(txtNewFirstName.Text.ToLower().Trim() + "#" + txtNewMiddleName.Text.ToLower().Trim() + "#" + txtNewLastName.Text.ToLower().Trim());
                        else
                            nameList.Add(txtNewFirstName.Text.ToLower().Trim() + "#" + txtNewMiddleName.Text.ToLower().Trim() + "#" + txtNewLastName.Text.ToLower().Trim() + "#"
                                + GetSuffixText());
                    }
                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                    if (!IsLocationServiceTenant)
                        nameList.AddRange(PersonAliasTempList.Select(x => x.FirstName.ToLower().Trim() + "#" + x.MiddleName.ToLower().Trim() + "#" + x.LastName.ToLower().Trim()).ToList());
                    else
                        nameList.AddRange(PersonAliasTempList.Select(x => x.FirstName.ToLower().Trim() + "#" + x.MiddleName.ToLower().Trim() + "#" + x.LastName.ToLower().Trim() + "#" + x.Suffix).ToList());
                }
                if (!IsLocationServiceTenant)
                {
                    nameList.Add(UserFirstName.ToLower().Trim() + "#" + UserMiddleName.ToLower().Trim() + "#" + UserLastName.ToLower().Trim());
                }
                else
                {
                    nameList.Add(UserFirstName.ToLower().Trim() + "#" + UserMiddleName.ToLower().Trim() + "#" + UserLastName.ToLower().Trim() + "#" + UserSuffix);
                }
                return !(nameList.Count() == nameList.Distinct().Count());

            }
        }

        public List<PersonAliasContract> PersonAliasList
        {
            get
            {
                if (IsEditModeOn)
                {
                    IsEditModeOn = false;
                    rptrAliasName.DataSource = PersonAliasTempList;
                    rptrAliasName.DataBind();
                }
                if (divErrorMessage.Visible)
                    divErrorMessage.Visible = false;
                if (PersonAliasTempList.IsNotNull())
                {
                    //  No point in adding anything if empty
                    if (!String.IsNullOrWhiteSpace(txtNewFirstName.Text) && chkShowHideAlias.Checked == true)
                    {
                        PersonAliasContract personAlias = new PersonAliasContract()
                        {
                            FirstName = txtNewFirstName.Text.Trim(),
                            LastName = txtNewLastName.Text.Trim(),
                            MiddleName = txtNewMiddleName.Text.Trim(),
                            Suffix = GetSuffixText()
                        };
                        if (!HasDuplicateNames)
                        {
                            // Add a new Alias name
                            PersonAliasTempList.Add(personAlias);

                            //rptrAliasName.DataSource = PersonAliasTempList;//UAT-605
                            rptrAliasName.DataSource = PersonAliasTempListMaxOcc;
                            rptrAliasName.DataBind();
                            SetCheckBoxAbeDisable();
                        }
                        txtNewFirstName.Text = String.Empty;
                        txtNewLastName.Text = string.Empty;
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        txtNewMiddleName.Text = String.Empty;
                        if (CurrentViewContext.IsLocationServiceTenant)
                        {
                            SetSuffixText("");
                            cmbAliasNewSuffix.DataSource = CurrentViewContext.lstAliasSuffixes;
                            cmbAliasNewSuffix.DataBind();
                        }

                        divErrorMessage.Visible = false;
                    }
                    if ((!MaxOccurance.IsNullOrEmpty() && MaxOccurance > AppConsts.NONE) && (PersonAliasTempList.Count() >= MaxOccurance))
                    {
                        divFooter.Visible = false;
                    }
                    else
                    {
                        if (MaxOccurance != AppConsts.NONE && !IsReadOnly)
                            divFooter.Visible = true;
                        else if (MaxOccurance == AppConsts.NONE)
                            divFooter.Visible = false;
                    }
                    return PersonAliasTempList;
                }
                return new List<PersonAliasContract>();
            }
            set
            {
                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                if (!value.IsNullOrEmpty())
                {
                    value.ForEach(x =>
                    {
                        if (x.MiddleName.IsNullOrEmpty())
                        {
                            x.MiddleName = NoMiddleNameText;
                        }
                    });
                }
                PersonAliasTempList = value;
            }
        }

        public List<PersonAliasContract> PersonAliasTempList
        {
            get
            {
                if (ViewState["PersonAliasList"] != null)
                {
                    return ViewState["PersonAliasList"] as List<PersonAliasContract>;
                }
                return null;
            }
            set
            {
                ViewState["PersonAliasList"] = value;
            }
        }

        public String UserFirstName
        {
            get
            {
                if (ViewState["UserFirstName"] != null)
                {
                    return Convert.ToString(ViewState["UserFirstName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserFirstName"] = value;
            }
        }

        public String UserLastName
        {
            get
            {
                if (ViewState["UserLastName"] != null)
                {
                    return Convert.ToString(ViewState["UserLastName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserLastName"] = value;
            }
        }
        //UAT-2212:
        public String UserMiddleName
        {
            get
            {
                if (ViewState["UserMiddleName"] != null)
                {
                    return Convert.ToString(ViewState["UserMiddleName"]);
                }
                return String.Empty;
            }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    value = NoMiddleNameText;
                }
                ViewState["UserMiddleName"] = value;
            }
        }

        public List<PersonAliasContract> PersonAliasTempListMaxOcc
        {
            get
            {
                List<PersonAliasContract> tempPersonAliasList = ViewState["PersonAliasList"] as List<PersonAliasContract>;
                if (IsEditProfile || (MaxOccurance == AppConsts.NONE) || MaxOccurance.IsNullOrEmpty())
                {

                    return tempPersonAliasList;
                }
                else
                {
                    return tempPersonAliasList.Take(MaxOccurance.Value).ToList();
                }
            }
        }

        public Boolean IsEditModeOn
        {
            get
            {
                if (ViewState["IsEditModeOn"] != null)
                {
                    return Convert.ToBoolean(ViewState["IsEditModeOn"]);
                }
                return false;
            }
            set
            {
                txtNewFirstName.Enabled = !value;
                txtNewLastName.Enabled = !value;
                GetSuffixControl().Enabled = !value;
                btnAddNewRecord.Enabled = !value;
                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                txtNewMiddleName.Enabled = !value;
                divMiddleNameCheckBox.Visible = !value;
                if (chkMiddleNameRequired.Checked)
                {
                    txtNewMiddleName.Enabled = false;
                }
                ViewState["IsEditModeOn"] = value;
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

        public Boolean IsReadOnly
        {
            get;
            set;
        }
        //if alias Name is to show in the label mark this property as true,else Disabled Textbox will show the Alias Name in Repeater 
        public Boolean IsLabelMode
        {
            get;
            set;
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

        public String NewFirstNameAlias
        {
            get
            {
                return txtNewFirstName.Text.Trim();
            }
        }

        public String NewLastNameAlias
        {
            get
            {
                return txtNewLastName.Text.Trim();
            }
        }

        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        public String NewMiddleNameAlias
        {
            get
            {
                return txtNewMiddleName.Text.Trim();
            }
        }

        public String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = String.Empty;
                if (!IsLocationServiceTenant)
                    noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }

        public Boolean IsUserRegistrationScreen
        {
            get
            {
                if (!(ViewState["IsUserRegistrationScreen"].IsNull()))
                {
                    return (Boolean)ViewState["IsUserRegistrationScreen"];
                }
                return false;
            }
            set
            {
                ViewState["IsUserRegistrationScreen"] = value;
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
        //UAT-3590
        public Boolean HasGreyBackground
        {
            get;
            set;
        }
        private Boolean? _IsLocationServiceTenant = null;
        public Boolean IsLocationServiceTenant
        {
            get
            {
                if (_IsLocationServiceTenant.IsNull())
                {
                    _IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(SelectedTenantId);

                }
                return _IsLocationServiceTenant ?? false;
            }


        }
        public Int32 SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        public List<Entity.lkpSuffix> lstAliasSuffixes
        {
            get
            {
                if (!ViewState["lstAliasSuffixes"].IsNullOrEmpty())
                    return (List<Entity.lkpSuffix>)ViewState["lstAliasSuffixes"];
                return new List<Entity.lkpSuffix>();
            }
            set
            {
                ViewState["lstAliasSuffixes"] = value;
            }
        }

        public String PageType
        {
            get
            {
                if (!ViewState["PageType"].IsNullOrEmpty())
                    return ViewState["PageType"].ToString();
                return String.Empty;
            }
            set
            {
                ViewState["PageType"] = value;
            }
        }

        public String UserSuffix
        {
            get
            {
                if (ViewState["UserSuffix"] != null)
                {
                    return Convert.ToString(ViewState["UserSuffix"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserSuffix"] = value;
            }
        }

        #endregion

        #region Events

        #region Page Event


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack || IsFalsePostBack)
                {
                    #region  Release 158 CBI
                    if (SelectedTenantId > 0)

                        if (CurrentViewContext.IsLocationServiceTenant)
                        {
                            MaxOccurance = AppConsts.FIVE;
                        }
                    #endregion

                    if (IsReadOnly)
                    {
                        lblChkShowHide.Visible = false;
                        chkShowHideAlias.Visible = false;
                        divFooter.Visible = false;
                    }
                    //UAT-3590
                    if (HasGreyBackground)
                    {
                        dvHasGreyBackground.Style["background-color"] = "#fff !Important";
                    }
                    divErrorMessage.Visible = false;

                    if (PersonAliasTempList.IsNull())
                        PersonAliasTempList = new List<PersonAliasContract>();
                    Presenter.GetSuffixes();
                    Presenter.IsDropDownSuffixType();
                    AddSuffixDropdownAndDesignChange();
                    //rptrAliasName.DataSource = PersonAliasTempList; //UAT-605
                    rptrAliasName.DataSource = PersonAliasTempListMaxOcc;
                    rptrAliasName.DataBind();
                    //hide the add new button for min/ max occurances in Order flow
                    // if (MaxOccurance > AppConsts.NONE && (PersonAliasTempList.Count() >= MaxOccurance))// commented code for UAT-605 and added below checks.
                    if ((!MaxOccurance.IsNullOrEmpty() && MaxOccurance > AppConsts.NONE) && (PersonAliasTempList.Count() >= MaxOccurance))
                    {
                        divFooter.Visible = false;
                    }
                    else
                    {
                        if (MaxOccurance != AppConsts.NONE && !IsReadOnly)
                            divFooter.Visible = true;
                        else if (MaxOccurance == AppConsts.NONE)
                            divFooter.Visible = false;
                    }
                    foreach (RepeaterItem ri in rptrAliasName.Items)
                    {
                        Label lblfirstName = ri.FindControl("lblfirstName") as Label;
                        WclTextBox txtFirstName1 = ri.FindControl("txtFirstName1") as WclTextBox;
                        Label lblLastName = ri.FindControl("lblLastName") as Label;
                        WclTextBox txtLastName1 = ri.FindControl("txtLastName1") as WclTextBox;
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        WclTextBox txtMiddleName1 = ri.FindControl("txtMiddleName1") as WclTextBox;
                        Label lblMiddleName = ri.FindControl("lblMiddleName") as Label;
                        Label lblSuffix = ri.FindControl("lblSuffix") as Label;
                        //WclComboBox cmbAliasSuffix1 = ri.FindControl("cmbAliasSuffix1") as WclComboBox;
                        WclTextBox txtAliasSuffix1 = ri.FindControl("txtAliasSuffix1") as WclTextBox;
                        Control dvAliasSuffix = ri.FindControl("dvAliasSuffix") as Control;
                        Label hdnSuffixId = ri.FindControl("hdnSuffixId") as Label;
                        if (IsLabelMode)
                        {
                            if (lblfirstName.IsNotNull())
                                lblfirstName.Visible = true;
                            if (txtFirstName1.IsNotNull())
                                txtFirstName1.Visible = false;
                            if (lblLastName.IsNotNull())
                            {
                                lblLastName.Visible = true;
                            }
                            if (CurrentViewContext.IsLocationServiceTenant)
                            {
                                if (lblSuffix.IsNotNull())
                                {
                                    lblSuffix.Visible = true;
                                }
                            }
                            if (txtLastName1.IsNotNull())
                                txtLastName1.Visible = false;
                            if (lblMiddleName.IsNotNull())
                                lblMiddleName.Visible = true;
                            if (txtMiddleName1.IsNotNull())
                                txtMiddleName1.Visible = false;
                            if (lblSuffix.IsNotNull())
                                lblSuffix.Visible = true;
                            //if (cmbAliasSuffix1.IsNotNull())
                            //    cmbAliasSuffix1.Visible = false;
                            if (txtAliasSuffix1.IsNotNull())
                                txtAliasSuffix1.Visible = false;
                        }
                        else
                        {
                            if (lblfirstName.IsNotNull())
                                lblfirstName.Visible = false;
                            if (txtFirstName1.IsNotNull())
                                txtFirstName1.Visible = true;
                            if (lblLastName.IsNotNull())
                            {
                                lblLastName.Visible = false;
                            }
                            if (CurrentViewContext.IsLocationServiceTenant)
                            {
                                if (PageType != PersonAliasPageType.OrderPaymentDetails.GetStringValue())
                                {
                                    if (lblSuffix.IsNotNull())
                                    {
                                        lblSuffix.Visible = false;
                                    }
                                }
                                else
                                {
                                    txtMiddleName1.Attributes.Add("PlaceHolder", "");
                                    txtMiddleName1.ToolTip = "";
                                }
                            }
                            if (txtLastName1.IsNotNull())
                                txtLastName1.Visible = true;
                            if (lblMiddleName.IsNotNull())
                                lblMiddleName.Visible = false;
                            if (txtMiddleName1.IsNotNull())
                                txtMiddleName1.Visible = true;
                            if (CurrentViewContext.IsLocationServiceTenant)
                            {
                                dvAliasSuffix.Visible = true;
                                if (PageType != PersonAliasPageType.OrderPaymentDetails.GetStringValue())
                                {
                                    //if (cmbAliasSuffix1.IsNotNull())
                                    //{
                                    //    cmbAliasSuffix1.DataSource = CurrentViewContext.lstAliasSuffixes;
                                    //    cmbAliasSuffix1.DataBind();
                                    //    cmbAliasSuffix1.SelectedValue = hdnSuffixId.Text;
                                    //    cmbAliasSuffix1.Visible = true;
                                    //}
                                    if (txtAliasSuffix1.IsNotNull())
                                    {
                                        txtAliasSuffix1.Text = hdnSuffixId.Text;
                                        txtAliasSuffix1.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }



                SetCheckBoxAbeDisable();
                SetNoMiddleNameCheckboxPadding(divMiddleNameCheckBox);
                ValidatePersonalInformation(IsLocationServiceTenant);
                if (IsLocationServiceTenant)
                {
                    revNewFirstName.Visible = true;
                    revNewMiddleName.Visible = true;
                    revNewLastName.Visible = true;
                    GetSuffixControl().Visible = true;
                    hdnIsPersonAliasLocationTenant.Value = "true";
                }
                chkShowHideAlias.Attributes["title"] = Resources.Language.CLKTOSHWHIDEALIASNAME;
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

        #region Button Events
        protected void OnAddRecord(object sender, EventArgs e)
        {
            try
            {
                if (PersonAliasTempList.IsNull())
                    PersonAliasTempList = new List<PersonAliasContract>();
                //  No point in adding anything if empty
                if (!String.IsNullOrWhiteSpace(txtNewFirstName.Text))
                {

                    PersonAliasContract personAlias = new PersonAliasContract()
                    {
                        FirstName = txtNewFirstName.Text.Trim(),
                        LastName = txtNewLastName.Text.Trim(),
                        MiddleName = txtNewMiddleName.Text.Trim(),
                        Suffix = GetSuffixText()
                    };

                    if ((!PersonAliasTempList.Any(cond => cond.FirstName.ToLower() == personAlias.FirstName.ToLower()
                                                && cond.LastName.ToLower() == personAlias.LastName.ToLower()
                                                && cond.MiddleName.ToLower().Trim() == personAlias.MiddleName.ToLower().Trim()
                                                && cond.Suffix.ToLower().Trim() == personAlias.Suffix.ToLower().Trim()))
                        && !(UserFirstName.ToLower().Equals(personAlias.FirstName.ToLower()) &&
                              UserLastName.ToLower().Equals(personAlias.LastName.ToLower()) &&
                              ((UserMiddleName ?? "").ToLower().Equals((personAlias.MiddleName ?? "").ToLower()))
                              && ((UserSuffix.IsNullOrWhiteSpace() && personAlias.Suffix.IsNullOrWhiteSpace())
                              || (UserSuffix ?? "").ToLower().Trim().Equals((personAlias.Suffix ?? "").ToLower().Trim()))
                              )
                        )
                    //String.Compare(UserMiddleName, personAlias.MiddleName, true) == AppConsts.NONE) && (personAlias.MiddleName.ToLower().Trim() != string.Empty || (personAlias.MiddleName.ToLower().Trim() == string.Empty && chkMiddleNameRequired.Checked)))
                    {
                        //Implementation for: Based on CBI's response below we need to allow for only one character per name (first, middle, last).
                        //The scenario we have to think about is the one where the applicant opts to provide no middle name with only one character for the first and last name.

                        if (!txtNewFirstName.IsNullOrEmpty() || !txtNewMiddleName.IsNullOrEmpty() || !txtNewLastName.IsNullOrEmpty())
                        {
                            if (CurrentViewContext.IsLocationServiceTenant)
                            {
                                Int32 totalLength = (txtNewFirstName.Text.Length) + (txtNewMiddleName.Text.Length) + (txtNewLastName.Text.Length)
                                    + GetSuffixText().Length;
                                if (totalLength < AppConsts.THREE)
                                {
                                    divErrorMessage.Visible = true;
                                    //lblErrorMsg.Text = "Total length for Alias Name should be atleast 3 characters.";
                                    lblErrorMsg.Text = Resources.Language.ALIASNAMETOTALLENGHTVALDTNMSG;
                                    rfvNewMiddleName.IsValid = true;
                                    return;
                                }
                            }
                        }

                        // Add a new Alias name
                        PersonAliasTempList.Add(personAlias);
                        txtNewFirstName.Text = String.Empty;
                        txtNewLastName.Text = String.Empty;
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        txtNewMiddleName.Text = String.Empty;
                        chkMiddleNameRequired.Checked = false;
                        rfvNewMiddleName.IsValid = true;

                        hdnNewSuffixId.Value = null;
                        SetSuffixText("");
                        divErrorMessage.Visible = false;
                        //rptrAliasName.DataSource = PersonAliasTempList; //UAT-605
                        rptrAliasName.DataSource = PersonAliasTempListMaxOcc;
                        rptrAliasName.DataBind();
                        //hide the add new button for min/ max occurances in Order flow
                        //if (MaxOccurance > AppConsts.NONE && (PersonAliasTempList.Count() == MaxOccurance))// commented code for UAT-605 and added below checks.
                        if ((!MaxOccurance.IsNullOrEmpty() && MaxOccurance > AppConsts.NONE) && (PersonAliasTempList.Count() == MaxOccurance))
                        {
                            divFooter.Visible = false;
                        }
                        else
                        {
                            if ((MaxOccurance != AppConsts.NONE && !IsReadOnly))
                                divFooter.Visible = true;
                            else if (MaxOccurance == AppConsts.NONE)
                                divFooter.Visible = false;
                        }
                        foreach (RepeaterItem ri in rptrAliasName.Items)
                        {
                            Label lblfirstName = ri.FindControl("lblfirstName") as Label;
                            WclTextBox txtFirstName1 = ri.FindControl("txtFirstName1") as WclTextBox;
                            Label lblLastName = ri.FindControl("lblLastName") as Label;
                            WclTextBox txtLastName1 = ri.FindControl("txtLastName1") as WclTextBox;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            WclTextBox txtMiddleName1 = ri.FindControl("txtMiddleName1") as WclTextBox;
                            Label lblMiddleName = ri.FindControl("lblMiddleName") as Label;
                            Label lblSuffix = ri.FindControl("lblSuffix") as Label;
                            Control dvAliasSuffix = ri.FindControl("dvAliasSuffix") as Control;
                            //WclComboBox cmbAliasSuffix1 = ri.FindControl("cmbAliasSuffix1") as WclComboBox;
                            WclTextBox txtAliasSuffix1 = ri.FindControl("txtAliasSuffix1") as WclTextBox;
                            Label hdnSuffixId = ri.FindControl("hdnSuffixId") as Label;
                            if (IsLabelMode)
                            {
                                if (lblfirstName.IsNotNull())
                                    lblfirstName.Visible = true;
                                if (txtFirstName1.IsNotNull())
                                    txtFirstName1.Visible = false;
                                if (lblLastName.IsNotNull())
                                {
                                    lblLastName.Visible = true;
                                }
                                if (CurrentViewContext.IsLocationServiceTenant)
                                {
                                    if (lblSuffix.IsNotNull())
                                    {
                                        lblSuffix.Visible = true;
                                    }
                                }

                                if (txtLastName1.IsNotNull())
                                    txtLastName1.Visible = false;
                                if (lblMiddleName.IsNotNull())
                                    lblMiddleName.Visible = true;
                                if (txtMiddleName1.IsNotNull())
                                    txtMiddleName1.Visible = false;
                                //if (cmbAliasSuffix1.IsNotNull())
                                //    cmbAliasSuffix1.Visible = false;

                                if (txtAliasSuffix1.IsNotNull())
                                    txtAliasSuffix1.Visible = false;
                            }
                            else
                            {
                                if (lblfirstName.IsNotNull())
                                    lblfirstName.Visible = false;
                                if (txtFirstName1.IsNotNull())
                                    txtFirstName1.Visible = true;
                                if (lblLastName.IsNotNull())
                                    lblLastName.Visible = false;
                                if (txtLastName1.IsNotNull())
                                    txtLastName1.Visible = true;
                                if (lblMiddleName.IsNotNull())
                                    lblMiddleName.Visible = false;
                                if (txtMiddleName1.IsNotNull())
                                    txtMiddleName1.Visible = true;
                                if (CurrentViewContext.IsLocationServiceTenant)
                                {
                                    dvAliasSuffix.Visible = true;
                                    //if (cmbAliasSuffix1.IsNotNull())
                                    //{
                                    //    cmbAliasSuffix1.DataSource = CurrentViewContext.lstAliasSuffixes;
                                    //    cmbAliasSuffix1.DataBind();
                                    //    cmbAliasSuffix1.SelectedValue = hdnSuffixId.Text;
                                    //    cmbAliasSuffix1.Visible = true;
                                    //}
                                    if (txtAliasSuffix1.IsNotNull())
                                    {
                                        txtAliasSuffix1.Text = hdnSuffixId.Text;
                                        txtAliasSuffix1.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (chkMiddleNameRequired.Checked && txtNewMiddleName.Text.Trim() == string.Empty)
                        {
                            divErrorMessage.Visible = true;
                            //lblErrorMsg.Text = "Duplicate names cannot be added.";
                            lblErrorMsg.Text = Resources.Language.DPLNAMECNTADD;
                            rfvNewMiddleName.IsValid = true;
                        }

                        else
                        {
                            if (txtNewMiddleName.Text.Trim() == string.Empty && chkMiddleNameRequired.Checked == false)
                            {
                                rfvNewMiddleName.IsValid = false;
                                rfvNewMiddleName.Visible = true;
                            }
                            else
                            {
                                divErrorMessage.Visible = true;
                                //lblErrorMsg.Text = "Duplicate names cannot be added.";
                                lblErrorMsg.Text = Resources.Language.DPLNAMECNTADD;
                            }
                        }
                    }
                }
                else
                {
                    divErrorMessage.Visible = true;
                    lblErrorMsg.Text = "Empty alias/maiden name cannot be added.";
                }
                IsEditModeOn = false;
                SetCheckBoxAbeDisable();
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

        #region Repeater Event
        protected void rptrAliasName_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                PersonAliasContract personAlias = null;

                if (e.CommandName == "delete")
                {
                    PersonAliasTempList.Remove(PersonAliasTempList[e.Item.ItemIndex]);
                    divErrorMessage.Visible = false;
                    IsEditModeOn = false;
                    //rptrAliasName.DataSource = PersonAliasTempList;//UAT-605
                    rptrAliasName.DataSource = PersonAliasTempListMaxOcc;
                    rptrAliasName.DataBind();
                    //hide or show the add new button for min/ max occurances in Order flow
                    //if (MaxOccurance > AppConsts.NONE && (PersonAliasTempList.Count() < MaxOccurance))//commented code for UAT-605 and added below checks.
                    if ((!MaxOccurance.IsNullOrEmpty() && MaxOccurance > AppConsts.NONE) && (PersonAliasTempList.Count() < MaxOccurance))
                    {
                        divFooter.Visible = true;
                    }
                }
                if (e.CommandName == "cancel")
                {
                    WclTextBox txtFirstName = e.Item.FindControl("txtFirstName") as WclTextBox;
                    if (txtFirstName.IsNotNull())
                        txtFirstName.Visible = false;
                    WclTextBox txtLastName = e.Item.FindControl("txtLastName") as WclTextBox;
                    if (txtLastName.IsNotNull())
                        txtLastName.Visible = false;
                    WebControl cntrlAliasSuffx = GetSuffixControl(e.Item);
                    if (!GetSuffixText(e.Item).IsNullOrEmpty())
                        cntrlAliasSuffx.Visible = false;

                    if (IsLabelMode)
                    {
                        Label lblfirstName = e.Item.FindControl("lblfirstName") as Label;
                        Label lblLastName = e.Item.FindControl("lblLastName") as Label;
                        if (lblfirstName.IsNotNull())
                            lblfirstName.Visible = true;
                        Label lblSuffix = e.Item.FindControl("lblSuffix") as Label;
                        if (lblLastName.IsNotNull())
                        {
                            lblLastName.Visible = true;
                        }
                        if (CurrentViewContext.IsLocationServiceTenant)
                        {
                            if (lblSuffix.IsNotNull())
                            {
                                lblSuffix.Visible = true;
                            }
                        }
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        Label lblMiddleName = e.Item.FindControl("lblMiddleName") as Label;
                        if (lblMiddleName.IsNotNull())
                            lblMiddleName.Visible = true;
                    }
                    else
                    {
                        WclTextBox txtFirstName1 = e.Item.FindControl("txtFirstName1") as WclTextBox;
                        WclTextBox txtLastName1 = e.Item.FindControl("txtLastName1") as WclTextBox;
                        if (txtFirstName1.IsNotNull())
                            txtFirstName1.Visible = true;
                        if (txtLastName1.IsNotNull())
                            txtLastName1.Visible = true;
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        WclTextBox txtMiddleName1 = e.Item.FindControl("txtMiddleName1") as WclTextBox;
                        if (txtMiddleName1.IsNotNull())
                            txtMiddleName1.Visible = true;
                        //WclComboBox cmbAliasSuffix1 = e.Item.FindControl("cmbAliasSuffix1") as WclComboBox;
                        WclTextBox txtAliasSuffix1 = e.Item.FindControl("txtAliasSuffix1") as WclTextBox;
                        Control dvAliasSuffix = e.Item.FindControl("dvAliasSuffix") as Control;
                        Label hdnSuffixId = e.Item.FindControl("hdnSuffixId") as Label;
                        if (CurrentViewContext.IsLocationServiceTenant)
                        {
                            dvAliasSuffix.Visible = true;
                            //if (cmbAliasSuffix1.SelectedValue.IsNotNull())
                            //{
                            //    cmbAliasSuffix1.SelectedValue = hdnSuffixId.Text;
                            //    cmbAliasSuffix1.Visible = true;
                            //}

                            if (txtAliasSuffix1.Text.IsNotNull())
                            {
                                txtAliasSuffix1.Text = hdnSuffixId.Text;
                                txtAliasSuffix1.Visible = true;
                            }

                        }

                    }

                    LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                    if (btnEdit.IsNotNull())
                    {
                        btnEdit.Text = Resources.Language.EDIT;
                        btnEdit.CommandName = "edit";
                    }
                    LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                    if (btnDelete.IsNotNull())
                    {
                        btnDelete.Text = Resources.Language.DLT;
                        btnDelete.CommandName = "delete";
                        //btnDelete.OnClientClick = "return confirm('Are you sure you want to delete the Alias/Maiden ?')";
                        string _confirmDelAliasText = Resources.Language.CONFMALIASDEL;
                        btnDelete.OnClientClick = "return confirm('" + _confirmDelAliasText + "')";
                    }
                    divErrorMessage.Visible = false;
                    IsEditModeOn = false;
                    //rptrAliasName.DataSource = PersonAliasTempList;UAT-605
                    rptrAliasName.DataSource = PersonAliasTempListMaxOcc;
                    rptrAliasName.DataBind();
                }
                else if (e.CommandName == "edit")
                {
                    if (IsEditModeOn)
                    {
                        divErrorMessage.Visible = true;
                        //lblErrorMsg.Text = "Only one alias/maiden can be updated at a time.";
                        lblErrorMsg.Text = Resources.Language.ONLYONEALIASCANBEUPDATED;
                    }
                    else
                    {
                        WclTextBox txtFirstName = e.Item.FindControl("txtFirstName") as WclTextBox;
                        if (txtFirstName.IsNotNull())
                            txtFirstName.Visible = true;
                        WclTextBox txtLastName = e.Item.FindControl("txtLastName") as WclTextBox;
                        if (txtLastName.IsNotNull())
                            txtLastName.Visible = true;
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        WclTextBox txtMiddleName = e.Item.FindControl("txtMiddleName") as WclTextBox;
                        Control divMiddleNameCheckBoxRepeater = e.Item.FindControl("divMiddleNameCheckBoxRepeater") as Control;
                        if (txtMiddleName.IsNotNull())
                            txtMiddleName.Visible = true;
                        String middleName = txtMiddleName.Text;
                        WebControl cntrlAliasSuffix = GetSuffixControl(e.Item);
                        HtmlGenericControl dvAliasSuffix = e.Item.FindControl("dvAliasSuffix") as HtmlGenericControl;
                        if (CurrentViewContext.IsLocationServiceTenant)
                        {
                            dvAliasSuffix.Visible = true;
                            //if (cmbAliasSuffix.SelectedValue.IsNotNull())
                            //    cmbAliasSuffix.Visible = true;
                            //if (!GetSuffixText(e.Item).IsNullOrEmpty())
                                if(!cntrlAliasSuffix.IsNullOrEmpty())
                                // {
                                cntrlAliasSuffix.Visible = true;
                            //}
                        }

                        if (IsLabelMode)
                        {
                            Label lblfirstName = e.Item.FindControl("lblfirstName") as Label;
                            Label lblLastName = e.Item.FindControl("lblLastName") as Label;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            Label lblMiddleName = e.Item.FindControl("lblMiddleName") as Label;
                            Label lblSuffix = e.Item.FindControl("lblSuffix") as Label;
                            if (lblfirstName.IsNotNull())
                                lblfirstName.Visible = false;
                            if (lblLastName.IsNotNull())
                                lblLastName.Visible = false;
                            if (lblMiddleName.IsNotNull())
                                lblMiddleName.Visible = false;
                            if (lblSuffix.IsNotNull())
                                lblSuffix.Visible = false;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            //middleName = String.Empty;
                            divMiddleNameCheckBoxRepeater.Visible = false;
                        }
                        else
                        {
                            WclTextBox txtFirstName1 = e.Item.FindControl("txtFirstName1") as WclTextBox;
                            WclTextBox txtLastName1 = e.Item.FindControl("txtLastName1") as WclTextBox;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            WclTextBox txtMiddleName1 = e.Item.FindControl("txtMiddleName1") as WclTextBox;
                            //WclComboBox cmbAliasSuffix1 = e.Item.FindControl("cmbAliasSuffix1") as WclComboBox;
                            WclTextBox txtAliasSuffix1 = e.Item.FindControl("txtAliasSuffix1") as WclTextBox;
                            //Control dvAliasSuffix = e.Item.FindControl("dvAliasSuffix") as Control;
                            if (txtFirstName1.IsNotNull())
                                txtFirstName1.Visible = true;
                            if (txtLastName1.IsNotNull())
                                txtLastName1.Visible = true;
                            if (txtMiddleName1.IsNotNull())
                                txtMiddleName1.Visible = true;

                            if (CurrentViewContext.IsLocationServiceTenant)
                            {
                                dvAliasSuffix.Visible = true;
                                //if (cmbAliasSuffix1.SelectedValue.IsNotNull())
                                //{
                                //    cmbAliasSuffix1.Visible = true;
                                //}
                                if (txtAliasSuffix1.Text.IsNotNull())
                                {
                                    txtAliasSuffix1.Visible = true;
                                }

                            }
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            //middleName = String.Empty;
                            divMiddleNameCheckBoxRepeater.Visible = false;
                        }
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        WclCheckBox chkMiddleNameRequiredRepeater = e.Item.FindControl("chkMiddleNameRequiredRepeater") as WclCheckBox;

                        RequiredFieldValidator rfvMiddleName = e.Item.FindControl("rfvMiddleName") as RequiredFieldValidator;
                        if (!middleName.IsNullOrEmpty())
                        {
                            EnableDisableMiddleNameCheckbox(middleName, rfvMiddleName, chkMiddleNameRequiredRepeater, divMiddleNameCheckBoxRepeater, txtMiddleName);
                        }
                        else
                        {
                            if (IsLocationServiceTenant)
                            {
                                if (!chkMiddleNameRequiredRepeater.Checked)
                                {
                                    rfvMiddleName.Enabled = false;
                                    divMiddleNameCheckBoxRepeater.Visible = true;
                                    chkMiddleNameRequiredRepeater.Checked = true;
                                    txtMiddleName.Enabled = false;
                                    txtMiddleName.Attributes.Add("PlaceHolder", "");
                                    txtMiddleName.ToolTip = "";
                                }
                                else
                                {
                                    divMiddleNameCheckBoxRepeater.Visible = true;
                                    chkMiddleNameRequiredRepeater.Enabled = true;
                                }
                            }
                        }

                        //AddSuffixDropdownAndDesignChange();
                        LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                        if (btnEdit.IsNotNull())
                        {
                            btnEdit.Text = Resources.Language.SAVE;
                            btnEdit.CommandName = "save";
                        }
                        LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                        if (btnDelete.IsNotNull())
                        {
                            btnDelete.Text = Resources.Language.CNCL;
                            btnDelete.CommandName = "cancel";
                            btnDelete.OnClientClick = "";
                        }
                        divErrorMessage.Visible = false;
                        IsEditModeOn = true;

                        if (IsLocationServiceTenant)
                        {
                            //HtmlGenericControl dvAliasSuffix = e.Item.FindControl("dvAliasSuffix") as HtmlGenericControl;
                            HtmlGenericControl dvSpanAliasFN = e.Item.FindControl("dvSpanAliasFN") as HtmlGenericControl;
                            HtmlGenericControl dvAliasFirstName = e.Item.FindControl("dvAliasFirstName") as HtmlGenericControl;
                            HtmlGenericControl dvSpanAliasMN = e.Item.FindControl("dvSpanAliasMN") as HtmlGenericControl;
                            HtmlGenericControl dvAliasMiddleName = e.Item.FindControl("dvAliasMiddleName") as HtmlGenericControl;
                            HtmlGenericControl dvSpanAliasLN = e.Item.FindControl("dvSpanAliasLN") as HtmlGenericControl;
                            HtmlGenericControl dvAliasLastName = e.Item.FindControl("dvAliasLastName") as HtmlGenericControl;
                            HtmlGenericControl divButtons = e.Item.FindControl("divButtons") as HtmlGenericControl;
                            if (PageType == PersonAliasPageType.ITSUserRegistration.GetStringValue() || PageType == PersonAliasPageType.EditProfile.GetStringValue() || PageType == PersonAliasPageType.ApplicantProfile.GetStringValue())
                            {
                                dvSpanAliasFN.Style.Add("Width", "10%");
                                dvSpanAliasMN.Style.Add("Width", "10%");
                                dvSpanAliasLN.Style.Add("Width", "10%");
                                dvAliasFirstName.Style.Add("Width", "15%");
                                dvAliasMiddleName.Style.Add("Width", "15%");
                                dvAliasLastName.Style.Add("Width", "15%");

                                dvAliasSuffix.Style.Add("display", "inline-block");
                                dvAliasSuffix.Style.Add("Width", "14%");
                                dvAliasSuffix.Style.Add("margin-left", "10px");

                                divButtons.Style.Add("margin-bottom", "10px");
                            }
                        }
                    }
                }
                else if (e.CommandName == "save")
                {
                    personAlias = PersonAliasTempList[e.Item.ItemIndex];
                    WclTextBox txtFirstName = e.Item.FindControl("txtFirstName") as WclTextBox;
                    WclTextBox txtLastName = e.Item.FindControl("txtLastName") as WclTextBox;
                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                    WclTextBox txtMiddleName = e.Item.FindControl("txtMiddleName") as WclTextBox;
                    Control divMiddleNameCheckBoxRepeater = e.Item.FindControl("divMiddleNameCheckBoxRepeater") as Control;
                    WclCheckBox chkMiddleNameRequiredRepeater = e.Item.FindControl("chkMiddleNameRequiredRepeater") as WclCheckBox;
                    Label lblMiddleName = e.Item.FindControl("lblMiddleName") as Label;
                    WclTextBox txtMiddleName1 = e.Item.FindControl("txtMiddleName1") as WclTextBox;
                    WebControl cntrlAliasSuffix = GetSuffixControl(e.Item);
                    Label lblSuffix = e.Item.FindControl("lblSuffix") as Label;
                    //var s = cmbAliasSuffix.SelectedValue;

                    String suffix = GetSuffixText(e.Item);
                    Label hdnSuffixId = e.Item.FindControl("hdnSuffixId") as Label;
                    if (txtFirstName.IsNotNull() && txtLastName.IsNotNull())
                    {
                        if (UserFirstName.ToLower().Equals(txtFirstName.Text.ToLower()) && UserLastName.ToLower().Equals(txtLastName.Text.ToLower())
                            && String.Compare(UserMiddleName, txtMiddleName.Text, true) == AppConsts.NONE
                            && (UserSuffix ?? "").ToLower().Equals(suffix == suffixDefaultText ? "" : suffix))
                        {
                            IsEditModeOn = true;
                            divErrorMessage.Visible = true;
                            //lblErrorMsg.Text = "Duplicate names cannot be added.";
                            lblErrorMsg.Text = Resources.Language.DPLNAMECNTADD;
                            rfvNewMiddleName.IsValid = true;
                            return;
                        }


                        //Implementation for: Based on CBI's response below we need to allow for only one character per name (first, middle, last).
                        //The scenario we have to think about is the one where the applicant opts to provide no middle name with only one character for the first and last name.

                        if (!txtFirstName.IsNullOrEmpty() || !txtMiddleName.IsNullOrEmpty() || !txtLastName.IsNullOrEmpty())
                        {
                            if (CurrentViewContext.IsLocationServiceTenant)
                            {
                                Int32 totalLength = (txtFirstName.Text.Length) + (txtMiddleName.Text.Length) + (txtLastName.Text.Length) + (suffix.Length);
                                if (totalLength < AppConsts.THREE)
                                {
                                    IsEditModeOn = true;
                                    divErrorMessage.Visible = true;
                                    //lblErrorMsg.Text = "Total length for Alias Name should be atleast 3 characters.";
                                    lblErrorMsg.Text = Resources.Language.ALIASNAMETOTALLENGHTVALDTNMSG;
                                    rfvNewMiddleName.IsValid = true;
                                    return;
                                }
                            }
                        }

                        if (
                           (personAlias.LastName.IsNullOrEmpty())
                           ||
                           !(personAlias.FirstName.ToLower() == txtFirstName.Text.ToLower().Trim() && personAlias.LastName.ToLower() == txtLastName.Text.ToLower().Trim()
                             && personAlias.MiddleName != null && personAlias.MiddleName.ToLower() == txtMiddleName.Text.ToLower().Trim() &&
                             //((personAlias.SuffixID ?? 0) == (txtAliasSuffix.Text.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(txtAliasSuffix.Text)))
                             personAlias.Suffix.ToLower().Trim() == suffix.ToLower().Trim()
                             )
                           )
                        {
                            if (!PersonAliasTempList.Any(cond => cond.FirstName.ToLower() == txtFirstName.Text.ToLower().Trim()
                                && cond.LastName.ToLower().Trim() == txtLastName.Text.ToLower().Trim()
                                && (cond.MiddleName.ToLower().Trim() == txtMiddleName.Text.ToLower().Trim())
                                && (cond.Suffix.ToLower().Trim() == (IsLocationServiceTenant == true && !suffix.Trim().ToLower().IsNullOrEmpty() ? suffix.ToLower().Trim() : String.Empty))))
                            {
                                personAlias.FirstName = txtFirstName.Text.Trim();
                                personAlias.LastName = txtLastName.Text.Trim();
                                txtFirstName.Visible = false;
                                txtLastName.Visible = false;
                                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                personAlias.MiddleName = txtMiddleName.Text.Trim();
                                txtMiddleName.Visible = false;
                                if (IsLocationServiceTenant)
                                {
                                    //personAlias.SuffixID = !txtAliasSuffix.IsNullOrEmpty() ? Convert.ToInt32(txtAliasSuffix.Text) : (Int32?)null;
                                    personAlias.Suffix = suffix;
                                }
                                if (cntrlAliasSuffix.IsNotNull())
                                {
                                    cntrlAliasSuffix.Visible = false;
                                }
                                if (IsLabelMode)
                                {
                                    Label lblfirstName = e.Item.FindControl("lblfirstName") as Label;
                                    if (lblfirstName.IsNotNull())
                                        lblfirstName.Visible = true;
                                    Label lblLastName = e.Item.FindControl("lblLastName") as Label;
                                    if (lblLastName.IsNotNull())
                                    {
                                        lblLastName.Visible = true;
                                    }
                                    Label lblSuffixs = e.Item.FindControl("lblSuffix") as Label;
                                    if (CurrentViewContext.IsLocationServiceTenant)
                                    {
                                        if (lblSuffixs.IsNotNull())
                                        {
                                            lblSuffixs.Visible = true;
                                        }
                                    }
                                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                    if (lblMiddleName.IsNotNull())
                                        lblMiddleName.Visible = true;
                                }
                                else
                                {
                                    WclTextBox txtFirstName1 = e.Item.FindControl("txtFirstName1") as WclTextBox;
                                    if (txtFirstName1.IsNotNull())
                                        txtFirstName1.Visible = true;
                                    WclTextBox txtLastName1 = e.Item.FindControl("txtLastName1") as WclTextBox;
                                    if (txtLastName1.IsNotNull())
                                        txtLastName1.Visible = true;
                                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                    if (txtMiddleName1.IsNotNull())
                                        txtMiddleName1.Visible = true;
                                    //WclComboBox cmbAliasSuffix1 = e.Item.FindControl("cmbAliasSuffix1") as WclComboBox;
                                    WclTextBox txtAliasSuffix1 = e.Item.FindControl("txtAliasSuffix1") as WclTextBox;
                                    Control dvAliasSuffix = e.Item.FindControl("dvAliasSuffix") as Control;
                                    if (CurrentViewContext.IsLocationServiceTenant)
                                    {
                                        dvAliasSuffix.Visible = true;
                                        //if (cmbAliasSuffix1.SelectedValue.IsNotNull())
                                        //    cmbAliasSuffix1.Visible = true;
                                        if (txtAliasSuffix1.Text.IsNotNull())
                                        {
                                            txtAliasSuffix1.Visible = true;
                                        }
                                    }
                                }

                                LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                                if (btnEdit.IsNotNull())
                                {
                                    btnEdit.Text = Resources.Language.EDIT;
                                    btnEdit.CommandName = "edit";
                                }
                                LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                                if (btnDelete.IsNotNull())
                                {
                                    btnDelete.Text = Resources.Language.DLT;
                                    btnDelete.CommandName = "delete";
                                    //btnDelete.OnClientClick = "return confirm('Are you sure you want to delete the Alias/Maiden ?')";
                                    string _confirmDelAliasText = Resources.Language.CONFMALIASDEL;
                                    btnDelete.OnClientClick = "return confirm('" + _confirmDelAliasText + "')";
                                }
                                divErrorMessage.Visible = false;
                                IsEditModeOn = false;
                                // rptrAliasName.DataSource = PersonAliasTempList;UAT-605
                                rptrAliasName.DataSource = PersonAliasTempListMaxOcc;
                                rptrAliasName.DataBind();
                                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                divMiddleNameCheckBoxRepeater.Visible = false;
                            }
                            else
                            {
                                IsEditModeOn = true;
                                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                if (chkMiddleNameRequiredRepeater.Checked && txtMiddleName1.Text.Trim() == string.Empty)
                                {
                                    divErrorMessage.Visible = true;
                                    //lblErrorMsg.Text = "Duplicate names cannot be added.";
                                    lblErrorMsg.Text = Resources.Language.DPLNAMECNTADD;
                                    rfvNewMiddleName.IsValid = true;

                                }

                                else
                                {
                                    if (txtMiddleName1.Text.Trim() == string.Empty && chkMiddleNameRequiredRepeater.Checked == false)
                                    {
                                        rfvNewMiddleName.IsValid = false;
                                        rfvNewMiddleName.Visible = true;
                                    }
                                    else
                                    {
                                        rfvNewMiddleName.IsValid = true;
                                        txtMiddleName1.CausesValidation = false;
                                        divErrorMessage.Visible = true;
                                        //lblErrorMsg.Text = "Duplicate names cannot be added.";
                                        lblErrorMsg.Text = Resources.Language.DPLNAMECNTADD;
                                    }
                                }
                                //divMiddleNameCheckBoxRepeater.Visible = true;
                                //divErrorMessage.Visible = true;
                                //lblErrorMsg.Text = "Duplicate names cannot be added.";
                            }
                        }
                        else
                        {
                            txtFirstName.Visible = false;
                            txtLastName.Visible = false;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            txtMiddleName.Visible = false;
                            if (cntrlAliasSuffix.IsNotNull())
                                cntrlAliasSuffix.Visible = false;
                            if (IsLabelMode)
                            {
                                Label lblfirstName = e.Item.FindControl("lblfirstName") as Label;
                                if (lblfirstName.IsNotNull())
                                    lblfirstName.Visible = true;
                                Label lblLastName = e.Item.FindControl("lblLastName") as Label;
                                if (lblLastName.IsNotNull())
                                {
                                    lblLastName.Visible = true;
                                }
                                if (CurrentViewContext.IsLocationServiceTenant)
                                {
                                    if (lblSuffix.IsNotNull())
                                    {
                                        lblSuffix.Visible = true;
                                    }
                                }
                                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                if (lblMiddleName.IsNotNull())
                                    lblMiddleName.Visible = true;
                            }
                            else
                            {
                                WclTextBox txtFirstName1 = e.Item.FindControl("txtFirstName1") as WclTextBox;
                                if (txtFirstName1.IsNotNull())
                                    txtFirstName1.Visible = true;
                                WclTextBox txtLastName1 = e.Item.FindControl("txtLastName1") as WclTextBox;
                                if (txtLastName1.IsNotNull())
                                    txtLastName1.Visible = true;
                                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                if (txtMiddleName1.IsNotNull())
                                    txtMiddleName1.Visible = true;
                                //WclComboBox cmbAliasSuffix1 = e.Item.FindControl("cmbAliasSuffix1") as WclComboBox;
                                WclTextBox txtAliasSuffix1 = e.Item.FindControl("txtAliasSuffix1") as WclTextBox;
                                Control dvAliasSuffix = e.Item.FindControl("dvAliasSuffix") as Control;
                                if (CurrentViewContext.IsLocationServiceTenant)
                                {
                                    dvAliasSuffix.Visible = true;
                                    //if (cmbAliasSuffix1.SelectedValue.IsNotNull())
                                    //    cmbAliasSuffix1.Visible = true;
                                    if (txtAliasSuffix1.Text.IsNotNull())
                                    {
                                        txtAliasSuffix1.Visible = true;
                                    }
                                }

                            }
                            LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                            if (btnEdit.IsNotNull())
                            {
                                btnEdit.Text = Resources.Language.EDIT;
                                btnEdit.CommandName = "edit";
                            }
                            LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                            if (btnDelete.IsNotNull())
                            {
                                btnDelete.Text = Resources.Language.DLT;
                                btnDelete.CommandName = "delete";
                                //btnDelete.OnClientClick = "return confirm('Are you sure you want to delete the Alias/Maiden ?')";
                                string _confirmDelAliasText = Resources.Language.CONFMALIASDEL;
                                btnDelete.OnClientClick = "return confirm('" + _confirmDelAliasText + "')";
                            }
                            divErrorMessage.Visible = false;
                            IsEditModeOn = false;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            divMiddleNameCheckBoxRepeater.Visible = false;

                            if (IsLocationServiceTenant)
                            {
                                HtmlGenericControl dvAliasSuffix = e.Item.FindControl("dvAliasSuffix") as HtmlGenericControl;
                                HtmlGenericControl dvSpanAliasFN = e.Item.FindControl("dvSpanAliasFN") as HtmlGenericControl;
                                HtmlGenericControl dvAliasFirstName = e.Item.FindControl("dvAliasFirstName") as HtmlGenericControl;
                                HtmlGenericControl dvSpanAliasMN = e.Item.FindControl("dvSpanAliasMN") as HtmlGenericControl;
                                HtmlGenericControl dvAliasMiddleName = e.Item.FindControl("dvAliasMiddleName") as HtmlGenericControl;
                                HtmlGenericControl dvSpanAliasLN = e.Item.FindControl("dvSpanAliasLN") as HtmlGenericControl;
                                HtmlGenericControl dvAliasLastName = e.Item.FindControl("dvAliasLastName") as HtmlGenericControl;
                                HtmlGenericControl divButtons = e.Item.FindControl("divButtons") as HtmlGenericControl;

                                if (PageType == PersonAliasPageType.ITSUserRegistration.GetStringValue() || PageType == PersonAliasPageType.EditProfile.GetStringValue() || PageType == PersonAliasPageType.ApplicantProfile.GetStringValue())
                                {
                                    dvSpanAliasFN.Style.Add("Width", "11%");
                                    dvSpanAliasMN.Style.Add("Width", "11%");
                                    dvSpanAliasLN.Style.Add("Width", "11%");
                                    dvAliasFirstName.Style.Add("Width", "15%");
                                    dvAliasMiddleName.Style.Add("Width", "15%");
                                    dvAliasLastName.Style.Add("Width", "15%");

                                    dvAliasSuffix.Style.Add("display", "inline-block");
                                    dvAliasSuffix.Style.Add("Width", "10%");
                                    dvAliasSuffix.Style.Add("margin-left", "10px");

                                    divButtons.Style.Add("margin-bottom", "10px");
                                }
                            }
                        }
                    }
                }
                SetCheckBoxAbeDisable();
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

        protected void rptrAliasName_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (IsReadOnly)
                {
                    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                    {
                        Control divsbutton = e.Item.FindControl("divButtons") as Control;
                        if (divsbutton.IsNotNull())
                            divsbutton.Visible = false;
                    }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                    SetMiddleName(e.Item);
                    HtmlGenericControl divMiddleNameCheckBoxRepeater = e.Item.FindControl("divMiddleNameCheckBoxRepeater") as HtmlGenericControl;
                    SetNoMiddleNameCheckboxPadding(divMiddleNameCheckBoxRepeater);

                    HtmlGenericControl dvAliasSuffix = e.Item.FindControl("dvAliasSuffix") as HtmlGenericControl;
                    HtmlGenericControl dvSpanAliasFN = e.Item.FindControl("dvSpanAliasFN") as HtmlGenericControl;
                    HtmlGenericControl dvAliasFirstName = e.Item.FindControl("dvAliasFirstName") as HtmlGenericControl;
                    HtmlGenericControl dvSpanAliasMN = e.Item.FindControl("dvSpanAliasMN") as HtmlGenericControl;
                    HtmlGenericControl dvAliasMiddleName = e.Item.FindControl("dvAliasMiddleName") as HtmlGenericControl;
                    HtmlGenericControl dvSpanAliasLN = e.Item.FindControl("dvSpanAliasLN") as HtmlGenericControl;
                    HtmlGenericControl dvAliasLastName = e.Item.FindControl("dvAliasLastName") as HtmlGenericControl;
                    HtmlGenericControl divButtons = e.Item.FindControl("divButtons") as HtmlGenericControl;

                    if (IsLocationServiceTenant)
                    {
                        RegularExpressionValidator regFirstName = e.Item.FindControl("revFirstName") as RegularExpressionValidator;
                        RegularExpressionValidator regMiddleName = e.Item.FindControl("revMiddleName") as RegularExpressionValidator;
                        RegularExpressionValidator regLastName = e.Item.FindControl("revLastName") as RegularExpressionValidator;
                        //RegularExpressionValidator RegSuffix = e.Item.FindControl("RegularExpressionValidator1") as RegularExpressionValidator;

                        regFirstName.Visible = true;
                        regLastName.Visible = true;
                        regMiddleName.Visible = true;
                        // RegSuffix.Visible = true;


                    }

                    if (CurrentViewContext.IsLocationServiceTenant)
                    {
                        BindSuffix(e.Item);
                        //doing styling//
                        if (PageType == PersonAliasPageType.ITSUserRegistration.GetStringValue() || PageType == PersonAliasPageType.EditProfile.GetStringValue() || PageType == PersonAliasPageType.ApplicantProfile.GetStringValue())
                        {
                            dvSpanAliasFN.Style.Add("Width", "11%");
                            dvSpanAliasMN.Style.Add("Width", "11%");
                            dvSpanAliasLN.Style.Add("Width", "11%");

                            dvAliasFirstName.Style.Add("Width", "15%");
                            dvAliasMiddleName.Style.Add("Width", "15%");
                            dvAliasLastName.Style.Add("Width", "15%");

                            dvAliasSuffix.Style.Add("display", "inline-block");
                            dvAliasSuffix.Style.Add("Width", "10%");
                            dvAliasSuffix.Style.Add("margin-left", "10px");

                            divButtons.Style.Add("margin-bottom", "10px");
                            // dvAliasSuffix.Style.Add("margin-left", "-3%");
                        }

                        if (PageType == PersonAliasPageType.OrderReview.GetStringValue() || PageType == PersonAliasPageType.OrderPaymentDetails.GetStringValue())
                        {
                            dvSpanAliasFN.Style.Add("Width", "15%");
                            dvSpanAliasMN.Style.Add("Width", "15%");
                            dvSpanAliasLN.Style.Add("Width", "15%");

                            dvAliasFirstName.Style.Add("Width", "13%");
                            dvAliasFirstName.Style.Add("margin-right", "5.3%");
                            dvAliasMiddleName.Style.Add("Width", "13%");
                            dvAliasMiddleName.Style.Add("margin-right", "5.3%");
                            dvAliasLastName.Style.Add("Width", "8%");
                            // dvAliasLastName.Style.Add("margin-right", "82px");

                            dvAliasSuffix.Style.Add("display", "inline-block");
                            dvAliasSuffix.Style.Add("Width", "5%");
                            dvAliasSuffix.Style.Add("margin-left", "0px");
                            if (PageType == PersonAliasPageType.OrderPaymentDetails.GetStringValue())
                            {
                                dvAliasSuffix.Style.Add("margin-top", "8px");
                            }
                        }
                    }
                    else
                    {
                        dvAliasSuffix.Style.Add("display", "none");
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

        #endregion

        #region Methods
        #region Private Methods
        private void SetCheckBoxAbeDisable()
        {
            if (rptrAliasName.Items.Count > 0)
            {
                chkShowHideAlias.Disabled = true;
                chkShowHideAlias.Checked = true;
                divPersonalAlias.Style["display"] = "block";
            }
            else
            {
                if (chkShowHideAlias.Checked)
                    divPersonalAlias.Style["display"] = "block";
                else
                    divPersonalAlias.Style["display"] = "none";
                chkShowHideAlias.Disabled = false;
            }
        }

        private void ValidatePersonalInformation(Boolean IsLocationTenant)
        {
            //if (!Presenter.IsDropDownSuffixType())
            //{
            if (IsLocationTenant)
            {
                //revSuffix.ValidationExpression = "^[a-z A-Z]*-?[a-z A-Z]*$";
                //revSuffix.ErrorMessage = "Only alphabets or single hyphen(-) is allowed in the suffix up to maximum of 10 characters.";
                //revSuffix.ErrorMessage = Resources.Language.SUFFIXNAMEVALDT;
            }
            else
            {
                //revSuffix.ValidationExpression = "^[\\w\\d\\s\\-\\.\\']{1,10}$";
            }
            // }

        }

        #region UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality

        private void SetMiddleName(RepeaterItem item)
        {
            Label lblMiddleName = item.FindControl("lblMiddleName") as Label;
            WclTextBox txtMiddleName1 = item.FindControl("txtMiddleName1") as WclTextBox;
            WclTextBox txtMiddleName = item.FindControl("txtMiddleName") as WclTextBox;
            RequiredFieldValidator rfvMiddleName = item.FindControl("rfvMiddleName") as RequiredFieldValidator;
            HtmlGenericControl divMiddleNameCheckBoxRepeater = item.FindControl("divMiddleNameCheckBoxRepeater") as HtmlGenericControl;
            CheckBox chkMiddleNameRequired = item.FindControl("chkMiddleNameRequiredRepeater") as CheckBox;

            PersonAliasContract personAliasContract = (PersonAliasContract)item.DataItem;
            divMiddleNameCheckBoxRepeater.Visible = false;
            if (personAliasContract.MiddleName.IsNullOrEmpty())
            {
                if (IsLocationServiceTenant)
                {
                    if (!chkMiddleNameRequired.Checked)
                    {
                        rfvMiddleName.Enabled = false;
                    }
                }
                txtMiddleName.Text = NoMiddleNameText;
                txtMiddleName1.Text = NoMiddleNameText;
                lblMiddleName.Text = NoMiddleNameText.HtmlEncode();
            }
            else
            {
                if (IsLocationServiceTenant)
                {
                    if (chkMiddleNameRequired.Checked)
                    {
                        rfvMiddleName.Enabled = true;
                    }
                }

                txtMiddleName.Text = personAliasContract.MiddleName;
                txtMiddleName1.Text = personAliasContract.MiddleName;
                lblMiddleName.Text = personAliasContract.MiddleName.HtmlEncode();
            }
        }

        private void BindSuffix(RepeaterItem item)
        {
            Label lblSuffix = item.FindControl("lblSuffix") as Label;
            WclTextBox txtAliasSuffix1 = item.FindControl("txtAliasSuffix1") as WclTextBox;
            PersonAliasContract personAliasContract = (PersonAliasContract)item.DataItem;
            String suffix = personAliasContract.Suffix.IsNullOrEmpty()
                            ? String.Empty : personAliasContract.Suffix;
            lblSuffix.Text = suffix.HtmlEncode();
            txtAliasSuffix1.Text = suffix;
            SetSuffixText(suffix, item);
        }

        private void EnableDisableMiddleNameCheckbox(String middleName, RequiredFieldValidator rfvMiddleNameRepeater, WclCheckBox chkMiddleNameRequiredRepeater
                                                     , Control divMiddleNameCheckBoxRepeater, WclTextBox txtMiddleName)
        {
            if (String.Compare(middleName, NoMiddleNameText, true) == AppConsts.NONE)
            {
                divMiddleNameCheckBoxRepeater.Visible = true;
                chkMiddleNameRequiredRepeater.Checked = true;
                txtMiddleName.Enabled = false;
                rfvMiddleNameRepeater.Enabled = false;
            }
            else
            {
                divMiddleNameCheckBoxRepeater.Visible = true;
                chkMiddleNameRequiredRepeater.Checked = false;
                rfvMiddleNameRepeater.Enabled = true;
            }
        }

        private void SetNoMiddleNameCheckboxPadding(HtmlGenericControl divNoMiddleNameChkBox)
        {
            if (IsUserRegistrationScreen && divNoMiddleNameChkBox.IsNotNull())
            {
                divNoMiddleNameChkBox.Style["Padding-bottom"] = "5px";
            }
        }
        #endregion
        private void AddSuffixDropdownAndDesignChange()
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                if (!PageType.IsNullOrEmpty())
                {
                    dvSpanNewAliasFN.Style.Add("Width", "11%");
                    dvSpanNewAliasMN.Style.Add("Width", "11%");
                    dvSpanNewAliasLN.Style.Add("Width", "11%");

                    dvAliasNewFirstName.Style.Add("Width", "15%");
                    dvAliasNewMiddleName.Style.Add("Width", "15%");
                    dvAliasNewLastName.Style.Add("Width", "15%");

                    dvAliasNewSuffix.Style.Add("display", "inline-block");
                    dvAliasNewSuffix.Style.Add("Width", "14%");
                    dvAliasNewSuffix.Style.Add("margin-left", "10px");
                }
            }
            else
            {
                dvAliasNewSuffix.Style.Add("display", "none");
            }
            BindSuffixDropdown(cmbAliasNewSuffix);
        }

        private void BindSuffixDropdown(WclComboBox suffixDropDown)
        {
            suffixDropDown.DataSource = CurrentViewContext.lstAliasSuffixes;
            suffixDropDown.DataBind();
            suffixDropDown.Items.Insert(AppConsts.NONE,
                                        new RadComboBoxItem(suffixDefaultText, "0"));
        }

        private WebControl GetSuffixControl(RepeaterItem item = null)
        {
            if (item.IsNotNull())
            {
                WclTextBox txtAliasSuffix = item.FindControl("txtAliasSuffix") as WclTextBox;
                WclComboBox cmbAliasSuffix = item.FindControl("cmbAliasSuffix") as WclComboBox;
                return CurrentViewContext.IsSuffixDropDownType ? (WebControl)cmbAliasSuffix
                                        : (WebControl)txtAliasSuffix;
            }
            else
            {
                return CurrentViewContext.IsSuffixDropDownType ? (WebControl)cmbAliasNewSuffix
                                        : (WebControl)txtAliasNewSuffix;
            }
        }

        private String GetSuffixText(RepeaterItem item = null)
        {
            String suffixText = String.Empty;
            if (item.IsNotNull())
            {
                WclTextBox txtAliasSuffix = item.FindControl("txtAliasSuffix") as WclTextBox;
                WclComboBox cmbAliasSuffix = item.FindControl("cmbAliasSuffix") as WclComboBox;
                if (CurrentViewContext.IsSuffixDropDownType)
                {
                    suffixText = cmbAliasSuffix.IsNotNull() && cmbAliasSuffix.SelectedItem.IsNotNull() && cmbAliasSuffix.SelectedIndex > AppConsts.NONE
                                 ? cmbAliasSuffix.SelectedItem.Text : String.Empty;
                }
                else
                {
                    suffixText = txtAliasSuffix.IsNotNull()
                     ? txtAliasSuffix.Text : String.Empty;
                }
            }
            else
            {
                if (CurrentViewContext.IsSuffixDropDownType)
                {
                    suffixText = cmbAliasNewSuffix.SelectedItem.IsNotNull() && cmbAliasNewSuffix.SelectedIndex > AppConsts.NONE
                                 ? cmbAliasNewSuffix.SelectedItem.Text : String.Empty;
                }
                else
                {
                    suffixText = txtAliasNewSuffix.Text;
                }
            }

            return suffixText;
        }
        private String SetSuffixText(String suffixText, RepeaterItem item = null)
        {
            if (item.IsNotNull())
            {
                WclTextBox txtAliasSuffix = item.FindControl("txtAliasSuffix") as WclTextBox;
                WclComboBox cmbAliasSuffix = item.FindControl("cmbAliasSuffix") as WclComboBox;
                if (CurrentViewContext.IsSuffixDropDownType)
                {
                    if (cmbAliasSuffix.IsNotNull())
                    {
                        BindSuffixDropdown(cmbAliasSuffix);
                        if (suffixText.IsNullOrEmpty())
                        {
                            cmbAliasSuffix.ClearSelection();
                        }
                        else
                        {
                            cmbAliasSuffix.Items.Where(x => x.Text.Equals(suffixText))
                                .ForEach(x => x.Selected = true);
                        }
                    }
                }
                else
                {
                    if (txtAliasSuffix.IsNotNull())
                    {
                        txtAliasSuffix.Text = suffixText;
                    }
                }
            }
            else
            {
                if (CurrentViewContext.IsSuffixDropDownType)
                {
                    if (suffixText.IsNullOrEmpty())
                    {
                        cmbAliasNewSuffix.ClearSelection();
                    }
                    else
                    {
                        cmbAliasNewSuffix.Items.Where(x => x.Text.Equals(suffixText))
                            .ForEach(x => x.Selected = true);
                    }
                }
                else
                {
                    txtAliasNewSuffix.Text = suffixText;
                }
            }

            return suffixText;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Rebind the Alias name, on 'Next' click from OrderPaymentDetails.ascx in admin order queue.
        /// </summary>
        /// <returns>
        /// If 'true', then display the div containing this user control, else hide the div, in OrderPaymentDetails.ascx
        /// </returns>
        public Boolean RebindAlias()
        {
            rptrAliasName.DataSource = null;
            if (!PersonAliasTempListMaxOcc.IsNullOrEmpty() && PersonAliasTempListMaxOcc.Count() > 0)
            {
                rptrAliasName.DataSource = PersonAliasTempListMaxOcc;
                rptrAliasName.DataBind();
                divPersonalAlias.Style["display"] = "block";
                return true;
            }
            else
            {
                rptrAliasName.DataBind();
                divPersonalAlias.Style["display"] = "none";
                return false;
            }
        }

        public void ResetAliasNewControls()
        {
            txtNewFirstName.Text = "";
            txtNewLastName.Text = "";
            txtNewMiddleName.Text = "";
            lblErrorMsg.Text = "";
            chkMiddleNameRequired.Checked = false;
            txtNewMiddleName.Enabled = true;
            SetSuffixText("");
        }
        #endregion
        #endregion
    }
}
