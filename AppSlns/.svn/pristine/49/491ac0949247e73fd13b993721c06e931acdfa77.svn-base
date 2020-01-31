<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ApplicantProfile" CodeBehind="ApplicantProfile.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="Location" Src="~/CommonControls/UserControl/LocationInfo.ascx" %>
<%@ Register TagPrefix="uc" TagName="PrevResident" Src="~/ComplianceOperations/UserControl/PreviousAddressControl.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoader.ascx"
    TagPrefix="infsu" TagName="CustomAttributes" %>
<%@ Register TagPrefix="uc" TagName="PersonAlias" Src="~/Shared/Controls/PersonAliasInfo.ascx" %>

<script src="../../Resources/Mod/Applicant/EditProfile.js" type="text/javascript"></script>
<style type="text/css">
    /*#dvSSNMask span.RadInput
    {
        width: 89% !important;
    }

    #dvSSNMask .rbToggleButton {
        padding-left: 0px !important;
        vertical-align: top;
    }*/
    .myControl .RadInput {
        width: 85% !important;
    }

    .myControl input[type="checkbox"] {
        margin-top: 4px;
        position: absolute;
        right: 10px;
        top: 2px;
    }


    .myControl {
        position: relative;
    }

    .errMsgUserGroup {
        font-size: 110% !important;
    }

    .FormatRadioButtonList label {
        margin-right: 15px;
    }

    .HideError {
        display: none;
    }

    .ShowError {
        display: block;
    }

    #dvNextBtnStyleInSpanish.nextBtnStyleInSpanish .rbNext + .rbPrimary {
        padding-right: 20px !important;
    }

    .addAlias {
        border: 1px solid #62798e;
    }
</style>
<script type="text/javascript">
    //UAT-2448
    function openPopUp() {
        var popupWindowName = "Country Identification Details";
        winopen = true;
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/BkgOperations/Pages/CountryIdentificationDetails.aspx");
        var win = $window.createPopup(url, { size: "500," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: popupWindowName, onclose: OnClientClose });
        return false;
    }
    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
            }
            winopen = false;
        }
    }
    var submitclicked = false;
    var openGoogleRecaptcha = false;
    function DelayButtonClick(s, e) {

        if (submitclicked == false) {
            submitclicked = true;
            s.set_autoPostBack(false);
            if (ConfirmSubmit(s, e)) {
                submitclicked = false;
                return;
            }
            else {

                s.set_autoPostBack(true);
                submitclicked = false;
                return true;
            }
        }
    }

    function ConfirmSubmit(s, e) {
        var IsLocationTenant = $jQuery("[id$=hdnIsLocationTenant]").val();
        var txtNewFirstName = $jQuery("[id$=txtNewFirstName]").val();
        var txtNewLastName = $jQuery("[id$=txtNewLastName]").val();
        var txtNewMiddleName = $jQuery("[id$=txtNewMiddleName]").val();
        //var txtNewSuffix = $jQuery("[id$=txtAliasNewSuffix]").val();
        //var cmbAliasNewSuffix = $jQuery("[id$=cmbAliasNewSuffix]").val();

        // if (txtNewSuffix == 'Enter Suffix' || txtNewSuffix == 'Ingrese un sufijo') { txtNewSuffix = '' }

        if (IsLocationTenant.toLowerCase() == "true" && Page_IsValid && txtNewLastName != undefined && txtNewFirstName != undefined && txtNewMiddleName != undefined &&
            (txtNewFirstName.trim() != '' || txtNewMiddleName.trim() != '' || txtNewLastName.trim() != ''))//|| cmbAliasNewSuffix.trim().length > 0))//txtNewSuffix.trim().length > 0)) {
        {
            $window.showDialog($jQuery(".confirmProfileSave").clone().show()
                , {
                    approvebtn: {
                        autoclose: true, text: "Ignore and Continue", click: function () {

                            $jQuery("[id$=txtNewFirstName]").val('');
                            $jQuery("[id$=txtNewLastName]").val('');
                            $jQuery("[id$=txtNewMiddleName]").val('');
                            $jQuery("[id$=cmbAliasNewSuffix]").val('');

                            window.setTimeout(function () {
                                $jQuery("#<%=btnhide.ClientID %>").trigger('click');
                                return true;
                            }, 100, s);
                        }
                    }, closeBtn: {
                        autoclose: true, text: "Cancel", click: function (s, e) {
                        }
                    }
                }, 475, 'Alert');
            return true;
        }
        else {
            return false;
        }
    }

        //function confirmClick() {
        //    var dialog = $window.showDialog($jQuery(".confirmSave").clone().show(), {
        //        approvebtn: {
        //            autoclose: true, text: "Ignore and Continue", click: function () {
        //debugger;
        //              $jQuery("[id$=hdnConfirmSave]").val(1);
        //$jQuery("#<%=btnhide.ClientID %>").trigger('click');
    //            }
    //        }, closeBtn: {
    //            autoclose: true, text: "Cancel", click: function () {
    //                $jQuery("[id$=hdnConfirmSave]").val(0);
    //                return false;
    //            }
    //        }
    //    }, 475, 'Alert');
    //}

    function DisableMailingSection() {

        var chkMailing = $jQuery("#<%=chkMailingAddress.ClientID%>");

        alert(chkMailing.length)
        if ($jQuery(chkMailing).prop("checked") == true) {

            $("#dvMailingAdress1 :input").attr("disabled", true);


        }


    }

    function CheckDOBForLocationTenant() {
        //debugger;
        var IslocationTenant = $jQuery("#<%= hdnIsLocationTenant.ClientID%>");
        if (IslocationTenant[0].value == "True") {
            Telerik.Web.UI.RadDateInput.prototype.parseDate = function (value, baseDate) {
                try {
                    var tokens;
                    var lexer = new Telerik.Web.UI.DateParsing.DateTimeLexer(this.get_dateFormatInfo());
                    try {
                        tokens = lexer.GetTokens(value);
                    } catch (e) {

                        return value;
                    }

                    var parser = new Telerik.Web.UI.DateParsing.DateTimeParser(this.get_dateFormatInfo().TimeInputOnly);
                    var entry = parser.Parse(tokens);
                    baseDate = this._getParsingBaseDate(baseDate);
                    var date = entry.Evaluate(baseDate, this.get_dateFormatInfo());
                    if (date.getDate() != entry.Second || date.getMonth() + 1 != entry.First || date.getFullYear() != entry.Third) {
                        //Page_IsValid = false;
                        $jQuery("[id$=lblDOBLocationError]").removeClass("HideError");
                        $jQuery("[id$=lblDOBLocationError]").addClass("ShowError");
                        return null;
                    }
                    else {
                        $jQuery("[id$=lblDOBLocationError]").removeClass("ShowError");
                        $jQuery("[id$=lblDOBLocationError]").addClass("HideError");
                    }

                    return date;
                }
                catch (parseError) {
                    if (parseError.isDateParseException) {
                        //return null;
                    }
                    else {
                        throw parseError;
                    }
                }
            }
        }
    }

    $jQuery(document).ready(function () {
        CheckDOBForLocationTenant();
    });

    function pageLoad() {
        // debugger;
        var LanguageCode = $jQuery("[id$=hdnLanguageCode]").val();
        if (LanguageCode == 'AAAA')
            $jQuery("[id$=dvNextBtnStyleInSpanish]").removeClass("nextBtnStyleInSpanish");
        if (LanguageCode == 'AAAB')
            $jQuery("[id$=dvNextBtnStyleInSpanish]").addClass("nextBtnStyleInSpanish");
    }

