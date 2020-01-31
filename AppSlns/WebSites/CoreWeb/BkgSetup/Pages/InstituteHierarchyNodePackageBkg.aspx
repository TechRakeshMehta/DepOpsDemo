<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstituteHierarchyNodePackageBkg.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.InstituteHierarchyNodePackageBkg" MaintainScrollPositionOnPostback="true" Title="InstituteHierarchyNodePackage"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/NodeNotificationSettings.ascx" TagPrefix="infsu" TagName="NodeNotificationSettings" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/Permissions.ascx" TagPrefix="infsu" TagName="Permissions" %>
<%@ Register Src="~/BkgSetup/UserControl/ReviewCriteriaHierarchyMapping.ascx" TagPrefix="infsu" TagName="ReviewCriteria" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/AdditionalDocumentsMapping.ascx" TagPrefix="infsu" TagName="AdditionalDocumentsMapping" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/PackagePaymentOptions.ascx" TagPrefix="infsu" TagName="PackagePaymentOptions" %>
<%@ Register Src="~/FingerPrintSetUp/UserControl/LocationTenantMapping.ascx" TagPrefix="uc" TagName="LocTenantMapping" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/AdminEntryAcctSettingPermissions.ascx" TagPrefix="infsu" TagName="AdminEntryAcctSettingPermissions" %>
<%@ Register Src="~/BkgSetup/UserControl/ContentEditor.ascx" TagPrefix="infsu" TagName="ContentEditor" %>
<%@ Register Src="~/BkgSetup/UserControl/AdminEntryNodeSpecificTemplates.ascx" TagPrefix="infsu" TagName="AdminEntryNodeSpecificTemplates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .paymentOption {
            background-color: #EEEEEE !important;
            opacity: 1 !important;
        }
    </style>
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

        function KeyPress(sender, args) {
            if (args.get_keyCharacter() == sender.get_numberFormat().DecimalSeparator || args.get_keyCharacter() == '-') {
                args.set_cancel(true);
            }
        }

        function ValidateAvailability(sender, args) {
            var options = $jQuery("[id$=chkAvailability] input:checked");
            for (var i = 0; i < options.length; i++) {
                if (options[i].checked) {
                    args.IsValid = true;
                    return false;
                }
            }
            args.IsValid = false;
        }

        //UAT-3268
        function ManageAdditionalPrice(sender) {
            //debugger;
            var selectedValue = $jQuery("[id$=rblAdditionalPrice] [type='radio']:checked").val();

            $jQuery("[id$=hdnIsAdditionalPriceAvailable]").val(selectedValue);

            if (selectedValue == "True") {
                $jQuery("[id$=dvAdditionalPrice]").attr('style', 'display:block;');
                EnableValidator($jQuery("[id$=rfvAdditionalPrice]")[0].id);
                EnableValidator($jQuery("[id$=rfvMinPrice]")[0].id);
                EnableValidator($jQuery("[id$=rfvAdditionalPricePaymentOption]")[0].id);

            }
            else {

                $jQuery("[id$=dvAdditionalPrice]").attr('style', 'display:none;');
                $jQuery("[id$=txtAdditionalPrice]").val("");
                $jQuery("[id$=rbtnAdditionalPricePaymentOption] [type='radio']:checked").removeAttr('checked');
                DisableValidator($jQuery("[id$=rfvAdditionalPrice]")[0].id);
                DisableValidator($jQuery("[id$=rfvMinPrice]")[0].id);
                DisableValidator($jQuery("[id$=rfvAdditionalPricePaymentOption]")[0].id);
            }
        }
        function EnableValidator(id) {
            if ($jQuery('#' + id)[0] != undefined) {
                ValidatorEnable($jQuery('#' + id)[0], true);
                $jQuery('#' + id).hide();
            }
        }

        function DisableValidator(id) {
            if ($jQuery('#' + id)[0] != undefined) {
                ValidatorEnable($jQuery('#' + id)[0], false);
            }
        }
        function FilterInstituteHierarchyPackageBundle(IsPackageBundleAvailable) {
            if (typeof IsPackageBundleAvailable != 'undefined') {
                $jQuery('[id$=hdnIsPackageBundleAvailableforOrder]', $jQuery(parent.theForm))[0].value = IsPackageBundleAvailable;
                var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
                btn.click();
            }
        }

       <%-- function changeValueForAdminEntry(args) {
            //debugger;
            //alert('new package for admin entry');
            var button = $find("<%= chkHRPortal.ClientID %>");
            if (args.get_checked()) {
                button.set_checked(false);
            }
            else {
                button.set_checked(true);
            }
        }--%>

       <%-- function changeValueForAvailableOrder(args) {
            //debugger;
            //alert('new package for available order');
            var button = $find("<%= chkAvailableForOrder.ClientID %>");
            if (args.get_checked()) {
                button.set_checked(false);
            }
            else {
                button.set_checked(true);
            }
        }--%>
