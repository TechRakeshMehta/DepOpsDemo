<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Search.Views.ApplicantPortfolioProfile" CodeBehind="ApplicantPortfolioProfile.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/NewLocationInfo.ascx" TagPrefix="uc"
    TagName="Location" %>
<%@ Register TagPrefix="uc" TagName="PersonAlias" Src="~/Shared/Controls/NewPersonAliasInfo.ascx" %>
<%@ Register Src="~/SearchUI/UserControl/NewApplicantProfileNotes.ascx" TagPrefix="uc"
    TagName="ApplicantProfileNotes" %>
<%@ Register TagPrefix="uc" TagName="PrevResident" Src="~/ComplianceOperations/UserControl/NewPreviousAddressControl.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CustomAttribute" Src="~/ComplianceAdministration/UserControl/NewCustomAttributeLoader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<script type="text/javascript">
    Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
    Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };

    $jQuery(document).ready(function () {
        //UAT-1955 
        $jQuery("[id$=divNotificationHeader]").on("keyup", function (e) {
            if (e.keyCode == 13) {
                $jQuery(this).click();
            }
        });

        $jQuery(".ruBrowse").attr('aria-hidden', 'true');
        $jQuery(".ruBrowse").attr('title', 'Click here to change your profile picture');
        $jQuery("[id$=uploadControlfile0]").attr("title", "Click here to change your profile picture");
        $jQuery("[id$=uploadControlfile0]").attr("tabindex", "0");
        $jQuery("[id$=uploadControlrow0]").attr("title", "Click here to change your profile picture");


        if ($jQuery("[id$=rdbSpecifyAuthentication]").length > 0) {
            $jQuery("[id$=rdbSpecifyAuthentication]").on('change', function () {
                CheckPhoneNumberRequiredStatus();
            });
        }
    });

    function DisplayPhoneNumberControl(id, controlNumber) {
        if (controlNumber == 1) {
            var dvMasking = $jQuery("[id$=dvMasking]");
            var dvUnmasking = $jQuery("[id$=dvUnmasking]");
            if (id.checked) {
                dvUnmasking[0].style.display = "block";
                dvMasking[0].style.display = "none";
                ValidatorEnable($jQuery("[id$=revTxtMobile]")[0], false);
                ValidatorEnable($jQuery("[id$=rfvTxtMobile]")[0], false);
                ValidatorEnable($jQuery("[id$=rfvTxtMobilePrmyNonMasking]")[0], true);
                ValidatorEnable($jQuery("[id$=revTxtMobilePrmyNonMasking]")[0], true);
                $jQuery("[id$=rfvTxtMobilePrmyNonMasking]").hide();
            }
            else {
                dvUnmasking[0].style.display = "none";
                dvMasking[0].style.display = "block";
                ValidatorEnable($jQuery("[id$=revTxtMobile]")[0], true);
                ValidatorEnable($jQuery("[id$=rfvTxtMobile]")[0], true);
                ValidatorEnable($jQuery("[id$=rfvTxtMobilePrmyNonMasking]")[0], false);
                ValidatorEnable($jQuery("[id$=revTxtMobilePrmyNonMasking]")[0], false);
                $jQuery("[id$=rfvTxtMobile]").hide();
            }
        }
        else if (controlNumber == 2) {
            var dvMaskingSecondary = $jQuery("[id$=dvMaskingSecondary]");
            var dvUnMaskingSecondary = $jQuery("[id$=dvUnMaskingSecondary]");
            if (id.checked) {
                dvUnMaskingSecondary[0].style.display = "block";
                dvMaskingSecondary[0].style.display = "none";
            }
            else {
                dvUnMaskingSecondary[0].style.display = "none";
                dvMaskingSecondary[0].style.display = "block";
            }
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
                            //$jQuery("[id$=cmbAliasNewSuffix]").val('');

                            window.setTimeout(function () {
                                $jQuery("#<%=btnContinue.ClientID %>").trigger('click');
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

</script>
<infs:WclResourceManagerProxy runat="server" ID="rprxApplicantPortfolioProfile">
    <infs:LinkedResource Path="~/Resources/Mod/Applicant/editprofile.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Applicant/EditProfile.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .chkIsMaskingRequired input[type="checkbox"] {
        margin-top: 4px;
        position: absolute;
        right: -35px;
        top: 2px;
    }
    /*#dvPrimaryPhone .vldx {
        margin-left : -180px;
    }*/
    div#dvPrimaryPhone, div#dvSecondaryPhone {
        position: relative;
    }

    .clsSaveButton {
        padding-left: 50% !important;
    }

    .FormatRadioButtonList label {
        margin-right: 15px;
    }
</style>

<div class="row">
    <div class="col-md-12">
        <div class="msgbox" id="msgBox" runat="server">
            <asp:Label ID="lblMessage" runat="server" CssClass="info"></asp:Label>
        </div>
    </div>
</div>
<div class="row bgLightGreen">
    <asp:Panel runat="server" ID="pnlEditProfile">
        <div class="col-md-2" id="dvProfilePicture" runat="server">
            <div id="dvProfilePic" runat="server">
                <div class="col-md-12" tabindex="0">
                    <asp:Image runat="server" ID="imgCntrl" class="thumb" AlternateText="Applicant's profile picture" />
                    <asp:Label runat="server" ID="lblNameInitials" Visible="false" class="initName" />
                </div>
                <div class="col-md-12">&nbsp;</div>
                <div class="col-md-12" title="Click this button to change your profile picture">
                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                        OnClientFileUploaded="onFileUploaded" MultipleFileSelection="Disabled" MaxFileInputsCount="1"
                        AllowedFileExtensions=".jpg,.jpeg,.tiff,.bmp,.bitmap,.png,.JPG,.PNG,.BITMAP,.JPEG,.TIFF,.BMP"
                        OnClientFileSelected="onClientFileSelected" TabIndex="0" ToolTip="Click here to change your profile picture"
                        Localization-Select="Change" Width="80px" OnClientValidationFailed="upl_OnClientValidationFailed" />
                    <div style="display: none">
                        <infs:WclButton ID="btnUpload" runat="server" OnClick="btnUpload_Click">
                        </infs:WclButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-10">
            <div id="dvAccountInfo" runat="server">
                <div class='row bgLightGreen'>
                    <div class='form-group col-md-3'>
                        <label id="lblUserName" class='cptn'>Username</label><span class="reqd">*</span>
                        <infs:WclTextBox EnableAriaSupport="true" aria-labelledby="lblUserName" runat="server" ID="txtUsername" MaxLength="256" ReadOnly="true"
                            Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <span id="spnUserNameErrorMsg" class="errmsg">
                                <asp:Label ID="lblUserNameMessage" runat="server" IsValidated="0" CssClass="errmsg"
                                    Text=""></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <infs:WclButton runat="server" ID="btnCheckUsername" Text="Check" Skin="Windows7"
                            AutoSkinMode="false" CausesValidation="true" ValidationGroup="grpFormSubmit"
                            Visible="false">
                        </infs:WclButton>
                    </div>
                </div>
                <div class='row bgLightGreen'>
                    <div class='form-group col-md-3'>
                        <label id="lblInstitute" class='cptn'>Institute</label>
                        <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-labelledby="lblInstitute" ID="txtOrganization" MaxLength="50" ReadOnly="true"
                            Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' id="divChangePwd" runat="server" visible="false">
                        <span class='cptn'>Change Password</span>
                        <asp:HyperLink ID="lnkChangePassword" runat="server" Visible="false" CssClass="user"
                            Text="Change Password"> </asp:HyperLink>

                    </div>
                </div>
            </div>
            <div class="row">
                <div class='col-md-12'>
                    <h2 class="header-color" tabindex="0">Personal Information</h2>
                </div>
            </div>

            <div class='row bgLightGreen'>
                <div class='form-group col-md-3'>
                    <label id="lblFName" class='cptn'>First Name</label><span class="reqd">*</span>
                    <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-labelledby="lblFName" ID="txtFirstName" MaxLength="50" Width="100%" CssClass="form-control">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="First Name is required." ValidationGroup="grpFormSubmit" />
                        <asp:RegularExpressionValidator runat="server" ID="revFirstName" ControlToValidate="txtFirstName"
                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                            ErrorMessage="Invalid character(s)." ValidationGroup="grpFormSubmit" />

                    </div>
                </div>
                <div class='form-group col-md-3' title='If you do not have a middle name, leave this field blank'>
                    <label id="lblMName" class='cptn'>Middle Name</label><span class="reqd" id="spnMiddleName" runat="server">*</span>
                    <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-labelledby="lblMName"
                        ID="txtMiddleName" MaxLength="50" Width="100%" CssClass="form-control"
                        PlaceHolder="If you don't have a middle name,check the box below."
                        ToolTip="If you don't have a middle name,check the box below.">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvMiddleName" ControlToValidate="txtMiddleName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Middle Name is required." ValidationGroup="grpFormSubmit" />
                        <asp:RegularExpressionValidator runat="server" ID="revMiddleName" ControlToValidate="txtMiddleName"
                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                            ErrorMessage="Invalid character(s)." ValidationGroup="grpFormSubmit" />
                    </div>
                </div>
                <div class='form-group col-md-3'>
                    <label id="lblLName" class='cptn'>Last Name</label><span class="reqd">*</span>
                    <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-labelledby="lblLName" ID="txtLastName" MaxLength="50" Width="100%" CssClass="form-control">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Last Name is required." ValidationGroup="grpFormSubmit" />
                        <asp:RegularExpressionValidator runat="server" ID="revLastName" ControlToValidate="txtLastName"
                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                            ErrorMessage="Invalid character(s)." ValidationGroup="grpFormSubmit" />
                    </div>
                </div>
                <div class='form-group col-md-3' id="dvSuffix" runat="server" style="display: none; width: 18%; margin-top:2%">
                         <infs:WclComboBox ID="cmbSuffix" runat="server" Visible="false" EmptyMessage="--Select Suffix--" Skin="Silk" AutoSkinMode="false" DataTextField="Suffix" DataValueField="SuffixID"></infs:WclComboBox>
                    <infs:WclTextBox runat="server" ID="txtSuffix" ToolTip="Enter Suffix if Applicable" placeholder="Enter Suffix if Applicable" MaxLength="10"></infs:WclTextBox>
                </div>

            </div>
            <div class='row bgLightGreen'>
                <div class='col-md-3'>
                </div>
                <div class='col-md-4'>
                    <infs:WclCheckBox CssClass="marBottom3" runat="server" ID="chkMiddleNameRequired"
                        onclick="MiddleNameEnableDisable(this)"></infs:WclCheckBox>
                    <asp:Label ID="lblChkMiddleName" Style="color: red; font-weight: bold" runat="server">I don't have a Middle Name.</asp:Label>
                </div>
            </div>
            <div class='row bgLightGreen'>
                <div class='form-group col-md-3' id="dvCheckSSN" runat="server">
                    <span class='cptn'>Do you have an SSN?</span><span class="reqd">*</span>
                    <asp:RadioButtonList ID="rblSSN" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="FormatRadioButtonList" OnSelectedIndexChanged="rblSSN_SelectedIndexChanged">
                        <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="No" Value="false"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class='form-group col-md-3'>
                    <div id="divSSN" runat="server">
                        <span class='cptn'>Social Security Number</span><span class="reqd">*</span>
                    </div>
                    <div id="dvSSNMask">
                        <div class='form-group col-md-10'>
                            <div class='row'>
                                <infs:WclMaskedTextBox runat="server" Skin="Silk" Width="100%" AutoSkinMode="false"
                                    ID="txtSSN" Mask="###-##-####" />
                            </div>
                        </div>
                        <%--<div class='form-group col-md-2'>--%>
                        <div class='row'>
                        </div>
                        <%--</div>--%>
                    </div>
                    <div class="valdx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                            Display="Dynamic" ErrorMessage="Social Security Number is required." ValidationGroup="grpFormSubmit" />
                        <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server" CssClass="errmsg"
                            ErrorMessage="Full Social Security Number is required." ValidationGroup="grpFormSubmit"
                            ControlToValidate="txtSSN" ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                    </div>
                </div>
            </div>
            <%--<div class='col-md-12'>--%>
            <uc:PersonAlias ID="ucPersonAlias" runat="server" Visible="true" IsEditProfile="true"
                IsLabelMode="true"></uc:PersonAlias>
            <%--</div>--%>
            <div>
                <div class='row bgLightGreen'>
                    <div class='form-group col-md-3'>
                        <label for="<%=cmbGender.ClientID %>_Input" class='cptn'>Gender</label><span class="reqd">*</span>
                        <infs:WclComboBox ID="cmbGender" runat="server" DataTextField="GenderName" Skin="Silk" OnClientKeyPressing="openCmbBoxOnTab"
                            AutoSkinMode="false" Width="100%" CssClass="form-control" DataValueField="GenderID">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3'>
                        <div id="divDOB" runat="server">
                            <label for="<%=dpkrDOB.ClientID %>_dateInput" class='cptn'>Date of Birth</label><span class="reqd">*</span>
                            <infs:WclDatePicker ID="dpkrDOB" Width="100%" EnableAriaSupport="true" runat="server"
                                DateInput-EmptyMessage="Select a date" DateInput-EnableAriaSupport="true" DateInput-SelectionOnFocus="CaretToBeginning"
                                ClientEvents-OnPopupClosing="OnCalenderClosing">
                                <Calendar EnableKeyboardNavigation="true" EnableAriaSupport="true"></Calendar>
                            </infs:WclDatePicker>
                            <div class="valdx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvDOB" CssClass="errmsg" ControlToValidate="dpkrDOB"
                                    Display="Dynamic" ErrorMessage="Date of Birth is required." ValidationGroup="grpFormSubmit" />
                                <asp:RangeValidator ID="rngvDOB" runat="server" ControlToValidate="dpkrDOB" Type="Date"
                                    ValidationGroup="grpFormSubmit" Display="Dynamic" CssClass="errmsg" Text="Date of birth should not be less than a year.">
                                </asp:RangeValidator>
                            </div>
                        </div>
                    </div>
                    <%--<div class='form-group col-md-3'>
                        <div id="divSSN" runat="server" title="Enter your social security number. If you do not have an SSN, enter 111-11-1111">
                            <span class='cptn'>Social Security Number</span><span class="reqd">*</span>
                        </div>
                        <div id="dvSSNMask">
                            <div class='form-group col-md-10'>
                                <div class='row'>
                                    <infs:WclMaskedTextBox runat="server" Skin="Silk" Width="100%" AutoSkinMode="false"
                                        ID="txtSSN" Mask="###-##-####" ToolTip="If you do not have an SSN, enter 111-11-1111" />
                                </div>
                            </div>
                            <div class='form-group col-md-2'>
                                <div class='row'>
                                    <infs:WclButton runat="server" ID="chkAutoFillSSN" OnClientCheckedChanged="AutoFillSSN"
                                        ToolTip="Check this box if you do not have an SSN" ToggleType="CheckBox"
                                        ButtonType="ToggleButton" AutoPostBack="false" CssClass="form-control" Visible="true">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Value="True" />
                                            <telerik:RadButtonToggleState Value="False" />
                                        </ToggleStates>
                                    </infs:WclButton>
                                </div>
                            </div>
                        </div>
                        <div class="valdx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                                Display="Dynamic" ErrorMessage="Social Security Number is required." ValidationGroup="grpFormSubmit" />
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server" CssClass="errmsg"
                                ErrorMessage="Full Social Security Number is required." ValidationGroup="grpFormSubmit"
                                ControlToValidate="txtSSN" ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                        </div>
                        </div>--%>
                    <div id="divSSNMasked" visible="false" runat="server">
                        <div class='form-group col-md-3'>
                            <label id="lblSSN" class='cptn'>Social Security Number</label><span class="reqd">*</span>
                            <infs:WclTextBox EnableAriaSupport="true" aria-labelledby="lblSSN" Width="100%" CssClass="form-control" ID="txtSSNMasked" runat="server"
                                ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <%--<div class='col-md-12'>--%>

            <%--</div>--%>
            <div>
                <div class='row'>
                    <div class='col-md-12'>
                        <h2 class="header-color" style="margin-top: 0px;">Contact Information</h2>
                    </div>
                </div>
            </div>
            <div class='row bgLightGreen'>
                <div class='form-group col-md-3'>
                    <label id="lblPrimaryEmail" class='cptn'>Primary Email</label><span class="reqd">*</span>
                    <infs:WclTextBox EnableAriaSupport="true" aria-labelledby="lblPrimaryEmail" Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtPrimaryEmail"
                        runat="server" MaxLength="250">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtPrimaryEmail"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is required."
                            ValidationGroup="grpFormSubmit" />
                        <asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ControlToValidate="txtPrimaryEmail"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is not valid."
                            ValidationGroup="grpFormSubmit"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                    </div>
                </div>
                <div class='form-group col-md-3'>
                    <div title="Retype email address">
                        <label id="lblConfirmPrimaryEmail" class='cptn'>Confirm Primary Email</label><span class="reqd">*</span>
                    </div>
                    <infs:WclTextBox EnableAriaSupport="true" aria-labelledby="lblConfirmPrimaryEmail" Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtConfrimPrimayEmail"
                        runat="server" MaxLength="250">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvConfirmEmail" ControlToValidate="txtConfrimPrimayEmail"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Confirm Email Address is required."
                            ValidationGroup="grpFormSubmit" />
                        <asp:CustomValidator ID="cstVConfirmPrimaryEmail" runat="server" ErrorMessage="Email Address did not match."
                            CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail" ClientIDMode="Static"
                            ValidationGroup="grpFormSubmit"
                            ControlToValidate="txtConfrimPrimayEmail">
                        </asp:CustomValidator>
                    </div>
                </div>
                <div class='form-group col-md-3'>
                    <div title="Send password recovery emails to this email address">
                        <label id="lblPasswordRecoveryEmail" class='cptn'>Password Recovery Email</label>
                    </div>
                    <infs:WclTextBox EnableAriaSupport="true" aria-labelledby="lblPasswordRecoveryEmail" Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtPswdRecoveryEmail"
                        runat="server" MaxLength="250" ReadOnly="true">
                    </infs:WclTextBox>
                </div>
            </div>
            <div class='row bgLightGreen'>
                <div class='col-md-12'>
                    <asp:CheckBox ID="chkChangeEmail" CssClass="form-control" runat="server" Text="Send password recovery emails to Primary Email address also"
                        ToolTip="Select this checkbox if you would like to use your primary email address for password recovery"></asp:CheckBox>
                </div>
            </div>
            <div class='row bgLightGreen'>
                <div class='form-group col-md-3'>
                    <div title="Enter a second email address if you'd like to include one">
                        <label id="lblSecondaryEmail" class='cptn'>Secondary Email</label>
                    </div>
                    <infs:WclTextBox EnableAriaSupport="true" aria-labelledby="lblSecondaryEmail" Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtSecondaryEmail"
                        runat="server" MaxLength="250">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RegularExpressionValidator ID="VldtxtSecondaryEmail1" runat="server" Display="Dynamic"
                            ValidationGroup="grpFormSubmit"
                            ErrorMessage="Email Address is not valid." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                            ControlToValidate="txtSecondaryEmail" CssClass="errmsg">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class='form-group col-md-3'>
                    <div title="Retype additional email address (if including a second email address)">
                        <label id="lblConfirmSecondaryEmail" class='cptn'>Confirm Secondary Email</label>
                    </div>
                    <infs:WclTextBox ID="txtConfirmSecEmail" EnableAriaSupport="true" aria-labelledby="lblConfirmSecondaryEmail" Skin="Silk" AutoSkinMode="false" Width="100%"
                        runat="server" MaxLength="250">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RegularExpressionValidator ID="VldtxtConfirmSecEmail1" runat="server" Display="Dynamic"
                            ValidationGroup="grpFormSubmit"
                            ErrorMessage="Secondary Email Address did not match." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                            ControlToValidate="txtConfirmSecEmail" CssClass="errmsg">
                        </asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="cstVConfirmSecEmail" runat="server" ErrorMessage="Email Address did not match."
                            ClientIDMode="Static" CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail"
                            ControlToValidate="txtConfirmSecEmail"></asp:CustomValidator>
                    </div>
                </div>
                <div class='form-group col-md-3'>
                    <label id="lblPhone" class='cptn'>Phone</label><span class="reqd">*</span>
                    <div id="dvPrimaryPhone" class="sxlm">
                        <div id="dvMasking" style="display: block" runat="server">
                            <infs:WclMaskedTextBox EnableAriaSupport="true" aria-labelledby="lblPhone" ID="txtPrimaryPhone" Skin="Silk" AutoSkinMode="false" Width="100%"
                                runat="server" Mask="(###)-###-####">
                            </infs:WclMaskedTextBox>
                        </div>
                        <div id="dvUnmasking" style="display: none" runat="server">
                            <infs:WclTextBox ID="txtPrimaryPhoneNonMasking" runat="server" MaxLength="15" Width="100%"></infs:WclTextBox>
                        </div>
                        <asp:CheckBox ID="chkIsMaskingRequiredPrimary" CssClass="chkIsMaskingRequired" runat="server" onclick="DisplayPhoneNumberControl(this,1)"
                            Checked="false" ToolTip="Check this box if you do not have a US number." />
                        <div class="vldx">
                            <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobile" runat="server" CssClass="errmsg"
                                ErrorMessage="Phone is required." ControlToValidate="txtPrimaryPhone" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobilePrmyNonMasking" runat="server" CssClass="errmsg"
                                ErrorMessage="Phone is required." ControlToValidate="txtPrimaryPhoneNonMasking" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobile" runat="server"
                                ValidationGroup="grpFormSubmit"
                                CssClass="errmsg" ErrorMessage="Format is (###)-###-####" ControlToValidate="txtPrimaryPhone"
                                ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobilePrmyNonMasking" runat="server"
                                ValidationGroup="grpFormSubmit"
                                CssClass="errmsg" ErrorMessage="Invalid phone number. This field only contains +, - and numbers." ControlToValidate="txtPrimaryPhoneNonMasking"
                                ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                        </div>
                    </div>
                </div>
                <div class='form-group col-md-3'>
                    <div title="Enter a second phone number here if you'd like to include one">
                        <label id="lblSecondaryPhone" class='cptn'>Secondary Phone</label>
                    </div>
                    <div id="dvSecondaryPhone" class="sxlm">
                        <div id="dvMaskingSecondary" style="display: block" runat="server">
                            <infs:WclMaskedTextBox Skin="Silk" EnableAriaSupport="true" aria-labelledby="lblSecondaryPhone" AutoSkinMode="false" Width="100%" ID="txtSecondaryPhone"
                                runat="server" Mask="(###)-###-####">
                            </infs:WclMaskedTextBox>
                        </div>
                        <div id="dvUnMaskingSecondary" style="display: none" runat="server">
                            <infs:WclTextBox ID="txtSecondaryPhoneNonMasking" runat="server" MaxLength="15" CssClass="ValidatetionCheck"></infs:WclTextBox>
                        </div>
                        <asp:CheckBox ID="chkIsMaskingRequiredSecondary" CssClass="chkIsMaskingRequired" runat="server" onclick="DisplayPhoneNumberControl(this,2)"
                            Checked="false" ToolTip="Check this box if you do not have a US number." />
                    </div>
                </div>
            </div>
            <div class='row bgLightGreen'>
                <div class='form-group col-md-6'>
                    <label id="lblAdress1" class='cptn'>Address 1</label><span class="reqd">*</span>
                    <infs:WclTextBox Skin="Silk" EnableAriaSupport="true" aria-labelledby="lblAdress1" AutoSkinMode="false" Width="100%" runat="server" ID="txtAddress1"
                        MaxLength="100">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvAddress1" ControlToValidate="txtAddress1"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Address 1 is required." ValidationGroup="grpFormSubmit" />
                    </div>
                </div>
                <div class='form-group col-md-3'>
                    <label id="lblAddress2" class='cptn'>Address 2</label>
                    <infs:WclTextBox Skin="Silk" EnableAriaSupport="true" aria-labelledby="lblAddress2" AutoSkinMode="false" Width="100%" runat="server" ID="txtAddress2"
                        MaxLength="100">
                    </infs:WclTextBox>
                </div>
            </div>
            <div class='row bgLightGreen'>
                <uc:Location ID="locationTenant" ZipTabIndex="6" CityTabIndex="7" runat="server"
                    IsReverselookupControl="true"
                    NumberOfColumn="Four" ValidationGroup="grpFormSubmit" ControlsExtensionId="AEP" />
            </div>
            <div id="Div1" class='row' runat="server" visible="false">
                <div class='form-group col-md-3'>
                    <label for="<%= dpCurResidentFrom.ClientID %>_dateInput" class='cptn'>Move in Date</label><span class="reqd">*</span>
                    <infs:WclDatePicker ID="dpCurResidentFrom" runat="server" DateInput-EmptyMessage="Select a date"
                        DateInput-EnableAriaSupport="true" DateInput-SelectionOnFocus="CaretToBeginning"
                        ClientEvents-OnPopupClosing="OnCalenderClosing" EnableAriaSupport="true">
                        <Calendar EnableKeyboardNavigation="true" EnableAriaSupport="true"></Calendar>
                    </infs:WclDatePicker>
                    <div class="valdx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvCurResFrom" CssClass="errmsg" ControlToValidate="dpCurResidentFrom"
                            Display="Dynamic" ErrorMessage="Move in Date is required." ValidationGroup="grpFormSubmit" />
                    </div>
                </div>
            </div>
            <%-- <div class="container-fluid">
                <div class="row">--%>
            <div runat="server" id="divSMSNotification" class=" section collapsed" style="height: auto !important; padding: 0px !important;">
                <%--<div class='row'>
                            <div class='col-md-12 colps'>--%>
                <div id="divNotificationHeader" runat="server" tabindex="0" class=" mhdr colps" onclick="CollapseExpandSMSNotificationPanel(this);">
                    <h2 class="header-color" tabindex="0">Text Message Notifications</h2>
                </div>
                <%--</div>
                        </div>--%>
                <div class="content" id="divContentNotification" runat="server" style="display: none; overflow-x: hidden !important;">
                    <div id="Div2" class='row' runat="server">
                        <div class='form-group col-md-3'>
                            <label id="lblCellularPhoneNumber" class='cptn'>Cellular Phone Number</label><span class="reqd" runat="server" id="spnPhoneNumberReq">*</span>
                            <infs:WclMaskedTextBox runat="server" EnableAriaSupport="true" aria-labelledby="lblCellularPhoneNumber" ID="txtPhoneNumber" Mask="(###)-###-####" Width="100%"
                                CssClass="form-control">
                            </infs:WclMaskedTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvPhoneNumber" ControlToValidate="txtPhoneNumber"
                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Cellular Phone Number is required."
                                    ValidationGroup="grpFormSubmit" />
                                <asp:RegularExpressionValidator Display="Dynamic" ID="revCellularPhoneNumber" runat="server"
                                    ValidationGroup="grpFormSubmit"
                                    CssClass="errmsg" ErrorMessage="Format is (###)-###-####" ControlToValidate="txtPhoneNumber"
                                    ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
                            </div>
                        </div>
                        <div class='form-group col-md-3'>
                            <span class='cptn'>Receive Text Notification</span>
                            <asp:RadioButtonList ID="rdbTextNotification" runat="server" onclick="HideShowPhoneNumber(this)"
                                RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes &nbsp;" Value="True"></asp:ListItem>
                                <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:HiddenField ID="hdnIsConfirmMsgVisible" Value="0" runat="server" />
                        </div>
                        <%-- <div id="divHideShowPhoneNumber" runat="server" style="display: none">
                        </div>--%>
                    </div>
                    <%-- <div id="divConfirmationStatus" runat="server" class='row' style="display: none">
                        <div class='form-group col-md-3'>
                            <span class='cptn'>Confirmation Status</span>
                            <asp:Label ID="lblConfirmationStatus" runat="server" Width="100%" CssClass="form-control"></asp:Label>
                            <%--<asp:LinkButton Text="Re-Send Subscription Message" runat="server" ID="lnkReSendSubMessage" />
                        </div>
                    </div>--%>
                </div>
            </div>
            <%-- </div>
            </div>--%>

            <div runat="server" id="dvResHistory">
                <div class='row'>
                    <div class='col-md-12'>
                        <h2 class="header-color" tabindex="0">Residential History</h2>
                    </div>
                </div>
                <uc:PrevResident ID="PrevResident" runat="server" class="PrevResident" IsEditProfile="true" />
            </div>

            <div runat="server" id="divProfileNotes" visible="false">
                <div class='row'>
                    <div class='col-md-12'>
                        <h2 class="header-color" tabindex="0">Portfolio Notes</h2>
                    </div>
                </div>
                <uc:ApplicantProfileNotes ID="ucApplicantNotes" runat="server" Visible="False"></uc:ApplicantProfileNotes>
            </div>
            <div class="row">&nbsp;</div>
        </div>
    </asp:Panel>
</div>

<%--<table border="0" style="width: 100%;" cellpadding="0" cellspacing="0">
    <tr>--%>
<%-- <td id="dvProfilePic" runat="server">
                      <div class="col-md-3">
                    <div class="">
                        <asp:Image runat="server" ID="imgCntrl" class="thumb" />
                        <asp:Label runat="server" ID="lblNameInitials" Visible="false" class="initName" />
                    </div>
                    <div class="" title="Click this button to change your profile picture">
                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                            OnClientFileUploaded="onFileUploaded" MultipleFileSelection="Disabled" MaxFileInputsCount="1"
                            AllowedFileExtensions=".jpg,.jpeg,.tiff,.bmp,.bitmap,.png,.JPG,.PNG,.BITMAP,.JPEG,.TIFF,.BMP" OnClientFileSelected="onClientFileSelected"
                            Localization-Select="Change" Width="80px" OnClientValidationFailed="upl_OnClientValidationFailed" />
                        <div style="display: none">
                            <infs:WclButton ID="btnUpload" runat="server" OnClick="btnUpload_Click">
                            </infs:WclButton>
                        </div>
                    </div> </div>
                </td>--%>
<%-- <td>
            <div class="row">
                <div class="col-md-9">
                    <h2 class="header-color">Account Information</h2>
                </div>
            </div>
            <div style="display: none;">--%>

<%--<div class="row">
                        <div id="dvAccountInfo" class='row'>
                            <div class='col-md-12'>
                                <div class='form-group col-md-3'>
                                    <span class='cptn'>Username</span><span class="reqd">*</span>
                                    <infs:WclTextBox runat="server" ID="txtUsername" MaxLength="256" ReadOnly="true" Width="100%" CssClass="form-control">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <span id="spnUserNameErrorMsg" class="errmsg">
                                            <asp:Label ID="lblUserNameMessage" runat="server" IsValidated="0" CssClass="errmsg" Text=""></asp:Label>
                                        </span>
                                    </div>
                                </div>
                                <div class='form-group col-md-3'>
                                    <infs:WclButton runat="server" ID="btnCheckUsername" Text="Check" Skin="Windows7" AutoSkinMode="false" CausesValidation="true" ValidationGroup="grpFormSubmit"
                                        Visible="false">
                                    </infs:WclButton>
                                </div>
                            </div>
                            <div class='col-md-12'>
                                <div class='form-group col-md-3'>
                                    <span class='cptn'>Institute</span>
                                    <infs:WclTextBox runat="server" ID="txtOrganization" MaxLength="50" ReadOnly="true" Width="100%" CssClass="form-control">
                                    </infs:WclTextBox>
                                </div>
                                <div class='form-group col-md-3' id="divChangePwd" runat="server" visible="false">
                                    <span class='cptn'>Change Password</span>
                                    <asp:HyperLink ID="lnkChangePassword" runat="server" Visible="false" CssClass="user" Text="Change Password"> </asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-9">
                            <h2 class="header-color">Personal Information</h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class='form-group col-md-3'>
                                <span class='cptn'>First Name</span><span class="reqd">*</span>
                                <infs:WclTextBox runat="server" ID="txtFirstName" MaxLength="50" Width="100%" CssClass="form-control">
                                </infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="First Name is required." ValidationGroup="grpFormSubmit" />
                                    <asp:RegularExpressionValidator runat="server" ID="revFirstName" ControlToValidate="txtFirstName"
                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                                        ErrorMessage="Invalid character(s)." ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                            <div class='form-group col-md-3' title='If you do not have a middle name, leave this field blank'>
                                <span class='cptn'>Middle Name</span>
                                <infs:WclTextBox runat="server" ID="txtMiddleName" MaxLength="50" Width="100%" CssClass="form-control">
                                </infs:WclTextBox>
                            </div>
                            <div class='form-group col-md-3'>
                                <span class='cptn'>Last Name</span><span class="reqd">*</span>
                                <infs:WclTextBox runat="server" ID="txtLastName" MaxLength="50" Width="100%" CssClass="form-control">
                                </infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Last Name is required." ValidationGroup="grpFormSubmit" />
                                    <asp:RegularExpressionValidator runat="server" ID="revLastName" ControlToValidate="txtLastName"
                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                                        ErrorMessage="Invalid character(s)." ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                        </div>

                        <div class='row'>
                            <uc:PersonAlias ID="ucPersonAlias" runat="server" Visible="true" IsEditProfile="true" IsLabelMode="true"></uc:PersonAlias>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class='cptn'>Gender</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbGender" runat="server" DataTextField="GenderName" DataValueField="GenderID">
                                </infs:WclComboBox>
                            </div>
                            <div id="divDOB" runat="server">
                                <div class='sxlb'>
                                    <span class='cptn'>Date of Birth</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date">
                                    </infs:WclDatePicker>
                                    <div class="valdx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDOB" CssClass="errmsg" ControlToValidate="dpkrDOB"
                                            Display="Dynamic" ErrorMessage="Date of Birth is required." ValidationGroup="grpFormSubmit" />

                                        <asp:RangeValidator ID="rngvDOB" runat="server" ControlToValidate="dpkrDOB" Type="Date"
                                            ValidationGroup="grpFormSubmit" Display="Dynamic" CssClass="errmsg" Text="Date of birth should not be less than a year."></asp:RangeValidator>
                                    </div>
                                </div>
                            </div>
                            <div id="divSSN" runat="server">
                                <div class='sxlb' title="Enter your social security number. If you do not have an SSN, enter 111-11-1111">
                                    <span class='cptn'>Social Security Number</span><span class="reqd">*</span>
                                </div>
                                <div id="dvSSNMask" class='sxlm'>
                                    <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="###-##-####" ToolTip="If you do not have an SSN, enter 111-11-1111" />
                                    <infs:WclButton runat="server" ID="chkAutoFillSSN" OnClientCheckedChanged="AutoFillSSN" ToolTip="Check this box if you do not have an SSN" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false" Visible="true">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Value="True" />
                                            <telerik:RadButtonToggleState Value="False" />
                                        </ToggleStates>
                                    </infs:WclButton>
                                    <div class="valdx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                                            Display="Dynamic" ErrorMessage="Social Security Number is required." ValidationGroup="grpFormSubmit" />
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server"
                                            CssClass="errmsg" ErrorMessage="Full Social Security Number is required." ValidationGroup="grpFormSubmit" ControlToValidate="txtSSN"
                                            ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                                    </div>
                                </div>
                            </div>
                            <div id="divSSNMasked" visible="false" runat="server">
                                <div class='sxlb'>
                                    <span class='cptn'>Social Security Number</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox ID="txtSSNMasked" runat="server" ReadOnly="true" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <h1 class="shdr">Contact Information</h1>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>Primary Email</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtPrimaryEmail" runat="server" MaxLength="250">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtPrimaryEmail"
                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is required." ValidationGroup="grpFormSubmit" />
                                <asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ControlToValidate="txtPrimaryEmail"
                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is not valid." ValidationGroup="grpFormSubmit"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                            </div>
                        </div>
                        <div class='sxlb' title="Retype email address">
                            <span class='cptn'>Confirm Primary Email</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtConfrimPrimayEmail" runat="server" MaxLength="250">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvConfirmEmail" ControlToValidate="txtConfrimPrimayEmail"
                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Confirm Email Address is required." ValidationGroup="grpFormSubmit" />
                                <asp:CustomValidator ID="cstVConfirmPrimaryEmail" runat="server" ErrorMessage="Email Address did not match."
                                    CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail" ClientIDMode="Static" ValidationGroup="grpFormSubmit"
                                    ControlToValidate="txtConfrimPrimayEmail"></asp:CustomValidator>
                            </div>
                        </div>
                        <div class='sxlb' title="Send password recovery emails to this email address">
                            <span class='cptn'>Password Recovery Email</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtPswdRecoveryEmail" runat="server" MaxLength="250" ReadOnly="true">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb nobg'>
                        </div>
                        <div class='sxlm m3spn'>
                            <asp:CheckBox ID="chkChangeEmail" runat="server" Text="Send password recovery emails to Primary Email address also"
                                ToolTip="Select this checkbox if you would like to use your primary email address for password recovery"></asp:CheckBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb' title="Enter a second email address if you'd like to include one">
                            <span class='cptn'>Secondary Email</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtSecondaryEmail" runat="server" MaxLength="250">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RegularExpressionValidator ID="VldtxtSecondaryEmail1" runat="server" Display="Dynamic" ValidationGroup="grpFormSubmit"
                                    ErrorMessage="Email Address is not valid." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                    ControlToValidate="txtSecondaryEmail" CssClass="errmsg">
                                </asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class='sxlb' title="Retype additional email address (if including a second email address)">
                            <span class='cptn'>Confirm Secondary Email</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtConfirmSecEmail" runat="server" MaxLength="250">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RegularExpressionValidator ID="VldtxtConfirmSecEmail1" runat="server" Display="Dynamic" ValidationGroup="grpFormSubmit"
                                    ErrorMessage="Secondary Email Address did not match." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                    ControlToValidate="txtConfirmSecEmail" CssClass="errmsg">
                                </asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="cstVConfirmSecEmail" runat="server" ErrorMessage="Email Address did not match."
                                    ClientIDMode="Static" CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail"
                                    ControlToValidate="txtConfirmSecEmail"></asp:CustomValidator>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>Phone</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclMaskedTextBox ID="txtPrimaryPhone" runat="server" Mask="(###)-###-####">
                            </infs:WclMaskedTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobile" runat="server" CssClass="errmsg"
                                    ErrorMessage="Phone is required." ControlToValidate="txtPrimaryPhone" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobile" runat="server" ValidationGroup="grpFormSubmit"
                                    CssClass="errmsg" ErrorMessage="Format is (###)-###-####" ControlToValidate="txtPrimaryPhone"
                                    ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
                            </div>
                        </div>
                        <div class='sxlb' title="Enter a second phone number here if you'd like to include one">
                            <span class='cptn'>Secondary Phone</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclMaskedTextBox ID="txtSecondaryPhone" runat="server" Mask="(###)-###-####">
                            </infs:WclMaskedTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>Address 1</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclTextBox runat="server" ID="txtAddress1" MaxLength="256">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvAddress1" ControlToValidate="txtAddress1"
                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Address 1 is required." ValidationGroup="grpFormSubmit" />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn'>Address 2</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtAddress2" MaxLength="256">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <uc:Location ID="locationTenant" ZipTabIndex="6" CityTabIndex="7" runat="server" IsReverselookupControl="true"
                            NumberOfColumn="Three" ValidationGroup="grpFormSubmit" ControlsExtensionId="AEP" />
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div id="Div1" class='sxro sx3co' runat="server" visible="false">
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
                    <div runat="server" id="dvResHistory">
                        <h1 class="shdr">Residential History</h1>
                        <uc:PrevResident ID="PrevResident" runat="server" class="PrevResident" IsEditProfile="true" />
                    </div>
                    <div runat="server" id="divProfileNotes" visible="false">
                        <h1 class="shdr">Profile Note(s)</h1>
                        <uc:ApplicantProfileNotes ID="ucApplicantNotes" runat="server" Visible="False"></uc:ApplicantProfileNotes>
                    </div>--%>
<%--  </div>
        </td>
    </tr>
</table>--%>

<infsu:CustomAttribute ID="caProfileCustomAttributes" runat="server" Title="Profile Information" />



<div id="divcontent" runat="server">
    <div class="row">
        <div class='col-md-12'>
            <h2 class="header-color" tabindex="0">Custom Attributes</h2>
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlTenant">
        <asp:Repeater ID="rptrCustomAttribute" runat="server" OnItemDataBound="rptrCustomAttribute_ItemDataBound" OnPreRender="rptrCustomAttribute_PreRender"
            OnLoad="rptrCustomAttribute_Load">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <infsu:CustomAttribute ID="customAttribute" Title='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("InstitutionHierarchyLabel"))) %>'
                    MappingRecordId='<%#Eval("InstitutionNode_ID") %>'
                    ValueRecordId='<%# Eval("DPM_ID") %>' ShowUserGroupCustomAttribute="false" runat="server" />
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:Repeater>
        <%-- UAT 1438: Enhancement to allow students to select a User Group.--%>
        <div id="dvMergedUserGroup" runat="server" visible="false">
            <infsu:CustomAttribute ID="customAttribute" Visible="false" Title="User Group" runat="server" />
        </div>
    </asp:Panel>
</div>

<div id="dvUsrGrp" runat="server" style="display: none;">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">User Group
            </h2>
        </div>
    </div>
    <div id="divForm1" class="row bgLightGreen">
        <div id="pnlRows1">
            <div class="col-md-12">
                <div class="row">
                    <div id="pnlRow1">
                        <div id="pnlControlsMode1">
                            <div class="form-group col-md-3">
                                <span id="lblLabel1" class="cptn">User Group</span>
                                <div id="divControlMode">
                                    <div id="GRP_0"
                                       class="sxlm" style="width: 100%; white-space: normal;">
                                        <infs:WclComboBox ID="cmdUserGroup" runat="server" DataTextField="UG_Name" EnableCheckAllItemsCheckBox="true" ChangeTextOnKeyBoardNavigation="true" CheckBoxes="true"
                                            DataValueField="UG_ID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"  Width="100%"  Skin="Silk" AutoSkinMode="false" AllowCustomText="true"
                                            EmptyMessage="--SELECT--">
                                        </infs:WclComboBox>
                                     
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

</div>


<div id="divGraduatedStatus" runat="server" visible="false">
    <div class="row">
        <div class='col-md-12'>
            <h2 class="header-color" tabindex="0">Graduated</h2>
        </div>
    </div>
    <asp:Label ID="lblGraduatedStatus" runat="server"></asp:Label>
</div>


<asp:Button ID="btnContinue" runat="server" Style="display: none;" OnClick="fsucCmdBar1_SaveClick" />

<infsu:CommandBar ID="cBarMain" runat="server" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel"
    ButtonPosition="Center" SaveButtonText="Update" DefaultPanel="pnlEditProfile"
    OnSaveClientClick="DelayButtonClick" OnSaveClick="fsucCmdBar1_SaveClick" OnCancelClick="fsucCmdBar1_CancelClick"
    ValidationGroup="grpFormSubmit"
    UseAutoSkinMode="false" ButtonSkin="Silk" />

<div id="confirmSave" class="confirmProfileSave" runat="server" style="display: none">
    <p style="text-align: center"><%=Resources.Language.WRNGIGNRCNTNUADDALIAS %></p>

</div>

<div class='col-md-12'>
    <div class="row">
        &nbsp;
    </div>
</div>
<div class='col-md-12'>
    <div class="row">
        &nbsp;
    </div>
</div>
<div class="row bgLightGreen">

    <div id="divAccountSettings" runat="server" visible="false">

        <asp:Panel runat="server" ID="pnlMUser">
            <div class="row">
                <div class='col-md-12'>
                </div>
            </div>
            <div class='form-group col-md-3 noUnderline'>
                <span class="cptn">Active</span>
                <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />
            </div>
            <div class='form-group col-md-3'>
                <span style="margin-left: 30px;" class="cptn">Locked</span>
                <infs:WclButton runat="server" ID="chkLocked" CssClass="noUnderline" ToggleType="CheckBox" ButtonType="ToggleButton"
                    AutoPostBack="false">
                    <ToggleStates>
                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                        <telerik:RadButtonToggleState Text="No" Value="False" />
                    </ToggleStates>
                </infs:WclButton>
            </div>
            <div id="divTwoFactorAuthentication" runat="server" visible="false" class='form-group col-md-3 noUnderline'>
                <span class="cptn">Two Factor Authentication Setting</span>
                <%--<asp:RadioButtonList ID="rdbTwoFactorAuth" runat="server"
                    RepeatDirection="Horizontal">
                    <asp:ListItem Text="Enabled &nbsp;" Value="True"></asp:ListItem>
                    <asp:ListItem Text="Disabled" Value="False"></asp:ListItem>
                </asp:RadioButtonList><span id="spnVerified" runat="server"></span>
                <asp:HiddenField ID="hdnIsTwoFactorAuthenticationPrevious" runat="server" Value="False" />--%>
                <asp:RadioButtonList ID="rdbSpecifyAuthentication" runat="server" Width="350px" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Disable &nbsp;" Value="NONE"></asp:ListItem>
                    <asp:ListItem Text="Google Authenticator &nbsp;" Value="AAAA" Enabled="false"></asp:ListItem>
                    <asp:ListItem Text="Text Message" Value="AAAB" Enabled="false"></asp:ListItem>
                </asp:RadioButtonList>
                <span id="spnIsTwoFactorAuthVerified" runat="server"></span>
            </div>
        </asp:Panel>
    </div>
</div>
<infsu:CommandBar ID="fsucCmdBarPortfolio" runat="server" DefaultPanel="pnlMUser"
    ButtonPosition="Right" SaveButtonIconClass="clsSaveButton" OnSaveClick="fsucCmdBarPortfolio_SaveClick"
    AutoPostbackButtons="Save" UseAutoSkinMode="false" ButtonSkin="Silk" />



<%--</div>--%>
<asp:HiddenField ID="hdnNoMiddleNameText" runat="server" Value="" />
<asp:HiddenField ID="hdnIsCollapsed" runat="server" Value="true" />
<asp:HiddenField ID="hdnIsIntegrationSectionCollapsed" runat="server" Value="true" />
<asp:HiddenField ID="hdnIsReceiveTextNotification" runat="server" Value="false" />
<asp:HiddenField ID="hdnorganizationUserId" runat="server" Value="" />
<asp:HiddenField ID="hdncurrentLoggedInUserId" runat="server" Value="" />
<asp:HiddenField ID="hdnSMSDataAvailableForSave" runat="server" Value="false" />
<asp:HiddenField ID="hdnIsSMSDataFetchedFromDB" runat="server" Value="false" />
<asp:HiddenField ID="hdnIsLocationTenant" runat="server" Value="false" />
<asp:HiddenField ID="hdnIFYOUDONTHAVEMIDDLENAME" runat="server" Value="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>" />
