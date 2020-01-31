<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ITSUserRegistration" CodeBehind="ITSUserRegistration.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/LocationInfo.ascx" TagPrefix="infsu"
    TagName="Location" %>
<%@ Register TagPrefix="uc" TagName="PersonAlias" Src="~/Shared/Controls/PersonAliasInfo.ascx" %>
<%@ Register TagPrefix="botDetect" Namespace="BotDetect.Web.UI" Assembly="BotDetect" %>


<%@ Register TagPrefix="infsu" TagName="CustomAttribute" Src="~/ComplianceAdministration/UserControl/CustomAttributeProfileLoader.ascx" %>
<script type="text/javascript">
    if (Telerik.Web.UI.RadAsyncUpload != undefined) {
        Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
        Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
    }
</script>


<script src="https://www.google.com/recaptcha/api.js" async="async" defer="defer"></script>
<style type="text/css">
    #dvSSNMask span.RadInput {
        width: 89% !important;
    }

    #dvSSNMask .rbToggleButton {
        padding-left: 0px !important;
        vertical-align: top;
    }

    #dvSSNMask .vldx {
        margin-left: -180px !important;
    }

    #dvPrimaryPhone span.RadInput {
        width: 85% !important;
    }

    .chkIsMaskingRequired input[type="checkbox"] {
        margin-top: 4px;
        position: absolute;
        right: 0;
        top: 2px;
    }

    .HideError {
        display: none;
    }

    .ShowError {
        display: block;
    }

    #dvPrimaryPhone .vldx {
        margin-left: -180px !important;
    }

    #dvSecondaryPhone span.RadInput {
        width: 85% !important;
    }

    div#dvPrimaryPhone, div#dvSecondaryPhone {
        position: relative;
    }

    .dvValidateMessage {
        z-index: 2;
        padding: 5px;
        /*background:rgba(255, 0, 0, 0.2);*/
        background-color: #eef4fb;
        /*border: 1px solid black;*/
        border-radius: 3px;
        box-shadow: 5px 5px 2px #888888;
        overflow: auto;
        width: 200px;
        min-width: 200px;
        min-height: 40px;
        cursor: move;
    }

    #close {
        float: right;
        display: inline-block;
        padding: 2px 5px;
        background: #ccc;
    }

        #close:hover {
            float: right;
            display: inline-block;
            padding: 2px 5px;
            background: #888888;
            color: #fff;
            cursor: pointer;
        }

    .toHide {
        background-color: #fff !Important;
    }

    .buttonHidden {
        display: none;
    }

    .addAlias {
        border: 1px solid #b8b8b8;
    }

    .aliasBottmMargin {
        margin-bottom: 10px !important;
    }
</style>

<infs:WclResourceManagerProxy runat="server" ID="WclResourceManagerProxy1">
    <infs:LinkedResource Path="~/Resources/Mod/Shared/registration.css" ResourceType="StyleSheet" />
    <%--<infs:LinkedResource Path="~/App_Themes/Default/core.css" ResourceType="StyleSheet" />--%>
    <infs:LinkedResource Path="~/Resources/Mod/Shared/registration.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Applicant/EditProfile.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div style="float: right">
    <div class="dvValidateMessage" id="dvValidateMessageID" style="display: none" draggable="true" ondragend="drop(event)" ondragstart="dragstart(event)">
        <span id="close" style="border-radius: 5px;" onclick='Close()'>x</span>
        <div style="padding-left: 5px; padding-top: 5px">
            <%--<span style="font-weight: bold; font-size: 10px;">The following fields are required to complete account creation:</span>--%>
            <span style="font-weight: bold; font-size: 10px;"><%=Resources.Language.FIELDSREQ %></span>
        </div>
        <div class="vldx">
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="cmbOrganization"
                InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" Text="Organization is required." />
        </div>
        <div class="vldx">
            <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtUsername"
                Display="Dynamic" CssClass="errmsg" Text="Username is required." />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtUsername"
                Display="Dynamic" CssClass="errmsg" Text="<%$Resources:Language,USERNAMEREQ %>" />
            <%-- <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtUsername"
                CssClass="errmsg" ErrorMessage="Invalid username. Must have at least 4 chars (A-Z a-z 0-9 . _ - @)." Display="Dynamic" ValidationExpression="^[\.\@a-zA-Z0-9_-]{4,50}$" />--%>
            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtUsername"
                CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDUSERNAME %>" Display="Dynamic" ValidationExpression="^[\.\@a-zA-Z0-9_-]{4,50}$" />
        </div>
        <div class="vldx">
            <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtPassword"
                Display="Dynamic" ErrorMessage="Password is required." CssClass="errmsg" />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtPassword"
                Display="Dynamic" ErrorMessage="<%$Resources:Language,PASSWORDREQ %>" CssClass="errmsg" />
            <%--   <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ControlToValidate="txtPassword"
                CssClass="errmsg" ErrorMessage="Password invalid" Display="Dynamic"
                ValidationExpression="(?=^.{8,15}$)^(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-])[a-zA-Z0-9@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-]{8,}$" />--%>
            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ControlToValidate="txtPassword"
                CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDPASSWORD %>" Display="Dynamic"
                ValidationExpression="(?=^.{8,15}$)^(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-])[a-zA-Z0-9@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-]{8,}$" />
        </div>
        <div class="vldx">
            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtConfirmPassword"
                Display="Dynamic" ErrorMessage="Confirm Password is required." CssClass="errmsg" />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtConfirmPassword"
                Display="Dynamic" ErrorMessage="<%$Resources:Language,CNFMPASSWORDREQ %>" CssClass="errmsg" />
            <asp:CompareValidator ID="CompareValidator1" runat="server" CssClass="errmsg" ControlToCompare="txtPassword"
                ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="<%$Resources:Language,PWDDIDNOTMATCH %>"></asp:CompareValidator>
        </div>
        <div class="vldx">
            <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtFirstName"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="First Name is required." />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtFirstName"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,FIRSTNAMEREQ %>" />
            <%-- <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ControlToValidate="txtFirstName"
                Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                ErrorMessage="Invalid character(s)." />--%>
            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ControlToValidate="txtFirstName"
                Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                ErrorMessage="<%$Resources:Language,INVALIDCHARS %>" />
        </div>
        <div class="vldx">
            <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="txtMiddleName"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="Middle Name is required." />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="txtMiddleName"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,MIDDLENAMEREQ %>" />
            <%--<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator4" ControlToValidate="txtMiddleName"
                Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                ErrorMessage="Invalid character(s)." />--%>
            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator4" ControlToValidate="txtMiddleName"
                Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                ErrorMessage="<%$Resources:Language,INVALIDCHARS %>" />
        </div>
        <div class="vldx">
            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="txtLastName"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="Last Name is required." />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="txtLastName"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,LASTNAMEREQ %>" />
            <%--<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator5" ControlToValidate="txtLastName"
                Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                ErrorMessage="Invalid character(s)." />--%>
            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator5" ControlToValidate="txtLastName"
                Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\']{1,50}$"
                ErrorMessage="<%$Resources:Language,INVALIDCHARS %>" />
        </div>
        <div class="vldx">
            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="cmbGender"
                InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" Text="Gender is required." />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="cmbGender"
                InitialValue="<%$Resources:Language,SELECTWITHHYPENS%>" Display="Dynamic" CssClass="errmsg" Text="<%$Resources:Language,GENDERREQ %>" />
        </div>

        <div class="vldx">

            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" CssClass="errmsg" ControlToValidate="dpkrDOB"
                Display="Dynamic" ErrorMessage="Date of Birth is required." />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" CssClass="errmsg" ControlToValidate="dpkrDOB"
                Display="Dynamic" ErrorMessage="<%$Resources:Language,DOBREQ %>" />
            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="dpkrDOB" Type="Date"
                Display="Dynamic" CssClass="errmsg" Text=""></asp:RangeValidator>
            <asp:Label ID="lblDOBLocationErrorSummary" runat="server" CssClass="errmsg HideError" Text="<%$Resources:Language,ENTERVALIDDOB %>"></asp:Label>

        </div>

        <div class="vldx">
            <%-- <asp:RequiredFieldValidator runat="server" ID="rfvtxtSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                Display="Dynamic" ErrorMessage="Social Security Number is required." /> --%>
            <asp:RequiredFieldValidator runat="server" ID="rfvtxtSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                Display="Dynamic" ErrorMessage="<%$Resources:Language,SSNREQ %>" />
            <%--<asp:RegularExpressionValidator Display="Dynamic" ID="rgvSSN" runat="server"
                CssClass="errmsg" ErrorMessage="Full Social Security Number is required." ControlToValidate="txtSSN"
                ValidationExpression="\d{3}\-\d{2}-\d{4}" />--%>
            <asp:RegularExpressionValidator Display="Dynamic" ID="rgvSSN" runat="server"
                CssClass="errmsg" ErrorMessage="<%$Resources:Language,FULLSSNREQ %>" ControlToValidate="txtSSN"
                ValidationExpression="\d{3}\-\d{2}-\d{4}" />
            <asp:RegularExpressionValidator Display="Dynamic" ID="rgvSSNCBI" runat="server"
                CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDSSN %>" ControlToValidate="txtSSN"
                ValidationExpression="(?!9)(?!\b(\d)\1+-(\d)\1+-(\d)\1+\b)(?!000)\d{3}-(?!00)\d{2}-(?!0{4})\d{4}" />
        </div>
        <div class="vldx">
            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" ControlToValidate="txtPrimaryEmail"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is required." />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" ControlToValidate="txtPrimaryEmail"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,EMAILADDRESSREQ %>" />
            <%--  <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator7" ControlToValidate="txtPrimaryEmail"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is not valid."
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />--%>
            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator7" ControlToValidate="txtPrimaryEmail"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,EMAILNOTVALID%>"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
        </div>
        <div class="vldx">
            <%--  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator13" ControlToValidate="txtConfrimPrimayEmail"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="Confirm Email Address is required." />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator13" ControlToValidate="txtConfrimPrimayEmail"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,CNFMEMAILADDRESSREQ %>" />
            <%--<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Email Address did not match."
                CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail" ClientIDMode="Static"
                ControlToValidate="txtConfrimPrimayEmail"></asp:CustomValidator>--%>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<%$Resources:Language,EMAILADDRESSNOTMATCH %>"
                CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail" ClientIDMode="Static"
                ControlToValidate="txtConfrimPrimayEmail"></asp:CustomValidator>
        </div>
        <div class="vldx">
            <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" Display="Dynamic"
                ErrorMessage="Email Address is not valid." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                ControlToValidate="txtSecondaryEmail" CssClass="errmsg">
            </asp:RegularExpressionValidator>--%>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" Display="Dynamic"
                ErrorMessage="<%$Resources:Language,EMAILNOTVALID%>" ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                ControlToValidate="txtSecondaryEmail" CssClass="errmsg">
            </asp:RegularExpressionValidator>
        </div>
        <div class="vldx">
            <%-- <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Email Address did not match."
                ClientIDMode="Static" CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail"
                ControlToValidate="txtConfirmSecEmail"></asp:CustomValidator>--%>
            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<%$Resources:Language,EMAILADDRESSNOTMATCH %>"
                ClientIDMode="Static" CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail"
                ControlToValidate="txtConfirmSecEmail"></asp:CustomValidator>
        </div>
        <div class="vldx">
            <%--  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" ControlToValidate="txtAddress1"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="Address 1 is required." />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" ControlToValidate="txtAddress1"
                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ADDRESS1REQ %>" />
            <asp:RegularExpressionValidator Display="Dynamic" Enabled="false" ID="RegularExpressionValidator6" runat="server"
                CssClass="errmsg" ErrorMessage="<%$Resources:Language,ENTERVALIDZIPASCIICODE%>" ControlToValidate="txtAddress1"
                ValidationExpression="^[\x01-\x7F]+$" />
        </div>
        <div class="vldx">
            <asp:Label ID="lblCountry" CssClass="errmsg" runat="server"></asp:Label>
        </div>
        <div class="vldx">
            <asp:Label ID="lblState" CssClass="errmsg" runat="server"></asp:Label>
        </div>
        <div class="vldx">
            <asp:Label ID="lblCity" CssClass="errmsg" runat="server"></asp:Label>
        </div>
        <div class="vldx">
            <asp:Label ID="lblCityInvalidCode" CssClass="errmsg" runat="server"></asp:Label>
        </div>
        <div class="vldx">
            <asp:Label ID="lblZipCode" CssClass="errmsg" runat="server"></asp:Label>
        </div>
        <div class="vldx">
            <asp:Label ID="lblZipAsciiCheck" CssClass="errmsg" runat="server"></asp:Label>
        </div>
        <div class="vldx">
            <asp:Label ID="lblCounty" CssClass="errmsg" runat="server"></asp:Label>
        </div>
        <div class="vldx">
            <%-- <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator15" runat="server" CssClass="errmsg"
                ErrorMessage="Phone is required." ControlToValidate="txtPrimaryPhone"></asp:RequiredFieldValidator>--%>
            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator15" runat="server" CssClass="errmsg"
                ErrorMessage="<%$Resources:Language,PHONEREQ %>" ControlToValidate="txtPrimaryPhone"></asp:RequiredFieldValidator>
            <%--  <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator9" runat="server"
                CssClass="errmsg" ErrorMessage="Format is (###)-###-####" ControlToValidate="txtPrimaryPhone"
                ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />--%>
            <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator9" runat="server"
                CssClass="errmsg" ErrorMessage="<%$Resources:Language,PHONEFORMAT %>" ControlToValidate="txtPrimaryPhone"
                ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
            <%--<asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator17" runat="server" CssClass="errmsg"
                ErrorMessage="Phone is required." ControlToValidate="txtPrimaryPhoneNonMasking"></asp:RequiredFieldValidator>--%>
            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator17" runat="server" CssClass="errmsg"
                ErrorMessage="<%$Resources:Language,PHONEREQ %>" ControlToValidate="txtPrimaryPhoneNonMasking"></asp:RequiredFieldValidator>
            <%--  <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator12" runat="server"
                CssClass="errmsg" ErrorMessage="Invalid phone number. This field only contains +, - and numbers." ControlToValidate="txtPrimaryPhoneNonMasking"
                ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />--%>
            <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator12" runat="server"
                CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDPHONE %>" ControlToValidate="txtPrimaryPhoneNonMasking"
                ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
        </div>
        <div class="vldx">
            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator16" ControlToValidate="txtCaptchaInput" CssClass="errmsg"
                Display="Dynamic" ErrorMessage="Verification Code is required." />--%>
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        </div>
    </div>
