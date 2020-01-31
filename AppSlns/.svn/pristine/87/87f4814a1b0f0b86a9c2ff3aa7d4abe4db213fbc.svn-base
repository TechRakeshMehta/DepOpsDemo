<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdditionalAccountVerification.aspx.cs" StylesheetTheme="NoTheme" MasterPageFile="~/Shared/PopupMaster.master"
    Inherits="CoreWeb.Shell.Views.AdditionalAccountVerification" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CustomAttribute" Src="~/ComplianceAdministration/UserControl/CustomAttributeProfileLoader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">

    <infs:WclResourceManagerProxy runat="server" ID="rprxAddUserInformation">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <style type="text/css">
        .popupCnt {
            width: 100% !important;
            height: 100vh !important;
            overflow: auto !important;
        }

        .msgbox .info {
            background-color: #fcf8e3 !important;
            border-radius: 4px !important;
            color: #000 !important;
            font-size: 14px;
            border: 1px solid #adadad;
        }

        .msgbox .info {
            background-color: #fffef0 !important;
            background-position: 10px 8px !important;
            background-repeat: no-repeat !important;
            color: #3071cd !important;
        }

        .msgbox .info {
            border-width: 1px;
            display: block;
            margin: 10px;
            padding: 15px 10px 20px 53px;
        }

        h2.subhead {
            margin-left: -16px;
        }

        html {
            overflow: hidden;
        }

        #popupProgress {
            display: block;
            position: absolute;
            bottom: 10px;
            right: 20px;
        }
        /*.msgbox .sucs {
            background-color: #dff0d8;
            border-color: green;
            border-radius: 4px;
            color: #000 !important;
            font-size: 14px;
        }

        .msgbox span.error {
            font-family: 'Titillium Web ', sans-serif !important;
            background-color: #f2dede !important;
            color: #000 !important;
            border-radius: 4px;
            font-size: 14px;
        }

        .msgbox .info {
            background-color: #fcf8e3;
            color: #000 !important;
            border-radius: 4px;
            font-size: 14px;
        }*/
        .container-fluid h2, .container-fluid .h2text {
            font-size: 20px !important;
            line-height: 20px;
            height: 20px;
            font-weight: 600;
        }
    </style>
    <script type="text/javascript"> 

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function returnToParent() {
            var hdnIsUserActive = $jQuery("[id$=hdnIsUserActive]")[0].value;
            var oArg = {};
            var count = 0;
            oArg.RedirectUrl = hdnIsUserActive;
            //get a reference to the current RadWindow
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }

        // To close the popup.
        function ClosePopup() {
            top.$window.get_radManager().getActiveWindow().close();
        }

        //hide the custom attr head
        function hideHead() {
            //debugger;
            //var ctr = $("#PoupContent_dvCustAttrAny").find('.subhead');
            //$("#PoupContent_dvCustAttrAny").find('.subhead').css({ "display": "none" });
        }

    </script>
    <div class="container-fluid">
        <div class="msgbox" id="divSuccessMsg">
            <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
        </div>
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Complio Account Verification</h2>
            </div>
        </div>
        <div class="row bgLightGreen">
            <asp:Panel ID="pnlAccountVerification" runat="server">
                <div runat="server" id="dvSectionWithAnyPrmsn" visible="false">
                    <div class="col-md-12">
                        <div class="row">
                            <div class='form-group col-md-12 col-sm-12'>
                                <span class="cptn">Please choose the verification question of your choice and then provide corresponding answer</span><span class='reqd'>*</span>
                                <infs:WclComboBox runat="server" ID="cmbQuestions" AutoPostBack="true" OnDataBound="cmbQuestions_DataBound" AutoSkinMode="false" Skin="Silk" Width="100%" ValidationGroup="NoValidation"
                                    DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="cmbQuestions_SelectedIndexChanged" OnClientDropDownClosed="hideHead">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvQuestions" ControlToValidate="cmbQuestions"
                                        InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" ErrorMessage="Question is required." />
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row">
                            <div class='form-group col-md-3'>
                                <div id="dvAnswer" runat="server" style="display: none">
                                    <span class="cptn">Answer</span><span class='reqd'>*</span>
                                    <infs:WclDatePicker ID="dpkrDOBAny" runat="server" DateInput-EmptyMessage="mm/dd/yyyy"
                                        Width="100%" CssClass="form-control" Visible="false" Skin="Windows7" AutoSkinMode="false">
                                    </infs:WclDatePicker>
                                    <infs:WclMaskedTextBox runat="server" ID="txtSSNAny" Mask="###-##-####" AutoPostBack="false" Width="100%" CssClass="form-control" Visible="false" />
                                    <infs:WclMaskedTextBox runat="server" ID="txtLastSSNAny" Mask="####" AutoPostBack="false" Width="100%" CssClass="form-control" Visible="false" />
                                </div>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvDOBAny" CssClass="errmsg" ControlToValidate="dpkrDOBAny" Enabled="false"
                                        Display="Dynamic" ErrorMessage="Date of Birth is required." />
                                    <asp:RequiredFieldValidator runat="server" ID="rfvSSNAny" CssClass="errmsg" ControlToValidate="txtSSNAny" Enabled="false"
                                        Display="Dynamic" ErrorMessage="Social Security Number is required." />
                                    <asp:RegularExpressionValidator Display="Dynamic" ID="revSSNAny" runat="server" Enabled="false"
                                        CssClass="errmsg" ErrorMessage="Full Social Security Number is required." ControlToValidate="txtSSNAny"
                                        ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                                    <asp:RequiredFieldValidator runat="server" ID="rfvLastSSNAny" CssClass="errmsg" ControlToValidate="txtLastSSNAny" Enabled="false"
                                        Display="Dynamic" ErrorMessage="Last four SSN is required." />
                                    <asp:RegularExpressionValidator Display="Dynamic" ID="revLastSSNAny" runat="server" Enabled="false"
                                        CssClass="errmsg" ErrorMessage="Last four SSN is required." ControlToValidate="txtLastSSNAny"
                                        ValidationExpression="\d{4}" />

                                </div>
                            </div>
                            <div class='col-md-12'>
                                <div class="col-md-12" style="padding-left: 15px;" runat="server" id="dvCustAttrAny" visible="false">
                                    <asp:Panel ID="pnlCustAttrAny" runat="server"></asp:Panel>

                                    <div style="margin-top: -40px;">
                                        <infsu:CustomAttribute ID="caProfileCustomAttributesAny" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div runat="server" id="dvSectionWithAllPrmsn" visible="false">
                    <div class="col-md-12">
                        <div class="row">
                            <div class='form-group col-md-12 col-sm-12'>
                                <span class="cptn">Please provide answer corresponding to each verification question</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row">
                            <div class='form-group col-md-3 col-sm-3' runat="server" id="dvDOB" visible="false">
                                <asp:Label ID="lblDOB" runat="server" Text="Date of Birth" CssClass="cptn"></asp:Label>
                                <span class="reqd">*</span>
                                <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="mm/dd/yyyy" Width="100%" CssClass="form-control"
                                    Skin="Windows7" AutoSkinMode="false">
                                </infs:WclDatePicker>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvDOB" CssClass="errmsg" ControlToValidate="dpkrDOB" Enabled="false"
                                        Display="Dynamic" ErrorMessage="Date of Birth is required." />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row">
                            <div class='form-group col-md-3 col-sm-3' runat="server" id="dvSSN" visible="false">
                                <asp:Label ID="lblSSN" runat="server" Text="SSN" CssClass="cptn"></asp:Label>
                                <span class="reqd">*</span>
                                <div id="dvSSNMask" class="sxlm">
                                    <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="###-##-####" AutoPostBack="false" Width="100%" CssClass="form-control" />
                                </div>
                                <div id="dvSSNValidator" class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN" Enabled="false"
                                        Display="Dynamic" ErrorMessage="Social Security Number is required." />
                                    <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server" Enabled="false"
                                        CssClass="errmsg" ErrorMessage="Full Social Security Number is required." ControlToValidate="txtSSN"
                                        ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row">
                            <div class='form-group col-md-3 col-sm-3' runat="server" id="dvLSSN" visible="false">
                                <asp:Label ID="lblLSSN" runat="server" Text="Last four SSN" CssClass="cptn"></asp:Label>
                                <span class="reqd">*</span>
                                <div id="Div1" class="sxlm">
                                    <infs:WclMaskedTextBox runat="server" ID="txtLSSN" Mask="####" AutoPostBack="false" Width="100%" CssClass="form-control" />
                                </div>
                                <div id="Div2" class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvLSSN" CssClass="errmsg" ControlToValidate="txtLSSN" Enabled="false"
                                        Display="Dynamic" ErrorMessage="Last four SSN is required." />
                                    <asp:RegularExpressionValidator Display="Dynamic" ID="revLSSN" runat="server"
                                        CssClass="errmsg" ErrorMessage="Last four SSN is required." ControlToValidate="txtLSSN" Enabled="false"
                                        ValidationExpression="\d{4}" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div runat="server" class="col-md-12" id="dvProfileCustAttr" style="display: none">
                        <div class="col-md-12" style="padding-left: 15px;">
                            <div style="margin-top: -40px;">
                                <infsu:CustomAttribute ID="caProfileCustomAttributes" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>

            </asp:Panel>
        </div>
        <div class="col-md-12">
            <div class="row">&nbsp;</div>
            <div class="row">
                <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Cancel" DefaultPanel="pnlAccountVerification" DefaultPanelButton="Submit"
                    AutoPostbackButtons="Submit,Cancel" SubmitButtonText="Verify Account" SubmitButtonIconClass=""
                    CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_SubmitClick"
                    OnCancelClick="fsucCmdBarButton_CancelClick"
                    ClearButtonIconClass="" UseAutoSkinMode="false" ButtonSkin="Silk">
                </infsu:CommandBar>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnIsUserActive" />


    <%--<div id="box_content">
        <div class="msgbox" id="divSuccessMsg">
            <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
        </div>
        <div class="section">
            <h1 class="mhdr">Verification Question and Answer
            </h1>
            <div class="content">
                <div class="sxform auto">
                    <div class="msgbox">
                        <asp:Label ID="Label1" runat="server" CssClass="info">
                        </asp:Label>
                    </div>
                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                        <div runat="server" id="dvSectionWithAnyPrmsn" visible="false">
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <asp:Label ID="lblTenant" runat="server" Text="Please choose the verification question of your choice and then provide corresponding answer" CssClass="cptn"></asp:Label>
                                    <span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclComboBox runat="server" ID="cmbQuestions" AutoPostBack="true" OnDataBound="cmbQuestions_DataBound"
                                        DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="cmbQuestions_SelectedIndexChanged">
                                    </infs:WclComboBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvQuestions" ControlToValidate="cmbQuestions"
                                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Question is required." />
                                    </div>
                                </div>
                            </div>
                            <div class='sxro sx3co' runat="server" id="dvAnswer">
                                <div class='sxlb'>
                                    <asp:Label ID="lblAnswer" runat="server" Text="Answer" CssClass="cptn"></asp:Label>
                                    <span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclDatePicker ID="dpkrDOBAny" runat="server" DateInput-EmptyMessage="mm/dd/yyyy" CssClass="ValidatetionCheck" Visible="false"
                                        Skin="Windows7" AutoSkinMode="false">
                                    </infs:WclDatePicker>
                                    <infs:WclMaskedTextBox runat="server" ID="txtSSNAny" Mask="###-##-####" AutoPostBack="false" CssClass="ValidatetionCheck" Visible="false" />
                                </div>
                                <div class='sxro sx3co' runat="server" id="dvCustAttrAny" visible="false">
                                    <infsu:CustomAttribute ID="custAttrAny" runat="server" Title="Profile Information" />
                                </div>
                            </div>
                        </div>
                        <div runat="server" id="dvSectionWithAllPrmsn" visible="false">

                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <asp:Label ID="lblHeading" runat="server" Text="Please provide answer corresponding to each verification question" CssClass="cptn"></asp:Label>
                                </div>
                            </div>
                            <div class='sxro sx3co' runat="server" id="dvDOB">
                                <div class='sxlb'>
                                    <asp:Label ID="lblDOB" runat="server" Text="Date of Birth" CssClass="cptn"></asp:Label>
                                    <span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="mm/dd/yyyy" CssClass="ValidatetionCheck"
                                        Skin="Windows7" AutoSkinMode="false">
                                    </infs:WclDatePicker>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDOB" CssClass="errmsg" ControlToValidate="dpkrDOB"
                                            Display="Dynamic" ErrorMessage="Date of Birth is required." />
                                    </div>
                                </div>

                            </div>
                            <div class='sxro sx3co' runat="server" id="dvSSN">
                                <div class='sxlb'>
                                    <asp:Label ID="lblSSN" runat="server" Text="SSN" CssClass="cptn"></asp:Label>
                                    <span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <div id="dvSSNMask" class="sxlm">
                                        <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="###-##-####" AutoPostBack="false" CssClass="ValidatetionCheck" />
                                    </div>
                                    <div id="dvSSNValidator" class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvSSN" CssClass="errmsg" ControlToValidate="txtSSN"
                                            Display="Dynamic" ErrorMessage="Social Security Number is required." />
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revtxtSSN" runat="server"
                                            CssClass="errmsg" ErrorMessage="Full Social Security Number is required." ControlToValidate="txtSSN"
                                            ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                                    </div>
                                </div>
                            </div>

                            <div class='sxro sx3co' runat="server" id="dvLSSN">
                                <div class='sxlb'>
                                    <asp:Label ID="lblLSSN" runat="server" Text="Last four of SSN" CssClass="cptn"></asp:Label>
                                    <span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <div id="Div1" class="sxlm">
                                        <infs:WclMaskedTextBox runat="server" ID="txtLSSN" Mask="###-##-####" AutoPostBack="false" CssClass="ValidatetionCheck" />
                                    </div>
                                    <div id="Div2" class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvLSSN" CssClass="errmsg" ControlToValidate="txtLSSN"
                                            Display="Dynamic" ErrorMessage="Last four of Social Security Number is required." />
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revLSSN" runat="server"
                                            CssClass="errmsg" ErrorMessage="Last four of Social Security Number is required." ControlToValidate="txtLSSN"
                                            ValidationExpression="\d{3}\-\d{2}-\d{4}" />
                                    </div>
                                </div>
                            </div>
                            <div class='sxro sx3co' runat="server" id="dvProfileCustAttr">
                                <infsu:CustomAttribute ID="caProfileCustomAttributes" runat="server" Title="Profile Information" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <div class="dvCommon">
                <infs:WclButton ID="btnSubmit" runat="server" Text="Submit" Value="Accept" OnClick="btnSubmit_Click">
                </infs:WclButton>
                <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
                </infs:WclButton>
            </div>
        </div>
    </div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
</asp:Content>
