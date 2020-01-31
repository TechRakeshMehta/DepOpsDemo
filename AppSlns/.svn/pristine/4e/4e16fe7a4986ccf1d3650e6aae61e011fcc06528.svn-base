<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ApplicantModule.Views.EditProfile" CodeBehind="EditProfile.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/LocationInfo.ascx" TagPrefix="uc"
    TagName="Location" %>
<%@ Register TagPrefix="uc" TagName="PersonAlias" Src="~/Shared/Controls/PersonAliasInfo.ascx" %>
<%@ Register TagPrefix="uc" TagName="PrevResident" Src="~/ComplianceOperations/UserControl/PreviousAddressControl.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CustomAttribute" Src="~/ComplianceAdministration/UserControl/CustomAttributeLoader.ascx" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantProfileNotes.ascx" TagPrefix="uc"
    TagName="ApplicantProfileNotes" %>
<%@ Register TagPrefix="infsu" TagName="TwoFactorAuthentication" Src="~/CommonOperations/TwoFactorAuthenticationSettings.ascx" %>

<script type="text/javascript">
    Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
    Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };

</script>
<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="~/Resources/Mod/Applicant/EditProfile.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Applicant/editprofile.css" ResourceType="StyleSheet" />

</infs:WclResourceManagerProxy>
<style type="text/css">
    #dvSSNMask span.RadInput {
        width: 89% !important;
    }

    #dvSSNMask .rbToggleButton {
        padding-left: 0px !important;
        vertical-align: top;
    }

    .section .content .shdr {
        font-size: 11px !important;
    }

    span.cptn {
        font-size: 11px !important;
    }

    input + label, input + span, .user {
        font-size: 11px !important;
    }

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

    .checkboxSelect.myControl input[type="checkbox"] {
        right: -3px !important;
    }

    .addAlias {
        border: 1px solid #62798e;
    }
</style>
<script type="text/javascript">