</div>
<div id="dvUserReg" runat="server">

    <%-- <div class="row dvLanguage" id="dvLanguage" runat="server">
        <div class="col-md-3">
            <infs:WclButton ID="btnLanguage" ControlName="btnLanguage" runat="server" AutoPostBack="true" Visible="true" CausesValidation="false" ButtonType="ToggleButton" ToggleType="None"
                CssClass="BtnLanguage" OnClientClicked="ManageButton" OnClick="btnLanguage_Click">
            </infs:WclButton>
        </div>
    </div>--%>
    <%-- <h1 class="page_header">Create an account
    </h1>--%>
    <h1 class="page_header"><%=Resources.Language.CRTACCOUNT%>
    </h1>
    <%--  <p class="page_ins">
        Please fill the form below to create an account. The items with <span class="reqd">*</span>
        are required.
    </p> --%>
    <p class="page_ins">
        <%=Resources.Language.USERREGTOPINFO %>
    </p>
    <div class="wrapper">
        <div class="msgbox" id="msgBox" runat="server">
            <div>
                <asp:Label ID="lblMessage" runat="server" Style="background-image: none"></asp:Label>
            </div>
        </div>
        <asp:Panel runat="server" ID="pnlRegForm">

            <%--<h2 class="subhead">Personal Information
            </h2>--%>
            <h2 class="subhead"><%=Resources.Language.PERSONALINFO %></h2>

            <div class="row">

                <div class="col" id="dvFirstName" runat="server">
                    <div class="lbl req" id="dvSpnFirstName" runat="server">
                        <%-- <span class="cptn">First Name</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.FIRSTNAME %></span><span class="reqd">*</span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtFirstName" MaxLength="50" CssClass="ValidatetionCheck">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%-- <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="First Name is required." />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,FIRSTNAMEREQ %>" />
                        <%--<asp:RegularExpressionValidator runat="server" ID="revFirstName" ControlToValidate="txtFirstName"
                            Display="Dynamic" CssClass="errmsg"
                            ErrorMessage="Invalid character(s)." />--%>
                        <asp:RegularExpressionValidator runat="server" ID="revFirstName" ControlToValidate="txtFirstName"
                            Display="Dynamic" CssClass="errmsg"
                            ErrorMessage="<%$Resources:Language,INVALIDCHARS %>" />
                    </div>
                </div>
                <div class="col" id="dvMiddleName" runat="server">
                    <div class="lbl" title="<%$Resources:Language,IFDONTHAVEMIDDLENAME%>" id="dvSpnMiddleName" runat="server">
                        <%--<span class="cptn">Middle Name</span><span class="reqd" id="spnMiddleName" runat="server">*</span>--%>
                        <span class="cptn"><%=Resources.Language.MIDDLENAME %></span><span class="reqd" id="spnMiddleName" runat="server">*</span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtMiddleName"
                        ToolTip="<%$Resources:Language, IFYOUDONTHAVEMIDDLENAME %>" PlaceHolder="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>"
                        MaxLength="50"
                        CssClass="ValidatetionCheck">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%--<asp:RequiredFieldValidator runat="server" ID="rfvMiddleName" ControlToValidate="txtMiddleName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Middle Name is required." />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvMiddleName" ControlToValidate="txtMiddleName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,MIDDLENAMEREQ %>" />
                        <%--                    <asp:RegularExpressionValidator runat="server" ID="revMiddleName" ControlToValidate="txtMiddleName"
                            Display="Dynamic" CssClass="errmsg"
                            ErrorMessage="Invalid character(s)." />--%>
                        <asp:RegularExpressionValidator runat="server" ID="revMiddleName" ControlToValidate="txtMiddleName"
                            Display="Dynamic" CssClass="errmsg"
                            ErrorMessage="<%$Resources:Language,INVALIDCHARS %>" />
                    </div>
                </div>
                <div class="col" id="dvLastName" runat="server">
                    <div class="lbl req" id="dvSpnLastName" runat="server">
                        <%--<span class="cptn">Last Name</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.LASTNAME%></span><span class="reqd">*</span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtLastName" MaxLength="50" CssClass="ValidatetionCheck">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%--<asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Last Name is required." />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,LASTNAMEREQ %>" />
                        <%--       <asp:RegularExpressionValidator runat="server" ID="revLastName" ControlToValidate="txtLastName"
                            Display="Dynamic" CssClass="errmsg"
                            ErrorMessage="Invalid character(s)." />--%>
                        <asp:RegularExpressionValidator runat="server" ID="revLastName" ControlToValidate="txtLastName"
                            Display="Dynamic" CssClass="errmsg"
                            ErrorMessage="<%$Resources:Language,INVALIDCHARS %>" />
                        <%--ValidationExpression="^[\w\d\s\-\.\']{1,50}$"--%>
                    </div>
                </div>
                <div class="col" id="dvSuffix" runat="server" style="display: none; width: 15%">
                    <infs:WclComboBox ID="cmbSuffix" Visible="false" runat="server" DataTextField="Suffix" DataValueField="SuffixID"></infs:WclComboBox>
                    <infs:WclTextBox runat="server" ID="txtSuffix" MaxLength="10" ToolTip="<%$Resources:Language, ENTERSUFFIXIFAPPLICABLE %>" PlaceHolder="<%$Resources:Language, ENTERSUFFIXIFAPPLICABLE %>">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <asp:RegularExpressionValidator runat="server" ID="revSuffix" ControlToValidate="txtSuffix"
                            Display="Dynamic" CssClass="errmsg"
                            ErrorMessage="<%$Resources:Language,INVALIDCHARS %>" />
                    </div>
                </div>

            </div>
            <div class="row">
                <div class="col">
                    <div class="lbl" style="background-color: #fff !important"></div>
                </div>
                <div class="sxlm nobg">
                </div>
                <div class="col">
                    <infs:WclCheckBox runat="server" ID="chkMiddleNameRequired" onclick="MiddleNameEnableDisable(this)"></infs:WclCheckBox>
                    <%--<asp:Label ID="lblChkMiddleName" runat="server">I don't have a Middle Name.</asp:Label>--%>
                    <asp:Label ID="lblChkMiddleName" Style="color: red; font-weight: bold" runat="server"><%=Resources.Language.ISMIDDLENAME %></asp:Label>
                </div>
                <div class='sxroend'>
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <div class="lbl req">
                        <%--<span class="cptn">Do you have an SSN?</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.ISSSN %></span><span class="reqd">*</span>
                    </div>
                    <div class="sxlm">
                        <asp:RadioButtonList ID="rblSSN" runat="server" RepeatDirection="Horizontal" AutoPostBack="false" onChange="ManageSSNValue(this);">
                            <%--  <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>--%>
                            <asp:ListItem Text="<%$Resources:Language,LSTITMY %> &nbsp;" Value="true" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Language,LSTITMN %>" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                        <div class="vldx">
                        </div>
                    </div>
                </div>
                <div class="col" runat="server" id="divSSN">
                    <div class="lbl req">
                        <%-- <span class="cptn">Social Security Number</span><span class="reqd" id="spnSSN" runat="server">*</span>--%>
                        <span class="cptn"><%=Resources.Language.SSN %></span><span class="reqd" id="spnSSN" runat="server">*</span>
                    </div>
                    <div id="dvSSNMask" class="sxlm">
                        <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="###-##-####" AutoPostBack="false" CssClass="ValidatetionCheck" />
                        <div id="dvSSNValidator" class="vldx" style="padding-top: 18px;">
                            <%--   <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                                Display="Dynamic" ErrorMessage="Social Security Number is required." />--%>
                            <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                                Display="Dynamic" ErrorMessage="<%$Resources:Language,SSNREQ %>" />
                            <%-- <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server"
                                CssClass="errmsg" ErrorMessage="Full Social Security Number is required." ControlToValidate="txtSSN"
                                ValidationExpression="\d{3}\-\d{2}-\d{4}" />--%>
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server"
                                CssClass="errmsg" ErrorMessage="<%$Resources:Language,FULLSSNREQ %>" ControlToValidate="txtSSN"
                                ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                            <%--  <asp:RegularExpressionValidator Display="Dynamic" ID="revSSNCBI" runat="server"
                                CssClass="errmsg" ErrorMessage="Invalid SSN." ControlToValidate="txtSSN"
                                ValidationExpression="(?!9)(?!\b(\d)\1+-(\d)\1+-(\d)\1+\b)(?!000)\d{3}-(?!00)\d{2}-(?!0{4})\d{4}" />--%>
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revSSNCBI" runat="server"
                                CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDSSN %>" ControlToValidate="txtSSN"
                                ValidationExpression="(?!9)(?!\b(\d)\1+-(\d)\1+-(\d)\1+\b)(?!000)\d{3}-(?!00)\d{2}-(?!0{4})\d{4}" />
                        </div>
                    </div>
                </div>
            </div>

            <div class='row'>
                <asp:UpdatePanel ID="udpnlPersonalAlias" HasGreyBackground="true" runat="server">
                    <ContentTemplate>
                        <uc:PersonAlias ID="ucPersonAlias" runat="server" Visible="true" IsLabelMode="true" IsUserRegistrationScreen="true"></uc:PersonAlias>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class='sxroend'>
                </div>
            </div>
            <%--<div class="row">
                <div class="col">
                    <div class="lbl" title="Enter any other names you've used">
                        <span class="cptn">Alias 1</span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtAlias1" MaxLength="20">
                    </infs:WclTextBox>
                </div>
                <div class="col">
                    <div class="lbl" title="Enter any other names you've used">
                        <span class="cptn">Alias 2</span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtAlias2" MaxLength="20">
                    </infs:WclTextBox>
                </div>
                <div class="col">
                    <div class="lbl" title="Enter any other names you've used">
                        <span class="cptn">Alias 3</span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtAlias3" MaxLength="20">
                    </infs:WclTextBox>
                </div>
            </div>--%>
            <div class="row">
                <div class="col">
                    <div class="lbl req">
                        <%--<span class="cptn">Gender</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.GENDER %></span><span class="reqd">*</span>
                    </div>
                    <%--  <infs:WclComboBox ID="cmbGender" runat="server" DataTextField="GenderName" DataValueField="GenderID" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" CssClass="ValidatetionCheck"
                        Skin="Windows7" AutoSkinMode="false">
                    </infs:WclComboBox>--%>
                    <infs:WclComboBox ID="cmbGender" runat="server" DataTextField="GenderName" DataValueField="DefaultLanguageKeyID" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" CssClass="ValidatetionCheck"
                        Skin="Windows7" AutoSkinMode="false">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <%--     <asp:RequiredFieldValidator runat="server" ID="rfvGender" ControlToValidate="cmbGender"
                            InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" Text="Gender is required." />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvGender" ControlToValidate="cmbGender"
                            InitialValue="<%$Resources:Language,SELECTWITHHYPENS%>" Display="Dynamic" CssClass="errmsg" Text="<%$Resources:Language,GENDERREQ %>" />
                    </div>
                </div>
                <div class="col">
                    <div class="lbl req">
                        <%-- <span class="cptn">Date of Birth</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.DOB %></span><span class="reqd">*</span>
                    </div>
                    <%--<infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="mm/dd/yyyy" AutoSkinMode="false" Skin="Web20" CssClass="ValidatetionCheck">
                    </infs:WclDatePicker>--%>
                    <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="<%$Resources:Language,DTINPTEMPMSG %>" AutoSkinMode="false" Skin="Web20" CssClass="ValidatetionCheck"
                        DateInput-DateFormat="MM/dd/yyyy">
                    </infs:WclDatePicker>
                    <div class="vldx">
                        <%--<asp:RequiredFieldValidator runat="server" ID="rfvDOB" CssClass="errmsg" ControlToValidate="dpkrDOB"
                            Display="Dynamic" ErrorMessage="Date of Birth is required." />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvDOB" CssClass="errmsg" ControlToValidate="dpkrDOB"
                            Display="Dynamic" ErrorMessage="<%$Resources:Language,DOBREQ %>" />

                        <asp:RangeValidator ID="rngvDOB" runat="server" ControlToValidate="dpkrDOB" Type="Date"
                            Display="Dynamic" CssClass="errmsg"></asp:RangeValidator>
                        <asp:Label ID="lblDOBLocationError" runat="server" CssClass="errmsg HideError" Text="<%$Resources:Language,ENTERVALIDDOB %>"></asp:Label>
                    </div>
                </div>
                <div class="col">
                    <asp:Panel ID="pnlCommLang" runat="server" Style="display: none" CssClass="col">
                        <div class="lbl req">
                            <span class="cptn"><%=Resources.Language.PRFRDCOMCTNLANG %></span><span class="reqd"></span>
                        </div>
                        <infs:WclComboBox ID="cmbCommLang" runat="server" DataTextField="LAN_Name"
                            DataValueField="LAN_ID">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ControlToValidate="WclComboCommLang"
                            InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" Text="communication language is required." />--%>
                        </div>
                    </asp:Panel>
                </div>

                <%-- <div class="col" runat="server" id="divSSN">
                    <div class="lbl req" title="Enter your social security number. If you do not have an SSN, enter 111-11-1111">
                        <span class="cptn">Social Security Number</span><span class="reqd">*</span>
                    </div>
                    <div id="dvSSNMask" class="sxlm">
                        <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="###-##-####" AutoPostBack="false" ToolTip="If you do not have an SSN, enter 111-11-1111" CssClass="ValidatetionCheck" />
                        <infs:WclButton runat="server" ID="chkAutoFillSSN" OnClientCheckedChanged="AutoFillSSN" ValidationGroup="grp1" ToolTip="Check this box if you do not have an SSN" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false" Visible="true">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Value="True" />
                                <telerik:RadButtonToggleState Value="False" />
                            </ToggleStates>
                        </infs:WclButton>
                        <div id="dvSSNValidator" class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                                Display="Dynamic" ErrorMessage="Social Security Number is required." />
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server"
                                CssClass="errmsg" ErrorMessage="Full Social Security Number is required." ControlToValidate="txtSSN"
                                ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                        </div>
                    </div>
                </div>--%>
            </div>
            <div class="row" style="display: none">
                <div class="col">
                    <div class="lbl">
                        <span class="cptn">Select Profile Picture</span>
                    </div>
                    <div class="sxlm">
                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" MultipleFileSelection="Disabled" Visible="false"
                            MaxFileInputsCount="1" AllowedFileExtensions=".jpg,.jpeg,.tiff,.bmp,.bitmap,.png,.JPG,.JPEG,.TIFF,.BMP,.BITMAP,.PNG"
                            Localization-Select="Browse..." CssClass="btn_upload" Skin="Windows7" AutoSkinMode="false" OnClientFileSelected="onClientFileSelected" OnClientFileUploaded="OnUploadedZeroSizeFileCheck"
                            OnClientValidationFailed="upl_OnClientValidationFailed">
                        </infs:WclAsyncUpload>
                    </div>
                </div>
            </div>

            <%-- <h2 class="subhead">Contact Information
            </h2>--%>
            <h2 class="subhead"><%=Resources.Language.CONTACTINFO %>
            </h2>
            <div class="row">
                <div class="col">
                    <div class="lbl req">
                        <%--<span class="cptn">Primary Email</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.PREMAIL %></span><span class="reqd">*</span>
                    </div>
                    <infs:WclTextBox ID="txtPrimaryEmail" runat="server" MaxLength="250" CssClass="ValidatetionCheck">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%--<asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtPrimaryEmail"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is required." />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtPrimaryEmail"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,EMAILADDRESSREQ %>" />
                        <%-- <asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ControlToValidate="txtPrimaryEmail"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is not valid."
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />--%>
                        <asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ControlToValidate="txtPrimaryEmail"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,EMAILNOTVALID%>"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                    </div>
                </div>
                <div class="col">
                    <%--<div class="lbl req" title="Retype email address">--%>
                    <div id="dvRetypeEmail" runat="server" class="lbl req">
                        <%--  <span class="cptn">Confirm Primary Email</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.CPREMAIL %></span><span class="reqd">*</span>
                    </div>
                    <infs:WclTextBox ID="txtConfrimPrimayEmail" runat="server" MaxLength="250" CssClass="ValidatetionCheck">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%--  <asp:RequiredFieldValidator runat="server" ID="rfvConfirmEmail" ControlToValidate="txtConfrimPrimayEmail"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Confirm Email Address is required." />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvConfirmEmail" ControlToValidate="txtConfrimPrimayEmail"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,CNFMEMAILADDRESSREQ %>" />
                        <%-- <asp:CompareValidator ID="cmpConfirmPrimaryEmail" ControlToCompare="txtPrimaryEmail"
                        ControlToValidate="txtConfrimPrimayEmail" runat="server" CssClass="errmsg" Display="Dynamic"
                        ErrorMessage="Email Address did not match."></asp:CompareValidator>--%>
                        <%--<asp:CustomValidator ID="cstVConfirmPrimaryEmail" runat="server" ErrorMessage="Email Address did not match."
                            CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail" ClientIDMode="Static"
                            ControlToValidate="txtConfrimPrimayEmail"></asp:CustomValidator>--%>
                        <asp:CustomValidator ID="cstVConfirmPrimaryEmail" runat="server" ErrorMessage="<%$Resources:Language,EMAILADDRESSNOTMATCH %>"
                            CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail" ClientIDMode="Static"
                            ControlToValidate="txtConfrimPrimayEmail"></asp:CustomValidator>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <%--  <div class="lbl" title="Enter a second email address if you'd like to include one">--%>
                    <div id="dvSecondEmail" runat="server" class="lbl">
                        <%-- <span class="cptn">Secondary Email</span>--%>
                        <span class="cptn"><%=Resources.Language.SECEMAIL %></span>
                    </div>
                    <infs:WclTextBox ID="txtSecondaryEmail" runat="server" MaxLength="250">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%--<asp:RegularExpressionValidator ID="VldtxtSecondaryEmail1" runat="server" Display="Dynamic"
                            ErrorMessage="Email Address is not valid." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                            ControlToValidate="txtSecondaryEmail" CssClass="errmsg">
                        </asp:RegularExpressionValidator>--%>
                        <asp:RegularExpressionValidator ID="VldtxtSecondaryEmail1" runat="server" Display="Dynamic"
                            ErrorMessage="<%$Resources:Language,EMAILNOTVALID%>" ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                            ControlToValidate="txtSecondaryEmail" CssClass="errmsg">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="col">
                    <%-- <div class="lbl" title="Retype additional email address (if including a second email address)">--%>
                    <div id="dvRetypeScndryEmail" class="lbl" runat="server">
                        <%-- <span class="cptn">Confirm Secondary Email</span>--%>
                        <span class="cptn"><%=Resources.Language.CSECEMAIL %></span>
                    </div>
                    <infs:WclTextBox ID="txtConfirmSecEmail" runat="server" MaxLength="250">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%--<asp:RegularExpressionValidator ID="VldtxtConfirmSecEmail1" runat="server" Display="Dynamic"
                        ErrorMessage="Secondary Email Address did not match." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                        ControlToValidate="txtConfirmSecEmail" CssClass="errmsg">
                    </asp:RegularExpressionValidator>--%>
                        <%-- <asp:CompareValidator ID="cmpConfirmSeEmail" ControlToCompare="txtSecondaryEmail"
                        ControlToValidate="txtConfirmSecEmail" runat="server" CssClass="errmsg" Display="Dynamic"
                        ErrorMessage="Secondary Email Address did not match."></asp:CompareValidator>--%>
                        <%-- <asp:CustomValidator ID="cstVConfirmSecEmail" runat="server" ErrorMessage="Email Address did not match."
                            ClientIDMode="Static" CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail"
                            ControlToValidate="txtConfirmSecEmail"></asp:CustomValidator>--%>  <%--Commented for Globalization--%>
                        <asp:CustomValidator ID="cstVConfirmSecEmail" runat="server" ErrorMessage="<%$Resources:Language,EMAILADDRESSNOTMATCH %>"
                            ClientIDMode="Static" CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail"
                            ControlToValidate="txtConfirmSecEmail"></asp:CustomValidator>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <div class="lbl req">
                        <%-- <asp:Label runat="server" ID="lblAddress1" class="cptn">Address 1</asp:Label><span class="reqd">*</span>--%>
                        <asp:Label runat="server" ID="lblAddress1" class="cptn"></asp:Label><span class="reqd">*</span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtAddress1" MaxLength="100" CssClass="ValidatetionCheck">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%--<asp:RequiredFieldValidator runat="server" ID="rfvAddress1" ControlToValidate="txtAddress1"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Address 1 is required." />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvAddress1" ControlToValidate="txtAddress1"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ADDRESS1REQ %>" />
                        <asp:RegularExpressionValidator Display="Dynamic" Enabled="false" ID="revAddress1" runat="server"
                            CssClass="errmsg" ErrorMessage="<%$Resources:Language,ENTERVALIDZIPASCIICODE%>" ControlToValidate="txtAddress1"
                            ValidationExpression="^[\x01-\x7F]+$" />
                    </div>
                </div>
                <div class="col" id="dvAddress2" runat="server">
                    <div class="lbl">
                        <%--<span class="cptn">Address 2</span>--%>
                        <span class="cptn"><%=Resources.Language.ADDRESS2 %></span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtAddress2" MaxLength="100">
                    </infs:WclTextBox>
                </div>
            </div>
            <div class="row">
                <div class="row loc_row">
                    <%--  <infsu:Location ID="locationTenant" runat="server" NumberOfColumn="Three" />row loc_row--%>
                    <infsu:Location ID="locationTenant" ZipTabIndex="6" CityTabIndex="7" runat="server" IsReverselookupControl="true"
                        NumberOfColumn="Three" />
                </div>
                <div class='sxroend'>
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <div class="lbl req">
                        <%--<span class="cptn">Primary Phone</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.PRPHONE %></span><span class="reqd">*</span>
                    </div>
                    <div id="dvPrimaryPhone" class="sxlm">
                        <div id="dvMasking" style="display: block" runat="server">
                            <infs:WclMaskedTextBox ID="txtPrimaryPhone" runat="server" Mask="(###)-###-####" CssClass="ValidatetionCheck">
                            </infs:WclMaskedTextBox>
                        </div>
                        <div id="dvUnmasking" style="display: none" runat="server">
                            <infs:WclTextBox ID="txtPrimaryPhoneNonMasking" runat="server" MaxLength="15" CssClass="ValidatetionCheck"></infs:WclTextBox>
                        </div>
                        <%--     <asp:CheckBox ID="chkIsMaskingRequiredPrimary" CssClass="chkIsMaskingRequired" runat="server" onclick="DisplayPhoneNumberControl(this,1)"
                            Checked="false" ToolTip="Check this box if you do not have a US number." />--%>
                        <asp:CheckBox ID="chkIsMaskingRequiredPrimary" CssClass="chkIsMaskingRequired" runat="server" onclick="DisplayPhoneNumberControl(this,1)"
                            Checked="false" ToolTip="<%$Resources:Language,NOUSNUMBER %>" />
                        <div class="vldx">
                            <%-- <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobile" runat="server" CssClass="errmsg"
                                ErrorMessage="Phone is required." ControlToValidate="txtPrimaryPhone"></asp:RequiredFieldValidator>--%>
                            <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobile" runat="server" CssClass="errmsg"
                                ErrorMessage="<%$Resources:Language,PHONEREQ %>" ControlToValidate="txtPrimaryPhone"></asp:RequiredFieldValidator>
                            <%--  <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobilePrmyNonMasking" runat="server" CssClass="errmsg"
                                ErrorMessage="Phone is required." ControlToValidate="txtPrimaryPhoneNonMasking"></asp:RequiredFieldValidator>--%>
                            <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobilePrmyNonMasking" runat="server" CssClass="errmsg"
                                ErrorMessage="<%$Resources:Language,PHONEREQ %>" ControlToValidate="txtPrimaryPhoneNonMasking"></asp:RequiredFieldValidator>
                            <%--  <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobile" runat="server"
                                CssClass="errmsg" ErrorMessage="Format is (###)-###-####" ControlToValidate="txtPrimaryPhone"
                                ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />--%>
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobile" runat="server"
                                CssClass="errmsg" ErrorMessage="<%$Resources:Language,PHONEFORMAT %>" ControlToValidate="txtPrimaryPhone"
                                ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
                            <%-- <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobilePrmyNonMasking" runat="server"
                                CssClass="errmsg" ErrorMessage="Invalid phone number. This field only contains +, - and numbers." ControlToValidate="txtPrimaryPhoneNonMasking"
                                ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />--%>
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobilePrmyNonMasking" runat="server"
                                CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDPHONE %>" ControlToValidate="txtPrimaryPhoneNonMasking"
                                ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="lbl" title="Enter a second phone number here if you'd like to include one">
                        <%--      <span class="cptn">Secondary Phone</span>--%>
                        <span class="cptn"><%=Resources.Language.SECPHONE %></span>
                    </div>
                    <div id="dvSecondaryPhone" class="sxlm">
                        <div id="dvMaskingSecondary" style="display: block" runat="server">
                            <infs:WclMaskedTextBox ID="txtSecondaryPhone" runat="server" Mask="(###) ###-####">
                            </infs:WclMaskedTextBox>
                        </div>
                        <div id="dvUnMaskingSecondary" style="display: none" runat="server">
                            <infs:WclTextBox ID="txtSecondaryPhoneNonMasking" runat="server" MaxLength="15" CssClass="ValidatetionCheck"></infs:WclTextBox>
                            <%-- <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator10" runat="server"
                                ValidationGroup="grpFormSubmit"
                                CssClass="errmsg" ErrorMessage="Invalid phone number. This field only contains +, - and numbers." ControlToValidate="txtSecondaryPhoneNonMasking"
                                ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />--%>
                            <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator10" runat="server"
                                ValidationGroup="grpFormSubmit"
                                CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDPHONE %>" ControlToValidate="txtSecondaryPhoneNonMasking"
                                ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                        </div>
                        <%-- <asp:CheckBox ID="chkIsMaskingRequiredSecondary" CssClass="chkIsMaskingRequired" runat="server" onclick="DisplayPhoneNumberControl(this,2)"
                            Checked="false" ToolTip="Check this box if you do not have a US number." />--%>
                        <asp:CheckBox ID="chkIsMaskingRequiredSecondary" CssClass="chkIsMaskingRequired" runat="server" onclick="DisplayPhoneNumberControl(this,2)"
                            Checked="false" ToolTip="<%$Resources:Language,NOUSNUMBER %>" />
                    </div>
                </div>
            </div>
            <infsu:CustomAttribute ID="caProfileCustomAttributes" runat="server" Title="Profile Information" />
            <%--<h2 class="subhead">Account Information
            </h2>--%>
            <h2 class="subhead"><%=Resources.Language.ACCOUNTINFO %>
            </h2>
            <div class="row">
                <asp:Panel ID="pnl" runat="server" Visible="false" CssClass="col">
                    <div class="lbl req">
                        <%--<span class="cptn">Institute</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.INSTITUTE %></span><span class="reqd">*</span>
                    </div>
                    <infs:WclComboBox ID="cmbOrganization" runat="server" DataTextField="TenantName" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" CssClass="ValidatetionCheck"
                        CausesValidation="false" DataValueField="TenantID" AutoPostBack="true" OnSelectedIndexChanged="cmbOrganization_SelectedIndexChanged"
                        Skin="Windows7" AutoSkinMode="false">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvOrganization" ControlToValidate="cmbOrganization"
                            InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" Text="Organization is required." />
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <div class="w_col">
                    <div class="lbl req">
                        <%--<span class="cptn">Username</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.USERNAME %></span><span class="reqd">*</span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtUsername" MaxLength="256" autocomplete="off" onkeyup="EnableDisableCheckButton();" CssClass="ValidatetionCheck">
                        <ClientEvents OnValueChanged="clearlblUserMessage" />
                    </infs:WclTextBox>&nbsp;
                   <%-- <infs:WclButton runat="server" ID="btnCheckUsername" Text="Check" OnClick="btnCheckUsername_Click"
                        Skin="Windows7" AutoSkinMode="false" CausesValidation="false">
                    </infs:WclButton>--%>
                    <infs:WclButton runat="server" ID="btnCheckUsername" Text="<%$Resources:Language,CHECK %>" OnClick="btnCheckUsername_Click"
                        Skin="Windows7" AutoSkinMode="false" CausesValidation="false">
                    </infs:WclButton>
                    <div class="vldx">
                        <%-- <asp:RequiredFieldValidator runat="server" ID="rfvUsername" ControlToValidate="txtUsername"
                            Display="Dynamic" CssClass="errmsg" Text="Username is required." />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvUsername" ControlToValidate="txtUsername"
                            Display="Dynamic" CssClass="errmsg" Text="<%$Resources:Language,USERNAMEREQ %>" />
                        <%--  <asp:RegularExpressionValidator runat="server" ID="revUserName" ControlToValidate="txtUsername"
                            CssClass="errmsg" ErrorMessage="Invalid username. Must have at least 4 chars (A-Z a-z 0-9 . _ - @)." Display="Dynamic" ValidationExpression="^[\.\@a-zA-Z0-9_-]{4,50}$" />--%>
                        <asp:RegularExpressionValidator runat="server" ID="revUserName" ControlToValidate="txtUsername"
                            CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDUSERNAME %>" Display="Dynamic" ValidationExpression="^[\.\@a-zA-Z0-9_-]{4,50}$" />
                        <span class="errmsg">
                            <asp:Label ID="lblUserNameMessage" runat="server" Text=""></asp:Label>
                        </span>
                    </div>
                </div>
            </div>
            <div class="row" id="dvPasswordSection" runat="server">
                <div class="col">
                    <div class="lbl req">
                        <%--  <span class="cptn">Password</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.PASSWORD %></span><span class="reqd">*</span>
                    </div>
                    <%--<asp:TextBox runat="server" ID="txtPassword" TextMode="Password" MaxLength="15" CssClass="ValidatetionCheck" AutoCompleteType="None"></asp:TextBox>--%>
                    <infs:WclTextBox runat="server" ID="txtPassword" TextMode="Password" MaxLength="15" CssClass="ValidatetionCheck"
                        autocomplete="off" ClientEvents-OnLoad="txtpwd_load" InputType="Text">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%--        <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ControlToValidate="txtPassword"
                            Display="Dynamic" ErrorMessage="Password is required." CssClass="errmsg" />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ControlToValidate="txtPassword"
                            Display="Dynamic" ErrorMessage="<%$Resources:Language,PASSWORDREQ %>" CssClass="errmsg" />
                        <%--     <asp:RegularExpressionValidator runat="server" ID="revNewPassword" ControlToValidate="txtPassword"
                            CssClass="errmsg" ErrorMessage="Password invalid" Display="Dynamic"
                            ValidationExpression="(?=^.{8,15}$)^(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-])[a-zA-Z0-9@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-]{8,}$" />--%>
                        <asp:RegularExpressionValidator runat="server" ID="revNewPassword" ControlToValidate="txtPassword"
                            CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDPASSWORD %>" Display="Dynamic"
                            ValidationExpression="(?=^.{8,15}$)^(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-])[a-zA-Z0-9@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-]{8,}$" />
                    </div>
                </div>
                <div class="col">
                    <div class="lbl req">
                        <%--  <span class="cptn">Confirm Password</span><span class="reqd">*</span>--%>
                        <span class="cptn"><%=Resources.Language.CNFMPASSWORD %></span><span class="reqd">*</span>
                    </div>
                    <infs:WclTextBox runat="server" ID="txtConfirmPassword" TextMode="Password" MaxLength="15" CssClass="ValidatetionCheck">
                    </infs:WclTextBox>
                    <div class="vldx">
                        <%--  <asp:RequiredFieldValidator runat="server" ID="rfvConfirmPassword" ControlToValidate="txtConfirmPassword"
                            Display="Dynamic" ErrorMessage="Confirm Password is required." CssClass="errmsg" />--%>
                        <asp:RequiredFieldValidator runat="server" ID="rfvConfirmPassword" ControlToValidate="txtConfirmPassword"
                            Display="Dynamic" ErrorMessage="<%$Resources:Language,CNFMPASSWORDREQ %>" CssClass="errmsg" />
                        <asp:CompareValidator ID="cmpvalCmpPassword" runat="server" CssClass="errmsg" ControlToCompare="txtPassword"
                            ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="<%$Resources:Language,PWDDIDNOTMATCH %>"></asp:CompareValidator>
                    </div>
                </div>
            </div>

            <div class="">
                <!-- BEGIN: ReCAPTCHA implementation example. -->
                <div id="recaptcha-demo" class="g-recaptcha" data-sitekey="<%= ConfigurationManager.AppSettings["GoogleRecaptchaKey"] %>" data-callback="onSuccess" data-bind="recaptcha-demo-submit"></div>
                <script>
                    var onSuccess = function (response) {
                        var errorDivs = document.getElementsByClassName("recaptcha-error");
                        if (errorDivs.length) {
                            errorDivs[0].className = "";
                        }
                        var errorMsgs = document.getElementsByClassName("recaptcha-error-message");
                        if (errorMsgs.length) {
                            errorMsgs[0].parentNode.removeChild(errorMsgs[0]);
                        }
                        //hdnIsGoogleRecaptchaVerified
                        var hdnIsGoogleRecaptchaVerified = document.getElementById("<%= hdnIsGoogleRecaptchaVerified.ClientID %>");
                        hdnIsGoogleRecaptchaVerified.value = "1";
                        var clickButton = document.getElementById("<%= btnCreateAccount.ClientID %>");
                        clickButton.click();
                    };
                </script>
                <!-- Optional noscript fallback. -->
                <!-- END: ReCAPTCHA implementation example. -->
            </div>
            <div style="display: none">
                <button id="recaptcha-demo-submit" type="button"><%=Resources.Language.SUBMIT %></button>
                <asp:Button ID="btnCreateAccount" runat="server" Text="<%$Resources:Language,CRTACCANDPROCEED %>" OnClick="fsucCmdBar1_SubmitClick" />
            </div>

            <div class="captcha">
                <%-- Commented code to implement new BotDetect Captcha --%>
                <%--<infs:WclCaptcha ID="radCpatchaPassword" runat="server" ErrorMessage="The code you entered is not valid."
                CaptchaLinkButtonText="Refresh Image" CaptchaTextBoxLabel="" ValidatedTextBoxID="txtCaptchaInput"
                EnableRefreshImage="true" CaptchaImage-TextChars="LettersAndNumbers" Display="Dynamic"
                IgnoreCase="true">
                <CaptchaImage EnableCaptchaAudio="false" LineNoise="None" RenderImageOnly="true" />
            </infs:WclCaptcha>--%>
                <%--<table border="0" cellpadding="0" cellspacing="0" class="cap_tbl">
                    <tr>
                        <td style="padding-left: 7px">
                            <span class="cptn">Please enter the Verification Code as shown in the image on the right</span><span class="reqd">*</span>
                            <infs:WclTextBox ID="txtCaptchaInput" runat="server" CssClass="ValidatetionCheck">
                            </infs:WclTextBox>
                            <div class="vldx" style="padding-left: 0px !important;">
                                <span class="errmsg">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtCaptchaInput" ControlToValidate="txtCaptchaInput"
                                        Display="Dynamic" ErrorMessage="Verification Code is required." />
                                    <asp:Label ID="lblCaptchaErrMsg" runat="server" Text=""></asp:Label>
                                </span>
                            </div>
                        </td>
                        <td>
                            <botDetect:Captcha ID="bdCaptcha" runat="server" />
                        </td>
                    </tr>
                </table>--%>
            </div>

        </asp:Panel>
        <br />
        <div style="display: none">
            <%--<button type="submit" id="invisible" data-sitekey="6LdCyyAUAAAAAM1bl_eh_rdFp0lpaImBoHPgjNcE">Submit</button>--%>
            <asp:HiddenField ID="hdnIsGoogleRecaptchaVerified" Value="0" runat="server" />
            <asp:HiddenField ID="hdnProfileConfirmSave" Value="0" runat="server" />
        </div>
        <infsu:CommandBar ID="fsucCmdBar1" runat="server" ShowAsLinkButtons="false" DisplayButtons="Submit,Cancel"
            AutoPostbackButtons="Submit,Cancel" SubmitButtonText="<%$Resources:Language,CRTACCANDPROCEED %>" CancelButtonText="<%$Resources:Language,CNCL %>"
            OnSubmitClientClick="DelayButtonClick"
            ButtonPosition="Right" DefaultPanel="pnlRegForm" OnSubmitClick="fsucCmdBar1_SubmitClick"
            OnCancelClick="fsucCmdBar1_CancelClick" />
        <br />
    </div>