</script>

    <div id="divHierarchyNodePackage" runat="server">
        <div class="msgbox">
            <asp:Label ID="lblMessage" runat="server" CssClass="info"> </asp:Label>
        </div>
        <div class="section" id="divNodePackage" runat="server">
            <div class="content">
                <div class="sxform auto">
                    <asp:Panel ID="Panel4" CssClass="sxpnl" runat="server">
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
                    </asp:Panel>
                </div>
                <div id="divAddButton" runat="server">
                    <infsu:CommandBar ID="fsucCmdBarNodePackage" runat="server" DefaultPanel="pnlNodePackage"
                        DisplayButtons="Save,Submit,Clear,Cancel,Extra" AutoPostbackButtons="Save,Submit,Clear,Cancel,Extra" SaveButtonText="Add Node"
                        SubmitButtonText="Add Package" SaveButtonIconClass="rbAdd" SubmitButtonIconClass="rbAdd"
                        ButtonPosition="Center" OnSaveClick="CmdBarAddNode_Click" OnSubmitClick="CmdBarAddPackage_Click"
                        CancelButtonIconClass="rbAdd" CancelButtonText="Map External Vendor Account" OnCancelClick="CmdBarAddExtVendorAcct_Click"
                        ExtraButtonIconClass="rbAdd" ExtraButtonText="Map Regulatory Entity" OnExtraClick="CmdBarAddRegulatoryEntity_Click"
                        ClearButtonText="Manage Contacts" OnClearClick="btnNodeContactsSettings_Click" ClearButtonIconClass="">
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
                                        <div class='sxlm m2spn'>
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
                                            <span class="cptn">Is Employment Type</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rbtnEmployment" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="no" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Splash Page URL</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox ID="txtSplashPageAdd" runat="server">
                                            </infs:WclTextBox>
                                        </div>
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

                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Background Package PDF Inclusion Option</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rbtnPDFInclusionAdd" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="Name" DataValueField="PDFInclusionID" AutoPostBack="false">
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Results Sent Directly To Applicant Option</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rbtResultSentToApplicantAdd" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="RSTA_Name" DataValueField="RSTA_ID" AutoPostBack="false">
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Exempt applicant(s) from rotation</span>
                                        </div>
                                        <div class='sxlm'>
                                            <%--<asp:RadioButtonList ID="rdAddNodeExemptedInRotaion" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                <asp:ListItem Text="Default" Value=""></asp:ListItem>
                                            </asp:RadioButtonList>--%>

                                            <asp:RadioButtonList ID="rdNodeExemptedInRotaionAdd" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="HNET_Name" DataValueField="HNET_ID" AutoPostBack="false">
                                            </asp:RadioButtonList>
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
                                                    DataTextField="BPA_Name" DataValueField="BPA_ID" OnDataBound="cmbMasterPackage_DataBound" OnSelectedIndexChanged="cmbMasterPackage_SelectedIndexChanged">
                                                </infs:WclComboBox>
                                                <div class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvMasterPackage" ControlToValidate="cmbMasterPackage"
                                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Package."
                                                        InitialValue="--SELECT--" />
                                                </div>
                                            </div>
                                            <div class='sxlb'>
                                                <span class="cptn">Base Price</span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclNumericTextBox ID="txtPrice" Type="Currency" runat="server" MinValue="0" MaxLength="9">
                                                </infs:WclNumericTextBox>
                                                <div class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPrice" ControlToValidate="txtPrice"
                                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Price is required." />
                                                </div>
                                            </div>
                                            <div class='sxlb'>
                                                <span class="cptn">Is Exclusive</span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <asp:RadioButtonList ID="rbtnExclusive" runat="server" RepeatDirection="Horizontal"
                                                    CssClass="radio_list" AutoPostBack="false">
                                                    <asp:ListItem Text="Yes" Value="True" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvExclusive" ControlToValidate="rbtnExclusive"
                                                        Display="Dynamic" CssClass="errmsg" Text="Is Exclusive is required." ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>
                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class="cptn">Transmit To Vendor</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclButton runat="server" ID="chkTransmitToVendor" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                                    </ToggleStates>
                                                </infs:WclButton>
                                            </div>
                                            <div class='sxlb'>
                                                <span class="cptn">Does Require First Review</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclButton runat="server" ID="chkFirstReview" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="true"
                                                    Value="false" OnToggleStateChanged="chkFirstReview_ToggleStateChanged">
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                                    </ToggleStates>
                                                </infs:WclButton>
                                            </div>
                                            <div class='sxlb'>
                                                <span class="cptn">Supplemental Type</span><span id="spSupptype" class="reqd" runat="server"
                                                    visible="false">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclComboBox ID="cmbSupplementalType" runat="server" Enabled="false"
                                                    DataTextField="PST_Name" DataValueField="PST_ID" EmptyMessage="--Select--">
                                                </infs:WclComboBox>
                                                <div class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvSupplementalType" ControlToValidate="cmbSupplementalType" Enabled="false"
                                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Supplemental Type." />
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>
                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class="cptn">Instruction</span>
                                            </div>
                                            <div class='sxlm m2spn'>
                                                <infs:WclTextBox runat="server" ID="txtInstruction"
                                                    MaxLength="1024">
                                                </infs:WclTextBox>
                                            </div>
                                            <div class='sxlb'>
                                                <span class="cptn">Available For Order</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclButton runat="server" ID="chkAvailableForOrder" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                    AutoPostBack="false">
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                                    </ToggleStates>
                                                </infs:WclButton>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>

                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class="cptn">Price Text</span>
                                            </div>
                                            <div class='sxlm m2spn'>
                                                <infs:WclTextBox runat="server" ID="txtPriceText"
                                                    MaxLength="1024">
                                                </infs:WclTextBox>
                                            </div>

                                            <div class='sxlb'>
                                                <span class="cptn">Available For Admin Entry</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclButton runat="server" ID="chkHRPortal" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                    AutoPostBack="false">
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                                    </ToggleStates>
                                                </infs:WclButton>
                                            </div>

                                            <%--<div id="divMaxYearForResidence" runat="server" visible="false">
                                                <div class='sxlb'>
                                                    <span class="cptn">Maximum Number Of Years For Residence</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtMaxNumberOfYearforResidence"
                                                        MaxValue="100" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                                        MinValue="0">
                                                        <NumberFormat AllowRounding="false" DecimalDigits="0" />
                                                        <ClientEvents OnKeyPress="KeyPress" />
                                                    </infs:WclNumericTextBox>
                                                </div>
                                            </div>--%>
                                            <div class='sxroend'>
                                            </div>
                                        </div>


                                        <div class='sxro sx3co' id="divMaxYearForResidence" runat="server" visible="false">
                                            <div runat="server">
                                                <div class='sxlb'>
                                                    <span class="cptn">Maximum Number Of Years For Residence</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtMaxNumberOfYearforResidence"
                                                        MaxValue="100" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                                        MinValue="0">
                                                        <NumberFormat AllowRounding="false" DecimalDigits="0" />
                                                        <ClientEvents OnKeyPress="KeyPress" />
                                                    </infs:WclNumericTextBox>
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>

                                        <div class='sxro sx3co'>
                                            <div id="dvIsAdditionalPriceAvailable" runat="server">
                                                <div class='sxlb'>
                                                    <span class="cptn">Include Additional Price</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButtonList ID="rblAdditionalPrice" runat="server" RepeatDirection="Horizontal"
                                                        CssClass="radio_list" AutoPostBack="false" onclick="ManageAdditionalPrice(this);">
                                                        <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                            <div id="dvAdditionalPrice" style="display: none">
                                                <div class='sxlb'>
                                                    <span class="cptn">Additional Price</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclNumericTextBox ID="txtAdditionalPrice" Type="Currency" runat="server" MinValue="0" MaxLength="9">
                                                    </infs:WclNumericTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAdditionalPrice" ControlToValidate="txtAdditionalPrice" Enabled="false"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Additional Price is required." />
                                                        <asp:CompareValidator runat="server" ID="rfvMinPrice" ValueToCompare="0" Type="Currency" Operator="GreaterThan" ControlToValidate="txtAdditionalPrice" Enabled="false"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Additional Price should be greater than zero." />
                                                    </div>
                                                </div>
                                                <div class='sxlb paymentOption'>
                                                    <span class="cptn">Payment Option</span> <span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButtonList ID="rbtnAdditionalPricePaymentOption" RepeatDirection="Horizontal" runat="server" DataTextField="Name" DataValueField="PaymentOptionID">
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAdditionalPricePaymentOption" ControlToValidate="rbtnAdditionalPricePaymentOption" Enabled="false"
                                                        Display="Dynamic" CssClass="errmsg" Text="Payment option is required." ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>

                                        <div class='sxro sx3co' id="HRPortal">
                                            <%--  <div class='sxlb'>
                                                <span class="cptn">Available For Admin Entry</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclButton runat="server" ID="chkHRPortal" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                    AutoPostBack="false" onclienttogglestatechanged="changeValueForAvailableOrder">
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                                    </ToggleStates>
                                                </infs:WclButton>
                                            </div>--%>
                                            <div class='sxroend'>
                                            </div>
                                        </div>

                                        <asp:HiddenField ID="hdnIsAdditionalPriceAvailable" runat="server" Value="" />
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>

                    <asp:Panel runat="server" ID="pnlPkgPaymentOptions" Visible="false">
                        <infsu:PackagePaymentOptions ID="ucPkgPaymentOptions" runat="server" />
                    </asp:Panel>

                    <div id="divExtVendorAcct" runat="server" visible="false">
                        <div class="content">
                            <h1 class="mhdr">
                                <asp:Label ID="lblEHAttr" Text="Add New External Vendor Account" runat="server" /></h1>
                            <div class="sxform auto">
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlExtVendorAcct">
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">External Vendor </span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="cmbExtVendor" AutoPostBack="true" runat="server" DataTextField="EVE_Name"
                                                DataValueField="EVE_ID" OnSelectedIndexChanged="cmbExtVendor_SelectedIndexChanged">
                                            </infs:WclComboBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvExtVendor" ControlToValidate="cmbExtVendor"
                                                    InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                    Text="External Vendor is required." />
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">External Vendor Account</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="cmbExtVendorAcct" AutoPostBack="true" runat="server" DataTextField="EVA_AccountName"
                                                DataValueField="EVA_ID">
                                                <%-- OnSelectedIndexChanged="cmbExtVendorAcct_SelectedIndexChanged"--%>
                                            </infs:WclComboBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvExtVendorAcct" ControlToValidate="cmbExtVendorAcct"
                                                    InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                    Text="External Vendor Account is required." />
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>

                    <div id="divAddRegulatoryEntity" runat="server" visible="false">
                        <div class="content">
                            <h1 class="mhdr">
                                <asp:Label ID="Label2" Text="Map New Regulatory Entity" runat="server" /></h1>
                            <div class="sxform auto">
                                <asp:Panel runat="server" CssClass="sxpnl" ID="Panel1">
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Regulatory Entity</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="cmbRegulatoryEntity" AutoPostBack="true" runat="server" DataTextField="RET_Name"
                                                DataValueField="RET_ID">
                                            </infs:WclComboBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvRegulatoryEntity" ControlToValidate="cmbRegulatoryEntity"
                                                    InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                    Text="Regulatory Entity is required." />
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
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
                    <asp:Panel ID="pnlAvailablity" runat="server">
                        <div class="sbsection">
                            <h1 class="sbhdr">
                                <asp:Literal ID="litAvailability" runat="server" Text="Availability"></asp:Literal>
                            </h1>
                            <div class="sbcontent">
                                <div class="sxform auto">
                                    <asp:Panel ID="Panel2" CssClass="sxpnl" runat="server">
                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class="cptn">Is Available for Order</span><span class="reqd">*</span>
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
                    <asp:Panel ID="pnlOuterEmployment" runat="server">
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
                    </asp:Panel>
                    <div class="sbsection">
                        <h1 class="sbhdr">
                            <asp:Label ID="lblSplashPage" runat="server" Text="Splash Page URL"></asp:Label>
                        </h1>
                        <div class="sbcontent">
                            <div class="sxform auto">
                                <asp:Panel ID="Panel3" CssClass="sxpnl" runat="server">
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
                            <asp:Label ID="lblPDFInclusion" runat="server" Text="Result PDF Inclusion Settings"></asp:Label>
                        </h1>
                        <div class="sbcontent">
                            <div class="sxform auto">
                                <asp:Panel ID="Panel5" CssClass="sxpnl" runat="server">
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Background Package PDF inclusion Option</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rbtnPDFInclusion" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="Name" DataValueField="PDFInclusionID" AutoPostBack="false">
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
                            <asp:Label ID="Label5" runat="server" Text="Results Sent Directly To Applicant Settings"></asp:Label>
                        </h1>
                        <div class="sbcontent">
                            <div class="sxform auto">
                                <asp:Panel ID="Panel6" CssClass="sxpnl" runat="server">
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Results Sent Directly To Applicant Option</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rbtResultSentToApplicant" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="RSTA_Name" DataValueField="RSTA_ID" AutoPostBack="false">
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
                            <asp:Label ID="Label6" runat="server" Text="Settings to exempt applicant(s) to assign in rotation"></asp:Label>
                        </h1>
                        <div class="sbcontent">
                            <div class="sxform auto">
                                <asp:Panel ID="Panel7" CssClass="sxpnl" runat="server">
                                    <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Exempt applicant(s) from rotation</span>
                                        </div>
                                        <div class='sxlm'>
                                            <%--       <asp:RadioButtonList ID="rdIsNodeExemptedInRotaion" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                <asp:ListItem Text="Default" Value=""></asp:ListItem>
                                            </asp:RadioButtonList>--%>

                                            <asp:RadioButtonList ID="rdNodeExemptedInRotaionEdit" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="HNET_Name" DataValueField="HNET_ID" AutoPostBack="false">
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
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
                    <infs:WclGrid runat="server" ID="grdNode" AllowPaging="False" AutoGenerateColumns="False"
                        AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                        GridLines="Both" EnableDefaultFeatures="False" ShowAllExportButtons="False" ShowExtraButtons="False"
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
                    <infs:WclGrid runat="server" ID="grdPackage" AllowPaging="False" AutoGenerateColumns="False"
                        AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                        EnableDefaultFeatures="False" ShowAllExportButtons="false" ShowExtraButtons="false"
                        OnNeedDataSource="grdPackage_NeedDataSource" OnDeleteCommand="grdPackage_DeleteCommand"
                        OnItemDataBound="grdPackage_ItemDataBound" OnInit="grdPackage_Init" OnRowDrop="grdPackage_RowDrop">
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="true" AllowAutoScrollOnDragDrop="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                            <%--    <ClientEvents OnRowDropping="onRowDropping"></ClientEvents>--%>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="BPHM_ID,BPHM_InstitutionHierarchyNodeID">
                            <CommandItemSettings ShowAddNewRecordButton="false"
                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                ShowRefreshButton="true" />
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="BackgroundPackage.BPA_Name" FilterControlAltText="Filter PackageName column"
                                    HeaderText="Package Name" SortExpression="BackgroundPackage.BPA_Name" UniqueName="PackageName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BackgroundPackage.BPA_Description" FilterControlAltText="Filter Description column"
                                    HeaderText="Description" SortExpression="BackgroundPackage.BPA_Description" UniqueName="Description">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BPHM_Sequence" FilterControlAltText="Filter DisplayOrder column"
                                    HeaderText="Display Order" SortExpression="BPHM_Sequence" UniqueName="DisplayOrder">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="IsActive" FilterControlAltText="Filter IsActive column"
                                    HeaderText="Is Active" SortExpression="BackgroundPackage.BPA_IsActive" UniqueName="IsActive">
                                    <ItemTemplate>
                                        <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BackgroundPackage.BPA_IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnCompliancePackageID" runat="server" Value='<%#Eval("BackgroundPackage.BPA_ID")%>' />
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

                <asp:Label ID="Label1" runat="server" Text="External Vendor Accounts"></asp:Label>
            </h1>
            <div class="content">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdExtVendorAcct" AllowPaging="False" AutoGenerateColumns="False"
                        AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                        EnableDefaultFeatures="False" ShowAllExportButtons="false" ShowExtraButtons="false"
                        OnNeedDataSource="grdExtVendorAcct_NeedDataSource" OnDeleteCommand="grdExtVendorAcct_DeleteCommand">
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="true" AllowAutoScrollOnDragDrop="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                            <%--    <ClientEvents OnRowDropping="onRowDropping"></ClientEvents>--%>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="DPMEVAM_ID,EVA_VendorID">
                            <CommandItemSettings ShowAddNewRecordButton="false"
                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                ShowRefreshButton="true" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="EVE_Name"
                                    HeaderText="External Vendor Name" SortExpression="EVE_Name" UniqueName="EVE_Name">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EVA_AccountNumber"
                                    HeaderText="External Vendor Account Number" SortExpression="EVA_AccountNumber" UniqueName="EVA_AccountNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EVA_AccountName"
                                    HeaderText="External Vendor Account Name" SortExpression="EVA_AccountName" UniqueName="EVA_AccountName">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Record?"
                                    Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle CssClass="tplcohdr" />
                                    <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                </telerik:GridButtonColumn>
                            </Columns>
                        </MasterTableView>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>

        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblRegulatoryEntity" runat="server" Text="Regulatory Entity"></asp:Label>
            </h1>
            <div class="content">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdRegulatoryEntity" AllowPaging="False" AutoGenerateColumns="False"
                        AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                        EnableDefaultFeatures="False" ShowAllExportButtons="false" ShowExtraButtons="false"
                        OnNeedDataSource="grdRegulatoryEntity_NeedDataSource" OnDeleteCommand="grdRegulatoryEntity_DeleteCommand">
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="true" AllowAutoScrollOnDragDrop="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                            <%--    <ClientEvents OnRowDropping="onRowDropping"></ClientEvents>--%>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="IHRE_ID">
                            <CommandItemSettings ShowAddNewRecordButton="false"
                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                ShowRefreshButton="true" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="RET_Name"
                                    HeaderText="Regulatory Entity" SortExpression="RET_Name" UniqueName="RET_Name">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Record?"
                                    Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle CssClass="tplcohdr" />
                                    <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                </telerik:GridButtonColumn>
                            </Columns>
                        </MasterTableView>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>

        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section" id="dvUserPermission" runat="server">
            <h1 class="mhdr">
                <asp:Label ID="Label3" runat="server" Text="Permission"></asp:Label>
            </h1>
            <div class="content">
                <infsu:Permissions runat="server" ID="Permissions" />
            </div>
        </div>

        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section" id="dvPermissionAdminEntryPortal" runat="server">
            <h1 class="mhdr">
                <asp:Label ID="lblPermissionAdminEntryPortal" runat="server" Text="Admin Entry Portal - Vendor Account Settings"></asp:Label>
            </h1>
            <div class="content">
                <infsu:AdminEntryAcctSettingPermissions runat="server" ID="AdminEntryAcctSettingPermissions" />
            </div>

        </div>

        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section" id="dvInviteContent" runat="server">
            <h1 class="mhdr">
                <asp:Label ID="Label7" runat="server" Text="Admin Entry Portal - Applicant Invite Content"></asp:Label>
            </h1>
            <div class="content">
                <infsu:AdminEntryNodeSpecificTemplates runat="server" ID="ucAdminEntryNodeSpecificTemplates" />
            </div>
        </div>


        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section" id="dvContentEditor" runat="server">
            <h1 class="mhdr">
                <asp:Label ID="lblContentEditor" runat="server" Text="Admin Entry Portal - Landing Page Content"></asp:Label>
            </h1>
            <div class="content">
                <infsu:ContentEditor runat="server" ID="ucContentEditor" />
            </div>
        </div>

        <hr style="border-bottom: solid 1px #c0c0c0;" />
        <div class="section" id="divReviewCriteriaMapping" runat="server">
            <h1 class="mhdr">
                <asp:Label ID="lblReviewCriteria" runat="server" Text=" Map Review Criteria Master List"></asp:Label>
            </h1>
            <div class="content">
                <infsu:ReviewCriteria runat="server" ID="ucReviewCriteria" />
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

    <div class="section" id="dvLocationTenantMapping" runat="server">
        <h1 class="mhdr">
            <asp:Label ID="lblLocation" runat="server" Text="Map Location"></asp:Label>
        </h1>
        <div class="content">
            <uc:LocTenantMapping ID="ucLocTenantMapping" runat="server" />
        </div>
    </div>
</asp:Content>