</script>
<div onclick="setPostBackSourceEP(event, this);">
    <asp:TextBox ID="hdnPostbacksource" class="postbacksource" runat="server" Style="display: none;" />
    <div class="section">
        <h1 class="mhdr"><%--Update Profile--%>
            <%=Resources.Language.UPDATEPROFILE %>
        </h1>
        <div class="content">
            <div id="modcmd_bar">
                <div id="vermod_cmds">
                    <asp:LinkButton Text="Back to Search" runat="server" ID="lnkGoBack" OnClick="lnkGoBack_click"
                        Visible="false" />
                </div>
            </div>
            <div id="dvMain" class="sxform auto">
                <div class="msgbox" id="msgBox" runat="server">
                    <asp:Label ID="lblMessage" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlEditProfile">
                    <table border="0" cellpadding="0" cellspacing="0" class="tblMain">
                        <tr>
                            <td class="tdLeft" id="dvProfilePic" runat="server">
                                <div class="bx_image">
                                    <asp:Image runat="server" ID="imgCntrl" class="thumb" />
                                    <asp:Label runat="server" ID="lblNameInitials" Visible="false" class="initName" />
                                </div>
                                <div class="bx_uploader" title="<%=Resources.Language.CLCKCHNGPRFLPICTOOLTIP %>">
                                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                        OnClientFileUploaded="onFileUploaded" MultipleFileSelection="Disabled" MaxFileInputsCount="1"
                                        AllowedFileExtensions=".jpg,.jpeg,.tiff,.bmp,.bitmap,.png,.JPG,.PNG,.BITMAP,.JPEG,.TIFF,.BMP" OnClientFileSelected="onClientFileSelected"
                                        Localization-Select="<%$Resources:Language,CHANGE%>" Width="80px" OnClientValidationFailed="upl_OnClientValidationFailed" />
                                    <div style="display: none">
                                        <infs:WclButton ID="btnUpload" runat="server" OnClick="btnUpload_Click">
                                        </infs:WclButton>
                                    </div>
                                </div>
                                <%--<span class="img_msg">Changed image will appear after you update the page.</span>--%>
                            </td>
                            <td class="tdRight">
                                <h1 class="shdr"><%--Account Information--%>
                                    <%=Resources.Language.ACCOUNTINFO %>
                                </h1>
                                <div id="dvAccountInfo" class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class='cptn'>
                                            <%=Resources.Language.USERNAME %>
                                            <%--Username--%></span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtUsername" MaxLength="256" ReadOnly="true">
                                        </infs:WclTextBox>
                                        <div class="vldx">
                                            <%-- <asp:RequiredFieldValidator runat="server" ID="rqfvUserName" ControlToValidate="txtUsername"
                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Username is required." ValidationGroup="grpFormSubmit" />--%>
                                            <%--<asp:RegularExpressionValidator runat="server" ID="revUserName" 
                                                CssClass="errmsg" ValidationGroup="grpFormSubmit" ErrorMessage="" Display="Dynamic" ValidationExpression="^[\.\@a-zA-Z0-9_-]{4,50}$" />--%>
                                            <span id="spnUserNameErrorMsg" class="errmsg">
                                                <asp:Label ID="lblUserNameMessage" runat="server" IsValidated="0" CssClass="errmsg" Text=""></asp:Label>
                                            </span>
                                        </div>
                                    </div>
                                    <div class='sxlm'>
                                        <%--Text="Check"--%>
                                        <infs:WclButton runat="server" ID="btnCheckUsername" Text="<% $Resources:Language, CHECK %>" OnClick="btnCheckUsername_Click"
                                            Skin="Windows7" AutoSkinMode="false" CausesValidation="true" ValidationGroup="grpFormSubmit">
                                        </infs:WclButton>
                                        <infs:WclButton runat="server" ID="btnLinkAccount" Text="<% $Resources:Language, LINKACCOUNT %>" OnClick="btnLinkAccount_Click"
                                            Skin="Windows7" AutoSkinMode="false">
                                        </infs:WclButton>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class='cptn'>
                                            <%=Resources.Language.INSTITUTE %>
                                            <%--Institute--%></span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtOrganisation" MaxLength="50" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxlb' id="divChangePwd" runat="server" visible="false">
                                        <span class='cptn'>
                                            <%=Resources.Language.CHGEPASSWRD %>
                                            <%--Change Password--%></span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:HyperLink ID="lnkChangePassword" runat="server" Visible="false" CssClass="user" Text="<%$Resources:Language,CHGEPASSWRD %>"> </asp:HyperLink>
                                    </div>
                                </div>
                                <div runat="server" id="dvTwoFactorAuthentication" style="display: none;">
                                    <h1 class="shdr">
                                        <%=Resources.Language.TWOFACTORAUTHEN %>
                                        <%--Two Factor Authentication--%></h1>
                                    <infsu:TwoFactorAuthentication ID="TwoFactorAuthentication" Visible="true" runat="server" />
                                </div>
                                <div class="sxro" style="padding-bottom: 0px; margin-bottom: 0px;">
                                    <h1 class="shdr" style="padding-bottom: 0px; margin-bottom: 0px !important;"><%=Resources.Language.PERSONALINFO %> </h1>
                                </div>

                                <div class='sxro sx4co'>
                                    <div class='sxlb' id="dvSpnFirstName" runat="server">
                                        <span class='cptn'><%=Resources.Language.FIRSTNAME %> </span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm' id="dvFirstName" runat="server">
                                        <infs:WclTextBox runat="server" ID="txtFirstName" MaxLength="30">
                                        </infs:WclTextBox>
                                        <div class="vldx">
                                            <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,FIRSTNAMEREQ%>" ValidationGroup="grpFormSubmit" />
                                            <asp:RegularExpressionValidator runat="server" ID="revFirstName" ControlToValidate="txtFirstName"
                                                Display="Dynamic" CssClass="errmsg"
                                                ErrorMessage="Invalid character(s)." ValidationGroup="grpFormSubmit" />
                                        </div>
                                    </div>
                                    <div class='sxlb' title="<%$Resources:Language,IFDONTHAVEMIDDLENAME%>" id="dvSpnMiddleName" runat="server">
                                        <span class='cptn'><%=Resources.Language.MIDDLENAME%></span><span class="reqd" id="spnMiddleName" runat="server">*</span>
                                    </div>
                                    <div class='sxlm' id="dvMiddleName" runat="server">
                                        <infs:WclTextBox runat="server" ID="txtMiddleName"
                                            MaxLength="30">
                                        </infs:WclTextBox>
                                        <div class="vldx">
                                            <asp:RequiredFieldValidator runat="server" ID="rfvMiddleName" ControlToValidate="txtMiddleName"
                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,MIDDLENAMEREQ%>" ValidationGroup="grpFormSubmit" />
                                            <asp:RegularExpressionValidator runat="server" ID="revMiddleName" ControlToValidate="txtMiddleName"
                                                Display="Dynamic" CssClass="errmsg"
                                                ErrorMessage="<%$Resources:Language,INVALIDCHARS%>" ValidationGroup="grpFormSubmit" />
                                        </div>
                                    </div>
                                    <div class='sxlb' id="dvSpnLastName" runat="server">
                                        <span class='cptn'><%=Resources.Language.LASTNAME%></span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm' id="dvLastName" runat="server" >
                                        <infs:WclTextBox runat="server" ID="txtLastName" MaxLength="30">
                                        </infs:WclTextBox>
                                        <div class="vldx">
                                            <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,LASTNAMEREQ%>" ValidationGroup="grpFormSubmit" />
                                            <asp:RegularExpressionValidator runat="server" ID="revLastName" ControlToValidate="txtLastName"
                                                Display="Dynamic" CssClass="errmsg"
                                                ErrorMessage="<%$Resources:Language,INVALIDCHARS%>" ValidationGroup="grpFormSubmit" />
                                        </div>
                                    </div>
                                    <div class='sxlm' id="dvSuffix" runat="server" style="display: none;width:15%">
                                         <infs:WclComboBox ID="cmbSuffix" runat="server" Visible="false"  DataTextField="Suffix" DataValueField="SuffixID" ></infs:WclComboBox>
                                        <infs:WclTextBox runat="server" ID="txtSuffix" MaxLength="10" ValidationGroup="grpFormSubmit" ToolTip="<%$Resources:Language, ENTERSUFFIXIFAPPLICABLE %>" placeholder="<%$Resources:Language, ENTERSUFFIXIFAPPLICABLE %>">
                                        </infs:WclTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator ControlToValidate="txtSuffix" ID="revSuffix" runat="server" Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDCHARS %>" ValidationGroup="grpFormSubmit">
                                            </asp:RegularExpressionValidator>

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
                                    <div class='sxro sx3co' runat="server" id="divSSN">
                                        <div class='sxlb'>
                                            <span class='cptn'><%=Resources.Language.ISSSN%></span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm' style="padding-top: 5px;">
                                            <asp:RadioButtonList ID="rblSSN" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="FormatRadioButtonList" OnSelectedIndexChanged="rblSSN_SelectedIndexChanged">
                                                <asp:ListItem Text="<% $Resources:Language, LSTITMY %>" Value="true" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<% $Resources:Language, LSTITMN %>" Value="false"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div id="dvSSNMain" runat="server">
                                            <div class='sxlb'>
                                                <span class='cptn'><%=Resources.Language.SSN%></span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm' id="dvSSNMask">
                                                <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="###-##-####" />
                                                <div class="valdx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                                                        Display="Dynamic" ErrorMessage="<%$Resources:Language,SSNREQ%>" ValidationGroup="grpFormSubmit" />
                                                    <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server"
                                                        CssClass="errmsg" ErrorMessage="<%$Resources:Language,FULLSSNREQ%>" ValidationGroup="grpFormSubmit" ControlToValidate="txtSSN"
                                                        ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                                                    <asp:RegularExpressionValidator Display="Dynamic" ID="rgvSSNCBI" runat="server" Enabled="false" ValidationGroup="grpFormSubmit"
                                                        CssClass="errmsg" ErrorMessage="<%$Resources:Language,INVALIDSSN%>" ControlToValidate="txtSSN"
                                                        ValidationExpression="(?!9)(?!\b(\d)\1+-(\d)\1+-(\d)\1+\b)(?!000)\d{3}-(?!00)\d{2}-(?!0{4})\d{4}" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class='sxro sx3co'>
                                        <uc:PersonAlias ID="ucPersonAlias" runat="server" Visible="true" IsEditProfile="true" IsLabelMode="true"></uc:PersonAlias>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class='cptn'><%=Resources.Language.GENDER%></span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <%--  <infs:WclComboBox ID="cmbGender" runat="server" DataTextField="GenderName" DataValueField="GenderID">
                                        </infs:WclComboBox>--%>
                                            <infs:WclComboBox ID="cmbGender" runat="server" DataTextField="GenderName" DataValueField="DefaultLanguageKeyID">
                                            </infs:WclComboBox>
                                        </div>
                                        <div id="divDOB" runat="server">
                                            <div class='sxlb'>
                                                <span class='cptn'><%=Resources.Language.DOB%></span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="<%$Resources:Language,SELECTDATE%>">
                                                </infs:WclDatePicker>
                                                <div class="valdx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvDOB" CssClass="errmsg" ControlToValidate="dpkrDOB"
                                                        Display="Dynamic" ErrorMessage="Date of Birth is required." ValidationGroup="grpFormSubmit" />

                                                    <asp:RangeValidator ID="rngvDOB" runat="server" ControlToValidate="dpkrDOB" Type="Date"
                                                        ValidationGroup="grpFormSubmit" Display="Dynamic" CssClass="errmsg"></asp:RangeValidator>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="dvCommLang" runat="server" style="display: none">
                                            <div class='sxlb'>
                                                <span class='cptn'><%=Resources.Language.PRFRDCOMCTNLANG %></span><span class="reqd"></span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclComboBox ID="cmbCommLang" runat="server" DataTextField="LAN_Name"
                                                    DataValueField="LAN_ID">
                                                </infs:WclComboBox>
                                            </div>
                                        </div>

                                        <%--   <div id="divSSN" runat="server">
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
                                    </div>--%>
                                        <div id="divSSNMasked" visible="false" runat="server">
                                            <div class='sxlb'>
                                                <span class='cptn'><%=Resources.Language.SSN%></span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclTextBox ID="txtSSNMasked" runat="server" ReadOnly="true" />
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <h1 class="shdr"><%=Resources.Language.CONTACTINFO%></h1>
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class='cptn'><%=Resources.Language.EMAIL%></span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox ID="txtPrimaryEmail" runat="server" MaxLength="250">
                                            </infs:WclTextBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtPrimaryEmail"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,EMAILADDRESSREQ%>" ValidationGroup="grpFormSubmit" />
                                                <asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ControlToValidate="txtPrimaryEmail"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,EMAILNOTVALID%>" ValidationGroup="grpFormSubmit"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                            </div>
                                        </div>
                                        <div class='sxlb' title="<%=Resources.Language.RTYPEMAILADDRS%>">
                                            <span class='cptn'><%=Resources.Language.CPREMAIL%></span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox ID="txtConfrimPrimayEmail" runat="server" MaxLength="250">
                                            </infs:WclTextBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvConfirmEmail" ControlToValidate="txtConfrimPrimayEmail"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,CNFMEMAILADDRESSREQ%>" ValidationGroup="grpFormSubmit" />
                                                <%--<asp:CompareValidator ID="cmpConfirmPrimaryEmail" ControlToCompare="txtPrimaryEmail"
                                            ControlToValidate="txtConfrimPrimayEmail" runat="server" CssClass="errmsg" Display="Dynamic"
                                            ErrorMessage="Email Address did not match."></asp:CompareValidator>--%>
                                                <asp:CustomValidator ID="cstVConfirmPrimaryEmail" runat="server" ErrorMessage="<%$Resources:Language,EMAILADDRESSNOTMATCH%>"
                                                    CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail" ClientIDMode="Static" ValidationGroup="grpFormSubmit"
                                                    ControlToValidate="txtConfrimPrimayEmail"></asp:CustomValidator>
                                            </div>
                                        </div>
                                        <div class='sxlb' title="<%=Resources.Language.SNDPWDRECEMAILSTOEMAILADDR%>">
                                            <span class='cptn'><%=Resources.Language.PWDRECVRYEMAIL%></span>
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
                                            <asp:CheckBox ID="chkChangeEmail" runat="server" Text="<%$Resources:Language,SNDPWDRECVYEMAILALSOPRMRYADDRS%>"
                                                ToolTip="<%$Resources:Language,PRMRYEMAILCHCKPWDRECVRY%>"></asp:CheckBox>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx3co'>
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
                                                <%--<asp:CompareValidator ID="cmpConfirmSecondaryEmail" ControlToCompare="txtSecondaryEmail"
                                            ControlToValidate="txtConfirmSecEmail" runat="server" CssClass="errmsg" Display="Dynamic"
                                            ErrorMessage="Email Address did not match."></asp:CompareValidator>--%>
                                                <asp:CustomValidator ID="cstVConfirmSecEmail" runat="server" ErrorMessage="<%$Resources:Language,EMAILADDRESSNOTMATCH%>"
                                                    ClientIDMode="Static" CssClass="errmsg" Display="Dynamic" ClientValidationFunction="CompareEmail"
                                                    ControlToValidate="txtConfirmSecEmail"></asp:CustomValidator>
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class='cptn'><%=Resources.Language.PHONE%></span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm myControl checkboxSelect'>
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
                                        <div class='sxlb' title="<%=Resources.Language.ENTERSECONDPHONE%>">
                                            <span class='cptn'><%=Resources.Language.SECPHONE%></span>
                                        </div>
                                        <div class='sxlm myControl checkboxSelect'>
                                            <div id="dvMaskedSecondaryPhone" runat="server">
                                                <infs:WclMaskedTextBox ID="txtSecondaryPhone" runat="server" Mask="(###)-###-####">
                                                </infs:WclMaskedTextBox>
                                            </div>
                                            <div id="dvUnMaskedSecondaryPhone" runat="server" style="display: none;">
                                                <infs:WclTextBox ID="txtUnmaskedSecondaryPhone" runat="server" MaxLength="15"></infs:WclTextBox>
                                                <div class="vldx">
                                                    <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtUnmaskedSecondary" runat="server" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                        ErrorMessage="<%$Resources:Language,INVALIDPHONE%>"
                                                        ControlToValidate="txtUnmaskedSecondaryPhone" ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                                                </div>
                                            </div>
                                            <infs:WclCheckBox runat="server" ID="chkSecondaryPhone" ToolTip="<%$Resources:Language,NOUSNUMBER%>" onclick="MaskUnmaskSecoundaryPhone(this)" CssClass="checkboxSetting"></infs:WclCheckBox>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <asp:Label runat="server" class='cptn' ID="lblAddress1">Address 1</asp:Label><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm m2spn'>
                                            <infs:WclTextBox runat="server" ID="txtAddress1" MaxLength="100">
                                            </infs:WclTextBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvAddress1" ControlToValidate="txtAddress1"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Address 1 is required." ValidationGroup="grpFormSubmit" />
                                                <asp:RegularExpressionValidator Display="Dynamic" Enabled="false" ID="revAddress1" runat="server"
                                                    CssClass="errmsg" ErrorMessage="<%$Resources:Language,ENTERVALIDZIPASCIICODE%>" ControlToValidate="txtAddress1"
                                                    ValidationExpression="^[\x01-\x7F]+$" ValidationGroup="grpFormSubmit" />
                                            </div>
                                        </div>
                                        <div id="dvAddress2" runat="server">
                                            <div class='sxlb'>
                                                <span class='cptn'>Address 2</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclTextBox runat="server" ID="txtAddress2" MaxLength="100">
                                                </infs:WclTextBox>
                                            </div>
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
                                    <div runat="server" id="divSMSNotification">
                                        <h1 class="shdr">
                                            <%=Resources.Language.TXTMSGNOTIFICATION %>
                                            <%--Text Message Notifications--%></h1>
                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class='cptn'>
                                                    <%=Resources.Language.CELLULARPHNNUM %>
                                                    <%--Cellular Phone Number--%></span>
                                                <span class="reqd" runat="server" id="spnPhoneNumberReq">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclMaskedTextBox runat="server" ID="txtPhoneNumber" Mask="(###)-###-####">
                                                </infs:WclMaskedTextBox>
                                                <div class="vldx">
                                                    <%--ErrorMessage="Cellular Phone Number is required."--%>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPhoneNumber" ControlToValidate="txtPhoneNumber"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="<% $Resources:Language, CELLULARPHNREQ %>" ValidationGroup="grpFormSubmit" />

                                                    <%--ErrorMessage="Format is (###)-###-####"--%>
                                                    <asp:RegularExpressionValidator Display="Dynamic" ID="revCellularPhoneNumber" runat="server" ValidationGroup="grpFormSubmit"
                                                        CssClass="errmsg" ErrorMessage="<% $Resources:Language, PHNFORMAT %>" ControlToValidate="txtPhoneNumber"
                                                        ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
                                                </div>
                                            </div>

                                            <div class='sxlb'>
                                                <span class='cptn'><%--Receive Text Notification--%>
                                                    <%=Resources.Language.RECVTXTNOTIFICATION %> 
                                                </span>
                                            </div>
                                            <%--<asp:ListItem Text="Yes &nbsp;" Value="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Selected="True" Value="False"></asp:ListItem>--%>
                                            <div class='sxlm'>
                                                <asp:RadioButtonList ID="rdbTextNotification" runat="server" onclick="HideShowPhoneNumber(this)" RepeatDirection="Horizontal">

                                                    <asp:ListItem Text="<% $Resources:Language, LSTITMY %>" Value="True"></asp:ListItem>
                                                    <asp:ListItem Text="<% $Resources:Language, LSTITMN %>" Selected="True" Value="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:HiddenField ID="hdnIsConfirmMsgVisible" Value="0" runat="server" />
                                            </div>
                                        </div>
                                        <%-- <div id="divHideShowPhoneNumber" runat="server" style="display: none">

                                            <div class="sxroend"></div>
                                        </div>--%>

                                        <%--<div id="divConfirmationStatus" class='sxro sx3co' runat="server" style="display: none">
                                        <div class='sxlb'>
                                            <span class='cptn'>Confirmation Status</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:Label ID="lblConfirmationStatus" runat="server"></asp:Label>
                                        </div>
                                        <div>
                                            <%--<asp:LinkButton Text="Re-Send Subscription Message" runat="server" ID="lnkReSendSubMessage" />
                                        </div>
                                        <div class="sxroend"></div>
                                    </div>--%>
                                    </div>

                                    <div runat="server" id="dvResHistory">
                                        <h1 class="shdr"><%--Residential History--%>
                                            <%=Resources.Language.RESIDENTIALHISTORY %>
                                        </h1>
                                        <uc:PrevResident ID="PrevResident" runat="server" class="PrevResident" IsEditProfile="true" />
                                    </div>
                                    <div runat="server" id="divProfileNotes" visible="false">
                                        <h1 class="shdr">Profile Note(s)</h1>
                                        <uc:ApplicantProfileNotes ID="ucApplicantNotes" runat="server" Visible="False"></uc:ApplicantProfileNotes>
                                    </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
    </div>
    <infsu:CustomAttribute ID="caProfileCustomAttributes" Title="Profile Information" runat="server" />

    <div id="divcontent" runat="server">
        <div class="section">
            <h1 class="mhdr">Custom Attributes</h1>
            <div class="content">
                <div class="sxform auto">
                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                        <asp:Repeater ID="rptrCustomAttribute" runat="server" OnItemDataBound="rptrCustomAttribute_ItemDataBound" OnLoad="rptrCustomAttribute_Load" OnPreRender="rptrCustomAttribute_PreRender">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="content">
                                    <div class="sxform auto">
                                        <infsu:CustomAttribute ID="customAttribute" Title='<%# Eval("InstitutionHierarchyLabel") %>' MappingRecordId='<%#Eval("InstitutionNode_ID") %>'
                                            ValueRecordId='<%# Eval("DPM_ID") %>' runat="server" />
                                        <div class='sxroend'>
                                        </div>
                                        <hr style="border-bottom: solid 1px #c0c0c0;" />
                                    </div>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                        <%-- UAT 1438: Enhancement to allow students to select a User Group.--%>
                        <div id="dvMergedUserGroup" runat="server" class="content" visible="false">
                            <div class="sxform auto">
                                <infsu:CustomAttribute ID="customAttribute" IsUserGroupSlctdValuesdisabled="false" IsMultipleValsSelected="false" IsMultiSelectionAllowed="false" Visible="false" Title="User Group" runat="server" />
                                <div class='sxlm'>
                                    <div class="vldx">
                                        <span id="spnUserGroupErrMsg" class="errMsgUserGroup">
                                            <asp:Label ID="errMsgUserGroup" runat="server" IsValidated="0" CssClass="errmsg" Text="" Visible="false"></asp:Label>
                                        </span>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <div id="dvUsrGrp" runat="server" style="display: none;">
        <div class="section">
        <h2 class="mhdr">User Group
    </h2>
        <div class="content">
            <div id="divForm" class="sxform auto">
                <div id="pnlRows" class="sxpnl">
                    <div class="sxro sx3co">
                        <div id="pnlRow">


                            <div id="pnlControlsMode">

                                <div class="sxlb">
                                    <span id="lblLabel" class="cptn">User Group</span>
                                </div>
                                <div id="divControlMode" class="sxlm">
                                    <infs:WclComboBox ID="cmdUserGroup" runat="server" DataTextField="UG_Name" EnableCheckAllItemsCheckBox="true"  CheckBoxes="true"
                                        DataValueField="UG_ID"  EnableTextSelection="true" Width="30%" 
                                        EmptyMessage="--SELECT--">
                                    </infs:WclComboBox>
                                </div>

                            </div>




                        </div>
                        <div class="sxroend">
                        </div>
                    </div>


                </div>
            </div>
        </div>
            </div>
    </div>


    <asp:Button ID="btnProfilehide" runat="server" Style="display: none;" OnClick="fsucCmdBar1_SaveClick" />

    <infsu:CommandBar ID="cBarMain" runat="server" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel"
        ButtonPosition="Center" SaveButtonText="<%$Resources:Language,UPDATE %>" CancelButtonText="<%$Resources:Language,CNCL %>" DefaultPanel="pnlEditProfile"
        OnSaveClientClick="DelayButtonClick" OnSaveClick="fsucCmdBar1_SaveClick" OnCancelClick="fsucCmdBar1_CancelClick" ValidationGroup="grpFormSubmit" />
    <br />
    <div id="confirmSave" class="confirmProfileSave" runat="server" style="display: none">
        <p style="text-align: center"><%=Resources.Language.WRNGIGNRCNTNUADDALIAS %></p>


    </div>
    <asp:HiddenField ID="hdnNoMiddleNameText" runat="server" Value="" />
    <asp:HiddenField ID="hdnrdbSpecifyAuthenticationCalculatedValue" Value="" runat="server" />
    <asp:HiddenField ID="hdnIsLocationTenant" runat="server" />
    <asp:HiddenField ID="hdnProfileConfirmSave" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIFYOUDONTHAVEMIDDLENAME" runat="server" Value="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>" />
</div>