</div>
<div id="dvExistingProfiles" runat="server" visible="false">
    <h1 class="page_header"><%=Resources.Language.LINKACCOUNT %>
    </h1>
    <p class="page_ins">
        <%=Resources.Language.ALREADYEXISTINPROFILEMSG %>
    </p>
    <div class="section">
        <%--<h2 class="subhead">
            <asp:Label ID="lblExistingProfiles" runat="server" Text="Existing User Profiles"></asp:Label></h2>--%>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="pnlExistingProfiles" CssClass="sxpnl" runat="server">
                    <asp:RadioButtonList runat="server" ID="rblExistingProfiles" DataTextField="Name"
                        DataValueField="Code">
                    </asp:RadioButtonList>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvExistingProfiles" ControlToValidate="rblExistingProfiles"
                            Display="Dynamic" CssClass="errmsg" Text="<%$Resources:Language,PLSSELONEOFABOVE %>" ValidationGroup="grpExisUsers" />
                    </div>
                </asp:Panel>
            </div>
            <infsu:CommandBar ID="cbExistingProfiles" runat="server" ShowAsLinkButtons="false"
                ValidationGroup="grpExisUsers" DisplayButtons="Submit" AutoPostbackButtons="Submit"
                ButtonPosition="Right" DefaultPanel="pnlExistingProfiles" SubmitButtonText="<%$Resources:Language,PROCEED %>"
                OnSubmitClick="cbExistingProfiles_SubmitClick" />
            <br />
        </div>
    </div>