</script>
<asp:Panel ID="pnlMain" runat="server" Width="100%" Height="100%">
    <div class="section">
        <%--<h1 class="mhdr">Personal Information</h1>--%>
        <h1 class="mhdr" id="PERSONALINFO" runat="server"><%=Resources.Language.PERSONALINFO%></h1>
        <div class="content">
            <div class="msgbox" id="msgBox" runat="server">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRegForm">
                    <div id="dvNonMailingSection" runat="server">
                        <div class='sxro sx3co' id="divPersonalInfoInst" runat="server">
                            <div class='sxlb'>
                                <span class='cptn'>Personal Information Instructions</span>
                            </div>
                            <div class='sxlm m3spn'>
                                <asp:Literal ID="litPersonalInfoInst" runat="server"></asp:Literal>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <%-- <div class="sxro sx3co" runat="server" id="divMiddleNameCheckBox">
                        <infs:WclCheckBox runat="server" ID="chkMiddleNameRequired" ToolTip="Click here to enable/disable your Middle Name" onclick="MiddleNameEnableDisable(this)"></infs:WclCheckBox>
                        <asp:Label ID="lblChkMiddleName" runat="server">I don't have a middle name</asp:Label>
                        <div class='sxroend'>
                        </div>
                    </div>--%>


                        <div class='sxro sx3co'>
                            <div class='sxlb' id="dvSpnFirstName" runat="server">
                                <span class='cptn'><%=Resources.Language.FIRSTNAME%></span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm' id="dvFirstName" runat="server">
                                <infs:WclTextBox runat="server" ID="txtFirstName" MaxLength="50">
                                </infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,FIRSTNAMEREQ%>" ValidationGroup="grpFormSubmit" />
                                    <asp:RegularExpressionValidator runat="server" ID="revFirstName" ControlToValidate="txtFirstName"
                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                                        ErrorMessage="<%$Resources:Language,INVALIDCHARS%>" ValidationGroup="grpFormSubmit" />
                                    <div id="DivRegexFirstName" runat="server"></div>
                                </div>
                            </div>
                            <div class='sxlb' title="<%$Resources:Language,IFDONTHAVEMIDDLENAME%>" id="dvSpnMiddleName" runat="server">
                                <span class='cptn'><%=Resources.Language.MIDDLENAME%></span><span class="reqd" id="spnMiddleName" runat="server">*</span>
                            </div>
                            <div class='sxlm' id="dvMiddleName" runat="server">
                                <infs:WclTextBox runat="server" ID="txtMiddleName" Placeholder="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>"
                                    ToolTip="<%$Resources:Language, IFYOUDONTHAVEMIDDLENAME %>" MaxLength="50">
                                </infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvMiddleName" ControlToValidate="txtMiddleName"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,MIDDLENAMEREQ%>" ValidationGroup="grpFormSubmit" />
                                    <asp:RegularExpressionValidator runat="server" ID="revMiddleName" ControlToValidate="txtMiddleName"
                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                                        ErrorMessage="<%$Resources:Language,INVALIDCHARS%>" ValidationGroup="grpFormSubmit" />
                                    <div id="DivRegexMiddleName" runat="server"></div>
                                </div>
                            </div>
                            <div class='sxlb' id="dvSpnLastName" runat="server">
                                <span class='cptn'><%=Resources.Language.LASTNAME%></span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm' id="dvLastName" runat="server">
                                <infs:WclTextBox runat="server" ID="txtLastName" MaxLength="50">
                                </infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,LASTNAMEREQ%>" ValidationGroup="grpFormSubmit" />
                                    <asp:RegularExpressionValidator runat="server" ID="revLastName" ControlToValidate="txtLastName"
                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                                        ErrorMessage="<%$Resources:Language,INVALIDCHARS%>" ValidationGroup="grpFormSubmit" />
                                    <div id="DivRegexLastName" runat="server"></div>
                                </div>
                            </div>
                            <div class='sxlm' id="dvSuffix" runat="server" style="display: none; width: 15%">
                                <infs:WclComboBox ID="cmbSuffix" Visible="false" runat="server" DataTextField="Suffix" DataValueField="SuffixID"></infs:WclComboBox>
                                <infs:WclTextBox runat="server" ID="txtSuffix" MaxLength="10" ToolTip="<%$Resources:Language, ENTERSUFFIXIFAPPLICABLE %>" PlaceHolder="<%$Resources:Language, ENTERSUFFIXIFAPPLICABLE %>">
                                </infs:WclTextBox>
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtSuffix"
                                    Display="Dynamic" CssClass="errmsg" ValidationExpression="^[a-z A-Z]*-?[a-z A-Z]*$"
                                    ErrorMessage="<%$Resources:Language,SUFFIXNAMEVALDT%>" ValidationGroup="grpFormSubmit" />
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class="sxro sx3co" runat="server" id="divMiddleNameCheckBox">
                            <div class='sxlb nobg'>
                            </div>
                            <div class='sxlm'>
                            </div>
                            <infs:WclCheckBox runat="server" ID="chkMiddleNameRequired" onclick="MiddleNameEnableDisable(this)"></infs:WclCheckBox>
                            <asp:Label ID="lblChkMiddleName" Style="color: red; font-weight: bold" runat="server"><%=Resources.Language.ISMIDDLENAME%></asp:Label>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div runat="server" id="divSSN">
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.ISSSN%></span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm' style="padding-top: 5px;">
                                    <asp:RadioButtonList ID="rblSSN" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="FormatRadioButtonList" OnSelectedIndexChanged="rblSSN_SelectedIndexChanged">
                                        <asp:ListItem Text="<%$Resources:Language,LSTITMY %>" Value="true" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Language,LSTITMN %>" Value="false"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class='sxlb' id="lblSSN" runat="server">
                                    <span class='cptn'><%=Resources.Language.SSN%></span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm' id="dvSSNMask">
                                    <div style="width: 100%; float: left">
                                        <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="###-##-####" />
                                    </div>
                                    <%--<div style="width: 5%; float: left">
                                    <infs:WclButton runat="server" ID="chkAutoFillSSN" OnClientCheckedChanged="AutoFillSSN" ToolTip="Check this box if you do not have an SSN" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false" Visible="true">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Value="True" />
                                            <telerik:RadButtonToggleState Value="False" />
                                        </ToggleStates>
                                    </infs:WclButton>
                                </div>--%>
                                    <div class="valdx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                                            Display="Dynamic" ErrorMessage="<%$Resources:Language,SSNREQ%>" ValidationGroup="grpFormSubmit" />
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server"
                                            CssClass="errmsg" ErrorMessage="<%$Resources:Language,FULLSSNREQ%>" ValidationGroup="grpFormSubmit" ControlToValidate="txtSSN"
                                            ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                                        <div id="DivRegexSSN" runat="server"></div>
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="rgvSSNCBI" runat="server"
                                            CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDSSN%>" ControlToValidate="txtSSN" Enabled="false"
                                            ValidationExpression="(?!9)(?!\b(\d)\1+-(\d)\1+-(\d)\1+\b)(?!000)\d{3}-(?!00)\d{2}-(?!0{4})\d{4}" ValidationGroup="grpFormSubmit" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <uc:PersonAlias ID="ucPersonAlias" runat="server" Visible="true" IsLabelMode="true"></uc:PersonAlias>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class='cptn'><%=Resources.Language.GENDER%></span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <%--<asp:RadioButtonList ID="rblGender" runat="server" DataTextField="GenderName" DataValueField="GenderID"
                            RepeatColumns="3">
                        </asp:RadioButtonList>--%>
                                <%--  <infs:WclComboBox ID="cmbGender" runat="server" DataTextField="GenderName" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" DataValueField="GenderID">
                            </infs:WclComboBox>--%>
                                <infs:WclComboBox ID="cmbGender" runat="server" DataTextField="GenderName" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" DataValueField="DefaultLanguageKeyID">
                                </infs:WclComboBox>
                            </div>
                            <div class='sxlb'>
                                <span class='cptn'><%=Resources.Language.DOB%></span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclDatePicker ID="dpkrDOB" DatePopupButton-ToolTip="<% $Resources:Language, OPNCALENDERPOPUP%>" Calendar-FastNavigationSettings-TodayButtonCaption="<% $Resources:Language, TODAY%>" Calendar-FastNavigationSettings-CancelButtonCaption="<% $Resources:Language, CNCL%>" Calendar-FastNavigationSettings-OkButtonCaption="<% $Resources:Language, OK%>" runat="server" DateInput-EmptyMessage="<%$Resources:Language,SELECTDATE%>">
                                    <DateInput DateFormat="MM/dd/yyyy"></DateInput>
                                </infs:WclDatePicker>
                                <div class="valdx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvDOB" CssClass="errmsg" ControlToValidate="dpkrDOB"
                                        Display="Dynamic" ErrorMessage="<%$Resources:Language,DOBREQ%>" ValidationGroup="grpFormSubmit" />

                                    <asp:RangeValidator ID="rngvDOB" runat="server" ControlToValidate="dpkrDOB" Type="Date"
                                        ValidationGroup="grpFormSubmit" Display="Dynamic" CssClass="errmsg"></asp:RangeValidator>
                                    <asp:Label ID="lblDOBLocationError" runat="server" CssClass="errmsg HideError" Text="<%$Resources:Language,ENTERVALIDDOB %>"></asp:Label>
                                </div>
                            </div>
                            <div id="dvCommLang" style="display: none" runat="server">
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.PRFRDCOMCTNLANG %></span><span class="reqd"></span>
                                </div>

                                <div class='sxlm'>
                                    <infs:WclComboBox ID="cmbCommLang" runat="server" DataTextField="LAN_Name"
                                        DataValueField="LAN_ID">
                                    </infs:WclComboBox>
                                </div>
                            </div>

                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class='cptn'><%=Resources.Language.PHONE%></span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm myControl'>
                                <div id="dvMaskedPrimaryPhone" runat="server">
                                    <infs:WclMaskedTextBox ID="txtPrimaryPhone" runat="server" Mask="(###)-###-####">
                                    </infs:WclMaskedTextBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobile" runat="server" CssClass="errmsg"
                                            ErrorMessage="<%$Resources:Language,PHONEREQ%>" ControlToValidate="txtPrimaryPhone" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobile" runat="server" ValidationGroup="grpFormSubmit"
                                            CssClass="errmsg" ErrorMessage="<%$Resources:Language,PHNFORMAT%>" ControlToValidate="txtPrimaryPhone"
                                            ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
                                    </div>
                                </div>
                                <div id="dvUnmaskedPrimaryPhone" runat="server" style="display: none;">
                                    <infs:WclTextBox ID="txtUnmaskedPrimaryPhone" runat="server" MaxLength="15"></infs:WclTextBox>

                                    <div class="vldx">
                                        <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobileUnmasked" runat="server" CssClass="errmsg"
                                            ErrorMessage="<%$Resources:Language,PHONEREQ%>" ControlToValidate="txtUnmaskedPrimaryPhone" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobilePrmyNonMasking" runat="server" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                            ErrorMessage="<%$Resources:Language,INVALIDPHONE%>"
                                            ControlToValidate="txtUnmaskedPrimaryPhone" ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                                    </div>
                                </div>
                                <infs:WclCheckBox runat="server" ID="chkPrimaryPhone" ToolTip="<%$Resources:Language,NOUSNUMBER%>" onclick="MaskedUnmaskedPhone(this)"></infs:WclCheckBox>
                            </div>

                            <%--<asp:Literal runat="server" Text="<%$Resources:Language,ENTERSECONDPHONE%>"/>--%>
                            <div class='sxlb' title="Enter a second phone number here if you'd like to include one">
                                <span class='cptn'><%=Resources.Language.SECPHONE%></span>
                            </div>
                            <div class='sxlm myControl'>
                                <div id="dvMaskedSecondaryPhone" runat="server">
                                    <infs:WclMaskedTextBox ID="txtSecondaryPhone" runat="server" Mask="(###) ###-####">
                                    </infs:WclMaskedTextBox>
                                </div>
                                <div id="dvUnMaskedSecondaryPhone" runat="server" style="display: none;">
                                    <infs:WclTextBox ID="txtUnmaskedSecondaryPhone" runat="server" MaxLength="15"></infs:WclTextBox>
                                    <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtUnmaskedSecondaryPhone" runat="server" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                        ErrorMessage="<%$Resources:Language,INVALIDPHONE%>"
                                        ControlToValidate="txtUnmaskedPrimaryPhone" ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                                </div>
                                <infs:WclCheckBox runat="server" ID="chkSecondaryPhone" ToolTip="<%$Resources:Language,NOUSNUMBER%>" onclick="MaskUnmaskSecoundaryPhone(this)"></infs:WclCheckBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class='cptn'><%=Resources.Language.EMAIL%></span><%--<span class="reqd">*</span>--%>
                            </div>
                            <div class='sxlm '>
                                <asp:Label ID="lblPrimaryEmail" runat="server" CssClass="ronly"></asp:Label>
                            </div>
                            <div class='sxlb' title="<%=Resources.Language.SCNDEMAILTOINCLUDE%>">
                                <span class='cptn'><%=Resources.Language.SECEMAIL%></span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtSecondaryEmail" runat="server" MaxLength="250">
                                </infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RegularExpressionValidator ID="VldtxtSecondaryEmail1" runat="server" Display="Dynamic" ValidationGroup="grpFormSubmit"
                                        ErrorMessage="<%$Resources:Language,EMAILNOTVALID%>" ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                        ControlToValidate="txtSecondaryEmail" CssClass="errmsg">
                                        <div id="DivRegexSecondaryEmail" runat="server"></div>
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class='sxlb' title="<%=Resources.Language.RTYPEADDTNLEMAILADDRESS%>">
                                <span class='cptn'><%=Resources.Language.CSECEMAIL%></span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtConfirmSecEmail" runat="server" MaxLength="250">
                                </infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RegularExpressionValidator ID="VldtxtConfirmSecEmail1" runat="server" Display="Dynamic" ValidationGroup="grpFormSubmit"
                                        ErrorMessage="<%$Resources:Language,SECONDARYEMAILNOTMATCHED%>" ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                        ControlToValidate="txtConfirmSecEmail" CssClass="errmsg">
                                    </asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="cstVConfirmSecEmail" runat="server" ErrorMessage="<%$Resources:Language,EMAILADDRESSNOTMATCH%>"
                                        ClientIDMode="Static" CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail"
                                        ControlToValidate="txtConfirmSecEmail"></asp:CustomValidator>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co' id="divEditEmailNotifications" runat="server">
                            <div>
                                <span style="font-size: 10px"><%=Resources.Language.EDITPROFILEHEADER%></span>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <asp:Label runat="server" ID="lblAddress1" class='cptn'></asp:Label><span class="reqd">*</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclTextBox runat="server" ID="txtAddress1" MaxLength="100">
                                </infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAddress1" ControlToValidate="txtAddress1"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ADDRESSREQ%>" ValidationGroup="grpFormSubmit" />
                                    <asp:RegularExpressionValidator Display="Dynamic" Enabled="false" ID="revAddress1" runat="server"
                                        CssClass="errmsg" ErrorMessage="<%$Resources:Language,ENTERVALIDZIPASCIICODE%>" ControlToValidate="txtAddress1"
                                        ValidationExpression="^[\x01-\x7F]+$" ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                            <div id="dvAddress2" runat="server">
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.ADDRESS2%></span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtAddress2" MaxLength="100">
                                    </infs:WclTextBox>
                                </div>
                            </div>
                            <%--<div class='sxlb' title="Enter the country where you currently live">
                        <span class='cptn'>Country</span>
                    </div>
                    <div class='sxlm'>
                        <asp:Label ID="lblCountry" runat="server" CssClass="ronly"></asp:Label>
                    </div>--%>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <uc:Location ID="locationTenant" ZipTabIndex="6" CityTabIndex="7" runat="server" ControlsExtensionId="AOP"
                                NumberOfColumn="Three" ValidationGroup="grpFormSubmit" IsReverselookupControl="true" />
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co' runat="server" visible="false">
                            <div class='sxlb'>
                                <span class='cptn'>Move in Date</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclDatePicker ID="dpCurResidentFrom" runat="server" DateInput-EmptyMessage="Select a date">
                                </infs:WclDatePicker>
                                <div class="valdx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvCurResFrom" CssClass="errmsg" ControlToValidate="dpCurResidentFrom"
                                        Display="Dynamic" ErrorMessage="Move in Date is required." ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co' id="divMVRInfo" visible="false" runat="server">
                            <div>
                                <div class='sxlb'>
                                    <span class='cptn'>Do you have a valid Driver's License?</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclButton runat="server" ID="chkIsMVRInfo" ToggleType="CheckBox" ButtonType="ToggleButton" Checked="true"
                                        AutoPostBack="false" OnClientCheckedChanged="EnableDisableLicenseValidators" CausesValidation="false">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                        </ToggleStates>
                                    </infs:WclButton>
                                </div>
                            </div>
                            <div id="dvDriverLicenseState" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class='cptn'>Driver License State</span><span id="spnLicenseState" class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclComboBox ID="cmbState" CssClass="doNotClearText" runat="server" CheckBoxes="false" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab"
                                        DataTextField="StateName" DataValueField="StateID" EmptyMessage="--Select--" MarkFirstMatch="true" OnClientBlur="onMVRStateBlur">
                                    </infs:WclComboBox>
                                    <div class="valdx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvLicenseState" CssClass="errmsg" ControlToValidate="cmbState"
                                            Display="Dynamic" ErrorMessage="License State is required." ValidationGroup="grpFormSubmit" />
                                    </div>
                                </div>
                            </div>
                            <div id="dvDriverLicenseNo" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class='cptn'>Driver License Number</span><span id="spnLicenseNumber" class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox ID="txtLicenseNO" runat="server" MaxLength="256">
                                    </infs:WclTextBox>
                                    <div class="valdx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvtxtLicenseNO" CssClass="errmsg" ControlToValidate="txtLicenseNO"
                                            Display="Dynamic" ErrorMessage="License Number is required." ValidationGroup="grpFormSubmit" />
                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>


                        <div class='sxro sx3co' id="divInternationalCriminalSearchAttributes" runat="server">
                            <div runat="server" id="divMothersName">
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.MOTHERMAIDENNAME%></span><span class="reqd" runat="server" id="spnMotherName" style="display: none">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox ID="txtMotherName" runat="server" MaxLength="256">
                                    </infs:WclTextBox>
                                    <div class="valdx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMotherName" CssClass="errmsg" ControlToValidate="txtMotherName"
                                            Enabled="false" Display="Dynamic" ErrorMessage="<%$Resources:Language,MOTHERMAIDENNAMEREQ%>" ValidationGroup="grpFormSubmit" />
                                    </div>
                                </div>
                            </div>
                            <div id="divIdentificationNumber" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.IDENTIFICATIONNUMBER%></span><span class="reqd" runat="server" id="spnIdentificationNumber" style="display: none">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox ID="txtIdentificationNumber" runat="server" MaxLength="256">
                                        </infs:WclTextBox>
                                        <div class="valdx">
                                            <asp:RequiredFieldValidator runat="server" ID="rfvIdentificationNumber" CssClass="errmsg" ControlToValidate="txtIdentificationNumber"
                                                Display="Dynamic" ErrorMessage="<%$Resources:Language,IDENTIFICATIONREQ%>" ValidationGroup="grpFormSubmit" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class='sxlm' style="padding-top: 5px">
                                        <a href="#" id="help" onclick="openPopUp();"><%=Resources.Language.HELP%></a>&nbsp;&nbsp   
                                    </div>
                                </div>
                            </div>
                            <div id="divCriminalLicenseNumber" runat="server">
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.DRIVERLICENCENUMBER%></span><span class="reqd" runat="server" id="spnCriminalLicenseNumber" style="display: none">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox ID="txtCriminalLicenseNumber" runat="server" MaxLength="256">
                                    </infs:WclTextBox>
                                    <div class="valdx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvCriminalLicenseNumber" CssClass="errmsg" ControlToValidate="txtCriminalLicenseNumber"
                                            Display="Dynamic" ErrorMessage="<%$Resources:Language,LICENSENUMREQ%>" ValidationGroup="grpFormSubmit" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co' id="dvBackgroundReport" runat="server">
                            <div class='sxlb'>
                                <span class='cptn'>Send Background Report</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclButton runat="server" ID="chkSendBkgReport" ToggleType="CheckBox" ButtonType="ToggleButton" Checked="true"
                                    AutoPostBack="false" CausesValidation="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes,please send me a copy by e-mail." Value="True" />
                                        <telerik:RadButtonToggleState Text="No,do not send me a copy." Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <%--                    <div class='sxro sx3co' id="dvBackgroundReport" runat="server">
                        <div class='sxlb'>
                            <span class='cptn'><%=Resources.Language.SENDBKGREPORT%></span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclButton runat="server" ID="chkSendBkgReport" ToggleType="CheckBox" ButtonType="ToggleButton" Checked="true"
                                AutoPostBack="false" CausesValidation="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="<%$Resources:Language,SENDCOPYEMAIL%>"." Value="True" />
                                    <telerik:RadButtonToggleState Text="<%$Resources:Language,DONTSENDCOPY%>" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>--%>
                    </div>

                    <div id="dvMailing" visible="false" runat="server">
                        <div id="Div1" runat="server">
                            <div>
                                <h1 class="shdr"><%=Resources.Language.MAILINGADDRESS%></h1>
                            </div>
                            <div>
                                <infs:WclCheckBox runat="server" ID="chkMailingAddress" Checked="true" AutoPostBack="true" OnCheckedChanged="chkMailingAddress_CheckedChanged"></infs:WclCheckBox>
                                <asp:Label ID="lblMailingCheck" Style="color: red; font-weight: bold" runat="server"><%=Resources.Language.MAILINGCHECK%></asp:Label>
                            </div>

                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <asp:Label runat="server" ID="Label1" class='cptn'></asp:Label><span class="reqd">*</span>
                                </div>
                                <div id="dvMailingAdress1" class='sxlm m2spn'>
                                    <infs:WclTextBox runat="server" ID="txtMailingAddress1" MaxLength="100">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMailingAddress1" ControlToValidate="txtMailingAddress1"
                                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ADDRESS1REQ%>" ValidationGroup="grpFormSubmit" />
                                        <asp:RegularExpressionValidator Display="Dynamic" Enabled="true" ID="revMailingAddress1" runat="server"
                                            CssClass="errmsg" ErrorMessage="<%$Resources:Language,ADDRESSASCIIINVALIDCODE%>" ControlToValidate="txtMailingAddress1"
                                            ValidationExpression="^[\x01-\x7F]+$" ValidationGroup="grpFormSubmit" />
                                    </div>
                                </div>
                                <div id="dvMailingAddress2" runat="server">
                                    <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.ADDRESS2%></span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtAdrress2" MaxLength="100">
                                        </infs:WclTextBox>
                                    </div>
                                </div>
                                <%--<div class='sxlb' title="Enter the country where you currently live">
                        <span class='cptn'>Country</span>
                    </div>
                    <div class='sxlm'>
                        <asp:Label ID="lblCountry" runat="server" CssClass="ronly"></asp:Label>
                    </div>--%>
                                <div class='sxroend'>
                                </div>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <uc:Location ID="locationMailingTenant" ZipTabIndex="6" CityTabIndex="7" runat="server" ControlsExtensionId="AOP"
                                NumberOfColumn="Three" ValidationGroup="grpFormSubmit" IsReverselookupControl="true" />
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div id="dvMailingOption" visible="false" runat="server">
                            <div class="sxro sx3co" runat="server">
                                <div class="sxlb">
                                    <span class="cptn"><%=Resources.Language.SELECTMAILINGOPTION%></span>
                                </div>
                                <div class="sxlm">
                                    <infs:WclComboBox ID="cmbMailingOption" runat="server" DataValueField="FieldID" DataTextField="DisplayText"></infs:WclComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div runat="server" id="dvSMSNotification">
                        <h1 class="shdr"><%=Resources.Language.TEXTMSGNOTIFICATION%></h1>
                        <div class='sxro sx3co' runat="server">
                            <div class='sxlb'>
                                <span class='cptn'><%=Resources.Language.RECIEVETEXTNOTIFICATION%></span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rdbTextNotification" runat="server" onclick="HideShowPhoneNumber(this)" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="<%$Resources:Language,LSTITMY %> &nbsp; " Value="True" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Language,LSTITMN %>" Value="False" Selected="False"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div id="divHideShowPhoneNumber" runat="server">
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.CELLULARPHNNUM%></span><span id="spnPhoneNumberReq" runat="server" class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclMaskedTextBox runat="server" ID="txtPhoneNumber" Mask="(###)-###-####">
                                    </infs:WclMaskedTextBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPhoneNumber" ControlToValidate="txtPhoneNumber"
                                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,CELLULARPHNREQ%>" ValidationGroup="grpFormSubmit" />
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revCellularPhoneNumber" runat="server" ValidationGroup="grpFormSubmit"
                                            CssClass="errmsg" ErrorMessage="<%$Resources:Language,PHNFORMAT%>" ControlToValidate="txtPhoneNumber"
                                            ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
                                        <div id="DivRegexPhoneNumber" runat="server"></div>
                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div runat="server" id="dvResHistory">
                        <h6 class="mhdr">Residential History</h6>
                        <div class='sxro sx3co' id="dvResHistoryText" runat="server">
                            <div class='sxlb'>
                                <span class='cptn'>Residential History Instructions</span>
                            </div>
                            <div class='sxlm m3spn'>
                                <asp:Literal ID="litresHistoryText" runat="server"></asp:Literal>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <uc:PrevResident ID="PrevResident" runat="server" />
                    </div>
                    <%--                    <div runat="server" id="dvResHistory">
                        <h6 class="mhdr"><%=Resources.Language.RESIDENTIALHISTORY%></h6>
                        <div class='sxro sx3co' id="dvResHistoryText" runat="server">
                            <div class='sxlb'>
                                <span class='cptn'><=Resources.Language.RESIDENTIALHISTORYINST></span>
                            </div>
                            <div class='sxlm m3spn'>
                                <asp:Literal ID="litresHistoryText" runat="server"></asp:Literal>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <uc:PrevResident ID="PrevResident" runat="server" />
                    </div>--%>

                    <div id="dvVerificationCode" style="display: none" runat="server">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class='cptn'><%=Resources.Language.VERIFICATIONCODE%></span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtVerificationCode" MaxLength="10">
                                </infs:WclTextBox>
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx3co' id="chkUpdatePersonalDetailsDiv" runat="server" title="Check this box to update our account profile with the information on this screen">
                        <asp:CheckBox ID="chkUpdatePersonalDetails" Checked="true" runat="server" Text="Save personal information changes to account profile." />
                    </div>
                </asp:Panel>
            </div>
        </div>
        <infsu:CustomAttributes ID="caOtherDetails" IsApplicantProfileScreen="true" IsUserGroupSlctdValuesdisabled="true" IsMultiSelectionAllowed="false" runat="server"></infsu:CustomAttributes>

        <div class='sxro sx3co'>
            <div class='sxlm'>
                <div class="vldx">
                    <span id="spnUserGroupErrMsg" class="errMsgUserGroup">
                        <asp:Label ID="errMsgUserGroup" runat="server" IsValidated="0" CssClass="errmsg" Text="" Visible="false"></asp:Label>
                    </span>
                </div>
            </div>
        </div>
        <%--<div class='sxroend'>
                </div>--%>
        <asp:Button ID="btnhide" runat="server" Style="display: none;" OnClick="fsucCmdBar1_SubmitClick" />
        <div class="cmdButtonMinSize">
            <div id="dvNextBtnStyleInSpanish" class="nextBtnStyleInSpanish">
                <infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Clear,Submit" AutoPostbackButtons="Submit,Clear"
                    ButtonPosition="Center" DefaultPanel="pnlMain" DefaultPanelButton="Clear"
                    SubmitButtonText="Restart Order" SubmitButtonIconClass="rbPrevious" OnSubmitClick="fsucCmdBar1_ExtraClick"
                    ClearButtonText="Continue" ClearButtonIconClass="rbNext" OnClearClientClick="DelayButtonClick" OnClearClick="fsucCmdBar1_SubmitClick">
                    <ExtraCommandButtons>
                        <infs:WclButton ID="btnCancelOrder" runat="server" Text="<%$Resources:Language,CNCL %>" UseSubmitBehavior="false" CssClass="margin-2 cancelposition"
                            AutoPostBack="true" OnClick="cmdbarSubmit_CancelClick">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </infs:WclButton>
                    </ExtraCommandButtons>
                </infsu:CommandBar>
            </div>
        </div>
        <div id="confirmSave" class="confirmProfileSave" runat="server" style="display: none">
            <p style="text-align: center"><%=Resources.Language.WRNGIGNRCNTNUADDALIAS %></p>

        </div>
    </div>

    <input type="hidden" runat="server" id="hdnIsMotherNameRequired" />
    <input type="hidden" runat="server" id="hdnIsIdentificationRequired" />
    <input type="hidden" runat="server" id="hdnLicenseRequired" />
    <asp:HiddenField ID="hdnNoMiddleNameText" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsFromApplicantProfile" runat="server" Value="1" />
    <asp:HiddenField ID="hdnIsLocationTenant" runat="server" />
    <asp:HiddenField ID="hdnConfirmSave" runat="server" Value="0" />
    <asp:HiddenField ID="hdnLanguageCode" Value="AAAA" runat="server" />
    <asp:HiddenField ID="hdnIFYOUDONTHAVEMIDDLENAME" runat="server" Value="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>" />
</asp:Panel>
