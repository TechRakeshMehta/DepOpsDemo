<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.InstituteHierarchyNodePackage"
    MaintainScrollPositionOnPostback="true" Title="InstituteHierarchyNodePackage"
    MasterPageFile="~/Shared/ChildPage.master" CodeBehind="InstituteHierarchyNodePackage.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/NodeNotificationSettings.ascx" TagPrefix="infsu" TagName="NodeNotificationSettings" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/PackagePaymentOptions.ascx" TagPrefix="infsu" TagName="PackagePaymentOptions" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/AdditionalDocumentsMapping.ascx" TagPrefix="infsu" TagName="AdditionalDocumentsMapping" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        function FilterInstituteHierarchyPackages(IsPackageAvailableforOrder) {

            if (typeof IsPackageAvailableforOrder != 'undefined') {
                $jQuery('[id$=hdnIsAvailableforOrder]', $jQuery(parent.theForm))[0].value = IsPackageAvailableforOrder;
                var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
                btn.click();
            }
        }

        // Function to validate Payment options
        function ValidatePaymentOption(sender, args) {
            var options = null;
            if (sender.validationGroup == "grpPaymentOptionsSubmit") {
                options = $jQuery("[id$=chkMappedPaymentOption] input:checked");
            }
            else {
                options = $jQuery("[id$=chkPaymentOption] input:checked");
            }
            for (var i = 0; i < options.length; i++) {
                if (options[i].checked) {
                    args.IsValid = true;
                    return false;
                }
            }
            args.IsValid = false;
        }

        function ValidatePermission(sender, args) {
            var permissionOptions = $jQuery("[id$=rblPermissions] input:checked").is(":checked");
            if (permissionOptions) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }

        function ValidatePackagePermission(sender, args) {
            var permissionOptions = $jQuery("[id$=rblPackagePermission] input:checked").is(":checked");
            if (permissionOptions) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }
        function FilterInstituteHierarchyPackageBundle(IsPackageBundleAvailable) {
            if (typeof IsPackageBundleAvailable != 'undefined') {
                $jQuery('[id$=hdnIsPackageBundleAvailableforOrder]', $jQuery(parent.theForm))[0].value = IsPackageBundleAvailable;
                var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
                btn.click();
            }
        }
    </script>
    <div id="divHierarchyNodePackage" runat="server">
        <div class="section" id="divNodePackage" runat="server">
            <div class="content">
                <div class="sxform auto">
                    <asp:Panel ID="Panel5" CssClass="sxpnl" runat="server">
                        <div class='sxro sx5co'>
                            <div class='sxlb m1spn'>
                                <span class="cptn">Include packages which are not available for Order</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rdIsAvailableforOrder" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdIsAvailableforOrder_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="false"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="true" Selected="True">
                                    </asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class='sxro sx5co'>
                            <div class='sxlb m1spn'>
                                <span class="cptn">Include Bundle Packages</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rdIsBundlePackageAvailableforOrder" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdIsBundlePackageAvailableforOrder_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="true" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="false">
                                    </asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                        <asp:HiddenField runat="server" ID="hdnIsPackagesNotAvailableForOrder" Value="true" />
                    </asp:Panel>
                </div>

                <div id="divAddButton" runat="server">
                    <infsu:CommandBar ID="fsucCmdBarNodePackage" runat="server" DefaultPanel="pnlNodePackage"
                        DisplayButtons="Save,Submit,Clear" AutoPostbackButtons="Save,Submit,Clear" SaveButtonText="Add Node"
                        SubmitButtonText="Add Package" SaveButtonIconClass="rbAdd" SubmitButtonIconClass="rbAdd"
                        ButtonPosition="Center" OnSaveClick="CmdBarAddNode_Click" OnSubmitClick="CmdBarAddPackage_Click"
                        ClearButtonText="Manage Node Notifications" OnClearClick="btnNodeNotificationSettings_Click" ClearButtonIconClass="">
                    </infsu:CommandBar>
                </div>
                <div class="content">
                    <div id="divShowNode" runat="server" visible="false">
                        <h1 class="mhdr">
                            <asp:Label ID="lblAddNodeTitle" runat="server" Text="Add Node"></asp:Label>
                        </h1>
                        <div class="content">
                            <div class="sxform auto">
                                <asp:Panel ID="pnlNode" CssClass="sxpnl" runat="server">
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Node Type</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="ddlNodeType" runat="server" DataTextField="INT_Name" DataValueField="INT_ID"
                                                OnSelectedIndexChanged="ddlNodeType_SelectedIndexChanged" AutoPostBack="true">
                                            </infs:WclComboBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvNodeType" ControlToValidate="ddlNodeType"
                                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Node Type."
                                                    InitialValue="--SELECT--" />
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Node</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="ddlNode" runat="server" DataTextField="IN_Name" DataValueField="IN_ID">
                                            </infs:WclComboBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvNode" ControlToValidate="ddlNode"
                                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Node."
                                                    InitialValue="--SELECT--" />
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Is Available for Order</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rbtnAvailabilityAdd" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                <asp:ListItem Text="Yes" Value="yes" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="no"></asp:ListItem>
                                            </asp:RadioButtonList>

                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Payment Option</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm m3spn'>
                                            <asp:CheckBoxList ID="chkPaymentOption" RepeatDirection="Horizontal" runat="server"
                                                DataTextField="Name" DataValueField="PaymentOptionID">
                                            </asp:CheckBoxList>
                                            <div class='vldx'>
                                                <asp:CustomValidator ID="cvPaymentOption" CssClass="errmsg" Display="Dynamic" runat="server"
                                                    EnableClientScript="true" ErrorMessage="Payment Option is required." ValidationGroup="grpFormSubmit"
                                                    ClientValidationFunction="ValidatePaymentOption">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Archival Grace Period(Days)</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclNumericTextBox ID="txtArchivedGracePeriod" runat="server" MinValue="1" MaxValue="2147483647" Type="Number">
                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                            </infs:WclNumericTextBox>
                                        </div>
                                        <div id="dvEffectiveArchival" runat="server" visible="false">
                                            <div class='sxlb'>
                                                <span class="cptn">Effective Archival Grace Period(Days)</span>
                                            </div>
                                            <div class='sxlm'>
                                                <asp:Label ID="lblEffectiveArchival" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Is Employment Type</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rbtnEmployment" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="no" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Splash Page URL</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox ID="txtSplashPageAdd" runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                     <%--   <div class='sxlb'>
                                            <span class="cptn">Before Expiration Frequency</span>
                                        </div>--%>
                                     <%--   <div class='sxlm'>
                                            <infs:WclTextBox ID="txtExpirationFreqAdd" runat="server">
                                            </infs:WclTextBox>
                                            <div class="vldx">
                                                <asp:RegularExpressionValidator Display="Dynamic" ID="revCmpExpirationfreq" runat="server"
                                                    ValidationExpression="^[0-9,]*$" class="errmsg" ControlToValidate="txtExpirationFreqAdd"
                                                    ErrorMessage="Only numeric value is allowed." ValidationGroup="grpFormSubmit">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>--%>
                                        <div class='sxlb'>
                                            <span class="cptn">Is School Approval Required for Credit Card</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rbtnApprovalRequiredBeforePaymentAdd" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="PA_Name" DataValueField="PA_ID" AutoPostBack="false">
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Label ID="lblComplianceNotification" runat="server" Text="Compliance Notification Override Settings"></asp:Label>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="Panel7" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Before Expiration Frequency</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox ID="txtExpirationFreqAdd" runat="server">
                                        </infs:WclTextBox>
                                        <div class="vldx">
                                          <asp:RegularExpressionValidator Display="Dynamic" ID="revCmpExpirationfreq" runat="server"
                                                    ValidationExpression="^[0-9,]*$" class="errmsg" ControlToValidate="txtExpirationFreqAdd"
                                                    ErrorMessage="Only numeric value is allowed." ValidationGroup="grpFormSubmit">
                                                </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">After Expiry Frequency (Days)</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ID="txtAfterExpiryFreqAdd" runat="server" MaxLength="3"
                                            Type="Number">
                                            <NumberFormat DecimalDigits="0" />
                                        </infs:WclNumericTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator2" runat="server"
                                                ValidationExpression="^[0-9]*$" class="errmsg" ControlToValidate="txtAfterExpiryFreqAdd"
                                                ErrorMessage="Only numeric value is allowed." ValidationGroup="grpFormSubmit">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                                     <div class="sbsection">

                    <h1 class="sbhdr">
                        <asp:Label ID="lblSubscriptionNotification" runat="server" Text="Subscription Notification Override Settings"></asp:Label>
                    </h1>
                    <%--   <h1 class="mhdr">Subscription Notification Settings
                    </h1>--%>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="Panel8" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Before Expiry (Days)</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ID="txtAddSubscriptionBeforeExpiry" runat="server" MaxLength="3" Type="Number">
                                            <NumberFormat DecimalDigits="0" />
                                        </infs:WclNumericTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator1" runat="server"
                                                ValidationExpression="^[0-9]*$" class="errmsg" ControlToValidate="txtAddSubscriptionBeforeExpiry"
                                                ErrorMessage="Only numeric value is allowed.">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">After Expiry (Days)</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ID="txtAddSubscriptionAfterExpiry" runat="server" MaxLength="3" Type="Number">
                                            <NumberFormat DecimalDigits="0" />
                                        </infs:WclNumericTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator3" runat="server"
                                                ValidationExpression="^[0-9]*$" class="errmsg" ControlToValidate="txtAddSubscriptionAfterExpiry"
                                                ErrorMessage="Only numeric value is allowed.">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">Email Frequency (Days)</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ID="txtAddSubscriptionExpiryFrequency" runat="server" MaxLength="3" Type="Number">
                                            <NumberFormat DecimalDigits="0" />
                                        </infs:WclNumericTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator4" runat="server"
                                                ValidationExpression="^[0-9]*$" class="errmsg" ControlToValidate="txtAddSubscriptionExpiryFrequency"
                                                ErrorMessage="Only numeric value is allowed.">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <%--    </div>--%>
                    </div>
                </div>
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Select File Extensions To Restrict</span>
                                        </div>
                                        <div class='sxlm' style="overflow:inherit !important;">
                                            <infs:WclComboBox ID="cmbFileTypes" runat="server" DataTextField="Name" DataValueField="FileExtensionID"
                                                CausesValidation="false" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains"
                                                OnClientKeyPressing="openCmbBoxOnTab" AllowCustomText="true" EmptyMessage="--SELECT--">
                                                <Localization CheckAllString="All" />
                                            </infs:WclComboBox>                                            
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <div id="divPackage" runat="server" visible="false">
                        <h1 class="mhdr">
                            <asp:Label ID="lblAddPackageTitle" runat="server" Text="Add Package"></asp:Label>
                        </h1>
                        <div class="content">
                            <div class="sxform auto">
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                                    <div class="sxgrp" id="divSelect" runat="server">
                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class="cptn">Select Package</span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclComboBox ID="cmbMasterPackage" runat="server" ToolTip="Select from a master list" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cmbMasterPackage_SelectedIndexChanged"
                                                    DataTextField="PackageName" DataValueField="CompliancePackageID" OnDataBound="cmbMasterPackage_DataBound">
                                                </infs:WclComboBox>
                                                <div class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvMasterPackage" ControlToValidate="cmbMasterPackage"
                                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Package."
                                                        InitialValue="--SELECT--" />
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
                    <asp:Panel ID="pnlPkgPaymentOptions" runat="server" Visible="false">
                        <infsu:PackagePaymentOptions ID="ucPkgPaymentOptions" runat="server" />
                    </asp:Panel>
                    <div id="divSaveButton" runat="server" visible="false">
                        <infsu:CommandBar ID="fsucCmdBarSaveNode" runat="server" DefaultPanel="pnlNodePackage"
                            DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel" ValidationGroup="grpFormSubmit"
                            ButtonPosition="Right" OnSaveClick="CmdBarSave_Click" OnCancelClick="CmdBarCancel_Click">
                        </infsu:CommandBar>
                    </div>
                </div>
            </div>
        </div>
        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblNodeTitle" runat="server" Text=""></asp:Label>
            </h1>
            <div class="content">

                <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Label ID="lblPaymentOptionsTitle" runat="server" Text="Payment Options"></asp:Label>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="pnlPaymentOption" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Payment Option</span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm m3spn'>
                                        <asp:CheckBoxList ID="chkMappedPaymentOption" RepeatDirection="Horizontal" runat="server"
                                            DataTextField="Name" DataValueField="PaymentOptionID">
                                        </asp:CheckBoxList>
                                        <div class='vldx'>
                                            <asp:CustomValidator ID="cvMappedPaymentOption" CssClass="errmsg" Display="Dynamic"
                                                runat="server" EnableClientScript="true" ErrorMessage="Payment Option is required."
                                                ValidationGroup="grpPaymentOptionsSubmit" ClientValidationFunction="ValidatePaymentOption">
                                            </asp:CustomValidator>
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Is School Approval Required for Credit Card</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:RadioButtonList ID="rbtnApprovalRequiredBeforePayment" runat="server" RepeatDirection="Horizontal"
                                            DataTextField="PA_Name" DataValueField="PA_ID" AutoPostBack="false">
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Label ID="Label3" runat="server" Text="Mobility Automation"></asp:Label>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="pnlMobility" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Enable Mobility Automation</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:CheckBox ID="chkEnableMobility" runat="server" AutoPostBack="true" OnCheckedChanged="chkEnableMobility_CheckedChanged" />
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div id="divMobilityFields" runat="server" visible="false">
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">First Instance Start Date</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclDatePicker ID="dpkrStartDate" runat="server" DateInput-EmptyMessage="Select a date">
                                            </infs:WclDatePicker>
                                            <div class="valdx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" CssClass="errmsg" ControlToValidate="dpkrStartDate"
                                                    Display="Dynamic" ErrorMessage="First Instance Start Date is required." ValidationGroup="grpPaymentOptionsSubmit" />
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Duration Type</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="cmbDurationType" runat="server" ToolTip="Select from a master list"
                                                DataTextField="DT_Name" DataValueField="DT_ID">
                                            </infs:WclComboBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvDurationType" ControlToValidate="cmbDurationType"
                                                    class="errmsg" ValidationGroup="grpPaymentOptionsSubmit" Display="Dynamic" ErrorMessage="Please select Duration Type."
                                                    InitialValue="--SELECT--" />
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Duration</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclNumericTextBox ID="txtDuration" Type="Number" runat="server" MinValue="0"
                                                MaxLength="3">
                                                <NumberFormat AllowRounding="false" DecimalDigits="0" DecimalSeparator="." />
                                            </infs:WclNumericTextBox>
                                            <div class="valdx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvDuration" CssClass="errmsg" ControlToValidate="txtDuration"
                                                    Display="Dynamic" ErrorMessage="Duration is required." ValidationGroup="grpPaymentOptionsSubmit" />
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Instance Interval (Days)</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclNumericTextBox ID="txtInstanceInterval" Type="Number" runat="server" MinValue="0"
                                                MaxLength="3">
                                                <NumberFormat AllowRounding="false" DecimalDigits="0" DecimalSeparator="." />
                                            </infs:WclNumericTextBox>
                                            <div class="valdx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvInstanceInterval" CssClass="errmsg"
                                                    ControlToValidate="txtInstanceInterval" Display="Dynamic" ErrorMessage="Instance Interval (Days) is required."
                                                    ValidationGroup="grpPaymentOptionsSubmit" />
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Successor Node</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="cmbSuccessorNode" runat="server" ToolTip="Select from a master list"
                                                DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="cmbSuccessorNode_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </infs:WclComboBox>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx3co'>
                                        <div>
                                        </div>
                                        <div class='sxlm m3spn'>
                                            <span id="spnEmptyRecord" runat="server" style="display: none;">No Package to display.</span>
                                            <asp:Repeater ID="rptrPackages" runat="server" OnItemDataBound="rptrPackages_ItemDataBound">
                                                <HeaderTemplate>
                                                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                                                    <table id="mytable" cellspacing="0" width="100%" align="center">
                                                        <tr>
                                                            <td style="width: 50%;">Source Package
                                                            </td>
                                                            <td style="width: 50%;">Successor Package
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table id="mytable" cellspacing="0" width="100%" align="center">
                                                        <tr>
                                                            <td style="width: 50%;">
                                                                <span>
                                                                    <%#Eval("CompliancePackage.PackageName")%></span>
                                                                <asp:HiddenField ID="hdnDPPID" Value='<%#Eval("DPP_ID")%>' runat="server" />
                                                            </td>
                                                            <td>
                                                                <div style="width: 50%;">
                                                                    <infs:WclComboBox ID="cmbSuccessorPackage" runat="server" ToolTip="Select from a master list"
                                                                        DataTextField="Name" DataValueField="ID">
                                                                    </infs:WclComboBox>
                                                                </div>
                                                            </td>
                                                            <%--<td style="width: 10%;">
                                                    <asp:CheckBox ID="chkBxAttributeName" runat="server" onclick="EnableDisableRequiredField();" />
                                                    <asp:HiddenField ID="hdnCustomAttributeId" Value='<%#Eval("CA_CustomAttributeID")%>'
                                                        runat="server" />
                                                    <asp:HiddenField ID="hdnCstAttrMappedId" Value="0" runat="server" />
                                                </td>
                                                <td style="width: 10%;">
                                                    <span>Required</span>
                                                </td>
                                                <td id="tdRadioAttribute" runat="server">
                                                    <asp:RadioButton ID="rdbCstAttributeRequiredYes" runat="server" GroupName="grpCustomAttribute"
                                                        Text="Yes &nbsp" />
                                                    <asp:RadioButton ID="rdbCstAttributeRequiredNo" runat="server" Checked="true" GroupName="grpCustomAttribute"
                                                        Text="No" />
                                                </td>--%>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Label ID="lblAGPHeader" runat="server" Text="Archival Grace Period"></asp:Label>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="pnlArchival" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Archival Grace Period(Days)</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ID="txtMappedArchivalGracePeriod" runat="server" MinValue="1" MaxValue="2147483647" Type="Number">
                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                        </infs:WclNumericTextBox>
                                    </div>
                                    <div id="dvMappedEffectiveArchival" runat="server" visible="false">
                                        <div class='sxlb'>
                                            <span class="cptn">Effective Archival Grace Period(Days)</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:Label ID="lblMappedEffectiveArchival" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlNodeAvailability" runat="server">
                    <div class="sbsection">
                        <h1 class="sbhdr">
                            <asp:Literal ID="litAvailability" runat="server" Text="Availability"></asp:Literal>
                        </h1>
                        <div class="sbcontent">
                            <div class="sxform auto">
                                <asp:Panel ID="Panel1" CssClass="sxpnl" runat="server">
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Is Available for Order</span>
                                        </div>
                                        <div class='sxlm m3spn'>
                                            <asp:RadioButtonList ID="rbtnAvailabilityEdit" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="no"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Literal ID="litAdminDataEntryAllow" runat="server" Text="Admin Data Entry"></asp:Literal>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="Panel4" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Is Admin Data Entry Allowed</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:RadioButtonList ID="rbtnAdminDataEntry" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                            <asp:ListItem Text="Default" Value="null" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="no"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Label ID="lblEmployment" runat="server" Text="Employment Type"></asp:Label>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="pnlEmployment" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Is Employment Type</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:RadioButtonList ID="rbtnEmploymentEdit" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                            <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="no" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>

                <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Label ID="lblSplashPage" runat="server" Text="Splash Page URL"></asp:Label>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="Panel2" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Splash Page URL</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox ID="txtSplashPageEdit" runat="server">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>

                <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Label ID="lblExpirationFreq" runat="server" Text="Compliance Notification Override Settings"></asp:Label>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="Panel3" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Before Expiration Frequency</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox ID="txtBeforeExpiryFreqEdit" runat="server">
                                        </infs:WclTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="revCmpItemBeforeExpiryEdit" runat="server"
                                                ValidationExpression="^[0-9,]*$" class="errmsg" ControlToValidate="txtBeforeExpiryFreqEdit"
                                                ErrorMessage="Only numeric value is allowed." ValidationGroup="grpPaymentOptionsSubmit">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">After Expiry Frequency (Days)</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ID="txtAfterExpiryFreqEdit" runat="server" MaxLength="3"
                                            Type="Number">
                                            <NumberFormat DecimalDigits="0" />
                                        </infs:WclNumericTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="revCmpItemExpiryFrqncy" runat="server"
                                                ValidationExpression="^[0-9]*$" class="errmsg" ControlToValidate="txtAfterExpiryFreqEdit"
                                                ErrorMessage="Only numeric value is allowed.">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="sbsection">

                    <h1 class="sbhdr">
                        <asp:Label ID="lblSubscriptionHeader" runat="server" Text="Subscription Notification Override Settings"></asp:Label>
                    </h1>
                    <%--   <h1 class="mhdr">Subscription Notification Settings
                    </h1>--%>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="Panel6" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Before Expiry (Days)</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ID="txtSubscriptionBeforeExpiry" runat="server" MaxLength="3" Type="Number">
                                            <NumberFormat DecimalDigits="0" />
                                        </infs:WclNumericTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="revBeforeExpiry" runat="server"
                                                ValidationExpression="^[0-9]*$" class="errmsg" ControlToValidate="txtSubscriptionBeforeExpiry"
                                                ErrorMessage="Only numeric value is allowed.">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">After Expiry (Days)</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ID="txtSubscriptionAfterExpiry" runat="server" MaxLength="3" Type="Number">
                                            <NumberFormat DecimalDigits="0" />
                                        </infs:WclNumericTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="revAfterExpiry" runat="server"
                                                ValidationExpression="^[0-9]*$" class="errmsg" ControlToValidate="txtSubscriptionAfterExpiry"
                                                ErrorMessage="Only numeric value is allowed.">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">Email Frequency (Days)</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ID="txtSubscriptionExpiryFrequency" runat="server" MaxLength="3" Type="Number">
                                            <NumberFormat DecimalDigits="0" />
                                        </infs:WclNumericTextBox>
                                        <div class="vldx">
                                            <asp:RegularExpressionValidator Display="Dynamic" ID="revExpiryFrequency" runat="server"
                                                ValidationExpression="^[0-9]*$" class="errmsg" ControlToValidate="txtSubscriptionExpiryFrequency"
                                                ErrorMessage="Only numeric value is allowed.">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <%--    </div>--%>
                    </div>
                </div>
                
                <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Label ID="lblRestrictedFileType" runat="server" Text="Select File Extensions To Restrict"></asp:Label>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="pnlRestrictedFileType" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Select File Extensions To Restrict </span>
                                    </div>
                                    <div class='sxlm' style="overflow:inherit !important;">
                                        <infs:WclComboBox ID="cmbMappedFileTypes" runat="server" DataTextField="Name" DataValueField="FileExtensionID"
                                            CausesValidation="false" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains"
                                            OnClientKeyPressing="openCmbBoxOnTab" AllowCustomText="true" EmptyMessage="--SELECT--">
                                            <Localization CheckAllString="All" />
                                        </infs:WclComboBox>                                        
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>

                <%--UAT 3683 start--%>
                <div class="sbsection">
                    <h1 class="sbhdr">
                        <asp:Label ID="lblOptionalCategorySetting" runat="server" Text="Optional Category Setting"></asp:Label>
                    </h1>
                    <div class="sbcontent">
                        <div class="sxform auto">
                            <asp:Panel ID="pnlOptionalCategorySetting" CssClass="sxpnl" runat="server">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Execute Compliance Rule When Optional Category Compliance Rule Met</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:RadioButtonList ID="rbtnOptionalCategorySettingEdit" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                            <asp:ListItem Text="Default" Value="null" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="no"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <%--UAT 3683 end--%>

                <infsu:CommandBar ID="fsucCmdBarPaymentOption" runat="server" DefaultPanel="pnlPaymentOption"
                    DisplayButtons="Save" AutoPostbackButtons="Save" ValidationGroup="grpPaymentOptionsSubmit"
                    ButtonPosition="Right" OnSaveClick="BtnSavePaymentOption_Click">
                </infsu:CommandBar>
            </div>
        </div>
        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblNodesTitle" runat="server" Text="Nodes"></asp:Label>
            </h1>
            <div class="content">
                <div id="dvNode" runat="server" class="swrap">
                    <infs:WclGrid runat="server" ID="grdNode" AllowPaging="True" AutoGenerateColumns="False"
                        AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                        GridLines="Both" EnableDefaultFeatures="true" ShowAllExportButtons="False" ShowExtraButtons="False"
                        PageSize="10" NonExportingColumns="DeleteColumn" OnNeedDataSource="grdNode_NeedDataSource"
                        OnDeleteCommand="grdNode_DeleteCommand" OnItemDataBound="grdNode_ItemDataBound"
                        OnInit="grdNode_Init" OnRowDrop="grdNode_RowDrop">

                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                            HideStructureColumns="true" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                            Pdf-PageRightMargin="20mm">
                            <Excel AutoFitImages="true" />
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="true" AllowAutoScrollOnDragDrop="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="DPM_ID">
                            <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add Node" ShowExportToCsvButton="false"
                                ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="IN_Name" FilterControlAltText="Filter Name column"
                                    HeaderText="Name" SortExpression="IN_Name" UniqueName="Name">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IN_Label" FilterControlAltText="Filter Label column"
                                    HeaderText="Label" SortExpression="IN_Label" UniqueName="Label">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IN_Description" FilterControlAltText="Filter Description column"
                                    HeaderText="Description" SortExpression="IN_Description" UniqueName="Description">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="INT_Name" FilterControlAltText="Filter NodeType column"
                                    HeaderText="Node Type" SortExpression="INT_Name" UniqueName="NodeType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DPM_DisplayOrder" FilterControlAltText="Filter DPM_DisplayOrder column"
                                    HeaderText="Display Sequence" SortExpression="DPM_DisplayOrder" UniqueName="DPM_DisplayOrder">
                                </telerik:GridBoundColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Node?"
                                    Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle CssClass="tplcohdr" />
                                    <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                                </telerik:GridButtonColumn>

                            </Columns>

                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />

                        </MasterTableView>
                        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>
        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblPackageTitle" runat="server" Text="Packages"></asp:Label>
            </h1>
            <div class="content">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdPackage" AllowPaging="True" AutoGenerateColumns="False"
                        AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                        EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false"
                        OnNeedDataSource="grdPackage_NeedDataSource" OnDeleteCommand="grdPackage_DeleteCommand"
                        OnItemDataBound="grdPackage_ItemDataBound" OnInit="grdPackage_Init">
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                        </ExportSettings>

                        <ClientSettings EnableRowHoverStyle="true">

                            <Selecting AllowRowSelect="true"></Selecting>

                        </ClientSettings>

                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="DPP_ID">

                            <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add New Package"
                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                ShowRefreshButton="true" />

                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>

                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>

                            <Columns>

                                <telerik:GridBoundColumn DataField="CompliancePackage.PackageName" FilterControlAltText="Filter PackageName column"
                                    HeaderText="Package Name" SortExpression="CompliancePackage.PackageName" UniqueName="PackageName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CompliancePackage.PackageLabel" FilterControlAltText="Filter PackageLabel column"
                                    HeaderText="Package Label" SortExpression="CompliancePackage.PackageLabel" UniqueName="PackageLabel">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CompliancePackage.Description" FilterControlAltText="Filter Description column"
                                    HeaderText="Description" SortExpression="CompliancePackage.Description" UniqueName="Description">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CompliancePackage.ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                                    HeaderText="Screen Label" SortExpression="CompliancePackage.ScreenLabel" UniqueName="ScreenLabel">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="IsActive" FilterControlAltText="Filter IsActive column"
                                    HeaderText="Is Active" SortExpression="CompliancePackage.IsActive" UniqueName="IsActive">
                                    <ItemTemplate>
                                        <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("CompliancePackage.IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnCompliancePackageID" runat="server" Value='<%#Eval("CompliancePackage.CompliancePackageID")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                                    HeaderText="Tenant" SortExpression="TenantName" UniqueName="TenantName">
                                </telerik:GridBoundColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Package?"
                                    Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle CssClass="tplcohdr" />
                                    <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings EditFormType="Template">
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                        </MasterTableView>
                        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>
        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblScreeningPackageTitle" runat="server" Text="Available Packages"></asp:Label>
            </h1>
            <div class="content">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdScreeningPackage" AllowPaging="True" AutoGenerateColumns="False"
                        AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                        EnableDefaultFeatures="False" ShowAllExportButtons="false" ShowExtraButtons="false"
                        OnNeedDataSource="grdScreeningPackage_NeedDataSource" OnInit="grdScreeningPackage_Init">
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>

                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top">
                            <CommandItemSettings ShowAddNewRecordButton="false"
                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                ShowRefreshButton="true" />
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter Package Name column"
                                    HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PackageType" FilterControlAltText="Filter PackageType column"
                                    HeaderText="Package Type" SortExpression="PackageType" UniqueName="PackageType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IsBundlePackage" FilterControlAltText="Filter Is Bundle Package column"
                                    HeaderText="Is Bundle Package" SortExpression="IsBundlePackage" UniqueName="IsBundlePackage">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <EditFormSettings EditFormType="Template">
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                        </MasterTableView>
                        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>
        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section" id="dvUserPermission" runat="server">
            <h1 class="mhdr">
                <asp:Label ID="Label1" runat="server" Text="Permission"></asp:Label>
            </h1>
            <div class="content">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdUsrPermission" AllowPaging="True" PageSize="10"
                        AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" OnNeedDataSource="grdUsrPermission_NeedDataSource"
                        OnItemCreated="grdUsrPermission_ItemCreated" OnInsertCommand="grdUsrPermission_InsertCommand"
                        OnUpdateCommand="grdUsrPermission_UpdateCommand" OnDeleteCommand="grdUsrPermission_DeleteCommand"
                        ShowAllExportButtons="False" NonExportingColumns="EditCommandColumn, DeleteColumn">
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                            Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="HierarchyPermissionID">
                            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Map User Permissions"
                                ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="UserFirstName" FilterControlAltText="Filter UserFirstName column"
                                    HeaderText="First Name" SortExpression="UserFirstName" UniqueName="UserFirstName"
                                    HeaderStyle-Width="130">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UserLastName" FilterControlAltText="Filter UserLastName column"
                                    HeaderText="Last Name" SortExpression="UserLastName" UniqueName="UserLastName"
                                    HeaderStyle-Width="130">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column"
                                    HeaderText="User Name" HeaderStyle-Width="50px" SortExpression="UserName" UniqueName="UserName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PermissionName" FilterControlAltText="Filter PermissionName column"
                                    HeaderText="Permission" SortExpression="PermissionName" UniqueName="PermissionName"
                                    HeaderStyle-Width="130">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VerificationPermissionName" FilterControlAltText="Filter VerificationPermissionName column"
                                    HeaderText="Verification Permission" SortExpression="VerificationPermissionName" UniqueName="VerificationPermissionName"
                                    HeaderStyle-Width="130">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProfilePermissionName" FilterControlAltText="Filter ProfilePermissionName column"
                                    HeaderText="Profile Permission" SortExpression="ProfilePermissionName" UniqueName="ProfilePermissionName"
                                    HeaderStyle-Width="130">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OrderQueuePermissionName" FilterControlAltText="Filter OrderQueuePermissionName column"
                                    HeaderText="Order Queue Permission" SortExpression="OrderQueuePermissionName" UniqueName="OrderQueuePermissionName"
                                    HeaderStyle-Width="130">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Package Permissions" DataField="PackagePermissionName" SortExpression="PackagePermissionName"
                                    UniqueName="PackagePermissionName" HeaderStyle-Width="130" FilterControlAltText="Filter PackagePermissionName Column">
                                </telerik:GridBoundColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                                    <HeaderStyle Width="130px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete?"
                                    Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle Width="30px" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings EditFormType="Template">
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                                <FormTemplate>
                                    <div class="section">
                                        <h1 class="mhdr">
                                            <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Map User Permissions" : "Edit User Permission"%>'
                                                runat="server"></asp:Label>
                                        </h1>
                                        <div class="content">
                                            <div class="sxform auto">
                                                <div class="msgbox">
                                                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                                </div>
                                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMUser">
                                                    <div class='sxro sx2co'>
                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblUserName" runat="server" AssociatedControlID="ddlHierPerUser" Text="Select User" CssClass="cptn">                                                        
                                                            </asp:Label><span class='reqd <%# (Container is GridEditFormInsertItem) ? "" : "nodisp" %>'>*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclComboBox ID="ddlHierPerUser" runat="server" AutoPostBack="true" DataTextField="FirstName"
                                                                MaxHeight="200px" DataValueField="OrganizationUserID" OnSelectedIndexChanged="ddlHierPerUser_SelectedIndexChanged">
                                                            </infs:WclComboBox>
                                                            <div class='vldx'>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvHierPerUser" ControlToValidate="ddlHierPerUser"
                                                                    class="errmsg" ValidationGroup="grpValdUsrPermission" Display="Dynamic" ErrorMessage="User is required."
                                                                    InitialValue="--SELECT--" />
                                                            </div>
                                                            <infs:WclTextBox Enabled="false" runat="server" Visible="false" ID="txtHierUser">
                                                            </infs:WclTextBox>
                                                        </div>
                                                        <div class='sxlb'>
                                                            <asp:Label ID="Label2" runat="server" AssociatedControlID="ddlHierPerUser" Text="Permissions" CssClass="cptn"></asp:Label><span
                                                                class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <asp:RadioButtonList ID="rblPermissions" runat="server" RepeatDirection="Horizontal"
                                                                DataTextField="PER_Description" DataValueField="PER_ID">
                                                            </asp:RadioButtonList>
                                                            <div class='vldx'>
                                                                <asp:CustomValidator ID="cvPermission" CssClass="errmsg" Display="Dynamic" runat="server"
                                                                    EnableClientScript="true" ErrorMessage="Permission is required." ValidationGroup="grpValdUsrPermission"
                                                                    ClientValidationFunction="ValidatePermission">
                                                                </asp:CustomValidator>
                                                            </div>
                                                        </div>
                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>
                                                    <div class='sxro sx2co'>
                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblVerificationPermission" runat="server" Text="Verification Permissions" CssClass="cptn"></asp:Label><span
                                                                class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <asp:RadioButtonList ID="rblVerificationPermission" runat="server" RepeatDirection="Horizontal"
                                                                DataTextField="PER_Description" DataValueField="PER_ID">
                                                            </asp:RadioButtonList>
                                                        </div>

                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblProfilePermission" runat="server" Text="Profile Permissions" CssClass="cptn"></asp:Label><span
                                                                class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <asp:RadioButtonList ID="rblProfilePermission" runat="server" RepeatDirection="Horizontal"
                                                                DataTextField="PER_Description" DataValueField="PER_ID">
                                                            </asp:RadioButtonList>
                                                        </div>

                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>

                                                    <div class='sxro sx2co'>
                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblOrderQueuePermission" runat="server" Text="Order Queue Permissions" CssClass="cptn"></asp:Label><span
                                                                class="reqd">*</span>
                                                        </div>

                                                        <div class='sxlm'>
                                                            <asp:RadioButtonList ID="rblOrderQueuePermission" runat="server" RepeatDirection="Horizontal"
                                                                DataTextField="PER_Description" DataValueField="PER_ID">
                                                            </asp:RadioButtonList>
                                                        </div>
                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblPackagePermissions" runat="server" Text="Package Permissions" CssClass="cptn"></asp:Label>
                                                            <span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <asp:RadioButtonList ID="rblPackagePermission" runat="server" RepeatDirection="Horizontal"
                                                                DataTextField="PER_Description" DataValueField="PER_ID">
                                                            </asp:RadioButtonList>
                                                            <div class='vldx'>
                                                                <asp:CustomValidator ID="cvPackagePermission" CssClass="errmsg" Display="Dynamic" runat="server"
                                                                    EnableClientScript="true" ErrorMessage="Permission is required." ValidationGroup="grpValdUsrPermission"
                                                                    ClientValidationFunction="ValidatePackagePermission">
                                                                </asp:CustomValidator>
                                                            </div>
                                                        </div>
                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>

                                                    <div class='sxro sx2co'>
                                                        <div class='sxlb'>
                                                            <span class="cptn">Apply Permission on Background Also</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclButton runat="server" ID="chkApplyOnBoth" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false" Checked="true">
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                                                </ToggleStates>
                                                            </infs:WclButton>
                                                        </div>
                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                            </div>
                                            <infsu:CommandBar ID="fsucCmdBarMUser" runat="server" GridMode="true" DefaultPanel="pnlMUser"
                                                ExtraButtonIconClass="icnreset" GridInsertText="Save" GridUpdateText="Save" ValidationGroup="grpValdUsrPermission" />
                                            <%-- <infsu:CommandBar ID="fsucCmdBarMUser" runat="server" DefaultPanel="pnlMUser" TabIndexAt="8">
                                            <ExtraCommandButtons>
                                                <infs:WclButton runat="server" ID="btnSaveForm" ValidationGroup="grpValdUsrPermission"
                                                    Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                                                    <Icon PrimaryIconCssClass="rbSave" />
                                                </infs:WclButton>
                                                <infs:WclButton runat="server" ID="btnCancelForm" Text="Cancel" CommandName="Cancel">
                                                    <Icon PrimaryIconCssClass="rbCancel" />
                                                </infs:WclButton>
                                            </ExtraCommandButtons>
                                        </infsu:CommandBar>--%>
                                        </div>
                                    </div>
                                </FormTemplate>
                            </EditFormSettings>
                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                        </MasterTableView>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>

        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section" id="dvAdditionalDocuments" runat="server">
            <h1 class="mhdr">
                <asp:Label ID="Label4" runat="server" Text="Map Additional Documents"></asp:Label>
            </h1>
            <div class="content">
                <infsu:AdditionalDocumentsMapping runat="server" ID="AdditionalDocumentsMapping" />
            </div>
        </div>

    </div>
    <div id="divNodeNotificationSettings" runat="server">
        <infsu:NodeNotificationSettings runat="server" ID="NodeNotificationSettings" />
    </div>
</asp:Content>