</div>
<div id="dvPswdVerification" runat="server" visible="false">
    <h1 class="page_header"><%=Resources.Language.LINKACCOUNT %>
    </h1>
    <asp:Panel ID="pnlLogin" runat="server">
        <h2 class="subhead"><%=Resources.Language.PSWDVERIFICATION %></h2>
        <div class="row">
            <div class="col">
                <asp:Label ID="lblUserName" runat="server" Text="<%$Resources:Language,USERNAME %>" AssociatedControlID="txtUserName"
                    CssClass="lbl cptn"></asp:Label>
                <infs:WclTextBox ID="txtLoginUserName" runat="server" Enabled="false" />
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:Label ID="lblPassword" runat="server" Text="<%$Resources:Language,PASSWORD %>" AssociatedControlID="txtPassword"
                    CssClass="lbl cptn"></asp:Label>
                <infs:WclTextBox ID="txtLoginPassword" runat="server" TextMode="Password" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                    Display="None" ForeColor="#FF0000" ValidationGroup="grpLogin"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ID="revPassword" ValidationGroup="grpLogin"
                    ControlToValidate="txtPassword" Display="None" ForeColor="#FF0000" ValidationExpression="^[^%^</^>]*$" />
            </div>
        </div>
        <infsu:CommandBar ID="cbPswdVerification" runat="server" ShowAsLinkButtons="false"
            DisplayButtons="Submit,Save,Cancel" AutoPostbackButtons="Submit,Save,Cancel"
            SubmitButtonText="Submit" SaveButtonText="Create New" ButtonPosition="Right"
            OnSaveClick="cbPswdVerification_SaveClick" DefaultPanel="pnlLogin" OnSubmitClick="cbPswdVerification_SubmitClick"
            OnCancelClick="cbPswdVerification_CancelClick" ValidationGroup="grpLogin" />
        <div class="reset">
        </div>
    </asp:Panel>
    <br />
    <div class="login-errors">
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="spnEr"></asp:Label>
        <asp:ValidationSummary ID="valSumLogin" DisplayMode="List" ValidationGroup="grpLogin"
            runat="server" />
    </div>
</div>
<div id="dvShibbonethErrorMessage" style="display: none" draggable="true" runat="server">
    <h1>Complio couldn't identify your account, please contact us at support@americandatabank.com or 800-200-0853.</h1>
</div>
<div id="dvShibbonethRoleErrorMessage" style="display: none" draggable="true" runat="server">
    <h1>Complio doesn't recognize your role, please contact us at support@americandatabank.com or 800-200-0853.</h1>
</div>
<div id="dvShibbonethConfirmation" style="display: none" runat="server">
    <asp:Panel ID="pnlShibbonethConfirmation" CssClass="sxpnl" runat="server">
        <h1 class="page_header">Do you have an account with Complio?</h1>

        <p id="paraText" runat="server">If you have an account in Complio then please click on “Yes” button, if you don’t have an account with Complio then please click on “Create an Account” button.</p>

    </asp:Panel>
    <infsu:CommandBar ID="cbShibbonethConfirmation" runat="server" ShowAsLinkButtons="false"
        ValidationGroup="grpExisUsers" DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
        ButtonPosition="Right" DefaultPanel="pnlShibbonethConfirmation" SubmitButtonText="Yes" CancelButtonText="Create an Account"
        SubmitButtonIconClass="rbOk" CancelButtonIconClass="rbNext" OnSubmitClick="cbShibbonethConfirmation_SubmitClick"
        OnCancelClick="cbShibbonethConfirmation_CancelClick" />
</div>
<infs:WclToolTip runat="server" ID="tltpCatExplanation" TargetControlID="txtPassword"
    Width="380px" ShowEvent="OnFocus" ManualClose="true" RelativeTo="Element" Position="BottomCenter"
    ClientKey="pwdTip" Skin="Windows7" AutoSkinMode="false" EnableShadow="true">
    <div class="pwd_hint">
        <%--<span style="font-weight: bold">Password should meet the following criteria-</span>--%>
        <span style="font-weight: bold"><%=Resources.Language.PASSWORDCRITERIA %></span>
        <ul>
            <%--  <li class="white yes">Should not have blank spaces</li>--%>
            <li class="white yes"><%=Resources.Language.NOBLANKSPACE %></li>
            <%--<li class="digit no">Should have at least one digit [0-9]</li>--%>
            <li class="digit no"><%=Resources.Language.ATLEASTONEDIGIT %></li>
            <%-- <li class="char no">Should have at least one capital letter [A-Z]</li>--%>
            <li class="char no"><%=Resources.Language.ATLEASTONECAPLETTER %></li>
            <%--<li class="sym no">Should have at least one special character [@#$%^_+~!?\':/,(){}[]-]</li>--%>
            <li id="specialchar" runat="server" class="sym no"><%=Resources.Language.ATLEASTONESPECIALCHAR %></li>
            <li id="specialcharcbi" runat="server" class="sym no"><%=Resources.Language.ATLEASTONESPECIALCHARCBI %></li>
            <%-- <li class="len no">Should have 8 to 15 characters.</li>--%>
            <li class="len no"><%=Resources.Language.PSWDCHARRANGE %></li>
        </ul>
    </div>
</infs:WclToolTip>
<asp:HiddenField ID="hdnSSN" runat="server" Value="" />
<asp:HiddenField ID="hdnNoMiddleNameText" runat="server" Value="" />
<asp:HiddenField ID="hdnShowError" runat="server" Value="true" />
<asp:HiddenField ID="hdnPassword" runat="server" />
<asp:HiddenField ID="hdnConfirmPassword" runat="server" />
<asp:HiddenField ID="hdnIsLocationTenant" runat="server" />
<asp:Button CssClass="buttonHidden" ID="btnDoPostBack" runat="server" OnClick="btnDoPostBack_Click" CausesValidation="false" />

<asp:HiddenField ID="hdnOneYear" runat="server" Value="<%$Resources:Language,DOBNOTLESSTHANYEAR %>" />
<asp:HiddenField ID="hdn10Year" runat="server" Value="<%$Resources:Language,DOBNOTLESSTHAT10YEAR %>" />

<asp:HiddenField ID="hdnStateReq" runat="server" Value="<%$Resources:Language,STATEREQ %>" />
<asp:HiddenField ID="hdnPlsSelState" runat="server" Value="<%$Resources:Language,PLSSELSTATE %>" />
<asp:HiddenField ID="hdnPlsSelCity" runat="server" Value="<%$Resources:Language,PLSSELCITY %>" />
<asp:HiddenField ID="HdnCityReq" runat="server" Value="<%$Resources:Language,CITYREQ %>" />
<asp:HiddenField ID="hdnCityInvalidCode" runat="server" Value="<%$Resources:Language,CITYINVALIDASCIICODE %>" />

<asp:HiddenField ID="hdnPlsSelZipCode" runat="server" Value="<%$Resources:Language,PLSSELZIPCODE %>" />
<asp:HiddenField ID="hdnZipCodeReq" runat="server" Value="<%$Resources:Language,ZIPCODEREQ %>" />
<asp:HiddenField ID="hdnPlsdSelCounty" runat="server" Value="<%$Resources:Language,PLSSELCOUNTY %>" />

<asp:HiddenField ID="hdnIFYOUDONTHAVEMIDDLENAME" runat="server" Value="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>" />
<%--<asp:HiddenField ID="hdnLanguageCode" runat="server" />--%>

<div id="confirmSave" class="confirmProfileSave" runat="server" style="display: none">
    <p style="text-align: center"><%=Resources.Language.WRNGIGNRCNTNUADDALIAS %></p>

</div>
<script type="text/javascript">

    function ConfirmSubmit(s, e) {
       
        var IsLocationTenant = $jQuery("[id$=hdnIsLocationTenant]").val();
        var txtNewFirstName = $jQuery("[id$=txtNewFirstName]").val();
        var txtNewLastName = $jQuery("[id$=txtNewLastName]").val();
        var txtNewMiddleName = $jQuery("[id$=txtNewMiddleName]").val();
        // var txtNewSuffix = $jQuery("[id$=txtAliasNewSuffix]").val();
        // var cmbAliasNewSuffix = $jQuery("[id$=cmbAliasNewSuffix]").val();

        //   if (txtNewSuffix == 'Enter Suffix' || txtNewSuffix == 'Ingrese un sufijo') { txtNewSuffix = '' }

        if (IsLocationTenant.toLowerCase() == "true" && Page_IsValid && txtNewLastName != undefined && txtNewFirstName != undefined && txtNewMiddleName != undefined &&
            (txtNewFirstName.trim() != '' || txtNewMiddleName.trim() != '' || txtNewLastName.trim() != ''))// {|| cmbAliasNewSuffix.trim().length > 0))//txtNewSuffix.trim().length > 0))
        {
            $window.showDialog($jQuery(".confirmProfileSave").clone().show()
                , {
                    approvebtn: {
                        autoclose: true, text: "Ignore and Continue", click: function (s, e) {
                            $jQuery("[id$=txtNewFirstName]").val('');
                            $jQuery("[id$=txtNewLastName]").val('');
                            $jQuery("[id$=txtNewMiddleName]").val('');
                            $jQuery("[id$=cmbAliasNewSuffix]").val('');
                            window.setTimeout(function () {
                                checkCaptcha(s, e);
                                s.set_autoPostBack(true);
                                if (!openGoogleRecaptcha) {
                                    s.click();
                                }
                                submitclicked = false;
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

    $jQuery(document).ready(function () {
        EnableDisableCheckButton();
        //to avoid Postback until button is disabled
        $jQuery("[id$=btnCheckUsername]").click(function (e) {
            if ($jQuery("[id$=btnCheckUsername]").prop("disabled") == true) {
                e.preventDefault();
            }
        });

        //restrict user to copy Email Address in Confirm Primary Email
        $jQuery("[id$=txtConfrimPrimayEmail]").bind("paste", function (e) {
            e.preventDefault();
        });

        $jQuery("[id$=txtConfirmSecEmail]").bind("paste", function (e) {
            e.preventDefault();
        });

        //btnOpenCaptcha
        //$jQuery("[id$=btnOpenCaptcha]").bind("click", function (e) {
        //    e.preventDefault();
        //    //

        //});
        //UAT-1848
        $jQuery(".ValidatetionCheck").each(function (i) {
            $jQuery(this).on("change", ShowHideValidateSummary);
        });
        //var dvValidateMessage = $jQuery("[id$=dvValidateMessageID]")[0];
        //dvValidateMessage.addEventListener("Change", function () {
        //    ShowHideValidateSummary();
        //}, true);
        SetPasswordValue();
        var IslocationTenant = $jQuery("#<%= hdnIsLocationTenant.ClientID%>");
        var rgvSSNCBI = $jQuery("#<%= rgvSSNCBI.ClientID %>");
        var revSSNCBI = $jQuery("#<%= revSSNCBI.ClientID %>");
        if (IslocationTenant[0].value == "True") {
            //$jQuery("[id$=dpkrDOB_dateInput").attr("disabled", true);
            dateValidator();
            var txtSSN = $jQuery("#<%= txtSSN.ClientID %>");
                var rgvSSN = $jQuery("#<%= rgvSSN.ClientID %>");
                var revtxtSSN = $jQuery("#<%= revtxtSSN.ClientID %>");
                ValidatorEnable(rgvSSN[0], false);
                ValidatorEnable(revtxtSSN[0], false);
                var id = "#<%= rblSSN.ClientID %>";
            var rbtnID = id + " input[type=radio]:checked";
            selectedValue = $jQuery(rbtnID)[0].value;
            if (selectedValue == "false" || selectedValue == "False") {
                $jQuery("[id$=divSSN]")[0].style.display = "none";
                $find(txtSSN[0].id).set_value('');
                ValidatorEnable(rgvSSNCBI[0], false);
                ValidatorEnable(revSSNCBI[0], false);
            }
        }
        else {
            var _doNotLessThanYear = $jQuery("[id$=hdnOneYear]").val();
            //$jQuery("[id$=DefaultContent_ctl00_RangeValidator1]")[0].textContent = "Date of birth should not be less than a year.";
            $jQuery("[id$=DefaultContent_ctl00_RangeValidator1]")[0].textContent = _doNotLessThanYear;
            ValidatorEnable(rgvSSNCBI[0], false);
            ValidatorEnable(revSSNCBI[0], false);
        }
        CheckDOBForLocationTenant();
    });

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
                        $jQuery("[id$=lblDOBLocationErrorSummary]").removeClass("HideError");
                        $jQuery("[id$=lblDOBLocationError]").removeClass("HideError");
                        $jQuery("[id$=lblDOBLocationErrorSummary]").addClass("ShowError");
                        $jQuery("[id$=lblDOBLocationError]").addClass("ShowError");
                        return null;
                    }
                    else {
                        $jQuery("[id$=lblDOBLocationErrorSummary]").removeClass("ShowError");
                        $jQuery("[id$=lblDOBLocationError]").removeClass("ShowError");
                        $jQuery("[id$=lblDOBLocationErrorSummary]").addClass("HideError");
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


    function dateValidator() {
        var date = new Date();
        var day = date.getDate();
        var month = date.getMonth() + 1;
        if (month < 10) month = "0" + month;
        if (day < 10) day = "0" + day;
        var rangevalidator = $jQuery("[id$=DefaultContent_ctl00_RangeValidator1]")[0];
        rangevalidator.maximumvalue = month + "/" + day + "/" + (date.getFullYear() - 10);
        //$jQuery("[id$=DefaultContent_ctl00_RangeValidator1]")[0].textContent = "Date of birth should not be less than 10 year.";
        var _doNotLessThan10Year = $jQuery("[id$=hdn10Year]").val();
        $jQuery("[id$=DefaultContent_ctl00_RangeValidator1]")[0].textContent = _doNotLessThan10Year;
    }

    function SetPasswordValue() {
        var txtPassword = $jQuery("[id$=txtPassword]");
        var txtConfirmPassword = $jQuery("[id$=txtConfirmPassword]");
        var value = $jQuery("[id$=hdnPassword]").val()
        var cnfrmValue = $jQuery("[id$=hdnConfirmPassword]").val()
        if (value != null || value != '')
            $jQuery("[id$=txtPassword]")[0].value = value;
        if (cnfrmValue != null || cnfrmValue != '')
            $jQuery("[id$=txtConfirmPassword]")[0].value = cnfrmValue;
    }
    //button will hide untill follow the client side validation
    function EnableDisableCheckButton() {
        var userNameText = $jQuery("[id$=txtUsername]").val();
        if (userNameText != undefined && userNameText != "") {
            if (userNameText.length != "undefined" && userNameText.length > 0) {
                if ($jQuery.trim(userNameText).length > 3 && validate(userNameText)) {
                    $jQuery("[id$=btnCheckUsername]").prop("disabled", false);
                }
                else {
                    $jQuery("[id$=btnCheckUsername]").prop('disabled', true);
                    $jQuery("[id$=lblUserNameMessage]").hide();
                }
            }
        }
        else {
            $jQuery("[id$=btnCheckUsername]").prop('disabled', true);
        }
    }

    function Key(event) {
        //alert("KeyCode :" + event.keyCode);
        var data = $jQuery("[id$=cmbOrganization]");
    }



    //to check the expression
    function validate(userNameText) {
        var filter = /^[\.\@a-zA-Z0-9_-]{4,50}$/;
        if (filter.test(userNameText)) {
            return true;
        }
    }

    //UAT 726
    function openCmbBoxOnTab(sender, e) {
        if (!sender.get_dropDownVisible()) sender.showDropDown();
    }

    function recaptcha_callback() {
        alert("callback working");
        $('.button').prop("disabled", false);
    }
    var submitclicked = false;
    var openGoogleRecaptcha = false;
    function DelayButtonClick(s, e) {
        if (submitclicked == false) {
            submitclicked = true;
            s.set_autoPostBack(false);
            validateLocationControls();
            ShowHideValidateSummary(true);
            if (ConfirmSubmit(s, e)) {
                submitclicked = false;
                return;
            }
            else {
                window.setTimeout(function () {
                    checkCaptcha(s, e);
                    s.set_autoPostBack(true);
                    if (!openGoogleRecaptcha) {
                        s.click();
                    }
                    submitclicked = false;
                }, 200, s);
            }
        }
    }

    function checkCaptcha(s, e) {
        //debugger;
        if (Page_IsValid) {
            $jQuery("[id$=recaptcha-demo-submit]")[0].click();
            s.set_autoPostBack(false);
            openGoogleRecaptcha = true;
        }
    }

    //UAT-1848
    function drop(event) {
        var sourceelmt = event.srcElement;
        if (sourceelmt == undefined) {
            sourceelmt = event.originalTarget;
        }
        $jQuery(sourceelmt).css({ top: event.screenY + ycal, left: event.screenX + xcal, position: 'absolute' });
    }
    var xcal = 0;
    var ycal = 0;
    function dragstart(event) {
        var sourceelmt = event.srcElement;
        if (sourceelmt == undefined) {
            event.dataTransfer.setData('text/plain', '');
            sourceelmt = event.currentTarget;
        }
        xcal = sourceelmt.offsetLeft - event.screenX;
        ycal = sourceelmt.offsetTop - event.screenY;
    }
    function Close() {
        $jQuery("[id$=close]")[0].parentElement.style.display = "none";
    }
    function validateLocationControls() {
        IsValidationSummaryAllow = true;
        var cmb_State = $jQuery("[id$=cmbRSL_State]")[0];
        var txt_State = $jQuery("[id$=txtRSL_State]")[0];

        var cmb_City = $jQuery("[id$=cmbRSL_City]")[0];
        var txt_City = $jQuery("[id$=txtRSL_City]")[0];

        var cmb_ZipId = $jQuery("[id$=cmbRSL_ZipId]")[0];
        var txt_ZipId = $jQuery("[id$=txtRSL_ZipId]")[0];

        var cmb_County = $jQuery("[id$=cmbRSL_County]")[0];

        //UAT-3910
        var cmb_country_LocationSpecific = $jQuery("[id$=cmbCountrylocationSpecific]")[0];
        var cmb_state_LocationSpecific = $jQuery("[id$=cmbStateLocationSpecific]")[0];
        var dv_state_LocationSpecific = $jQuery("[id$=dvStateLocationSpecificTenant]")[0];
        var txt_city_LocationSpecific = $jQuery("[id$=txtCityLocationSpecific]")[0];
        var txt_ZipCode_LocationSpecific = $jQuery("[id$=txtZipCodeLocationSpecific]")[0];
        var hdnIsLocationServiceTenant = $jQuery("[id$=hdnIsLocationServiceTenant]")[0];
        //For globalization  
        var _stateReq = $jQuery("[id$=hdnStateReq]").val();
        var _plsSelState = $jQuery("[id$=hdnPlsSelState]").val();
        var _plsSelCity = $jQuery("[id$=hdnPlsSelCity]").val();
        var _cityReq = $jQuery("[id$=HdnCityReq]").val();
        var _CityInvalidCode = $jQuery("[id$=hdnCityInvalidCode]").val();
        var _plsSelZipCode = $jQuery("[id$=hdnPlsSelZipCode]").val();
        var _zipCodeReq = $jQuery("[id$=hdnZipCodeReq]").val();
        var _plsdSelCounty = $jQuery("[id$=hdnPlsdSelCounty]").val();

        if (hdnIsLocationServiceTenant != undefined && hdnIsLocationServiceTenant.value != undefined && hdnIsLocationServiceTenant.value == "True") {
            if ((dv_state_LocationSpecific.style.display == "" || dv_state_LocationSpecific.style.display == "block" || dv_state_LocationSpecific.style.display == "inline-block")
                && (cmb_state_LocationSpecific.value == null || cmb_state_LocationSpecific.value == undefined || cmb_state_LocationSpecific.value == ""))
                //$jQuery("[id$=lblState]")[0].textContent = "Please select State.";
                $jQuery("[id$=lblState]")[0].textContent = _plsSelState;
            else
                $jQuery("[id$=lblState]")[0].textContent = "";
        }
        else {
            if ((cmb_State.style.display == "" || cmb_State.style.display == "block")
                && (cmb_State.value == null || cmb_State.value == undefined || cmb_State.value == ""))
                //$jQuery("[id$=lblState]")[0].textContent = "Please select State.";
                $jQuery("[id$=lblState]")[0].textContent = _plsSelState;
            else {
                if (($jQuery("[id$=dvTxtState]")[0].style.display == "" || $jQuery("[id$=dvTxtState]")[0].style.display == "block") && txt_State.value == "")
                    //$jQuery("[id$=lblState]")[0].textContent = "State is required.";
                    $jQuery("[id$=lblState]")[0].textContent = _stateReq;
                else
                    $jQuery("[id$=lblState]")[0].textContent = "";
            }
        }

        //UAT-3910
        if (hdnIsLocationServiceTenant != undefined && hdnIsLocationServiceTenant.value != undefined && hdnIsLocationServiceTenant.value == "True") {
            if ((txt_city_LocationSpecific.style.display == "" || txt_city_LocationSpecific.style.display == "block") && txt_city_LocationSpecific.value == "")
                //$jQuery("[id$=lblCity]")[0].textContent = "City is required.";
                $jQuery("[id$=lblCity]")[0].textContent = _cityReq;
            else
                $jQuery("[id$=lblCity]")[0].textContent = "";
        } else {
            if ((cmb_City.style.display == "" || cmb_City.style.display == "block")
                && (cmb_City.value == null || cmb_City.value == undefined || cmb_City.value == ""))
                //$jQuery("[id$=lblCity]")[0].textContent = "Please select City.";
                $jQuery("[id$=lblCity]")[0].textContent = _plsSelCity;
            else {
                if (($jQuery("[id$=dvTxtCity]")[0].style.display == "" || $jQuery("[id$=dvTxtCity]")[0].style.display == "block") && txt_City.value == "")
                    //$jQuery("[id$=lblCity]")[0].textContent = "City is required.";
                    $jQuery("[id$=lblCity]")[0].textContent = _cityReq;
                else
                    $jQuery("[id$=lblCity]")[0].textContent = "";
            }
        }


        var _enterValidZipCodeValMsg = "<%=Resources.Language.ENTERVALIDZIPCODEVALDTN %>";
        var _enterValidPostalCodeValMsg = "<%=Resources.Language.ENTERVALIDPOSTALCODEVALDTN %>";
        var rfvtxtCityLocationSpecific = $jQuery('[id$=rfvtxtCityLocationSpecific]');
        if (rfvtxtCityLocationSpecific != undefined && rfvtxtCityLocationSpecific.length > 0 && rfvtxtCityLocationSpecific[0].style.display != 'none') {
            $jQuery("[id$=lblCity]")[0].textContent = rfvtxtCityLocationSpecific[0].innerHTML;
        }




        //UAT-3910
        if (hdnIsLocationServiceTenant != undefined && hdnIsLocationServiceTenant.value != undefined && hdnIsLocationServiceTenant.value == "True") {
            if ((txt_ZipCode_LocationSpecific.style.display == "" || txt_ZipCode_LocationSpecific.style.display == "block") && txt_ZipCode_LocationSpecific.value == "") {
                if (dv_state_LocationSpecific.style.display == "" || dv_state_LocationSpecific.style.display == "block" || dv_state_LocationSpecific.style.display == "inline-block") {

                    //$jQuery("[id$=lblZipCode]")[0].textContent = "Zip Code is required.If you do not have a zip code, please enter 00000.";
                    $jQuery("[id$=lblZipCode]")[0].textContent = _enterValidZipCodeValMsg;
                }
                else {
                    //$jQuery("[id$=lblZipCode]")[0].textContent = "Postal code is required.If you do not have a postal code, please enter 00000.";
                    $jQuery("[id$=lblZipCode]")[0].textContent = _enterValidPostalCodeValMsg;
                }
            }
            else
                $jQuery("[id$=lblZipCode]")[0].textContent = "";
        } else {
            if ((cmb_ZipId.style.display == "" || cmb_ZipId.style.display == "block")
                && (cmb_ZipId.value == null || cmb_ZipId.value == undefined || cmb_ZipId.value == ""))
                //$jQuery("[id$=lblZipCode]")[0].textContent = "Please select Zip Code.";
                $jQuery("[id$=lblZipCode]")[0].textContent = _plsSelZipCode;
            else {
                if (($jQuery("[id$=dvTxtZipId]")[0].style.display == "" || $jQuery("[id$=dvTxtZipId]")[0].style.display == "block") && txt_ZipId.value == "")
                    //$jQuery("[id$=lblZipCode]")[0].textContent = "Zip Code is required.";
                    $jQuery("[id$=lblZipCode]")[0].textContent = _zipCodeReq;
                else
                    $jQuery("[id$=lblZipCode]")[0].textContent = "";
            }
        }

        var rfvtxtZipCodeLocationSpecific = $jQuery('[id$=rfvtxtZipCodeLocationSpecific]');
        if (rfvtxtZipCodeLocationSpecific != undefined && rfvtxtZipCodeLocationSpecific.length > 0 && rfvtxtZipCodeLocationSpecific[0].style.display != 'none') {
            $jQuery("[id$=lblZipCode]")[0].textContent = rfvtxtZipCodeLocationSpecific[0].innerHTML;
        }
        //UAT-3910
        if (hdnIsLocationServiceTenant != undefined && hdnIsLocationServiceTenant.value != undefined && hdnIsLocationServiceTenant.value == "True") {
            $jQuery("[id$=lblCounty]")[0].textContent = "";
        } else {
            if (($jQuery("[id$=dvCntRSLCounty]")[0].style.display == "" || $jQuery("[id$=dvCntRSLCounty]")[0].style.display == "block" || $jQuery("[id$=dvCntRSLCounty]")[0].style.display == "inline-block")
                && (cmb_County.value == null || cmb_County.value == undefined || cmb_County.value == ""))
                //$jQuery("[id$=lblCounty]")[0].textContent = "Please select County.";
                $jQuery("[id$=lblCounty]")[0].textContent = _plsdSelCounty;
            else
                $jQuery("[id$=lblCounty]")[0].textContent = "";
        }
        //UAT-3910
        if (hdnIsLocationServiceTenant != undefined && hdnIsLocationServiceTenant.value != undefined && hdnIsLocationServiceTenant.value == "True") {
            if ((cmb_country_LocationSpecific.style.display == "" || cmb_country_LocationSpecific.style.display == "block")
                && (cmb_country_LocationSpecific.value == null || cmb_country_LocationSpecific.value == undefined || cmb_country_LocationSpecific.value == ""))
                $jQuery("[id$=lblCountry]")[0].textContent = "Please select Country.";
            else
                $jQuery("[id$=lblCountry]")[0].textContent = "";
        }
    }

    function ShowHideValidateSummary(ButtonClicked) {

        var isValid = true;
        var aliasFirstNameValidator = $jQuery("[id$=rfvNewFirstName]")[0];
        var aliasLastNameValidator = $jQuery("[id$=rfvNewLastName]")[0];
        var rptAliasFirstNameValidator = $jQuery("[id$=rfvFirstName]")[0];
        var rptAliasLastNameValidator = $jQuery("[id$=rfvLastName]")[0];
        for (i = 0; i < Page_Validators.length; i++) {
            if (!Page_Validators[i].isvalid) {
                if (Page_Validators[i] != aliasFirstNameValidator
                    && Page_Validators[i] != aliasLastNameValidator
                    && Page_Validators[i] != rptAliasFirstNameValidator
                    && Page_Validators[i] != rptAliasLastNameValidator) {
                    if (!(Page_Validators[i].id.indexOf('caProfileCustomAttributes') > 0)) {
                        isValid = false;
                        break;
                    }
                }
            }
        }
        if (isValid) {
            $jQuery(".dvValidateMessage")[0].style.display = "none";
        }
        else {
            if (ButtonClicked != undefined && ButtonClicked == true) {
                $jQuery(".dvValidateMessage")[0].style.display = "block";
            }
        }
    }

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
                //Validation Summary validator's
                ValidatorEnable($jQuery("[id$=RequiredFieldValidator15]")[0], false);
                ValidatorEnable($jQuery("[id$=RegularExpressionValidator9]")[0], false);
                ValidatorEnable($jQuery("[id$=RequiredFieldValidator17]")[0], true);
                ValidatorEnable($jQuery("[id$=RegularExpressionValidator12]")[0], true);
                $jQuery("[id$=RequiredFieldValidator17]").hide();
            }
            else {
                dvUnmasking[0].style.display = "none";
                dvMasking[0].style.display = "block";
                ValidatorEnable($jQuery("[id$=revTxtMobile]")[0], true);
                ValidatorEnable($jQuery("[id$=rfvTxtMobile]")[0], true);
                ValidatorEnable($jQuery("[id$=rfvTxtMobilePrmyNonMasking]")[0], false);
                ValidatorEnable($jQuery("[id$=revTxtMobilePrmyNonMasking]")[0], false);
                $jQuery("[id$=rfvTxtMobile]").hide();
                //Validation Summary validator's
                ValidatorEnable($jQuery("[id$=RequiredFieldValidator15]")[0], true);
                ValidatorEnable($jQuery("[id$=RegularExpressionValidator9]")[0], true);
                ValidatorEnable($jQuery("[id$=RequiredFieldValidator17]")[0], false);
                ValidatorEnable($jQuery("[id$=RegularExpressionValidator12]")[0], false);
                $jQuery("[id$=RequiredFieldValidator15]").hide();
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
        ShowHideValidateSummary();
    }

    function OnKeyPress(sender, args) {
        var re = /^[0-9\-\:\b/]$/;
        args.set_cancel(!re.test(args.get_keyCharacter()));
    }

    function ManageSSNValue(rblSSN) {
        var id;
        var rblSSNId;
        var selectedValue;
        var txtSSN = $jQuery("#<%= txtSSN.ClientID %>");
        var rfvSSN = $jQuery("#<%= rfvSSN.ClientID %>");
        var rfvtxtSSN = $jQuery("#<%= rfvtxtSSN.ClientID %>")
        var rgvSSN = $jQuery("#<%= rgvSSN.ClientID %>");
        var rgvSSNCBI = $jQuery("#<%= rgvSSNCBI.ClientID %>");
        var revSSNCBI = $jQuery("#<%= revSSNCBI.ClientID %>");
        var revtxtSSN = $jQuery("#<%= revtxtSSN.ClientID %>");
        var IslocationTenant = $jQuery("#<%= hdnIsLocationTenant.ClientID%>");

        if (rblSSN != "" && rblSSN != undefined) {
            id = "[id$=" + rblSSN.id + "]";
            rblSSNId = "#" + rblSSN.id + " input[type=radio]:checked";
            selectedValue = $jQuery(rblSSNId)[0].value;
        }

        if (rfvSSN != "" && rfvSSN != undefined && rfvtxtSSN != "" && rfvtxtSSN != undefined && txtSSN != "" && txtSSN != undefined) {
            if (selectedValue == "true" || selectedValue == "True") {
                if (IslocationTenant[0].value == "True") {
                    $jQuery("[id$=divSSN]")[0].style.display = "inline-block";
                    ValidatorEnable(rgvSSNCBI[0], true);
                    ValidatorEnable(revSSNCBI[0], true);
                    ValidatorEnable(rgvSSN[0], false);
                    ValidatorEnable(revtxtSSN[0], false);
                }
                else {
                    ValidatorEnable(revtxtSSN[0], true);
                    ValidatorEnable(rgvSSN[0], true);

                }
                // txtSSN.prop("disabled", false);
                //txtSSN.removeClass("riDisabled ").addClass("riEnabled"); 
                $find(txtSSN[0].id).enable();
                if (txtSSN[0]._oldValue == '111-11-1111' || IslocationTenant[0].value == "True") {
                    $find(txtSSN[0].id).set_value('');
                }
                else {
                    txtSSN[0].value = txtSSN[0]._oldValue;
                }
                ValidatorEnable(rfvSSN[0], true);
                ValidatorEnable(rfvtxtSSN[0], true);
                $jQuery('[id$=spnSSN]').show();
                rfvSSN.hide();
            }
            else {
                if (IslocationTenant[0].value == "True") {
                    $jQuery("[id$=divSSN]")[0].style.display = "none";
                    txtSSN[0].value = "";
                }
                else {
                    txtSSN[0].value = "111-11-1111";
                    txtSSN.prop("disabled", true);
                    txtSSN.removeClass("riEnabled").addClass("riDisabled");
                }


                ValidatorEnable(rfvSSN[0], false);
                ValidatorEnable(rfvtxtSSN[0], false);
                //rgvSSN[0].disabled = true;
                //revtxtSSN[0].disabled = true;
                ValidatorEnable(rgvSSN[0], false);
                ValidatorEnable(revtxtSSN[0], false);
                ValidatorEnable(rgvSSNCBI[0], false);
                ValidatorEnable(revSSNCBI[0], false);
                $jQuery('[id$=spnSSN]').hide();
            }
        }
    }

</script>
